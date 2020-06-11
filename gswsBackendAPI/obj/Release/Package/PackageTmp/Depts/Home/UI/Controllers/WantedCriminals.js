(function () {
	/* eslint-disable */
	var app = angular.module("GSWS");
	app.controller("Home_WantedCriminalsCntrl", ["$scope", "Home_Services", '$sce', '$http', "$state", Cert_CTRL]);
	function Cert_CTRL(scope, home_services, sce, $http, state) {

		var token = sessionStorage.getItem("Token"); var user = sessionStorage.getItem("user");
		if (!(token) || !(user)) {
			alert('Session expired..!');
			state.go("Login");
		} else {
			Loaddistricts();
			GetCD();
		}
		scope.detailsshow = false;

		scope.getdetails = function () {
			if (!(scope.ddlgender)) {
				alert('Please select Gender');
				return;
			}
			if (!(scope.ddldist)) {
				alert('Please select District');
				return;
			}
			if (!(scope.ddlps)) {
				alert('Please select Police Station');
				return;
			}
			if (!(scope.ddlcrimehead)) {
				alert('Please select Crime Head');
				return;
			}

			var req = {
				psCd: scope.ddlps, districtCd: scope.ddldist, majorHead: scope.ddlcrimehead, gender: scope.ddlgender, age: scope.txtname, name: scope.txtage
			};

			home_services.DemoAPI("CheckWantedcriminals", req, function (value) {
				var res = value.data[0];

				if (res.status == "Success") {
					if (res.wantedPersonDetails.length == 0) {
						alert('No Data Found');
						scope.RData = "";
						scope.detailsshow = false;
					}
					else {
						scope.detailsshow = true;
						scope.RData = res.wantedPersonDetails;

					}
				}
				else {
					alert('No Data Found');
					scope.RData = "";
					scope.detailsshow = false;
				}
			});
		};

		function Loaddistricts() {

			var input = { PTYPE: 1 }

			home_services.DemoAPI("GetDistricts", input, function (value) {
				var res = value.data;
				if (res.Status == "Success") {

					scope.distlist = res.Data;
				}
				else {

					swal('info', 'Some Inputs not loaded.Please try again later.', 'info');
					return;
				}

			});
		}

		scope.GetPS = function (dist) {
			var input = { PTYPE: 2, PDIST: dist }

			home_services.DemoAPI("GetDistricts", input, function (value) {
				var res = value.data;
				if (res.Status == "Success") {

					scope.PSlist = res.Data;
				}
				else {

					swal('info', 'Some Inputs not loaded.Please try again later.', 'info');
					return;
				}

			});
		}
		function GetCD() {
			var input = { PTYPE: 4 }

			home_services.DemoAPI("GetDistricts", input, function (value) {
				var res = value.data;
				if (res.Status == "Success") {

					scope.CHlist = res.Data;
				}
				else {

					swal('info', 'Some Inputs not loaded.Please try again later.', 'info');
					return;
				}

			});
		}

		scope.Refresh = function () {
			location.reload(true);
		}
	}
})();


