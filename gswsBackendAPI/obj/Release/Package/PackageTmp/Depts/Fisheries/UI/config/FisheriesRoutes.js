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
            .state("ui.FisheriesAppStatus", { url: "/FisheriesAppStatus", templateUrl: pre_path + "Depts/Fisheries/UI/FApplicationStatus.html", controller: "FisheriesAppStatus" })
            .state("ui.RIDSAppStatus", { url: "/RIDSAppStatus", templateUrl: pre_path + "Depts/Fisheries/UI/RIDSAppStatus.html", controller: "RIDSAppStatus" })
            .state("ui.LLCSAppStatus", { url: "/LLCSAppStatus", templateUrl: pre_path + "Depts/Fisheries/UI/LLCSAppStatus.html", controller: "LLCSAppStatus" })
            ;

    }

})();