(function () {
	var app = angular.module("GSWS");

	app.controller("Agri_EligibleBenf_Controller", ["$scope", "$state", "$log", "AgriCulture_Services", SeedEligibilty_CTRL]);

	function SeedEligibilty_CTRL(scope, state, log, Agri_Services) {

		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			state.go("Login");
			return;
		}
		LoadDistricts();
		LoadGroups(); LoadSeasons();

		//get Data
		scope.getdetails = function () {

			if (!(scope.distcode)) {
				swal('Info', 'Please select District', 'error');
				return;
			}
			if (!(scope.ruflag)) {
				swal('Info', 'Please select Rural/Urban flag', 'error');
				return;
			}
			if (!(scope.mcode)) {
				swal('Info', 'Please select Mandal', 'error');
				return;
			}
			if (!(scope.ddlvillage)) {
				swal('Info', 'Please select Village', 'error');
				return;
			}
			if (!(scope.ddlgroup)) {
				swal('Info', 'Please select Seed Group', 'error');
				return;
			}
			if (!(scope.ddlvariety)) {
				swal('Info', 'Please select Seed Variety', 'error');
				return;
			}
			if (!(scope.ddlseason)) {
				swal('Info', 'Please select Crop Season', 'error');
				return;
			}
			if ((scope.txtfromdate)) {
				if (!(scope.txttodate)) {
					swal('Info', 'Please select To Date', 'error');
					return;
				}
			}
			if ((scope.txttodate)) {
				if (!(scope.txtfromdate)) {
					swal('Info', 'Please select From Date', 'error');
					return;
				}
			}

			var datefrom = new Date($('#fromdate').val());
			//var year = date.getFullYear();
			var day = datefrom.getDate();
			if (day < 10) {
				day = "0" + day;
			}
			var month = datefrom.getMonth() + 1;
			var year = datefrom.getFullYear();
			var from_date = [year, month, day].join('-');

			var dateto = new Date($('#todate').val());
			//var year = date.getFullYear();
			var day1 = dateto.getDate();
			if (day1 < 10) {
				day1 = "0" + day;
			}
			var month1 = dateto.getMonth() + 1;
			var year1 = dateto.getFullYear();
			var to_date = [year1, month1, day1].join('-');

			var input = {
				dcode: scope.distcode, mcode: scope.mcode, vcode: scope.ddlvillage, seasoncropyear: scope.ddlseason, seedvariety: scope.ddlvariety,
				fromdate: from_date, todate: to_date
				
			};

			Agri_Services.DemoAPI("GetEligibleBeneficiaries", input, function (value) {
				var res = value.data;
				if (res.Status == "Success") {
					scope.divdetails = true;
					scope.Eligibilities_Seed = res.Data.rtgsDetails;// JSON.parse(res.Data.rtgsDetails);
				}
				else {
					scope.Eligibilities_Seed = "";
					scope.divdetails = false;
					swal('Error', 'No Data Found', 'error');
					return;
				}
			});
		}

		//Load Varietys
		scope.GetVariety = function (val) {
			var input = { seedgroup: val };
			Agri_Services.DemoAPI("GetSeedVariety", input, function (value) {
				var res = value.data;
				if (res.Status == "Success") {
					scope.SeedVariety = res.Data;
				}
			});
		}

		//Load Varietys
		scope.GetMandals = function () {
			loadmandalmaster();
		}
		scope.GetVillages = function () {
			loadPanchayatmaster();
		}

		//Load Districts
		function LoadDistricts() {
			var input = { FTYPE: 4 };

			Agri_Services.POSTENCRYPTAPI("GetLGDMaster", input, token, function (value) {

				var res = value.data;
				if (res.Status == "100") {

					scope.distlist = res.DataList;
					scope.mandallist = "";
					scope.gplist = "";
				}
				else if (res.Status == "428") {
					sessionStorage.clear();
					state.go("Login");
					return;
				}
				else {

					swal('info', res.Reason, 'info');
					return;
				}
			});
		}

		function loadmandalmaster() {

			var input = { FTYPE: 5, DISTCODE: scope.distcode, RUFLAG: scope.ruflag }

			Agri_Services.POSTENCRYPTAPI("GetLGDMaster", input, token, function (value) {

				var res = value.data;
				if (res.Status == "100") {
					scope.mandallist = res.DataList;
					scope.gplist = "";
				}
				else if (res.Status == "428") {
					sessionStorage.clear();
					state.go("Login");
					return;
				}
				else {

					swal('info', res.Reason, 'info');
					return;
				}
			});
		}

		function loadPanchayatmaster() {

			var input = { FTYPE: 6, DISTCODE: scope.distcode, RUFLAG: scope.ruflag, MCODE: scope.mcode }

			Agri_Services.POSTENCRYPTAPI("GetLGDMaster", input, token, function (value) {

				var res = value.data;
				if (res.Status == "100") {

					scope.gplist = res.DataList;
				}
				else if (res.Status == "428") {
					sessionStorage.clear();
					state.go("Login");
					return;
				}
				else {

					swal('info', res.Reason, 'info');
					return;
				}
			});
		}
		//Load Seed Groups
		function LoadGroups() {
			var input = {};
			Agri_Services.DemoAPI("GetSeedGroup", input, function (value) {
				var res = value.data;
				if (res.Status == "Success") {
					scope.SeedGroup = res.Data;
				}
			});
		}
		//Load Seasons
		function LoadSeasons() {
			var input = {};
			Agri_Services.DemoAPI("GetSeasonFNY", input, function (value) {
				var res = value.data;
				if (res.Status == "Success") {
					scope.Seasons = res.Data;
				}
			});
		}
		scope.Refresh = function () {
			location.reload(true);
		}

	}
})();
