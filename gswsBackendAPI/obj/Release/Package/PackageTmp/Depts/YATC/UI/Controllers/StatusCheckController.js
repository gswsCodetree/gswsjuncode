(function () {
    var app = angular.module("GSWS");

    app.controller("StatusCheckController", ["$scope", "$state", "$log", "YATC_Services", Status_CTLR]);

    function Status_CTLR(scope, state, log, status_services) {
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }
        scope.pagename = "Status Check";
        scope.dttable = false;
        scope.preloader = false;
        scope.typejob = false;
        scope.typebatch = false;

        if (!(sessionStorage.getItem("SkillUserMasterId"))) {
            swal("Info", "For Check Status you must login.", "alert");
            state.go("ui.CandidateLogin");
        }

        scope.GoTO = function (type) {
            if (type == 'jobs')
                state.go("ui.UpCommingAllJobs");
            else if (type == 'batches')
                state.go("ui.UpCommingBatches");
            else
                state.go("ui.SkillStatusCheck");
        }

        scope.CategoryChange = function () {
            scope.dttable = false;
            if (!(sessionStorage.getItem("SkillUserMasterId"))) {
                swal("Info", "For Check Status you must login.", "alert");
                state.go("ui.CandidateLogin");
            }
            else {
                scope.JobsDD = [];
                scope.BatchesDD = [];
                if (scope.selcategory == "JFO") {
                    LoadJobs();
                    scope.typejob = true;
                    scope.typebatch = false;
                }
                else if (scope.selcategory == "TC_BATCH") {
                    LoadBatches();
                    scope.typejob = false;
                    scope.typebatch = true;
                }
                else { scope.typejob = false; scope.typebatch = false; }
            }
        }

        scope.GetStatus = function () {
            scope.dttable = false;
            if (!(sessionStorage.getItem("SkillUserMasterId"))) {
                swal("Info", "For Check Status you must login.", "alert");
                state.go("ui.CandidateLogin");
            }
            else {
                if (!(scope.selcategory)) {
                    alert("Please Select Category");
                    return false;
                }
                else if (scope.selcategory == "JFO" && !(scope.seljob)) {
                    alert("Please Select Job Name");
                    return false;
                }
                else if (scope.selcategory == "TC_BATCH" && !(scope.selbatch)) {
                    alert("Please Select Batch Name");
                    return false;
                }
                else {
                    scope.preloader = true;
                    var req = {};
                    if (scope.selcategory == "JFO")
                        req = { key: scope.selcategory, userMasterId: sessionStorage.getItem("SkillUserMasterId"), batchId: scope.seljob };
                    else
                        req = { key: scope.selcategory, userMasterId: sessionStorage.getItem("SkillUserMasterId"), batchId: scope.selbatch };

                    //var req = {
                    //    key: "JFO",
                    //    //userMasterId: "1064473",
                    //    userMasterId: sessionStorage.getItem("SkillUserMasterId"),
                    //    batchId: scope.seljob,
                    //    //batchId: "464",
                    //};

                    status_services.POSTENCRYPTAPI("GetSkillAppStatus", req, token, function (value) {
                        scope.preloader = false;
                        if (value.data.Status == 100) {
                            scope.dttable = true;
                            if (value.data.Details.status)
                                scope.app_status = value.data.Details.data;
                            
                            else
                                scope.app_status = value.data.Details.message;
                           
                        }
                        else if (value.data.Status == "428") {
                            sessionStorage.clear();
                            swal("info", "Session Expired !!!", "info");
                            state.go("Login");
                            return;
                        }
                        else
                            swal("Info", value.data.Reason, "alert");
                    });
                }
            }
        }

        function LoadJobs() {
            var req = {
                appKey: "apssdc-rtg-jobskills",
                userMasterId: sessionStorage.getItem("SkillUserMasterId")
            };
            status_services.POSTENCRYPTAPI("LoadCanJobs", req, token, function (value) {
                if (value.data.Status == 100) {
                    scope.JobsDD = value.data.Details.data;
                }
                else if (value.data.Status == "428") {
                    sessionStorage.clear();
                    swal("info", "Session Expired !!!", "info");
                    state.go("Login");
                    return;
                }
                else
                    swal("Info", value.data.Reason, "alert");
            });
        }

        function LoadBatches() {
            var req = {
                userMasterId: sessionStorage.getItem("SkillUserMasterId")
            };
            status_services.POSTENCRYPTAPI("LoadCanBatches", req, token, function (value) {
                if (value.data.Status == 100) {
                    scope.BatchesDD = value.data.Details.data;
                }
                else if (value.data.Status == "428") {
                    sessionStorage.clear();
                    swal("info", "Session Expired !!!", "info");
                    state.go("Login");
                    return;
                }
                else
                    swal("Info", value.data.Reason, "alert");
            });
        }

    }
})();