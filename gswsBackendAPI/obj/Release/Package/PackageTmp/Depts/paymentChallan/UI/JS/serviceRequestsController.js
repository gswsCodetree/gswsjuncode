(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("serviceRequestsController", ["$scope", "cfmsPaymentServices", cfmsPayment_CTRL]);


	function cfmsPayment_CTRL(scope, ps) {

		scope.token = sessionStorage.getItem("Token");


		scope.lastChallan = function () {
			scope.lastChallanDetails = '';
			scope.loader = true;
			var requestData = {
				secId: sessionStorage.getItem("secccode")
			};
			ps.encrypt_post("lastChallanDetails", requestData, scope.token, function (data) {
				var res = data.data;
				if (res.status) {
					scope.lastChallanDetails = res.result;
				}
				scope.loader = false;
			}, function (error) {
				scope.loader = false;
				console.log(error);
			});
		};

		scope.loadServiceRequests = function () {
			scope.serviceRequestsDetails = '';
			scope.loader = true;
			var requestData = {
				secId: sessionStorage.getItem("secccode")
			};
			ps.encrypt_post("serviceRequests", requestData, scope.token, function (data) {
				var res = data.data;
				if (res.status) {
					scope.serviceRequestsDetails = res.result;

					scope.totamount = 0; scope.totservice = 0; scope.totamountval = 0;
					for (var i = 0; i < scope.serviceRequestsDetails.length; i++) {

						scope.totamount += parseInt(scope.serviceRequestsDetails[i].AMOUNT);
						scope.totservice += parseInt(scope.serviceRequestsDetails[i].SERVICE_CHARGE);
						scope.totamountval += parseInt(scope.serviceRequestsDetails[i].TOTAL_AMOUNT);
					}
					scope.lastChallan();
				}
				else {
					alert(res.result);
				}
				scope.loader = false;
			}, function (error) {
				scope.loader = false;
				console.log(error);
			});
		};

		scope.loadServiceRequests();

		scope.btnGenerateChallan = function () {
			scope.challanRespDetails = '';
			scope.loader = true;
			var requestData = {
				secId: sessionStorage.getItem("secccode")
			};
			ps.encrypt_post("generateChallan", requestData, scope.token, function (data) {
				var res = data.data;
				if (res.status) {
					scope.challanRespDetails = res.result;
				}
				scope.loader = false;
			}, function (error) {
				scope.loader = false;
				console.log(error);
			});
		};

		scope.btnPrintChallan = function () {
			var url = "";
			window.open(url, "_Blank");
		};

	}

})();