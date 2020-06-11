(function () {
	/* eslint-disable */
	var app = angular.module("GSWS");
	app.controller("Home_ArrestParticularsCntrl", ["$scope", "Home_Services", '$sce', '$http', "$state", Cert_CTRL]);
	function Cert_CTRL(scope, home_services, sce, $http, state) {
		scope.Preloader = false;
		var token = sessionStorage.getItem("Token"); var user = sessionStorage.getItem("user");
		if (!(token) || !(user)) {
			alert('Session expired..!');
			state.go("Login");
		}
		else {
			Loaddistricts(); GetCrimeheads();
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
			if (!(scope.txtfromdate)) {
				alert('Please enter from date');
				return;
			}
			if (!(scope.txttodate)) {
				alert('Please enter To date');
				return;
			}
			var datefrom = new Date($('#frmdate').val());
			//var year = date.getFullYear();
			var day = datefrom.getDate();
			var month = datefrom.getMonth() + 1;
			var year = datefrom.getFullYear();
			var from_date = [day, month, year].join('/');

			var dateto = new Date($('#todate').val());
			//var year = date.getFullYear();
			var day1 = dateto.getDate();
			var month1 = dateto.getMonth() + 1;
			var year1 = dateto.getFullYear();
			var to_date = [day1, month1, year1].join('/');



			var uName = ""; var uAge = "";
			if (scope.txtage == undefined)
				uAge = "";
			else
				uAge = scope.txtage
			if (scope.txtname == undefined)
				uName = "";
			else
				uName = scope.txtage
			var req = {
				psCd: scope.ddlps, districtCd: scope.ddldist, majorHead: scope.ddlcrimehead, fromDt: from_date, toDt:
					to_date, gender: scope.ddlgender, age: uAge, name: uName
			};
			scope.Preloader = true;
			if (!(token) || !(user)) {
				alert('Session expired,Please login..!');
				state.go("Login");
			}
			else {
				home_services.POSTENCRYPTAPI("CheckArrestParticularsReport", req, token, function (value) {

					if (value.status == 200) {
						var res = value.data[0];
						scope.Preloader = false;
						if (res.status == "Success") {
							if (res.arrestParticulars.length == 0) {
								swal('Info', 'No Data Found', 'error');
								scope.RData = "";
								scope.detailsshow = false;
							}
							else {
								scope.detailsshow = true;
								scope.RData = res.arrestParticulars;
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
			}
		};
		function Loaddistricts() {

			if (!(token) || !(user)) {
				alert('Session expired..!');
				state.go("Login");
			}

			var input = { PTYPE: 1 }

			home_services.POSTENCRYPTAPI("GetDistricts", input, token, function (value) {
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

			home_services.POSTENCRYPTAPI("GetDistricts", input, token, function (value) {
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
		function GetCrimeheads() {
			var input = { PTYPE: 3 }

			home_services.POSTENCRYPTAPI("GetDistricts", input, token, function (value) {
				var res = value.data;
				if (res.Status == "Success") {

					scope.CHlist = res.Data;
				}
				else {

					swal('info', res.Reason, 'info');
					return;
				}

			});
		}
		scope.Refresh = function () {
			location.reload(true);
		}
	}
})();


