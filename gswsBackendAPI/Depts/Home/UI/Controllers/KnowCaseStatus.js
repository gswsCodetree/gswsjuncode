(function () {
	/* eslint-disable */
	var app = angular.module("GSWS");
	app.controller("Home_KnowCaseStatusCntrl", ["$scope", "Home_Services", '$sce', "$state", Cert_CTRL]);
	function Cert_CTRL(scope, home_services, sce,state) {
		scope.Preloader = false;
		var token = sessionStorage.getItem("Token"); var user = sessionStorage.getItem("user");
		if (!(token) || !(user)) {
			alert('Session expired..!');
			state.go("Login");
		}
		else {
			Loaddistricts();
		}
		scope.detailsshow = false;

		scope.getdetails = function () {

			if (!(scope.ddldist)) {
				alert('Please select District.');
				return;
			}
			if (!(scope.ddlps)) {
				alert('Please select Police Station.');
				return;
			}
			if (!(scope.txtfir)) {
				alert('Please enter FIR No.');
				return;
			}
			var req = {
				districtCd: scope.ddldist, psCd: scope.ddlps, firNo: scope.txtfir
			};
			scope.Preloader = true;
			home_services.POSTENCRYPTAPI("CheckCaseStatus", req, token, function (value) {
				if (value.status == 200) {
					var res = value.data[0];
					scope.Preloader = false;
					if (res.status == "Success") {
						if (res.viewFIRReponse.length == 0) {
							alert('No Data Found');
							scope.RData = "";
							scope.detailsshow = false;
						}
						else {
							scope.detailsshow = true;
							scope.RData = res.viewFIRReponse[0];
						}
					}
					else {
						alert('No Data Found');
						scope.RData = "";
						scope.detailsshow = false;
					}
				}
				else {
					scope.Preloader = false;
					swal('Exception!', 'Something went wrong', 'error');
					return;
				}

			});
		};
		function Loaddistricts() {

			var input = { PTYPE: 1 }
			scope.Preloader = true;
			home_services.POSTENCRYPTAPI("GetDistricts", input, token, function (value) {
				scope.Preloader = false;
				if (value.status == 200) {
					var res = value.data;
					if (res.Status == "Success") {

						scope.distlist = res.Data;
					}
					else {

						swal('info', res.Reason, 'info');
						return;
					}
				} else {
					scope.Preloader = false;
					swal('Exception!', 'Something went wrong', 'error');
					return;
				}

			});
		}

		scope.GetPS = function (dist) {
			var input = { PTYPE: 2, PDIST: dist }
			scope.Preloader = true;
			home_services.POSTENCRYPTAPI("GetDistricts", input, token, function (value) {
				scope.Preloader = false;
				if (value.status == 200) {
					var res = value.data;

					if (res.Status == "Success") {

						scope.PSlist = res.Data;
					}
					else {

						swal('info', res.Reason, 'info');
						return;
					}
				} else {
					scope.Preloader = false;
					swal('Exception!', 'Something went wrong', 'error');
					return;
				}

			});
		}
		scope.Refresh = function () {
			location.reload(true);
		}
	}
})();


