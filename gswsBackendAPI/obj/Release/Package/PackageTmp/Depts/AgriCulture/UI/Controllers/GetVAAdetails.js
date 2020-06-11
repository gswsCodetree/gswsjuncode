(function () {
	var app = angular.module("GSWS");

	app.controller("Agri_VAADetails_Controller", ["$scope", "$state", "$log", "AgriCulture_Services", SeedEligibilty_CTRL]);

	function SeedEligibilty_CTRL(scope, state, log, Agri_Services) {

		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}
		LoadDistricts();

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



			var input = {
				dcode: scope.distcode, mcode: scope.mcode
			};

			Agri_Services.DemoAPI("GetVAADetails", input, function (value) {
				var res = value.data;
				if (res.Status == "Success") {
					scope.divdetails = true;
					scope.Eligibilities_Seed = res.Data.VADet.VAList;// JSON.parse(res.Data.rtgsDetails);
					scope.Appinfo = res.Data.VADet;
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
		scope.GetMandals = function () {
			scope.mandallist = [];
			scope.ruflag = "";
			loadmandalmaster();
		}

		//Load Districts
		function LoadDistricts() {
			var input = { FTYPE: 4 };

			Agri_Services.POSTENCRYPTAPI("GetLGDMaster", input, token, function (value) {

				var res = value.data;
				if (res.Status == "100") {

					scope.distlist = res.DataList;
					scope.mandallist = "";
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



		scope.Refresh = function () {
			location.reload(true);
		}

	}
})();
