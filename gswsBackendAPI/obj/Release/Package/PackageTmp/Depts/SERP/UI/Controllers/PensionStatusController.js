(function () {
	var app = angular.module("GSWS");

	app.controller("PensionStatusController", ["$scope", "$state", "$log", "SERP_Services", SERPStatus_CTRL]);

	function SERPStatus_CTRL(scope, state, log, SERP_Services) {
		scope.preloader = false;

		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}
		scope.getdata = function () {
			scope.divtable = false;
			if (scope.inputradio == "" || scope.inputradio == null || scope.inputradio == undefined) {
				alert("Please Select Input type");
				return;
			}
			else if (scope.uid == "" || scope.uid == null || scope.uid == undefined) {
				alert("Please Enter Input");
				return;
			}
			else {
				scope.preloader = true;
				if (scope.inputradio == "option1") {
					var req = {
						CRI: "2",
						UID: scope.uid
					};
				}
				else if (scope.inputradio == "option2") {
					var req = {
						CRI: "2",
						PID: scope.uid
					};
				}
				SERP_Services.POSTENCRYPTAPI("GetApplicantStatus", req,token, function (value) {
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
	}
})();

