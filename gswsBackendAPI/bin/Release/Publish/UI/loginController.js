(function () {
    var app = angular.module("GSWS");

    app.controller("LoginController", ["$scope" , "$state", "$log", Login_CTRL]);

    function Login_CTRL(scope,state, log) {
        scope.pagename = "Login";
        alert("Hello World");
    }
})();

