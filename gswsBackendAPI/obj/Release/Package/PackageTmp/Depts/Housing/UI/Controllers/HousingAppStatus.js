(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("HousingAppStatus", ["$scope", "$state", "$log", "Housing_Services", House_CTRL]);

    function House_CTRL(scope, state, log, ho_services) {
        scope.pagename = "Housing";
        scope.preloader = false;
        scope.dttable = false;
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

        scope.GetStatus = function () {
            if (!scope.userinput) {
                swal('info', "Please Enter Number.", 'info');
                return false;
            }
            
            else {
                scope.preloader = true;
                scope.dttable = false;
                var req = { ref_no: scope.userinput };
                ho_services.POSTENCRYPTAPI("GetAppStatus", req, token, function (value) {
                    console.log(value);
                    
                    if (value.data.Status == 100) {
                        if (value.data.Details) {
                            
							if (value.data.Details.property.entry["0"]) {
								scope.ben_id = value.data.Details.property.entry["9"].value.$;
								scope.ben_name = value.data.Details.property.entry["10"].value.$;
								scope.dist_name = value.data.Details.property.entry["1"].value.$;
								scope.mandal_name = value.data.Details.property.entry["2"].value.$;
								scope.village_name = value.data.Details.property.entry["3"].value.$;
								scope.reg_no = value.data.Details.property.entry["4"].value.$;
								scope.uid_no = value.data.Details.property.entry["5"].value.$;
								scope.ration_card_no = value.data.Details.property.entry["6"].value.$;
								scope.phase_status = value.data.Details.property.entry["7"].value.$;
								scope.scheme_name = value.data.Details.property.entry["8"].value.$;
								scope.ben_status = value.data.Details.property.entry["0"].value.$;
								console.log(value.data.Details);
								scope.dttable = true;
							}
							else { swal('info', value.data.Details.property.entry["value"].$, 'info'); }
                        }
                           
                        else
                            swal('info', "No Data Found", 'info');
                       
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