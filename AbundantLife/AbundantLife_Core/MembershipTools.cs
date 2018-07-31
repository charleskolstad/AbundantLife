using AbundantLife_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace AbundantLife_Core
{
    internal class MembershipTools : IMembershipTools
    {
        public bool CreateUser(string userName, string email)
        {
            try
            {
                Membership.CreateUser(userName, "p@ssword1", email);
                return true;
            }
            catch (Exception ex)
            {
                DBCommands.RecordError(ex);
                return false;
            }
        }

        public bool DeleteUser(string userName)
        {
            return Membership.DeleteUser(userName);
        }

        public string GetUserEmail(string userName)
        {
            MembershipUser user = Membership.GetUser(userName);

            return user.Email;
        }

        public bool SetTempPassword(string userName, string tempPassword)
        {
            try
            {
                MembershipUser user = Membership.GetUser(userName);

                if (user.ChangePassword(user.ResetPassword(), tempPassword) == false)
                    throw new Exception("Error changing password.");
                user.LastActivityDate = DateTime.Now.AddMonths(-1);
                Membership.UpdateUser(user);

                return true;
            }
            catch (Exception ex)
            {
                DBCommands.RecordError(ex);
                return false;
            }
        }

        public bool UpdatePassword(string userName, string password)
        {
            try
            {
                MembershipUser user = Membership.GetUser(userName);

                if (user.ChangePassword(user.ResetPassword(), password) == false)
                    throw new Exception("Error changing password.");

                return true;
            }
            catch (Exception ex)
            {
                DBCommands.RecordError(ex);
                return false;
            }
        }

        public bool UpdateUserEmail(string userName, string email)
        {
            try
            {
                string currentEmail = GetUserEmail(userName);
                if (email != currentEmail)
                {
                    MembershipUser user = Membership.GetUser(userName);
                    user.Email = email;
                    Membership.UpdateUser(user);
                }

                return true;
            }
            catch (Exception ex)
            {
                DBCommands.RecordError(ex);
                return false;
            }
        }
    }

    internal class FakeMembershipTools : IMembershipTools
    {
        public bool CreateUser(string userName, string email)
        {
            if (string.IsNullOrEmpty(userName))
                return false;

            return true;
        }

        public bool DeleteUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return false;

            return true;
        }

        public string GetUserEmail(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return null;

            return "Test@test.com";
        }

        public bool SetTempPassword(string userName, string tempPassword)
        {
            if (string.IsNullOrEmpty(userName))
                return false;

            return true;
        }

        public bool UpdatePassword(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
                return false;

            return true;
        }

        public bool UpdateUserEmail(string userName, string email)
        {
            if (string.IsNullOrEmpty(userName))
                return false;

            return true;
        }
    }

    internal interface IMembershipTools
    {
        string GetUserEmail(string userName);
        bool SetTempPassword(string userName, string tempPassword);
        bool UpdatePassword(string userName, string password);
        bool UpdateUserEmail(string userName, string email);
        bool CreateUser(string userName, string email);
        bool DeleteUser(string userName);
    }
}
