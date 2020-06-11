﻿(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("VahanaMitraRegStatus", ["$scope", "$state", "$log", "Transport_RandB_Services", Voh_CTRL]);

    function Voh_CTRL(scope, state, log, voh_services) {
        scope.pagename = "YSR Vahana Mitra Application Status";
        scope.preloader = false;
        scope.dttable = false;
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

        scope.validateaadhaar = function () {
            status = true;
            //if ((scope.student_aadhaar + "").toString().length == "12") {
            //    var aadhaar = scope.student_aadhaar;
            //    status = validateVerhoeff(aadhaar);
            //    if (!status) {
            //        //jQuery("#refresh").trigger("click");
            //        return status;
            //    }
            //}
        }

        scope.GetStatus = function () {
            if (!scope.ben_aadhaar) {
                swal('info', "Please Enter Aadhaar Number.", 'info');
                return;
            }
            if (scope.ben_aadhaar.toString().length != "12") {
                swal('info', "Aadhaar Number should be 12 digits.", 'info');
                return;
            }
            else if (scope.ben_aadhaar == "000000000000" || scope.ben_aadhaar == "111111111111" || scope.ben_aadhaar == "222222222222" || scope.ben_aadhaar == "333333333333" || scope.ben_aadhaar == "444444444444" || scope.ben_aadhaar == "555555555555" || scope.ben_aadhaar == "666666666666" || scope.ben_aadhaar == "777777777777" || scope.ben_aadhaar == "888888888888" || scope.ben_aadhaar == "999999999999") {
                swal('info', "Please Enter a valid Aadhaar Number", 'info');
                return;
            }
            //else if (status != true) {
            //    swal('info',"Please Enter a valid Aadhaar Number");
            //    return;
            //}
            else {
                scope.preloader = true;
                scope.dttable = false;
                var req = { AppType: "VAHANAMITRA", BenID: scope.ben_aadhaar };
                voh_services.POSTENCRYPTAPI("GetVahanaMitraRegStatus", req, token, function (value) {

                    if (value.data.Status == 100) {
                        console.log(value.data.Details);
                        var data = value.data.Details;
                        if (!data.errorMsg) {
                            scope.benfificiaryId = data.benfificiaryId;
                            scope.benfificiaryName = data.benfificiaryName;
                            scope.address = data.coaddress + " " + data.address;
                            scope.vill = data.vill;
                            scope.mandal = data.mandal;
                            scope.dist = data.dist;
                            scope.bank = data.bank;
                            scope.Status = data.Status;
                            scope.dttable = true;
                        }
                        else
                            swal('info', data.errorMsg, 'info');
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