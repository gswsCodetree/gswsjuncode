(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("LLCSAppStatus", ["$scope", "$state", "$log", "Fisheries_Services", LLCS_CTRL]);

    function LLCS_CTRL(scope, state, log, llcs_services) {
        scope.pagename = "RIDS Application Status";
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

        scope.ChangeText = function () {
            if (scope.seltype == "1")
                $("#lbltxt").html("Aadhaar Number ");
            else if (scope.seltype == "2")
                $("#lbltxt").html("Application Number ");
            else
                $("#lbltxt").html("Input Text ");
        }

        scope.GetStatus = function () {
            if (!scope.seltype) {
                swal('info', "Please Select Type of Search.", 'info');
                return false;
            }
            if (!scope.userinput) {
                scope.userinput = '';
                if (scope.seltype == "1")
                    swal('info', "Please Enter Aadhaar Number.", 'info');
                else
                    swal('info', "Please Enter Application Number.", 'info');
                return false;
            }

            else {
                scope.preloader = true;
                scope.divtable = false;
                var req = { Type: scope.seltype, UniqueNo: scope.userinput };
                llcs_services.POSTENCRYPTAPI("LLCSAppStatus", req, token, function (value) {
                    if (value.data.Status == 100) {
                        console.log(value.data.Details);
                        var data = value.data.Details[0];
                        if (!data.Message) {
                            scope.BeneficiaryName = data.BeneficiaryName;
                            scope.TagNumber = data.TagNumber;
                            scope.MobileNo = data.MobileNo;
                            scope.DeathDate = data.DeathDate;
                            scope.ApplicationNo = data.ApplicationNo;
                            scope.AadharNo = data.AadharNo;
                            scope.VillageName = data.VillageName;
                            scope.MandalName = data.MandalName;
                            scope.DistrictName = data.DistrictName;
                            scope.StatusName = data.StatusName;
                            

                            scope.divtable = true;
                        }
                        else
                            swal('info', data.Message, 'info');
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