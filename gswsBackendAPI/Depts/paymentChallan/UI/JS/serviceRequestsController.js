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
				secId: '10790724' //sessionStorage.getItem("secccode")
			};
			ps.encrypt_post("serviceRequests", requestData, scope.token, function (data) {
				var res = data.data;
				if (res.status) {
					scope.divservice = true;
					scope.serviceRequestsDetails = res.result;

					scope.totamount = 0; scope.totservice = 0; scope.totamountval = 0;
					for (var i = 0; i < scope.serviceRequestsDetails.length; i++) {

						scope.totamount += parseInt(scope.serviceRequestsDetails[i].AMOUNT);
						scope.totservice += parseInt(scope.serviceRequestsDetails[i].SERVICE_CHARGE);
						scope.totamountval += parseInt(scope.serviceRequestsDetails[i].TOTAL_AMOUNT);
					}
					//scope.lastChallan();
				}
				else {
					scope.divservice = false;
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
				secId: '10790724'//sessionStorage.getItem("secccode")
			};
			ps.encrypt_post("generateChallan", requestData, scope.token, function (data) {
				var res = data.data;
				if (res.status) {
					//scope.challanRespDetails = res.result;
					swal('info', res.result, 'success');
					window.open(res.Returnurl);
				}
				else {
					swal('info', res.result, 'failure');
				}
				scope.loader = false;
			}, function (error) {
				scope.loader = false;
				console.log(error);
			});
		};

		scope.btnPrintChallan = function (id) {
			var url = "https://devcfms.apcfss.in:44300/sap/bc/ui5_ui5/sap/zfi_rcp_cstatus/index.html?sap-client=150&DeptID="+id;
			window.open(url, "_Blank");
		};

		scope.btnTransactionhistory = function () {
			scope.loader = true;
			var requestData = {
				secId: '11090984'//sessionStorage.getItem("secccode")
			};
			ps.encrypt_post("TransactioHistory", requestData, scope.token, function (data) {
				var res = data.data;
				if (res.status) {
					scope.divservice = false;
					scope.divtrans = true;
					scope.TransList = res.result;
				}
				else {
					swal('info', res.result, 'info');
					scope.divtrans = false;
					scope.divservice = false;
				}
				scope.loader = false;
			}, function (error) {
				scope.loader = false;
				console.log(error);
			});
		}
	}

})();