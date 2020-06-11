(function () {
	var app = angular.module("GSWS");

	app.controller("ExciseComplaintCheck_Controller", ["$scope", "$state", "$log", "REVENUE_Services", Excise_CTRL]);

	function Excise_CTRL(scope, state, log, REVENUE_Services, ) {
		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}
		scope.Preloader = false;
		scope.getdetails = function () {

			if (!(scope.txtcmplttid)) {
				alert('Please enter Complaint ID');
				return;
			}
			var req = {
				appcode: scope.txtcmplttid
			};
			scope.Preloader = true;
			REVENUE_Services.POSTREVENCRYPTAPI("CheckComplaintStatus", req, token, function (value) {
				var res = value.data;
				scope.Preloader = false;
				if (res.Status == "Success") {
					scope.detailsshow = true;
					if (!(res.Data)) {
						scope.detailsshow = false;
						scope.ResData = "";
						alert("No Data available for this complaint id");
						return;
					}
					else
						scope.ResData = res.Data;
				}
				else if (res.Status == "428") {
					swal('info', res.Reason, 'info');
					sessionStorage.clear();
					state.go("Login");
					return;
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
		};

		
	}


})();
