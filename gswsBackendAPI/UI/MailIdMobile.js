(function () {
	var app = angular.module("GSWS");

	app.controller("MailMobileController", ["$scope", "$state", "$log", "Login_Services", MailMobileController]);

	function MailMobileController(scope, state, log, Login_Services) {
		
		scope.FMailId = '';
		scope.FMailPwd = '';

		scope.UpdateMailPassword = function () {

			var IndNum = /^[0]?[6789]\d{9}$/;
			var pattern = new RegExp(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/);
			if (!scope.Fmobilenum) {
				swal('info', 'Please Enter Mobile Number', 'info');
				return;
			}
			if (!IndNum.test(scope.Fmobilenum)) {
				swal('info', 'Please Valid Enter Mobile Number', 'info');
				return;
			}
			if (!scope.FMailId) {
				swal('info', 'Please Valid Enter Mail Id', 'info');
				return;
			}
			if (!pattern.test(scope.FMailId)) {
				swal('info', 'Please Valid Enter Mail Id', 'info');
				return;
			}
			if (!scope.FMailPwd) {
				swal('info', 'Please Valid Enter Mail Id Password', 'info');
				return;
			}

			if (!scope.FConfirmMailPwd) {
				swal('info', 'Please Valid Enter Confirm Mail Id Password', 'info');
				return;
			}
			if (scope.FMailPwd != scope.FConfirmMailPwd) {
				swal('info', 'Please Mail Id and Confirm Mail Id are MisMatch', 'info');
				return;
			}
			else {
				var obj = {
					Ftype: 1, FDistrictCode: sessionStorage.getItem("distcode"),
					FMandalCode: sessionStorage.getItem("mcode"), FSeccCode: sessionStorage.getItem("secccode"),
					FUserId: sessionStorage.getItem("user"),
					FMobileNumber: scope.Fmobilenum,
					FMailID: scope.FMailId, FPassword: scope.FMailPwd, FRoleId: sessionStorage.getItem("desinagtion"),
					Insertby: sessionStorage.getItem("user")

				}
				var token = sessionStorage.getItem("Token")
				Login_Services.POSTENCRYPTAPI("UpdateMailMobileForm", obj, token, function (value) {
					var res = value.data;
					if (res.Status == '100') {
						swal('info', 'Data Updated Successfully', 'success');
						state.go("ue.Dashboard");
					}
					else {
						
						swal('info', res.Reason, 'error');
						return;
					}
				});
			}


		}
		

	}
})();