(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("VillageProfile", ["$scope", "$state", "$log", "AgriCulture_Services", Vil_CTRL]);

    function Vil_CTRL(scope, state, log, vol_services) {
     
        scope.pagename = "EPanta Village Profile";
        scope.preloader = false;
        scope.dttable = false;
        scope.MandalDD = [];
        scope.VillageDD = [];
        scope.inputradio = "option1";
        scope.divsurvey = false;
        scope.divaadhar = false;

        var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}
		LoadMaster();
        scope.changeOption = function (type) {
            scope.dttable = false;
            scope.VilDetails = [];
            scope.MandalDD = [];
            scope.VillageDD = [];

            scope.seldistrict = null;
            scope.selsurveyNo = "";
            scope.selaadhaar = "";

            if (type == "option2") {
                scope.divsurvey = true;
                scope.divaadhar = false;
            }
            else if (type == "option3") {
                scope.divsurvey = false;
                scope.divaadhar = true;
            }
            else {
                scope.divsurvey = false;
                scope.divaadhar = false;
            }
        }

        scope.LoadMandal = function () {
            scope.VillageDD = [];
            scope.MandalDD = $(scope.RevenuMasterDD).filter(function (i, n) { return n.DCode === scope.seldistrict.DCode; });

        }

        scope.LoadVillages = function () {
            scope.VillageDD = $(scope.RevenuMasterDD).filter(function (i, n) { return n.Mcode === scope.selmandal.Mcode && n.DCode === scope.selmandal.DCode; });
        }

        scope.GetDetails = function (type) {
            if (!scope.seldistrict) {
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
            else if (type == "option2" && !scope.selsurveyNo) {
                swal('info', "Please Enter Survey Number", 'info');
                return false;
            }
            else if (type == "option3" && !scope.selaadhaar) {
                swal('info', "Please Enter Aadhaar Number", 'info');
                return false;
            }
            else if (type == "option3" && scope.selaadhaar.toString().length != "12") {
                swal('info', "Aadhaar Number should be 12 digits.", 'info');
                return false;
            }
            else if (type == "option3" && (scope.selaadhaar == "000000000000" || scope.selaadhaar == "111111111111" || scope.selaadhaar == "222222222222" || scope.selaadhaar == "333333333333" || scope.selaadhaar == "444444444444" || scope.selaadhaar == "555555555555" || scope.selaadhaar == "666666666666" || scope.selaadhaar == "777777777777" || scope.selaadhaar == "888888888888" || scope.selaadhaar == "999999999999")) {
                swal('info', "Please Enter Valid Aadhaar Number", 'info');
                return false;
            }
            else {
                scope.preloader = true;
                var req = {};
                var MethodName = "";

                if (type == "option1") {
                    req = { District: scope.seldistrict.DCode, Mandal: scope.selmandal.Mcode, Village: scope.selvill.Vcode };
                    MethodName = "GetVillageDetails";
                }
                else if (type == "option2") {
                    req = { District: scope.seldistrict.DCode, Mandal: scope.selmandal.Mcode, Village: scope.selvill.Vcode, SurveyNo: scope.selsurveyNo };
                    MethodName = "GetVillageDetailsByServey";
                }
                else if (type == "option3") {
                    req = { District: scope.seldistrict.DCode, Mandal: scope.selmandal.Mcode, Village: scope.selvill.Vcode, Aadhar: scope.selaadhaar };
                    MethodName = "GetVillageDetailsByAadhaar";
                }

				vol_services.POSTENCRYPTAPIAGRI(MethodName, req, token, function (value) {
                    //vol_services.POSTENCRYPTAPIAGRI(MethodName, req, token, function (value) {
					scope.VilDetails = [];
					scope.preloader = false;
                    if (value.data.Status == 100) {
                        var data = value.data.Details.ServiceData;
                        if (data != "No Data Available!!!") {

                            scope.VilDetails = data;
                            scope.dttable = true;
						}
						else if (value.data.Status == "428") {
							sessionStorage.clear();
							state.go("Login");
							return;
						}
                        else
                            swal('info', data, 'info');

                        console.log(data);

                    }
                    else { swal('info', value.data.Reason, 'info'); }

                    scope.preloader = false;

                });

            }
        }


		function LoadMaster() {

			vol_services.POSTENCRYPTAPIAGRI("AgricultureserviceMaster", "", token, function (value) {
				
					if (value.data.Status == 100) {

						scope.RevenuMasterDD = JSON.parse(value.data.RevenuMaster);
					}
					else {
						swal('info', value.data.Reason, 'info');
						return;
					}
				});
		}


       

    }
})();