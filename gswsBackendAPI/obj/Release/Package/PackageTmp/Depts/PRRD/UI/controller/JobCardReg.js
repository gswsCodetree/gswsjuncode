(function () {
    var app = angular.module("GSWS");

    app.controller("Jobcard", ["$scope", "$state", "PRRD_Services", "entityService", '$sce', "$filter", JobCardReg_CTRL]);


    angular.module("GSWS")
        .factory("entityService", ["akFileUploaderService", function (akFileUploaderService) {
            var saveTutorial = function (tutorial, url) {
                return akFileUploaderService.saveModel(tutorial, "/GSWSUAT" + url);
                //return akFileUploaderService.saveModel(tutorial, "" + url);
            };
            return {
                saveTutorial: saveTutorial
            };
        }]);
    angular.module("akFileUploader", [])
        .factory("akFileUploaderService", ["$q", "$http",
            function ($q, $http) {
                var getModelAsFormData = function (data) {
                    var dataAsFormData = new FormData();
                    angular.forEach(data, function (value, key) {
                        dataAsFormData.append(key, value);
                    });
                    return dataAsFormData;
                };

                var saveModel = function (data, url) {
                    var deferred = $q.defer();
                    $http({
                        url: url,
                        method: "POST",
                        data: getModelAsFormData(data),
                        transformRequest: angular.identity,
                        headers: { 'Content-Type': undefined }
                    }).then(function (result) {

                        deferred.resolve(result);
                    }, function (result, status) {
                        deferred.reject(status);
                    });
                    return deferred.promise;
                };

                return {
                    saveModel: saveModel
                }

            }]).directive("akFileModel", ["$parse",
                function ($parse) {
                    return {
                        restrict: "A",
                        link: function (scope, element, attrs) {
                            var model = $parse(attrs.akFileModel);
                            var modelSetter = model.assign;
                            element.bind("change", function () {
                                scope.$apply(function () {
                                    modelSetter(scope, element[0].files[0]);
                                });
                            });
                        }
                    };
                }]);
    function JobCardReg_CTRL(scope, state, PRRD_Services, entityService, sce, filter) {
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");
        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return;
        }

        var usertype = user.split('-')[1].toString();
        if (usertype != "DA") {
            scope.IsSave = false;
            scope.IsApprove = true;
            scope.IsReject = true;
            scope.IsView = true;
            scope.IsUpload = false;
            scope.IsGrid = true;
            scope.IsAddRemove = false;
            scope.IsEdit = false;
            scope.IsCheck = false;
            scope.IsUpdate = false;
            scope.Pensiondisabled = true;
            scope.Mobiledisabled = true;
            scope.Acresdisabled = true;
            scope.SHGDisabled = true;
            scope.MemberDisabled = true;
            isReadonly();

        } else {
            scope.IsSave = false;
            scope.IsApprove = false;
            scope.IsReject = false;
            scope.IsView = false;
            scope.IsUpload = true;
            scope.IsGrid = true;
            scope.IsEdit = true;
            scope.IsCheck = true;
            scope.IsUpdate = true;
            scope.chkLandowner = "no";
            scope.Acresdisabled = true;
            scope.chkAssignedLands = "no";
            scope.chkIAYBeneficiary = "no";
            scope.HIVAffected = "no";
            scope.Disabled = "no";
            scope.Pensiondisabled = true;
            scope.MemberofSHG = "no";
            scope.SHGDisabled = true;
            scope.PermanentlyJobcardid = "no";
            scope.chkMobileNo = "no";
            scope.Mobiledisabled = true;
        }
        scope.registrationdate = new Date();
        scope.panchayatlist = [];
        scope.habitationlist = [];
        scope.banklist = [];
        scope.bankbranchlist = [];
        scope.addmultiplejobcard = [];
        scope.getJobcarddetails = [];
        scope.relationwithfamilyheadlist = [];
        scope.districtname = sessionStorage.getItem("distname");
        scope.mandalname = sessionStorage.getItem("mname");
        var distcode = sessionStorage.getItem("distcode");
        var mcode = sessionStorage.getItem("mcode");
        var gpcode = sessionStorage.getItem("gpcode");
        var userid = sessionStorage.getItem("user");
        var sachivalaymid = sessionStorage.getItem("secccode");
        var JC_ID = sessionStorage.getItem("JC_ID");
        var type = sessionStorage.getItem("Type");
        var PRRDdistcode = "";
        var PRRDmcode = "";
        if (mcode != null) {
            loadDistandMandalCode();
            scope.villagecode = "";
            loadpanchayatmaster();
        }

        function loadDistandMandalCode() {

            var input = {
                FTYPE: 3, DISTCODE: distcode, MandalCode: mcode

            };

            PRRD_Services.POSTENCRYPTAPI("JobcardDistandMandalCode", input, token, function (value) {
                debugger;
                var res = value.data;
                console.log(res.Details[0].DISTRICT_NAME);
                if (res.Status == "100") {
                    scope.PRRDdistrictname = res.Details[0].DISTRICT_NAME;
                    scope.PRRDmandalname = res.Details[0].MANDAL_NAME;
                    PRRDdistcode = res.Details[0].DISTRICT_ID;
                    PRRDmcode = res.Details[0].MANDAL_ID;
                    return;
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

        var listofrelations = [{ 'userid': 1, 'name': 'Father' }, { 'userid': 2, 'name': 'Mother' }, { 'userid': 3, 'name': 'Son' }, { 'userid': 4, 'name': 'Daughter' }, { 'userid': 5, 'name': 'Brother' }, { 'userid': 6, 'name': 'Elder Brother' }, { 'userid': 7, 'name': 'Younger Brother' }, { 'userid': 8, 'name': 'Sister' }, { 'userid': 9, 'name': 'Elder Sister' }, { 'userid': 10, 'name': 'Son-in-Law' }, { 'userid': 11, 'name': 'Daughter-in-Law' }, { 'userid': 12, 'name': 'Brother-in-Law' }, { 'userid': 13, 'name': 'Sister-in-Law' }, { 'userid': 14, 'name': 'Father-in-Law' }, { 'userid': 15, 'name': 'Mother-in-Law' }, { 'userid': 16, 'name': 'Grand Father' }, { 'userid': 17, 'name': 'Grand Mother' }, { 'userid': 18, 'name': 'Grand Son' }, { 'userid': 19, 'name': 'Grand Daughter' }, { 'userid': 20, 'name': 'Wife' }];
        var headrelation = [{ 'userid': 0, 'name': 'Self' }];

        scope.yesheadoffamily = function () {
            scope.relationwithfamilyheadlist = headrelation;
        }
        scope.noheadoffamily = function () {
            scope.relationwithfamilyheadlist = listofrelations;
        }
        loadbankmaster();

        if (JC_ID != "") {
            loadJobCardDetails();
            scope.IsAddRemove = false;
            scope.IsCheck = false;
        }
        else {
            scope.IsAddRemove = true;
            scope.IsUpdate = false;
            scope.IsEdit = false;
        }

        scope.GetBranchload = function () {
            scope.branchname = "";
            scope.IfscCode = "";
            loadbranchdetails(scope.bankname);
            //ValidBankAccountNumbers();
        }

        scope.GetIFSCCodeload = function (str) {
            scope.IfscCode = "";
            loadifsccodedetails(str);
        }
        scope.GetBankNameandBranchload = function (str) {
            scope.branchname = "";
            scope.bankname = "";
            LoadBankNameandBranchdetails(str);
        }

        scope.GetHabitationload = function () {
            scope.habitation = "";
            LoadHabitation(scope.panchayatcode);
        }

        // All Controllers are readonly mode
        function isReadonly() {
            $('input, select, textarea').attr('disabled', 'disabled');
        };
        //bind district and mandal codes


        // Required Field Validators
        function requiredFieldValidators() {


            if (!scope.panchayatcode) {
                swal("Info", "Please select Grama Panchayat", "info");
                return false;
            }
            else if (!scope.habitation) {
                swal("Info", "Please select Habitation", "info");
                return false;
            }
            else if (!scope.applicationnumber) {
                swal("Info", "Please enter Applicationnumber", "info");
                return false;
            }
            else if (!scope.panchayatsecretary) {
                swal("Info", "Please enter Panchayat Secretary", "info");
                return false;
            }
            //HouseHold Details
            else if (!scope.ngcast) {
                swal("Info", "Please select Caste", "info");
                return false;
            }
            else if (!scope.EnglishAddress) {
                swal("Info", "Please enter Address in English", "info");
                return false;
            }
            else if (!scope.EnglishTypingAddress) {
                swal("Info", "Please enter Address English to Telugu Converter", "info");
                return false;
            }
            //else if (!scope.RationcardNumber) {
            //    swal("Info", "Please enter Rationcard Number", "info");
            //    return false;
            //}
            //else if (!scope.BplNo) {
            //    swal("Info", "Please enter BPL No", "info");
            //    return false;
            //}
            //else if (!scope.KathaNo) {
            //    swal("Info", "Please enter Khata Number", "info");
            //    return false;
            //}
            else if (!scope.chkLandowner) {
                swal("Info", "Please select Land Owner", "info");
                return false;
            }
            else if (!scope.Acres && scope.chkLandowner == "yes") {
                swal("Info", "Please enter Acres", "info");
                return false;
            }
            else if (!scope.chkAssignedLands) {
                swal("Info", "Please select Assigned Lands/Land Reforms Beneficiary", "info");
                return false;
            }
            else if (!scope.chkIAYBeneficiary) {
                swal("Info", "Please select IAY Beneficiary", "info");
                return false;
            }

            //Member Details
            else if (!scope.Name) {
                swal("Info", "Please enter Name", "info");
                return false;
            }
            else if (!scope.EnglishInputName) {
                swal("Info", "Please enter Telugu Name", "info");
                return false;
            }
            else if (!scope.SurName) {
                swal("Info", "Please enter Surname", "info");
                return false;
            }
            else if (!scope.TeluguSurName) {
                swal("Info", "Please enter Telugu Surname", "info");
                return false;
            }
            else if (!scope.HeadofFamily) {
                swal("Info", "Please select Head of the Family", "info");
                return false;
            }
            else if (!scope.RelationwithHeadofFamily) {
                swal("Info", "Please select Relationship with the Head of the family", "info");
                return false;
            }
            else if (!scope.Gender) {
                swal("Info", "Please select Gender", "info");
                return false;
            }
            else if (!scope.HIVAffected) {
                swal("Info", "Please select HIV Affected", "info");
                return false;
            }
            else if (!scope.age) {
                swal("Info", "Please enter Age", "info");
                return false;
            }
            else if (!scope.Disabled) {
                swal("Info", "Please select Disabled", "info");
                return false;
            }
            else if (!scope.MemberofSHG) {
                swal("Info", "Please select Member of SHG", "info");
                return false;
            }
            else if (!scope.SHGName && scope.MemberofSHG == "yes") {
                swal("Info", "Please enter SHG Name", "info");
                return false;
            }
            else if (!scope.SHGID && scope.MemberofSHG == "yes") {
                swal("Info", "Please enter SHG ID", "info");
                return false;
            }
            //else if (!scope.MPHSSID) {
            //    swal("Info", "Please enter MPHSS ID", "info");
            //    return false;
            //}
            else if (!scope.TypeofPayingAgency) {
                swal("Info", "Please select Type of Paying Agency", "info");
                return false;
            }
            else if (!scope.bankname) {
                swal("Info", "Please select Paying Agency Name", "info");
                return false;
            }
            else if (!scope.branchname) {
                swal("Info", "Please select Branch Name", "info");
                return false;
            }
            else if (!scope.IfscCode) {
                swal("Info", "Please enter IFSC Code", "info");
                return false;
            }
            else if (!scope.NameasperBankPassbook) {
                swal("Info", "Please enter Name as per Bank Passbook", "info");
                return false;
            }
            else if (!scope.BankAccountNumber) {
                swal("Info", "Please enter Bank Account Number", "info");
                return false;
            }
            //else if (!scope.VoterID) {
            //    swal("Info", "Please enter Voter ID", "info");
            //    return false;
            //}
            else if (!scope.uid1) {
                swal("Info", "Please enter UID", "info");
                return false;
            } else if (!scope.uid2) {
                swal("Info", "Please enter UID", "info");
                return false;
            } else if (!scope.uid3) {
                swal("Info", "Please enter UID", "info");
                return false;
            }
            else if (!scope.MobileNo && scope.chkMobileNo == 'yes') {
                swal("Info", "Please enter Mobile No", "info");
                return false;
            }
            else if (scope.passbookpath == null || scope.passbookpath == "undefined") {

                swal("Info", "Please upload pass book", "info");
                return false;
            }
            else if (scope.uidpath == null || scope.uidpath == "undefined") {

                swal("Info", "Please upload uid", "info");
                return false;
            }
            else if (scope.f1path == null || scope.f1path == "undefined") {

                swal("Info", "Please upload f1 path", "info");
                return false;
            }

            return true;
        }


        // Clear Member Details after add or update
        function clearMemberDetails() {
            scope.Name = "";
            scope.SurName = "";
            scope.TeluguName = "";
            scope.TeluguSurName = "";
            scope.RelationwithHeadofFamily = "";
            scope.Gender = "";
            scope.age = "";
            scope.SHGName = "";
            scope.SHGID = "";
            scope.MPHSSID = "";
            scope.TypeofPayingAgency = "";
            scope.bankname = "";
            scope.branchname = "";
            scope.IfscCode = "";
            scope.NameasperBankPassbook = "";
            scope.BankAccountNumber = "";
            scope.VoterID = "";
            scope.uid1 = "";
            scope.uid2 = "";
            scope.uid3 = "";
            scope.MobileNo = "";

            // Radio buttons
            //scope.HeadofFamily = "no";
            scope.HIVAffected = "no";
            scope.Disabled = "no";
            scope.Pensiondisabled = true;
            scope.MemberofSHG = "no";
            scope.SHGDisabled = true;
            scope.PermanentlyJobcardid = "no";
            scope.chkMobileNo = "no";
            scope.Mobiledisabled = true;

            //conversions
            //scope.EnglishTypingAddress = "";
            //scope.Teluguddress = "";
            scope.EnglishInputName = "";
            scope.TranslateTeluguName = "";
            scope.TeluguSurName = "";
            scope.TranslateTeluguNameSurname = "";
        }

        // GET VALUES FROM INPUT BOXES AND ADD A NEW ROW TO THE TABLE.
        scope.addRow = function () {
            if (requiredFieldValidators()) {
                if (scope.RelationwithHeadofFamily != "Self" && scope.addmultiplejobcard.length == 0) {
                    swal("Info", "Please add Head of the family memeber first", "info");
                    return false;
                }
                var singlejobcard = [];
                var output;
                angular.forEach(scope.addmultiplejobcard, function (item) {
                    if (scope.RelationwithHeadofFamily == "Self") {
                        output = "familyHead";
                        return false;
                    }
                    if (scope.BankAccountNumber == item.BANK_ACC_NO) {
                        output = "bank";
                        return false;
                    }
                    if (scope.uid1 + scope.uid2 + scope.uid3 == item.UID_NO) {
                        output = "uid";
                        return false;
                    }
                });
                if (output == "familyHead") {
                    swal("Info", "Head of the family member already exists in below table", "info");
                    output = "";
                    return false;
                }
                else if (output == "bank") {
                    swal("Info", "Bankaccountnumber already exists in below table", "info");
                    output = "";
                    return false;
                }
                else if (output == "uid") {
                    swal("Info", "Aadhar already exists in below table", "info");
                    output = "";
                    return false;
                }
                singlejobcard.MEMBER_NAME = scope.Name;
                singlejobcard.SUR_NAME = scope.SurName;
                singlejobcard.MEMBER_NAME_TEL = scope.TranslateTeluguName;
                singlejobcard.SUR_NAME_TEL = scope.TranslateTeluguNameSurname;
                singlejobcard.ADDRESS_TEL = scope.Teluguddress;
                singlejobcard.ADDRESS = scope.EnglishAddress;
                singlejobcard.FAMILY_HEAD = scope.HeadofFamily;
                singlejobcard.RELATION_HOF = scope.RelationwithHeadofFamily;
                singlejobcard.GENDER = scope.Gender;
                singlejobcard.AGE = scope.age;
                singlejobcard.APPLICATION_NUMBER = scope.applicationnumber;
                singlejobcard.CASTE = scope.ngcast;
                singlejobcard.RATIONCARD_NO = scope.RationcardNumber;
                singlejobcard.BPL_NO = scope.BplNo;
                singlejobcard.KHATA_NO = scope.KathaNo;
                singlejobcard.LAND_OWNER = scope.chkLandowner;
                if (scope.chkLandowner == "yes" || scope.chkLandowner == "YES")
                    singlejobcard.LAND_OWNER_ACRES = scope.Acres;
                else
                    singlejobcard.LAND_OWNER_ACRES = "";
                singlejobcard.ASSIGNED_LAND_BENEFICIARY = scope.chkAssignedLands;
                singlejobcard.IAY_BENEFICIARY = scope.chkIAYBeneficiary;
                singlejobcard.SHG_NAME = scope.SHGName;
                singlejobcard.SHG_ID = scope.SHGID;
                singlejobcard.SHG_MEMBER = scope.MemberofSHG;
                singlejobcard.MPHSS_ID = scope.MPHSSID;
                singlejobcard.VOTER_ID = scope.VoterID;
                singlejobcard.DISABLED = scope.Disabled;
                singlejobcard.PENSION_NUMBER = scope.PensionNumber; //Pension number
                singlejobcard.PAYING_AGENCY_TYPE = scope.TypeofPayingAgency;
                singlejobcard.PAYING_AGENCY_NAME = scope.bankname; //bank name
                singlejobcard.BANK_ACC_NO = scope.BankAccountNumber;
                singlejobcard.BRANCH_NAME = scope.branchname;
                singlejobcard.IFSC_CODE = scope.IfscCode;
                singlejobcard.PERMANENT_JOB_CARD = scope.PermanentlyJobcardid;
                singlejobcard.HIV = scope.HIVAffected;
                singlejobcard.UID_NO = scope.uid1 + scope.uid2 + scope.uid3;
                singlejobcard.MOBILE_NO = scope.MobileNo;
                singlejobcard.MOBILE_NO_Req = scope.chkMobileNo;
                singlejobcard.BANK_ACC_NAME = scope.NameasperBankPassbook;
                singlejobcard.GP_CODE = scope.panchayatcode;
                singlejobcard.GP_NAME = scope.panchayatcode;
                singlejobcard.HB_CODE = scope.habitation;
                singlejobcard.GP_SECRETARY_NAME = scope.panchayatsecretary;
                singlejobcard.P_UID_PATH = scope.uidpath;
                singlejobcard.P_PASSBOOK_PATH = scope.passbookpath;
                singlejobcard.P_F1_PATH = scope.f1path;
                singlejobcard.P_PENSION_NUMBER = scope.PensionNumber;
                scope.addmultiplejobcard.push(singlejobcard);
                clearMemberDetails();
            }
        };

        // REMOVE SELECTED ROW(s) FROM TABLE.
        scope.removeRow = function () {
            var arrRemoveDetails = [];
            angular.forEach(scope.addmultiplejobcard, function (value) {
                if (!value.Remove) {
                    arrRemoveDetails.push(value);
                }
            });
            scope.addmultiplejobcard = arrRemoveDetails;
        };

        // FINALLY SUBMIT THE DATA.
        scope.saveJobcard = function () {
            if (!sessionStorage.getItem("TransID"))
                //window.location.href = "../#!/MainDashboard";
                window.open('', '_self').close();
            else
                SubmitJobcardDetails();
            return;
        };

        function loadpanchayatmaster() {
            var input = {
                FTYPE: 1, DISTCODE: distcode, MandalCode: mcode,
                UserId: sessionStorage.getItem("user"),
                SacId: sessionStorage.getItem("secccode"),
                DesignId: sessionStorage.getItem("desinagtion"),
                TranId: ""
            };

            PRRD_Services.POSTENCRYPTAPI("GetHabitationCode", input, token, function (value) {
                var res = value.data;
                if (res.Status == "100") {
                    scope.panchayatlist = res.Details;
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

        function loadbankmaster() {
            var input = {
                TYPE: 1,
                UserId: sessionStorage.getItem("user"),
                SacId: sessionStorage.getItem("secccode"),
                DesignId: sessionStorage.getItem("desinagtion"),
                TranId: ""
            };

            PRRD_Services.POSTENCRYPTAPI("GetJobCardBankMaster", input, token, function (value) {
                var res = value.data;
                if (res.Status == "100") {
                    scope.banklist = res.BankDetails;
                }
                else if (res.Status == "428") {
                    swal('info', "Failed due to an error occured", 'info');
                    //sessionStorage.clear();
                    //state.go("Login");
                    return;
                }
                else {

                    swal('info', res.Reason, 'info');
                    return;
                }
            });
        }

        function loadbranchdetails(bankname) {
            var input = {
                TYPE: 2,
                BankName: bankname,
                UserId: sessionStorage.getItem("user"),
                SacId: sessionStorage.getItem("secccode"),
                DesignId: sessionStorage.getItem("desinagtion"),
                TranId: ""
            };

            PRRD_Services.POSTENCRYPTAPI("GetJobCardBankMaster", input, token, function (value) {
                var res = value.data;
                if (res.Status == "100") {
                    scope.bankbranchlist = res.BankDetails;
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

        function loadifsccodedetails(str) {
            if (!(scope.branchname) && str == "") {
                scope.branchname = "";
            }
            var input = {
                TYPE: 3,
                BankName: scope.bankname,
                BranchName: scope.branchname,
                UserId: sessionStorage.getItem("user"),
                SacId: sessionStorage.getItem("secccode"),
                DesignId: sessionStorage.getItem("desinagtion"),
                TranId: ""
            };

            PRRD_Services.POSTENCRYPTAPI("GetJobCardBankMaster", input, token, function (value) {
                var res = value.data;
                if (res.Status == "100") {
                    console.log(res.BankDetails);
                    scope.IfscCode = res.BankDetails[0]["IFSC_CODE"];
                    // scope.IfscCode = res.BankDetails.IFSC_CODE;
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

        function LoadBankNameandBranchdetails(str) {
            if (!(scope.branchname) && str == "") {
                scope.branchname = "";
            }
            var input = {
                TYPE: 4,
                IFSCCode: scope.IfscCode,
                UserId: sessionStorage.getItem("user"),
                SacId: sessionStorage.getItem("secccode"),
                DesignId: sessionStorage.getItem("desinagtion"),
                TranId: ""
            };

            PRRD_Services.POSTENCRYPTAPI("GetJobCardBankMaster", input, token, function (value) {
                var res = value.data;
                if (res.Status == "100") {
                    console.log(res.BankDetails);
                    scope.bankname = res.BankDetails["0"]["BANK_NAME"];
                    loadbranchdetails(res.BankDetails["0"]["BANK_NAME"]);
                    scope.branchname = res.BankDetails["0"]["BRANCH_NAME"];
                    // scope.IfscCode = res.BankDetails.IFSC_CODE;
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

        function SubmitJobcardDetails() {
            debugger;
            var arrJobcard = [];
            angular.forEach(scope.addmultiplejobcard, function (value) {
                arrJobcard.push(
                    {
                        'P_TYPE': 1,
                        'P_JC_ID': sessionStorage.getItem("TransID"),
                        'P_LGD_DISTRICT_CODE': PRRDdistcode,
                        'P_LGD_MANDAL_CODE': mcode,
                        'P_GP_ID': value.GP_CODE,
                        'P_GP_NAME': value.GP_NAME,
                        'P_HB_CODE': value.HB_CODE,
                        'P_HB_NAME': value.HB_CODE,
                        'P_REGD_DATE': scope.registrationdate,
                        'P_APPLICATION_NUMBER': value.APPLICATION_NUMBER,
                        'P_GP_SECRETARY_NAME': value.GP_SECRETARY_NAME,
                        'P_CASTE': value.CASTE,
                        'P_ADDRESS': value.ADDRESS,
                        'P_ADDRESS_TEL': value.ADDRESS_TEL,
                        'P_RATIONCARD_NO': value.RATIONCARD_NO,
                        'P_BPL_NO': value.BPL_NO,
                        'P_KHATA_NO': value.KHATA_NO,
                        'P_LAND_OWNER': value.LAND_OWNER,
                        'P_LAND_OWNER_ACRES': value.LAND_OWNER_ACRES,
                        'P_ASSIGNED_LAND_BENEFICIARY': value.ASSIGNED_LAND_BENEFICIARY,
                        'P_IAY_BENEFICIARY': value.IAY_BENEFICIARY,
                        'P_MEMBER_NAME': value.MEMBER_NAME,
                        'P_MEMBER_NAME_TEL': value.MEMBER_NAME_TEL,
                        'P_SUR_NAME': value.SUR_NAME,
                        'P_SUR_NAME_TEL': value.SUR_NAME_TEL,
                        'P_FAMILY_HEAD': value.FAMILY_HEAD,
                        'P_RELATION_HOF': value.RELATION_HOF,
                        'P_GENDER': value.GENDER,
                        'P_AGE': value.AGE,
                        'P_HIV': value.HIV,
                        'P_DISABLED': value.DISABLED,
                        'P_PENSION_NUMBER': value.PENSION_NUMBER, //Added two columns Pension number and SmartCard number
                        'P_SMARTCARD_NUMBER': "",
                        'P_SHG_MEMBER': value.SHG_MEMBER,
                        'P_SHG_ID': value.SHG_ID,
                        'P_SHG_NAME': value.SHG_NAME,
                        'P_PERMANENT_JOB_CARD': value.PERMANENT_JOB_CARD,
                        'P_MPHSS_ID': value.MPHSS_ID,
                        'P_PAYING_AGENCY_TYPE': value.PAYING_AGENCY_TYPE,
                        'P_PAYING_AGENCY_NAME': value.PAYING_AGENCY_NAME,
                        'P_BANK_ACC_NAME': value.BANK_ACC_NAME,
                        'P_BRANCH_NAME': value.BRANCH_NAME,
                        'P_IFSC_CODE': value.IFSC_CODE,
                        'P_BANK_ACC_NO': value.BANK_ACC_NO,
                        'P_VOTER_ID': value.VOTER_ID,
                        'P_UID_NO': value.UID_NO,
                        'P_MOBILE_NO': value.MOBILE_NO,
                        'P_USER_ID': userid,
                        'P_SACHIVALAYAM_ID': sachivalaymid,
                        'P_UID_PATH': value.P_UID_PATH,
                        'P_PASSBOOK_PATH': value.P_PASSBOOK_PATH,
                        'P_F1_PATH': value.P_F1_PATH
                    }
                );
            });
            // var arrJobcardjson = JSON.stringify(arrJobcard, (k, v) => v === undefined ? null : v);
            PRRD_Services.DemoAPI("CreateJobCard", arrJobcard, function (value) {
                debugger;
                var res = value.data;
                if (res.Status == "100") {
                    swal('info', res.Reason, 'info');

                   // window.location.href = "../#!/MainDashboard";
                    window.open('', '_self').close();
                }
                else if (res.Status == "102") {
                    swal('info', res.Reason, 'info');

                    return;
                }
                else {

                    swal('info', res.Reason, 'info');
                    return;
                }
            });
            //  state.go("ui.Jobcard");
        }

        function LoadHabitation(panchcode) {
            var input = {
                FTYPE: 2, DISTCODE: distcode, MandalCode: mcode, GPCODE: panchcode,
                UserId: sessionStorage.getItem("user"),
                SacId: sessionStorage.getItem("secccode"),
                DesignId: sessionStorage.getItem("desinagtion"),
                TranId: ""
            };

            PRRD_Services.POSTENCRYPTAPI("GetHabitationCode", input, token, function (value) {
                var res = value.data;
                if (res.Status == "100") {
                    scope.habitationlist = res.Details;
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

        function loadJobCardDetails() {
            sessionStorage.setItem("index", 0);
            var input = {
                P_TYPE: 6,
                P_JC_ID: JC_ID,
                UserId: sessionStorage.getItem("user"),
                SacId: sessionStorage.getItem("secccode"),
                DesignId: sessionStorage.getItem("desinagtion"),
                TranId: ""
            };
            PRRD_Services.POSTENCRYPTAPI("EachJobCardData", input, token, function (value) {
                debugger;
                var res = value.data;
                if (res.Status == "100") {
                    scope.addmultiplejobcard = res.DataList;
                    loadbranchdetails(res.DataList["0"]["PAYING_AGENCY_NAME"]);
                    LoadHabitation(res.DataList["0"]["GP_CODE"])
                    scope.applicationnumber = res.DataList["0"]["APPLICATION_NUMBER"];
                    scope.panchayatsecretary = res.DataList["0"]["GP_SECRETARY_NAME"];
                    scope.habitation = res.DataList["0"]["HB_NAME"];
                    scope.panchayatcode = res.DataList["0"]["GP_CODE"];
                    // HouseHold details
                    scope.ngcast = res.DataList["0"]["CASTE"];
                    scope.EnglishAddress = res.DataList["0"]["ADDRESS"];
                    scope.EnglishTypingAddress = res.DataList["0"]["ADDRESS"];
                    scope.Teluguddress = res.DataList["0"]["ADDRESS_TEL"];
                    scope.RationcardNumber = res.DataList["0"]["RATIONCARD_NO"];
                    scope.BplNo = res.DataList["0"]["BPL_NO"];

                    scope.Acres = res.DataList["0"]["LAND_OWNER_ACRES"];
                    if (res.DataList["0"]["LAND_OWNER_ACRES"] == "") {
                        scope.chkLandowner = "no";
                        scope.Acresdisabled = true;
                    }
                    else {
                        scope.chkLandowner = "yes";
                        scope.Acresdisabled = false;
                    }
                    scope.KathaNo = res.DataList["0"]["KHATA_NO"];

                    //Member Details
                    scope.Name = res.DataList["0"]["MEMBER_NAME"];
                    scope.EnglishInputName = res.DataList["0"]["MEMBER_NAME"];
                    scope.TranslateTeluguName = res.DataList["0"]["MEMBER_NAME_TEL"];
                    scope.SurName = res.DataList["0"]["SUR_NAME"];
                    scope.TeluguSurName = res.DataList["0"]["SUR_NAME"];
                    scope.TranslateTeluguNameSurname = res.DataList["0"]["SUR_NAME_TEL"];
                    scope.Gender = res.DataList["0"]["GENDER"];
                    scope.age = res.DataList["0"]["AGE"];
                    scope.MemberofSHG = res.DataList["0"]["SHG_MEMBER"].toLowerCase();
                    scope.SHGName = res.DataList["0"]["SHG_NAME"];
                    scope.SHGID = res.DataList["0"]["SHG_ID"];
                    if (res.DataList["0"]["SHG_NAME"] == "") {
                        //scope.radioSHG = "no";
                        scope.SHGDisabled = false;
                    }
                    else {
                        //scope.radioSHG = "yes";
                        scope.SHGDisabled = true;
                    }
                    scope.MPHSSID = res.DataList["0"]["MPHSS_ID"];
                    scope.TypeofPayingAgency = res.DataList["0"]["PAYING_AGENCY_TYPE"];
                    scope.bankname = res.DataList["0"]["PAYING_AGENCY_NAME"];
                    scope.branchname = res.DataList["0"]["BRANCH_NAME"];
                    scope.IfscCode = res.DataList["0"]["IFSC_CODE"];
                    scope.NameasperBankPassbook = res.DataList["0"]["BANK_ACC_NAME"];
                    scope.BankAccountNumber = res.DataList["0"]["BANK_ACC_NO"];
                    scope.VoterID = res.DataList["0"]["VOTER_ID"];
                    scope.uid = res.DataList["0"]["UID_NO"];
                    scope.MobileNo = res.DataList["0"]["MOBILE_NO"];
                    if (res.DataList["0"]["MOBILE_NO"] == "") {
                        scope.chkMobileNo = "no";
                        scope.Mobiledisabled = true;
                    }
                    else {
                        scope.chkMobileNo = "yes";
                        scope.Mobiledisabled = false;
                    }
                    scope.uidpath = res.DataList["0"]["UID_PATH"];
                    scope.passbookpath = res.DataList["0"]["PASSBOOK_PATH"];
                    scope.f1path = res.DataList["0"]["F1_PATH"];
                    var uidNo = res.DataList["0"]["UID_NO"];
                    var UidNoOne = uidNo.slice(0, 4);
                    var UidNoTwo = uidNo.slice(4, 8);
                    var UidNoThree = uidNo.slice(8, 12);
                    scope.uid1 = UidNoOne.toString();
                    scope.uid2 = UidNoTwo.toString();
                    scope.uid3 = UidNoThree.toString();

                    var utype = sessionStorage.getItem("user").split('-')[1].toString();
                    if (utype != 'DA') {
                        scope.Acresdisabled = true;
                        scope.Mobiledisabled = true;
                        scope.UIddisabled = true;
                        if (scope.passbookpath == null || scope.passbookpath == undefined) {
                            //scope.passbook="No File uploded.";
                            scope.IsDownload = false;
                            scope.IsFile = true;
                        }
                        else {
                            scope.IsDownload = true;
                            scope.IsFile = false;
                        }

                        if (scope.f1path == null || scope.f1path == undefined) {
                            //scope.passbook="No File uploded.";
                            scope.IsDownload = false;
                            scope.IsFile = true;
                        }
                        else {
                            scope.IsDownload = true;
                            scope.IsFile = false;
                        }
                        if (scope.uidpath == null || scope.uidpath == undefined) {
                            //scope.passbook="No File uploded.";
                            scope.IsDownload = false;
                            scope.IsFile = true;
                        }
                        else {
                            scope.IsDownload = true;
                            scope.IsFile = false;
                        }
                    }
                    else {

                        if (type == "Edit") {
                            scope.UIddisabled = true;
                            if (scope.passbookpath == null || scope.passbookpath == undefined) {
                                //scope.passbook="No File uploded.";
                                scope.IsDownload = false;
                                scope.IsFile = true;
                                scope.IsUpload = true;
                            }
                            else {
                                scope.IsDownload = true;
                                scope.IsFile = false;
                                scope.IsUpload = true;
                            }
                            if (scope.f1path == null || scope.f1path == undefined) {
                                //scope.passbook="No File uploded.";
                                scope.IsDownload = false;
                                scope.IsFile = true;
                                scope.IsUpload = true;
                            }
                            else {
                                scope.IsDownload = true;
                                scope.IsFile = false;
                                scope.IsUpload = true;
                            }
                            if (scope.uidpath == null || scope.uidpath == undefined) {
                                //scope.passbook="No File uploded.";
                                scope.IsDownload = false;
                                scope.IsFile = true;
                                scope.IsUpload = true;
                            }
                            else {
                                scope.IsDownload = true;
                                scope.IsFile = false;
                                scope.IsUpload = true;
                            }
                        }
                    }
                    scope.chkAssignedLands = res.DataList["0"]["ASSIGNED_LAND_BENEFICIARY"].toLowerCase();
                    scope.chkIAYBeneficiary = res.DataList["0"]["IAY_BENEFICIARY"].toLowerCase();
                    scope.Disabled = res.DataList["0"]["DISABLED"].toLowerCase();
                    if (res.DataList["0"]["DISABLED"].toLowerCase() == "yes") {
                        scope.PensionNumber = res.DataList["0"]["PENSION_NUMBER"];
                        scope.Pensiondisabled = false;
                    }
                    else {
                        scope.Pensiondisabled = true;
                    }
                    scope.HeadofFamily = res.DataList["0"]["FAMILY_HEAD"].toLowerCase();
                    if (res.DataList["0"]["FAMILY_HEAD"].toLowerCase() == "yes") {
                        scope.relationwithfamilyheadlist = headrelation;
                    }
                    else {
                        scope.relationwithfamilyheadlist = listofrelations;
                    }
                    scope.RelationwithHeadofFamily = res.DataList["0"]["RELATION_HOF"];
                    scope.HIVAffected = res.DataList["0"]["HIV"].toLowerCase();
                    if (res.DataList["0"]["PERMANENT_JOB_CARD"] != null) {
                        scope.PermanentlyJobcardid = res.DataList["0"]["PERMANENT_JOB_CARD"].toLowerCase();
                    }
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

        // FINALLY APPROVE THE DATA.
        scope.sendAPI = function () {
            sendjobcardAPI();
        };

        function sendjobcardAPI() {
            debugger;
            var APIJobcard = [];
            angular.forEach(scope.addmultiplejobcard, function (value) {
                var SHG_MEMBER = value.SHG_MEMBER == "no" ? "N" : "Y";
                var DISABLED = value.DISABLED == "no" ? "N" : "Y";
                var HIV = value.HIV == "no" ? "N" : "Y";
                var ASSIGNED_LAND_BENEFICIARY = value.ASSIGNED_LAND_BENEFICIARY == "no" ? "N" : "Y";
                var IAY_BENEFICIARY = value.IAY_BENEFICIARY == "no" ? "N" : "Y";
                var FAMILY_HEAD = value.FAMILY_HEAD == "no" ? "N" : "Y";
                var MOBILE_NO_Req = value.MOBILE_NO == "" ? "N" : "Y";
                var GENDER = value.GENDER == "Male" ? "M" : "F";
                APIJobcard.push(
                    {
                        'DistrictId': PRRDdistcode,
                        'MandalId': PRRDmcode,
                        'PanchayatId': value.GP_CODE,
                        'HabCode': value.HB_CODE,
                        'PCCInfoCode': "",
                        'Category': "New Jobcard for Family",
                        'Applnnumber': value.APPLICATION_NUMBER,
                        'RegDate': filter('date')(scope.registrationdate, 'yyyy-MM-dd'),
                        'Cast': value.CASTE,
                        'Address': scope.EnglishAddress,
                        'AddressEngToTelugu': value.ADDRESS_TEL,
                        'RationCard': value.RATIONCARD_NO,
                        'LandOwner': value.LAND_OWNER,
                        'LandOwned': value.LAND_OWNER_ACRES,
                        'BplNum': value.BPL_NO,
                        'KathaNum': value.KHATA_NO,
                        'AssignedLandReforms': ASSIGNED_LAND_BENEFICIARY,
                        'IayBenf': IAY_BENEFICIARY,
                        'Name': value.MEMBER_NAME,
                        'Surname': value.SUR_NAME,
                        'Headoffamily': FAMILY_HEAD,
                        'Relationwithhead': value.RELATION_HOF,
                        'Gender': GENDER,
                        'HIVeffected': HIV,
                        'Age': value.AGE,
                        'MemberofSHG': SHG_MEMBER,
                        'SHG_Name': value.SHG_NAME,
                        'SHG_ID': value.SHG_ID,
                        'MPHSS_ID': value.MPHSS_ID,
                        'TypeOfPayingAgency': value.PAYING_AGENCY_TYPE,
                        'PayingAgencyName': value.PAYING_AGENCY_NAME,
                        'BranchName': value.BRANCH_NAME,
                        'IFSCcode': value.IFSC_CODE,
                        'NameAsPerBank': value.BANK_ACC_NAME,
                        'BankAccountNumber': value.BANK_ACC_NO,
                        'VoterId': value.VOTER_ID,
                        'UID': value.UID_NO,
                        'Mobilenum': value.MOBILE_NO,
                        'Postalaccnum': "",
                        'Solid': "",
                        'Disabled': DISABLED,
                        'BankName': value.PAYING_AGENCY_NAME,
                        'BankPassbookName': value.BANK_ACC_NAME,
                        'SmartCardNo': "",
                        'PensionNumber': value.P_PENSION_NUMBER,
                        'PMJobCardId': "",
                        'IsBenfcielingLand': ASSIGNED_LAND_BENEFICIARY,
                        'CategoryCode': "01",
                        'MobNoRequired': MOBILE_NO_Req,
                        'InterfaceUniqueId': value.JC_ID,
                        'Transactionid': "",
                        'NameEngToTelugu': value.MEMBER_NAME_TEL,
                        'SurnameEngToTelugu': value.SUR_NAME_TEL
                    }
                );
            });

            var input = {
                deptId: "1234",
                deptName: "Rural Development",
                serviceName: "SaveNewJobCardDetails",
                serviceType: "REST",
                method: "POST",
                simulatorFlag: "false",
                application: "GWS",
                username: sessionStorage.getItem("user"),
                userid: sessionStorage.getItem("user"),
                schemeId: "New JobCard",
                data: APIJobcard
            };
            PRRD_Services.DemoAPI("SendJobcardAPI", input, function (value) {
                debugger;
                var res = value.data;
                console.log(res);
                if (res.Status != "") {
                    swal('info', res.REASON, 'info');
                }
                else if (res.Status == "428") {
                    swal('info', res.REASON, 'info');
                    return;
                }
                else {

                    swal('info', res.REASON, 'info');
                    return;
                }
            });
        }

        scope.clickedview = function (index) {
            debugger;
            //sessionStorage.setItem("index", index);
            
            //house hold details 
            scope.ngcast = scope.addmultiplejobcard[index]["CASTE"];
            scope.EnglishAddress = scope.addmultiplejobcard[index]["ADDRESS"];
            scope.EnglishTypingAddress = scope.addmultiplejobcard[index]["ADDRESS"];
            scope.Teluguddress = scope.addmultiplejobcard[index]["ADDRESS_TEL"];
            scope.RationcardNumber = scope.addmultiplejobcard[index]["RATIONCARD_NO"];
            scope.BplNo = scope.addmultiplejobcard[index]["BPL_NO"];

            scope.Acres = scope.addmultiplejobcard[index]["LAND_OWNER_ACRES"];
            if (scope.addmultiplejobcard[index]["LAND_OWNER_ACRES"] == "") {
                scope.chkLandowner = "no";
                scope.Acresdisabled = true;
                scope.KathaNo = true;
            }
            else {
                scope.chkLandowner = "yes";
                scope.Acresdisabled = true;
                scope.KathaNo = true;
            }
            scope.KathaNo = scope.addmultiplejobcard[index]["KHATA_NO"];

            scope.chkAssignedLands = scope.addmultiplejobcard[index]["ASSIGNED_LAND_BENEFICIARY"].toLowerCase(); 
           // if (scope.addmultiplejobcard[index]["IAY_BENEFICIARY"].toLowerCase()!="")
            scope.chkIAYBeneficiary = scope.addmultiplejobcard[index]["IAY_BENEFICIARY"].toLowerCase();
            //member details
            scope.Name = scope.addmultiplejobcard[index]["MEMBER_NAME"];
            scope.SurName = scope.addmultiplejobcard[index]["SUR_NAME"];
            scope.TranslateTeluguName = scope.addmultiplejobcard[index]["MEMBER_NAME_TEL"];
            scope.TranslateTeluguNameSurname = scope.addmultiplejobcard[index]["SUR_NAME_TEL"]; 

            scope.EnglishTypingAddress = scope.addmultiplejobcard[index]["ADDRESS"];
            scope.TeluguSurName = scope.addmultiplejobcard[index]["SUR_NAME"];
            scope.EnglishInputName = scope.addmultiplejobcard[index]["MEMBER_NAME"];

            scope.Gender = scope.addmultiplejobcard[index]["GENDER"];
            scope.age = scope.addmultiplejobcard[index]["AGE"];
            scope.SHGName = scope.addmultiplejobcard[index]["SHG_NAME"];
            scope.SHGID = scope.addmultiplejobcard[index]["SHG_ID"];
            scope.MPHSSID = scope.addmultiplejobcard[index]["MPHSS_ID"];
            scope.TypeofPayingAgency = scope.addmultiplejobcard[index]["PAYING_AGENCY_TYPE"];
            scope.IfscCode = scope.addmultiplejobcard[index]["IFSC_CODE"];
            scope.NameasperBankPassbook = scope.addmultiplejobcard[index]["BANK_ACC_NAME"];
            scope.BankAccountNumber = scope.addmultiplejobcard[index]["BANK_ACC_NO"];
            scope.VoterID = scope.addmultiplejobcard[index]["VOTER_ID"];
            scope.MobileNo = scope.addmultiplejobcard[index]["MOBILE_NO"];
            scope.uidpath = scope.addmultiplejobcard[index]["UID_PATH"];
            scope.passbookpath = scope.addmultiplejobcard[index]["PASSBOOK_PATH"];
            scope.f1path = scope.addmultiplejobcard[index]["F1_PATH"];
            scope.UIdNo = scope.addmultiplejobcard[index]["UID_NO"];
            var uidNo = scope.addmultiplejobcard[index]["UID_NO"];
            var UidNoOne = uidNo.slice(0, 4);
            var UidNoTwo = uidNo.slice(4, 8);
            var UidNoThree = uidNo.slice(8, 12);
            scope.uid1 = UidNoOne.toString();
            scope.uid2 = UidNoTwo.toString();
            scope.uid3 = UidNoThree.toString();
            scope.bankname = scope.addmultiplejobcard[index]["PAYING_AGENCY_NAME"];
            scope.branchname = scope.addmultiplejobcard[index]["BRANCH_NAME"];
            scope.HeadofFamily = scope.addmultiplejobcard[index]["FAMILY_HEAD"].toLowerCase();
            scope.RelationwithHeadofFamily = scope.addmultiplejobcard[index]["RELATION_HOF"];
            scope.HIVAffected = scope.addmultiplejobcard[index]["HIV"].toLowerCase();
            scope.Disabled = scope.addmultiplejobcard[index]["DISABLED"].toLowerCase();
            scope.PensionNumber = scope.addmultiplejobcard[index]["PENSION_NUMBER"]; //Added Pension number
            scope.PermanentlyJobcardid = scope.addmultiplejobcard[index]["PERMANENT_JOB_CARD"].toLowerCase();
            scope.MemberofSHG = scope.addmultiplejobcard[index]["SHG_MEMBER"].toLowerCase();
            scope.UIddisabled = true; 
        }

        //Jobcard file upload.
        scope.uploadfile = function (type, categoryid, category, imgname) {
            var file = type.attachment;
            var fileexten = file.type;
            var FileSize = file.size;
            if (!scope.uid1 || !scope.uid2 || !scope.uid3) {
                swal('info', 'Please Enter UID', 'error');
                return;
            }

            if (FileSize > 1000000) {
                swal('info', 'File size exceeded 1 MB', 'error');
                scope.preloader = false;
                angular.element("input[type='file']").val('');
                return;
            }
            if (fileexten.split("/")[1] == "jpg" || fileexten.split("/")[1] == "JPG" || fileexten.split("/")[1] == "JPEG" || fileexten.split("/")[1] == "jpeg") {
            }
            else {
                scope.preloader = false;
                swal("info", "Only JPG Image Accepted", "info");
                return;
            }
            scope.preloader = true;
            var prop = { AadharCardNumber: imgname, Attachment: type.attachment, CertifcateID: categoryid, CertificateCategory: category };
            entityService.saveTutorial(prop, "/Home/JobCardPassbook")
                .then(function (data) {
                    scope.preloader = false;
                    scope.imagedata = data.data;
                    if (scope.imagedata.match("Failure")) {
                        swal('info', scope.imagedata, 'error');
                        scope.preloader = false;
                        angular.element("input[type='file']").val('');
                        return;
                    }
                    if (category == "uid") {
                        scope.uidpath = data.data;
                        swal('info', "file Uploaded Successfully", 'success');
                        angular.element("input[type='file']").val('');
                    }
                    else if (category == "passbook") {
                        scope.passbookpath = data.data;
                        swal('info', "file Uploaded Successfully", 'success');
                        angular.element("input[type='file']").val('');
                    }
                    else if (category == "f1") {
                        scope.f1path = data.data;
                        swal('info', "file Uploaded Successfully", 'success');
                        angular.element("input[type='file']").val('');
                    }
                    console.log(data);
                });
        }

        //Jobcard Rejection.
        scope.RejectJobcard = function () {
            JobcardRejection();
        };
        function JobcardRejection() {
            var input = {
                P_TYPE: 11,
                P_STATUS: 2,
                P_JC_ID: JC_ID,
                P_USER_ID: sessionStorage.getItem("user"),
                UserId: sessionStorage.getItem("user"),
                SacId: sessionStorage.getItem("secccode"),
                DesignId: sessionStorage.getItem("desinagtion"),
                TranId: ""
            };
            PRRD_Services.POSTENCRYPTAPI("RejectJobcard", input, token, function (value) {
                var res = value.data;
                if (res.Status == "100") {
                    swal('info', "Jobcard rejected successfully..!", 'info');
                }
                else if (res.Status == "101") {
                    swal('info', res.Reason, 'info');
                    return;
                }
                else {

                    swal('info', res.Reason, 'info');
                    return;
                }
            });
            state.go("ui.Jobcard");
        }

        //jobcard edit
        scope.clickedEdit = function (index) {
            sessionStorage.setItem("index", index);
            //house hold details 
            scope.ngcast = scope.addmultiplejobcard[index]["CASTE"];
            scope.EnglishAddress = scope.addmultiplejobcard[index]["ADDRESS"];
            scope.EnglishTypingAddress = scope.addmultiplejobcard[index]["ADDRESS"];
            scope.Teluguddress = scope.addmultiplejobcard[index]["ADDRESS_TEL"];
            scope.RationcardNumber = scope.addmultiplejobcard[index]["RATIONCARD_NO"];
            scope.BplNo = scope.addmultiplejobcard[index]["BPL_NO"];

            scope.Acres = scope.addmultiplejobcard[index]["LAND_OWNER_ACRES"];
            if (scope.addmultiplejobcard[index]["LAND_OWNER_ACRES"] == "") {
                scope.chkLandowner = "no";
                scope.Acresdisabled = true;
            }
            else {
                scope.chkLandowner = "yes";
                scope.Acresdisabled = false;
            }
            scope.KathaNo = scope.addmultiplejobcard[index]["KHATA_NO"];

            scope.chkAssignedLands = scope.addmultiplejobcard[index]["ASSIGNED_LAND_BENEFICIARY"].toLowerCase();
            scope.chkIAYBeneficiary = scope.addmultiplejobcard[index]["IAY_BENEFICIARY"].toLowerCase();
            //member details
            scope.Name = scope.addmultiplejobcard[index]["MEMBER_NAME"];

            scope.EnglishTypingAddress = scope.addmultiplejobcard[index]["ADDRESS"];
            scope.TeluguSurName = scope.addmultiplejobcard[index]["SUR_NAME"];
            scope.EnglishInputName = scope.addmultiplejobcard[index]["MEMBER_NAME"];

            scope.SurName = scope.addmultiplejobcard[index]["SUR_NAME"];
            scope.TranslateTeluguName = scope.addmultiplejobcard[index]["MEMBER_NAME_TEL"];
            scope.TranslateTeluguNameSurname = scope.addmultiplejobcard[index]["SUR_NAME_TEL"];
            scope.Teluguddress = scope.addmultiplejobcard[index]["ADDRESS_TEL"];
            scope.Gender = scope.addmultiplejobcard[index]["GENDER"];
            scope.age = scope.addmultiplejobcard[index]["AGE"];
            scope.SHGName = scope.addmultiplejobcard[index]["SHG_NAME"];
            if (scope.SHGName == "") {
                scope.SHGDisabled = true;
                scope.radioSHG = "no";
            }
            else {
                scope.SHGDisabled = false;
                scope.radioSHG = "yes";
            }
            scope.SHGID = scope.addmultiplejobcard[index]["SHG_ID"];
            scope.MPHSSID = scope.addmultiplejobcard[index]["MPHSS_ID"];
            scope.TypeofPayingAgency = scope.addmultiplejobcard[index]["PAYING_AGENCY_TYPE"];
            scope.IfscCode = scope.addmultiplejobcard[index]["IFSC_CODE"];
            scope.NameasperBankPassbook = scope.addmultiplejobcard[index]["BANK_ACC_NAME"];
            scope.BankAccountNumber = scope.addmultiplejobcard[index]["BANK_ACC_NO"];
            scope.PensionNumber = scope.addmultiplejobcard[index]["PENSION_NUMBER"];
            scope.VoterID = scope.addmultiplejobcard[index]["VOTER_ID"];
            scope.MobileNo = scope.addmultiplejobcard[index]["MOBILE_NO"];
            if (scope.addmultiplejobcard[index]["MOBILE_NO"] == "") {
                scope.chkMobileNo = "no";
                scope.Mobiledisabled = true;
            }
            else {
                scope.chkMobileNo = "yes";
                scope.Mobiledisabled = false;
            }
            scope.uidpath = scope.addmultiplejobcard[index]["UID_PATH"];
            scope.passbookpath = scope.addmultiplejobcard[index]["PASSBOOK_PATH"];
            scope.f1path = scope.addmultiplejobcard[index]["F1_PATH"];
            scope.UIdNo = scope.addmultiplejobcard[index]["UID_NO"];
            var uidNo = scope.addmultiplejobcard[index]["UID_NO"];
            var UidNoOne = uidNo.slice(0, 4);
            var UidNoTwo = uidNo.slice(4, 8);
            var UidNoThree = uidNo.slice(8, 12);
            scope.uid1 = UidNoOne.toString();
            scope.uid2 = UidNoTwo.toString();
            scope.uid3 = UidNoThree.toString();
            scope.bankname = scope.addmultiplejobcard[index]["PAYING_AGENCY_NAME"];
            scope.branchname = scope.addmultiplejobcard[index]["BRANCH_NAME"];
            scope.HeadofFamily = scope.addmultiplejobcard[index]["FAMILY_HEAD"].toLowerCase();
            scope.RelationwithHeadofFamily = scope.addmultiplejobcard[index]["RELATION_HOF"];
            scope.HIVAffected = scope.addmultiplejobcard[index]["HIV"].toLowerCase();
            scope.Disabled = scope.addmultiplejobcard[index]["DISABLED"].toLowerCase();
            if (scope.addmultiplejobcard[index]["DISABLED"].toLowerCase() == "no") {
                scope.Pensiondisabled = true;
            }
            else {
                scope.Pensiondisabled = false;
            }
            scope.PermanentlyJobcardid = scope.addmultiplejobcard[index]["PERMANENT_JOB_CARD"].toLowerCase();
            scope.MemberofSHG = scope.addmultiplejobcard[index]["SHG_MEMBER"].toLowerCase();
            scope.UIddisabled = true;
        }
        scope.update = function () {
            debugger;
            if (requiredFieldValidators()) {
                var i = sessionStorage.getItem("index");
                var output;
                angular.forEach(scope.addmultiplejobcard, function (item) {
                    if (scope.RelationwithHeadofFamily == item.RelationwithHeadofFamily) {
                        output = "familyHead";
                        return false;
                    }
                    //if (scope.BankAccountNumber == item.BANK_ACC_NO) {
                    //    output = "bank";
                    //    return false;
                    //}
                    //if (scope.uid1 + scope.uid2 + scope.uid3 == item.UID_NO) {
                    //    output = "uid";
                    //    return false;
                    //}
                });
                if (output == "familyHead") {
                    swal("Info", "Head of the family member already exists in below table", "info");
                    output = "";
                    return false;
                }
                //else if (output == "bank") {
                //    swal("Info", "Bankaccountnumber already exists in below table", "info");
                //    output = "";
                //    return false;
                //}
                //else if (output == "uid") {
                //    swal("Info", "Aadhar already exists in below table", "info");
                //    output = "";
                //    return false;
                //}
                scope.addmultiplejobcard[i]["MEMBER_NAME"] = scope.Name;
                scope.addmultiplejobcard[i]["SUR_NAME"] = scope.SurName;
                scope.addmultiplejobcard[i]["MEMBER_NAME_TEL"] = scope.TranslateTeluguName;
                scope.addmultiplejobcard[i]["SUR_NAME_TEL"] = scope.TranslateTeluguNameSurname;
                scope.addmultiplejobcard[i]["ADDRESS_TEL"] = scope.Teluguddress;
                scope.addmultiplejobcard[i]["ADDRESS"] = scope.EnglishAddress;
                scope.addmultiplejobcard[i]["FAMILY_HEAD"] = scope.HeadofFamily;
                scope.addmultiplejobcard[i]["RELATION_HOF"] = scope.RelationwithHeadofFamily;
                scope.addmultiplejobcard[i]["GENDER"] = scope.Gender;
                scope.addmultiplejobcard[i]["AGE"] = scope.age;
                scope.addmultiplejobcard[i]["APPLICATION_NUMBER"] = scope.applicationnumber;
                scope.addmultiplejobcard[i]["CASTE"] = scope.ngcast;
                scope.addmultiplejobcard[i]["RATIONCARD_NO"] = scope.RationcardNumber;
                scope.addmultiplejobcard[i]["BPL_NO"] = scope.BplNo;
                scope.addmultiplejobcard[i]["KHATA_NO"] = scope.KathaNo;
                scope.addmultiplejobcard[i]["LAND_OWNER"] = scope.chkLandowner;
                scope.addmultiplejobcard[i]["LAND_OWNER_ACRES"] = scope.Acres;
                scope.addmultiplejobcard[i]["ASSIGNED_LAND_BENEFICIARY"] = scope.chkAssignedLands;
                scope.addmultiplejobcard[i]["IAY_BENEFICIARY"] = scope.chkIAYBeneficiary;
                scope.addmultiplejobcard[i]["SHG_NAME"] = scope.SHGName;
                scope.addmultiplejobcard[i]["SHG_ID"] = scope.SHGID;
                scope.addmultiplejobcard[i]["SHG_MEMBER"] = scope.MemberofSHG;
                scope.addmultiplejobcard[i]["MPHSS_ID"] = scope.MPHSSID;
                scope.addmultiplejobcard[i]["VOTER_ID"] = scope.VoterID;
                scope.addmultiplejobcard[i]["DISABLED"] = scope.Disabled;
                scope.addmultiplejobcard[i]["PENSION_NUMBER"] = scope.PensionNumber; //Pension number
                scope.addmultiplejobcard[i]["PAYING_AGENCY_TYPE"] = scope.TypeofPayingAgency;
                scope.addmultiplejobcard[i]["PAYING_AGENCY_NAME"] = scope.bankname; //bank name
                scope.addmultiplejobcard[i]["BANK_ACC_NO"] = scope.BankAccountNumber;
                scope.addmultiplejobcard[i]["BRANCH_NAME"] = scope.branchname;
                scope.addmultiplejobcard[i]["IFSC_CODE"] = scope.IfscCode;
                scope.addmultiplejobcard[i]["PERMANENT_JOB_CARD"] = scope.PermanentlyJobcardid;
                scope.addmultiplejobcard[i]["HIV"] = scope.HIVAffected;
                scope.addmultiplejobcard[i]["MOBILE_NO"] = scope.MobileNo;
                scope.addmultiplejobcard[i]["MOBILE_NO_Req"] = scope.chkMobileNo;
                scope.addmultiplejobcard[i]["BANK_ACC_NAME"] = scope.NameasperBankPassbook;
                scope.addmultiplejobcard[i]["GP_CODE"] = scope.panchayatcode;
                scope.addmultiplejobcard[i]["GP_NAME"] = scope.panchayatcode;
                scope.addmultiplejobcard[i]["HB_CODE"] = scope.habitation;
                scope.addmultiplejobcard[i]["GP_SECRETARY_NAME"] = scope.panchayatsecretary;
                scope.addmultiplejobcard[i]["P_UID_PATH"] = scope.uidpath;
                scope.addmultiplejobcard[i]["P_PASSBOOK_PATH"] = scope.passbookpath;
                scope.addmultiplejobcard[i]["P_F1_PATH"] = scope.f1path;
                scope.addmultiplejobcard[i]["P_PENSION_NUMBER"] = scope.PensionNumber;
                console.log(scope.addmultiplejobcard[i]);
                clearMemberDetails();
            }
           
        };

        //jobcard update in db
        scope.updateJobcard = function () {
            UpdateJobcardDetails();
        };

        function UpdateJobcardDetails() {
            var arrJobcard = [];
            angular.forEach(scope.addmultiplejobcard, function (value) {
                arrJobcard.push(
                    {
                        'P_TYPE': 2,
                        'P_JC_ID': JC_ID,
                        'P_LGD_DISTRICT_CODE': PRRDdistcode,
                        'P_LGD_MANDAL_CODE': mcode,
                        'P_GP_ID': value.GP_CODE,
                        'P_GP_NAME': value.GP_NAME,
                        'P_HB_CODE': value.HB_CODE,
                        'P_HB_NAME': value.HB_CODE,
                        'P_REGD_DATE': scope.registrationdate,
                        'P_APPLICATION_NUMBER': value.APPLICATION_NUMBER,
                        'P_GP_SECRETARY_NAME': value.GP_SECRETARY_NAME,
                        'P_CASTE': value.CASTE,
                        'P_ADDRESS': value.ADDRESS,
                        'P_ADDRESS_TEL': value.ADDRESS_TEL,
                        'P_RATIONCARD_NO': value.RATIONCARD_NO,
                        'P_BPL_NO': value.BPL_NO,
                        'P_KHATA_NO': value.KHATA_NO,
                        'P_LAND_OWNER': value.LAND_OWNER,
                        'P_LAND_OWNER_ACRES': value.LAND_OWNER_ACRES,
                        'P_ASSIGNED_LAND_BENEFICIARY': value.ASSIGNED_LAND_BENEFICIARY,
                        'P_IAY_BENEFICIARY': value.IAY_BENEFICIARY,
                        'P_MEMBER_NAME': value.MEMBER_NAME,
                        'P_MEMBER_NAME_TEL': value.MEMBER_NAME_TEL,
                        'P_SUR_NAME': value.SUR_NAME,
                        'P_SUR_NAME_TEL': value.SUR_NAME_TEL,
                        'P_FAMILY_HEAD': value.FAMILY_HEAD,
                        'P_RELATION_HOF': value.RELATION_HOF,
                        'P_GENDER': value.GENDER,
                        'P_AGE': value.AGE,
                        'P_HIV': value.HIV,
                        'P_DISABLED': value.DISABLED,
                        'P_PENSION_NUMBER': value.PENSION_NUMBER, //Added two columns Pension number and SmartCard number
                        'P_SMARTCARD_NUMBER': "",
                        'P_SHG_MEMBER': value.SHG_MEMBER,
                        'P_SHG_ID': value.SHG_ID,
                        'P_SHG_NAME': value.SHG_NAME,
                        'P_PERMANENT_JOB_CARD': value.PERMANENT_JOB_CARD,
                        'P_MPHSS_ID': value.MPHSS_ID,
                        'P_PAYING_AGENCY_TYPE': value.PAYING_AGENCY_TYPE,
                        'P_PAYING_AGENCY_NAME': value.PAYING_AGENCY_NAME,
                        'P_BANK_ACC_NAME': value.BANK_ACC_NAME,
                        'P_BRANCH_NAME': value.BRANCH_NAME,
                        'P_IFSC_CODE': value.IFSC_CODE,
                        'P_BANK_ACC_NO': value.BANK_ACC_NO,
                        'P_VOTER_ID': value.VOTER_ID,
                        'P_UID_NO': value.UID_NO,
                        'P_MOBILE_NO': value.MOBILE_NO,
                        'P_USER_ID': userid,
                        'P_SACHIVALAYAM_ID': sachivalaymid,
                        'P_UID_PATH': value.P_UID_PATH,
                        'P_PASSBOOK_PATH': value.P_PASSBOOK_PATH,
                        'P_F1_PATH': value.P_F1_PATH
                    }
                );
            });
            PRRD_Services.DemoAPI("CreateJobCard", arrJobcard, function (value) {
                var res = value.data;
                if (res.Status == "100") {
                    swal('info', "Jobcard updated successfully.....!", 'info');
                    // window.location.href = "../#!/MainDashboard";
                   // window.open('', '_self').close();
                }
                else if (res.Status == "102") {
                    swal('info', res.Reason, 'info');

                    return;
                }
                else {

                    swal('info', res.Reason, 'info');
                    return;
                }
                
            });

        }
        scope.clicked = function () {
            state.go("ui.Jobcard")
        }


        //Translate english to telugu 
        scope.TranslateTelugu = function (name) {
            if (name == 'name')
                TranslateEnglishTelugu(scope.EnglishInputName, name);
            else if (name == 'address')
                TranslateEnglishTelugu(scope.EnglishTypingAddress, name);
            else
                TranslateEnglishTelugu(scope.TeluguSurName, name);
        };
        function TranslateEnglishTelugu(txt, name) {
            var chkname = name;
            var input = {
                itext: txt,
                translitaration: "ADDRESS",
                locale: "tl_in",
                transRev: "undefined",
                UserId: sessionStorage.getItem("user"),
                SacId: sessionStorage.getItem("secccode"),
                DesignId: sessionStorage.getItem("desinagtion"),
                TranId: ""
            };
            PRRD_Services.POSTENCRYPTAPI("TranslateTextAPI", input, token, function (value) {
                var res = value.data;
                if (chkname == 'name')
                    scope.TranslateTeluguName = res;
                else if (name == 'address')
                    scope.Teluguddress = res;
                else
                    scope.TranslateTeluguNameSurname = res;
            });
        };

        //Aadhar Test

        scope.TestAadar = function () {


            if (scope.uid3.length == 4) {
                AadarValidate();
                ValidUIDfromDB();
            }
        };
        function AadarValidate() {

            var input = {
                deptId: "1234",
                deptName: "Rural Development",
                serviceName: "ValidateUID",
                serviceType: "REST",
                method: "GET",
                simulatorFlag: "false",
                application: "GWS",
                username: sessionStorage.getItem("user"),
                userid: sessionStorage.getItem("user"),
                schemeId: "New JobCard",
                strUID: scope.uid1 + scope.uid2 + scope.uid3,
                data: { "body": "dummy" }
            };

            PRRD_Services.POSTENCRYPTAPI("Aadharvalidate", input, token, function (value) {
                var res = value.data;

                if (res.Status == "100") {
                    if (res.REASON != "\"SUCCESS\"") {
                        swal('info', res.REASON, 'info');
                        scope.uid1 = "";
                        scope.uid2 = "";
                        scope.uid3 = "";
                    }
                }
                else if (res.Status == "428") {
                    swal('info', res.REASON, 'info');
                    return;
                }
                else {

                    swal('info', res.REASON, 'info');
                    return;
                }
            });
        }

        //radio button validations
        scope.EnableDisableLandAcres = function () {
            if (scope.chkLandowner.toLowerCase() == "yes") {
                scope.Acresdisabled = false; 
            }
            else {                       
                scope.Acres = "";
                scope.KathaNo = "";
                scope.Acresdisabled = true;       
            }

        };
        scope.EnableDisableSHGDetails = function () {
            if (scope.MemberofSHG.toLowerCase()  == "yes") {
                scope.SHGDisabled = false;
            }
            else {              
                scope.SHGID = "";
                scope.SHGName = "";
                scope.SHGDisabled = true;
            }

        };
        scope.EnableDisablePension = function () {
            if (scope.Disabled.toLowerCase()  == "yes") {
                scope.Pensiondisabled = false;
            }
            else {               
                scope.PensionNumber = ""; 
                scope.Pensiondisabled = true;
            }

        };
        scope.EnableDisableMobileNo = function () {
            if (scope.chkMobileNo.toLowerCase()  == "yes") {
                scope.Mobiledisabled = false;
            }
            else {               
                scope.MobileNo = "";
                scope.Mobiledisabled = true;
            }

        };
        // UID validation in Database
        function ValidUIDfromDB() {
            var input = {
                P_TYPE: 3,
                P_STATUS: 0,
                P_UID_NO: scope.uid1 + scope.uid2 + scope.uid3,
                P_JC_ID: "",
                P_USER_ID: sessionStorage.getItem("user"),
                UserId: sessionStorage.getItem("user"),
                SacId: sessionStorage.getItem("secccode"),
                DesignId: sessionStorage.getItem("desinagtion"),
                TranId: ""
            };
            PRRD_Services.POSTENCRYPTAPI("EachJobCardData", input, token, function (value) {
                var res = value.data;
                if (res.Status == "100") {
                    swal('info', "Aadhar already existed...Please enter another Aadhar number", 'info');
                    scope.uid1 = ""; scope.uid2 = ""; scope.uid3 = "";
                    return false;
                }
                else if (res.Status == "102") {
                    return true;
                }
            });
        }

        // Valid Rationcard Number
        scope.RationValidation = function () {
            debugger;
            var ration = angular.lowercase(scope.RationcardNumber.slice(0, 3));
            var rationnames = "wap,yap,pap,aap,rap,tap";
            if (!rationnames.includes(ration)) {

                swal('info', 'First three characters should be WAP / AAP / YAP / PAP / RAP / TAP', 'info');
                scope.RationcardNumber = "";
                return false;
            }
            var reg = new RegExp("^([A-Za-z]{3,3})([0-9]{6,6})([A-Z0-9]{2,2})([0-9]{4,4})\s*$");
            if (!reg.test(scope.RationcardNumber)) {
                swal('info', 'Please Enter Valid Ration Number', 'info');
                scope.RationcardNumber = "";
                return false;
            }
        };

        //change SGH based on gender       
        scope.changeSHG = function () {
            var gender = scope.Gender;
            if (gender == 'Female') {
                scope.MemberofSHG = 'yes';
                scope.MemberDisabled = false;
                scope.SHGDisabled = false;
            }
            else {
                scope.MemberofSHG = "no";
                scope.MemberDisabled = true;
                scope.SHGDisabled = true;
            }
        }
    }
    app.config([
        '$compileProvider',
        function ($compileProvider) {
            $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|ftp|mailto|chrome-extension):/);
            // Angular before v1.2 uses $compileProvider.urlSanitizationWhitelist(...)
        }
    ]);

})();