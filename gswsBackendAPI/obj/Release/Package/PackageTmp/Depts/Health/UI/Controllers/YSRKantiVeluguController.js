(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("YSRKantiVeluguController", ["$scope", "$state", "$log", "Health_Services", Health_CTRL]);

    function Health_CTRL(scope, state, log, health_services) {
		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}
        scope.pagename = "YSR Kanti Velugu";
        scope.preloader = false;
        scope.dttable = false;
      //  var token = sessionStorage.getItem("Token");
        scope.GetStatus = function () {
            if (!scope.studentid) {
                alert("Please Enter Student ID.");
                return;
            }

            else {
                scope.preloader = true;
                scope.dttable = false;
                var req = { student_id: scope.studentid };
                //health_services.DemoAPI("GetKantiVeluguStatus", req, function (value) {
                health_services.POSTENCRYPTAPI("GetKantiVeluguStatus", req, token, function (value) {
                    scope.StudentDetails = [];
                    if (value.data.Status == 100) {
                        if (value.data.Details.length > 0) {
                            //scope.StudentDetails = value.data.Details;
                            var stddata = value.data.Details[0];
                            scope.CHILD_NAME = stddata.CHILD_NAME;
                            scope.STUDYINGCLASS = stddata.STUDYINGCLASS;
                            scope.CONCLUSION = stddata.CONCLUSION;
                            scope.DISTRICTNAME = stddata.DISTRICTNAME;
                            scope.MANDALNAME = stddata.MANDALNAME;
                            scope.VILLAGENAME = stddata.VILLAGENAME;
                            scope.GENDER1 = stddata.GENDER1;
                            scope.SCHNAME = stddata.SCHNAME;
                            scope.SCHOOLCOMPLEXNAME = stddata.SCHOOLCOMPLEXNAME;

                            scope.CC_R_BLURRED = stddata.CC_R_BLURRED;
                            scope.CC_R_PAIN = stddata.CC_R_PAIN;
                            scope.CC_R_RED = stddata.CC_R_RED;
                            scope.CC_R_OTHERS = stddata.CC_R_OTHERS;
                            scope.CC_L_BLURRED = stddata.CC_L_BLURRED;
                            scope.CC_L_PAIN = stddata.CC_L_PAIN;
                            scope.CC_L_RED = stddata.CC_L_RED;
                            scope.CC_L_OTHERS = stddata.CC_L_OTHERS;

                            scope.ASE_R_SQUINT = stddata.ASE_R_SQUINT;
                            scope.ASE_R_CONG_CATARACT = stddata.ASE_R_CONG_CATARACT;
                            scope.ASE_R_NYSTAGMUS = stddata.ASE_R_NYSTAGMUS;
                            scope.ASE_R_ALLRGC_CONJ = stddata.ASE_R_ALLRGC_CONJ;
                            scope.ASE_R_OTHERS = stddata.ASE_R_OTHERS;
                            scope.ASE_L_SQUINT = stddata.ASE_L_SQUINT;
                            scope.ASE_L_CONG_CATARACT = stddata.ASE_L_CONG_CATARACT;
                            scope.ASE_L_NYSTAGMUS = stddata.ASE_L_NYSTAGMUS;
                            scope.ASE_L_ALLRGC_CONJ = stddata.ASE_L_ALLRGC_CONJ;
                            scope.ASE_L_OTHERS = stddata.ASE_L_OTHERS;

                            scope.VAT_DV_R_WITHGLASS = stddata.VAT_DV_R_WITHGLASS;
                            scope.VAT_DV_R_WITHOUTGLASS = stddata.VAT_DV_R_WITHOUTGLASS;
                            scope.VAT_DV_R_PINHOLE = stddata.VAT_DV_R_PINHOLE;
                            scope.VAT_NV_R_VISION = stddata.VAT_NV_R_VISION;
                            scope.VAT_DV_L_WITHGLASS = stddata.VAT_DV_L_WITHGLASS;
                            scope.VAT_DV_L_WITHOUTGLASS = stddata.VAT_DV_L_WITHOUTGLASS;
                            scope.VAT_DV_L_PINHOLE = stddata.VAT_DV_L_PINHOLE;
                            scope.VAT_NV_L_VISION = stddata.VAT_NV_L_VISION;

                            scope.RBL_PSA = stddata.RBL_PSA;
                            scope.P_SPH_R = stddata.P_SPH_R;
                            scope.SA_SPH_R = stddata.SA_SPH_R;
                            scope.SA_CYL_R = stddata.SA_CYL_R;
                            scope.SA_AXIS_R = stddata.SA_AXIS_R;
                            scope.SA_SPH_L = stddata.SA_SPH_L;
                            scope.SA_CYL_L = stddata.SA_CYL_L;
                            scope.SA_AXIS_L = stddata.SA_AXIS_L;

                            scope.SC_DV_VBC_R = stddata.SC_DV_VBC_R;
                            scope.SC_DV_R_SPH = stddata.SC_DV_R_SPH;
                            scope.SC_DV_R_CYL = stddata.SC_DV_R_CYL;
                            scope.SC_DV_R_AXIS = stddata.SC_DV_R_AXIS;
                            scope.SC_DV_R_VISION = stddata.SC_DV_R_VISION;
                            scope.SC_DV_VBC_L = stddata.SC_DV_VBC_L;
                            scope.SC_DV_L_SPH = stddata.SC_DV_L_SPH;
                            scope.SC_DV_L_CYL = stddata.SC_DV_L_CYL;
                            scope.SC_DV_L_AXIS = stddata.SC_DV_L_AXIS;
                            scope.SC_DV_L_VISION = stddata.SC_DV_L_VISION;
                            scope.SC_NV_VBC_R = stddata.SC_NV_VBC_R;
                            scope.SC_NV_R_SPH = stddata.SC_NV_R_SPH;
                            scope.SC_NV_R_CYL = stddata.SC_NV_R_CYL;
                            scope.SC_NV_R_AXIS = stddata.SC_NV_R_AXIS;
                            scope.SC_NV_R_VISION = stddata.SC_NV_R_VISION;
                            scope.SC_NV_VBC_L = stddata.SC_NV_VBC_L;
                            scope.SC_NV_L_SPH = stddata.SC_NV_L_SPH;
                            scope.SC_NV_L_CYL = stddata.SC_NV_L_CYL;
                            scope.SC_NV_L_AXIS = stddata.SC_NV_L_AXIS;
                            scope.SC_NV_L_VISION = stddata.SC_NV_L_VISION;

                            scope.IS_SPECTS_REQ = stddata.IS_SPECTS_REQ;
                            scope.SPECTS_FRAME_SIZE = stddata.SPECTS_FRAME_SIZE;
                            scope.SPECTS_FRAME_COLOR = stddata.SPECTS_FRAME_COLOR;
                            scope.MEDICATION = stddata.MEDICATION;
                            scope.NAME_OF_REF_INST = stddata.NAME_OF_REF_INST;
                            scope.REFFERED_FOR = stddata.REFFERED_FOR;

                            scope.vtype = (scope.RBL_PSA == "PHOROPTER" ? true : false);

                            scope.dttable = true;
                        }
                        else { scope.dttable = false; swal('info', "No Data Found", 'info'); }


					}
					else if (value.data.Status == "428") {
						sessionStorage.clear();
						swal('info', value.data.Reason, 'info');
						state.go("Login");
						return;
					}
                    else {

                        swal('info', value.data.Reason, 'info');
                    }

                    scope.preloader = false;

                });
            }
        }
    };
})();