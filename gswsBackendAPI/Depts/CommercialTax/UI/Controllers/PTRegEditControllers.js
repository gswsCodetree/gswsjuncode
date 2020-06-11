(function () {
    var app = angular.module("GSWS");

    app.controller("PTRegEditControllers", ["$scope", "$state", "$log", "CT_Services", PTREG_CTRL]);

    function PTREG_CTRL(scope, state, log, ptreg_services) {
        
        scope.rnrno = sessionStorage.getItem("editrnr");
        if (!scope.rnrno) {
            state.go("ui.PTReg");
        }

        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

        scope.preloader = false;
        scope.CategoryDD = ptreg_services.Categories;
        scope.DivisonsDD = ptreg_services.Divisons;
        scope.CirclesDD = ptreg_services.Circles;
        scope.DistrictsDD = ptreg_services.Districts;
        scope.ProfessionDD = ptreg_services.ProfessionTypes;
        scope.CountriesDD = ptreg_services.Countries;
        scope.StatesDD = ptreg_services.States;
        scope.BanksDD = ptreg_services.Banks;
        scope.PartnersDD = ptreg_services.Partners;

        scope.TPeriodsDD = ptreg_services.TaxPeriods;
        scope.GendersDD = ptreg_services.Genders;

        scope.BanksTable = [];
        scope.PartnersTable = [];
        scope.BusinessTable = [];
        scope.UplFilesTable = [];

        scope.MDNominee = "N";
        scope.show_step1 = true;
        //scope.show_step1 = true;

        scope.IsSuccess = false;

        LoadTaxPeriods();
        GetDetails(sessionStorage.getItem("editrnr"));

        // Add Bank Row to table
        scope.addBank = function () {
            if (BankValidation()) {
                //Add the new item to the Array.
                var bak = {
                    bank_code: scope.BankName.bank_code,
                    bankname: scope.BankName.bank_name,
                    ifsc_code: scope.BankIFSC,
                    bank_branch_address: scope.BranchAddr,
                    account_number: scope.BankACNo,
                };

                scope.BankName = null;
                scope.BankIFSC = "";
                scope.BranchAddr = "";
                scope.BankACNo = "";

                scope.BanksTable.push(bak);
            }
        }

        // Remove Bank Row to table
        scope.removeBank = function (index) {
            //Find the record using Index from Array.
            var name = scope.BanksTable[index].bankname;
            if (window.confirm("Do you want to delete: " + name)) {
                //Remove the item from Array using Index.
                scope.BanksTable.splice(index, 1);
            }
        }

        // Add Partner Row to table
        scope.addPartner = function () {
            if (PartnersValidation()) {
                //Add the new item to the Array.
                var partner = {
                    name: scope.PartnerName,
                    DOB: moment(scope.PartnerDOB).format('DD-MM-YYYY'),
                    partner_type: scope.PartnerType,
                    gender: scope.PartnerGender,
                    email_id: scope.PartnerEmail,
                    door_no: scope.PartnerDoor,
                    street: scope.PartnerStreet,
                    location: scope.PartnerLocation,
                    city: scope.PartnerCity,
                    district_name: scope.PartnerDistrict,
                    pin: scope.PartnerPIN,
                    pan_number: scope.PartnerPAN,
                    uid: scope.PartnerUID,
                    phone_number: scope.PartnerPhone,
                    mobile_number: scope.PartnerMobile,
                    is_housewife: scope.PartnerHouseWife,
                };

                scope.PartnerName = "";
                scope.PartnerDOB = null;
                scope.PartnerType = "";
                scope.PartnerGender = "";
                scope.PartnerEmail = "";
                scope.PartnerDoor = "";
                scope.PartnerStreet = "";
                scope.PartnerLocation = "";
                scope.PartnerCity = "";
                scope.PartnerDistrict = "";
                scope.PartnerPIN = "";
                scope.PartnerPAN = "";
                scope.PartnerUID = "";
                scope.PartnerPhone = "";
                scope.PartnerMobile = "";
                scope.PartnerHouseWife = null;

                scope.PartnersTable.push(partner);
            }
        }

        // Remove Partner Row to table
        scope.removePartner = function (index) {
            //Find the record using Index from Array.
            var name = scope.PartnersTable[index].name;
            if (window.confirm("Do you want to delete: " + name)) {
                //Remove the item from Array using Index.
                scope.PartnersTable.splice(index, 1);
            }
        }

        // Add Business Row to table
        scope.addBusiness = function () {
            if (BusinessValidation()) {
                //Add the new item to the Array.
                var business = {
                    door_no: scope.BusinessDoor,
                    street: scope.BusinessStreet,
                    location: scope.BusinessLocation,
                    mandal_code: scope.BusinessMandal,
                    city: scope.BusinessCity,
                    district_name: scope.BusinessDistrict,
                    state_code: scope.BusinessState,
                    pin: scope.BusinessPIN,

                };

                scope.BusinessDoor = "";
                scope.BusinessStreet = "";
                scope.BusinessLocation = "";
                scope.BusinessMandal = "";
                scope.BusinessCity = "";
                scope.BusinessDistrict = "";
                scope.BusinessState = "";
                scope.BusinessPIN = "";

                scope.BusinessTable.push(business);
            }
        }

        // Remove Business Row to table
        scope.removeBusiness = function (index) {

            if (window.confirm("Do you want to delete bussiness address ? ")) {
                //Remove the item from Array using Index.
                scope.BusinessTable.splice(index, 1);
            }
        }

        // Remove Upload Row to table
        scope.removeUpload = function (index) {
            //Find the record using Index from Array.
            var name = scope.UplFilesTable[index].doc_uplname;
            if (window.confirm("Do you want to delete: " + name)) {
                //Remove the item from Array using Index.
                scope.UplFilesTable.splice(index, 1);
            }
        }

        scope.CalPStanding = function () {
            if (scope.selcomyear) {
                var Totalmonths = monthDiff(new Date(scope.selcomyear, 3, 1), new Date());
                var years = Math.floor(Totalmonths / 12);
                var months = Totalmonths % 12;

                scope.profyearmonths = years + " Years & " + months + " Months";
            }
            else { scope.profyearmonths = ""; }
        }

        // Check confirm
        scope.confirm = function () {
            if (!scope.IsOnline) {
                swal("Info", "Please Check and Upload Documents", "info");
                return false;
            }
            else {
                scope.show_step8 = true;
                scope.show_step7 = false;
            }
        }

        // Check the document check box
        scope.DocumentCheckChange = function (sno) {
            if (sno == "1") { if (scope.UplDoc1 == "1") scope.isupload1 = true; else scope.isupload1 = false; }
            else if (sno == "2") { if (scope.UplDoc2 == "1") scope.isupload2 = true; else scope.isupload2 = false; }
            else if (sno == "3") { if (scope.UplDoc3 == "1") scope.isupload3 = true; else scope.isupload3 = false; }
            else if (sno == "4") { if (scope.UplDoc4 == "1") scope.isupload4 = true; else scope.isupload4 = false; }
            else if (sno == "5") { if (scope.UplDoc5 == "1") scope.isupload5 = true; else scope.isupload5 = false; }
            else if (sno == "6") { if (scope.UplDoc6 == "1") scope.isupload6 = true; else scope.isupload6 = false; }
            else if (sno == "7") { if (scope.UplDoc7 == "1") scope.isupload7 = true; else scope.isupload7 = false; }
            else if (sno == "8") { if (scope.UplDoc8 == "1") scope.isupload8 = true; else scope.isupload8 = false; }
            else if (sno == "9") { if (scope.UplDoc9 == "1") scope.isupload9 = true; else scope.isupload9 = false; }
            else if (sno == "10") { if (scope.UplDoc10 == "1") scope.isupload10 = true; else scope.isupload10 = false; }
            else if (sno == "11") { if (scope.UplDoc11 == "1") scope.isupload11 = true; else scope.isupload11 = false; }
        }



        // Upload the document
        scope.UploadDocumnet = function (sno,listcode) {
            var currsno = sno;
            var curruplname = FileNames[sno - 1];
            var chklistcode = listcode;
            var currfilename = "";
            if (!$("#file" + sno).val()) {
                swal("Info", "Please Select File to Upload", "info");
                return false;
            }
            else {
                if (scope.UplFilesTable.length > 1) {
                    swal("Info", "You can Upload Maximum 2 Files Only.", "info");
                    return false;
                }
                else {

                    var fn = $('#file' + sno).val();
                    var startIndex = (fn.indexOf('\\') >= 0 ? fn.lastIndexOf('\\') : fn.lastIndexOf('/'));
                    currfilename = fn.substring(startIndex);
                    currfilename = currfilename.substring(1);
                    //alert(currfilename);
                    var ImageURL = GetCurrBase64(currsno);

                    // Split the base64 string in data and contentType
                    var block = ImageURL.split(";");
                    // Get the content type of the image
                    var contentType = block[0].split(":")[1];// In this case "image/gif"
                    // get the real base64 content of the file
                    var Base64Data = block[1].split(",")[1];// In this case "R0lGODlhPQBEAPeoAJosM...."

                    var req = {
                        "action": "DOCUPLD",
                        "data": Base64Data,
                        "dty": (contentType.indexOf("image") != -1) ? "PHOT" : "ADPR",
                        "cty": contentType,
                        "apty": "REG"
                    }

                    scope.preloader = true;

                    ptreg_services.POSTENCRYPTAPI("UploadDoc", req, token, function (value) {

                        if (value.data.Status == 100) {
                            if (value.data.Details.status_cd == "1") {
                                var upl = {
                                    doc_id: value.data.Details.docupdtls.doc_id,
                                    doc_docname: curruplname,
                                    doc_uplname: currfilename,
                                    doc_ty: value.data.Details.docupdtls.doc_type,
                                    checklist_code: chklistcode,
                                };
                                ClearUploads(currsno);
                                swal("Success", "Document Uploaded Successfully.", "success");

                                scope.UplFilesTable.push(upl);
                            }
                            else if (value.data.Status == "428") {
                                sessionStorage.clear();
                                swal("info", "Session Expired !!!", "info");
                                state.go("Login");
                                return;
                            }
                            else {
                                swal("Info", value.data.Details.error[0].message, "info");
                            }
                            console.log(value.data);
                        }
                        else { swal("Info", value.data.Reason, "info"); }

                        scope.preloader = false;

                    });

                }
            }
        }

        //scope.Register = function () {
        //    var str = '';
        //}

        //PT Registration
        scope.RegisterEdit = function () {
            scope.preloader = true;
            if (Step1Validations() && Step2Validations() && scope.BanksTable.length > 0 && scope.UplFilesTable.length > 1) {
                var AuthDtls = "";
                var EnterpriseDtls = {
                    "rnr": scope.rnrno,
                    "vat_tin": scope.EnteredTIN,
                    "et_tin_grn": scope.EnteredETIN,
                    "Lt_tin_grn": scope.EnteredLTIN,
                    "application_date": moment(scope.AppDate).format('DD-MM-YYYY'),
                    "pt_reg_date": moment(scope.selEDR).format('DD-MM-YYYY'),
                    "owner_type": scope.seltaxperiod,
                    "state_code": "1",
                    "division_code": scope.seldivision,
                    "circle_code": scope.selcircle,
                    "category_type": scope.selcategory,
                    "enterprise_name": scope.Nameofenterprise,
                    "business_pan": scope.EnteredPAN,
                    "email_id": scope.EnteredEmail,
                    "door_no": scope.Entereddoorno,
                    "street": scope.Enteredaddress,
                    "location": scope.Enteredlocality,
                    "city": scope.EnteredCity,
                    "pin": scope.EnteredPIN,
                    "mandal_code": scope.Enteredmandal,
                    "district_name": scope.seldistrict,
                    "phone_number": scope.EnteredPhone,
                    "mobile_number": scope.EnteredMobile,
                    "prof_type": scope.selprofession,
                    "prof_comm_year": scope.selcomyear,
                    "service_tax_no": scope.servtaxno,
                    "annual_turnover": scope.annualturnover,
                    "no_of_employees": scope.noofemployees,
                };

                var OwnerDtls = {
                    "name": scope.MDName,
                    "father_name": scope.MDFName,
                    "DOB": moment(scope.MDdob).format('DD-MM-YYYY'),
                    "gender": scope.MDGender,
                    "email_id": scope.MDEmail,
                    "door_no": scope.MDDoorno,
                    "street": scope.MDstreet,
                    "location": scope.MDLocality,
                    "city": scope.MDCity,
                    "country_code": scope.MDCountry,
                    "district_name": scope.MDDistrict,
                    "state_name": scope.MDState,
                    "pin": scope.MDPIN,
                    "pan_number": scope.MDPAN,
                    "uid": scope.MDAadhaar,
                    "phone_number": scope.MDPhone,
                    "mobile_number": scope.MDMobile,
                    "IsNotAuthrised": scope.MDNominee,
                };

                if (scope.MDNominee == "Y") {
                    AuthDtls = {
                        "name": scope.AuthorName,
                        "father_name": scope.AuthorFName,
                        "DOB": moment(scope.Authordob).format('DD-MM-YYYY'),
                        "gender": scope.AuthorGender,
                        "email_id": scope.AuthorEmail,
                        "door_no": scope.AuthorDoorno,
                        "street": scope.Authorstreet,
                        "location": scope.AuthorLocality,
                        "city": scope.AuthorCity,
                        "district_name": scope.AuthorDistrict,
                        "pin": scope.AuthorPIN,
                        "pan_number": scope.AuthorPAN,
                        "uid": scope.AuthorAadhaar,
                        "phone_number": scope.AuthorPhone,
                        "mobile_number": scope.AuthorMobile,
                    };
                }

                var BankDtls = [];

                for (i = 0; i < scope.BanksTable.length; i++) {
                    var BankObj = {};
                    BankObj.bank_code = scope.BanksTable[i].bank_code;
                    BankObj.ifsc_code = scope.BanksTable[i].ifsc_code;
                    BankObj.bank_branch_address = scope.BanksTable[i].bank_branch_address;
                    BankObj.bank_account_number = scope.BanksTable[i].account_number;

                    BankDtls.push(BankObj);
                }

                var PartdirAddr = [];

                for (i = 0; i < scope.PartnersTable.length; i++) {
                    var ParObj = {};
                    ParObj.name = scope.PartnersTable[i].name;
                    ParObj.DOB = moment(scope.PartnersTable[i].DOB).format('DD-MM-YYYY');
                    ParObj.partner_type = scope.PartnersTable[i].partner_type;
                    ParObj.gender = scope.PartnersTable[i].gender;
                    ParObj.email_id = scope.PartnersTable[i].email_id;
                    ParObj.door_no = scope.PartnersTable[i].door_no;
                    ParObj.street = scope.PartnersTable[i].street;
                    ParObj.location = scope.PartnersTable[i].location;
                    ParObj.city = scope.PartnersTable[i].city;
                    ParObj.district_name = scope.PartnersTable[i].district_name;
                    ParObj.pin = scope.PartnersTable[i].pin;
                    ParObj.pan_number = scope.PartnersTable[i].pan_number;
                    ParObj.uid = scope.PartnersTable[i].uid;
                    ParObj.phone_number = scope.PartnersTable[i].phone_number;
                    ParObj.mobile_number = scope.PartnersTable[i].mobile_number;
                    ParObj.is_housewife = scope.PartnersTable[i].is_housewife;

                    PartdirAddr.push(ParObj);
                }

                var AddbusAddr = [];

                for (i = 0; i < scope.BusinessTable.length; i++) {
                    var AddrObj = {};
                    AddrObj.door_no = scope.BusinessTable[i].door_no;
                    AddrObj.street = scope.BusinessTable[i].street;
                    AddrObj.location = scope.BusinessTable[i].location;
                    AddrObj.city = scope.BusinessTable[i].city;
                    AddrObj.district_name = scope.BusinessTable[i].district_name;
                    AddrObj.state_code = scope.BusinessTable[i].state_code;
                    AddrObj.pin = scope.BusinessTable[i].pin;
                    AddrObj.mandal_code = scope.BusinessTable[i].mandal_code;

                    AddbusAddr.push(AddrObj);
                }

                var Documents = [];

                for (i = 0; i < scope.UplFilesTable.length; i++) {
                    var FileObj = {};

                    FileObj.doc_id = scope.UplFilesTable[i].doc_id; 
                    FileObj.doc_ty = scope.UplFilesTable[i].doc_ty;
                    FileObj.checklist_code = scope.UplFilesTable[i].checklist_code;
                    Documents.push(FileObj);
                }



                var OfficeDtls = {
                    "emp_id": sessionStorage.getItem("secccode"),
                    "emp_name": sessionStorage.getItem("username"),
                    "Place": sessionStorage.getItem("secname"),
                    "designation": sessionStorage.getItem("desinagtionname"),
                    "date": moment(new Date()).format('DD-MM-YYYY'),
                }

                //var req = {
                //    "enterprisedtls": EnterpriseDtls,
                //    "ownerdtls": OwnerDtls,
                //    "authdtls": AuthDtls,
                //    "bank_dtls": scope.BanksTable,
                //    "partdir_addr": scope.PartnersTable,
                //    "addbus_addr": scope.BusinessTable,
                //    "documents": scope.UplFilesTable,
                //    "offdtls": OfficeDtls
                //};

                var req = {
                    "enterprisedtls": EnterpriseDtls,
                    "ownerdtls": OwnerDtls,
                    "authdtls": AuthDtls,
                    "bank_dtls": BankDtls,
                    "partdir_addr": PartdirAddr,
                    "addbus_addr": AddbusAddr,
                    "documents": Documents,
                    "offdtls": OfficeDtls
                };



                var obj = {
                    "action": "MODREG",
                    "data": window.btoa(JSON.stringify(req))
                }

                ptreg_services.POSTENCRYPTAPI("SubmitEditData", obj, token, function (value) {

                    if (value.data.Status == 100) {
                        if (value.data.Details.status_cd == "1") {
                            scope.IsSuccess = true;
                            swal("Success", "Profession Tax Edited Successfully.", "success");
                        }
                        else if (value.data.Status == "428") {
                            sessionStorage.clear();
                            swal("info", "Session Expired !!!", "info");
                            state.go("Login");
                            return;
                        }
                        else {
                            var Message = "";
                            for (i = 0; i < value.data.Details.error.length; i++) {
                                Message += value.data.Details.error[i].message + "\n";
                            }
                            swal("Info", Message, "info");
                        }
                        console.log(value.data);
                    }
                    else { swal("Info", value.data.Reason, "info"); }

                    scope.preloader = false;

                });
            }
            else { scope.preloader = false; }
        }

        scope.Home = function () {
            //window.location.reload();
            state.go("ui.PTReg");
        }

        scope.step2_countrychange = function () {
            if (scope.MDCountry) {
                if (scope.MDCountry == "1")
                    scope.step2_sate = true;
                else
                    scope.step2_sate = false;
            }
            else {
                scope.step2_sate = false;
            }
        }

        scope.step2_statechange = function () {
            if (scope.MDState) {
                if (scope.MDState == "ANDHRA PRADESH")
                    scope.step2_dis = true;
                else
                    scope.step2_dis = false;
            }
            else {
                scope.step2_dis = false;
            }
        }

        scope.Next1 = function () {
            if (Step1Validations()) {
                scope.show_step1 = false;
                scope.show_step2 = true;
            }
        }

        scope.Previous2 = function () {
            scope.show_step1 = true;
            scope.show_step2 = false;
        }

        scope.Next2 = function () {
            if (Step2Validations()) {
                if (scope.MDNominee == "Y") {
                    scope.show_step2 = false;
                    scope.show_step3 = true;
                }
                else {
                    scope.show_step2 = false;
                    scope.show_step4 = true;
                }
            }
        }

        scope.Previous3 = function () {
            scope.show_step2 = true;
            scope.show_step3 = false;
        }

        scope.Next3 = function () {
            if (Step3Validations()) {
                scope.show_step4 = true;
                scope.show_step3 = false;
            }
        }

        scope.Previous4 = function () {
            if (scope.MDNominee == "Y") {
                scope.show_step4 = false;
                scope.show_step3 = true;
            }
            else {
                scope.show_step4 = false;
                scope.show_step2 = true;
            }
        }

        scope.Next4 = function () {
            if (scope.BanksTable.length > 0) {
                scope.show_step4 = false;
                scope.show_step5 = true;
            }
            else {
                swal("Info", "Please Add Bank Details", "info");
                return false;
            }
        }

        scope.Previous5 = function () {
            scope.show_step4 = true;
            scope.show_step5 = false;
        }

        scope.Next5 = function () {
            scope.show_step6 = true;
            scope.show_step5 = false;
        }

        scope.Previous6 = function () {
            scope.show_step5 = true;
            scope.show_step6 = false;
        }

        scope.Next6 = function () {
            scope.show_step7 = true;
            scope.show_step6 = false;
        }

        scope.Previous7 = function () {
            scope.show_step6 = true;
            scope.show_step7 = false;
        }

        scope.Next7 = function () {
            if (scope.IsOnline == "Y") {
                scope.show_step8 = true;
                scope.show_step7 = false;
            }
            else {
                swal("Info", "You Must Confirm to Finish", "info");
                return false;
            }
        }

        scope.Previous8 = function () {
            scope.show_step7 = true;
            scope.show_step8 = false;
        }

        scope.Next8 = function () {
            

            //scope.show_step9 = true;
            //scope.show_step8 = false;
            if (scope.UplFilesTable.length > 1) {
                scope.show_step9 = true;
                scope.show_step8 = false;
            }
            else {
                swal("Info", "Please Upload 2 Documents to Finish", "info");
                return false;
            }
        }

        scope.Edit = function () {
            scope.show_step1 = true;
            scope.show_step9 = false;
        }


        // Get Details By RNR
        function GetDetails(tno) {
            if (tno) {
                var obj = {
                    "action": "GETDTLS",
                    "tin": tno
                }
                ptreg_services.POSTENCRYPTAPI("GetDetailsByRNR", obj, token, function (value) {

                    if (value.data.Status == 100) {
                        var res = value.data.Details;
                        if (res.status_cd == "1") {
                            var decriptdata = window.atob(res.data);
                            var JsonData = JSON.parse(decriptdata);
                            FillDetails(JsonData);
                        }
                        else if (value.data.Status == "428") {
                            sessionStorage.clear();
                            swal("info", "Session Expired !!!", "info");
                            state.go("Login");
                            return;
                        }
                        else {
                            swal("Info", res.error[0].message, "info");
                        }
                        console.log(res);
                    }
                    else { swal("Info", value.data.Reason, "info"); }

                    scope.preloader = false;

                });
            }
        }

        function FillDetails(data) {
            var enobj = data.enterprisedetails;
            var owobj = data.ownerdetails;
            var auobj = data.autheriseddetails;
            var baobj = data.bankdetails;
            var probj = data.partnerdetails;
            var addrobj = data.get_addbus_addr;

            if (enobj) {
                scope.EnteredTIN = enobj.tin_grn;
                scope.AppDate = new Date(enobj.app_date);
                scope.selEDR = new Date(enobj.reg_date);
                scope.seltaxperiod = enobj.owner_type;
                scope.seldivision = enobj.divisioncode;
                scope.selcircle = enobj.circlecode;
                scope.selcategory = enobj.category_type;

                scope.Nameofenterprise = enobj.enterprise_name;
                scope.EnteredPAN = enobj.pan_number;
                scope.EnteredEmail = enobj.email_id;
                scope.EnteredPhone = enobj.phone_number;
                scope.EnteredMobile = enobj.mobile_number;
                scope.Entereddoorno = enobj.door_no;
                scope.Enteredaddress = enobj.street;
                scope.Enteredlocality = enobj.location;
                scope.Enteredmandal = enobj.mandal_code;
                scope.seldistrict = enobj.district_name;
                scope.EnteredPIN = enobj.pin;


                scope.servtaxno = enobj.service_tax_no;
                scope.annualturnover = enobj.annual_turnover;
                scope.noofemployees = enobj.employees;
                scope.selprofession = enobj.prof_type;
                scope.selcomyear = enobj.prof_comm_year;
                scope.CalPStanding();
            }

            if (owobj) {
                scope.MDCity = owobj.city;
                scope.MDCountry = owobj.country_code;
                scope.MDDistrict = owobj.district_name;
                scope.MDdob = new Date(owobj.dob);
                scope.MDDoorno = owobj.door_no;
                scope.MDEmail = owobj.email_id;
                scope.MDFName = owobj.father_name;
                scope.MDGender = owobj.gender;
                scope.MDLocality = owobj.location;
                scope.MDMobile = owobj.mobile_number;
                scope.MDName = owobj.name;
                scope.MDPAN = owobj.pan_number;
                scope.MDNominee = (owobj.partner_type == "1" ? "Y" : "N");
                scope.MDPhone = owobj.phone_number;
                scope.MDPIN = owobj.pin;
                scope.MDState = owobj.state_code;
                scope.MDstreet = owobj.street;
                scope.MDAadhaar = owobj.uid;
            }

            if (auobj) {
                scope.AuthorCity = auobj.city;
                scope.AuthorCountry = auobj.country_code;
                scope.AuthorDistrict = auobj.district_name;
                scope.Authordob = new Date(auobj.dob);
                scope.AuthorDoorno = auobj.door_no;
                scope.AuthorEmail = auobj.email_id;
                scope.AuthorFName = auobj.father_name;
                scope.AuthorGender = auobj.gender;
                scope.AuthorLocality = auobj.location;
                scope.AuthorMobile = auobj.mobile_number;
                scope.AuthorName = auobj.name;
                scope.AuthorPAN = auobj.pan_number;
                scope.AuthorPatrner = auobj.partner_type;
                scope.AuthorPhone = auobj.phone_number;
                scope.AuthorPIN = auobj.pin;
                scope.AuthorState = auobj.state_code;
                scope.Authorstreet = auobj.street;
                scope.AuthorAadhaar = auobj.uid;
            }

            if (baobj) {
                for (i = 0; i < baobj.length; i++) {
                    var bObj = {};
                    var bankname = baobj[i].bankname;
                    var bankcode = baobj[i].bankcode;
                    if (!bankname) {
                        var obj = $(scope.BanksDD).filter(function (i, n) { return n.bank_code === bankcode });
                        bankname = obj[0]["bank_name"];
                    }
                    bObj.bankname = bankname;
                    bObj.bank_code = baobj[i].bankcode;
                    bObj.ifsc_code = baobj[i].ifsccode;
                    bObj.bank_branch_address = baobj[i].BankBranchCode;
                    bObj.account_number = baobj[i].accountnumber;

                    scope.BanksTable.push(bObj);
                }
            }

            if (probj) {
                for (i = 0; i < probj.length; i++) {
                    var pObj = {};

                    pObj.city = probj[i].city;
                    pObj.country_code = probj[i].country_code;
                    pObj.district_name = probj[i].district_name;
                    pObj.DOB = moment(probj[i].dob).format('DD-MM-YYYY');
                    pObj.door_no = probj[i].door_no;
                    pObj.email_id = probj[i].email_id;
                    pObj.father_name = probj[i].father_name;
                    pObj.gender = probj[i].gender;
                    pObj.location = probj[i].location;
                    pObj.mobile_number = probj[i].mobile_number;
                    pObj.name = probj[i].name;
                    pObj.pan_number = probj[i].pan_number;
                    pObj.partner_type = probj[i].partner_type;
                    pObj.phone_number = probj[i].phone_number;
                    pObj.pin = probj[i].pin;
                    pObj.state_code = probj[i].state_code;
                    pObj.street = probj[i].street;
                    pObj.uid = probj[i].uid;

                    scope.PartnersTable.push(pObj);
                }
            }

            if (addrobj) {
                for (i = 0; i < addrobj.length; i++) {
                    var aObj = {};

                    aObj.door_no = addrobj[i].door_no;
                    aObj.street = addrobj[i].street;
                    aObj.location = addrobj[i].location;
                    aObj.mandal_code = addrobj[i].mandal_code;
                    aObj.city = addrobj[i].city;
                    aObj.district_name = addrobj[i].district_name;
                    aObj.state_code = addrobj[i].state_code;
                    aObj.pin = addrobj[i].pin;
                    

                    scope.BusinessTable.push(aObj);
                }
            }

            //var file1 = {};
            //file1.doc_id = "201911267661";
            //file1.doc_ty = "PHOT";
            //scope.UplFilesTable.push(file1);

            //var file2 = {};
            //file2.doc_id = "201911266683";
            //file2.doc_ty = "PHOT";
            //scope.UplFilesTable.push(file2);
        }

        function monthDiff(start, end) {
            var tempDate = new Date(start);
            var monthCount = 0;
            while ((tempDate.getMonth() + '' + tempDate.getFullYear()) != (end.getMonth() + '' + end.getFullYear())) {
                monthCount++;
                tempDate.setMonth(tempDate.getMonth() + 1);
            }
            return monthCount + 1;
        }

        function LoadTaxPeriods() {
            var PeriodYears = [];
            var CurrYear = new Date().getFullYear();
            var MinYear = 2014;
            CurrYear = (new Date().getMonth() < 3 ? CurrYear - 1 : CurrYear);
            for (i = CurrYear; i >= MinYear; i--) {
                item = {}
                item["id"] = i;
                item["value"] = i + " - " + (i + 1);
                PeriodYears.push(item);
            }

            scope.TaxPeriodDD = PeriodYears;
        }

        function GetCurrBase64(sno) {
            var ImageURL = "";

            if (sno == "1")
                ImageURL = File1Base64;
            else if (sno == "2")
                ImageURL = File2Base64;
            else if (sno == "3")
                ImageURL = File3Base64;
            else if (sno == "4")
                ImageURL = File4Base64;
            else if (sno == "5")
                ImageURL = File5Base64;
            else if (sno == "6")
                ImageURL = File6Base64;
            else if (sno == "7")
                ImageURL = File7Base64;
            else if (sno == "8")
                ImageURL = File8Base64;
            else if (sno == "9")
                ImageURL = File9Base64;
            else if (sno == "10")
                ImageURL = File10Base64;
            else if (sno == "11")
                ImageURL = File11Base64;

            return ImageURL;
        }

        // Clear Upload Values
        function ClearUploads(sno) {

            $('#file' + sno).val("");
            if (sno == "1") {
                File1Base64 = "";
                scope.isupload1 = false;
                scope.UplDoc1 = "";
            }
            else if (sno == "2") {
                File2Base64 = "";
                scope.isupload2 = false;
                scope.UplDoc2 = "";
            }
            else if (sno == "3") {
                File3Base64 = "";
                scope.isupload3 = false;
                scope.UplDoc3 = "";
            }
            else if (sno == "4") {
                File4Base64 = "";
                scope.isupload4 = false;
                scope.UplDoc4 = "";
            }
            else if (sno == "5") {
                File5Base64 = "";
                scope.isupload5 = false;
                scope.UplDoc5 = "";
            }
            else if (sno == "6") {
                File6Base64 = "";
                scope.isupload6 = false;
                scope.UplDoc6 = "";
            }
            else if (sno == "7") {
                File7Base64 = "";
                scope.isupload7 = false;
                scope.UplDoc7 = "";
            }
            else if (sno == "8") {
                File8Base64 = "";
                scope.isupload8 = false;
                scope.UplDoc8 = "";
            }
            else if (sno == "9") {
                File9Base64 = "";
                scope.isupload9 = false;
                scope.UplDoc9 = "";
            }
            else if (sno == "10") {
                File10Base64 = "";
                scope.isupload10 = false;
                scope.UplDoc10 = "";
            }
            else if (sno == "11") {
                File11Base64 = "";
                scope.isupload111 = false;
                scope.UplDoc11 = "";
            }
        }

        // Step 1 Validations 
        function Step1Validations() {
            var emailRegex = new RegExp("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");
            var AlphaNumericRegex = new RegExp("^(?=.*[a-zA-Z0-9&,/ -])");

            if (!scope.AppDate) {
                swal("Info", "Please Enter Application Date", "info");
                return false;
            }
            else if (!scope.selEDR) {
                swal("Info", "Please Enter EDR Date", "info");
                return false;
            }
            else if (!scope.seltaxperiod) {
                swal("Info", "Please Select Tax Period", "info");
                return false;
            }
            else if (!scope.seldivision) {
                swal("Info", "Please Select Division", "info");
                return false;
            }
            else if (!scope.selcircle) {
                swal("Info", "Please Select Circle", "info");
                return false;
            }
            else if (!scope.selcategory) {
                swal("Info", "Please Select Category Type", "info");
                return false;
            }
            else if (!scope.Nameofenterprise) {
                swal("Info", "Please Enter Name of Enterprise", "info");
                return false;
            }
            else if (!scope.EnteredPAN) {
                swal("Info", "Please Enter PAN of Enterprise", "info");
                return false;
            }
            else if (!scope.EnteredEmail) {
                swal("Info", "Please Enter Email of Enterprise", "info");
                return false;
            }
            else if (!emailRegex.test(scope.EnteredEmail)) {
                swal("Info", "Invalid Email.", "info");
                return false;
            }
            else if (!scope.Entereddoorno) {
                swal("Info", "Please Enter Door Number", "info");
                return false;
            }
            else if (!scope.Enteredaddress) {
                swal("Info", "Please Enter Street", "info");
                return false;
            }
            else if (!scope.Enteredlocality) {
                swal("Info", "Please Enter Loaction", "info");
                return false;
            }
            else if (!scope.EnteredCity) {
                swal("Info", "Please Enter City", "info");
                return false;
            }
            else if (!scope.Enteredmandal) {
                swal("Info", "Please Enter Mandal/Muncipality", "info");
                return false;
            }
            else if (!scope.seldistrict) {
                swal("Info", "Please Select District", "info");
                return false;
            }
            else if (!scope.EnteredPIN) {
                swal("Info", "Please Enter PIN", "info");
                return false;
            }
            else if (!scope.EnteredPhone) {
                swal("Info", "Please Enter Phone Number", "info");
                return false;
            }
            else if (!scope.EnteredMobile) {
                swal("Info", "Please Enter Mobile Number", "info");
                return false;
            }
            else if (scope.EnteredMobile.length < 10) {
                swal("Info", "Mobile Number Should be 10 Digits", "info");
                return false;
            }
            else if (!scope.selprofession) {
                swal("Info", "Please Select Profession,Trade or Calling", "info");
                return false;
            }
            else if (!scope.selcomyear) {
                swal("Info", "Please Select Profession Commitment Year", "info");
                return false;
            }

            return true;
        }

        // Step 2 Validations 
        function Step2Validations() {
            var emailRegex = new RegExp("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");
            var AlphaNumericRegex = new RegExp("^(?=.*[a-zA-Z0-9&,/ -])");

            if (!scope.MDName) {
                swal("Info", "Please Enter Name of MD/Owner", "info");
                return false;
            }
            else if (!scope.MDFName) {
                swal("Info", "Please Enter Father Name of MD/Owner", "info");
                return false;
            }
            else if (!scope.MDdob) {
                swal("Info", "Please Enter DOB of MD/Owner", "info");
                return false;
            }
            else if (!scope.MDGender) {
                swal("Info", "Please Select Gender of MD/Owner", "info");
                return false;
            }

            else if (scope.MDEmail && !emailRegex.test(scope.MDEmail)) {
                swal("Info", "Invalid Email. of MD/Owner", "info");
                return false;
            }

            else if (!scope.MDDoorno) {
                swal("Info", "Please Enter Door Number of MD/Owner", "info");
                return false;
            }
            else if (!scope.MDstreet) {
                swal("Info", "Please Enter Street Name of MD/Owner", "info");
                return false;
            }
            else if (!scope.MDLocality) {
                swal("Info", "Please Enter Locality of MD/Owner", "info");
                return false;
            }
            else if (!scope.MDCity) {
                swal("Info", "Please Enter City of MD/Owner", "info");
                return false;
            }
            else if (!scope.MDCountry) {
                swal("Info", "Please Select Country of MD/Owner", "info");
                return false;
            }
            else if (scope.MDCountry == "1" && !scope.MDState) {
                swal("Info", "Please Select State of MD/Owner", "info");
                return false;
            }
            else if (scope.MDState == "1" && !scope.MDDistrict) {
                swal("Info", "Please Select District of MD/Owner", "info");
                return false;
            }
            else if (!scope.MDPIN) {
                swal("Info", "Please Enter PIN Code of MD/Owner", "info");
                return false;
            }
            else if (!scope.MDPAN) {
                swal("Info", "Please Enter PAN of MD/Owner", "info");
                return false;
            }
            else if (!scope.MDAadhaar) {
                swal("Info", "Please Enter Aadhaar of MD/Owner", "info");
                return false;
            }
            else if (!scope.MDPhone) {
                swal("Info", "Please Enter Phone Number of MD/Owner", "info");
                return false;
            }
            else if (!scope.MDMobile) {
                swal("Info", "Please Enter Mobile Number of MD/Owner", "info");
                return false;
            }
            else if (scope.MDMobile.length < 10) {
                swal("Info", "MD/Owner Mobile Number Should be 10 Digits ", "info");
                return false;
            }

            return true;
        }

        // Step 3 Validations 
        function Step3Validations() {
            var emailRegex = new RegExp("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");
            var AlphaNumericRegex = new RegExp("^(?=.*[a-zA-Z0-9&,/ -])");


            if (!scope.AuthorName) {
                swal("Info", "Please Enter Name of Authorized Person", "info");
                return false;
            }
            else if (!scope.AuthorFName) {
                swal("Info", "Please Enter Father Name of Authorized Person", "info");
                return false;
            }
            else if (!scope.Authordob) {
                swal("Info", "Please Enter DOB of Authorized Person", "info");
                return false;
            }
            else if (!scope.AuthorGender) {
                swal("Info", "Please Select Gender of Authorized Person", "info");
                return false;
            }
            else if (scope.AuthorEmail && !emailRegex.test(scope.AuthorEmail)) {
                swal("Info", "Invalid Email. of MD/Owner", "info");
                return false;
            }
            else if (!scope.AuthorDoorno) {
                swal("Info", "Please Enter Door No of Authorized Person", "info");
                return false;
            }
            else if (!scope.Authorstreet) {
                swal("Info", "Please Enter Street Name of Authorized Person", "info");
                return false;
            }
            else if (!scope.AuthorLocality) {
                swal("Info", "Please Enter Locality of Authorized Person", "info");
                return false;
            }
            else if (!scope.AuthorCity) {
                swal("Info", "Please Enter City of Authorized Person", "info");
                return false;
            }
            else if (!scope.AuthorDistrict) {
                swal("Info", "Please Select District of Authorized Person", "info");
                return false;
            }
            else if (!scope.AuthorPIN) {
                swal("Info", "Please Enter PIN Number of Authorized Person", "info");
                return false;
            }
            else if (!scope.AuthorPAN) {
                swal("Info", "Please Enter PAN Number of Authorized Person", "info");
                return false;
            }
            else if (!scope.AuthorAadhaar) {
                swal("Info", "Please Enter Aadhaar Number of Authorized Person", "info");
                return false;
            }
            else if (!scope.AuthorPhone) {
                swal("Info", "Please Enter Phone Numbeer of Authorized Person", "info");
                return false;
            }
            else if (!scope.AuthorMobile) {
                swal("Info", "Please Enter Mobile Number of Authorized Person", "info");
                return false;
            }
            else if (scope.AuthorMobile.length < 10) {
                swal("Info", "Authorized Person Mobile Number Should be 10 Digits ", "info");
                return false;
            }

            return true;
        }

        function Validations() {
            var emailRegex = new RegExp("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");
            var AlphaNumericRegex = new RegExp("^(?=.*[a-zA-Z0-9&,/ -])");

            // Step 4 Validations



            return true;
        }

        function BankValidation() {
            var emailRegex = new RegExp("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");

            if (!scope.BankName) {
                swal("Info", "Please Select Bank Name", "info");
                return false;
            }
            else if (!scope.BankIFSC) {
                swal("Info", "Please Enter IFSC Code", "info");
                return false;
            }
            else if (!scope.BranchAddr) {
                swal("Info", "Please Enter Branch Address", "info");
                return false;
            }
            else if (!scope.BankACNo) {
                swal("Info", "Please Enter Account Number", "info");
                return false;
            }

            return true;
        }

        function PartnersValidation() {
            var emailRegex = new RegExp("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");

            if (!scope.PartnerName) {
                swal("Info", "Please Enter Name", "info");
                return false;
            }
            else if (!scope.PartnerDOB) {
                swal("Info", "Please Select Date of Birth", "info");
                return false;
            }
            else if (!scope.PartnerType) {
                swal("Info", "Please Select Partner Type", "info");
                return false;
            }
            else if (!scope.PartnerGender) {
                swal("Info", "Please Select Gender", "info");
                return false;
            }
            else if (!scope.PartnerHouseWife) {
                swal("Info", "Please Select House Wife", "info");
                return false;
            }
            else if (!scope.PartnerEmail) {
                swal("Info", "Please Enter Email of Partner", "info");
                return false;
            }
            else if (scope.PartnerEmail && !emailRegex.test(scope.PartnerEmail)) {
                swal("Info", "Invalid Email. of Partner", "info");
                return false;
            }
            else if (!scope.PartnerDoor) {
                swal("Info", "Please Enter Door Number", "info");
                return false;
            }
            else if (!scope.PartnerStreet) {
                swal("Info", "Please Enter Street", "info");
                return false;
            }
            else if (!scope.PartnerLocation) {
                swal("Info", "Please Enter Location", "info");
                return false;
            }
            else if (!scope.PartnerCity) {
                swal("Info", "Please Enter City", "info");
                return false;
            }
            else if (!scope.PartnerDistrict) {
                swal("Info", "Please Select District", "info");
                return false;
            }
            else if (!scope.PartnerPIN) {
                swal("Info", "Please Enter PIN", "info");
                return false;
            }
            else if (!scope.PartnerPAN) {
                swal("Info", "Please Enter PAN", "info");
                return false;
            }
            else if (!scope.PartnerUID) {
                swal("Info", "Please Enter Aadhaar", "info");
                return false;
            }
            else if (!scope.PartnerPhone) {
                swal("Info", "Please Enter Phone", "info");
                return false;
            }
            else if (!scope.PartnerMobile) {
                swal("Info", "Please Enter Mobile", "info");
                return false;
            }
            else if (scope.PartnerMobile.length < 10) {
                swal("Info", "Mobile Number Should be 10 Digits ", "info");
                return false;
            }

            return true;
        }

        function BusinessValidation() {

            if (!scope.BusinessDoor) {
                swal("Info", "Please Enter Door Number", "info");
                return false;
            }
            else if (!scope.BusinessStreet) {
                swal("Info", "Please Enter Street", "info");
                return false;
            }
            else if (!scope.BusinessLocation) {
                swal("Info", "Please Enter Location", "info");
                return false;
            }
            else if (!scope.BusinessMandal) {
                swal("Info", "Please Enter Mandal", "info");
                return false;
            }
            else if (!scope.BusinessCity) {
                swal("Info", "Please Enter City", "info");
                return false;
            }
            else if (!scope.BusinessDistrict) {
                swal("Info", "Please Select District", "info");
                return false;
            }
            else if (!scope.BusinessState) {
                swal("Info", "Please Select State", "info");
                return false;
            }
            else if (!scope.BusinessPIN) {
                swal("Info", "Please Enter PIN", "info");
                return false;
            }

            return true;
        }
    }
})();

