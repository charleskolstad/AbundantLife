angular.module("Admin", ["ngRoute"]).config(function ($routeProvider) {
    $routeProvider.when("/allUsers", {
        templateUrl: "allusers.html"
    }).when("/userProfile", {
        templateUrl: "profile.html"
    }).when("/allEvents", {
        templateUrl: "Events.html"
    }).otherwise({

    });
}).constant("dataUrl", "http://localhost:52055/");