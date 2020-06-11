(function () {
    var app = angular.module("GSWS");

    app.controller("MainController", ["$scope", "$state", "$log", "AgriCulture_Services", Login_CTRL]);

    function Login_CTRL(scope, state, log, agri_services) {
        scope.pagename = "Main";

        var req = {
            id: 1
        };
        agri_services.DemoAPI(req, function (value) {
            console.log(value.data);
        });
    }
})();

