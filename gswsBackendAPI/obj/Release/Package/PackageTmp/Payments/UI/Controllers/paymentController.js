(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("paymentController", ["$scope", "$state", "$interval", "Payment_Services", "$http", payment_CTRL]);

	function payment_CTRL(scope, state, interval, Payment_Services, http) {


		scope.paymentProcess = false;
		scope.countDown = 120;
		scope.intervalStopper = false;
		interval(function () {
			if (!scope.intervalStopper) {
				scope.countDown = scope.countDown - 1;
				if (scope.countDown < 0) {
					scope.intervalStopper = true;
					alert("session Expired !!!");
					//sessionStorage.clear();
					//localStorage.clear();
					state.go("Login");
					return;
				}
			}
		}, 1000);

		//var requset = {
		//	ipAddress: "192.168.1.1",
		//	mobileNumber: "9494331344",
		//	orderId: "TXN10100101",
		//	TxnDate: "01-20-2019 09:00 AM",
		//	Amount: "10",
		//	Description: "Text Payment",
		//	gswsCode: "10690581",
		//	applicantName: "TEST NAME",
		//	deptCode: "2010",
		//	deptRecieptCode: "TXN45225125",
		//	districtCode: "01",
		//	serviceCode: "1",
		//	staffCode: "DOPR001",
		//	userCharges: "5",
		//	totalAmount: "15",
		//	userName: "user",
		//	password: "user123"
		//};

		//Payment_Services.DemoAPI("getEncrypt", requset, function (value) {
		//	console.log(value);
		//});

		scope.wallet_type = "";
		var encrypted_data = "";
		var iv = "";

		scope.getDetails = function (data, key) {

			//var baseurl = "/api/paymentsGateWay/";
			//var url = baseurl + "OrderDetails?encryptedData=" + data + "&iv=" + key;
			//http({
			//	method: 'GET',
			//	url: url
			//}).then(function successCallback(response) {
			//	console.log(response);
			//}, function errorCallback(response) {
			//		console.log(response);
			//});
			//return;

			var encReq = {
				encryptedData: data,
				iv: key
			};

			Payment_Services.DemoAPI("Order", encReq, function (value) {

				if (value.data.status == 200) {

					scope.payment_details = JSON.parse(value.data.result);
					scope.wallet_type = scope.payment_details.walletType;
					scope.encrypttext = scope.payment_details.encrypttext;
					scope.iv = scope.payment_details.iv;
					if (scope.wallet_type == "") {
						scope.intervalStopper = true;
						alert("Invalid Gateway !!!");
						sessionStorage.clear();
						localStorage.clear();
						state.go("Login");
						return;
					}


					sessionStorage.setItem("Token", value.data.access_token);

					if (scope.wallet_type == "APW") {
						window.open("http://www.ap-wallet.org?tid=" + scope.payment_details.orderId + "&amt=" + scope.payment_details.totalAmount + "", "_Self");
						return;
					}
				}
				else {
					scope.intervalStopper = true;
					alert(value.data.result);
					sessionStorage.clear();
					localStorage.clear();
					state.go("Login");
				}
			}, function (error) {
				console.log(error);
			});

		};

		var meesevaReq_data = getParameterByName('meeSevaReq');
		if (meesevaReq_data == "" || meesevaReq_data == null || meesevaReq_data == undefined) {
			encrypted_data = "EapTNVfKRH2E9nx9sVEfA/u/boSbOFwgEW8BzzjgcdGJDYrH7oSVq74bXbpwES7QVE3zp+WIabfR97RZaxKfpHzhhLVi9h+Du6vIlufvxLgygCOZyZpE/MG7t3bF6DUvXlO3C48c7Vxx02cle7AlRh2IHMtT0qwzv1q5zin4v+Nv+PbwcjZdPdLyhDPA4qsNlQ63HWxgpiri0WPHbZxPC7inuHh7gwy4wcLtjLzlf6cpwKG2wjsWLmUUEe62cW0eIS115octsTPODzqP6gdKLdOBx5rxvFSnIb/YDE8IWnknJYPvBGpBUbuXqOqBIQVbgEE3QHeb8E/sjrKD+jU4aw2NhhYwXefQ0EM6L0YIRPgI6kQMs4E/dtxVh3L0rpX59hC6sO4roNq4LSVre/rVOFhE0xUDwrG72z2Nqj2QQ+m4klYM05iIsrby5QTqnKQhXkSeW+qaRlS8lETv/2fTCM6uWbU+wIGaIHkg+argHOMa1wBZfbDs6JxusxIUSOfXCKJfDIPyuKmqHeTLmv5WkQ==";
			iv = "9oC-7rt0aXChqDTR";
			scope.getDetails(encrypted_data, iv);
			//state.go("Login");
			//return;
		}
		else {

			var values = meesevaReq_data.split('^');
			//encrypted_data = decodeURIComponent(values[0]);


			temp = values[0].split(' ');
			encrypted_data = '';
			for (var i = 0; i < temp.length; i++) {
				if (i > 0) {
					encrypted_data += "+";
				}
				encrypted_data += temp[i];
			}
			console.log(encrypted_data);
			iv = decodeURIComponent(values[1]);
			scope.getDetails(encrypted_data, iv);

		}

		scope.btnCancelPayment = function () {
			scope.intervalStopper = true;
			alert("Payment Cancelled");
			sessionStorage.clear();
			localStorage.clear();
			state.go("Login");
		};
		scope.btnFailedPayment = function () {
			scope.intervalStopper = true;
			sessionStorage.clear();
			localStorage.clear();
			state.go("Login");
		};

		scope.btnPayment = function () {
			if (scope.wallet_type == "TA") {
				if (scope.taOtp == '' || scope.taOtp == null || scope.taOtp == undefined) {
					Swal.fire('Success', "Please Enter OTP", 'info');
					return;
				}
			}

			var req = {
				encryptedData: scope.encrypttext,
				iv: scope.iv,
				type: scope.wallet_type,
				otp: scope.taOtp
			};

			Payment_Services.DemoAPI("walletpay	", req, function (value) {
				console.log(value);
				scope.paymentProcess = true;
				if (scope.wallet_type == "TA") {
					scope.payment_response_remarks = value.data.reason;
					if (value.data.status == 200) {
						alert(value.data.result);
					}
					else {
						alert(value.data.result);
					}

					var form = $('<form action="' + value.data.callbackUrl + '" method="post">' +
						'<input type="hidden" id="deptOrderId" name="MEESEVA_APPNO" value="' + value.data.deptOrderId + '" />' +
						'<input type="hidden" id="txnId" name="APTS_TXNID" value="' + value.data.txnId + '" />' +
						'<input type="hidden" id="reason" name="STATUS_DESC" value="' + value.data.reason + '" />' +
						'<input type="hidden" id="status" name="STATUS_ID" value="' + value.data.status + '" />' +
						'<input type="hidden" id="merchantId" name="MERCHANTID" value="' + value.data.merchantId + '" />' +
						'</form>');
					$('body').append(form);
					form.submit();
				}
				else if (scope.wallet_type == "WONE") {
					scope.paymentProcess = true;
					scope.payment_response_remarks = value.data.message + ' ( TXN NO : ' + value.data.transaction_no + ' )';
					if (value.data.status == 200) {
						alert(value.data.message);
					}
					else {
						alert(value.data.message);
					}

					var formq = $('<form action="' + value.data.callbackUrl + '" method="post">' +
						'<input type="hidden" id="deptOrderId" name="MEESEVA_APPNO" value="' + value.data.deptOrderId + '" />' +
						'<input type="hidden" id="txnId" name="APTS_TXNID" value="' + value.data.txnId + '" />' +
						'<input type="hidden" id="reason" name="STATUS_DESC" value="' + value.data.message + '" />' +
						'<input type="hidden" id="status" name="STATUS_ID" value="' + value.data.status + '" />' +
						'<input type="hidden" id="merchantId" name="MERCHANTID" value="' + value.data.merchantId + '" />' +
						'</form>');
					$('body').append(formq);
					formq.submit();
				}

			});

		};


		//scope.today_date = Date.now();
		//scope.order_id = Date.now();
		//scope.amount = '10';
		//scope.service_name = 'EMCET';
		//scope.transaction_desc = 'EMCET Fee Payment';

		//var response_data = getParameterByName('TAWalletData');
		//if (response_data != null && response_data != undefined && response_data != '') {
		//	var req = {
		//		encrypted_data: response_data,
		//		user_id: '12345'
		//	};
		//	Payment_Services.DemoAPI("TAresponseDecrypt", req, function (value) {

		//		console.log(value);
		//		if (value.data.status == 200) {
		//			Swal.fire('Success', value.data.result, 'info');
		//			return;
		//		}
		//		else {
		//			Swal.fire('info', value.data.result, 'info');
		//			return;
		//		}
		//	});
		//}


		//scope.printThis = function () {
		//	$('#example').printThis({ importCSS: true, importStyle: true, printContainer: true });
		//};

		//scope.btnTaWallet = function () {

		//	var req = {
		//		order_id: Date.now(),
		//		amount: '10',
		//		callback_url: 'http://localhost:3831/#!/payment',
		//		service_name: 'EMCET',
		//		transaction_desc: 'EMCET Fee Payment',
		//		user_id: '12345'
		//	};

		//	Payment_Services.DemoAPI("TAWalletEncryptedKey", req, function (value) {

		//		console.log(value);

		//		if (value.data.status == 200) {
		//			var form = $('<form action="' + value.data.url + '" method="post">' +
		//				'<input type="hidden" id="encData" name="Data" value=' + value.data.result + ' />' +
		//				'<input type="hidden" id="encSKey" name="Skey" value=' + value.data.sec_key + ' />' +
		//				'</form>');
		//			$('body').append(form);
		//			form.submit();
		//		}
		//		else {
		//			Swal.fire('info', value.data.result, 'info');
		//			return;
		//		}

		//	});


	};

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