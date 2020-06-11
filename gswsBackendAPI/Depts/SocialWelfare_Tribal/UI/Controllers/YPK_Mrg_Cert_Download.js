(function () {
	/* eslint-disable */
	var app = angular.module("GSWS");
	app.controller("Mrg_CertificateDownlaod", ["$scope", "Socialwelfare_Services", '$sce', "$state", Cert_CTRL]);
	function Cert_CTRL(scope, socialwelfare_services, sce, state) {
		scope.Preloader = false;
		var token = sessionStorage.getItem("Token"); var user = sessionStorage.getItem("user");
		if (!(token) || !(user)) {
			alert('Session expired..!');
			state.go("Login");
		}
		scope.detailsshow = false;
		scope.getdetails = function () {

			if (scope.txtcertid == undefined || scope.txtcertid == null) {
				alert('Please enter Applicant Aadhaar Number.');
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
				//return status;
			}

			var req = {
				Aadhaar: scope.txtcertid

			};
			scope.Preloader = true;
			socialwelfare_services.POSTENCRYPTAPI("MrgCert_Download", req, token, function (value) {
				scope.Preloader = false;
				if (value.status == 200) {
					var res = value.data;

					if (res.Status == "Success") {
						scope.Status = "Available";
						scope.detailsshow = true;
						var att = res.Data;
						sce.trustAsUrl(att);
						scope.Attachment = att.replace('https', 'http');
					}
					else {
						scope.Status = "Not Available";
						scope.detailsshow = false;
						alert('No Data Found');
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


