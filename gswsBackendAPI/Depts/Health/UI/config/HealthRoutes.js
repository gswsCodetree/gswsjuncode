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
            .state("ui.ArogyarakshaStatus", { url: "/ArogyarakshaStatus", templateUrl: pre_path + "Depts/Health/UI/ArogyaRakshaStatus.html", controller: "ArogyaRakshaController" })
			.state("ui.SadaremCertificate", { url: "/SadaremCertificate", templateUrl: pre_path + "Depts/Health/UI/SadaremCertificate.html", controller: "SadaremCertificate" })
			.state("ui.YSRKantiVelugu", { url: "/YSRKantiVelugu", templateUrl: pre_path + "Depts/Health/UI/YSRKantiVelugu.html", controller: "YSRKantiVeluguController" })
            ;

    }

})();