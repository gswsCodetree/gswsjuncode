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
            .state("ui.TaxAppSearch", { url: "/TaxAppSearch", templateUrl: pre_path + "Depts/PRRD/UI/TaxAppSearch.html", controller: "TaxAppSearch" })
         .state("ui.JobCardPaymentDetails", { url: "/JobCardPaymentDetails", templateUrl: pre_path + "Depts/PRRD/UI/JobCardPaymentDetails.html", controller: "JobCardPaymentDetails" })
        .state("ui.FarmerData", { url: "/FarmerData", templateUrl: pre_path + "Depts/PRRD/UI/FarmerData.html", controller: "FarmerData" })
        .state("ui.DemandCapture", { url: "/DemandCapture", templateUrl: pre_path + "Depts/PRRD/UI/DemandCapture.html", controller: "DemandCapture" })
                .state("ui.ConfirmDemand", { url: "/ConfirmDemand", templateUrl: pre_path + "Depts/PRRD/UI/ConfirmDemand.html", controller: "ConfirmDemand" })
			.state("ui.MarriageApplication", { url: "/MarriageApplication", templateUrl: pre_path + "Depts/PRRD/UI/MarriageApplication.html", controller: "MarriageApplication" })
			.state("ui.NOCApplication", { url: "/NOCApplication", templateUrl: pre_path + "Depts/PRRD/UI/NOCApplication.html", controller: "NOCApplication" })
			.state("ui.KnowYourVolunteer", { url: "/KnowYourVolunteer", templateUrl: pre_path + "Depts/PRRD/UI/KnowYourVolunteer.html", controller: "KnowYourVolunteer" })
			.state("ui.VolunteerMapping", { url: "/VolunteerMapping", templateUrl: pre_path + "Depts/PRRD/UI/VolunteerMapping.html", controller: "VolunteerMapping" })
			.state("ui.SecToVolunteerMap", { url: "/SecToVolunteerMap", templateUrl: pre_path + "Depts/PRRD/UI/SecToVolunteerMap.html", controller: "SecToVolunteerMap" })
			.state("ui.TaxAppStatus", { url: "/TaxAppStatus", templateUrl: pre_path + "Depts/PRRD/UI/TaxAppStatus.html", controller: "TaxAppStatus" })
			.state("ui.JobcardReg", { url: "/JobcardReg", templateUrl: pre_path + "Depts/PRRD/UI/JobCardReg.html", controller: "Jobcard" })
			.state("ui.Jobcard", { url: "/Jobcard", templateUrl: pre_path + "Depts/PRRD/UI/JobCardDetails.html", controller: "JobcardDetails" });

    }

})();