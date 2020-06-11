(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("SecToVolunteerMap", ["$scope", "$state", "$log", "PRRD_Services", Vol_CTRL]);

	function Vol_CTRL(scope, state, log, sec_services) {
		scope.pagename = "Secretriat to Volunteer Mapping";
		scope.preloader = false;
		scope.dttable = false;

		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return false;
		}

		//LoadDistricts();
		LoadData();

		scope.GetDatails = function () {
			if (!scope.SelDistrict) {
				swal('info', "Please Select District.", 'info');
				return false;
			}
			else if (!scope.SelMandal) {
				swal('info', "Please Select Mandal.", 'info');
				return false;
			}
			else if (!scope.SelSec) {
				swal('info', "Please Select Secretriat.", 'info');
				return false;
			}
			else {
				scope.preloader = true;
				scope.dttable = false;
				var req = { ftype: 1, fvv_id: scope.volunteerid };
				sec_services.POSTENCRYPTAPI("GetValunteerDetails", req, token, function (value) {
					scope.StudentDetails = [];
					if (value.data.Status == 100) {
						scope.VolDetails = value.data.Details;
						console.log(value.data.Details);
						scope.dttable = true;
					}
					else if (value.data.Status == "428") {
						sessionStorage.clear();
						swal("info", "Session Expired !!!", "info");
						state.go("Login");
						return;
					}
					else { swal('info', value.data.Reason, 'info'); }

					scope.preloader = false;

				});

			}
		}

		scope.GetVVDetails = function (id) {
			var req = { TYPE: "2", VVID:id };
			sec_services.POSTENCRYPTAPI("SecretriattoVoluteerData", req, token, function (value) {
				scope.VDetails = [];
				if (value.data.Status == 100) {
					scope.VDetails = value.data.Details;
					//console.log(value.data.Details);
					scope.dttablev = true;
					scope.dttable = false;
				}
				else if (value.data.Status == "428") {
					sessionStorage.clear();
					swal("info", "Session Expired !!!", "info");
					state.go("Login");
					return;
				}
				else {
					scope.dttablev = false;
					scope.dttable = true;
					swal('info', value.data.Reason, 'info');
				}

				scope.preloader = false;

			});
		}

		scope.GetHHDetails = function (hid) {
			var req = { TYPE: "3", HHID: hid };
			sec_services.POSTENCRYPTAPI("SecretriattoVoluteerData", req, token, function (value) {
				scope.model.hhDetails = [];
				if (value.data.Status == 100) {
					scope.model.hhDetails = value.data.Details;
					//console.log(value.data.Details);
					scope.dttablehh = true;
					scope.dttablev = false;
					scope.dttable = false;
				}
				else if (value.data.Status == "428") {
					sessionStorage.clear();
					swal("info", "Session Expired !!!", "info");
					state.go("Login");
					return;
				}
				else {
					scope.dttablehh = false;
					scope.dttablev = true;
					scope.dttable = false;
					swal('info', value.data.Reason, 'info');
				}

				scope.preloader = false;

			});
		}
		function LoadData() {
			var req = { TYPE: "1", DISTRICTCODE: "12", MANDALCODE: "1234", SECRETRIATCODE: "4321" };
			sec_services.POSTENCRYPTAPI("SecretriattoVoluteerData", req, token, function (value) {
				scope.StudentDetails = [];
				if (value.data.Status == 100) {
					scope.VolDetails = value.data.Details;
					console.log(value.data.Details);
					scope.dttable = true;
				}
				else if (value.data.Status == "428") {
					sessionStorage.clear();
					swal("info", "Session Expired !!!", "info");
					state.go("Login");
					return;
				}
				else { swal('info', value.data.Reason, 'info'); }

				scope.preloader = false;

			});
		}

		scope.selectAll = function () {
			// Loop through all the entities and set their isChecked property
			for (var i = 0; i < scope.model.hhDetails.length; i++) {
				scope.model.hhDetails[i].isChecked = scope.model.allItemsSelected;
			}
			if (scope.model.allItemsSelected) {
				scope.divbtn = true;
			}
			else {
				scope.divbtn = false;
			}
		};
		scope.model = {};
		scope.model.allItemsSelected = false;
	  scope.selectEntity = function () {
			// If any entity is not checked, then uncheck the "allItemsSelected" checkbox
		  for (var i = 0; i < scope.model.hhDetails.length; i++) {
			  if (!scope.model.hhDetails[i].isChecked) {
					scope.model.allItemsSelected = false;
					return;
				}
			}

			//If not the check the "allItemsSelected" checkbox
		  scope.model.allItemsSelected = true;
		  scope.divbtn = true;


		};

		scope.GetApproval = function () {
			scope.divremarks = false;
			scope.divapp = true;
			$("#mobile-modal").modal('show');
			return;
		}
		scope.GetReject = function () {
			scope.divremarks = true;
			scope.divapp = false;
			$("#mobile-modal").modal('show');
			return;
		}
		scope.GetSubmit = function () {
			var req = { TYPE: "4", HHlist: scope.model.hhDetails, LoginUser: user, Remarks: scope.remarks };
			sec_services.POSTENCRYPTAPI("SecctoVolunteerApprovalData", req, token, function (value) {
				//scope.VDetails = [];
				if (value.data.Status == 100) {
					swal('info', value.data.Reason, 'info'); 
					window.location.reload();
				}
				else if (value.data.Status == "428") {
					sessionStorage.clear();
					swal("info", "Session Expired !!!", "info");
					state.go("Login");
					return;
				}
				else { swal('info', value.data.Reason, 'info'); }
			});
		}

		function LoadDistricts() {
			var obj = { FTYPE: 1 }
			sec_services.WebPOSTENCRYPTAPI("GetGSWSSecretariatMaster", obj, token, function (value) {
				console.log(value.data.DataList);
				if (value.data.Status == 100) {
					scope.distlist = value.data.DataList;
				}
				else if (value.data.Status == "428") {
					sessionStorage.clear();
					swal("info", "Session Expired !!!", "info");
					state.go("Login");
					return;
				}
				else { swal('info', value.data.Reason, 'info'); }

				scope.preloader = false;

			});
		}

		function loadmandalmaster() {

			var input = { FTYPE: 5, DISTCODE: scope.distcode, RUFLAG: scope.ruflag }

			Login_Services.POSTENCRYPTAPI("GetLGDMaster", input, token, function (value) {

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

			Login_Services.POSTENCRYPTAPI("GetLGDMaster", input, token, function (value) {

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