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

            return (UserInfo)DBCommands.DataReader("p_UserInfo_GetByUser", DBCommands.ObjectTypes.UserInfo);
        }

        public override bool UserInfoUpdate(UserInfo user)
        {
            DBCommands.PopulateParams("@UserName", user.UserName);
            DBCommands.PopulateParams("@ProfileImage", user.ProfileImage);
            DBCommands.PopulateParams("@GroupUsers", MapGroupListToTable(user.GroupUsers));

            return DBCommands.ExecuteNonQuery("p_UserInfo_Update");
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

