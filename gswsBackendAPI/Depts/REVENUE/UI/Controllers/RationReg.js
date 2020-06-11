(function () {
	var app = angular.module("GSWS");

	app.controller("RationRegController", ["$scope", "$state", "$log", "REVENUE_Services", RationReg_CTRL]);

	function RationReg_CTRL(scope, state, log, REVENUE_Services) {
		scope.preloader = false;

		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}
		loaddistmaster();

		scope.GetMandalload = function () {
			
			scope.mcode = "";
			scope.gpcode = "";
			loadmandalmaster();
		}

		scope.GPLoad = function () {
			scope.gpcode = "";
			loadPanchayatmaster();
		}

        function getspadanatoken() {
            scope.spandanatoken = "";
            scope.surl = "";
            REVENUE_Services.DemoAPI("GetSpadanaToken", "",function (value) {

                var res = value.data;
                if (res.StatusCode == "200" && res.Status =="Success") {

                    scope.spandanatoken = res.Token;
					scope.surl = res.url;

					
					var stoken = sessionStorage.getItem("spanecncypt");
					var TOKENurl = "accessToken=" + scope.spandanatoken + "&Volunteerid=2255667788&AadhaarNo=" + scope.UID + "&vvstype=VVS2&DistId=" + scope.distcode + "&MandalId=" + scope.mcode + "&GpId=" + scope.gpcode + "&GpFlag=" + scope.ruflag+"&"+stoken;

                    window.open(scope.surl + TOKENurl, "_blank");
                }
                else {

                    swal('info', res.Message, 'info');
                    return;
                }
            });
        }
        scope.ApplyRation = function () {


            if (scope.UID == "" || scope.UID == undefined || scope.UID == null || scope.UID.length<12) {

                swal('info', 'Please Enter 12 Digit Aadhaar Number', 'info')
                return;
            }
            if (scope.distcode == "" || scope.distcode == undefined || scope.distcode == null) {

                swal('info', 'Please Select District', 'info')
                return;
            }
            if (scope.ruflag == "" || scope.ruflag == undefined || scope.ruflag == null) {

                swal('info', 'Please Select rural/urban flag', 'info')
                return;
            }
            if (scope.mcode == "" || scope.mcode == undefined || scope.mcode == null) {

                swal('info', 'Please Select Mandal', 'info')
                return;
            }
            if (scope.gpcode == "" || scope.gpcode == undefined || scope.gpcode == null) {

                swal('info', 'Please Please Select Pachayat', 'info')
                return;
            }
            else {
                getspadanatoken();

            }
          
        }
		function loaddistmaster() {

			var input = { FTYPE: 4 }

			REVENUE_Services.POSTENCRYPTAPI("GetLGDMaster", input, token, function (value) {

				var res = value.data;
				if (res.Status == "100") {

					scope.distlist = res.DataList;
				}

				else if (res.Status == "428") {
					swal('info', res.Reason, 'info');
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

			REVENUE_Services.POSTENCRYPTAPI("GetLGDMaster", input, token, function (value) {

				var res = value.data;
				if (res.Status == "100") {

					scope.mandallist = res.DataList;
				}
				else if (res.Status == "428") {
					swal('info', res.Reason, 'info');
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

			REVENUE_Services.POSTENCRYPTAPI("GetLGDMaster", input, token, function (value) {

				var res = value.data;
				if (res.Status == "100") {

					scope.gplist = res.DataList;
				}
				else if (res.Status == "428") {
					swal('info', res.Reason, 'info');
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
	}
})();

