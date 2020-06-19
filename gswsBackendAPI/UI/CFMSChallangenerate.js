(function () {
	var app = angular.module("GSWS");

	app.controller("CFMSChallanGenerateCTRL", ["$scope", "$state", "$log", "Login_Services", CFMSChallanGenerateCTRL]);

	function CFMSChallanGenerateCTRL(scope, state, log, Login_Services) {
		//alert(rs.username);
		var username = sessionStorage.getItem("user");
		var secid = sessionStorage.getItem("secccode");
		var desingation = sessionStorage.getItem("desinagtion");

		var token = sessionStorage.getItem("Token");
		if (!username || !token) {

			state.go("Login");
			return;
		}
		else
			LoadServiceData();

		function LoadServiceData() {			
			
			var input = { Ftype: 4, SecretartaitCode: sessionStorage.getItem("secccode") }
			Login_Services.POSTENCRYPTAPI("GetCFMSPaymentService", input, token, function (value) {
				var res = value.data;
				if (res.Status == "100") {
					scope.PaymentDataList = res.DataList;
					scope.tottrans = 0; scope.totamount = 0;
					for (var i = 0; i < scope.PaymentDataList.length; i++) {
						scope.tottrans += parseInt(scope.PaymentDataList[i].NO_OF_TXNS);
						scope.totamount += parseInt(scope.PaymentDataList[i].TOTAL_AMOUNT);
					}
				}
				else if (res.Status == "428") {
					sessionStorage.clear();
					swal('info', res.Reason, 'info');
					state.go("Login");
					return;

				}
				else {
					swal('info', res.Reason, 'info');
				}
			});
		}

		scope.BtnGenerateChallan = function () {

			var input = { Ftype: 5, SecretartaitCode: sessionStorage.getItem("secccode") }
			Login_Services.POSTENCRYPTAPI("GetCFMSPaymentGeneration", input, token, function (value) {
				var res = value.data;
				if (res.Status == "100") {
					swal('info', res.Reason, 'success');
					window.open(res.Returnurl);
				}
				else if (res.Status == "428") {
					sessionStorage.clear();
					swal('info', res.Reason, 'info');
					state.go("Login");
					return;

				}
				else {
					swal('info', res.Reason, 'info');
				}
			});
		}
	}
})();