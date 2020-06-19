(function () {

    var app = angular.module("GSWS");
    app.config(["$stateProvider", App_config]);

    function App_config($stateProvider) {
        var web_site = location.hostname;
        var pre_path = "";
        if (web_site !== "localhost") {
            pre_path = "/" + location.pathname.split("/")[1] + "/";
        }
        $stateProvider
            .state("ui.pensionNewApplication", { url: "/pensionNewApplication", templateUrl: pre_path + "Depts/Pension/UI/pensionNewApplication.html", controller: "pensionDeptController" })
            .state("uc.pensionGrevinanceList", { url: "/pensionGrevinanceList", templateUrl: pre_path + "Depts/Pension/UI/pensionGrevinanceList.html", controller: "pensionGrevinanceListController" })
            .state("uc.pensionWEAVerification", { url: "/pensionWEAVerification", templateUrl: pre_path + "Depts/Pension/UI/pensionWEAVerification.html", controller: "pensionWEAVerificationController", params: { txnId: null, grevId: null } })
            .state("uc.PensionEndorsement", { url: "/PensionEndorsement", templateUrl: pre_path + "Depts/Pension/UI/PensionEndorsement.html", controller: "PensionEndorsementController" })
            .state("uc.NegativeEndorsementPrint", { url: "/NegativeEndorsementPrint", templateUrl: pre_path + "Depts/Pension/UI/NegativeEndorsementPrint.html", controller: "NegativeEndorsementPrintController" })
            .state("uc.SanctionOrderPrint", { url: "/SanctionOrderPrint", templateUrl: pre_path + "Depts/Pension/UI/SanctionOrderPrint.html", controller: "SanctionOrderPrintController" })
            .state("uc.MpdoRejectedListForAppeal", { url: "/MpdoRejectedListForAppeal", templateUrl: pre_path + "Depts/Pension/UI/MpdoRejectedListForAppeal.html", controller: "MpdoRejectedListForAppealController" })
            .state("uc.rejectedIndividualData", { url: "/rejectedIndividualData", templateUrl: pre_path + "Depts/Pension/UI/rejectedIndividualData.html", controller: "rejectedIndividualDataController", params: { txnId: null, grevId: null } })
            .state("uc.PensionsBeneficiaryList", { url: "/PensionsBeneficiaryList", templateUrl: pre_path + "Depts/Pension/UI/PensionsBeneficiaryList.html", controller: "PensionsBeneficiaryListController" })
            .state("uc.PensionSocialAudit", { url: "/PensionSocialAudit", templateUrl: pre_path + "Depts/Pension/UI/PensionSocialAudit.html", controller: "PensionSocialAuditController" });

    }

    app.service("pensionDeptServices", ["network_service", pensionDeptServices]);

    function pensionDeptServices(ns, state) {

        var Internal_Services = this;
        baseurl = "/api/pensionDept/";

        Internal_Services.post = function (methodname, input, callback) {

            ns.post(baseurl + methodname, input, function (data) {
                callback(data);

            }, function (error) {
                callback(error);
            });
        };

        Internal_Services.encrypt_post = function (methodname, input, token, callback) {

            ns.encrypt_post(baseurl + methodname, input, token, function (data) {
                callback(data);

            }, function (error) {
                callback(error);
            });
        };
    }


})();