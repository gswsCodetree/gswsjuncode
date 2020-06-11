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
			.state("ui.RandB_ComplaintCheck", { url: "/RandB_ComplaintCheck", templateUrl: pre_path + "Depts/Transport_RandB/UI/RandB_ComplaintStatusCheck.html", controller: "RandBComplaintCheck_Controller" })
			.state("ui.RTARegAppStatus", { url: "/RTARegAppStatus", templateUrl: pre_path + "Depts/Transport_RandB/UI/RTARegAppStatus.html", controller: "RTARegAppStatus" })
			.state("ui.LLRAppStatus", { url: "/LLRAppStatus", templateUrl: pre_path + "Depts/Transport_RandB/UI/LLRAppStatus.html", controller: "LLRAppStatus" })
			.state("ui.VahanaMitraRegStatus", { url: "/VahanaMitraRegStatus", templateUrl: pre_path + "Depts/Transport_RandB/UI/VahanaMitraRegStatus.html", controller: "VahanaMitraRegStatus" })
			;
	}

})();