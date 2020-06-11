(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("rbkPaymentsController", ["$scope", "rbkPaymentsServices", rbkPayments_CTRL]);


	function rbkPayments_CTRL(scope, rs) {


		scope.token = sessionStorage.getItem("Token");
		if (scope.token == null || scope.token == undefined || scope.token == undefined) {
			window.open("http://uat.gramawardsachivalayam.ap.gov.in/GSWSUAT/#!/Login", "_Self");
			return;
		}
		scope.loader = false;
		scope.orderDetailsData = "";

		scope.encrypted_data = getParameterByName('Id');
		scope.iv = getParameterByName('IV');
		
		if (scope.encrypted_data != "" && scope.encrypted_data != null && scope.encrypted_data != undefined || scope.iv != "" && scope.iv != null && scope.iv != undefined) {
			console.log(scope.encrypted_data, scope.iv);
		}
		else {
			alert("Invalid Request !!!");
			window.open("http://uat.gramawardsachivalayam.ap.gov.in/GSWSUAT/#!/Login", "_Self");
			return;
		}

		scope.orderDetails = function () {
			scope.orderflag = true;
			scope.orderDetailsData = "";
			if (scope.orderId == null || scope.orderId == undefined || scope.orderId == undefined) {
				alert("Please Enter Order ID");
				return;
			}

			scope.loader = true;
			var requestData = {
				encrypted_data: scope.encrypted_data,
				iv: scope.iv,
				uniqueId: sessionStorage.getItem("uniqueid"),
				orderId: scope.orderId,
				desgId: sessionStorage.getItem("desinagtion"),
				districtId: sessionStorage.getItem("distcode"),
				gpWardId: sessionStorage.getItem("gpcode") == "undefined" ? "" : sessionStorage.getItem("gpcode"),
				loginuser: sessionStorage.getItem("user"),
				mandalId: sessionStorage.getItem("mcode"),
				secId: sessionStorage.getItem("secccode"),
				serviceId: ""
			};
			rs.encrypt_post("paymentOrderDetails", requestData, scope.token, function (data) {
				var res = data.data;
				if (res.status) {
					scope.orderDetailsData = res.result;
				}
				else {
					scope.orderflag = false;
					alert(res.result);
				}
				scope.loader = false;
			}, function (error) {
				scope.orderflag = false;
				scope.loader = false;
				console.log(error);
			});
		};


		scope.makePayment = function () {
			var req = {
				GSWSCODE: sessionStorage.getItem("secccode"),
				OPERATORID: sessionStorage.getItem("user"),
				APPLICATIONNO: scope.orderId,
				CONSUMERNAME: scope.orderDetailsData.Name,
				serviceAmt: scope.orderDetailsData.Amount,
				GSWSREFNO: scope.orderDetailsData.gswsTxnId,
				rbkName: scope.orderDetailsData.RbkName,
				hubName: scope.orderDetailsData.HubName
			};

			rs.encrypt_post("makePayment", req, scope.token, function (data) {
				var res = data.data;
				if (res.status) {
					//scope.url = res.result;
					//window.open(scope.url, "_Self");
					//console.log(scope.url);
					scope.orderflag = false;
					scope.rbkDetails = res.rbkDetails;
					scope.walletDetails = res.walletDetails;
					scope.DISTRICT_NAME = sessionStorage.getItem("distname");
					scope.SECRETARIAT_NAME = sessionStorage.getItem("secname");
					scope.username = sessionStorage.getItem("user");
					scope.MANDAL_NAME = sessionStorage.getItem("mname");
					scope.orderDetails = res.orderDetails;
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

			//	scope.divbutton = true;
			var divprint = document.getElementById("printdiv").innerHTML;

			var popupWinindow = window.open('', 'Print-Window');
			popupWinindow.document.open();
			popupWinindow.document.write('<html><head><link href="assets/css/bootstrap.css" rel="stylesheet" type="text/css"><link rel="stylesheet" type="text/css" href="assets/css/printcss.css" /></head><body onload="window.print()">' + divprint + '</html>');
			popupWinindow.document.close();
		}
	}
	


	function getParameterByName(name, url) {
		if (!url) url = window.location.href;
		name = name.replace(/[\[\]]/g, '\\$&');
		var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
			results = regex.exec(url);
		if (!results) return null;
		if (!results[2]) return '';
		return decodeURIComponent(results[2].replace(/\+/g, ' '));
	}


	app.directive('numbersOnly', function () {
		return {
			require: 'ngModel',
			restrict: 'A',
			link: function (scope, element, attr, ctrl) {
				function inputValue(val) {
					if (val) {
						var digits = val.replace(/[^0-9]/g, '');
						if (digits !== val) {
							ctrl.$setViewValue(digits);
							ctrl.$render();
						}
						return digits;//ParseInt(digits,10);
					}
					return undefined;
				}
				ctrl.$parsers.push(inputValue);
			}
		};
	});

})();