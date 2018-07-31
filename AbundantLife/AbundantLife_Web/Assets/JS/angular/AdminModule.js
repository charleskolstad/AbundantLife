angular.module("Admin", ["ngRoute"]).config(function ($routeProvider) {
    $routeProvider.when("/allUsers", {
        templateUrl: "/Admin/AllUsers"
    }).when("/myProfile", {
        templateUrl: "/Admin/UserProfile"
    }).otherwise({

    });
}).constant("dataUrl", "http://adminapps.chuckcreates.com/");