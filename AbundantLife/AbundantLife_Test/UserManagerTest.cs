using AbundantLife_Core;
using AbundantLife_Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbundantLife_Test
{
    [TestFixture]
    public class UserManagerTest
    {

        [TestCase("testUser", "Test@test.com", true)]
        [TestCase(null, "Test@test.com", false)]
        [TestCase("testUser", null, false)]
        public void RecoverPassword_ReturnString(string userName, string email, bool expected)
        {
            RecoverModel model = new RecoverModel();
            model.UserName = userName;
            model.Email = email;
            string errorMessage;
            bool isEmptyString;

            UserManager.RecoverPassword(model, "Admin", out errorMessage, true);
            isEmptyString = string.IsNullOrEmpty(errorMessage);

            Assert.IsTrue(isEmptyString == expected);
        }

        [TestCase("current", "newpassword", "newpassword", "testuser", true)]
        [TestCase("current", "new", "newpassword", "testuser", false)]
        [TestCase("current", null, "newpassword", "testuser", false)]
        [TestCase("current", "newpassword", null, "testuser", false)]
        [TestCase("current", "newpassword", "newpassword", null, false)]
        [TestCase("newpassword", "newpassword", "newpassword", "testuser", false)]
        public void UpdatePassword_ReturnString(string cPassword, string nPassword, string rPassword, string userName, bool expected)
        {
            UpdateAccount model = new UpdateAccount();
            model.CurrentPassword = cPassword;
            model.NewPassword = nPassword;
            model.RepeatPassword = rPassword;
            model.UserName = userName;
            string errorMessage;
            bool isEmpty;

            UserManager.UpdatePassword(model, out errorMessage, true);
            isEmpty = string.IsNullOrEmpty(errorMessage);

            Assert.IsTrue(isEmpty == expected);
        }

        [Test]
        public void GetAllUsers_ReturnList()
        {
            List<UserInfo> allUsers = new List<UserInfo>();

            allUsers = UserManager.GetAllUsers(true);

            Assert.IsTrue(allUsers.Count == 1);
        }

        [TestCase("TestUser", true)]
        [TestCase(null, false)]
        public void GetUserByName_ReturnUserInfo(string name, bool expected)
        {
            string userName = name;
            bool emptyObject;

            UserInfo user = UserManager.GetUserByName(name, true);
            emptyObject = (user != null);

            Assert.IsTrue(expected == emptyObject);
        }

        [Test]
        public void GetAllUsers_CheckGroupList_ReturnList()
        {
            List<UserInfo> allUsers = new List<UserInfo>();

            allUsers = UserManager.GetAllUsers(true);

            Assert.IsTrue(allUsers.Count > 0 && allUsers[0].GroupUsers.Count > 0);
        }

        [Test]
        public void GroupsGetAll_ReturnList()
        {
            List<UserGroups> groupList;

            groupList = UserManager.GroupsGetAll(true);

            Assert.IsTrue(groupList.Count > 0);
        }

        [TestCase("Test@test.com", "Name", 1, "TestUser", true, true)]
        [TestCase("Test@test.com", "Name", 1, null, true, false)]
        [TestCase("Test@test.com", "Name", 1, "TestUser", true, true)]
        [TestCase("Test@test.com", "Name", 1, "TestUser", false, false)]
        public void InsertUser_ReturnString(string email, string name, int id, string userName, bool includeGroups, bool expected)
        {
            UserInfo user = new UserInfo();
            user.Email = email;
            user.UserInfoID = id;
            user.UserName = userName;

            if (includeGroups)
            {
                List<UserGroups> groupList = new List<UserGroups>();
                UserGroups groups = new UserGroups();
                groups.Active = true;
                groups.GroupLevel = 1;
                groups.GroupName = "UserGroup";
                groups.UserGroupID = 1;
                groupList.Add(groups);

                user.GroupUsers = groupList;
            }

            string errorMessage;
            bool isEmptyString;

            //act
            UserManager.InsertUser(user, out errorMessage, true);
            isEmptyString = string.IsNullOrEmpty(errorMessage);

            //assert
            Assert.IsTrue(isEmptyString == expected);
        }

        [TestCase("Test@test.com", "Name", 1, "TestUser", true, true)]
        [TestCase("Test@test.com", "Name", 1, null, true, false)]
        [TestCase("Test@test.com", "Name", 1, "TestUser", true, true)]
        [TestCase("Test@test.com", "Name", 1, "TestUser", false, false)]
        public void UpdateUser_ReturnString(string email, string name, int id, string userName, bool includeGroups, bool expected)
        {
            //arrange
            UserInfo user = new UserInfo();
            user.Email = email;
            user.UserInfoID = id;
            user.UserName = userName;

            if (includeGroups)
            {
                List<UserGroups> groupList = new List<UserGroups>();
                UserGroups groups = new UserGroups();
                groups.Active = true;
                groups.GroupLevel = 1;
                groups.GroupName = "UserGroup";
                groups.UserGroupID = 1;
                groupList.Add(groups);

                user.GroupUsers = groupList;
            }

            string errorMessage;
            bool isEmptyString;

            //act
            UserManager.UpdateUser(user, out errorMessage, true);
            isEmptyString = string.IsNullOrEmpty(errorMessage);

            //assert
            Assert.IsTrue(isEmptyString == expected);
        }

        [TestCase("TestUser", true)]
        [TestCase(null, false)]
        public void DeleteUser_ReturnBool(string userName, bool expected)
        {
            bool result;

            result = UserManager.DeleteUser(userName, true);

            Assert.IsTrue(result == expected);
        }

        [TestCase("TestCode", true)]
        [TestCase(null, false)]
        public void GetUserByCode_ReturnUserInfo(string name, bool expected)
        {
            string userName = name;
            bool emptyObject;

            UserInfo user = UserManager.GetUserByName(name, true);
            emptyObject = (user != null);

            Assert.IsTrue(expected == emptyObject);
        }
    }
}
