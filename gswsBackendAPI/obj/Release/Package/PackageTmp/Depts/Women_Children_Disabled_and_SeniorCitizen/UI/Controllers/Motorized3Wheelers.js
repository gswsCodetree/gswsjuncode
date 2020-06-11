(function () {
    var app = angular.module("GSWS");

    app.controller("MotorizedWheelers_Controller", ["$scope", "$state", "$log", "Women_Services", WomenDis_CTRL]);

    function WomenDis_CTRL(scope, state, log, wom_services) {
        //sessionStorage.setItem("WCDWToken", "XzPsdNGUxPgdAERajNovVbwVgEyGQxBqcfKYnAamfscWZyiCrIuMQFTK");

        var wcdwtoken = sessionStorage.getItem("WCDWToken");
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!(token) || !(user)) {
            sessionStorage.clear();
            alert('Session expired..!');
            state.go("Login");
        }

        if (!wcdwtoken)
            state.go("ui.APDASCAC_RegForm");

        scope.Preloader = false;
        ShowandHideTabs(1);

        scope.AllMandalsDD = [];

        scope.PersonalDetails = [];
        scope.EducationDetails = [];
        scope.DisabilityDetails = [];

        scope.DisabilityDD = [{ "optid": "1", "optvalue": "Orthopedically handicapped (OH)", "opt_name": "Orthopedically handicapped (OH)" }, { "optid": "2", "optvalue": "Deaf or Hard of Hearing", "opt_name": "Deaf or Hard of Hearing" }, { "optid": "3", "optvalue": "Vision Impairment", "opt_name": "Vision Impairment" }]
        scope.DistrictsDD = [];
        scope.EduDistrictsDD = [];
        scope.StatesDD = [];
        scope.UniveristiesDD = [];
        scope.CourseStartYearsData = [];
        scope.CourseEndYearsData = [];
        scope.EligibleList = [];
        scope.PrevReceiveList = [];
        scope.ApplyEligibleList = [];
        scope.ApplyList = [];

        scope.PrevRecLaptop = "2";
        scope.PrevRecDaisyPlayers = "2";
        scope.PrevRecTricycle = "2";
        scope.PrevRecTouchPhone = "2";
        scope.PrevRecCalipers = "2";
        scope.PrevRecCrutches = "2";
        scope.PrevRecHearing = "2";
        scope.PrevRecBilindSticks = "2";
        scope.PrevRecwheelchair = "2";

        scope.ApplyLaptop = "2";
        scope.ApplyDaisyPlayers = "2";
        scope.ApplyTricycle = "2";
        scope.ApplyTouchPhone = "2";
        scope.ApplyCalipers = "2";
        scope.ApplyCrutches = "2";
        scope.ApplyHearing = "2";
        scope.ApplyBilindSticks = "2";
        scope.Applywheelchair = "2";

        LoadInitialData();
        Loadyears();
        LoadCourseStartYears();
        LoadCourseEndYears();
        ShowSteps('1');

        scope.DistrictChange = function (type) {
            if (type == "Native") {
                scope.MandalsDD = [];
                if (scope.ddldist) {
                    scope.MandalsDD = $(scope.AllMandalsDD).filter(function (i, n) { return n.DistrictID === scope.ddldist.optid });
                }
            }
            else if (type == "Perminent") {
                scope.MandalsDD_Address = [];
                if (scope.ddl_per_dist) {
                    scope.MandalsDD_Address = $(scope.AllMandalsDD).filter(function (i, n) { return n.DistrictID === scope.ddl_per_dist.optid });
                }
            }
            else if (type == "Wage") {
                scope.WageMandalsDD = [];
                if (scope.WageDistrict) {
                    scope.WageMandalsDD = $(scope.AllMandalsDD).filter(function (i, n) { return n.DistrictID === scope.WageDistrict.optid });
                }
            }
            else if (type == "Employee") {
                scope.EmployMandalsDD = [];
                if (scope.SelfDistrict) {
                    scope.EmployMandalsDD = $(scope.AllMandalsDD).filter(function (i, n) { return n.DistrictID === scope.SelfDistrict.optid });
                }
            }
            else if (type == "Education") {
                scope.EduMandalsDD = [];
                if (scope.EduDistrict) {
                    scope.EduMandalsDD = $(scope.AllMandalsDD).filter(function (i, n) { return n.DistrictID === scope.EduDistrict.optid });
                }
            }
        }

        scope.MandalChange = function (type) {
            if (type == "Native") {
                scope.VillagesDD = [];
                if (scope.ddlmandal) {
                    LoadVillages(type, scope.ddlmandal.MandalID);
                }
            }
            else if (type == "Perminent") {
                scope.VillagesDD_Address = [];
                if (scope.ddl_per_mandal) {
                    LoadVillages(type, scope.ddl_per_mandal.MandalID);
                }
            }
            else if (type == "Wage") {
                scope.WaseVillagesDD = [];
                if (scope.WageMandal) {
                    LoadVillages(type, scope.WageMandal.MandalID);
                }
            }
            else if (type == "Employee") {
                scope.EmployVillagesDD = [];
                if (scope.SelfMandal) {
                    LoadVillages(type, scope.SelfMandal.MandalID);
                }
            }
            else if (type == "Education") {
                scope.EduVillagesDD = [];
                if (scope.EduMandal) {
                    LoadVillages(type, scope.EduMandal.MandalID);
                }
            }
        }

        scope.FirstNext = function () {
            if (Step1PersonDetValidations())
                ShowandHideTabs(2);
        }

        scope.FirstPrevious = function () { ShowandHideTabs(1); }

        scope.SecondNext = function () {
            if (Step2EducationValidations())
                ShowandHideTabs(3);
        }

        scope.SecondPrevious = function () { ShowandHideTabs(2); }

        scope.ThirdNext = function () {
            if (Step3DisableValidations()) {
                scope.EligibleList = [];

                var disstr = "";
                $.each($("#Disability").val(), function (index, value) {
                    disstr += value + ",";
                });

                scope.DisabilityType = disstr.slice(0, -1);

                if ((scope.DisabilityType.indexOf("Vision Impairment") != -1 || scope.DisabilityType.indexOf("Orthopedically handicapped (OH)") != -1 || scope.DisabilityType.indexOf("Deaf or Hard of Hearing") != -1) && scope.DisabilityPercentage >= 40 && scope.Dateofbirth <= new Date("2002-04-01") && scope.CurrentlyStudying == 'Y' && (scope.PresentEducation == 'PROFESSIONAL COURSES' || scope.PresentEducation == 'POST GRADUATE & ABOVE' || scope.PresentEducation == 'OTHERS' || scope.PresentEducation == 'DEGREE')) {
                    scope.isLaptop = true;
                    scope.EligibleList.push("3");
                }
                else
                    scope.isLaptop = false;

                if (scope.DisabilityType.indexOf("Vision Impairment") != -1 && scope.DisabilityPercentage >= 40 && scope.Dateofbirth <= new Date("2003-04-01") && (scope.HighestEducation != '' || scope.PresentEducation != '')) {
                    scope.isDaisyPlayer = true;
                    scope.EligibleList.push("4");
                }
                else
                    scope.isDaisyPlayer = false;

                if (scope.DisabilityType.indexOf("Orthopedically handicapped (OH)") != -1 && scope.DisabilityPercentage >= 40 && scope.Dateofbirth <= new Date("2003-04-01")) {
                    scope.isTrycycle = true;
                    scope.EligibleList.push("5");
                }
                else
                    scope.isTrycycle = false;

                if (scope.DisabilityType.indexOf("Deaf or Hard of Hearing") != -1 && scope.DisabilityPercentage >= 40 && scope.Dateofbirth <= new Date("2003-04-01")) {
                    scope.isTouchPhone = true;
                    scope.EligibleList.push("6");
                }
                else
                    scope.isTouchPhone = false;

                if (scope.DisabilityType.indexOf("Orthopedically handicapped (OH)") != -1 && scope.DisabilityPercentage >= 40 && scope.Dateofbirth <= new Date("2012-04-01")) {
                    scope.isLimbs = true;
                    scope.isCrutches = true;
                    scope.EligibleList.push("7");
                    scope.EligibleList.push("8");
                }
                else {
                    scope.isLimbs = false;
                    scope.isCrutches = false;
                }

                if (scope.DisabilityType.indexOf("Deaf or Hard of Hearing") != -1 && scope.DisabilityPercentage >= 40 && scope.Dateofbirth <= new Date("2003-04-01")) {
                    scope.isHearingAid = true;
                    scope.EligibleList.push("9");
                }
                else
                    scope.isHearingAid = false;

                if (scope.DisabilityType.indexOf("Vision Impairment") != -1 && scope.DisabilityPercentage >= 40 && scope.Dateofbirth <= new Date("2003-04-01")) {
                    scope.isBilindSticks = true;
                    scope.EligibleList.push("21");
                }
                else
                    scope.isBilindSticks = false;

                if (scope.DisabilityType.indexOf("Orthopedically handicapped (OH)") != -1 && scope.DisabilityPercentage >= 40 && scope.Dateofbirth <= new Date("2003-04-01")) {
                    scope.isWheelChair = true;
                    scope.EligibleList.push("22");
                }
                else
                    scope.isWheelChair = false;


                ShowandHideTabs(4);
            }
        }

        scope.ThirdPrevious = function () { ShowandHideTabs(3); }

        scope.FourNext = function () {
            if (Step4Validations())
                scope.StepChange('2');
        }

        scope.Step2Next = function () {
            if (MainStep2Validations())
                scope.StepChange('3');
        }

        scope.Step2Previous = function () { scope.StepChange(1); }

        scope.Step3Next = function () {
            if (MainStep3Validations())
                scope.StepChange('4');
        }

        scope.Step3Previous = function () { scope.StepChange(2); }

        scope.Refresh = function () {
            location.reload(true);
        };

        scope.showtab = function (type) {
            ShowandHideTabs(type);
        };

        scope.StepChange = function (value) {
            HideSteps();
            ShowSteps(value);
        }

        scope.EditApplication = function () {
            HideSteps();
            ShowSteps('1');
        }

        scope.SaveApplication = function () {
            if (!scope.IsAccept) {
                swal("Info", "Please Accept Terms & Conditions", "info");
                return false;
            }
            else if (Step1PersonDetValidations() && Step2EducationValidations() && Step3DisableValidations() && scope.ApplyList.length > 0 && MainStep3Validations()) {

                var PerDetails = {
                    "token": wcdwtoken,
                    "applicant": scope.ApplicantName,
                    "father": scope.parentsName,
                    "aadhar": scope.AadhaarNumber,
                    "dob": moment(scope.Dateofbirth).format('YYYY-MM-DD'),
                    "gender": scope.Gender,
                    "maritalstatus": scope.MaritalStatus,
                    "femaletype": "",
                    "minority": (scope.ddlminority == 'y' ? 'yes' : (scope.ddlminority == 'n' ? "no" : "")),
                    "religion": scope.Religion,
                    "district": (scope.ddldist ? scope.ddldist.optid : ""),
                    "mandal": (scope.ddlmandal ? scope.ddlmandal.MandalID : ""),
                    "village": (scope.ddlvillage ? scope.ddlvillage.optid : ""),
                    "othervillage": "",
                    "habitation": scope.habitaton,
                    "wardnumber": scope.wardnumber,
                    "housenumber": scope.housenumber,
                    "pincode": scope.pincode,
                    "nativity": scope.NativePlace,

                    "sameaddress": (scope.isPerAddr == 'Y' ? 'Yes' : (scope.isPerAddr == 'N' ? "No" : "")),
                    "present_district": (scope.ddl_per_dist ? scope.ddl_per_dist.optid : ""),
                    "present_mandal": (scope.ddl_per_mandal ? scope.ddl_per_mandal.MandalID : ""),
                    "present_village": (scope.ddl_per_Village ? scope.ddl_per_Village.optid : ""),
                    "present_othervillage": "",
                    "present_habitation": scope.per_Habitation,
                    "present_wardnumber": scope.per_ward,
                    "present_housenumber": scope.per_HouseNo,
                    "present_pincode": scope.per_Pincode,
                    "nativity_present": scope.ddl_per_NativePlace,

                    "annualincomeoffamily": scope.Income,
                    "incomeissueddistrict": (scope.ddlIncomedist ? scope.ddlIncomedist.optid : ""),
                    "dateofincome": moment(scope.IncomeIssuedDate).format('YYYY-MM-DD'),
                    "pensioner": (scope.isPensioner == 'Y' ? 'Yes' : (scope.isPensioner == 'N' ? "No" : "")),
                    "pensiontype": scope.PensionAuthorityperson,
                    "pensionnumber": scope.PensionNumber,
                    "pensionissueddistrict": (scope.ddlBPLdist ? scope.ddlBPLdist.optid : ""),

                    "whiteration": (scope.ddlration == 'Y' ? 'Yes' : (scope.ddlration == 'N' ? "No" : "")),
                    "rationnumber": scope.rationNo,
                    "rationdistrict": (scope.ddlrationdist ? scope.ddlrationdist.optid : ""),
                    "licencetype": scope.ddlLicence,
                    "drivinglicenceno": scope.LicenceLLRNo,
                    "licensevalidity": moment(scope.LicenceLLRDate).format('YYYY-MM-DD'),
                    "llrno": scope.LicenceNo,
                    "llrdate": scope.ddllicencevalidity,

                    "sportsman": (scope.ddlsports == 'Y' ? 'Yes' : (scope.ddlsports == 'N' ? "No" : "")),
                    "sportslevel": scope.SportsLevel,
                    "sportsorg": scope.SportsName,
                    "sportevent": scope.SportsEvent,
                    "sportsdate": moment(scope.SportsEventDate).format('YYYY-MM-DD'),
                }

                var EduDetails = {
                    "token": wcdwtoken,
                    "literate": (scope.Literate == 'Y' ? 'Yes' : (scope.Literate == 'N' ? "No" : "")),
                    "employmenttype": (scope.SelfEmployed == 'Y' ? 'Yes' : (scope.SelfEmployed == 'N' ? "No" : "")),
                    "studying": (scope.CurrentlyStudying == 'Y' ? 'Yes' : (scope.CurrentlyStudying == 'N' ? "No" : "")),
                    "selfemployment": (scope.SelfEmployed == 'Y' ? 'Yes' : (scope.SelfEmployed == 'N' ? "No" : "")),
                    "wageemployment": (scope.WageEmployee == 'Y' ? 'Yes' : (scope.WageEmployee == 'N' ? "No" : "")),

                    "unitname": scope.EmployeementUnit,
                    "noofemployees": scope.NoofEmployeesUnit,
                    "annualturnover": scope.AnnualTurnover,
                    "governmentsubsidy": (scope.GovtSubsidy == 'Y' ? 'Yes' : (scope.GovtSubsidy == 'N' ? "No" : "")),
                    "bankloan": (scope.SelfBankLoan == 'Y' ? 'Yes' : (scope.SelfBankLoan == 'N' ? "No" : "")),

                    "self_sameaddress": (scope.SelfIsPerminent == 'Y' ? 'Yes' : (scope.SelfIsPerminent == 'N' ? "No" : "")),
                    "self_district": (scope.SelfDistrict ? scope.SelfDistrict.optid : ""),
                    "self_mandal": (scope.SelfMandal ? scope.SelfMandal.MandalID : ""),
                    "self_village": (scope.SelfVillage ? scope.SelfVillage.optid : ""),
                    "self_othervillage": "",
                    "self_habitation": scope.SelfHabitation,
                    "self_wardnumber": scope.SelfWardNo,
                    "self_housenumber": scope.SelfHouseNo,
                    "self_pincode": scope.SelfPincode,

                    "currenteducation": scope.PresentEducation,
                    "collegename": scope.NameofCollege,
                    "admissionnumber": scope.AdmissionNumber,
                    "coursename": scope.CourseName,
                    "studyingprofessionalcourses": (scope.StudyingProfessionalCourse == 'Y' ? 'Yes' : (scope.StudyingProfessionalCourse == 'N' ? "No" : "")),
                    "university": scope.University,
                    "otheruniversity": "",
                    "courseduration": scope.CourseStartDuration,
                    "coursedurationto": scope.CourseEndDuration,
                    "schoolname": scope.NameofSchool,
                    "schooladmissionnumber": scope.AdmissionNumber,

                    "college_district": (scope.EduDistrict ? scope.EduDistrict.optid : ""),
                    "college_state": (scope.OutEduState ? scope.OutEduState.optid : ""),
                    "college_state_district": scope.OutEduCityName,
                    "college_state_address": scope.OutEduClgAddress,
                    "college_mandal": (scope.EduMandal ? scope.EduMandal.MandalID : ""),
                    "college_village": (scope.EduVillage ? scope.EduVillage.optid : ""),
                    "college_othervillage": "",
                    "college_habitation": scope.EduHabitation,
                    "college_wardnumber": scope.EduWardNo,
                    "college_housenumber": scope.EduHouseNo,
                    "college_pincode": scope.EduPincode,

                    "organizationname": scope.WageEmpOrganization,
                    "designation": scope.WageEmpDesignation,
                    "wage_district": (scope.WageDistrict ? scope.WageDistrict.optid : ""),
                    "wage_mandal": (scope.WageMandal ? scope.WageMandal.MandalID : ""),
                    "wage_village": (scope.WageVillage ? scope.WageVillage.optid : ""),
                    "wage_othervillage": scope.WagePanchayat,
                    "wage_habitation": scope.WageHabitation,
                    "wage_wardnumber": scope.WageWardNo,
                    "wage_housenumber": scope.WageHouseNo,
                    "wage_pincode": scope.WagePincode
                }

                var DisableDetails = {
                    "token": wcdwtoken,
                    "natureofdisability": (scope.DisabilityType ? scope.DisabilityType : ""),
                    "natureofdisability_m": "",
                    "percentageofdisability": scope.DisabilityPercentage,
                    "sadaremnumber": scope.SadaremCertificate
                }

                var PrevRecList = {
                    "token": wcdwtoken,
                    "List": scope.PrevReceiveList
                }

                var UploadDocuments = {
                    "token": wcdwtoken,
                    "photo_aadhar": File1Base64,
                    "photo_dob": File2Base64,
                    "photo_disability": File3Base64,
                    "photo_sadarem": File4Base64,
                    "photo_income": File5Base64,
                    "photo_caste": File6Base64,
                }

                var ApplyList = {
                    "token": wcdwtoken,
                    "List": scope.ApplyList
                }

                var MainObj = {
                    "GSWS_ID": sessionStorage.getItem("TransID"),
                    "PerDetails": PerDetails,
                    "EduDetails": EduDetails,
                    "DisableDetails": DisableDetails,
                    "PrevRecList": PrevRecList,
                    "UploadDocuments": UploadDocuments,
                    "ApplyList": ApplyList,
                }

                scope.Preloader = true;
                wom_services.POSTENCRYPTAPI("POSTWCDWApplication", MainObj, token, function (value) {
                    var res = value.data;
                    scope.Preloader = false;
                    if (res.Status == 100) {
                        swal({
                            title: "Success!",
                            text: "Application is submitted successfully. \n Application No : " + value.data.Data,
                            icon: "warning",
                            buttons: true,
                            dangerMode: false,
                        }).then((willDelete) => {
                            if (willDelete) {
                                state.go("ui.APDASCAC_RegForm");
                            }
                        });
                        //swal("Success", "Application is submitted successfully. \n Application No : " + value.data.Data, "success");
                    }
                    else if (value.data.Status == "428") {
                        sessionStorage.clear();
                        swal("info", "Session Expired !!!", "info");
                        state.go("Login");
                        return;
                    }
                    else {
                        swal('Error', res.Reason, 'error');
                        return;
                    }
                });
            }

        }

        scope.showtabbutton = function (type) {
            if (type == 2) {
                if (Step1PersonDetValidations())
                    ShowandHideTabs(2);
                else
                    ShowandHideTabs(1);
            }
            else if (type == 3) {
                if (Step1PersonDetValidations()) {
                    if (Step2EducationValidations())
                        ShowandHideTabs(3);
                    else
                        ShowandHideTabs(2);
                }
                else
                    ShowandHideTabs(1);
            }
        }

        function LoadInitialData() {
            LoadCommonData("Districts", "2");
            LoadCommonData("Mandlas", "3");
            LoadCommonData("Education", "5");
            LoadCommonData("University", "6");
            LoadCommonData("States", "7");
            //LoadCommonData("Disable", "8");
        }

        function LoadCommonData(ltype, i_type) {
            var restype = i_type;
            var req = {
                type: restype,
                wcdwtoken: wcdwtoken
            };
            scope.Preloader = true;
            wom_services.POSTENCRYPTAPI("LoadGeneralData", req, token, function (value) {

                var res = value.data;
                scope.Preloader = false;
                if (res.Status == 100) {
                    if (restype == "2") {
                        scope.EduDistrictsDD = res.Data;
                        scope.EduDistrictsDD.push({ "optid": "14", "opt_value": "notinlist", "opt_name": "Not In List" });
                        scope.DistrictsDD = res.Data;
                    }
                    else if (restype == "3") {
                        scope.AllMandals = [];
                        for (var i = 0; i < res.Data.length; i++) {
                            scope.AllMandals.push({
                                DistrictID: (res.Data[i].opt_district_id ? res.Data[i].opt_district_id.replace(/(\r\n|\n|\r)/gm, "") : ""),
                                MandalID: res.Data[i].optid,
                                MandalName: res.Data[i].opt_name
                            });
                        }
                        scope.AllMandalsDD = scope.AllMandals;
                    }
                    else if (restype == "5") {
                        scope.EducationsDD = res.Data;
                    }
                    else if (restype == "6") {
                        scope.UniveristiesDD = res.Data;
                    }
                    else if (restype == "7") {
                        scope.StatesDD = res.Data;
                    }
                    else if (restype == "8") {
                        scope.DisabilityDD = res.Data;
                    }
                }
                else if (value.data.Status == "428") {
                    sessionStorage.clear();
                    swal("info", "Session Expired !!!", "info");
                    state.go("Login");
                    return;
                }
                else {
                    swal('Error', res.Reason, 'error');
                    return;
                }
            });
        }

        function LoadVillages(type, value) {
            var restype = type;
            var inputvallue = value;
            var req = { Mandalid: inputvallue, wcdwtoken: wcdwtoken };
            scope.Preloader = true;
            wom_services.POSTENCRYPTAPI("GetVillages", req, token, function (value) {
                var res = value.data;
                scope.Preloader = false;
                if (res.Status == 100) {
                    if (restype == "Native")
                        scope.VillagesDD = res.Data;
                    else if (restype == "Perminent")
                        scope.VillagesDD_Address = res.Data;
                    else if (restype == "Wage")
                        scope.WaseVillagesDD = res.Data;
                    else if (restype == "Employee")
                        scope.EmployVillagesDD = res.Data;
                    else if (restype == "Education")
                        scope.EduVillagesDD = res.Data;
                    //if (type == '1')
                    //    scope.VillageData = scope.AllVillages;
                    //if (type == '2')
                    //    scope.VillageAddressData = scope.AllVillages;
                    //if (type == '3')
                    //    scope.VillageData_Education = scope.AllVillages;
                    //if (type == '4')
                    //    scope.VillageData_Employeement = scope.AllVillages;
                }
                else if (value.data.Status == "428") {
                    sessionStorage.clear();
                    swal("info", "Session Expired !!!", "info");
                    state.go("Login");
                    return;
                }
                else {
                    swal('Error', res.Reason, 'error');
                    return;
                }
            });
        }

        function Loadyears() {

            var req = {};
            scope.Preloader = true;
            wom_services.POSTENCRYPTAPI("GetYears", req, token, function (value) {
                var res = value.data;
                scope.Preloader = false;
                if (res.Status == "Success") {
                    scope.YearsData = res.Data;
                }
                else if (value.data.Status == "428") {
                    sessionStorage.clear();
                    swal("info", "Session Expired !!!", "info");
                    state.go("Login");
                    return;
                }
                else {
                    scope.YearsData = "";
                    swal('Error', res.Reason, 'error');
                    return;
                }
            });
        }

        function LoadCourseStartYears() {
            var startyear = (new Date).getFullYear() - 10;
            var endyear = (new Date).getFullYear();
            for (i = startyear; i < endyear; i++) {
                scope.CourseStartYearsData.push({ "year_ID": i, "year": i });
            }
        }

        function LoadCourseEndYears() {
            var startyear = (new Date).getFullYear() - 3;
            var endyear = (new Date).getFullYear() + 5;

            for (i = startyear; i < endyear; i++) {
                scope.CourseEndYearsData.push({ "year_ID": i, "year": i });
            }
        }

        function ShowSteps(value) {
            jQuery('.maintab-cls').each(function () {
                $(this).removeClass("active");
            });

            if (value == 1) {
                scope.tab1 = true;
                ShowandHideTabs(1);
                $("#step1tab").addClass("active");
            }
            if (value == 2) {
                FillStep2();
                scope.tab2 = true;
                $("#step2tab").addClass("active");
            }
            if (value == 3) {
                scope.tab3 = true;
                $("#step3tab").addClass("active");
            }
            if (value == 4) {
                FillStep4();
                scope.tab4 = true;
                $("#step4tab").addClass("active");
            }
            if (value == 5) {
                scope.tab5 = true;
                $("#step5tab").addClass("active");
            }
        }

        function HideSteps() {
            scope.tab1 = false;
            scope.tab2 = false;
            scope.tab3 = false;
            scope.tab4 = false;
            scope.tab5 = false;
        }

        function FillStep2() {
            if (scope.ApplyEligibleList.length <= 0) {
                swal("Info", "Based on Your Profile, Currently not Eligible for any scheme", "info");
                return false;
            }

            $.each(scope.ApplyEligibleList, function (index, value) {
                if (value == "3") {
                    scope.isAplLaptop = true;
                }
                else if (value == "4") {
                    scope.isAplDaisyPlayer = true;
                }
                else if (value == "5") {
                    scope.isAplTrycycle = true;
                }
                else if (value == "6") {
                    scope.isAplTouchPhone = true;
                }
                else if (value == "7") {
                    scope.isAplLimbs = true;
                }
                else if (value == "8") {
                    scope.isAplCrutches = true;
                }
                else if (value == "9") {
                    scope.isAplHearingAid = true;
                }
                else if (value == "21") {
                    scope.isAplBilindSticks = true;
                }
                else if (value == "22") {
                    scope.isAplWheelChair = true;
                }

            });

            scope.ApplyList = [];
        }

        function FillStep4() {
            // Applicant Personal Details
            scope.RevApplicantName = scope.ApplicantName;
            scope.RevparentsName = scope.parentsName;
            scope.RevAadhaarNumber = scope.AadhaarNumber;
            scope.RevDateofbirth = moment(scope.Dateofbirth).format('DD-MM-YYYY');
            scope.RevGender = scope.Gender;
            scope.RevMaritalStatus = scope.MaritalStatus;
            scope.RevCaste = scope.Caste;
            scope.Revddlminority = (scope.ddlminority == 'y' ? 'yes' : (scope.ddlminority == 'n' ? "no" : ""));
            scope.RevReligion = scope.Religion;
            scope.Revddldist = (scope.ddldist ? scope.ddldist.opt_name : "");
            scope.Revddlmandal = (scope.ddlmandal ? scope.ddlmandal.MandalName : "");
            scope.Revddlvillage = (scope.ddlvillage ? scope.ddlvillage.opt_name : "");
            scope.Revhabitaton = scope.habitaton;
            scope.Revwardnumber = scope.wardnumber;
            scope.Revhousenumber = scope.housenumber;
            scope.Revpincode = scope.pincode;
            scope.RevNativePlace = scope.NativePlace;
            scope.RevisPerAddr = (scope.isPerAddr == 'Y' ? 'Yes' : (scope.isPerAddr == 'N' ? "No" : ""));

            scope.Revddl_per_dist = (scope.ddl_per_dist ? scope.ddl_per_dist.opt_name : "");
            scope.Revddl_per_mandal = (scope.ddl_per_mandal ? scope.ddl_per_mandal.MandalName : "");
            scope.Revddl_per_Village = (scope.ddl_per_Village ? scope.ddl_per_Village.opt_name : "");
            scope.Revper_Habitation = scope.per_Habitation;
            scope.Revper_ward = scope.per_ward;
            scope.Revper_HouseNo = scope.per_HouseNo;
            scope.Revper_Pincode = scope.per_Pincode;
            scope.Revddl_per_NativePlace = scope.ddl_per_NativePlace;

            scope.RevisPensioner = (scope.isPensioner == 'Y' ? 'Yes' : (scope.isPensioner == 'N' ? "No" : ""));
            scope.RevPensionAuthorityperson = scope.PensionAuthorityperson;
            scope.RevPensionNumber = scope.PensionNumber;

            scope.RevddlBPLdist = (scope.ddlBPLdist ? scope.ddlBPLdist.opt_name : "");
            scope.Revddlration = (scope.ddlration == 'Y' ? 'Yes' : (scope.ddlration == 'N' ? "No" : ""));
            scope.RevrationNo = scope.rationNo;
            scope.Revddlrationdist = (scope.ddlrationdist ? scope.ddlrationdist.opt_name : "");

            scope.RevIncome = scope.Income;
            scope.RevIncomeIssuedDate = moment(scope.IncomeIssuedDate).format('DD-MM-YYYY');
            scope.RevddlIncomedist = (scope.ddlIncomedist ? scope.ddlIncomedist.opt_name : "");

            scope.RevddlLicence = scope.ddlLicence;
            scope.RevLicenceLLRNo = scope.LicenceLLRNo;
            scope.RevLicenceLLRDate = moment(scope.LicenceLLRDate).format('DD-MM-YYYY');
            scope.RevLicenceNo = scope.LicenceNo;
            scope.Revddllicencevalidity = scope.ddllicencevalidity;

            scope.Revddlsports = (scope.ddlsports == 'Y' ? 'Yes' : (scope.ddlsports == 'N' ? "No" : ""));
            scope.RevSportsLevel = scope.SportsLevel;
            scope.RevSportsName = scope.SportsName;
            scope.RevSportsEvent = scope.SportsEvent;
            scope.RevSportsEventDate = moment(scope.SportsEventDate).format('DD-MM-YYYY');

            //Applicant Education Details
            scope.RevLiterate = (scope.Literate == 'Y' ? 'Yes' : (scope.Literate == 'N' ? "No" : ""));
            scope.RevCurrentlyStudying = (scope.CurrentlyStudying == 'Y' ? 'Yes' : (scope.CurrentlyStudying == 'N' ? "No" : ""));
            scope.RevHighestEducation = scope.HighestEducation;
            scope.RevSelfEmployed = (scope.SelfEmployed == 'Y' ? 'Yes' : (scope.SelfEmployed == 'N' ? "No" : ""));
            scope.RevWageEmployee = (scope.WageEmployee == 'Y' ? 'Yes' : (scope.WageEmployee == 'N' ? "No" : ""));
            scope.RevPresentEducation = scope.PresentEducation;

            scope.NameofSchool = scope.NameofSchool;
            scope.RevNameofCollege = scope.NameofCollege;
            scope.RevAdmissionNumber = scope.AdmissionNumber;
            scope.RevStudyingProfessionalCourse = (scope.StudyingProfessionalCourse == 'Y' ? 'Yes' : (scope.StudyingProfessionalCourse == 'N' ? "No" : ""));
            scope.RevCourseName = scope.CourseName;
            scope.RevUniversity = scope.University;
            scope.RevCourseStartDuration = scope.CourseStartDuration;
            scope.RevCourseEndDuration = scope.CourseEndDuration;

            scope.RevEduDistrict = (scope.EduDistrict ? scope.EduDistrict.opt_name : "");
            scope.RevEduMandal = (scope.EduMandal ? scope.EduMandal.MandalName : "");
            scope.RevEduVillage = (scope.EduVillage ? scope.EduVillage.opt_name : "");
            scope.RevEduHabitation = scope.EduHabitation;
            scope.RevEduWardNo = scope.EduWardNo;
            scope.RevEduHouseNo = scope.EduHouseNo;
            scope.RevEduPincode = scope.EduPincode;
            scope.RevOutEduState = (scope.OutEduState ? scope.OutEduState.opt_name : "");
            scope.RevOutEduCityName = scope.OutEduCityName;
            scope.RevOutEduClgAddress = scope.OutEduClgAddress;

            scope.RevEmployeementUnit = scope.EmployeementUnit;
            scope.RevNoofEmployeesUnit = scope.NoofEmployeesUnit;
            scope.RevAnnualTurnover = scope.AnnualTurnover;
            scope.RevGovtSubsidy = (scope.GovtSubsidy == 'Y' ? 'Yes' : (scope.GovtSubsidy == 'N' ? "No" : ""));
            scope.RevSelfBankLoan = (scope.SelfBankLoan == 'Y' ? 'Yes' : (scope.SelfBankLoan == 'N' ? "No" : ""));
            scope.RevSelfIsPerminent = (scope.SelfIsPerminent == 'Y' ? 'Yes' : (scope.SelfIsPerminent == 'N' ? "No" : ""));
            scope.RevSelfDistrict = (scope.SelfDistrict ? scope.SelfDistrict.opt_name : "");
            scope.RevSelfMandal = (scope.SelfMandal ? scope.SelfMandal.MandalName : "");
            scope.RevSelfVillage = (scope.SelfVillage ? scope.SelfVillage.opt_name : "");
            scope.RevSelfHabitation = scope.SelfHabitation;
            scope.RevSelfWardNo = scope.SelfWardNo;
            scope.RevSelfHouseNo = scope.SelfHouseNo;
            scope.RevSelfPincode = scope.SelfPincode;

            scope.RevWageEmpOrganization = scope.WageEmpOrganization;
            scope.RevWageEmpDesignation = scope.WageEmpDesignation;
            scope.RevWageDistrict = (scope.WageDistrict ? scope.WageDistrict.opt_name : "");
            scope.RevWageMandal = (scope.WageMandal ? scope.WageMandal.MandalName : "");
            scope.RevWageVillage = (scope.WageVillage ? scope.WageVillage.opt_name : "");
            scope.RevWagePanchayat = scope.WagePanchayat;
            scope.RevWageHabitation = scope.WageHabitation;
            scope.RevWageWardNo = scope.WageWardNo;
            scope.RevWageHouseNo = scope.WageHouseNo;
            scope.RevWagePincode = scope.WagePincode;

            scope.RevDisabilityType = (scope.DisabilityType ? scope.DisabilityType : "");
            scope.RevDisabilityPercentage = scope.DisabilityPercentage;
            scope.RevSadaremCertificate = scope.SadaremCertificate;
        }

        function ShowandHideTabs(type) {
            jQuery('.nav-link').each(function () {
                $(this).removeClass("active");
            });

            disabletabs();
            scope.tabvalue = type;
            if (type == "1") {
                scope.tab_Personal = true;
                $("#personal-details-tab").addClass("active");
            }
            if (type == "2") {
                scope.tab_Education = true;
                $("#employement-tab").addClass("active");
            }
            if (type == "3") {
                scope.tab_disability = true;
                $("#disability-details-tab").addClass("active");
            }
            if (type == "4") {
                scope.tab_PreviousReceived = true;
                $("#previous-received-tab").addClass("active");
            }
        }

        function disabletabs() {
            scope.tab_Personal = false;
            scope.tab_Education = false;
            scope.tab_disability = false;
            scope.tab_PreviousReceived = false;
        }

        function HideSchemes() {
            scope.isLaptop = false;
            scope.isDaisyPlayer = false;
            scope.isTrycycle = false;
        }

        function Step1PersonDetValidations() {
            //return true;

            if (!scope.ApplicantName) {
                swal("Info", "Please Enter Applicant Name", "info");
                return false;
            }
            else if (!scope.parentsName) {
                swal("Info", "Please Enter Applicant Parent Name", "info");
                return false;
            }
            else if (!scope.AadhaarNumber) {
                swal("Info", "Please Enter Applicant Aadhaar Number", "info");
                return false;
            }
            else if (scope.AadhaarNumber.length < 12) {
                swal("Info", "Aadhaar Number Should be 12 Digits", "info");
                return false;
            }
            else if (scope.AadhaarNumber == "111111111111" || scope.AadhaarNumber == "222222222222" || scope.AadhaarNumber == "333333333333" || scope.AadhaarNumber == "444444444444" || scope.AadhaarNumber == "555555555555" || scope.AadhaarNumber == "666666666666" || scope.AadhaarNumber == "777777777777" || scope.AadhaarNumber == "888888888888" || scope.AadhaarNumber == "999999999999" || scope.AadhaarNumber == "000000000000" || !validateVerhoeff(scope.AadhaarNumber)) {
                swal("Info", "Invalid Aadhaar Number", "info");
                return false;
            }
            else if (!scope.Dateofbirth) {
                swal("Info", "Please Select Applicant Date of Birth", "info");
                return false;
            }
            else if (!scope.Gender) {
                swal("Info", "Please Select Applicant Gender", "info");
                return false;
            }
            else if (!scope.MaritalStatus) {
                swal("Info", "Please Select Applicant Marital Status", "info");
                return false;
            }
            else if (!scope.Caste) {
                swal("Info", "Please Select Applicant Caste", "info");
                return false;
            }
            else if (!scope.ddlminority) {
                swal("Info", "Please Select Applicant Minority", "info");
                return false;
            }
            else if (scope.ddlminority == "y" && !scope.Religion) {
                swal("Info", "Please Select Applicant Religion", "info");
                return false;
            }
            else if (!scope.ddldist) {
                swal("Info", "Please Select Native Address District", "info");
                return false;
            }
            else if (!scope.ddlmandal) {
                swal("Info", "Please Select Native Address Mandal", "info");
                return false;
            }
            else if (!scope.ddlvillage) {
                swal("Info", "Please Select Native Address GP/Nagar Panchayat/Municipality/Municipal Corporation", "info");
                return false;
            }
            else if (!scope.habitaton) {
                swal("Info", "Please Enter Native Address Habitation", "info");
                return false;
            }
            else if (!scope.wardnumber) {
                swal("Info", "Please Enter Native Address Ward Number", "info");
                return false;
            }
            else if (!scope.housenumber) {
                swal("Info", "Please Enter Native Address House Number", "info");
                return false;
            }
            else if (!scope.pincode) {
                swal("Info", "Please Enter Native Address Pincode", "info");
                return false;
            }
            else if (!scope.NativePlace) {
                swal("Info", "Please Select Native Local Authority", "info");
                return false;
            }
            else if (!scope.isPerAddr) {
                swal("Info", "Please Select Native Address is Perminent Address", "info");
                return false;
            }
            else if (scope.isPerAddr == "N" && !scope.ddl_per_dist) {
                swal("Info", "Please Select Perminent Address District", "info");
                return false;
            }
            else if (scope.isPerAddr == "N" && !scope.ddl_per_mandal) {
                swal("Info", "Please Select Perminent Address Mandal", "info");
                return false;
            }
            else if (scope.isPerAddr == "N" && !scope.ddl_per_Village) {
                swal("Info", "Please Select Perminent Address GP/Nagar Panchayat/Municipality/Municipal Corporation", "info");
                return false;
            }
            else if (scope.isPerAddr == "N" && !scope.per_Habitation) {
                swal("Info", "Please Enter Perminent Address Habitation", "info");
                return false;
            }
            else if (scope.isPerAddr == "N" && !scope.per_ward) {
                swal("Info", "Please Enter Perminent Address Ward Number", "info");
                return false;
            }
            else if (scope.isPerAddr == "N" && !scope.per_HouseNo) {
                swal("Info", "Please Enter Perminent Address House Number", "info");
                return false;
            }
            else if (scope.isPerAddr == "N" && !scope.per_Pincode) {
                swal("Info", "Please Enter Perminent Address Pincode", "info");
                return false;
            }
            else if (scope.isPerAddr == "N" && !scope.ddl_per_NativePlace) {
                swal("Info", "Please Select Perminent Local Authority", "info");
                return false;
            }
            else if (!scope.isPensioner) {
                swal("Info", "Please Select Social Security Pensioner", "info");
                return false;
            }
            else if (scope.isPensioner == "Y" && !scope.PensionAuthorityperson) {
                swal("Info", "Please Enter Pension Issued Authority", "info");
                return false;
            }
            else if (scope.isPensioner == "Y" && !scope.PensionNumber) {
                swal("Info", "Please Enter Pension Number", "info");
                return false;
            }
            else if (scope.isPensioner == "Y" && !scope.ddlBPLdist) {
                swal("Info", "Please Select Pension Issued District", "info");
                return false;
            }
            else if (scope.isPensioner == "N" && !scope.ddlration) {
                swal("Info", "Please Select White Ration Card Holder", "info");
                return false;
            }
            else if (scope.isPensioner == "N" && scope.ddlration == "Y" && !scope.rationNo) {
                swal("Info", "Please Enter Ration Number", "info");
                return false;
            }
            else if (scope.isPensioner == "N" && scope.ddlration == "Y" && !scope.ddlrationdist) {
                swal("Info", "Please Select Ration Issued District", "info");
                return false;
            }
            else if (scope.isPensioner == "N" && scope.ddlration == "N" && !scope.Income) {
                swal("Info", "Please Select Annual Income of Family", "info");
                return false;
            }
            else if (scope.isPensioner == "N" && scope.ddlration == "N" && !scope.IncomeIssuedDate) {
                swal("Info", "Please Select Income Certificate Issued Date", "info");
                return false;
            }
            else if (scope.isPensioner == "N" && scope.ddlration == "N" && !scope.ddlIncomedist) {
                swal("Info", "Please Select Income Certificate Issued District", "info");
                return false;
            }
            else if (!scope.ddlLicence) {
                swal("Info", "Please Select Driving Licence Type", "info");
                return false;
            }
            else if (scope.ddlLicence == "LLR" && !scope.LicenceLLRNo) {
                swal("Info", "Please Enter LLR Number", "info");
                return false;
            }
            else if (scope.ddlLicence == "LLR" && !scope.LicenceLLRDate) {
                swal("Info", "Please Select LLR Issued Date", "info");
                return false;
            }
            else if (scope.ddlLicence == "Permanent" && !scope.LicenceNo) {
                swal("Info", "Please Enter Driving Licence Number", "info");
                return false;
            }
            else if (scope.ddlLicence == "Permanent" && !scope.ddllicencevalidity) {
                swal("Info", "Please Select Date of Validity", "info");
                return false;
            }
            else if (!scope.ddlsports) {
                swal("Info", "Please Select Winner of State Level Sports meet or above", "info");
                return false;
            }
            else if (scope.ddlsports == "Y" && !scope.SportsLevel) {
                swal("Info", "Please Select Sports Level", "info");
                return false;
            }
            else if (scope.ddlsports == "Y" && !scope.SportsName) {
                swal("Info", "Please Enter Name Of Sports", "info");
                return false;
            }
            else if (scope.ddlsports == "Y" && !scope.SportsEvent) {
                swal("Info", "Please Enter Event of Sport", "info");
                return false;
            }
            else if (scope.ddlsports == "Y" && !scope.SportsEventDate) {
                swal("Info", "Please Select Date of Sports Meet", "info");
                return false;
            }

            return true;
        }

        function Step2EducationValidations() {
            //return true;

            if (!scope.Literate) {
                swal("Info", "Please Select Literate", "info");
                return false;
            }
            else if (scope.Literate == 'Y' && !scope.CurrentlyStudying) {
                swal("Info", "Please Select Currently Studying in School / College", "info");
                return false;
            }
            else if (scope.CurrentlyStudying == 'N' && !scope.HighestEducation) {
                swal("Info", "Please Select Highest Level of Education", "info");
                return false;
            }
            else if ((scope.HighestEducation == '10th CLASS' || scope.HighestEducation == 'INTERMEDIATE' || scope.HighestEducation == 'PROFESSIONAL COURSES' || scope.HighestEducation == 'POST GRADUATE') && !scope.SelfEmployed) {
                swal("Info", "Please Select Self-Employed", "info");
                return false;
            }
            //Wage Employee Validations
            else if (scope.SelfEmployed == 'N' && scope.WageEmployee) {
                swal("Info", "Please Select Wage-Employee", "info");
                return false;
            }
            else if (scope.WageEmployee == "Y" && !scope.WageEmpOrganization) {
                swal("Info", "Please Enter Name of The organization", "info");
                return false;
            }
            else if (scope.WageEmployee == "Y" && !scope.WageEmpDesignation) {
                swal("Info", "Please Enter Your Designation", "info");
                return false;
            }
            else if (scope.WageEmployee == "Y" && !scope.WageDistrict) {
                swal("Info", "Please Select Wage District", "info");
                return false;
            }
            else if (scope.WageEmployee == "Y" && !scope.WageMandal) {
                swal("Info", "Please Select Wage Mandal", "info");
                return false;
            }
            else if (scope.WageEmployee == "Y" && !scope.WageVillage) {
                swal("Info", "Please Select Wage Village", "info");
                return false;
            }
            else if (scope.WageEmployee == "Y" && !scope.WagePanchayat) {
                swal("Info", "Please Enter Grama Panchayat / Town Name", "info");
                return false;
            }
            else if (scope.WageEmployee == "Y" && !scope.WageHabitation) {
                swal("Info", "Please Enter Wage Habitation", "info");
                return false;
            }
            else if (scope.WageEmployee == "Y" && !scope.WageWardNo) {
                swal("Info", "Please Enter Wage Ward Number", "info");
                return false;
            }
            else if (scope.WageEmployee == "Y" && !scope.WageHouseNo) {
                swal("Info", "Please Enter Wage House Number", "info");
                return false;
            }
            else if (scope.WageEmployee == "Y" && !scope.WagePincode) {
                swal("Info", "Please Enter Wage Pincode", "info");
                return false;
            }

            //Self Employee Validations
            else if (scope.SelfEmployed == 'N' && scope.EmployeementUnit) {
                swal("Info", "Please Enter Name of The Self employment Unit", "info");
                return false;
            }
            else if (scope.SelfEmployed == "Y" && !scope.NoofEmployeesUnit) {
                swal("Info", "Please Enter Number of other employees working in the Unit", "info");
                return false;
            }
            else if (scope.SelfEmployed == "Y" && !scope.AnnualTurnover) {
                swal("Info", "Please Enter Annual Turnover Of The Unit", "info");
                return false;
            }
            else if (scope.SelfEmployed == "Y" && !scope.GovtSubsidy) {
                swal("Info", "Please Select Government Subsidy at Any Time for This Unit", "info");
                return false;
            }
            else if (scope.SelfEmployed == "Y" && !scope.SelfBankLoan) {
                swal("Info", "Please Select Have you received Bank Loan at any time to this Unit", "info");
                return false;
            }
            else if (scope.SelfEmployed == "Y" && !scope.SelfIsPerminent) {
                swal("Info", "Please Select Self Employment unit is same as Present Address", "info");
                return false;
            }
            else if (scope.SelfEmployed == "Y" && scope.SelfIsPerminent == 'N' && !scope.SelfDistrict) {
                swal("Info", "Please Select Employment District", "info");
                return false;
            }
            else if (scope.SelfEmployed == "Y" && scope.SelfIsPerminent == 'N' && !scope.SelfMandal) {
                swal("Info", "Please Select Employment Mandal", "info");
                return false;
            }
            else if (scope.SelfEmployed == "Y" && scope.SelfIsPerminent == 'N' && !scope.SelfVillage) {
                swal("Info", "Please Select Employment Village", "info");
                return false;
            }

            else if (scope.SelfEmployed == "Y" && scope.SelfIsPerminent == 'N' && !scope.SelfHabitation) {
                swal("Info", "Please Enter Employment Habitation", "info");
                return false;
            }
            else if (scope.SelfEmployed == "Y" && scope.SelfIsPerminent == 'N' && !scope.SelfWardNo) {
                swal("Info", "Please Enter Employment Ward Number", "info");
                return false;
            }
            else if (scope.SelfEmployed == "Y" && scope.SelfIsPerminent == 'N' && !scope.SelfHouseNo) {
                swal("Info", "Please Enter Employment House Number", "info");
                return false;
            }
            else if (scope.SelfEmployed == "Y" && scope.SelfIsPerminent == 'N' && !scope.SelfPincode) {
                swal("Info", "Please Enter Employment Pincode", "info");
                return false;
            }

            // Studing Validations
            else if (scope.CurrentlyStudying == 'Y' && !scope.PresentEducation) {
                swal("Info", "Please Select Present Education", "info");
                return false;
            }
            else if (scope.CurrentlyStudying == "Y" && (scope.PresentEducation == 'BELOW 8th CLASS' || scope.PresentEducation == '8th CLASS' || scope.PresentEducation == '9th CLASS' || scope.PresentEducation == '10th CLASS') && !scope.NameofSchool) {
                swal("Info", "Please Enter Name of the School", "info");
                return false;
            }
            else if (scope.CurrentlyStudying == "Y" && (scope.PresentEducation == 'INTERMEDIATE' || scope.PresentEducation == 'DEGREE' || scope.PresentEducation == 'PROFESSIONAL COURSES' || scope.PresentEducation == 'POST GRADUATE & ABOVE' || scope.PresentEducation == 'OTHERS' || scope.PresentEducation == 'ITI' || scope.PresentEducation == 'POLYTECHNIC') && !scope.NameofCollege) {
                swal("Info", "Please Enter Name of the College", "info");
                return false;
            }
            else if (scope.CurrentlyStudying == "Y" && scope.PresentEducation != '' && !scope.AdmissionNumber) {
                swal("Info", "Please Enter Admission Number", "info");
                return false;
            }
            else if (scope.CurrentlyStudying == "Y" && (scope.PresentEducation == 'DEGREE' || scope.PresentEducation == 'POST GRADUATE & ABOVE' || scope.PresentEducation == 'OTHERS' || scope.PresentEducation == 'ITI' || scope.PresentEducation == 'POLYTECHNIC') && !scope.StudyingProfessionalCourse) {
                swal("Info", "Please Select studying professional course", "info");
                return false;
            }
            else if (scope.CurrentlyStudying == "Y" && (scope.PresentEducation == 'INTERMEDIATE' || scope.PresentEducation == 'DEGREE' || scope.PresentEducation == 'PROFESSIONAL COURSES' || scope.PresentEducation == 'POST GRADUATE & ABOVE' || scope.PresentEducation == 'OTHERS' || scope.PresentEducation == 'ITI' || scope.PresentEducation == 'POLYTECHNIC') && !scope.CourseName) {
                swal("Info", "Please Enter Course Name", "info");
                return false;
            }
            else if (scope.CurrentlyStudying == "Y" && (scope.PresentEducation == 'DEGREE' || scope.PresentEducation == 'PROFESSIONAL COURSES' || scope.PresentEducation == 'POST GRADUATE & ABOVE' || scope.PresentEducation == 'OTHERS' || scope.PresentEducation == 'ITI' || scope.PresentEducation == 'POLYTECHNIC') && !scope.University) {
                swal("Info", "Please Select Name of the University", "info");
                return false;
            }
            else if (scope.CurrentlyStudying == "Y" && (scope.PresentEducation == 'DEGREE' || scope.PresentEducation == 'PROFESSIONAL COURSES' || scope.PresentEducation == 'POST GRADUATE & ABOVE' || scope.PresentEducation == 'OTHERS' || scope.PresentEducation == 'ITI' || scope.PresentEducation == 'POLYTECHNIC') && !scope.CourseStartDuration) {
                swal("Info", "Please Select Course Duration Start", "info");
                return false;
            }
            else if (scope.CurrentlyStudying == "Y" && (scope.PresentEducation == 'DEGREE' || scope.PresentEducation == 'PROFESSIONAL COURSES' || scope.PresentEducation == 'POST GRADUATE & ABOVE' || scope.PresentEducation == 'OTHERS' || scope.PresentEducation == 'ITI' || scope.PresentEducation == 'POLYTECHNIC') && !scope.CourseEndDuration) {
                swal("Info", "Please Select Course Duration End", "info");
                return false;
            }
            else if (scope.CurrentlyStudying == "Y" && !scope.EduDistrict) {
                swal("Info", "Please Select Education District", "info");
                return false;
            }
            else if (scope.CurrentlyStudying == "Y" && scope.EduDistrict.opt_value != 'notinlist' && !scope.EduMandal) {
                swal("Info", "Please Select Education Mandal", "info");
                return false;
            }
            else if (scope.CurrentlyStudying == "Y" && scope.EduDistrict.opt_value != 'notinlist' && !scope.EduVillage) {
                swal("Info", "Please Select Education Village", "info");
                return false;
            }

            else if (scope.CurrentlyStudying == "Y" && scope.EduDistrict.opt_value != 'notinlist' && !scope.EduHabitation) {
                swal("Info", "Please Enter Education Habitation", "info");
                return false;
            }
            else if (scope.CurrentlyStudying == "Y" && scope.EduDistrict.opt_value != 'notinlist' && !scope.EduWardNo) {
                swal("Info", "Please Enter Education Ward Number", "info");
                return false;
            }
            else if (scope.CurrentlyStudying == "Y" && scope.EduDistrict.opt_value != 'notinlist' && !scope.EduHouseNo) {
                swal("Info", "Please Enter Education House Number", "info");
                return false;
            }
            else if (scope.CurrentlyStudying == "Y" && scope.EduDistrict.opt_value != 'notinlist' && !scope.EduPincode) {
                swal("Info", "Please Enter Education Pincode", "info");
                return false;
            }
            else if (scope.CurrentlyStudying == "Y" && scope.EduDistrict.opt_value == 'notinlist' && !scope.OutEduCityName) {
                swal("Info", "Please Select Education State", "info");
                return false;
            }
            else if (scope.CurrentlyStudying == "Y" && scope.EduDistrict.opt_value == 'notinlist' && !scope.OutEduCityName) {
                swal("Info", "Please Enter Education Town / City Name", "info");
                return false;
            }
            else if (scope.CurrentlyStudying == "Y" && scope.EduDistrict.opt_value == 'notinlist' && !scope.OutEduClgAddress) {
                swal("Info", "Please Enter Address of the College", "info");
                return false;
            }
            return true;
        }

        function Step3DisableValidations() {
            //return true;

            if ($("#Disability").val().length == 0) {
                swal("Info", "Please Select Disability", "info");
                return false;
            }
            else if (!scope.DisabilityPercentage) {
                swal("Info", "Please Enter Disability Percentage", "info");
                return false;
            }
            else if (!scope.SadaremCertificate) {
                swal("Info", "Please Enter Sadarem Certificate Number", "info");
                return false;
            }

            return true;
        }

        function Step4Validations() {
            //return true;

            if (scope.EligibleList.length <= 0) {
                swal("Info", "Based on Your Profile, Currently not Eligible for any scheme", "info");
                return false;
            }

            scope.ApplyEligibleList = [];

            $.each(scope.EligibleList, function (index, value) {
                if (value == "3") {
                    if (scope.PrevRecLaptop == "2")
                        scope.ApplyEligibleList.push("3");
                    else
                        scope.PrevReceiveList.push("3");
                }
                else if (value == "4") {
                    if (scope.PrevRecLaptop == "2")
                        scope.ApplyEligibleList.push("4");
                    else
                        scope.PrevReceiveList.push("4");
                }
                else if (value == "5") {
                    if (scope.PrevRecTricycle == "2")
                        scope.ApplyEligibleList.push("5");
                    else
                        scope.PrevReceiveList.push("5");
                }
                else if (value == "6") {
                    if (scope.PrevRecTouchPhone == "2")
                        scope.ApplyEligibleList.push("6");
                    else
                        scope.PrevReceiveList.push("6");
                }
                else if (value == "7") {
                    if (scope.PrevRecCalipers == "2")
                        scope.ApplyEligibleList.push("7");
                    else
                        scope.PrevReceiveList.push("7");
                }
                else if (value == "8") {
                    if (scope.PrevRecCrutches == "2")
                        scope.ApplyEligibleList.push("8");
                    else
                        scope.PrevReceiveList.push("8");
                }
                else if (value == "9") {
                    if (scope.PrevRecHearing == "2")
                        scope.ApplyEligibleList.push("9");
                    else
                        scope.PrevReceiveList.push("9");
                }
                else if (value == "21") {
                    if (scope.PrevRecBilindSticks == "2")
                        scope.ApplyEligibleList.push("21");
                    else
                        scope.PrevReceiveList.push("21");
                }
                else if (value == "22") {
                    if (scope.PrevRecwheelchair == "2")
                        scope.ApplyEligibleList.push("22");
                    else
                        scope.PrevReceiveList.push("22");
                }

            });

            return true;
        }

        function MainStep2Validations() {
            //return true;
            if (scope.ApplyEligibleList.length <= 0) {
                swal("Info", "Based on Your Profile, Currently not Eligible for any scheme", "info");
                return false;
            }

            scope.ApplyList = [];

            $.each(scope.ApplyEligibleList, function (index, value) {
                if (value == "3") {
                    if (scope.ApplyLaptop == "1")
                        scope.ApplyList.push("3");
                }
                else if (value == "4") {
                    if (scope.ApplyDaisyPlayers == "1")
                        scope.ApplyList.push("4");
                }
                else if (value == "5") {
                    if (scope.ApplyTricycle == "1")
                        scope.ApplyList.push("5");
                }
                else if (value == "6") {
                    if (scope.ApplyTouchPhone == "1")
                        scope.ApplyList.push("6");
                }
                else if (value == "7") {
                    if (scope.ApplyCalipers == "1")
                        scope.ApplyList.push("7");
                }
                else if (value == "8") {
                    if (scope.ApplyCrutches == "1")
                        scope.ApplyList.push("8");
                }
                else if (value == "9") {
                    if (scope.ApplyHearing == "1")
                        scope.ApplyList.push("9");
                }
                else if (value == "21") {
                    if (scope.ApplyBilindSticks == "1")
                        scope.ApplyList.push("21");
                }
                else if (value == "22") {
                    if (scope.Applywheelchair == "1")
                        scope.ApplyList.push("22");
                }

            });

            if (scope.ApplyList.length <= 0) {
                swal("Info", "Please apply atleast once scheme", "info");
                return false;
            }

            return true;

        }

        function MainStep3Validations() {
            //return true;
            if (!File1Base64) {
                swal("Info", "Please Upload Aadhar Card", "info");
                return false;
            }
            else if (!File2Base64) {
                swal("Info", "Please Upload SSC/Certificate issued by Local Body Authority", "info");
                return false;
            }
            else if (!File3Base64) {
                swal("Info", "Please Upload Photo Showing the Disability", "info");
                return false;
            }
            else if (!File4Base64) {
                swal("Info", "Please Upload SADAREM Certificate", "info");
                return false;
            }
            else if (!File5Base64) {
                swal("Info", "Please Upload Pension / White Ration card / Income Certificate", "info");
                return false;
            }
            else if (!File6Base64) {
                swal("Info", "Please Upload Caste Certificate Issued by Tahsildar", "info");
                return false;
            }

            return true;
        }
    }
})();
