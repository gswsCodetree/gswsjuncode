(function () {
	/* eslint-disable */
	var app = angular.module("GSWS");
	app.controller("Home_VehicleRecoverySearchCntrl", ["$scope", "Home_Services", '$sce', "$state", Cert_CTRL]);
	function Cert_CTRL(scope, home_services, sce, state) {
		scope.Preloader = false;
		var token = sessionStorage.getItem("Token"); var user = sessionStorage.getItem("user");
		if (!(token) || !(user)) {
			alert('Session expired..!');
			state.go("Login");
		} else {
			Loaddistricts();
		}
		scope.detailsshow = false;
		scope.divchange = function (type) {
			if (type == "1") {
				scope.typevehiclesearch = true;
				scope.typePropertysearch = false;
			}
			else {
				scope.typevehiclesearch = false;
				scope.typePropertysearch = true;
			}

		}
		scope.getdetails = function (type) {
			var method = "";
			if (type == 1) {
				//if (scope.txtcertid == undefined || scope.txtcertid == null) {
				//	alert('Please enter Applicant Aadhaar Number.');
				//	return;
				//}
				var req = {

				};
				if ((scope.frmdate) && (scope.todate)) {
					var datefrom = new Date($('#fromdate').val());
					var day = datefrom.getDate();
					var month = datefrom.getMonth() + 1;
					var year = datefrom.getFullYear();
					var dateto = new Date($('#todate').val());
					var day1 = dateto.getDate();
					var month1 = dateto.getMonth() + 1;
					var year1 = dateto.getFullYear();
					var fdate = [day, month, year].join('/');
					var tdate = [day1, month1, year1].join('/');

					//
					var startDate = $("#fromdate").val();
					var endDate = $("#todate").val();
					var datefrom = new Date(startDate); var dateto = new Date(endDate);
					var diff = daydiff(datefrom, dateto);

					if (diff > 7) {
						alert('above 7 days');
						return;
					}
					//Final Dates
					req.toDt = tdate;
					req.fromDt = fdate;
				}
				else {
					if (scope.txtcertid == undefined || scope.txtcertid == null) {
						alert('Please enter Applicant Aadhaar Number.');
						return;
					}
					req.vehicleRegnum = scope.txtcertid;
				}


				method = "VehicleRecoverStatusCheck";
			}
			else {
				var req = {
					districtCd: scope.ddldist, psCd: scope.ddlps, firNo: scope.txtfir
				};
				method = "PropertyStatusCheck";
			}

			scope.Preloader = true;
			home_services.POSTENCRYPTAPI(method, req, token, function (value) {
				scope.Preloader = false;
				if (value.status == 200) {
					var res = value.data[0];

					var RelResponseData = "";

					if (res.status == "Success") {
						if (type == 1) {
							RelResponseData = res.stolenPropVehicle;
						}
						else {
							RelResponseData = res.stolenPropProperty;
						}
						if (RelResponseData.length == 0) {
							alert('No Data Found');
							scope.RData = "";
							scope.detailsshow = false;
						}
						else {
							scope.detailsshow = true;
							if (type == 1) {
								scope.divVehicle = true; scope.divproperty = false;
								scope.RData1 = RelResponseData;
							}
							else {
								scope.divproperty = true; scope.divVehicle = false;
								scope.RData = RelResponseData;
							}

						}

					}
					else {
						scope.detailsshow = false;
						scope.RData = "";
						alert('No Data Found');
					}
				} else {
					scope.Preloader = false;
					swal('Exception!', 'Something went wrong', 'error');
					return;
				}

			});
		};
		function daydiff(first, second) {
			return (second - first) / (1000 * 60 * 60 * 24)
		}
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
				}
				else {
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


