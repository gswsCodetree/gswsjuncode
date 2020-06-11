(function () {
    var app = angular.module("GSWS");

    app.controller("CanLoginController", ["$scope", "$state", "$log", "YATC_Services", CanLogin_CTLR]);

    function CanLogin_CTLR(scope, state, log, canlog_services) {
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }
        scope.pagename = "Home page";
        var req = "1";

        scope.SkillLogin = function () {
            if (!(scope.skilluserid)) {
                swal("Info", "Plase Enter User ID", "info");
                return false;
            }
            else if (!(scope.skillpassword)) {
                swal("Info", "Plase Enter User ID", "info");
                return false;
            }
            else {
                var req = {
                    username: scope.skilluserid,
                    password: scope.skillpassword,
                    type: "mobile",
                };
                canlog_services.POSTENCRYPTAPI("VerifySkillCanLogin", req, token, function (value) {
                    console.log(value);
                    if (value.data.Status == 100) {
                        if (value.data.Details.status) {
                            swal("Info", "User Login Successfully.");
                            sessionStorage.setItem("SkillUserMasterId", value.data.Details.result.userMaster.id);
                            sessionStorage.setItem("SkillUserAadhaar", value.data.Details.result.aadharNumber)
                            state.go("ui.UpCommingAllJobs");
                            //console.log(value.data);
                            //scope.MandalsDD = value.data.MANDAL_DETAILS;
                        }
                        else if (value.data.Status == "428") {
                            sessionStorage.clear();
                            swal("info", "Session Expired !!!", "info");
                            state.go("Login");
                            return;
                        }
                        else
                            swal("Info", value.data.Details.message, "info");
                    }
                    else
                        swal("Info", value.data.Reason, "info");
                });
            }
        }
    }
})();