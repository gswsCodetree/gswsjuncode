(function () {
    var app = angular.module("GSWS");

    app.controller("UpCommingBatchesController", ["$scope", "$state", "$log", "YATC_Services", Batches_CTLR]);

    function Batches_CTLR(scope, state, log, batches_services) {
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }
        scope.pagename = "Upcomming Batches";
        scope.dttable = false;
        var req = "1";
        LoadDistrics();

        scope.GoTO = function (type) {
            if (type == 'jobs')
                state.go("ui.UpCommingAllJobs");
            else if (type == 'batches')
                state.go("ui.UpCommingBatches");
            else
                state.go("ui.SkillStatusCheck");
        }

        scope.GetBatches = function () {
            var req = {
                appKey: "APCS",
                district: scope.district
            };
            batches_services.POSTENCRYPTAPI("UpCommingBatches", req, token, function (value) {
                if (value.data.Status == 100) {
                    if (value.data.Details.status == "success") {
                        scope.dttable = true;
                        scope.BatchesDetails = value.data.Details.data;
                    }
                    else
                        swal("Info", value.data.Details.message, "info");
                }
                else if (value.data.Status == "428") {
                    sessionStorage.clear();
                    swal("info", "Session Expired !!!", "info");
                    state.go("Login");
                    return;
                }
                else
                    swal("Info", value.data.Reason, "info");
            });
        }

        scope.ApplyBatch = function (row) {
            if (!(sessionStorage.getItem("SkillUserMasterId"))) {
                swal("Info", "For Apply the you must login.", "info");
            }
            else {
                console.log(row.batch.batchId);
                var req = {
                    appKey: "mobile",
                    tcId: "0",
                    programId: row.batch.programId,
                    applicationType: "TC_BATCH",
                    batchId: row.batch.batchId,
                    userMasterId: sessionStorage.getItem("SkillUserMasterId"),
                };
                batches_services.POSTENCRYPTAPI("ApplyForBatch", req, token, function (value) {
                    if (value.data.Status == 100) {
                        if (value.data.Details.status) {
                            swal("Success", value.data.Details.message, "success");
                        }
                        else
                            swal("Info", value.data.Details.message, "info");
                    }
                    else if (value.data.Status == "428") {
                        sessionStorage.clear();
                        swal("info", "Session Expired !!!", "info");
                        state.go("Login");
                        return;
                    }
                    else
                        swal("Info", value.data.Reason, "info");
                });
            }
        }

        function LoadDistrics() {
            var req = {
                key: "DISTRICT",
                ref_id: "189"
            };
            batches_services.POSTENCRYPTAPI("SkillDistrics", req, token, function (value) {
                if (value.data.Status == 100) {
                    scope.DistrictDD = value.data.Details;
                }
                else if (value.data.Status == "428") {
                    sessionStorage.clear();
                    swal("info", "Session Expired !!!", "info");
                    state.go("Login");
                    return;
                }
                else 
                    swal("Info", value.data.Reason, "info");
            });
        }
    }
})();