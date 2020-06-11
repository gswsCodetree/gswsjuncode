(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("FisheriesAppStatus", ["$scope", "$state", "$log", "Fisheries_Services", Fishe_CTRL]);

    function Fishe_CTRL(scope, state, log, fi_services) {
        scope.pagename = "Fisheries";
        scope.preloader = false;
        scope.divtable = false;
        scope.inputradio = "App";

        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

        scope.changeOption = function (stype) {
            if (stype == "App") 
                $("#apptext").html("Enter Application Number");
            else
                $("#apptext").html("Enter Transaction Number");

            scope.userinput = "";
        }

        scope.GetStatus = function () {
            if (!scope.userinput) {
                if (scope.inputradio == "App")
                    swal('info', "Please Enter Application Number.", 'info');
                else
                    swal('info', "Please Enter Transaction Number.", 'info');
                return false;
            }

            else {
                scope.preloader = true;
                scope.divtable = false;
                var req = { rtype: scope.inputradio, ref_no: scope.userinput };
                fi_services.POSTENCRYPTAPI("GetAppStatus", req, token, function (value) {
                    
                    scope.StudentDetails = [];
                    if (value.data.Status == 100) {
                        scope.Name = value.data.Details.Name;
                        scope.MobileNumber = value.data.Details["Mobile Number"];
                        scope.Village = value.data.Details.Village;
                        scope.Mandal = value.data.Details.Mandal;
                        scope.District = value.data.Details.District;
                        scope.Status = value.data.Details.Status;
                        
                        scope.divtable = true;
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