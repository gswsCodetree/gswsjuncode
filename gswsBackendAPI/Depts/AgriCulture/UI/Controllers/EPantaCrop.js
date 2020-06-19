(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("EPantaCrop", ["$scope", "$state", "$window", "$log", "AgriCulture_Services", Crop_CTRL]);

	function Crop_CTRL(scope, state, window, log, vol_services) {

		scope.pagename = "EPanta Village Profile";
		scope.preloader = false;
		// scope.dttable = false;
		scope.MandalDD = [];
		scope.VillageDD = [];
		scope.inputradio = "option1";
		scope.divsurvey = false;
		scope.divaadhar = false;
		scope.divcrop = false;
		//window.scrollTo(0, 200);
		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}
		loadcropdetails();
		loaddistmaster();
		scope.GetMandalload = function () {
			scope.mcode = "";
			scope.villagecode = "";
			loadmandalmaster();
		}
		scope.GetVillageload = function () {
			scope.villagecode = "";
			loadvillagemaster();
		}
		//LoadMaster();
		scope.changeOption = function (type) {
			//scope.dttable = false;
			scope.VilDetails = [];
			scope.MandalDD = [];
			scope.VillageDD = [];

			scope.seldistrict = null;
			scope.selsurveyNo = "";
			scope.selaadhaar = "";
			document.getElementById('dttable').style.display = "none";
			//if (type == "option2") {
			//    scope.divsurvey = true;
			//    scope.divaadhar = false;
			//    scope.divcrop = false;
			//}
			//else if (type == "option3") {
			//    scope.divsurvey = false;
			//    scope.divaadhar = true;
			//    scope.divcrop = false;
			//}
			if (type == "option4") {
				scope.divsurvey = false;
				scope.divaadhar = false;
				scope.divcrop = true;
			}
			else {
				scope.divsurvey = false;
				scope.divaadhar = false;
				scope.divcrop = false;
			}
		}

		//scope.LoadMandal = function () {
		//    scope.VillageDD = [];
		//    scope.MandalDD = $(scope.RevenuMasterDD).filter(function (i, n) { return n.DCode === scope.seldistrict.DCode; });

		//}

		//scope.LoadVillages = function () {
		//    scope.VillageDD = $(scope.RevenuMasterDD).filter(function (i, n) { return n.Mcode === scope.selmandal.Mcode && n.DCode === scope.selmandal.DCode; });
		//}

		scope.GetDetails = function (type) {
			if (!scope.distcode) {
				swal('info', "Please Select District", 'info');
				return false;
			}
			else if (!scope.selmandal) {
				swal('info', "Please Select Mandal", 'info');
				return false;
			}
			else if (!scope.selvill) {
				swal('info', "Please Select Village", 'info');
				return false;
			}
			else if (type == "option4" && !scope.cropcode) {
				swal('info', "Please Select Crop", 'info');
				return false;
			}

			//else if (type == "option2" && !scope.selsurveyNo) {
			//    swal('info', "Please Enter Survey Number", 'info');
			//    return false;
			//}
			//else if (type == "option3" && !scope.selaadhaar) {
			//    swal('info', "Please Enter Aadhaar Number", 'info');
			//    return false;
			//}
			//else if (type == "option3" && scope.selaadhaar.toString().length != "12") {
			//    swal('info', "Aadhaar Number should be 12 digits.", 'info');
			//    return false;
			//}
			//else if (type == "option3" && (scope.selaadhaar == "000000000000" || scope.selaadhaar == "111111111111" || scope.selaadhaar == "222222222222" || scope.selaadhaar == "333333333333" || scope.selaadhaar == "444444444444" || scope.selaadhaar == "555555555555" || scope.selaadhaar == "666666666666" || scope.selaadhaar == "777777777777" || scope.selaadhaar == "888888888888" || scope.selaadhaar == "999999999999")) {
			//    swal('info', "Please Enter Valid Aadhaar Number", 'info');
			//    return false;
			//}
			else {
				scope.preloader = true;
				var req = {};
				var MethodName = "";

				if (type == "option1") {

					var CropDetails = { deptcode: "6", dcode: scope.distcode, mcode: scope.selmandal, vcode: scope.selvill }
					var req = {
						deptId: "1234",
						deptName: "Agriculture",
						serviceName: "e-Panta _CropBookingFarmersDetails",
						serviceType: "REST",
						method: "POST",
						simulatorFlag: "false",
						application: "GWS",
						username: sessionStorage.getItem("user"),
						userid: sessionStorage.getItem("user"),
						data: CropDetails
					};
					MethodName = "GetFarmerDetailsByVil";
				}
				else if (type == "option4") {
					//if (!scope.cropcode) {
					//    swal('info', "Please Select Crop", 'info');
					//    return false;
					//}
					var CropDetails = { deptcode: "6", dcode: scope.distcode, mcode: scope.selmandal, vcode: scope.selvill, cropcode: scope.cropcode }
					var req = {
						deptId: "1234",
						deptName: "Agriculture",
						serviceName: "e-Panta _CropBookingFarmersDetails",
						serviceType: "REST",
						method: "POST",
						simulatorFlag: "false",
						application: "GWS",
						username: sessionStorage.getItem("user"),
						userid: sessionStorage.getItem("user"),
						data: CropDetails
					};

					MethodName = "GetFarmerDetails";
				}
				//else if (type == "option2") {
				//    req = {
				//        District: scope.seldistrict.DCode, Mandal: scope.selmandal.Mcode, Village: scope.selvill.Vcode, SurveyNo: scope.selsurveyNo, UserId: sessionStorage.getItem("user"),
				//        SacId: sessionStorage.getItem("secccode"),
				//        DesignId: sessionStorage.getItem("desinagtion"),
				//        TranId: scope.seldistrict.DCode
				//    };
				//    MethodName = "GetVillageDetailsByServey";
				//}
				//else if (type == "option3") {
				//    req = {
				//        District: scope.seldistrict.DCode, Mandal: scope.selmandal.Mcode, Village: scope.selvill.Vcode, Aadhar: scope.selaadhaar, UserId: sessionStorage.getItem("user"),
				//        SacId: sessionStorage.getItem("secccode"),
				//        DesignId: sessionStorage.getItem("desinagtion"),
				//        TranId: scope.seldistrict.DCode
				//    };
				//    MethodName = "GetVillageDetailsByAadhaar";
				//}

				vol_services.DemoAPI(MethodName, req, function (value) {
					debugger;
					scope.VilDetails = [];
					var res = value.data;
					scope.preloader = false;
					if (res.Status == 100 && JSON.parse(res.REASON)["response"].status == "1") {
						//if (JSON.parse(res.REASON)["response"].status == "1") {
						//scope.dttable = true;
						document.getElementById('dttable').style.display = "block";
						scope.VilDetails = JSON.parse(res.REASON)["response"].farmerList;
						window.scrollTo(0, 170);
					}
					else if (res.Status == 100 && JSON.parse(res.REASON)["response"].status == "0") {
						//scope.dttable = false;
						document.getElementById('dttable').style.display = "none";
						swal('info', "No Data Found.", 'info');

					}
					//if (data != "No Data Available!!!") {

					//    scope.VilDetails = data;
					//    scope.dttable = true;
					//}
					//else if (value.data.Status == "428") {
					//    sessionStorage.clear();
					//    state.go("Login");
					//    return;
					//}
					//else
					//    swal('info', data, 'info');

					//console.log(data);
					//}
					else { swal('info', value.data.Reason, 'info'); }


					scope.preloader = false;

				});

			}
		}


		//function LoadMaster() {

		//    vol_services.POSTENCRYPTAPIAGRI("AgricultureserviceMaster", "", token, function (value) {

		//        if (value.data.Status == 100) {

		//            scope.RevenuMasterDD = JSON.parse(value.data.RevenuMaster);
		//        }
		//        else {
		//            swal('info', value.data.Reason, 'info');
		//            return;
		//        }
		//    });
		//}
		function loadcropdetails() {

			var input = {

				deptId: "1234",
				deptName: "Agriculture",
				serviceName: "e-Panta_CropDetails",
				serviceType: "REST",
				method: "POST",
				simulatorFlag: "false",
				application: "GWS",
				username: sessionStorage.getItem("user"),
				userid: sessionStorage.getItem("user"),
				data: { "deptcode": "6" }
			};
			vol_services.DemoAPI("GetCropDetails", input, function (value) {

				var res = value.data;
				if (res.Status == "100") {

					scope.Cropdetails = JSON.parse(res.REASON)["response"].mandalDetails;
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


		function loaddistmaster() {
			var input = {
				deptId: "1234",
				deptName: "Agriculture",
				serviceName: "e-Karshak_DistrictDetails",
				serviceType: "REST",
				method: "POST",
				simulatorFlag: "false",
				application: "GWS",
				username: sessionStorage.getItem("user"),
				userid: sessionStorage.getItem("user"),
				data: { "deptcode": "6" }
			};

			vol_services.DemoAPI("AgriMaster", input, function (value) {
				var res = value.data;
				if (res.Status == "100") {

					scope.distlist = JSON.parse(res.REASON)["response"].districtDetails;
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
			if (!(scope.distcode)) {
				scope.Mandallist = "";
				scope.Villageslist = "";
				alert('Select District');
			}
			var distDetails = { deptcode: "6", dcode: scope.distcode }
			var input = {
				deptId: "1234",
				deptName: "Agriculture",
				serviceName: "e-Karshak_MandalDetails",
				serviceType: "REST",
				method: "POST",
				simulatorFlag: "false",
				application: "GWS",
				username: sessionStorage.getItem("user"),
				userid: sessionStorage.getItem("user"),
				data: distDetails
			};

			vol_services.DemoAPI("AgriMaster", input, function (value) {
				var res = value.data;
				if (res.Status == "100") {

					scope.Mandallist = JSON.parse(res.REASON)["response"].mandalDetails;
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
		function loadvillagemaster() {
			if (!(scope.selmandal)) {
				scope.Villageslist = "";
				alert('Select Mandal');
			}
			var distDetails = { deptcode: "6", dcode: scope.distcode, mcode: scope.selmandal }
			var input = {
				deptId: "1234",
				deptName: "Agriculture",
				serviceName: "e-Panta_VillageDetails",
				serviceType: "REST",
				method: "POST",
				simulatorFlag: "false",
				application: "GWS",
				username: sessionStorage.getItem("user"),
				userid: sessionStorage.getItem("user"),
				data: distDetails
			};

			vol_services.DemoAPI("AgriMaster", input, function (value) {
				var res = value.data;
				if (res.Status == "100") {

					scope.Villageslist = JSON.parse(res.REASON)["response"].villageDetails;
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