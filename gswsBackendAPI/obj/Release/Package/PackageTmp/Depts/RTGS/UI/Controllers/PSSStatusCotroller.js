(function () {
	var app = angular.module("GSWS");

	app.controller("PSSStatusController", ["$scope", "$state", "$log", "RTGS_Services", RTGSStatus_CTRL]);

	function RTGSStatus_CTRL(scope, state, log, RTGS_Services) {
		scope.preloader = false;
		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}
		scope.getdata = function () {
			scope.preloader = true;
			scope.divtable = false;
			var req = {
				CRI: "2",
				INPUT: scope.uid
			};
			RTGS_Services.POSTENCRYPTAPI("GetApplicantStatus", req,token, function (value) {
				if (value.data.Status == "Success") {
					scope.divtable = true;
					scope.preloader = false;
					scope.benstatus = value.data["Details"];
				}
				else if (value.data.Status == "428") {
					swal('info', value.data.Reason, 'info');
					sessionStorage.clear();
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
})();

