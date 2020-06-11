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
            .state("ui.CandidateRegistration", { url: "/CandidateRegistration", templateUrl: pre_path + "Depts/YATC/UI/CandidateRegistration.html", controller: "CandidateRegController" })
            .state("ui.CandidateLogin", { url: "/CandidateLogin", templateUrl: pre_path + "Depts/YATC/UI/CanLogin.html", controller: "CanLoginController" })
            .state("ui.UpCommingAllJobs", { url: "/UpCommingAllJobs", templateUrl: pre_path + "Depts/YATC/UI/ShowAllJobs.html", controller: "ShowAllJobsController" })
            .state("ui.UpCommingBatches", { url: "/UpCommingBatches", templateUrl: pre_path + "Depts/YATC/UI/UpCommingBatches.html", controller: "UpCommingBatchesController" })
            .state("ui.SkillStatusCheck", { url: "/SkillStatusCheck", templateUrl: pre_path + "Depts/YATC/UI/SkillStatusCheck.html", controller: "StatusCheckController" })
            ;

    }

})();