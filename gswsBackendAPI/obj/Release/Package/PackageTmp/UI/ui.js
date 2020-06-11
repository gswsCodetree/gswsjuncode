(function () {
	var app = angular.module("GSWS");

	app.controller("UIController", ["$scope", "$state","Login_Services", Main_CTRL]);
	app.controller("UITController", ["$scope", "$state", MainT_CTRL]);

	function Main_CTRL(scope, state, Login_Services) {
scope.loginrole = sessionStorage.getItem("desinagtion");
                   var localurl = window.location.href.split("#!/")[1]
	if (!localurl.includes("ForgotPassword")) {
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
		} 

	   scope.role = sessionStorage.getItem("desinagtion");
		CheckroleAccess();
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

		function CheckroleAccess() {
			var token = sessionStorage.getItem("Token");
			var user = sessionStorage.getItem("user");
			var role = sessionStorage.getItem("desinagtion");
			var localurl = window.location.href.split("#!/")[1];
			if (!token || !user || !role) {
			}
			else if (localurl == "InternalURL" || localurl == "RationVolunteerMapping" || localurl == "DistrictLogin" || localurl == "PhysicalForms" || localurl == "SPandanaGrievance" || localurl == "InchargeChange"  || localurl == "PensionVolunteerMapping" || localurl == "ProfileUpdate" || localurl == "UpdateURL" || localurl == "ReportIssue" || localurl == "HardwareIssues" || localurl == "TransResponse" || localurl == "ReceivedData" || localurl == "ApplicationStatus") {

			}
			else {
				var req = { SOURCE: localurl, id: role }

				Login_Services.POSTENCRYPTAPI("GetRoleAccess", req, token, function (data) {

					if (data.data == true) {
					}
					else {
						localStorage.removeItem('logout-event');
						sessionStorage.clear();
						sessionStorage.removeItem("Token");
						sessionStorage.removeItem("user");
						//state.go("Login");
						state.go("Login");
					}

				})
			}
		}
		scope.PrintReceipt = function () {

		//	scope.divbutton = true;
			var divprint = document.getElementById("printdiv").innerHTML;

			var popupWinindow = window.open('', 'Print-Window');
			popupWinindow.document.open();
			popupWinindow.document.write('<html><head><link href="assets/css/bootstrap.css" rel="stylesheet" type="text/css"><link rel="stylesheet" type="text/css" href="assets/css/printcss.css" /></head><body onload="window.print()">' + divprint + '</html>');
			popupWinindow.document.close();
		}
	}
	function MainT_CTRL(scope, state) {
		scope.GetLogout = function () {
			localStorage.removeItem('logout-event');
			sessionStorage.clear();
			sessionStorage.removeItem("Token");
			sessionStorage.removeItem("user");
			state.go("Login");
		//	state.go("Login");
		};
	}



})();