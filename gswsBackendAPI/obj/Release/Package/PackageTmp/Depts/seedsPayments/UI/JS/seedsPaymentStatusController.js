(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("seedsPaymentStatusController", ["$scope", "seedsPaymentsServices", seedsPayments_CTRL]);


	function seedsPayments_CTRL(scope, rs) {


		scope.token = sessionStorage.getItem("Token");
		if (scope.token == null || scope.token == undefined || scope.token == undefined) {
			window.open("http://uat.gramawardsachivalayam.ap.gov.in/GSWSUAT/#!/Login", "_Self");
			return;
		}
		scope.loader = false;
		scope.orderDetailsData = "";
		scope.productDetails = "";
		scope.orderDetails = function () {

			scope.orderDetailsData = "";
			if (scope.orderId == null || scope.orderId == undefined || scope.orderId == undefined) {
				alert("Please Enter Order ID");
				return;
			}

			scope.loader = true;
			var requestData = {
				uniqueId: sessionStorage.getItem("uniqueid"),
				orderId: scope.orderId
			};
			rs.encrypt_post("OrderDetails", requestData, scope.token, function (data) {
				var res = data.data;
				if (res.status) {
					scope.orderDetailsData = res.result;
					scope.orderList = res.DataList;
					scope.priflag = res.Pflag;
					scope.DISTRICT_NAME = sessionStorage.getItem("distname");
					scope.SECRETARIAT_NAME = sessionStorage.getItem("secname");
					scope.username = sessionStorage.getItem("user");
					scope.MANDAL_NAME = sessionStorage.getItem("mname");
					scope.paymentResponse = res.DataList[0];
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

		scope.PrintReceipt = function () {
			var divprint = document.getElementById("printdiv").innerHTML;

			var popupWinindow = window.open('', 'Print-Window');
			popupWinindow.document.open();
			popupWinindow.document.write('<html><head><link href="assets/css/bootstrap.css" rel="stylesheet" type="text/css"><link rel="stylesheet" type="text/css" href="assets/css/printcss.css" /></head><body onload="window.print()">' + divprint + '</html>');
			popupWinindow.document.close();
			return;
		};
	}

})();