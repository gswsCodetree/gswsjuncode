(function () {
	var app = angular.module("GSWS");

	app.controller("MeesevaAppStatusCTRL", ["$scope", "$state", "$log", "Meeseva_Services", MeesevaAppStatusCTRL]);

	function MeesevaAppStatusCTRL(scope, state, log, Meeseva_Services) {
		scope.preloader = false;
		var username = sessionStorage.getItem("user");
		var token = sessionStorage.getItem("Token");
		if (username == null || username == undefined || token == null || token == undefined) {

			state.go("Login");
			return;
		}


		scope.GetMeesevaStatus = function () {
			if (!scope.MeesevaAppNo) {
				swal('info', "Please Enter Meeseva Application Number", 'info');
				return false;
			}
			else {
				scope.dttable = false;
				var input = { PARAM1: scope.MeesevaAppNo }
				Meeseva_Services.DemoAPI("GetMeesevaAppStatus", input, function (value) {
					var res = value.data;
					if (res.status == "100") {
						scope.dttable = true;
						scope.APPNO = res.APPNO;
						scope.STATUS = res.STATUS;
					}
					else if (res.Status == "428") {
						sessionStorage.clear();
						swal('info', res.Reason, 'info');
						state.go("Login");
						return;

					}
					else {
						swal('info', res.remarks, 'info');
					}
				});
			}

		}


	}
})();