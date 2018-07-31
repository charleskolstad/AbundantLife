angular.module("Admin").controller("UserCtrl", function ($http, $scope, dataUrl) {
    $scope.data = {};
    $scope.User = {};
    $scope.delUser;

    $http.get(dataUrl + '/Admin/GetAllUsers').then(function (data) {
        $scope.data.Users = data.data;
    }), function (error) {
        $scope.data.error = error;
    };

    $http.get(dataUrl + '/Admin/GetAllGroups').then(function (data) {
        $scope.data.GroupUsers = data.data;
    }), function (error) {
        $scope.data.error = error;
    };

    $scope.GetAllUsers = function () {
        $http({
            method: "get",
            url: dataUrl + "/Admin/GetAllUsers"
        }).then(function (response) {
            $scope.data.Users = response.data;
        }, function () {
            alert("Error Occur");
        })
    };

    $scope.GetAllGroups = function () {
        $http({
            method: "get",
            url: dataUrl + "/Admin/GetAllGroups"
        }).then(function (response) {
            $scope.data.GroupUsers = response.data;
        }, function () {
            alert("Error Occur");
        })
    };

    $scope.GetUser = function (user) {
        $scope.User.UserName = user.UserName;
        $scope.User.Email = user.Email;
        $scope.User.UserInfoID = user.UserInfoID;
        $scope.User.GroupUsers = user.GroupUsers;

        angular.element('#editMod').modal("show");
    }

    $scope.NewUser = function () {
        $scope.GetAllGroups();
        $scope.User.UserName = '';
        $scope.User.Email = '';
        $scope.User.UserInfoID = null;
        $scope.User.GroupUsers = $scope.data.GroupUsers;

        angular.element('#editMod').modal("show");
    }

    $scope.SaveUser = function (user) {
        if (user.UserInfoID <= 0)
            user.Groups = $scope.data.UserGroups;

        $http({
            method: "post",
            url: dataUrl + "Admin/SaveUser",
            datatype: "json",
            data: JSON.stringify(user)
        }).then(function (response) {
            alert(response.data);
            angular.element('#editMod').modal("hide");
            $scope.GetAllUsers();
        })
    }

    $scope.DeleteUser = function (userName) {
        $scope.delUser = userName;
        document.getElementById('delMessage').innerHTML = "Are you sure you want to delete user " + userName;
        angular.element('#deleteMod').modal("show");
    }

    $scope.CommitDelete = function () {
        $http({
            method: "post",
            url: dataUrl + "Admin/DeleteUser",
            datatype: "json",
            data: JSON.stringify({ delUser: $scope.delUser })
        }).then(function (response) {
            alert(response.data);
            angular.element('#deleteMod').modal("hide");
            $scope.GetAllUsers();
        })
    }
});