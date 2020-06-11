(function () {
	var app = angular.module("GSWS");

	app.controller("PublicHController", ["$scope", "$state", "Login_Services", PublicHController]);

	function PublicHController(scope, state, Login_Services) {
		scope.loginrole = sessionStorage.getItem("desinagtion");
	
			var token = sessionStorage.getItem("Token");
			var user = sessionStorage.getItem("user");
			if (!token || !user) {

				sessionStorage.clear();
				localStorage.removeItem('logout-event');
				sessionStorage.removeItem("Token");
				sessionStorage.removeItem("user");
				state.go("Login");
				//state.go("Login");
				return;
			}
		

		
		scope.GetLogout = function () {

			// scope.pagename = "Login";

			var req = { Ftype: 3, UserId: sessionStorage.getItem("user") }

			Login_Services.POSTENCRYPTAPI("GetPSLogOut", req, token, function (value) {
				localStorage.removeItem('logout-event');
				sessionStorage.clear();
				sessionStorage.removeItem("Token");
				sessionStorage.removeItem("user");
				state.go("Login");
				//state.go("Login");

			});


		};

		
		scope.PrintReceipt = function () {

			//	scope.divbutton = true;
			var divprint = document.getElementById("printdiv").innerHTML;

			var popupWinindow = window.open('', 'Print-Window');
			popupWinindow.document.open();
			popupWinindow.document.write('<html><head><link href="assets/css/bootstrap.css" rel="stylesheet" type="text/css"><link rel="stylesheet" type="text/css" href="assets/css/printcss.css" /></head><body onload="window.print()">' + divprint + '</html>');
			popupWinindow.document.close();
		}
	}
	



})();