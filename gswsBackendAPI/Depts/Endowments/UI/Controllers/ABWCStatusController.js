(function () {
	var app = angular.module("GSWS");

	app.controller("ABWCStatusController", ["$scope", "$state", "$log", "Endowment_Services", Login_CTRL]);

	function Login_CTRL(scope, state, log, Endow_Services) {
		scope.preloader = false;

        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

		scope.getdata = function () {
			
			scope.divtable = false;
			if (scope.inputradio == "" || scope.inputradio == null || scope.inputradio == undefined) {
				alert("Please Select Input type");
				return;
			}
			else if (scope.benid == "" || scope.benid == null || scope.benid == undefined) {
				alert("Please Enter Input");
				return;
			}
			else {
				scope.preloader = true;
				if (scope.inputradio == "option1") {
					var req = {
						CRI: "UID",
						INPUT: scope.benid
					};
				}
				else if (scope.inputradio == "option2") {
					var req = {
						CRI: "BID",
						INPUT: scope.benid
					};
				}
				else if (scope.inputradio == "option3") {
					var req = {
						CRI: "MOB",
						INPUT: scope.benid
					};
				}
                Endow_Services.POSTENCRYPTAPI("GetApplicantStatus", req, token, function (value) {
					if (value.data.Status == "Success") {
						scope.divtable = true;
						scope.preloader = false;
						scope.benstatus = value.data["Details"];
                    }
                    else if (value.data.Status == "428") {
                        sessionStorage.clear();
                        swal("info", "Session Expired !!!", "info");
                        state.go("Login");
                        return;
                    }
					else {
						scope.preloader = false;
						alert("No Data Found");
					}
				});
			}
		}
	}
})();

