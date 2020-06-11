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
            .state("ui.SERP", { url: "/SERP", templateUrl: pre_path + "Depts/SERP/UI/Home.html", controller: "SERPHomeController" })
            .state("ui.PensionStatus", { url: "/PensionStatus", templateUrl: pre_path + "Depts/SERP/UI/PensionStatus.html", controller: "PensionStatusController" })
            .state("ui.SERPBIMA", { url: "/SERPBIMA", templateUrl: pre_path + "Depts/SERP/UI/SerpBima.html", controller: "SerpBimaController" })
            .state("ui.SERPBIMAREG", { url: "/SERPBIMAREG", templateUrl: pre_path + "Depts/SERP/UI/SerpBimaRegsitration.html", controller: "SerpBimaRegsitrationController" })
            .state("ui.BLSVerification", { url: "/BLSVerification", templateUrl: pre_path + "Depts/SERP/UI/BLSVerification.html", controller: "BLSVerificationController" })
            .state("ui.SRSVerification", { url: "/SRSVerification", templateUrl: pre_path + "Depts/SERP/UI/SRSVerification.html", controller: "SRSVerificationController" })
            .state("ui.YSRAsaraSVerification", { url: "/YSRAsaraSVerification", templateUrl: pre_path + "Depts/SERP/UI/YSRAsaraSVer.html", controller: "BLSVerificationController" })
			.state("ui.VLRSVerification", { url: "/VLRSVerification", templateUrl: pre_path + "Depts/SERP/UI/VLRSVerification.html", controller: "BLSVerificationController" })
			.state("ui.PensionApplication", { url: "/PensionApplication", templateUrl: pre_path + "Depts/SERP/UI/PensionApplication.html", controller: "PensionApplication" })
			.state("ui.BuildingPlanApplicationStatus", { url: "/BuildingPlanApplicationStatus", templateUrl: pre_path + "Depts/PRRD/UI/BuildingPlanApplicationStatus.html", controller: "BuildingPlanApplicationStatus" })
			.state("ui.PensionAppStatus", { url: "/PensionAppStatus", templateUrl: pre_path + "Depts/SERP/UI/PensionAppStatus.html", controller: "PensionAppStatus" })

			.state("ui.LayoutPlanApplicationStatus", { url: "/LayoutPlanApplicationStatus", templateUrl: pre_path + "Depts/PRRD/UI/LayoutPlanApplicationStatus.html", controller: "LayoutPlanApplicationStatus" })
			.state("ui.LayoutPlanInformation", { url: "/LayoutPlanInformation", templateUrl: pre_path + "Depts/PRRD/UI/LayoutPlanInformation.html", controller: "LayoutPlanInformation" })
			.state("ui.TransportDrinkingWater", { url: "/TransportDrinkingWater", templateUrl: pre_path + "Depts/PRRD/UI/TransportDrinkingWater.html", controller: "TransportDrinkingWater" })
	}

})();