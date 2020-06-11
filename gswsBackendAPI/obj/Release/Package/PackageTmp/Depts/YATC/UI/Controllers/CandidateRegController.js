(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("CandidateRegController", ["$scope", "$state", "$log", "YATC_Services", YATC_CTRL]);

    function YATC_CTRL(scope, state, log, yatc_services) {
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }
        scope.pagename = "Candidate Registration";
        scope.preloader = false;
        scope.locality = "true";
        scope.isrural = true;

        LoadReligions();
        LoadCategory();
        LoadStates();

        scope.changelocality = function (value) {
            ClearAdderess();
            if (value) { scope.isrural = true; }
            else { scope.isrural = false; }
        }

        scope.LoadDistricts = function () {
            scope.DistrictDD = [];
            scope.ConstituencyDD = [];
            scope.MandalDD = [];
            scope.MunicipalityDD = [];

            if (scope.state) {
                var req = {
                    key: "DISTRICT",
                    ref_id: scope.state
                };
                yatc_services.POSTENCRYPTAPI("SkillDistrics", req, token, function (value) {
                    if (value.data.Status == 100)
                        scope.DistrictDD = value.data.Details;
                    else if (res.Status == "428") {
                        sessionStorage.clear();
                        swal("info", "Session Expired !!!", "info");
                        state.go("Login");
                        return;
                    }
                    else
                        swal("Failed", value.data.Reason, "info");

                });
            }
        }

        scope.changeDistrict = function () {
            scope.ConstituencyDD = [];
            scope.MandalDD = [];
            scope.MunicipalityDD = [];

            if (scope.district) {
                var req1 = {
                    key: "CONSTITUTION",
                    ref_id: scope.district
                };
                yatc_services.POSTENCRYPTAPI("SkillConstituency", req1, token, function (value) {
                    if (value.data.Status == 100)
                        scope.ConstituencyDD = value.data.Details;
                    else if (value.data.Status == "428") {
                        sessionStorage.clear();
                        swal("info", "Session Expired !!!", "info");
                        state.go("Login");
                        return;
                    }
                    else
                        swal("Failed", value.data.Reason, "info");
                });

                if (scope.isrural) {
                    var req = {
                        key: "MANDAL",
                        ref_id: scope.district
                    };
                    yatc_services.POSTENCRYPTAPI("SkillMandals", req, token, function (value) {
                        if (value.data.Status == 100)
                            scope.MandalDD = value.data.Details;
                        else if (value.data.Status == "428") {
                            sessionStorage.clear();
                            swal("info", "Session Expired !!!", "info");
                            state.go("Login");
                            return;
                        }
                        else
                            swal("Failed", value.data.Reason, "info");
                    });
                }
                else {
                    var req = {
                        key: "MUNICIPALITY",
                        ref_id: scope.district
                    };
                    yatc_services.POSTENCRYPTAPI("SkillMuncipality", req, token, function (value) {
                        if (value.data.Status == 100)
                            scope.MunicipalityDD = value.data.Details;
                        else if (value.data.Status == "428") {
                            sessionStorage.clear();
                            swal("info", "Session Expired !!!", "info");
                            state.go("Login");
                            return;
                        }
                        else
                            swal("Failed", value.data.Reason, "info");
                    });
                }

            }
        }

        scope.Register = function () {

            if (Validations()) {
                scope.preloader = true;

                var req = {
                    aadharNumber: scope.useraadhaar,
                    firstname: scope.fname,
                    lastname: scope.lname,
                    dob: scope.dob,
                    maritalstatus: scope.maritalstatus,
                    gender: scope.gender,
                    religion: scope.religion,
                    casteId: scope.category,
                    fatherName: scope.fathername,
                    fmlyAnnualIncome: scope.anualincome,
                    bloodGrop: scope.bloddgroup,
                    isDomicile: (!(scope.isresident) ? "false" : scope.isresident),
                    physicallyChallenged: (!(scope.isdisability) ? "false" : scope.isdisability),

                    alternatecontact: scope.alterno,

                    isRural: scope.locality,
                    state: scope.state,
                    addressline: scope.housenumber,
                    panchayat: scope.panchayat,
                    pincode: scope.pincode,
                    district: scope.district,
                    constititionId: scope.constituency,
                    village: (scope.locality == "true" ? scope.village : scope.wardnumber),
                    street: (!(scope.street) ? "" : scope.street),
                    mndlMunc: (scope.locality == "true" ? scope.mandal : scope.municipality),
                    registrationId: "",
					isUnEmployed: "Y",
					GSWS_ID: sessionStorage.getItem("TransID"),
                    userMaster: {
                        contact: scope.contactno,
                        email: scope.uemail,
                        password: scope.upwd,
                        confirmpassword: scope.urepwd,
                        username: scope.useraadhaar
                    }
                };
                yatc_services.POSTENCRYPTAPI("SkillCandidateReg", req, token, function (value) {
                    if (value.data.Status == 100) {
                        if (value.data.Details.status) {
                            swal("Success", "Registered successfully", "info");
                            state.go("ui.CandidateLogin");
                        }
                        else if (value.data.Status == "428") {
                            sessionStorage.clear();
                            swal("info", "Session Expired !!!", "info");
                            state.go("Login");
                            return;
                        }
                        else
                            swal("Failed", value.data.Details.message, "info");
                    }

                    else
                        swal("Failed", value.data.Reason, "info");
                });
            }
        }

        function ClearAdderess() {
            scope.state = "";
            scope.panchayat = "";
            scope.pincode = "";
            scope.district = "";
            scope.housenumber = "";
            scope.wardnumber = "";
            scope.constituency = "";
            scope.village = "";
            scope.street = "";
            scope.mandal = "";
            scope.municipality = "";

            scope.DistrictDD = [];
            scope.ConstituencyDD = [];
            scope.MandalDD = [];
            scope.MunicipalityDD = [];
        }

        function LoadStates() {
            scope.DistrictDD = [];
            scope.ConstituencyDD = [];
            scope.MandalDD = [];
            scope.MunicipalityDD = [];

            var req = {};
            yatc_services.POSTENCRYPTAPI("SkillStates", req, token, function (value) {
                if (value.data.Status == 100)
                    scope.StateDD = value.data.Details;
                else if (value.data.Status == "428") {
                    sessionStorage.clear();
                    swal("info", "Session Expired !!!", "info");
                    state.go("Login");
                    return;
                }
                else
                    swal("Failed", value.data.Reason, "info");

            });
        }

        function LoadReligions() {
            var req = {};
            yatc_services.POSTENCRYPTAPI("SkillReligions", req, token, function (value) {
                if (value.data.Status == 100)
                    scope.ReligionDD = value.data.Details;
                else if (value.data.Status == "428") {
                    sessionStorage.clear();
                    swal("info", "Session Expired !!!", "info");
                    state.go("Login");
                    return;
                }
                else
                    swal("Failed", value.data.Reason, "info");

            });
        }

        function LoadCategory() {
            var req = {};
            yatc_services.POSTENCRYPTAPI("SkillCategory", req, token, function (value) {
                if (value.data.Status == 100)
                    scope.CategoryDD = value.data.Details;
                else if (value.data.Status == "428") {
                    sessionStorage.clear();
                    swal("info", "Session Expired !!!", "info");
                    state.go("Login");
                    return;
                }
                else
                    swal("Failed", value.data.Reason, "info");

            });
        }

        function Validations() {
            var strongRegex = new RegExp("^(?=.*[a-zA-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{5,})");
            var emailRegex = new RegExp("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");

            if (!(scope.useraadhaar)) {
                swal("Info", "Please Enter Aadhaar Number.", "info");
                return false;
            }
            if (scope.useraadhaar.toString().length != "12") {
                swal("Info", "Aadhaar Number should be 12 digits.", "info");
                return false;
            }
            else if (scope.useraadhaar == "000000000000" || scope.useraadhaar == "111111111111" || scope.useraadhaar == "222222222222" || scope.useraadhaar == "333333333333" || scope.useraadhaar == "444444444444" || scope.useraadhaar == "555555555555" || scope.useraadhaar == "666666666666" || scope.useraadhaar == "777777777777" || scope.useraadhaar == "888888888888" || scope.useraadhaar == "999999999999") {
                swal("Info", "Please Enter a valid Aadhaar Number", "info");
                return false;
            }
            else if (!(scope.fname)) {
                swal("Info", "Please Enter First Name.", "info");
                return false;
            }
            else if (!(scope.lname)) {
                swal("Info", "Please Enter Last Name.", "info");
                return false;
            }
            else if (!(scope.dob)) {
                swal("Info", "Please Enter Date of Birth.", "info");
                return false;
            }
            else if (!(scope.maritalstatus)) {
                swal("Info", "Please Select Marital Status.", "info");
                return false;
            }
            else if (!(scope.gender)) {
                swal("Info", "Please Select Gender.", "info");
                return false;
            }
            else if (!(scope.religion)) {
                swal("Info", "Please Select Religion.", "info");
                return false;
            }
            else if (!(scope.category)) {
                swal("Info", "Please Select Category.", "info");
                return false;
            }
            else if (!(scope.anualincome)) {
                swal("Info", "Please Enter Anual Income.", "info");
                return false;
            }
            else if (!(scope.uemail)) {
                swal("Info", "Please Enter Email.", "info");
                return false;
            }
            else if (!(emailRegex.test(scope.uemail))) {
                swal("Info", "Please Enter Valid Email.", "info");
                return false;
            }
            else if (!(scope.contactno)) {
                swal("Info", "Please Enter Contact Number.", "info");
                return false;
            }
            else if (!(scope.alterno)) {
                swal("Info", "Please Enter Alternate Number.", "info");
                return false;
            }
            else if (!(scope.state)) {
                swal("Info", "Please Select State.", "info");
                return false;
            }
            else if (!(scope.district)) {
                swal("Info", "Please Select District.", "info");
                return false;
            }
            else if (!(scope.constituency)) {
                swal("Info", "Please Select Constituency.", "info");
                return false;
            }
            else if (scope.isrural && !(scope.mandal)) {
                swal("Info", "Please Select Mandal.", "info");
                return false;
            }
            else if (!scope.isrural && !(scope.municipality)) {
                swal("Info", "Please Select Municipality.", "info");
                return false;
            }
            else if (scope.isrural && !(scope.panchayat)) {
                swal("Info", "Please Enter Panchayat.", "info");
                return false;
            }
            else if (!scope.isrural && !(scope.wardnumber)) {
                swal("Info", "Please Enter Ward Number.", "info");
                return false;
            }
            else if (scope.isrural && !(scope.village)) {
                swal("Info", "Please Enter Village.", "info");
                return false;
            }
            else if (!(scope.upwd)) {
                swal("Info", "Please Enter Password.", "info");
                return false;
            }
            else if (!(strongRegex.test(scope.upwd))) {
                swal("Info", "Password must contain at least five characters, including letters, special characters and numbers.", "info");
                return false;
            }
            else if (scope.upwd != scope.urepwd) {
                swal("Info", "Password & Confirm Password Should be Same.", "info");
                return false;
            }


            return true;
        }
    };
})();