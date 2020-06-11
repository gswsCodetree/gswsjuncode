(function () {
	var app = angular.module("GSWS");

	app.controller("PublicLoginController", ["$scope", "$state", "$log", "Login_Services", PublicLoginController]);

	function PublicLoginController(scope, state, log, Login_Services) {
		scope.div = "1";

		//Send OTP
		scope.sendotp = function () {
			if (!(scope.enteredaadhaarno)) {
				swal("",'Please enter Aadhaar Number',"");
				return;
			}
			else if (scope.enteredaadhaarno.length != "12") {
				swal("",'Please enter 12 digit Aadhaar Number',"");
				return;
			}
			else if (scope.enteredaadhaarno == "111111111111" || scope.enteredaadhaarno == "222222222222" || scope.enteredaadhaarno == "333333333333" || scope.enteredaadhaarno == "444444444444" || scope.enteredaadhaarno == "555555555555" || scope.enteredaadhaarno == "666666666666"
				|| scope.enteredaadhaarno == "777777777777" || scope.enteredaadhaarno == "888888888888" || scope.enteredaadhaarno == "999999999999" || scope.enteredaadhaarno == "000000000000") {
				swal("","Please Enter 12 Digit Aadhaar Number","");
				return;
			}
			var status=Validateaadhaar();
			if (status == "Failure") {
				swal("", "Please Enter Valid Aadhaar Number", "");
				return;
			}

			scope.preloader = true;
			var req = {
				AADHAR: scope.enteredaadhaarno,
			}
			Login_Services.POSTAPI("AadhaarLogin", req, function (data) {
				scope.preloader = false;
				var res = data.data;
				if (res.Status == '100') {
					scope.div = "2";
					swal("","OTP sent to your Mobile Number","");
				}
				else {
					swal("", res.Reason, "error");
				}
			});

		}

		//Verify OTP
		scope.verifyotp = function () {
			if (!(scope.enteredaadhaarno)) {
				swal("", 'Please enter Aadhaar Number', "");
				return;
			}
			else if (!(scope.entredotp)) {
				swal("", 'Please enter OTP Number', "");
				return;
			}
			else {
				scope.preloader = true;
				var req = {
					AADHAR: scope.enteredaadhaarno,
					OTP: scope.entredotp
				}
				Login_Services.POSTAPI("AadhaarVerifyOTP", req, function (data) {
					scope.preloader = false;
					var res = data.data;
					if (res.Status == '100') {
						sessionStorage.setItem("Token", res.access_token);
						sessionStorage.setItem("user", res.Name);
						console.log(res.access_token);
						console.log(res.Name);
						swal({
							title: "Good job!",
							text: "OTP Verified Successfully",
							icon: "success"
						})
							.then((value) => {
								if (value) {
									state.go("ue.Dashboard");
								}
							});
					}
					else {
						swal("", res.Reason, "error");
					}
				});
			}
		}

		//Vaildate Aadhaar
		function Validateaadhaar() {
			var status = validateVerhoeff(scope.enteredaadhaarno);
			if (status) {
				
			}
			else {
				return "Failure";
			}
		}
	}
})();