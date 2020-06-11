(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("APEPDCLServiceStatusController", ["$scope", "$state", "$log", "Energy_Services", EnergyStatus_CTRL]);

    function EnergyStatus_CTRL(scope, state, log, enerstatus_services) {
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }
        scope.pagename = "APEPDCL Service Status";
        scope.preloader = false;
        scope.dttable = false;

        scope.GetStatus = function () {
            if (!(scope.servicergno)) {
                alert("Please Enter Sevice Reg. Number.");
                return false;
            }
            else if (!(scope.mobileno)) {
                alert("Please Enter Mobile Number.");
                return false;
            }
            else if (scope.mobileno.length < 10) {
                alert("Mobile Number Should be 10 Digits.");
                return false;
            }

            else {
                scope.preloader = true;
                scope.dttable = false;
                var req = { REQUEST_NO: scope.servicergno, MOBILE: scope.mobileno };
                enerstatus_services.POSTENCRYPTAPI("GetAPEPDCLServiceStatus", req, token,  function (value) {
                    scope.TranDetals = [];
                    if (value.data.Status == 100) {
                        if (value.data.Details.STATUS) {
                            scope.TranDetals = value.data.Details.ACTION;
                            scope.dttable = true;
                        }
                        else {
                            swal("Info", value.data.Details.ERROR_MSG, "info");
                        }

                    }
                    else if (value.data.Status == "428") {
                        sessionStorage.clear();
                        swal("info", "Session Expired !!!", "info");
                        state.go("Login");
                        return;
                    }
                    else {
                        swal("Info", value.data.Reason, "info");

                    }
                    scope.preloader = false;
                });

            }
        }
    };
})();