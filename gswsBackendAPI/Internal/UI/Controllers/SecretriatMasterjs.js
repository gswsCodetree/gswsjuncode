(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("SecretriatController", ["$scope", "$state", "$log", "Internal_Services", Internal_CTRL]);

	function Internal_CTRL(scope, state, log, Internal_Services) {
		scope.preloader = false;
	//	LoadDepartments();
		LoadDistrics();

		//Submit
		scope.Savedata = function () {

			if (scope.seldistrict == "" || scope.seldistrict == undefined || scope.seldistrict == null) {

				swal('info', 'Please Select District', 'info');
				return;
			}
			if (scope.selruflag == "" || scope.selruflag == undefined || scope.selruflag == null) {

				swal('info', 'Please Select Rural/Urban', 'info');
				return;
			}

			if (scope.selmandal == "" || scope.selmandal == undefined || scope.selmandal == null) {

				swal('info', 'Please Select Mandal', 'info');
				return;
			}


			if (scope.selpanchayath == "" || scope.selpanchayath == undefined || scope.selpanchayath == null) {

				swal('info', 'Please Select Panchayat', 'info');
				return;
			}
			if (scope.secretriatname == "" || scope.secretriatname == undefined || scope.secretriatname == null) {

				swal('info', 'Please Enter Secretriat Name', 'info');
				return;
			}
			if (scope.secretriatid == "" || scope.secretriatid == undefined || scope.secretriatid == null) {

				swal('info', 'Please Enter Secretriat Id', 'info');
				return;
			}		

			else {

				var distname = $.grep(scope.DistricsDD, function (b) {
					return b.LGD_DISTRICT_CODE === scope.seldistrict;
				}).LGD_DISTRICT_NAME;
				var manname = $.grep(scope.MandalsDD, function (b) {
					return b.LGD_MANDAL_CODE === scope.selmandal;
				}).LGD_MANDAL_NAME;
				var gpname = $.grep(scope.PanchayatDD, function (b) {
					return b.LGD_PANCHAYAT_CODE === scope.selpanchayath;
				}).LGD_PANCHAYAT_NAME;
				scope.preloader = true;
				var req =
					{
						DISTRICTID: scope.seldistrict,
						DISTRICTNAME: distname,
						MANDALID: scope.selmandal,
						MANDALNAME: manname,
						PANCHAYATID: scope.selpanchayath,
						PANCHAYATNAME: gpname,
						RUFLAG: scope.selruflag,
						SECRETRIATNAME: scope.secretriatname,
						SECRETRIATID:scope.secretriatid
					}
				Internal_Services.DemoAPI("PostSecretriatData", req, function (value) {
					if (value.data.Status == "100") {
						scope.preloader = false;
						//scope.benstatus = value.data["Details"];
						alert("Data Inserted Successfully");
						window.location.reload();
					}
					else {
						scope.preloader = false;
						alert("Data Submission Failed");
					}
				});
			}
		}

		scope.loaddistrict = function () {
			scope.selruflag = "";
			scope.selmandal = "";
			scope.selpanchayath = "";
			scope.secretriatname = "";
			scope.secretriatid = "";
			scope.MandalsDD = [];
			scope.PanchayatDD = [];
		}
		
		function LoadDistrics() {
			var req = {
				TYPE: "4"
			};
			Internal_Services.DemoAPI("LoadDepartments", req, function (value) {
				if (value.data.Status == "Success") {
					scope.DistricsDD = value.data.Details;
				}
				else {
					alert("Districs Loading Failed");
				}
			});
		}

		// Load Madals's
		scope.LoadMadals = function () {

			scope.selmandal = "";
			scope.selpanchayath = "";
			scope.MandalsDD = [];
			scope.PanchayatDD = [];
			scope.secretriatname = "";
			scope.secretriatid = "";
			var req = {
				TYPE: "5",
				DISTRICT: scope.seldistrict,
				RUFLAG: scope.selruflag
			};
			Internal_Services.DemoAPI("LoadDepartments", req, function (value) {
				if (value.data.Status == "Success") {
					scope.MandalsDD = value.data.Details;
				}
				else {
					alert("Mandals Loading Failed");
				}
			});
		}

		// Load Panchayats's
		scope.LoadPachayats = function () {
			scope.selpanchayath = "";
			scope.PanchayatDD = [];
			scope.secretriatname = "";
			scope.secretriatid = "";
			var req = {
				TYPE: "6",
				DISTRICT: scope.seldistrict,
				MANDAL: scope.selmandal,
				RUFLAG: scope.selruflag
			};
			Internal_Services.DemoAPI("LoadDepartments", req, function (value) {
				if (value.data.Status == "Success") {
					scope.PanchayatDD = value.data.Details;
				}
				else {
					alert("Panchayats Loading Failed");
				}
			});
		}

		//Change in access level
		scope.changeaccesslvl = function () {
			if (scope.selaccesslvl == "0") {
				scope.divdistrict = false;
				scope.divmandal = false;
				scope.divvillage = false;
			} if (scope.selaccesslvl == "1") {
				scope.divdistrict = true;
				scope.divmandal = false;
				scope.divvillage = false;
			} if (scope.selaccesslvl == "2") {
				scope.divdistrict = true;
				scope.divmandal = true;
				scope.divvillage = false;
			} if (scope.selaccesslvl == "3") {
				scope.divdistrict = true;
				scope.divmandal = true;
				scope.divvillage = true;
			}
		}
	};
})();