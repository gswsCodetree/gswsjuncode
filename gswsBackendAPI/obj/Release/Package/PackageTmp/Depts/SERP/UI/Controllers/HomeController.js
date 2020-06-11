(function () {
	var app = angular.module("GSWS");

	app.controller("SERPHomeController", ["$scope", "$state", "$log", "RTGS_Services", SERPHomeController]);

	function SERPHomeController(scope, state, log, serp_services) {
		scope.pagename = "Home page";
		var req = "1";
		serp_services.DemoAPI("GetApplicantStatus", req, function (value) {

		});
	}
})();