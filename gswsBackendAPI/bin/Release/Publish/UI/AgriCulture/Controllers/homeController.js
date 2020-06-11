(function () {
    var app = angular.module("GSWS");

    app.controller("HomeController", ["$scope", "$state", "$log", Login_CTRL]);

    function Login_CTRL(scope, state, log) {
        scope.pagename = "Home";
    }
})();

