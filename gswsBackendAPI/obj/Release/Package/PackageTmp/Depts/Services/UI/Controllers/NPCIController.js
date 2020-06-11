(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("NPCIController", ["$scope", "$state", "$log", "Ser_Services", Services_CTRL]);

    function Services_CTRL(scope, state, log, ser_services) {
        scope.pagename = "NPCI Status";
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
            //if ((scope.aadhaar + "").toString().length == "12") {
            //    var aadhaar = scope.aadhaar;
            //    status = validateVerhoeff(aadhaar);
            //    if (!status) {
            //        //jQuery("#refresh").trigger("click");
            //        return status;
            //    }
            //}
        }

        scope.GetStatus = function () {
            if (scope.aadhaar == "" || scope.aadhaar == null || scope.aadhaar == undefined) {
                swal('info', "Please Enter Aadhaar Number.", "info");
                return;
            }
            if (scope.aadhaar.toString().length != "12") {
                swal('info', "Aadhaar Number should be 12 digits.", "info");
                return;
            }
            else if (scope.aadhaar == "000000000000" || scope.aadhaar == "111111111111" || scope.aadhaar == "222222222222" || scope.aadhaar == "333333333333" || scope.aadhaar == "444444444444" || scope.aadhaar == "555555555555" || scope.aadhaar == "666666666666" || scope.aadhaar == "777777777777" || scope.aadhaar == "888888888888" || scope.aadhaar == "999999999999") {
                swal('info', "Please Enter a valid Aadhaar Number", "info");
                return;
            }
            //else if (status != true) {
            //    swal('info',"Please Enter a valid Aadhaar Number");
            //    return;
            //}
            else {
                scope.preloader = true;
                scope.dttable = false;
                var req = { ftype: 1, fdpart_id: null, fadhar_no: scope.aadhaar };
                ser_services.POSTENCRYPTAPI("GetNPCIStatus", req, token, function (value) {
                    scope.StudentDetails = [];
                    if (value.data.Status == 100) {
                        scope.app_status = value.data.Details[0]["STATUS"];
                        if (scope.app_status == "Active")
                            scope.lblactive = true;
                        else
                            scope.lblactive = false;
                        console.log(value.data.Details);
                    }
                    else if (value.data.Status == "428") {
                        sessionStorage.clear();
                        swal("info", "Session Expired !!!", "info");
                        state.go("Login");
                        return;
                    }
                    else { swal('info', value.data.Reason, "info"); }

                    scope.preloader = false;
                    scope.dttable = true;
                });

            }
        }
    };
})();