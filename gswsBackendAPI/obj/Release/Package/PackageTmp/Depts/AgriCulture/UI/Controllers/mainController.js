(function () {
    var app = angular.module("GSWS");

    app.controller("MainController1", ["$scope", "$state", "$log", "AgriCulture_Services", Login_CTRL]);

    function Login_CTRL(scope, state, log, agri_services) {
        scope.pagename = "Main";

        var req = {
            id: 1
        };
		agri_services.DemoAPI(methodname,req, function (value) {

			var res = value.data;

            console.log(value.data);
        });
    }
})();

