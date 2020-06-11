(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("APEPDCLServiceHistoryController", ["$scope", "$state", "$log", "Energy_Services", EnergyHistory_CTRL]);

    function EnergyHistory_CTRL(scope, state, log, enerhistory_services) {
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }
        scope.pagename = "APEPDCL Service History";
        scope.preloader = false;
        scope.dttable = false;

        scope.GetStatus = function () {
            if (!(scope.servicergno)) {
                alert("Please Enter Sevice Reg. Number.");
                return false;
            }
            //else if (!(scope.mobileno)) {
            //    alert("Please Enter Mobile Number.");
            //    return false;
            //}
            //else if (scope.mobileno.length < 10) {
            //    alert("Mobile Number Should be 10 Digits.");
            //    return false;
            //}

            else {
                scope.preloader = true;
                scope.dttable = false;
                var req = { REG_NO: scope.servicergno };
                enerhistory_services.POSTENCRYPTAPI("GetAPEPDCLTransactionHistory", req,token, function (value) {
                    scope.TranDetals = [];
                    if (value.data.Status == 100) {
                        if (value.data.Details) {
                            scope.StatusOn = value.data.Details[0]["Status On"];
                            scope.CustInfo = value.data.Details[0]["Information to Consumer"];
                            scope.PReason = value.data.Details[0]["PendingReason/Details"];
                            scope.Status = value.data.Details[0]["Status"];

                            //scope.TranDetals = value.data.Details;
                            scope.dttable = true;
                        }
                        else {
                            swal("Info", "No Data Found.", "info");
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