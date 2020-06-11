(function () {
	var app = angular.module("GSWS");

	app.controller("RTGSHomeController", ["$scope", "$state", "$log", "RTGS_Services", RTGS_Home_CTRL]);

	function RTGS_Home_CTRL(scope, state, log, endow_services) {
		scope.pagename = "Home page";
		var req = "1";
		endow_services.DemoAPI("GetApplicantStatus", req, function (value) {
			alert("Hai");
		});
	}
})();