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
			.state("ui.Endowments", { url: "/Endowments", templateUrl: pre_path + "Depts/Endowments/UI/Home.html", controller: "HomeController" })
			.state("ui.ABWCStatus", { url: "/ABWCStatus", templateUrl: pre_path + "Depts/Endowments/UI/ABWCStatus.html", controller: "ABWCStatusController" })
			}

})();