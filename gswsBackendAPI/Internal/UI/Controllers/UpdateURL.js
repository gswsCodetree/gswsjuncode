(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("InternalUpdateURLController", ["$scope", "$state", "$log", "Internal_Services", Internal_CTRL]);

	function Internal_CTRL(scope, state, log, Internal_Services) {
		scope.preloader = false;
		LoadDepartments();
		var username = localStorage.getItem("user");


		//Submit
		scope.Savedata = function () {

			if (scope.selDept == null || scope.selDept == undefined || scope.selDept == "") {
				swal('info', 'Please Select Department', 'info');
				return;
			}

			if (scope.selHOD == null || scope.selHOD == undefined || scope.selHOD == "") {
				swal('info', 'Please Select HOD', 'info');
				return;
			}

			if (scope.selservice == null || scope.selservice == undefined || scope.selservice == "") {
				swal('info', 'Please Select Service name', 'info');
				return;
			}

			if (scope.selurlid == null || scope.selurlid == undefined || scope.selurlid == "") {
				swal('info', 'Please Select Url Description', 'info');
				return;
			}

			if (scope.updatedurl == undefined || scope.updatedurl == null || scope.updatedurl == "") {

				swal('info', 'Please Enter New URL', 'info');
				return;
			}

			var req = {
				DEPTID: scope.selDept,
				HODID: scope.selHOD,
				SERVICEID: scope.selservice,
				URL_ID: scope.selurlid,
				NEWURL: scope.updatedurl,
				USER: username,

				TELUGUDESCRIPTION: scope.telugudescription,
				ENGLISHDESCRIPTION: scope.englishdescription
			}
			Internal_Services.DemoAPI("UpdateURL", req, function (value) {
				if (value.data.Status == "100") {
					swal('info', value.data.Reason, 'info');
					window.location.reload();
				}
				else {
					swal('info', value.data.Reason, 'error');
				}
			});
		}

		//Load Department's
		function LoadDepartments() {
			var req = {
				TYPE: "1"
			};
			Internal_Services.DemoAPI("LoadDepartments", req, function (value) {
				if (value.data.Status == "Success") {
					scope.DepartmentDD = value.data.Details;
				}
				else {
					alert("Departmets Loading Failed");
				}
			});
		}

		//Load HOD's
		scope.LoadHODs = function () {
			var req = {
				TYPE: "2",
				DEPARTMENT: scope.selDept
			};
			Internal_Services.DemoAPI("LoadDepartments", req, function (value) {
				if (value.data.Status == "Success") {
					scope.HODDD = value.data.Details;
				}
				else {
					alert("HOD Loading Failed");
				}
			});
		}

		//Load Services
		scope.LoadServices = function () {
			var req = {
				TYPE: "7",
				DEPARTMENT: scope.selDept,
				HOD: scope.selHOD
			};
			Internal_Services.DemoAPI("LoadDepartments", req, function (value) {
				if (value.data.Status == "Success") {
					scope.ServicesDD = value.data.Details;
				}
				else {
					alert("Services Loading Failed");
				}
			});
		}

		//Load Service URL's
		scope.LoadServicesurls = function () {
			var req = {
				TYPE: "10",
				DEPARTMENT: scope.selDept,
				HOD: scope.selHOD,
				DISTRICT: scope.selservice
			};
			Internal_Services.DemoAPI("LoadDepartments", req, function (value) {
				if (value.data.Status == "Success") {
					scope.URLLIST = value.data.Details;
				}
				else {
					alert("Service Url names Loading Failed");
				}
			});
		}

		//Chanage URL Name
		scope.changeurlname = function () {
			for (var i = 0; i <= scope.URLLIST.length; i++) {
				if (scope.URLLIST[i]["URL_ID"] == scope.selurlid) {
					scope.englishdescription = scope.URLLIST[i]["URL_ENGLISH_DES"];
					scope.telugudescription = scope.URLLIST[i]["URL_TELUGU_DES"];
					scope.updatedurl = scope.URLLIST[i]["URL"];
				}
			}

		}

	};
})();