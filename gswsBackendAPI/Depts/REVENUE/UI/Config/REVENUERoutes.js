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
            .state("ui.REVENUE", { url: "/REVENUE", templateUrl: pre_path + "Depts/REVENUE/UI/Home.html", controller: "REVENUEHomeController" })
            .state("ui.RationStatus", { url: "/RationStatus", templateUrl: pre_path + "Depts/REVENUE/UI/RationStatus.html", controller: "RationStatusController" })
            .state("ui.RationRegistration", { url: "/RationRegistration", templateUrl: pre_path + "Depts/REVENUE/UI/RationReg.html", controller: "RationRegController" })

            .state("ui.ExciseComplaints", { url: "/ExciseComplaints", templateUrl: pre_path + "Depts/REVENUE/UI/Revenue_Proh_ExciseComplaints.html", controller: "ExciseComplaintController" })
            .state("ui.ExciseComplaintsStatus", { url: "/ExciseComplaintsStatus", templateUrl: pre_path + "Depts/REVENUE/UI/Excise_applicationStatusCheck.html", controller: "ExciseComplaintCheck_Controller" });
    }
    }) ();