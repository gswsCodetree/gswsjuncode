(function () {
    var app = angular.module("GSWS");

	app.controller("PhysicalForms", ["$scope", "$state", "$log", "Login_Services", PhyController]);

	app.controller("AudioForms", ["$scope", "$state", "$log", "Login_Services", AudioFormsController]);

    function PhyController(scope, state, log, Login_Services) {
        scope.preloader = false;
	}
	function AudioFormsController(scope, state, log, Login_Services) {
		scope.preloader = false;
	}
})();