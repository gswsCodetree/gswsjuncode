(function () {
    var app = angular.module("GSWS");

    app.controller("SerpBimaController", ["$scope", "$state", "$log", "SERP_Services", SERPBIMAStatus_CTRL]);

    function SERPBIMAStatus_CTRL(scope, state, log, SERP_Services) {
        scope.pagename = "SERPBIMA";
        scope.preloader = false;
		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}
        var status = true;


        scope.GetStatus = function () {
            if (scope.Bimaclaim_aadhaar == "" || scope.Bimaclaim_aadhaar == null || scope.Bimaclaim_aadhaar == undefined) {
                alert("Please Enter Student Aadhaar Number.");
                return;
            }
            if (scope.Bimaclaim_aadhaar.toString().length != "12") {
                alert("Student Aadhaar Number should be 12 digits.");
                return;
            }
            else if (scope.Bimaclaim_aadhaar == "000000000000" || scope.Bimaclaim_aadhaar == "111111111111" || scope.Bimaclaim_aadhaar == "222222222222" || scope.Bimaclaim_aadhaar == "333333333333" || scope.Bimaclaim_aadhaar == "444444444444" || scope.Bimaclaim_aadhaar == "555555555555" || scope.Bimaclaim_aadhaar == "666666666666" || scope.Bimaclaim_aadhaar == "777777777777" || scope.Bimaclaim_aadhaar == "888888888888" || scope.Bimaclaim_aadhaar == "999999999999") {
                alert("Please Enter a valid Aadhaar Number");
                return;
            }
            else if (status != true) {
                alert("Please Enter a valid Aadhaar Number");
                return;
            }
            else {
                scope.divdetails = true;
                scope.divtrack = true;
                scope.preloader = true;
                var req = { claimuid: scope.Bimaclaim_aadhaar };
				SERP_Services.POSTENCRYPTAPI("GetClaimStatus", req,token, function (value) {
                    scope.CliamDetails = [];
                    if (value.data.Status == "Success") {

                        if (value.data.ClaimStatus == "0") {
                            scope.preloader = false;
                            scope.divdetails = false;
                            scope.divtrack = false;
                            alert("Invalid security key");
                            return;

                        }
                        else if (value.data.ClaimStatus == "1") {
                            scope.preloader = false;
                            scope.CliamDetails = value.data.Details.Data;
                            scope.TrackDetails = value.data.TrackDetails;
                            scope.divdetails = true;
                            scope.divtrack = true;
                        }
                        else if (value.data.ClaimStatus == "2") {
                            scope.preloader = false;
                            scope.divdetails = false;
                            scope.divtrack = false;
                            alert("Claim Not Registered with given UID");
                            return;

                        }
                        else if (value.data.ClaimStatus == "3") {
                            scope.preloader = false;
                            scope.divdetails = false;
                            scope.divtrack = false;
                            alert("No policy available with given UID");
                            return;

                        }
                        console.log(value.data.Details);
					}
					else if (value.data.Status == "428") {
						swal('info', value.data.Reason, 'info');
						sessionStorage.clear();
						state.go("Login");
						return;
					}
                    else { alert(value.data.Reason); }

                    scope.preloader = false;
                });

            }
        }

    }
})();

