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
            .state("ui.HAppStatus", { url: "/HAppStatus", templateUrl: pre_path + "Depts/Housing/UI/HousingAppStatus.html", controller: "HousingAppStatus" })
            .state("ui.HSitesAppStatus", { url: "/HSitesAppStatus", templateUrl: pre_path + "Depts/Housing/UI/HSitesAppStatus.html", controller: "HSitesAppStatus" })
            .state("ui.HSitesAppReg", { url: "/HSitesAppReg", templateUrl: pre_path + "Depts/Housing/UI/HSitesAppReg.html", controller: "HSitesAppReg" })
            ;

    }

})();