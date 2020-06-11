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
			.state("ui.APDASCAC_RegForm", { url: "/APDASCAC_RegForm", templateUrl: pre_path + "Depts/Women_Children_Disabled_and_SeniorCitizen/UI/RegistrationForm.html", controller: "APDASCACREGFORM_Controller" })
			.state("ui.San_2wheeler", { url: "/San_2wheeler", templateUrl: pre_path + "Depts/Women_Children_Disabled_and_SeniorCitizen/UI/Motorized_3_Wheelers.html", controller: "MotorizedWheelers_Controller" })
			.state("ui.WCDWAppStatus", { url: "/WCDWAppStatus", templateUrl: pre_path + "Depts/Women_Children_Disabled_and_SeniorCitizen/UI/WCDWAppStatus.html", controller: "WCDWAppStatus" })

;

	}

})();
