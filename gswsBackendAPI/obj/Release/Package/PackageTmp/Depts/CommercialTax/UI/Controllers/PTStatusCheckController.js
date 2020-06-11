(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("PTStatusCheckController", ["$scope", "$state", "$log", "CT_Services", PTREG_CTRL]);

    function PTREG_CTRL(scope, state, log, ptsta_services) {
        sessionStorage.setItem("editrnr", "");
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }
        scope.preloader = false;
        scope.divdate = true;
        scope.inputradio = "option1";

        scope.changeOption = function (type) {
            scope.dttable = false;
            scope.fromdate = "";
            scope.todate = "";
            scope.rnrno = "";

            if (type == "option1") {
                scope.divdate = true;
            }
            else {
                scope.divdate = false;
            }
        }

        scope.AppModify = function (tin) {
            sessionStorage.setItem("editrnr", tin);
            state.go("ui.PTRegEdit");
        }

        scope.GetStatus = function (type) {
            scope.dttable = false;
            if (type == "1" && !scope.fromdate) {
                swal('info', "Please Select From Date.", 'info');
                return false;
            }
            else if (type == "1" && !scope.todate) {
                swal('info', "Please Select To Date.", 'info');
                return false;
            }
            else if (type == "2" && !scope.rnrno) {
                swal('info', "Please Enter RNR Number.", 'info');
                return false;
            }
            else {
                scope.preloader = true;
                var req = {};
                if (type == "1")
					req = { action: "GETALT", gen_date: (moment(scope.fromdate).format('YYYY-MM-DD') + " 00:00"), to_date: (moment(scope.todate).format('YYYY-MM-DD') + " 23:59"), rnr: "", gsws_id: ""}
                else
					req = { action: "GETALT", gen_date: "", to_date: "", rnr: scope.rnrno, gsws_id: "" }

                ptsta_services.POSTENCRYPTAPI("GetAppStatus", req, token, function (value) {
                //ptsta_services.DemoAPI("GetAppStatus", req, function (value) {
                    if (value.data.Status == 100) {
                        if (value.data.Details.status_cd == "1") {
                            var decriptdata = window.atob(value.data.Details.data);
                            var JsonData = JSON.parse(decriptdata);
                            scope.StaDetails = JsonData;
                            console.log(JsonData);
                            scope.dttable = true;
                        }
                        else {
                            swal("Info", value.data.Details.error[0].message, "info");
                        }
                        console.log(value.data);
                    }
                    else if (value.data.Status == "428") {
                        sessionStorage.clear();
                        swal("info", "Session Expired !!!", "info");
                        state.go("Login");
                        return;
                    }
                    else { swal("Info", value.data.Reason, "info"); }

                    scope.preloader = false;

                });
            }

        }


    }
})();

