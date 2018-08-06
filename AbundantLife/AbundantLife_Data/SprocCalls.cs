using AbundantLife_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbundantLife_Data
{
    public class SprocCalls : ISprocCalls
    {
        #region userinfo 
        public override DataTable UserInfoGetAll()
        {
            return DBCommands.AdapterFill("p_UserInfo_GetAll");
        }

        public override UserInfo UserInfoGetByUser(string userName)
        {
            DBCommands.PopulateParams("@UserName", userName);

            DataTable userData = DBCommands.AdapterFill("p_UserInfo_GetByUser");
            UserInfo user = new UserInfo();

            foreach (DataRow row in userData.Rows)
            {
                user.UserName = row["UserName"].ToString();
                user.Email = row["Email"].ToString();
                user.ProfileImage = (row["ProfileImage"] != DBNull.Value) ? row["ProfileImage"].ToString() : null;
            }

            return user;
        }

        public override bool UserInfoUpdate(UserInfo user)
        {
            DBCommands.PopulateParams("@UserName", user.UserName);
            DBCommands.PopulateParams("@ProfileImage", user.ProfileImage);
            DBCommands.PopulateParams("@RecoverCode", user.RecoverCode);
            DBCommands.PopulateParams("@GroupUsers", MapGroupListToTable(user.GroupUsers));

            return DBCommands.ExecuteNonQuery("p_UserInfo_Update");
        }

        public override UserInfo UserInfoGetByCode(string code)
        {
            DBCommands.PopulateParams("@code", code);
            DataTable userTable = DBCommands.AdapterFill("p_UserInfo_GetByCode");
            UserInfo user = null;

            foreach (DataRow row in userTable.Rows)
            {
                user = new UserInfo();
                user.UserInfoID = Convert.ToInt32(row["UserInfoID"]);
                user.UserName = row["UserName"].ToString();
                user.Email = (row["Email"] != DBNull.Value) ? row["Email"].ToString() : null;
                user.ProfileImage = (row["ProfileImage"] != DBNull.Value) ? row["ProfileImage"].ToString() : null;
                user.RecoverCode = code;
            }


            return user;
        }
        #endregion

        #region group users
        public override DataTable GroupUsersGetByUserName(string userName)
        {
            DBCommands.PopulateParams("@UserName", userName);

            return DBCommands.AdapterFill("p_GroupUsers_GetByUserName");
        }

        public override DataTable UserGroupsGetActive()
        {
            return DBCommands.AdapterFill("p_UserGroup_GetActive");
        }
        #endregion
    }

    public class FakeSprocCalls : ISprocCalls
    {
        #region userinfo 
        public override DataTable UserInfoGetAll()
        {
            DataTable users = new DataTable();
            users.Columns.Add("UserInfoID");
            users.Columns.Add("UserName");
            users.Columns.Add("Email");
            users.Columns.Add("ProfileImage");

            DataRow row = users.NewRow();
            row["UserInfoID"] = 1;
            row["UserName"] = "TestUser";
            row["Email"] = "test@Test.com";
            row["ProfileImage"] = "Image";
            users.Rows.Add(row);

            return users;
        }

        public override UserInfo UserInfoGetByUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return null;
            else
            {
                UserInfo user = new UserInfo();
                user.UserName = userName;

                return user;
            }
        }

        public override bool UserInfoUpdate(UserInfo user)
        {
            if (string.IsNullOrEmpty(user.UserName))
                return false;
            else
            {
                List<UserGroups> groups = new List<UserGroups>();
                UserGroups g = new UserGroups() { UserGroupID = 1, GroupName = "Admin" };
                groups.Add(g);

                DataTable groupsTable = MapGroupListToTable(groups);

                return (groupsTable.Rows.Count == 1);
            }
        }

        public override UserInfo UserInfoGetByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;
            else
            {
                UserInfo user = new UserInfo();
                user.RecoverCode = code;

                return user;
            }
        }
        #endregion

        #region group users
        public override DataTable GroupUsersGetByUserName(string userName)
        {
            DataTable groups = new DataTable();
            groups.Columns.Add("GroupUsersID");
            groups.Columns.Add("UserName");
            groups.Columns.Add("UserGroupID");
            groups.Columns.Add("GroupName");
            groups.Columns.Add("GroupLevel");
            groups.Columns.Add("Active");

            if (string.IsNullOrEmpty(userName) == false)
            {
                DataRow row = groups.NewRow();
                row["GroupUsersID"] = 1;
                row["UserName"] = "TestUser";
                row["UserGroupID"] = 1;
                row["GroupName"] = "TestGroup";
                row["GroupLevel"] = 1;
                row["Active"] = true;
                groups.Rows.Add(row);
            }

            return groups;
        }

        public override DataTable UserGroupsGetActive()
        {
            DataTable groups = new DataTable();
            groups.Columns.Add("UserGroupID");
            groups.Columns.Add("GroupName");
            groups.Columns.Add("GroupLevel");
            groups.Columns.Add("Active");
            DataRow row = groups.NewRow();

            row["UserGroupID"] = 1;
            row["GroupName"] = "TestGroup";
            row["GroupLevel"] = 1;
            row["Active"] = true;
            groups.Rows.Add(row);

            return groups;
        }
        #endregion
    }

    public abstract class ISprocCalls
    {
        #region userinfo    
        public abstract DataTable UserInfoGetAll();
        public abstract UserInfo UserInfoGetByUser(string userName);
        public abstract bool UserInfoUpdate(UserInfo user);
        public abstract UserInfo UserInfoGetByCode(string code);
        #endregion

        #region group users
        public abstract DataTable GroupUsersGetByUserName(string userName);
        public abstract DataTable UserGroupsGetActive();
        #endregion

        public DataTable MapGroupListToTable(List<UserGroups> groups)
        {
            DataTable groupTable = new DataTable();
            groupTable.Columns.Add("UserGroupID");
            groupTable.Columns.Add("Active");

            if (groups.Count > 0)
            {
                DataRow row;

                foreach (UserGroups g in groups)
                {
                    row = groupTable.NewRow();
                    row["UserGroupID"] = g.UserGroupID;
                    row["Active"] = g.Active;

                    groupTable.Rows.Add(row);
                }
            }

            return groupTable;
        }
    }
}

