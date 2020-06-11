(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("TextileCntrl", ["$scope", "$state", "$log", "Ser_Services", Services_CTRL]);

    function Services_CTRL(scope, state, log, ser_services) {
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

        scope.pagename = "Textile";
        scope.submittedby = sessionStorage.getItem("user");
        scope.preloader = false;
        scope.step_position = 1;
        scope.finalsave = false;
        scope.finalsavevalue == "0"
        LoadDistrics("0");

        //Get Data
        scope.getdata = function () {
            if (scope.uid.length == "12") {
                var req = {
                    fadhar_no: scope.uid,
                    ftype: "1"
                };
                ser_services.POSTENCRYPTAPI("GetTextileData", req, token, function (value) {
                    if (value.data.Status == "Success" && value.data.Details[0]['RATION_STATUS'] == "0") {
                        scope.finalsavevalue = "1";
                        scope.appname = value.data.Details[0]['CITIZEN_NAME'];
                        scope.appfatname = value.data.Details[0]['FATHER_HUSBEND_NAME'];
                        scope.appage = value.data.Details[0]['AGE'];
                        scope.appmobile = value.data.Details[0]['MOBILE_NUMBER'];
                        scope.appration = value.data.Details[0]['RATION_CARD'];
                        scope.appcaste = value.data.Details[0]['CASTE_CATEGORY'];
                        scope.appfamilycount = value.data.Details[0]['FAMILY_MEMBERS'];
                        scope.reginpss = (value.data.Details[0]['PSS_STATUS'] == "Y" ? "0" : "1");
                        scope.seldistrict = value.data.Details[0]['DISTRICT_CODE'];
                        scope.selmandal = value.data.Details[0]['TEHSIL_CODE'];
                        scope.selpanchayath = value.data.Details[0]['VT_CODE'];
                        LoadDistrics("1");
                        scope.doorno = value.data.Details[0]['BUILDING_NAME'];
                    }
                    else if (value.data.Status == "Success" && value.data.Details[0]['RATION_STATUS'] == "1") {
                        swal({
                            title: "",
                            text: "You are not a white Ration Card Holder",
                            type: "error"
                        }).then(function () {
                            window.location.reload();
                        });
                    }
                    else if (value.data.Status == "428") {
                        sessionStorage.clear();
                        swal("info", "Session Expired !!!", "info");
                        state.go("Login");
                        return;
                    }
                    else {
                        swal("info", "Data Loading Failed", "error");
                    }
                });
            }
            else {

            }
        }

        //Save Data
        scope.save = function () {
            if (!scope.uid) {
                swal('info', "Please Enter Aadhaar Number", 'error');
            }
            else if (scope.uid.length != "12") {
                swal('info', "Please Enter 12 Digit Aadhaar Number", 'error');
            }
            else if (!scope.appname) {
                swal('info', "Please Enter Applicant Name", 'error');
            }
            else if (!scope.appfatname) {
                swal('info', "Please Enter Applicant Father Name", 'error');
            }
            else if (!scope.appmobile) {
                swal('info', "Please Enter Applicant Mobile Number", 'error');
            }
            else if (!scope.appidno) {
                swal('info', "Please Enter Applicant ID Card Number", 'error');
            }
            else if (!scope.appage) {
                swal('info', "Please Enter Applicant age", 'error');
            }
            else if (!scope.appration) {
                swal('info', "Please Enter White Ration Card Number", 'error');
            }
            else if (!scope.appincome) {
                swal('info', "Please Enter Applicant Annual Income", 'error');
            }
            else if (!scope.appcaste) {
                swal('info', "Please Select Applicant Caste", 'error');
            }
            else if (!scope.appsubcaste) {
                swal('info', "Please Select Applicant Sub-Caste", 'error');
            }
            else if (!scope.appfamilycount) {
                swal('info', "Please Enter Applicant Family Members Count", 'error');
            }
            else if (!scope.loomcount) {
                swal('info', "Please Enter No. of Looms Present in House", 'error');
            }
            else if (!scope.loomtype) {
                swal('info', "Please Select Loom Type", 'error');
            }
            else if (!scope.loom) {
                swal('info', "Please Select wheather loom is own or not", 'error');
            }
            else if (!scope.doingloom) {
                swal('info', "Please Select whether using loom or not", 'error');
            }
            else if (!scope.housetype) {
                swal('info', "Please Select House Ownership type", 'error');
            }
            else if (!scope.reginpss) {
                swal('info', "Please Select Wheather registered in PSS or Not", 'error');
            }
            else if (!scope.bankname) {
                swal('info', "Please Enter Applicant Bank Name", 'error');
            }
            else if (!scope.branchname) {
                swal('info', "Please Enter Applicant Branch Name", 'error');
            }
            else if (!scope.accntnum) {
                swal('info', "Please Enter Applicant Account Number", 'error');
            }
            else if (!scope.bankname) {
                swal('info', "Please Enter Applicant IFSC Code", 'error');
            }
            else if (!scope.doorno) {
                swal('info', "Please Enter Applicant Door No.", 'error');
            }
            else if (!scope.seldistrict) {
                swal('info', "Please Select Applicant District", 'error');
            }
            else if (!scope.selruflag) {
                swal('info', "Please Select Applicant R/U Flag", 'error');
            }
            else if (!scope.selmandal) {
                swal('info', "Please Select Applicant Mandal", 'error');
            }
            else if (!scope.selpanchayath) {
                swal('info', "Please Select Applicant Panchayath/Ward/Village", 'error');
            }
            else if (!scope.pincode) {
                swal('info', "Please Enter Applicant Pincode", 'error');
            }
            else if (!scope.chkconsent) {
                swal('info', "Please Check the Checkbox", 'error');
            }
            else {
                scope.preloader = true;
                var req =
                {
                    NAME: scope.appname,
                    FATHERNAME: scope.appfatname,
                    AADHAAR: scope.uid,
                    MOBILE: scope.appmobile,
                    IDCARD: scope.appidno,
                    AGE: scope.appage,
                    RATION: scope.appration,
                    ANNINCOME: scope.appincome,
                    CASTE: scope.appcaste,
                    SUBCASTE: scope.appsubcaste,
                    FAMILYCOUNT: scope.appfamilycount,
                    LOOMCOUNT: scope.loomcount,
                    LOOMTYPE: scope.loomtype,
                    LOOMOWNERSHIP: scope.loom,
                    DOINGLOOM: scope.doingloom,
                    HOUSETYPE: scope.housetype,
                    PSSREG: scope.reginpss,
                    BANK: scope.bankname,
                    BRANCH: scope.branchname,
                    ACCOUNT: scope.accntnum,
                    IFSC: scope.IFSC,
                    DOORNO: scope.doorno,
                    DISTRICT: scope.seldistrict,
                    RURUALURBAN: scope.selruflag,
                    MANDAL: scope.selmandal,
                    VILLAGEPANCHAYATH: scope.selpanchayath,
                    PINCODE: scope.pincode,
                    SUBMITTEDBY: scope.submittedby
                }
                ser_services.POSTENCRYPTAPI("SaveTextileData", req,token, function (value) {
                    if (value.data.Status == "Success") {
                        scope.preloader = false;

                        swal({
                            title: "",
                            text: "Data Inserted Successfully",
                            type: "success"
                        }).then(function () {
                            window.location.reload();
                        });

                    }
                    else if (value.data.Status == "428") {
                        sessionStorage.clear();
                        swal("info", "Session Expired !!!", "info");
                        state.go("Login");
                        return;
                    }
                    else {
                        scope.preloader = false;
                        alert("info", "Data Submission Failed", "error");
                    }
                });
            }
        }

        // Load District's
        function LoadDistrics(e) {
            var req = {
                TYPE: "4"
            };
            ser_services.DemoInternalAPI("LoadDepartments", req, function (value) {
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
            ser_services.DemoInternalAPI("LoadDepartments", req, function (value) {
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
            ser_services.DemoInternalAPI("LoadDepartments", req, function (value) {
                if (value.data.Status == "Success") {
                    scope.PanchayatDD = value.data.Details;
                }
                else {
                    alert("Panchayats Loading Failed");
                }
            });
        }

        scope.steps = {
            step1: true,
            step2: false,
            step3: false
        };

        scope.btn_get_tab = function (tab_id) {
            scope.step_position = tab_id;
        };

        scope.$watch('step_position', function (e) {
            if (e == 1) {
                scope.steps.step1 = true;
            }
            else if (e == 2) {
                scope.steps.step2 = true;
            }
            else if (e == 3) {
                scope.steps.step3 = true;
                if (scope.finalsavevalue == "1") {
                    scope.finalsave = true;
                }
                else {
                    scope.finalsave = false;
                }
            }
        });

    };
})();