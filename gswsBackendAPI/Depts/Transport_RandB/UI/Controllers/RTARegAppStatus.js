(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("RTARegAppStatus", ["$scope", "$state", "$log", "Transport_RandB_Services", RTA_CTRL]);

    function RTA_CTRL(scope, state, log, ho_services) {
        scope.pagename = "Housing";
        scope.preloader = false;
        scope.dttable = false;
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

        scope.GetStatus = function () {
            if (!scope.AppNo) {
                swal('info', "Please Enter Application Number.", 'info');
                return false;
            }
            else if (!scope.ChassisNo) {
                swal('info', "Please Enter Chassis Number.", 'info');
                return false;
            }

            else {
                scope.preloader = true;
                scope.dttable = false;
                var req = { module: "REG", applicationNo: scope.AppNo, chassisNo: scope.ChassisNo };
                ho_services.POSTENCRYPTAPI("GetRTARegAppStatus", req, token, function (value) {
                    console.log(value);

                    if (value.data.Status == 100) {
                        var data = value.data.Details;
                        if (data.status) {
                            scope.applicationNumber = data.result[0].applicationNumber;
                            scope.applicantName = data.result[0].applicantName;
                            scope.prNo = data.result[0].prNo;
                            scope.trNo = data.result[0].trNo;
                            scope.applicationStatus = data.result[0].applicationStatus;
                            scope.fatherName = data.result[0].fatherName;
                            scope.dttable = true;
                        }
                        else
                            swal('info', data.message, 'info');
                    }
                    else if (value.data.Status == "428") {
                        sessionStorage.clear();
                        swal("info", "Session Expired !!!", "info");
                        state.go("Login");
                        return;
                    }
                    else { swal('info', value.data.Reason, 'info'); }

                    scope.preloader = false;

                });

            }
        }
    }
})();