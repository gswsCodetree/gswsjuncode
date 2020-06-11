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
            .state("ui.AmmavodiStatus", { url: "/AmmavodiStatus", templateUrl: pre_path + "Depts/Education/UI/AmmavodiStatus.html", controller: "AmmavodiController" })
            //.state("AgriCultureMainPage", { url: "/AgriCultureMainPage", templateUrl: pre_path + "Depts/AgriCulture/UI/Main.html", controller: "MainController" })
            ;

    }

})();