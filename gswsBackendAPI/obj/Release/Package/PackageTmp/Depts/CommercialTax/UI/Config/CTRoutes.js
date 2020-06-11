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
			//.state("PTLogin", { url: "/PTLogin", templateUrl: pre_path + "Depts/CommercialTax/UI/PTLogin.html", controller: "PTLoginController" })
			//.state("PTEmailReg", { url: "/PTEmailReg", templateUrl: pre_path + "Depts/CommercialTax/UI/PTEmailReg.html", controller: "PTEmailRegController" })
            .state("ui.PTReg", { url: "/PTReg", templateUrl: pre_path + "Depts/CommercialTax/UI/PTNewRegistartion.html", controller: "PTRegistrationControllers" })
            .state("ui.PTStatus", { url: "/PTStatus", templateUrl: pre_path + "Depts/CommercialTax/UI/PTStatusCheck.html", controller: "PTStatusCheckController" })
            .state("ui.PTRegEdit", { url: "/PTRegEdit", templateUrl: pre_path + "Depts/CommercialTax/UI/PTRegEdit.html", controller: "PTRegEditControllers" })
            .state("ui.PTReturns", { url: "/PTReturns", templateUrl: pre_path + "Depts/CommercialTax/UI/PTReturns.html", controller: "PTReturnsController" })
            .state("ui.PTRC", { url: "/PTRC", templateUrl: pre_path + "Depts/CommercialTax/UI/PTRC.html", controller: "PTRCController" })
	}

})();