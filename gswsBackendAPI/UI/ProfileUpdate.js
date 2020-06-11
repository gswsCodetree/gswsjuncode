(function () {
	var app = angular.module("GSWS");

	app.controller("ProfileUpdateController", ["$scope", "$state", "$log", "Login_Services", ProfileUpdateController]);

	function ProfileUpdateController(scope, state, log, Login_Services) {
		scope.preloader = true;

		scope.Districtname = sessionStorage.getItem("distname");
		scope.Mandalname = sessionStorage.getItem("mname");
		scope.Secratariatname = sessionStorage.getItem("secname");

		scope.DesignationName = sessionStorage.getItem("desinagtionname");
		scope.EmpName = sessionStorage.getItem("username");
		scope.EmpID = sessionStorage.getItem("user");
		scope.UniqueID = sessionStorage.getItem("uniqueid");

		scope.MobileNo = sessionStorage.getItem("usermobileno");
		if (!scope.MobileNo || scope.MobileNo=="null") {
			scope.mobilestatus = "1";
		}
		else {
			scope.mobilestatus = "0";
		}
		scope.MailID = sessionStorage.getItem("usermailid");
		scope.PWDUpdatedStatus = sessionStorage.getItem("pwdupdateddtatus");
		scope.token = sessionStorage.getItem("Token");

		scope.diveditmail = "";
		scope.divpwdmatching = false;
		scope.preloader = false;
		Checksessionexpire();

		var strongRegex = new RegExp("^(?=.*[a-zA-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})");
		var emailRegex = new RegExp("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");

		//Save Data
		scope.save = function () {
			if (scope.PWDUpdatedStatus == 0 && (!scope.newpassword)) {
				swal("", "Please enter New Password", "error");
				return;
			}
			else if (scope.PWDUpdatedStatus == 0 && (!scope.confirmpassword)) {
				swal("", "Please enter Confirm Password", "error");
				return;
			}
			else if (scope.newpassword != scope.confirmpassword) {
				swal("", "New Password and Confirm Password should be same", "error");
				return;
			}
			else if (scope.mobilestatus == "1") {
				swal("", "Please Validate Your Mobile Number and Update", "error");
				return;
			}
			else {
				scope.preloader = true;
				var req = {
					UNIQUEID: scope.UniqueID,
					PASSWORD: scope.newpassword,
					MOBILE: scope.MobileNo,
					EMAILID: scope.MailID,
					TYPE:"1"
				}

				var token = sessionStorage.getItem("Token");
				if (!(token) || !(scope.EmpID)) {
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

				Login_Services.POSTENCRYPTAPI("ProfileUpdate", req, sessionStorage.getItem("Token"), function (data) {
					scope.preloader = false;
					var res = data.data;
					if (res.Status == '100') {
						swal({
							title: "Good job!",
							text: "Your Password Updated Successfully",
							icon: "success"
						})

							.then((value) => {
								if (value) {
									state.go("ue.Dashboard");
								}
							});
					}
					else {
						swal("", res.Reason, "success");
					}
				});
			}
		}

		//Model Show
		scope.showmodel = function (e) {
			scope.diveditmail = e;
			if (e == 0) {
				scope.modelMobileNo = "";
				scope.enteredmobileOTP = "";
			}
			else if (e == 1) {
				scope.modelMailID = "";
				scope.enteredmailOTP = "";
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

		//Password Validation
		scope.changenewpwd = function () {
			if (!(strongRegex.test(scope.newpassword))) {
				scope.divpwdstrength = true;
			}
			else {
				scope.divpwdstrength = false;
			}
		}

		//Mail Validation
		scope.changemailid = function () {
			if (!(emailRegex.test(scope.modelMailID))) {
				scope.divmailvalidation = true;
			}
			else {
				scope.divmailvalidation = false;
			}
		}

		//Send OTP
		scope.sendotpformodel = function (e) {
			if (e == 0 && !scope.modelMobileNo) {
				swal("", "Please Enter Mobile Number", "error");
				return;
			}
			else if (e == 0 && scope.modelMobileNo.length != 10) {
				swal("", "Please Enter 10 Digit Mobile Number", "error");
				return;
			}
			else if (e == 0 && scope.modelMobileNo == "1111111111" || scope.modelMobileNo == "2222222222" || scope.modelMobileNo == "3333333333" || scope.modelMobileNo == "4444444444" || scope.modelMobileNo == "5555555555" || scope.modelMobileNo == "6666666666"
				|| scope.modelMobileNo == "7777777777" || scope.modelMobileNo == "8888888888" || scope.modelMobileNo == "9999999999" || scope.modelMobileNo == "0000000000") {
				swal("", "Please Enter valid 10 Digit Mobile Number", "error");
				return;
			}
			else if (e == 1 && !scope.modelMailID) {
				swal("", "Please Enter Mail ID", "error");
				return;
			}
			else if (e == 1 && !(emailRegex.test(scope.modelMailID))) {
				swal("", "Please Enter Valid Mail ID", "error");
				return;
			}
			else {
				scope.preloader = true;
				var req = {
					UNIQUEID: scope.UniqueID,
					TYPE: e,
					MOBILE: scope.modelMobileNo,
					EMAILID: scope.modelMailID
				}

				Login_Services.POSTENCRYPTAPI("ProfileSendOTP", req, sessionStorage.getItem("Token"), function (data) {
					scope.preloader = false;
					var res = data.data;
					if (res.Status == '100') {
						swal("", "OTP Sent Successfully", "success");
					}
					else {
						swal("", res.Reason, "success");
					}
				});
			}
		}

		//Verify OTP
		scope.verifyotpformodel = function (e) {
			if (e == 1) {
				scope.OTP = scope.enteredmailOTP;
			}
			else if (e == 0) {
				scope.OTP = scope.enteredmobileOTP;
			}
			if (e == 0 && !scope.modelMobileNo) {
				swal("", "Please Enter Mobile Number", "error");
				return;
			}
			else if (e == 0 && scope.modelMobileNo.length != 10) {
				swal("", "Please Enter 10 Digit Mobile Number", "error");
				return;
			}
			else if (e == 0 && scope.modelMobileNo == "1111111111" || scope.modelMobileNo == "2222222222" || scope.modelMobileNo == "3333333333" || scope.modelMobileNo == "4444444444" || scope.modelMobileNo == "5555555555" || scope.modelMobileNo == "6666666666"
				|| scope.modelMobileNo == "7777777777" || scope.modelMobileNo == "8888888888" || scope.modelMobileNo == "9999999999" || scope.modelMobileNo == "0000000000") {
				swal("", "Please Enter valid 10 Digit Mobile Number", "error");
				return;
			}
			else if (e == 0 && !scope.enteredmobileOTP) {
				swal("", "Please Enter OTP sent to your Mobile NUmber", "error");
				return;
			}
			else if (e == 0 && scope.enteredmobileOTP.length < 5) {
				swal("", "Please Enter OTP sent to your Mobile Number", "error");
				return;
			}
			else if (e == 1 && !scope.modelMailID) {
				swal("", "Please Enter Mail ID", "error");
				return;
			}
			else if (e == 1 && !(emailRegex.test(scope.modelMailID))) {
				swal("", "Please Enter Valid Mail ID", "error");
				return;
			}
			else if (e == 1 && !scope.enteredmailOTP) {
				swal("", "Please Enter OTP sent to your Mail", "error");
				return;
			}
			else {
				scope.preloader = true;
				var req = {
					UNIQUEID: scope.UniqueID,
					TYPE: e,
					OTP: scope.OTP,
					STATUS: "1"
				}

				Login_Services.POSTENCRYPTAPI("ProfileVerifyOTP", req, sessionStorage.getItem("Token"), function (data) {
					scope.preloader = false;
					var res = data.data;
					if (res.Status == '100') {
						scope.diveditmail = "";
						$("#verification").modal('hide');
						swal("", "OTP Verified Successfully", "success");
						if (e == 0) {
							scope.MobileNo = scope.modelMobileNo;
							scope.mobilestatus = "0";
						}
						else if (e == 1) {
							scope.MailID = scope.modelMailID;
						}
					}
					else {
						swal("", res.Reason, "success");
					}
				});
			}
		}

		//Check Session Expire
		function Checksessionexpire() {
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
		}

	}
})();