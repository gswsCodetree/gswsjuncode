(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("RBPaymentStatus", ["$scope", "$state", "$window", "$log", "AgriCulture_Services", Rythu_CTRL]);

	function Rythu_CTRL(scope, state, window, log, vol_services) {
		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");
		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}
		scope.uid_mask_flag = false;
		scope.show_uid = function () {
			scope.uid_mask_flag = !scope.uid_mask_flag;
		};
		scope.ValidateAadhar = function () {
			if (scope.aadharNo.length < 12) {
				swal('info', 'Please Enter Valid Aadhar number.', 'info')
				return;
			}
		}
		CaptchLoad();
		function CaptchLoad() {
			var obj = { ftype: "1" }
			vol_services.DemoAPI("GetCaptcha", obj, function (data) {
				var res = data.data;
				if (res.code == "100") {
					scope.capatchval = "";
					$("#capid").html(res.idval);
					scope.capId = res.idval;
					$("#captchdis tr").remove();
					var img = $("<img>", { "id": res.idval, "src": "data:image/Gif;base64," + res.imgurl, "width": "90px", "height": "40px" });
					var rowget = $('<tr></tr>').append('<td></td>').html(img);
					$("#captchdis tbody").append(rowget);
				}

				else {
					scope.apploader = false;

					CaptchLoad();
				}

			});
		}
		scope.GetRefresh = function () {
			CaptchLoad();

		}
		scope.uid_mask_flag = false;
		scope.show_uid = function () {
			scope.uid_mask_flag = !scope.uid_mask_flag;
		};

		scope.GetData = function () {
			if (scope.aadharNo == "" || scope.aadharNo == undefined) {
				swal('info', 'Please Enter Aadhar number.', 'info')
				return;
			}
			else if (scope.aadharNo.length < 12) {
				swal('info', 'Please Enter Valid Aadhar number.', 'info')
				return;
			}
			else if (scope.Fcaptcha == "" || scope.Fcaptcha == undefined || scope.Fcaptcha == null) {
				swal('info', 'Please Enter Captcha', 'info')
				return;
			}
			var UId = scope.aadharNo;
			var UserDetails = { PTYPE: "1", PUIDNUM: UId + "760535773709671457294710381147965833" }
			var input = {
				Captcha: scope.Fcaptcha,
				ConfirmCaptch: scope.capId,
				deptId: "1234",
				deptName: "Agriculture",
				serviceName: "Ysr-RythuBharosa-Payment-Status",
				serviceType: "REST",
				method: "POST",
				simulatorFlag: "false",
				application: "GWS",
				username: sessionStorage.getItem("user"),
				userid: sessionStorage.getItem("user"),
				data: UserDetails
			};

			vol_services.DemoAPI("GetRBStatus1", input, function (value) {
				debugger;
				var res = value.data;
				//scope.result = {};
				var result = res.REASON;

				var result1 = res.REASON1;
				var result1status;
				if (result1 == undefined || result1 == "") {
					result1status = "";
				}
				else {
					result1status = JSON.parse(result1.REASON).Status;
				}
				scope.RBData = [];
				scope.BeneficiaryData = [];

				if (res.Status == "100") {
					if (JSON.parse(result).Status == "102") {
						swal('info', "No Data Found.", 'info');
						document.getElementById('PermanentDetails').style.display = "none";
						document.getElementById('BeneficiaryDetails').style.display = "none";
						return;
					}
					else if (JSON.parse(result).Status == "100" && result1status == "100") {
						document.getElementById('PermanentDetails').style.display = "block";
						document.getElementById('BeneficiaryDetails').style.display = "block";
						scope.RBData = JSON.parse(result).Details;
						scope.BeneficiaryData = JSON.parse(result1.REASON).Details;
						window.scrollTo(0, 500);
						// }

					}
					else if (JSON.parse(result).Status == "100") {
						document.getElementById('PermanentDetails').style.display = "block";
						document.getElementById('BeneficiaryDetails').style.display = "none";
						scope.RBData = JSON.parse(result).Details;
						window.scrollTo(0, 500);

					}

				}
				else if (res.Status == "428") {
					swal('info', res.Reason, 'info');
					//sessionStorage.clear();
					//state.go("Login");
					document.getElementById('PermanentDetails').style.display = "none";
					document.getElementById('BeneficiaryDetails').style.display = "none";
					return;
				}
				else if (res.Status == "102") {
					swal('info', res.REASON, 'info');
					document.getElementById('PermanentDetails').style.display = "none";
					document.getElementById('BeneficiaryDetails').style.display = "none";
					CaptchLoad();
					return;
				}
				else {
					document.getElementById('PermanentDetails').style.display = "none";
					document.getElementById('BeneficiaryDetails').style.display = "none";
					swal('info', res.Reason, 'info');
					return;
				}
			});
			CaptchLoad();
			scope.Fcaptcha = "";

		}


	}
})();