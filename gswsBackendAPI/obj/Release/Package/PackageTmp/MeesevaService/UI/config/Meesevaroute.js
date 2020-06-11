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
			.state("ui.MIncome", { url: "/MIncome", templateUrl: pre_path + "MeesevaService/UI/GswsMeesevaPage.html", controller: "MeesevaController" })
			.state("uc.MeeSevaStatus", { url: "/MeeSevaApplicationStatus", templateUrl: pre_path + "MeesevaService/UI/MeesevaStatusCheck.html", controller: "MeesevaAppStatusCTRL" });
			
	}

})();