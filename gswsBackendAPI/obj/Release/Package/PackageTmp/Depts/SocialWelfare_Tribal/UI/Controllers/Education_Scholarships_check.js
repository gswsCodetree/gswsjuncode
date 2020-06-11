(function () {
	/* eslint-disable */
	var app = angular.module("GSWS");
	app.controller("Education_Overseas_Check", ["$scope", "Socialwelfare_Services", '$sce', "$state", Cert_CTRL]);
	function Cert_CTRL(scope, socialwelfare_services, sce, state) {
		scope.Preloader = false;
		var token = sessionStorage.getItem("Token"); var user = sessionStorage.getItem("user");
		var type = GetParameterValues('type');
		if (!(token) || !(user) || (type.toLowerCase() != "overseas" && type.toLowerCase() != "corporate" && type.toLowerCase() != "hostels" && type.toLowerCase() != "postmetric" && type.toLowerCase() != "premetric" && type.toLowerCase() != "bas")) {
			sessionStorage.clear();
			alert('Session expired..!');
			state.go("Login");
		}
		var type = GetParameterValues('type');
		scope.P_Head = type;
		function GetParameterValues(param) {
			var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
			for (var i = 0; i < url.length; i++) {
				var urlparam = url[i].split('=');
				if (urlparam[0] == param) {
					return urlparam[1];
				}
			}
		}
		LoadAcadamicyears();
		scope.detailsshow = false;
		function LoadAcadamicyears() {
			scope.Preloader = true;
			socialwelfare_services.POSTENCRYPTAPI("GetAnnaualyears", "", token, function (data) {
				scope.Preloader = false;
				if (data.status == 200) {
					var res = data.data;
					if (res.Status == "Success") {
						scope.AcadamicyearsList = res.Data;
					}
					else {
						swal('Error', 'No Acadamics data found', 'error');
						return;
					}
				}
				else {
					swal('Error', 'Something went wrong..', 'error');
					return;
				}
			});
		}
		scope.getdetails = function () {

			if (!(scope.txtcertid)) {
				alert('Please enter Applicant Aadhaar Number.');
				return;
			}
			if (!(scope.ddlacdyear)) {
				alert('Please select Acadamic year');
				return;
			}
			if (!(type)) {
				alert('Something went wrong.Please Login again.');
				return;
			}
			var val = scope.txtcertid.length;
			var card = scope.txtcertid;
			if (val < 12) {
				alert('Please Enter 12 Digit Aadhaar Number.');
				return;
			}
			if (card == "111111111111" || card == "222222222222" || card == "333333333333" || card == "444444444444" || card == "555555555555" || card == "666666666666"
				|| card == "777777777777" || card == "888888888888" || card == "999999999999" || card == "000000000000") {
				alert("Please Enter 12 Digit Aadhaar Number");
				return;
			}


			var status = validateVerhoeff(card);

			if (status) {

			}
			else {
				alert('Enter Valid Aadhaar Number');
				return;
			}

			var req = {
				Aadhaar: scope.txtcertid, Acadamicyear: scope.ddlacdyear, Scheme: type

			};
			scope.Preloader = true;
			socialwelfare_services.POSTENCRYPTAPI('Education_ApplicationCheck', req, token, function (value) {
				scope.Preloader = false;
				if (value.status == 200) {
					var res = value.data;

					if (res.Status == "Success") {
						scope.Status = "Available";
						scope.detailsshow = true;
						scope.trackingstatus = res.TrackingStatus;
						scope.department = res.Deapartment;
						scope.benfificiaryId = res.BeneficiaryID;
						scope.benfificiaryName = res.BeneficiaryName;
					}
					else {
						scope.Status = "Not Available";
						scope.detailsshow = false;
						scope.trackingstatus = "";
						scope.department = "";
						scope.benfificiaryId = "";
						scope.benfificiaryName = "";
						alert(res.Reason);
					}
				}
				else {
					swal('Error', 'Something went wrong..', 'error');
					return;
				}
			});
		};

		scope.Refresh = function () {
			location.reload(true);
		}


	}
})();


