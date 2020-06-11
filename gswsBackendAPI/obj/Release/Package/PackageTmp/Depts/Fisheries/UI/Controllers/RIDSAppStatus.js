(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("RIDSAppStatus", ["$scope", "$state", "$log", "Fisheries_Services", RIDS_CTRL]);

    function RIDS_CTRL(scope, state, log, ri_services) {
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
                ri_services.POSTENCRYPTAPI("RIDSAppStatus", req, token, function (value) {
                    if (value.data.Status == 100) {
                        console.log(value.data.Details);
                        var data = value.data.Details[0];
                        if (!data.Message) {
                            scope.FarmerName = data.FarmerName;
                            scope.MobileNo = data.MobileNo;
                            scope.VillageName = data.VillageName;
                            scope.MandalName = data.MandalName;
                            scope.DistrictName = data.DistrictName;
                            scope.ItemName = data.ItemName;
                            scope.Item_Quantity = data.Item_Quantity;
                            scope.ItemPrice = data.ItemPrice;
                            
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