(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("PTRCController", ["$scope", "$state", "$log", "CT_Services", PTREG_CTRL]);

    function PTREG_CTRL(scope, state, log, ptsta_services) {
        sessionStorage.setItem("editrnr", "");
        var token = sessionStorage.getItem("Token");
        scope.preloader = false;
        scope.dttable = false;

        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

        scope.GetRC = function () {
            scope.dttable = false;
            if (!scope.entertin) {
                swal('info', "Please Enter TIN Number.", 'info');
                return false;
            }

            else {
                scope.dttable = false;
                scope.preloader = true;
                var req = { action: "GETRC", tin: scope.entertin }

                ptsta_services.POSTENCRYPTAPI("GetRCData", req, token, function (value) {
                    if (value.data.Status == 100) {
                        if (value.data.Details.status_cd == "1") {
                            var decriptdata = window.atob(value.data.Details.data);
                            var JsonData = JSON.parse(decriptdata).get_rc_dtls;

                            scope.prof_tin = JsonData.prof_tin;
                            scope.pt_reg_date = JsonData.pt_reg_date;
                            scope.division_name = JsonData.division_name;
                            scope.circle_name = JsonData.circle_name;
                            scope.enterprise_name = JsonData.enterprise_name;
                            scope.profession_type = JsonData.profession_type;
                            scope.address = JsonData.address;
                            scope.city = JsonData.city;
                            scope.mandal_code = JsonData.mandal_code;
                            scope.district_code = JsonData.district_code;
                            scope.pin = JsonData.pin;
                            scope.mobile_number = JsonData.mobile_number;
                            scope.email_id = JsonData.email_id;

                            //scope.StaDetails = JsonData;
                            console.log(JsonData);
                            scope.dttable = true;
                        }
                        else if (value.data.Status == "428") {
                            sessionStorage.clear();
                            swal("info", "Session Expired !!!", "info");
                            state.go("Login");
                            return;
                        }
                        else {
                            swal("Info", value.data.Details.error[0].message, "info");
                        }
                        console.log(value.data);
                    }
                    else { swal("Info", value.data.Reason, "info"); }

                    scope.preloader = false;

                });
            }

        }

        scope.CalPayble = function () {

        }

        scope.AddPay = function () { }

        scope.PrintReceipt = function () {

            var divprint = document.getElementById("printdiv").innerHTML;

            var popupWinindow = window.open('', 'Print-Window');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head><link href="assets/css/bootstrap.css" rel="stylesheet" type="text/css"><link rel="stylesheet" type="text/css" href="assets/css/printcss.css" /></head><body onload="window.print()">' + divprint + '</html>');
            popupWinindow.document.close();
        }
    }
})();

