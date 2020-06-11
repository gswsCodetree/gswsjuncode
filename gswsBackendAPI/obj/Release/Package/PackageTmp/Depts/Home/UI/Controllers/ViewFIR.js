(function () {
	/* eslint-disable */
	var app = angular.module("GSWS");
	app.controller("Home_ViewFIRCntrl", ["$scope", "Home_Services", '$sce', "$state", Cert_CTRL]);
	function Cert_CTRL(scope, home_services, sce, state) {
		var token = sessionStorage.getItem("Token"); var user = sessionStorage.getItem("user");
		if (!(token) || !(user)) {
			alert('Session expired..!');
			state.go("Login");
		} else
			Loaddistricts();
		scope.detailsshow = false;
		scope.RData = [];
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
			//scope.txtfir = "357";
			//scope.txtdate = "06/10/2019";
			//scope.txtyear = "2019";
			var reqdate = ""; var reg_year = "";
			if ((scope.FIRregDate)) {
				var date = new Date($('#regdate').val());
				//var year = date.getFullYear();
				day = date.getDate();
				month = date.getMonth() + 1;
				year = date.getFullYear();
				reqdate = [day, month, year].join('/');
				reg_year = year;
			}


			var req = {
				districtCd: scope.ddldist, psCd: scope.ddlps, firNo: scope.txtfir, firYear: reg_year, firDate: moment(scope.FIRregDate).format('DD/MM/YYYY')
			};
			scope.Preloader = true;
			home_services.POSTENCRYPTAPI("CheckViewFIRStatus", req, token, function (value) {
				scope.Preloader = false;
				if (value.status == 200) {
					var res = value.data[0];

					if (res.status == "Success") {
						if (res.viewFIRReponse.length == 0) {
							alert('No Data Found');
							scope.RData = "";
							scope.divdetails = false;
						}
						else {
							scope.divdetails = true;
							scope.RData = res.viewFIRReponse[0];
						}
					}
					else {
						alert('No Data Found');
						scope.RData = "";
						scope.divdetails = false;
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
				}
				else {
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

		scope.GetFile = function () {
			var req = {
				districtCd: scope.ddldist, psCd: scope.ddlps, firNo: scope.txtfir, firRegNum: scope.RData.fir_reg_num, sdpoCd: scope.RData.sdpo_cd, circleCd: scope.RData.circle_cd

			};
			scope.Preloader = true;
			home_services.POSTENCRYPTAPI("DownloadFIR", req, token, function (value) {
				scope.Preloader = false;
				if (value.data.status == 200) {

					var pdf = 'data:application/octet-stream;base64,' + value.data.result;
					var dlnk = document.getElementById('dwnldLnk');
					dlnk.href = pdf;
					dlnk.click();
				}
				else {
					alert(value.data.result);
				}
			}, function (error) {
				console.log(error);
			});
		}
	}
})();


