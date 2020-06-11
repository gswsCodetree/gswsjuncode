(function () {
	var app = angular.module("GSWS");

	app.controller("ForgotPasswordController", ["$scope", "$state", "$log", "Login_Services", ForgotPasswordController]);

	function ForgotPasswordController(scope, state, log, Login_Services) {
		scope.div = "1";

		var strongRegex = new RegExp("^(?=.*[a-zA-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})");

		//Update Password
		scope.update = function () {
			if ((!scope.newpassword)) {
				swal("", "Please enter New Password", "error");
				return;
			}
			else if ((!scope.confirmpassword)) {
				swal("", "Please Re-enter Password", "error");
				return;
			}
			else if (scope.newpassword != scope.confirmpassword) {
				swal("", "New Password and Confirm Password should be same", "error");
				return;
			}
			else if (!(strongRegex.test(scope.newpassword))) {
				swal("", "Password Should Contain atleast 1 digit,1 Uppercase,1 Lowercase and 1 special character", "error");
				return;
			}
			else {
				scope.preloader = true;
				var req = {
					UNIQUEID: scope.UniqueID,
					PASSWORD: scope.newpassword,
					TYPE:"2"
				}

				Login_Services.POSTAPI("ForgotpasswordUpdate", req, function (data) {
					scope.preloader = false;
					var res = data.data;
					if (res.Status == '100') {
						swal({
							title: "Good job!",
							text: "Password Updated Successfully",
							icon: "success"
						})
							.then((value) => {
								if (value) {
									window.location.href = "../GSWS/Home/Main";
								}
							});
					}
					else {
						swal("", res.Reason, "error");
					}
				});
			}
		}

		//Send OTP
		scope.sendotp = function () {
			if (!scope.enteredusername) {
				swal("", "Please Enter Your Username", "error");
				return;
			}
			else {
				scope.preloader = true;
				var req = {
					Ftype: "3",
					FUsername: scope.enteredusername
				}
	     sessionStorage.setItem("hskey", "admin");
				Login_Services.POSTAPI("ForgotpasswordsendOTP", req, function (data) {
					scope.preloader = false;
					var res = data.data;
					if (res.Status == '100') {
						scope.UniqueID = res.DataList[0]["UNIQUE_ID"];
						scope.div = "2";
						swal("", res.Reason, "success");
					}
					else {
						swal("", res.Reason, "success");
					}
				});
			}
		}

		//Verify OTP
		scope.verifyotp = function () {
			if (!scope.enteredusername) {
				swal("", "Please Enter Your Username", "error");
				return;
			}
			else if (!scope.entredotp) {
				swal("", "Please Enter OTP Sent to Your Mobile Number", "error");
				return;
			}
			else if (scope.entredotp.length<6) {
				swal("", "Please Enter Valid OTP Sent to Registered Mobile Number", "error");
				return;
			}
			else {
				scope.preloader = true;
				var req = {
					UNIQUEID: scope.UniqueID,
					OTP: scope.entredotp,
					TYPE: "0",
					STATUS: "1"
				}

				Login_Services.POSTAPI("ForgotpasswordVerifyOTP", req, function (data) {
					scope.preloader = false;
					var res = data.data;
					if (res.Status == '100') {
						scope.div = "3";
						swal("", "OTP Verified Successfully", "success");
					}
					else {
						swal("", res.Reason, "success");
					}
				});
			}
		}

		//Password Validation
		scope.changenewpwd = function () {
			if (!(strongRegex.test(scope.newpassword))) {
				scope.divpwdstrength = true;
			}
			else {
				scope.divpwdstrength = false;
			}
		}

		//Password and Confirm Password Match
		scope.changecngpwd = function () {
			if (scope.newpassword == scope.confirmpassword) {
				scope.divpwdmatching = false;
			}
			else {
				scope.divpwdmatching = true;
			}
		}

		//Check Session Expire 
		function checksessionexpire() {
			scope.preloader = false;
			if (!(scope.token) || !(scope.EmpID)) {
				swal({
					title: "OOPS!",
					text: "Session expired..!",
					icon: "error"
				})
					.then((value) => {
						if (value) {
							state.go("Login");
						}
					});
			}
			else {

			}
		}

	}
})();