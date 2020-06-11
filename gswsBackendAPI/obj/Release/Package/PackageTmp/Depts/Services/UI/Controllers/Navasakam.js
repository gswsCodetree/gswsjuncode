(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("NavasakamCTRL", ["$scope", "$state", "$log", "Ser_Services", NavasakamCTRL]);

	function NavasakamCTRL(scope, state, log, pas_services) {

		var token = sessionStorage.getItem("Token");
		var username = sessionStorage.getItem("user");
		if (!token || !username) {
			sessionStorage.clear();
			state.go("Login");
			return false;
		}
		else if(username.split["-"][1]!= 'DA') {

		}
		else {
			var splitval =window.location.href.split("&")[0];
			var type = splitval.split("?Id=")[1];
			NavaSakam(type);
		}
		function NavaSakam(type) {

			var token = sessionStorage.getItem("Token");
			var username = sessionStorage.getItem("user");
			
			if (username == "brsreddy" || username == "ramsaid" || username == "chandu" || username == "user" || username == "BA5") {
				scope.nuser = "11290586-DA";
			}
			else {
				scope.nuser = username;
			}
			
			var req = { secr_user: scope.nuser };

			pas_services.POSTENCRYPTGSWSAPI("NavaSakamaData", req, token, function (value) {
				if (value.data.Status == "100") {

					var encval = window.location.href.split("enc=")[1];
					//var ivval = window.location.href.split("iv=")[1];
					window.open(value.data.Returnurl + "&service_id=" + type+"&Id="+encval);
				}
				else {
					swal('info', value.data.Reason, 'info');
				}
			});
		}
	}
})();