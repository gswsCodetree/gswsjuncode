(function () {
	var app = angular.module("GSWS");

	app.controller("PTLoginController", ["$scope", "$state", "$log", "CT_Services", Login_CTRL]);

	function Login_CTRL(scope, state, log, CT_Services) {
		scope.preloader = false;
	}
})();

