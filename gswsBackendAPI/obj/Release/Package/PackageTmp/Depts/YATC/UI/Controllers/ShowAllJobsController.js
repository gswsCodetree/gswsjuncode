(function () {
    var app = angular.module("GSWS");

    app.controller("ShowAllJobsController", ["$scope", "$state", "$log", "YATC_Services", AllJobs_CTLR]);

    function AllJobs_CTLR(scope, state, log, alljobs_services) {
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }
        scope.pagename = "Upcomming Jobs";
        scope.dttable = false;
        var req = "1";
        LoadUpCommingJobs();

        scope.GoTO = function (type) {
            if (type == 'jobs')
                state.go("ui.UpCommingAllJobs");
            else if (type == 'batches')
                state.go("ui.UpCommingBatches");
            else
                state.go("ui.SkillStatusCheck");
        }

        scope.ApplyJob = function (row) {
            //var currjobid = row.job.job_id;
            var currjobid = row.job.JObid;
            if (!(sessionStorage.getItem("SkillUserMasterId"))) {
                swal("Info", "For Apply the you must login.", "info");
            }
            else {
                //console.log(row.job.job_id);
                console.log(row.job.JObid);
                var req = {

                    appKey: "mobile",
                    userMasterId: sessionStorage.getItem("SkillUserMasterId"),
                    //JobIds: { "data": [row.job.job_id] }
                    JobIds: { "data": [row.job.JObid] }
                };
                alljobs_services.POSTENCRYPTAPI("ApplyForJobs", req, token, function (value) {
                    if (value.data.Status == 100) {
                        if (value.data.Details.status) {
                            swal("Success", value.data.Details.message[0].replace(currjobid + ' :', ""), "success");
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

        function LoadUpCommingJobs() {
            var req = {};
            alljobs_services.POSTENCRYPTAPI("UpCommingJobs", req, token, function (value) {
                if (value.data.Status == 100) {
                    if (value.data.Details.status) {
                        scope.dttable = true;
                        scope.JobDetails = value.data.Details.data;
                    }
                    else
                        swal("Info", value.data.Details.message, "info");
                }
                else if (res.Status == "428") {
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