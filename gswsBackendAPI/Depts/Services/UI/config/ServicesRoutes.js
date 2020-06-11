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
			.state("ui.NPCIAppStatus", { url: "/NPCIAppStatus", templateUrl: pre_path + "Depts/Services/UI/NPCIStatus.html", controller: "NPCIController" })
			//.state("AgriCultureMainPage", { url: "/AgriCultureMainPage", templateUrl: pre_path + "Depts/AgriCulture/UI/Main.html", controller: "MainController" })
			.state("ui.Textile", { url: "/Textile", templateUrl: pre_path + "Depts/Services/UI/Textile.html", controller: "TextileCntrl" })
			.state("ui.CertificatesDownload", { url: "/CertificatesDownload", templateUrl: pre_path + "Depts/Services/UI/PelliKaanuka_Certificates_Download.html", controller: "CertificateDownlaod" })
			.state("ui.AssetTracking", { url: "/AssetTracking", templateUrl: pre_path + "Depts/Services/UI/AssetTracking.html", controller: "AssetTrackCntrl" })
			.state("ui.HardwareIssues", { url: "/HardwareIssues", templateUrl: pre_path + "Depts/Services/UI/HardwareIssues.html", controller: "HardwareIssuesCntrl" })
			.state("uc.Navasakam", { url: "/Navasakam", templateUrl: pre_path + "Depts/Services/UI/Navasakam.html", controller: "NavasakamCTRL" })
			.state("ui.RajakasAppStatus", { url: "/RajakasAppStatus", templateUrl: pre_path + "Depts/Services/UI/RajakasAppStatus.html", controller: "RajakasAppStatus" })
			.state("ui.TailorsAppStatus", { url: "/TailorsAppStatus", templateUrl: pre_path + "Depts/Services/UI/TailorsAppStatus.html", controller: "TailorsAppStatus" })
			.state("ui.NayeeBrahminsAppStatus", { url: "/NayeeBrahminsAppStatus", templateUrl: pre_path + "Depts/Services/UI/NayeeBrahminsAppStatus.html", controller: "NayeeBrahminsAppStatus" })
			.state("ui.PastorsAppStatus", { url: "/PastorsAppStatus", templateUrl: pre_path + "Depts/Services/UI/PastorsAppStatus.html", controller: "PastorsAppStatus" })
			.state("ui.KapuNestamAppStatus", { url: "/KapuNestamAppStatus", templateUrl: pre_path + "Depts/Services/UI/KapuNestamAppStatus.html", controller: "KapuNestamAppStatus" })
			.state("ui.MUIDAppStatus", { url: "/MUIDAppStatus", templateUrl: pre_path + "Depts/Services/UI/MUIDAppStatus.html", controller: "MUIDAppStatus" })
			.state("ui.ArogyasreeAppStatus", { url: "/ArogyasreeAppStatus", templateUrl: pre_path + "Depts/Services/UI/ArogyasreeAppStatus.html", controller: "ArogyasreeAppStatus" })
			.state("ui.LaborAppStatus", { url: "/LaborAppStatus", templateUrl: pre_path + "Depts/Services/UI/LaborAppStatus.html", controller: "LaborAppStatus" })
			.state("ui.JVDAppStatus", { url: "/JVDAppStatus", templateUrl: pre_path + "Depts/Services/UI/JVDAppStatus.html", controller: "JVDAppStatus" })
			.state("uc.MeesevaAppStatus", { url: "/MeesevaAppStatus", templateUrl: pre_path + "Depts/Services/UI/MeesevaAppStatus.html", controller: "MeesevaAppStatus" });
    }

})();