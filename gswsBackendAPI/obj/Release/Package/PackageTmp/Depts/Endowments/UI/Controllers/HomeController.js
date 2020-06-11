(function () {
	var app = angular.module("GSWS");

	app.controller("HomeController", ["$scope", "$state", "$log", "Endowment_Services", Home_CTRL]);

	function Home_CTRL(scope, state, log, endow_services) {
		scope.pagename = "Home page";
		var req = "1";
		endow_services.DemoAPI("GetApplicantStatus", req, function (value) {
			alert("Hai");
		});
	}
})();