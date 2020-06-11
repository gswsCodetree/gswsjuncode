(function () {
	var app = angular.module("GSWS");

	app.controller("Agri_SeedEligibility_Controller", ["$scope", "$state", "$log", "AgriCulture_Services", SeedEligibilty_CTRL]);

	function SeedEligibilty_CTRL(scope, state, log, Agri_Services) {

		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}
		LoadDistricts();
		LoadGroups(); LoadSeasons();
		scope.getdetails = function () {
			if (!(scope.txtcmplttid)) {
				alert('Please enter Complaint ID');
				return;
			}
			var req = {
				appcode: scope.txtcmplttid
			};
			Agri_Services.DemoAPI("Complaint_StatusCheck", req, function (value) {
				var res = value.data;
				if (res.Status == "Success") {
					scope.detailsshow = true;
					if (!(res.Data)) {
						scope.detailsshow = false;
						scope.ResData = "";
						alert("No Data available for this complaint id");
						return;
					}
					else
						scope.ResData = res.Data;
				}

				else {
					scope.detailsshow = false;
					scope.ResData = "";
					alert("No Data available for this complaint id");
					return;
				}
			});
		};

		//get Data
		scope.getdetails = function () {


			var input = {
				dcode: scope.ddldist, seasoncropyear: scope.ddlseason, seedvariety: scope.ddlvariety
			};

			Agri_Services.DemoAPI("GetSeedVarietyData", input, function (value) {
				var res = value.data;
				if (res.Status == "Success") {
					if ((scope.distlist)) {
						scope.distName = $.grep(scope.distlist, function (x) {
							return x.LGD_DISTRICT_CODE == scope.ddldist;
						})[0].LGD_DISTRICT_NAME;
					}
					else
						scope.distName = "";
					scope.divdetails = true;
					scope.Eligibilities_Seed = JSON.parse(res.Data.eligibilities);
					scope.Info = res.Data;
				}
				else {
					scope.distName = "";
					scope.Eligibilities_Seed = "";
					scope.Info = "";
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

		//Load Districts
		function LoadDistricts() {
			var input = { FTYPE: 4 };

			Agri_Services.POSTENCRYPTAPI("GetLGDMaster", input, token, function (value) {

				var res = value.data;
				if (res.Status == "100") {

					scope.distlist = res.DataList;
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
