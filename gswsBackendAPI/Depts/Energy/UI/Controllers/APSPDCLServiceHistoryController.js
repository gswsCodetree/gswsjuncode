(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("APSPDCLServiceHistoryController", ["$scope", "$state", "$log", "Energy_Services", EnergyHistory_CTRL]);

    function EnergyHistory_CTRL(scope, state, log, enerhistory_services) {
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }
        scope.pagename = "APSPDCL Service Status";
        scope.preloader = false;
        scope.dttable = false;

        scope.GetStatus = function () {
            if (!(scope.servicergno)) {
                alert("Please Enter Sevice Reg. Number.");
                return;
            }

            else {
                scope.preloader = true;
                scope.dttable = false;
                var req = { CSC_REGNO: scope.servicergno, PS_TXN_ID: "231231321" };
                enerhistory_services.POSTENCRYPTAPI("GetTransactionHistory", req, token,  function (value) {
                    //scope.TranDetals = [];
                    if (value.data.Status == 100) {
                        if (value.data.Details.TranDetals.FLAG == "Yes") {
                            scope.SERVICE_NAME = value.data.Details.TranDetals.SERVICE_NAME;
                            scope.STATUS_MSG = value.data.Details.TranDetals.STATUS_MSG;
                            scope.REMARKS = value.data.Details.TranDetals.REMARKS;
                            scope.TRANS_DATE = value.data.Details.TranDetals.TRANS_DATE;
                            //scope.TranDetals = value.data.TranDetals;
                            scope.dttable = true;
                        }
                        else {
                            swal("Info", value.data.Details.TranDetals.STATUS_MSG, "info");
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