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
			//.state("CertificatesDownload", { url: "/CertificatesDownload", templateUrl: pre_path + "Depts/SocialWelfare_Tribal/UI/PelliKaanuka_Certificates_Download.html", controller: "CertificateDownlaod" })
			.state("ui.MrgCertDownload", { url: "/MrgCertDownload", templateUrl: pre_path + "Depts/SocialWelfare_Tribal/UI/PellikaanukaMrgCertDownload.html", controller: "Mrg_CertificateDownlaod" })
			.state("ui.PelliKaanukaStatusCheck", { url: "/PelliKaanukaStatusCheck", templateUrl: pre_path + "Depts/SocialWelfare_Tribal/UI/PelliKaanuka_Statuscheck.html", controller: "PK_StatusCheck" })
			.state("ui.Education_Overseas_Check", { url: "/Education_Overseas_Check", templateUrl: pre_path + "Depts/SocialWelfare_Tribal/UI/Education_Overseas_App_Check.html", controller: "Education_Overseas_Check" });
	}

})();
