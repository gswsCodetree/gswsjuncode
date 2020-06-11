(function () {
	/* eslint-disable */
	var app = angular.module("GSWS");
	app.controller("EchallanController", ["$scope", "Home_Services", '$sce', '$http', EchallanController]);
	function EchallanController(scope, home_services, sce, $http) {
		var token = sessionStorage.getItem("Token");
		if (!token) {
			('info', 'Session Experied', 'info');
			sessionStorage.clear();
			state.go("Login");
			
		}

		scope.GetDetails = function () {


			if (!scope.VehicleNumber) {
				swal('info', 'Please Enter Vehicle Number', 'info');
				return;
			}

			
			var input = { VehicleNum: scope.VehicleNumber };
			home_services.POSTENCRYPTAPI("EchallanStatus", input, token, function (value) {

				var res = value.data;
				if (res.code == 200) {

					scope.EchallanList = res.result;
				}
				else if (res.Status == "428") {
					sessionStorage.clear();
					swal('info', res.Reason, 'info');
					state.go("LOGIN");
					return;
				}
				else {
					swal('info', res.Message, 'info');
					return;
				}
			});
		}

	}
})();


