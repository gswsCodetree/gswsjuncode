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
            .state("ui.WomenDivorcedStatus", { url: "/WomenDivorcedStatus", templateUrl: pre_path + "Depts/Minority/UI/WomenDivorcedStatus.html", controller: "WomenDivorced" })
            .state("ui.ImamAndMouzansDetails", { url: "/ImamAndMouzansDetails", templateUrl: pre_path + "Depts/Minority/UI/ImamAndMouzansData.html", controller: "ImamAndMouzans" });


    }

})();