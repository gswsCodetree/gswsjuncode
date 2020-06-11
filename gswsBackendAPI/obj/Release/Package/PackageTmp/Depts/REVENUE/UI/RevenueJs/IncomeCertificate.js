(function () {
	var app = angular.module("GSWS", ["Network"]);

	app.controller("IncomeController", ["$scope", "network_service", IncomeController]);

	function IncomeController(scope, ns) {
		var baseurl = "/api/GSWSWeb/";

		var token = sessionStorage.getItem("TOKEN");
		loaddistmaster();
		function loaddistmaster() {

			var input = { FTYPE: 1 }

			ns.encrypt_post(baseurl +"GetGSWSSecretariatMaster", input, token, function (value) {

				var res = value.data;
				if (res.Status == "100") {

					scope.distlist = res.DataList;
				}

				else if (res.Status == "428") {
					swal('info', res.Reason, 'info');
					sessionStorage.clear();
					//state.go("Login");
					window.location = window.location.origin+location.pathname+"Home/Main";
					return;
				}

				else {

					swal('info', res.Reason, 'info');
					return;
				}
			});
		}

		scope.GetMandal = function () {
			scope.Smcode = "";
			scope.Secclist = [];
			loadmandalmaster();
		}
		scope.GetMandal = function () {
			scope.Smcode = "";
			scope.SeccCode = "";
			scope.mandallist = [];
			scope.Secclist = [];
			loadmandalmaster();
		}
		scope.GetSecretariat = function () {
					scope.SeccCode = "";
					scope.Secclist = [];
			loadSecretariatmaster();
		}
		function loadmandalmaster() {

			var input = { FTYPE: 2, DISTCODE: scope.Sdistcode }

			Login_Services.POSTENCRYPTAPI("GetGSWSSecretariatMaster", input, token, function (value) {

				var res = value.data;
				if (res.Status == "100") {

					scope.mandallist = res.DataList;
				}
				else if (res.Status == "428") {
					swal('info', res.Reason, 'info');
					sessionStorage.clear();
					window.location = window.location.origin + location.pathname + "Home/Main";
					return;
				}
				else {

					swal('info', res.Reason, 'info');
					return;
				}
			});
		}

		function loadSecretariatmaster() {

			var input = { FTYPE: 3, DISTCODE: scope.Sdistcode, MCODE: scope.Smcode }

			Login_Services.POSTENCRYPTAPI("GetGSWSSecretariatMaster", input, token, function (value) {

				var res = value.data;
				if (res.Status == "100") {

					scope.Secclist = res.DataList;
				}
				else if (res.Status == "428") {
					swal('info', res.Reason, 'info');
					sessionStorage.clear();
					window.location = window.location.origin + location.pathname + "Home/Main";
					return;
				}
				else {

					swal('info', res.Reason, 'info');
					return;
				}
			});
		}

		scope.CheckedSameAddress = function () {

		}
	}
})();
