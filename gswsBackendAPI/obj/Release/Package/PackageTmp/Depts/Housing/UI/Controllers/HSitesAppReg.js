(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("HSitesAppReg", ["$scope", "$state", "$log", "Housing_Services", House_CTRL]);

    function House_CTRL(scope, state, log, ho_services) {
        scope.pagename = "Housing Sites";
        scope.preloader = false;
        scope.dttable = false;
        scope.isrural = true;

        scope.RelationsDD = HSRelations;
        scope.CastsDD = HSCommunitys;
        scope.DistrictDD = HSDistricts;
        scope.OccupationDD = HSOccupation;
        scope.MandalDD = [];
        scope.VillageDD = [];
        scope.PanchayatDD = [];
        scope.MunicyDD = [];
        scope.WardDD = [];


        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

        scope.LoadSubCasts = function () {
            scope.SubCastsDD = [];
            if (scope.SelBenCast) {
                scope.SubCastsDD = $(HSSubCasts).filter(function (i, n) { return n.Caste_Category_AlphaCode === scope.SelBenCast });
            }
        }

        scope.LoadMandals = function () {
            scope.MandalDD = [];

            if (scope.SelBenRType == "Urban")
                scope.isrural = false;
            else
                scope.isrural = true;


            if (scope.SelBenRType && scope.SelBenDistrict) {
                if (scope.SelBenRType == "Rural")
                    scope.MandalDD = $(HSDMVDD).filter(function (i, n) { return n.District_Code === scope.SelBenDistrict && n.LocalType === scope.SelBenRType });
                else
					scope.MunicyDD = $(HSDMVDD).filter(function (i, n) { return n.District_Code === scope.SelBenDistrict && n.LocalType === scope.SelBenRType && n.Mandal_Code=='5010' });
            }
        }

        scope.LoadVillages = function () {
            scope.VillageDD = [];
            if (scope.SelBenRType && scope.SelBenDistrict && scope.SelBenMandal.Mandal_Code) {
                scope.VillageDD = $(HSDMVDD).filter(function (i, n) { return n.LocalType === scope.SelBenRType && n.District_Code === scope.SelBenDistrict && n.Mandal_Code === scope.SelBenMandal.Mandal_Code });
            }
        }

        scope.LoadWards = function () {
            scope.WardDD = [];
            if (scope.SelBenRType && scope.SelBenDistrict && scope.SelBenMunicy.Mandal_Code) {
                scope.WardDD = $(HSDMVDD).filter(function (i, n) { return n.LocalType === scope.SelBenRType && n.District_Code === scope.SelBenDistrict && n.Mandal_Code === scope.SelBenMunicy.Mandal_Code });
            }
        }

        scope.LoadPanchayat = function () {
            scope.PanchayatDD = [];
            if (scope.SelBenRType && scope.SelBenDistrict && scope.SelBenMandal.Mandal_Code && scope.SelBenVillage.Village_Code) {
                scope.PanchayatDD = $(HSDMVDD).filter(function (i, n) { return n.LocalType === scope.SelBenRType && n.District_Code === scope.SelBenDistrict && n.Mandal_Code === scope.SelBenMandal.Mandal_Code && n.Village_Code === scope.SelBenVillage.Village_Code });
            }
        }


        scope.Register = function () {
            if (Validations()) {
				var req = {
					Appno: sessionStorage.getItem("TransID"),
                    VolunteerId: scope.VolIDNo,
                    VName: scope.VolName,
                    VMobile: scope.VolMobile,
                    VAadhaar: scope.VolAadhaar,
                    SachivalayamCodeno: scope.VolScCode,
                    SachivalayamName: scope.VolScName,

                    BAdhaarno: scope.BenAadhaar,
                    BenficiaryName: scope.BenName,
                    Bmobile: scope.BenMobile,
                    RelationID: scope.SelBenRel,
                    RelationName: scope.BenRelName,
                    Age: scope.BenAge,
                    Gender: scope.SelBenGen,
                    Religion: scope.BenReligion,
                    isPhysChall: scope.BenDisable,
                    CasteID: scope.SelBenCast,
                    SubCasteID: scope.SelBenSubCast,
                    Occupation: scope.SelBenOccupation,
                    OtherOccupation: (scope.SelBenOccupation == '10' ? scope.SelBenOtherOccupation : ""),

                    Houseno: scope.BenHouse,
                    Street: scope.BenStreet,
                    DistrictID: scope.SelBenDistrict,
                    MandalID: scope.SelBenRType == "Rural" ? scope.SelBenMandal.Mandal_Code : scope.SelBenMunicy.Mandal_Code,
                    VillageID: scope.SelBenRType == "Rural" ? scope.SelBenVillage.Village_Code : scope.SelBenWard.Village_Code,
                    PanchayathID: scope.SelBenRType == "Rural" ? scope.SelBenPanchayat : 0,
                    Pincode: scope.BenPin,					
                    isRation: scope.BenIsWCard,
                    Rationcardno: scope.BenWCardNo,
                    isHouse: scope.BenIsOHouse,
                    isHouseSite: scope.BenIsOHSite,
                    isHousingScheme: scope.BenIsGHouse,
                    isHouseSiteScheme: scope.BenIsGHSite,
                    isLand: scope.BenIsLand,
                    isIncome: scope.SelBenRType == "Rural" ? scope.BenIsIncome : 0,
                    isPMAY: scope.SelBenRType == "Rural" ? scope.BenIsPMAY : 0,
                    PMAYBenefitSchemeID: scope.SelBenRType == "Rural" ? scope.SelPMAYName : 0,
                    isAHPAllotmentReceived: scope.SelBenRType == "Rural" ? scope.BenIsAHP : 0,
                    AHPAllotmentDetails: scope.SelBenRType == "Rural" ? scope.AHPAllowet : 0,
					USERID: sessionStorage.getItem("user"),//sessionStorage.getItem("secccode"),
                    file: HouseUplFile,
                    MandaltypeID: scope.SelBenRType == "Rural" ? "R" : "U",

                };
                ho_services.POSTENCRYPTAPI("HSitesApplicationReg", req, token, function (value) {
                    if (value.data.Status == 100) {
                        var data = jQuery.parseJSON(value.data.Details);
                        console.log(value.data.Details);
                        if (data.status == "0") {
                            swal({
                                title: "alert!",
                                text: "Application Registred Successfully. Application Number : " + data.appno,
                                icon: "warning",
                                buttons: true,
                                dangerMode: false,
                            }).then((willDelete) => {
                                if (willDelete) {
                                   // window.location.reload();
									//window.location = "../GSWS/#!/MainDashboard";
									state.go("ue.Dashboard");

                                } else {
                                   // window.location.reload();
									state.go("ue.Dashboard");
                                }
                            });
                        }
                        else
                            swal('info', data.remarks, 'info');
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


        function Validations() {
            var emailRegex = new RegExp("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");

            if (!scope.VolAadhaar) {
                swal("Info", "Please Enter Volunteer Aaadhaar No", "info");
                return false;
            }
            else if (scope.VolAadhaar.length < 12) {
                swal("Info", "Volunteer Aadhaar No Shoud be 10 Digits", "info");
                return false;
            }
            else if (!scope.VolMobile) {
                swal("Info", "Please Enter Volunteer Mobile No", "info");
                return false;
            }
            else if (scope.VolMobile.length < 10) {
                swal("Info", "Volunteer Mobile No Shoud be 10 Digits", "info");
                return false;
            }
            else if (!scope.VolName) {
                swal("Info", "Please Enter Volunteer Name", "info");
                return false;
            }
            else if (!scope.VolIDNo) {
                swal("Info", "Please Enter Volunteer Identification Number", "info");
                return false;
            }
            else if (!scope.VolScName) {
                swal("Info", "Please Enter Name of Village Secretariat", "info");
                return false;
            }
            else if (!scope.BenAadhaar) {
                swal("Info", "Please Enter Beneficiary Aadhaar Number", "info");
                return false;
			}
			
			else if (scope.BenAadhaar.length < 12) {
				swal("Info", "Beneficiary Aadhaar No Shoud be 12 Digits", "info");
				return false;
			}
			else if (!validateVerhoeff(scope.BenAadhaar)) {
				swal("Info", "Invalid Beneficiary Aadhaar Number", "info");
				return false;
			}
			
            else if (scope.VolAadhaar.length < 12) {
                swal("Info", "Beneficiary Aadhaar No Shoud be 12 Digits", "info");
                return false;
            }
            else if (!scope.BenName) {
                swal("Info", "Please Enter Beneficiary Name", "info");
                return false;
            }
            else if (!scope.SelBenRel) {
                swal("Info", "Please Select Relationship With Beneficiary", "info");
                return false;
            }
            else if (!scope.BenRelName) {
                swal("Info", "Please Enter Relation Name", "info");
                return false;
            }
            else if (!scope.BenAge) {
                swal("Info", "Please Enter Beneficiary Age", "info");
                return false;
            }
            else if (!scope.SelBenGen) {
                swal("Info", "Please Select Gender", "info");
                return false;
            }
            else if (!scope.BenReligion) {
                swal("Info", "Please Enter Religion", "info");
                return false;
            }
            else if (!scope.SelBenCast) {
                swal("Info", "Please Select Cast", "info");
                return false;
            }
            else if (!scope.SelBenSubCast) {
                swal("Info", "Please Select Sub Cast", "info");
                return false;
            }
            else if (!scope.SelBenOccupation) {
                swal("Info", "Please Select Occupation", "info");
                return false;
            }
            else if (scope.SelBenOccupation == '10' && !scope.SelBenOtherOccupation) {
                swal("Info", "Please Enter Other Occupation", "info");
                return false;
            }
            else if (!scope.BenDisable) {
                swal("Info", "Please Select IsDisabled", "info");
                return false;
            }
            else if (scope.BenMobile && scope.BenMobile.length < 10) {
                swal("Info", "Benificiary Mobile No Should be 10 Digits", "info");
                return false;
            }
            else if (!scope.BenHouse) {
                swal("Info", "Please Enter House Number", "info");
                return false;
            }
            else if (!scope.SelBenDistrict) {
                swal("Info", "Please Select District", "info");
                return false;
            }
            else if (!scope.SelBenRType) {
                swal("Info", "Please Select Rural/Urban", "info");
                return false;
            }

            else if (scope.SelBenRType == "Rural" && !scope.SelBenMandal.Mandal_Code) {
                swal("Info", "Please Select Mandal", "info");
                return false;
            }
            else if (scope.SelBenRType == "Urbal" && !scope.SelBenMunicy.Mandal_Code) {
                swal("Info", "Please Select Municipality", "info");
                return false;
            }

            else if (scope.SelBenRType == "Rural" && !scope.SelBenVillage.Village_Code) {
                swal("Info", "Please Select Village", "info");
                return false;
            }
            else if (scope.SelBenRType == "Urbal" && !scope.SelBenWard.Village_Code) {
                swal("Info", "Please Select Ward", "info");
                return false;
            }

            else if (scope.SelBenRType == "Rural" && !scope.SelBenPanchayat) {
                swal("Info", "Please Select Panchayat", "info");
                return false;
            }

            else if (!scope.BenIsWCard) {
                swal("Info", "Please Select Does the beneficiary have a white ration card", "info");
                return false;
            }
            else if (scope.BenIsWCard && !scope.BenWCardNo) {
                swal("Info", "Please Enter Ration Card Number", "info");
                return false;
            }
            else if (!scope.BenIsOHouse) {
                swal("Info", "Please Select Does the beneficiary own a house in the state of Andhra Pradesh", "info");
                return false;
            }
            else if (!scope.BenIsOHSite) {
                swal("Info", "Please Select Does the beneficiary own a house site in the state of Andhra Pradesh", "info");
                return false;
            }
            else if (!scope.BenIsGHouse) {
                swal("Info", "Please Select Does the beneficiary already have a government-sanctioned home", "info");
                return false;
            }
            else if (!scope.BenIsGHSite) {
                swal("Info", "Please Select Does the beneficiary already have a government-sanctioned home site", "info");
                return false;
            }
            else if (!scope.BenIsLand) {
                swal("Info", "Please Select Does the beneficiary own 2.5 acres of Wetland or 5.0 acres of Dryland", "info");
                return false;
            }
            else if (scope.isrural && !scope.BenIsIncome) {
                swal("Info", "Please Select Whether the beneficiary has an annual income of less than three (3) lakhs", "info");
                return false;
            }
            else if (scope.isrural && !scope.BenIsPMAY) {
                swal("Info", "Please Select Does the beneficiary own a house previously granted under the PMAY schemes", "info");
                return false;
            }
            else if (scope.isrural && scope.BenIsPMAY && !scope.SelPMAYName) {
                swal("Info", "Please Select Benefit Scheme", "info");
                return false;
            }
            else if (scope.isrural && scope.SelPMAYName == "1" && !scope.BenIsAHP) {
                swal("Info", "Please Select Have you assigned the assignment order under AHP", "info");
                return false;
            }
            else if (scope.isrural && scope.BenIsAHP && !scope.AHPAllowet) {
                swal("Info", "Please Allotment Deatils", "info");
                return false;
            }
            else if (!$("#UplDoc").val()) {
                swal("Info", "Please Upload Application Form", "info");
                return false;
            }


            return true;
        }
    }
})();