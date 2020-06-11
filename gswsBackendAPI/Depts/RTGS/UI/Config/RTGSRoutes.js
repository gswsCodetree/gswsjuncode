(function () {

	var app = angular.module("GSWS");
	app.config(["$stateProvider", "$urlRouterProvider", '$logProvider', '$locationProvider', App_config]);

	function App_config($stateProvider, $urlRouterProvider, $logProvider, $locationProvider) {
		var web_site = location.hostname;
		var pre_path = "";
		if (web_site !== "localhost") {
			pre_path = "/" + location.pathname.split("/")[1] + "/";
		}

		$stateProvider
			.state("ui.RTGS", { url: "/RTGS", templateUrl: pre_path + "Depts/RTGS/UI/Home.html", controller: "RTGSHomeController" })
			.state("ui.PSSStatus", { url: "/PSSStatus", templateUrl: pre_path + "Depts/RTGS/UI/PSSStatus.html", controller: "PSSStatusController" })
			.state("ui.UnsurveyRequest", { url: "/UnsurveyRequest", templateUrl: pre_path + "Depts/RTGS/UI/PSSUnSurveyRequest.html", controller: "PSSUNSurveyController" })
	}

})();