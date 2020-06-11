(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("LLRAppStatus", ["$scope", "$state", "$log", "Transport_RandB_Services", LLR_CTRL]);

    function LLR_CTRL(scope, state, log, llr_services) {
        scope.pagename = "LLR Application Status";
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
            if (!scope.appnumber) {
                swal('info', "Please Enter Application Number.", 'info');
                return;
            }
            if (!scope.dateofbirth) {
                swal('info', "Please Select Date Of Birht", 'info');
                return;
            }
            else {
                scope.preloader = true;
                scope.dttable = false;
                var req = {
                    applicationFormNo: scope.appnumber,
                    dob: moment(scope.dateofbirth).format('DD-MM-YYYY')
                };
                llr_services.POSTENCRYPTAPI("LLRAplicationsStatus", req, token, function (value) {

                    if (value.data.Status == 100) {
                        console.log(value.data.Details);
                        var data = value.data.Details;
                        if (data.status) {
                            var result = data.result[0];
                            scope.applicantFirstName = result.applicantFirstName;
                            scope.applicantLastName = result.applicantLastName;
                            scope.aadharNo = result.aadharNo ;
                            scope.llrRenewed = result.llrRenewed;
                            scope.llrApplicationFormNo = result.llrApplicationFormNo;
                            scope.applicantDob = result.applicantDob;
                            scope.applicationStatus = result.applicationStatus;
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