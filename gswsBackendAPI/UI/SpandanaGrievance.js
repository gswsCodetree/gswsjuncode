(function () {
	var app = angular.module("GSWS");

	app.controller("SpandanaGrievanceCTRL", ["$scope", "$state", "$log", "Login_Services", SpandanaGrievanceCTRL]);

	function SpandanaGrievanceCTRL(scope, state, log, Login_Services) {
		//alert(rs.username);
		var username = sessionStorage.getItem("user");
		var secid = sessionStorage.getItem("secccode");
		var desingation = sessionStorage.getItem("desinagtion");
		scope.username = username;
		var token = sessionStorage.getItem("Token");
		if (username == null || username == undefined || token == null || token == undefined) {

			state.go("Login");
			return;
		}
		else
			LoadDahboardDetails(sessionStorage.getItem("Spandanatype"));

		function LoadDahboardDetails(val) {

			if (val == "1") {
				scope.stitle = "Registered";
			}
			else if (val == "2") {
				scope.stitle = "Readdressed";
			}
			else {
				scope.stitle = "Pending";
			}
			Login_Services.DEMREVOAPI("GetSpandaGrievanceToken", "", function (value) {

				var res = value.data;
				if (res.StatusCode == "200" && res.Status == "Success") {
					scope.token = res.Token;
					
					var token = sessionStorage.getItem("Token");
					var input = { Statusid: val, SeccCode: sessionStorage.getItem("secccode"), token: scope.token };

					Login_Services.POSTREVENUEENCRYPTAPI("GetSpandanaSideDashboardDetailed", input, token, function (value) {
						scope.divtable = true;
						var res = value.data;
						if (res.StatusCode == "200" && res.Status == "Success") {
							scope.divtable = true;
							scope.SpandanaDashboardList = res.Data;
						}
						else {
							scope.divtable = false;
							scope.errormsg = "No Data Found";
							swal('info', res.Message, 'info');
							return;
						}
					});
				}
				else {
					scope.divtable = false;
					scope.errormsg = res.Message;
					swal('info', res.Message, 'error');
				}
			});
		}
	}
	
	

	
})();