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
            .state("ui.APSPDCLServiceStatus", { url: "/APSPDCLServiceStatus", templateUrl: pre_path + "Depts/Energy/UI/APSPDCLServiceStatus.html", controller: "APSPDCLServiceStatusController" })
            .state("ui.APSPDCLServiceHistory", { url: "/APSPDCLServiceHistory", templateUrl: pre_path + "Depts/Energy/UI/APSPDCLServiceHistory.html", controller: "APSPDCLServiceHistoryController" })
            .state("ui.APEPDCLServiceStatus", { url: "/APEPDCLServiceStatus", templateUrl: pre_path + "Depts/Energy/UI/APEPDCLServiceStatus.html", controller: "APEPDCLServiceStatusController" })
            .state("ui.APEPDCLServiceHistory", { url: "/APEPDCLServiceHistory", templateUrl: pre_path + "Depts/Energy/UI/APEPDCLServiceHistory.html", controller: "APEPDCLServiceHistoryController" })
            ;

    }

})();