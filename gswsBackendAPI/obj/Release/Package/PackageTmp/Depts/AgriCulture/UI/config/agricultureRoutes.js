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
			.state("ui.AgriCulture", { url: "/AgriCulture/Home", templateUrl: pre_path + "Depts/AgriCulture/UI/Home.html", controller: "HomeController" })
			.state("ui.AgriCultureMainPage", { url: "/AgriCultureMainPage", templateUrl: pre_path + "Depts/AgriCulture/UI/Main.html", controller: "MainController" })
			.state("ui.SeedEligibility", { url: "/SeedEligibility", templateUrl: pre_path + "Depts/AgriCulture/UI/SeedEligibilityCheck.html", controller: "Agri_SeedEligibility_Controller" })
			.state("ui.EligibleBeneficiarys", { url: "/EligibleBenificiarys", templateUrl: pre_path + "Depts/AgriCulture/UI/GetBenificiaryDetails.html", controller: "Agri_EligibleBenf_Controller" })
			.state("ui.VAADetails", { url: "/VAADetails", templateUrl: pre_path + "Depts/AgriCulture/UI/GetVAADetails.html", controller: "Agri_VAADetails_Controller" })
			.state("ui.VillageProfile", { url: "/VillageProfile", templateUrl: pre_path + "Depts/AgriCulture/UI/VillageProfile.html", controller: "VillageProfile" })
			.state("ui.FMStatus", { url: "/FMStatus", templateUrl: pre_path + "Depts/AgriCulture/UI/FMStatus.html", controller: "FMStatus" })
			.state("ui.FMRegistration", { url: "/FMRegistration", templateUrl: pre_path + "Depts/AgriCulture/UI/FMRegistration.html", controller: "FMRegistration" })

	}

})();
