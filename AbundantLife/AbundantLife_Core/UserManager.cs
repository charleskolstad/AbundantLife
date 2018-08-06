using AbundantLife_Data;
using AbundantLife_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Security;

namespace AbundantLife_Core
{
    public class UserManager
    {
        #region login methods
        public static void RecoverPassword(RecoverModel model, string location, out string errorMessage, bool isTest = false)
        {
            try
            {

                ISprocCalls sprocCalls = AppTools.InitSprocCalls(isTest);
                errorMessage = string.Empty;

                UserInfo user = sprocCalls.UserInfoGetByUser(model.UserName);
                if (model.Email == user.Email)
                {
                    string code = RandomCode(7);
                    while (sprocCalls.UserInfoGetByCode(code) != null)
                    {
                        code = RandomCode(7);
                    }

                    user.GroupUsers = GetGroupsByUserName(model.UserName, sprocCalls);
                    user.RecoverCode = RandomCode(7);

                    if (sprocCalls.UserInfoUpdate(user) == false)
                        errorMessage = "Error recovering password.";
                    else if (SendRecoverEmail(user, location, isTest) == false)
                        errorMessage = "Error sending email.";
                }
                else
                    errorMessage = (user != null) ? "Email and username is not valid." : "Error loading user";
            }
            catch (Exception ex)
            {
                DBCommands.RecordError(ex);
                errorMessage = ex.Message;
            }
        }

        private static string RandomCode(int length)
        {
            string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(validCharacters[rnd.Next(validCharacters.Length)]);
            }

            return res.ToString();
        }
        
        private static bool SendRecoverEmail(UserInfo user, string location, bool isTest)
        {
            try
            {
                StringBuilder builder = new StringBuilder();

                builder.Append("Hello, we have received your message that your password has been forgotten.  To reset your password please click the link below.<br />");
                builder.Append(@"http://localhost:61860/" + location + "/CompeRec/" + user.RecoverCode);

                IEmailTools emailTools = AppTools.InitEmailTools(isTest);
                if (emailTools.SendEmail(user.Email, builder.ToString()) == false)
                    throw new Exception("Error sending email.");

                return true;
            }
            catch (Exception ex)
            {
                DBCommands.RecordError(ex);
                return false;
            }
        }

        public static void UpdatePassword(UpdateAccount model, out string errorMessage, bool isTest = false)
        {
            try
            {
                IMembershipTools membershipTools = AppTools.InitMembershipTools(isTest);
                errorMessage = string.Empty;

                if (model.NewPassword.Length < 9)
                    errorMessage = "Password must be at least 9 characters and have a special character";
                else if (model.CurrentPassword == model.NewPassword)
                    errorMessage = "New password must be different from the current password.";
                else if (model.NewPassword != model.RepeatPassword)
                    errorMessage = "New password and repeated password does not match.";
                else if (membershipTools.UpdatePassword(model.UserName, model.NewPassword) == false)
                    errorMessage = "Error updating account.";
            }
            catch (Exception ex)
            {
                DBCommands.RecordError(ex);
                errorMessage = ex.Message;
            }
        }
        #endregion

        #region user info
        public static void InsertUser(UserInfo user, out string errorMessage, bool isTest = false)
        {
            try
            {
                errorMessage = ValidateUser(user);

                if (string.IsNullOrEmpty(errorMessage))
                {
                    IMembershipTools membershipTools = AppTools.InitMembershipTools(isTest);
                    ISprocCalls sprocCalls = AppTools.InitSprocCalls(isTest);

                    if (membershipTools.CreateUser(user.UserName, user.Email) == false ||
                        sprocCalls.UserInfoUpdate(user) == false)
                    {
                        errorMessage = "Error saving user information.";
                    }

                }
            }
            catch (Exception ex)
            {
                DBCommands.RecordError(ex);
                errorMessage = ex.Message;
            }
        }

        public static void UpdateUser(UserInfo user, out string errorMessage, bool isTest = false)
        {
            try
            {
                errorMessage = ValidateUser(user);

                if (string.IsNullOrEmpty(errorMessage))
                {
                    IMembershipTools membershipTools = AppTools.InitMembershipTools(isTest);
                    ISprocCalls sprocCalls = AppTools.InitSprocCalls(isTest);

                    if (membershipTools.UpdateUserEmail(user.UserName, user.Email) == false)
                        errorMessage = "Error updating email.";

                    if (sprocCalls.UserInfoUpdate(user) == false)
                        errorMessage = "Error saving user information";
                }
            }
            catch (Exception ex)
            {
                DBCommands.RecordError(ex);
                errorMessage = ex.Message;
            }
        }

        public static bool DeleteUser(string userName, bool isTest = false)
        {
            try
            {
                IMembershipTools membershipTools = AppTools.InitMembershipTools(isTest);
                return membershipTools.DeleteUser(userName);
            }
            catch (Exception ex)
            {
                DBCommands.RecordError(ex);
                return false;
            }
        }

        private static string ValidateUser(UserInfo user)
        {
            try
            {
                string emailRegEx = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
                if (Regex.IsMatch(user.Email, emailRegEx, RegexOptions.IgnoreCase) == false)
                    return "Please provide valid email.";

                if (user.GroupUsers.Count <= 0)
                    return "Error loading user groups.";

                return string.Empty;
            }
            catch (Exception ex)
            {
                DBCommands.RecordError(ex);
                return "Error validating user.";
            }
        }

        public static List<UserInfo> GetAllUsers(bool isTest = false)
        {
            List<UserInfo> allUsers = new List<UserInfo>();

            try
            {
                ISprocCalls sprocCalls = AppTools.InitSprocCalls(isTest);
                DataTable userTable = sprocCalls.UserInfoGetAll();
                UserInfo user;

                foreach (DataRow row in userTable.Rows)
                {
                    user = new UserInfo();
                    user.UserInfoID = Convert.ToInt32(row["UserInfoID"]);
                    user.UserName = row["UserName"].ToString();
                    user.Email = row["Email"].ToString();
                    user.ProfileImage = row["ProfileImage"].ToString();
                    user.GroupUsers = GetGroupsByUserName(user.UserName, sprocCalls);

                    allUsers.Add(user);
                }
            }
            catch (Exception ex)
            {
                DBCommands.RecordError(ex);
            }

            return allUsers;
        }

        public static UserInfo GetUserByName(string userName, bool isTest = false)
        {
            ISprocCalls sprocCalls = AppTools.InitSprocCalls(isTest);

            UserInfo user = sprocCalls.UserInfoGetByUser(userName);
            if (user != null)
                user.GroupUsers = GetGroupsByUserName(userName, sprocCalls);

            return user;
        }

        private static List<UserGroups> GetGroupsByUserName(string userName, ISprocCalls sprocCalls)
        {
            List<UserGroups> groupList = new List<UserGroups>();
            DataTable groupTable = sprocCalls.GroupUsersGetByUserName(userName);
            UserGroups group;

            foreach (DataRow row in groupTable.Rows)
            {
                group = new UserGroups();

                group.UserGroupID = Convert.ToInt32(row["UserGroupID"]);
                group.GroupName = row["GroupName"].ToString();
                group.GroupLevel = Convert.ToInt32(row["GroupLevel"]);
                group.Active = Convert.ToBoolean(row["Active"]);

                groupList.Add(group);
            }

            return groupList;
        }

        public static List<UserGroups> GroupsGetAll(bool isTest = false)
        {
            List<UserGroups> groupList = new List<UserGroups>();

            try
            {
                ISprocCalls sprocCalls = AppTools.InitSprocCalls(isTest);

                DataTable groupTable = sprocCalls.UserGroupsGetActive();
                UserGroups group;

                foreach (DataRow row in groupTable.Rows)
                {
                    group = new UserGroups();
                    group.Active = false;
                    group.GroupLevel = Convert.ToInt32(row["GroupLevel"]);
                    group.GroupName = row["GroupName"].ToString();
                    group.UserGroupID = Convert.ToInt32(row["UserGroupID"]);

                    groupList.Add(group);
                }
            }
            catch (Exception ex)
            {
                DBCommands.RecordError(ex);
            }

            return groupList;
        }

        public static UserInfo GetUserByCode(CompleteRec model, string code, bool isTest = false)
        {
            UserInfo user = null;

            try
            {
                ISprocCalls sprocCalls = AppTools.InitSprocCalls(isTest);
                IMembershipTools membershipTools = AppTools.InitMembershipTools(isTest);
                user = sprocCalls.UserInfoGetByCode(code);

                if (user.Email == model.Email)
                {
                    if (membershipTools.UpdatePassword(model.UserName, model.Password))
                    {
                        string errorMessage = string.Empty;
                        user.GroupUsers = GetGroupsByUserName(user.UserName, sprocCalls);
                        user.RecoverCode = null;
                        UpdateUser(user, out errorMessage, isTest);

                        if (string.IsNullOrEmpty(errorMessage) == false)
                            throw new Exception(errorMessage);
                    }
                }
                else
                    throw new Exception("User recovered email does not match.");

                return user;
            }
            catch (Exception ex)
            {
                DBCommands.RecordError(ex);
            }

            return user;
        }
        #endregion
    }
}
