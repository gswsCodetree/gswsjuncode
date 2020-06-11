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
			.state("ui.InternalURL", { url: "/InternalURL", templateUrl: pre_path + "Internal/UI/Internal.html", controller: "InternalController" })
			.state("ui.SeccURL", { url: "/SECCMASTER", templateUrl: pre_path + "Internal/UI/SecretariatMaster.html", controller: "SecretriatController" })
			.state("ui.InternalupdateURL", { url: "/InternalUpdateURL", templateUrl: pre_path + "Internal/UI/URLUpdation.html", controller: "InternalUpdateController" })
			.state("ui.UpdateURL", { url: "/UpdateURL", templateUrl: pre_path + "Internal/UI/UpdateURL.html", controller: "InternalUpdateURLController" })
			.state("ui.Feedback", { url: "/ReportIssue", templateUrl: pre_path + "Internal/UI/Feedback.html", controller: "FeedBackController" })
			.state("ui.issueTrackingReport", { url: "/issueTrackingReport", templateUrl: pre_path + "Internal/UI/issueTrackingReport.html", controller: "issueTrackingController" })
			.state("ui.IssueClosing", { url: "/IssueClosing", templateUrl: pre_path + "Internal/UI/IssueClosing.html", controller: "IssueClosingController" })
			.state("ui.HardwareIssueClosing", { url: "/HardwareIssueClosing", templateUrl: pre_path + "Internal/UI/HardwareIssueClosing.html", controller: "HWIssueClosingCntrl" });
		

	}

})();