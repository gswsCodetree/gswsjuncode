(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("seedsPaymentRespController", ["$scope", "seedsPaymentsServices", seedsPayments_CTRL]);


	function seedsPayments_CTRL(scope, rs) {


		scope.token = sessionStorage.getItem("Token");
		if (scope.token == null || scope.token == undefined || scope.token == undefined) {
			window.open("http://uat.gramawardsachivalayam.ap.gov.in/GSWSUAT/#!/Login", "_Self");
			return;
		}
		scope.paymentFailed = "0";
		scope.loader = false;
		scope.orderDetailsData = "";
		scope.productDetails = "";
		scope.orderDetails = function (encryptedString) {

			scope.orderDetailsData = "";

			scope.loader = true;
			var requestData = {
				encryptedString: encryptedString,
				secId: sessionStorage.getItem("secccode")
			};
			rs.encrypt_post("recieptData", requestData, scope.token, function (data) {
				var res = data.data;
				console.log(res);
				if (res.status) {
					scope.orderDetails = res.orderDetails[0];
					scope.seedDetails = res.seedDetails;
					scope.walletDetails = res.walletDetails;
					scope.DISTRICT_NAME = sessionStorage.getItem("distname");
					scope.SECRETARIAT_NAME = sessionStorage.getItem("secname");
					scope.username = sessionStorage.getItem("user");
					scope.MANDAL_NAME = sessionStorage.getItem("mname");
				}
				else {
					alert(res.result);
					scope.paymentFailed = "1";
					return;
				}
				scope.loader = false;
			}, function (error) {
				scope.loader = false;
				scope.paymentFailed = "1";
				console.log(error);
			});
		};

		var meesevaReq_data = getParameterByName('rps');
		if (meesevaReq_data != "" && meesevaReq_data != null && meesevaReq_data != undefined) {
			console.log(meesevaReq_data);
			scope.orderDetails(meesevaReq_data);
		}
		else {
			alert("Invalid Request !!!");
			window.open("http://uat.gramawardsachivalayam.ap.gov.in/GSWSUAT/#!/Login", "_Self");
			return;
		}

		scope.PrintReceipt = function () {
			window.print();
			return;
		};

		scope.login = function () {
			sessionStorage.clear();
			window.open("http://uat.gramawardsachivalayam.ap.gov.in/GSWSUAT/#!/Login", "_Self");
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

})();