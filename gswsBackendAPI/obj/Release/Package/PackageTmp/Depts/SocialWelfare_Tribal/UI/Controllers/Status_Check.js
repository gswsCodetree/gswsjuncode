(function () {
	/* eslint-disable */
	var app = angular.module("GSWS");
	app.controller("PK_StatusCheck", ["$scope", "Socialwelfare_Services", '$sce', "$state", Cert_CTRL]);
	function Cert_CTRL(scope, socialwelfare_services, sce, state) {
		scope.preloader = false;
		var token = sessionStorage.getItem("Token"); var user = sessionStorage.getItem("user");
		if (!(token) || !(user)) {
			alert('Session expired..!');
			state.go("Login");
		}
		scope.detailsshow = false;
		scope.getdetails = function () {

			if (scope.txtcertid == undefined || scope.txtcertid == null) {
				alert('Please enter 12 digit Bride/Groom Aadhaar [or] 11 digit Reg.ID.');
				return;
			}
			var val = scope.txtcertid.length;
			var card = scope.txtcertid;
			if (val == 12) {

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

			}
			var req = {
				Aadhaar: scope.txtcertid
			};
			scope.preloader = true;
			socialwelfare_services.POSTENCRYPTAPI("StatusCheck", req, token, function (value) {
				scope.preloader = false;
				if (value.status == 200) {
					var res = value.data;

					if (res.Status == "Success") {
						scope.Status = "Available";
						scope.detailsshow = true;
						scope.details = res.Data[0]
					}
					else {
						scope.Status = "Not Available";
						scope.detailsshow = false;
						scope.details = "";
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


