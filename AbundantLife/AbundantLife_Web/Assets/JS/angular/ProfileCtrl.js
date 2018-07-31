angular.module("Admin").controller("ProfileCtrl", function ($http, $scope, dataUrl) {
    $scope.Profile = {};

    $http.get(dataUrl + '/Admin/GetUserByName').then(function (data) {
        $scope.Profile = data.data;
        $scope.Profile.ProfileImage = $scope.Profile.ProfileImage.replace('~', '../../..');
    }), function (error) {
        $scope.data.error = error;
    };

    $scope.GetProfile = function () {
        $http({
            method: "get",
            url: dataUrl + "/Admin/GetUserByName"
        }).then(function (response) {
            $scope.Profile = data.data;
            $scope.Profile.ProfileImage = $scope.Profile.ProfileImage.replace('~', '../../..');
        }, function () {
            alert("Error Occur");
        })
    }

    $scope.UpdateProfile = function () {
        $http({
            method: "post",
            url: dataUrl + "Admin/UpdateProfile",
            datatype: "json",
            data: JSON.stringify($scope.Profile)
        }).then(function (response) {
            if (response.data == '') 
                alert('Profile updated successfully!');
            else 
                alert(response.data);
            
            $scope.GetProfile();
        })
    }

    $scope.SaveProfileImage = function () {
        let vData = null;

        if (ctx.canvas == '') {
        }
        else {
            let temp_canvas = document.createElement("canvas");
            let temp_ctx = temp_canvas.getContext('2d');
            temp_canvas.width = 250;
            temp_canvas.height = 250;
            temp_ctx.drawImage(ctx.canvas, 20, 20, 200, 200, 0, 0, 250, 250);
            vData = temp_canvas.toDataURL();
            vData = vData.replace('data:image/png;base64,', '');
        }

        $http({
            method: "post",
            url: dataUrl + "Admin/SaveProfileImage",
            datatype: "json",
            data: JSON.stringify({ newImage: vData })
        }).then(function (response) {
            let message = response.data;
            if (message == '')
                message = "Image saved successfully!";

            alert(message);
            HideModal();
            $scope.GetProfile();
        });
    }

    $scope.ProfileImage = function (element) {
        document.getElementById("ProfileCancel").classList.remove('cancel-image');

        let reader = new FileReader();
        reader.onload = function (event) {
            image.setAttribute('crossOrigin', 'anonymous');
            image.src = event.target.result;
            setupCanvas("ProfileCanvas");
            image.onload = onImageLoad.bind(this);
        }
        reader.readAsDataURL(element.files[0]);

        document.getElementById("ProfileSlider").oninput = updateScale.bind(this);
    }

    $scope.cancelFile = function () {
        document.getElementById("ProfileFile").value = "";
        ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);
        document.getElementById("ProfileCancel").classList.add('cancel-image');
    }

    function HideModal() {
        angular.element('#profileImage').modal("hide");
    }
});