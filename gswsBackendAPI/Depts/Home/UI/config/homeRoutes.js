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
			.state("ui.HomeLhms", { url: "/HomeLhmsPage", templateUrl: pre_path + "Depts/Home/UI/Home.html", controller: "HomePoliceController" })
			.state("ui.HomerecoverVehcileStatus", { url: "/HomerecoverVehcileStatus", templateUrl: pre_path + "Depts/Home/UI/Recover_VehicleSearch.html", controller: "Home_VehicleRecoverySearchCntrl" })
			.state("ui.HomeCheckPetitionStatus", { url: "/HomeCheckPetitionStatus", templateUrl: pre_path + "Depts/Home/UI/PetitionStatusCheck.html", controller: "Home_PetitionStatusCntrl" })
			.state("ui.HomeKnowCaseStatus", { url: "/HomeKnowCaseStatus", templateUrl: pre_path + "Depts/Home/UI/KnowCaseStatus.html", controller: "Home_KnowCaseStatusCntrl" })
			.state("ui.HomeViewFIR", { url: "/HomeViewFIR", templateUrl: pre_path + "Depts/Home/UI/ViewFIR.html", controller: "Home_ViewFIRCntrl" })
			.state("ui.HomeArrestParticular", { url: "/HomeArrestParticular", templateUrl: pre_path + "Depts/Home/UI/ArrestParticularsReport.html", controller: "Home_ArrestParticularsCntrl" })
			.state("ui.HomeWantedCriminals", { url: "/HomeWantedCriminals", templateUrl: pre_path + "Depts/Home/UI/WantedCriminals.html", controller: "Home_WantedCriminalsCntrl" })
			.state("ui.HomeUnknownBodies", { url: "/HomeUnknownBodies", templateUrl: pre_path + "Depts/Home/UI/Unknowndeadbodies.html", controller: "Home_UnknownBodiesCntrl" })
			.state("ui.HomeMissedKidnappedPersons", { url: "/HomeMissedKidnappedPersons", templateUrl: pre_path + "Depts/Home/UI/Missed_Kidnapped_Persons.html", controller: "Home_MissedKidnappedCntrl" })
			.state("ui.EchallanStatus", { url: "/EchallanStatus", templateUrl: pre_path + "Depts/Home/UI/EchallanStatus.html", controller: "EchallanController" })
			.state("ui.HomeLHMS", { url: "/LHMS", templateUrl: pre_path + "Depts/Home/UI/LHMS.html", controller: "Home_LHMSCntrl" })
			.state("ui.CrimePetition", { url: "/CrimePetition", templateUrl: pre_path + "Depts/Home/UI/CrimePetition.html", controller: "CrimePetition" });
	}

})();
