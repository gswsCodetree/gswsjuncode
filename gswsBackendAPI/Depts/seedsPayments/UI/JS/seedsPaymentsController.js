(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("seedsPaymentsController", ["$scope", "seedsPaymentsServices", rbkPayments_CTRL]);


	function rbkPayments_CTRL(scope, rs) {


		scope.token = sessionStorage.getItem("Token");
		if (scope.token == null || scope.token == undefined || scope.token == undefined) {
			window.open("https://gramawardsachivalayam.ap.gov.in/GSWS/#!/Login", "_Self");
			return;
		}
		scope.paymentFailed = "0";
		scope.orderFlag = "0";
		scope.loader = false;
		scope.orderDetailsData = "";
		scope.productDetails = "";
		scope.encrypted_data = getParameterByName('Id');
		scope.iv = getParameterByName('IV');

		if (scope.encrypted_data != "" && scope.encrypted_data != null && scope.encrypted_data != undefined || scope.iv != "" && scope.iv != null && scope.iv != undefined) {
			console.log(scope.encrypted_data, scope.iv);
		}
		else {
			alert("Invalid Request !!!");
			window.open("https://gramawardsachivalayam.ap.gov.in/GSWS/#!/Login", "_Self");
			return;
		}

		scope.orderDetails = function () {

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
					console.log(scope.orderDetailsData);
					if (sessionStorage.getItem("secccode") == scope.orderDetailsData.vscode) {
						scope.orderFlag = "1";
					} else {
						scope.orderFlag = "3";
					}
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


		scope.makePayment = function () {
			var req = {
				GSWSCODE: sessionStorage.getItem("secccode"),
				OPERATORID: sessionStorage.getItem("user"),
				APPLICATIONNO: scope.orderId,
				CONSUMERNAME: scope.orderDetailsData.farmerName,
				serviceAmt: scope.orderDetailsData.amountTobePaid,
				GSWSREFNO: scope.orderDetailsData.gswsTxnId,
				serviceCharge: scope.orderDetailsData.amountTobePaid
			};

			rs.encrypt_post("makePayment", req, scope.token, function (data) {
				var res = data.data;
				if (res.status) {
					scope.orderFlag = "2";
					scope.paymentResponse = res.result[0];
					scope.DISTRICT_NAME = sessionStorage.getItem("distname");
					scope.SECRETARIAT_NAME = sessionStorage.getItem("secname");
					scope.username = sessionStorage.getItem("user");
					scope.MANDAL_NAME = sessionStorage.getItem("mname");
					console.log(scope.paymentResponse);
				}
				else {
					scope.paymentFailed = "1";
					alert(res.result);
				}
				scope.loader = false;
			}, function (error) {
				scope.paymentFailed = "1";
				scope.loader = false;
				console.log(error);
			});
		};

		scope.PrintReceipt = function () {
			window.print();
			return;
		};

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