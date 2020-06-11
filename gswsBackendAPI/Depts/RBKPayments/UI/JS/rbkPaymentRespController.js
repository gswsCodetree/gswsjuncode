(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("rbkPaymentRespController", ["$scope", "rbkPaymentsServices", rbkPayments_CTRL]);


	function rbkPayments_CTRL(scope, rs) {


		scope.token = sessionStorage.getItem("Token");
		if (scope.token == null || scope.token == undefined || scope.token == undefined) {
			window.open("http://uat.gramawardsachivalayam.ap.gov.in/GSWSUAT/#!/Login", "_Self");
			return;
		}
		scope.loader = false;
		scope.orderDetailsData = "";
		scope.orderDetails = function (encryptedString) {

			scope.orderDetailsData = "";

			scope.loader = true;
			var requestData = {
				encryptedString: encryptedString
			};
			rs.encrypt_post("recieptData", requestData, scope.token, function (data) {
				var res = data.data;
				console.log(res);
				if (res.status) {
					scope.rbkDetails = res.rbkDetails;
					scope.walletDetails = res.walletDetails;
					scope.DISTRICT_NAME = sessionStorage.getItem("distname");
					scope.SECRETARIAT_NAME = sessionStorage.getItem("secname");
					scope.username = sessionStorage.getItem("user");
					scope.MANDAL_NAME = sessionStorage.getItem("mname");
				}
				else {
					alert(res.result);
					window.open("http://uat.gramawardsachivalayam.ap.gov.in/GSWSUAT/#!/Login", "_Self");
					return;
				}
				scope.loader = false;
			}, function (error) {
				scope.loader = false;
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