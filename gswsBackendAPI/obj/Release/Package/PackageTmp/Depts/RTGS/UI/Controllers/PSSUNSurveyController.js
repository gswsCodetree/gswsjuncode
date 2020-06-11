(function () {
	var app = angular.module("GSWS");

	app.controller("PSSUNSurveyController", ["$scope", "$state", "$log", "RTGS_Services", PSSUNSurveyController]);

	function PSSUNSurveyController(scope, state, log, RTGS_Services) {
		scope.preloader = false;
		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}
		loadmaster(1);

		scope.CheckAadhaar = function () {

			if (scope.uid == "" || scope.uid == undefined || scope.uid == null) {
				swal('info', 'Please enter Aadhaar Number', 'info');
				return;
			}

			scope.divmob = true;
			scope.divotp = true;


		}

		scope.SendOTP = function () {

			
		}
		scope.VerifyOTP = function () {

			

			if (scope.otpval == "123456") {
				scope.divotp = false;
				scope.divmaster = true;
			}
			else {
				scope.divotp = true;
				scope.divmaster = false;
			}
		}
		scope.GetMandalload = function () {

			scope.smanname = "";
			scope.svtcode = "";
			loadmaster(2);
		}
		scope.getVillageload = function () {

			
			scope.svtcode = "";
			loadmaster(3);
		}

		function loadmaster(ftype) {

			var token = sessionStorage.getItem("Token");
			var req = {
				Ftype: ftype, Fdistrict: scope.sdistname, Fruflag: scope.sruflag, Fmandal: scope.smanname, Fvillage: scope.svtcode
			}

			RTGS_Services.POSTREVENUEENCRYPTAPI("GetSeccMaster", req, token, function (value) {

				var res = value.data;
				console.log(res);
				if (res.Status == "100") {
					if (ftype == "1") {

						scope.seccdistlist = res.Details;

					}
					else if (ftype == "2") {
						scope.seccmandallist = res.Details;
					}
					else if (ftype == "3") {
						scope.seccvillagelist = res.Details;
					}

				}
				else {

					swal('info', res.Reason, 'info');
					return;
				}
			});
		}
	}
})();

