(function () {
	var app = angular.module("GSWS");

	app.controller("RandBComplaintCheck_Controller", ["$scope", "$state", "$log", "Transport_RandB_Services", Transport_CTRL]);

	function Transport_CTRL(scope, state, log, Transport_Services) {
		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}

		scope.getdetails = function () {
			if (!(scope.txtcmplttid)) {
				alert('Please enter Complaint ID');
				return;
			}
			var req = {
				appcode: scope.txtcmplttid
			};
			Transport_Services.POSTENCRYPTAPI("Complaint_StatusCheck", req,token, function (value) {
				var res = value.data;
				if (res.Status == "Success") {
					scope.detailsshow = true;
					if (!(res.Data)) {
						scope.detailsshow = false;
						scope.ResData = "";
						alert("No Data available for this complaint id");
						return;
					}
					else if (value.data.Status == "428") {
						swal('info', value.data.Reason, 'info');
						sessionStorage.clear();
						state.go("Login");
						return;
					}
					else
						scope.ResData = res.Data;
				}

				else {
					scope.detailsshow = false;
					scope.ResData = "";
					alert("No Data available for this complaint id");
					return;
				}
			});
		};

		scope.Refresh = function () {
			location.reload(true);
		}

	}
})();
