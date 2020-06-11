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
            .state("ui.YSRNavodayamOTRReg", { url: "/YSRNavodayamOTRReg", templateUrl: pre_path + "Depts/Industries/UI/YSRNavodayamOTRReg.html", controller: "YSRNavodayamOTRRegController" })

            ;

    }

})();