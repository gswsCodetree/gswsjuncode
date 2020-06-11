(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("InternalController", ["$scope", "$state", "$log", "Internal_Services", Internal_CTRL]);

	function Internal_CTRL(scope, state, log, Internal_Services) {
		scope.preloader = false;
		LoadDepartments();
		LoadDistrics();

		//Submit
		scope.Savedata = function () {
			if (!(scope.selDept)) {
				alert("Please Select Department Name");
				return;
			}
			else if (!(scope.selHOD)) {
				swal("Please Select HOD Name");
				return;
			}
			else if (!(scope.selservice)) {
				alert("Please Select Service Name");
				return;
			}
			else if (!(scope.reqtype)) {
				alert("Please Select Request Type");
				return;
			}
			else if (!(scope.Enteredurl)) {
				alert("Please Enter URL");
				return;
			}
			else if (!(scope.Enteredurldesc)) {
				alert("Please Enter URL Description");
				return;
			}
			else if (!(scope.selservicetype)) {
				alert("Please Select Service Type");
				return;
			}
			else if (!(scope.selruraldesignation)) {
				alert("Please Select Rural Designation");
				return;
			}
			else if (!(scope.selurbandesignation)) {
				alert("Please Select Urban Designation");
				return;
			}
			else if (!(scope.selaccesslvl)) {
				alert("Please Select Access Level");
				return;
			}
			else if (scope.selaccesslvl == "1" && !(scope.seldistrict)) {
				alert("Please Select District Name");
				return;
			}
			else if (scope.selaccesslvl == "2" && !(scope.selmandal)) {
				alert("Please Select Mandal Name");
				return;
			}
			else if (scope.selaccesslvl == "3" && !(scope.selpanchayath)) {
				alert("Please Select Panchayath Name");
				return;
			}
			else {
				scope.preloader = true;
				var req =
					{
						TYPE:"1",
						DEPARTMENT: scope.selDept,
						HOD: scope.selHOD,
						SERVICE: scope.selservice,
						REQUESTTYPE: scope.reqtype,
						URL_ID: scope.Enteredurl,
						URL: scope.Enteredurl,
						URLDESCRIPTION: scope.Enteredurldesc,
						USERNAME: scope.username,
						PASSWORD: scope.password,
						ENCRYPT_PASSWORD: scope.password,
						SERVICETYPE: scope.selservicetype,
						ACCESSLEVEL: scope.selaccesslvl,
						DISTRICT: scope.seldistrict,
						MANDAL: scope.selmandal,
						PANCHAYAT: scope.selpanchayath,
						RUFLAG: scope.selruflag,
						P_URL_DESC_TEL: scope.EnteredTeluguurldesc,

						RURALDESIGNATION: scope.selruraldesignation,
						URBANDESIGNATION: scope.selurbandesignation
					}
				Internal_Services.DemoAPI("SaveServicesURLData", req, function (value) {
					if (value.data.Status == "Success") {
						scope.preloader = false;
						scope.benstatus = value.data["Details"];
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

		//Load Department's
		function LoadDepartments() {
			var req = {
				TYPE:"1"
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
					alert("Departmets Loading Failed");
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
					alert("Departmets Loading Failed");
				}
			});
		}

		// Load District's
		function LoadDistrics() {
			var req = {
				TYPE:"4"
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