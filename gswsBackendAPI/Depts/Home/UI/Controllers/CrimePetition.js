(function () {
    /* eslint-disable */
    var app = angular.module("GSWS");
    app.controller("CrimePetition", ["$scope", "Home_Services", '$sce', '$http', "$state", Cert_CTRL]);

       function Cert_CTRL(scope, home_services, sce, $http, state) {

        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");
        if (!(token) || !(user)) {
            alert('Session expired..!');
            state.go("Login");
        } 
        scope.pagename = "CrimePetition";
       
        scope.preloader = false;
        scope.step_position = 1;
        scope.finalsave = false;
        scope.finalsavevalue == "0"
        scope.MasterData = [];
         //  LoadMasters();
           scope.RespondentTable = [];
           scope.filesuplTable = [];

           scope.DistrictDD5 = HDDistricts;
           //scope.PoliceStationDD = HDPoliceStations;
           scope.GenderDD = HDGender;
           scope.PetStateDD = HDState;
           scope.RelationDD = HDRelation;
           scope.OccupationDD = HDOccupation;
           scope.CasteDD = HDCaste;
           scope.MaritalDD = HDMaritalStatus;
           scope.EducationDD = HDEducation;
           scope.ReligionDD = HDReligion;
           scope.NationalityDD = HDNationality;
           scope.ResidenceTypeDD = HDResidency;
           scope.CountryDD = HDCountry;
           scope.StateDD = HDState;
           scope.IDProofDD = HDIDProof;
           scope.AddrProofDD = HDAddressProof;
           scope.BankNameDD = HDBanks;
           scope.FileSubDD = HDFileSubType;
           scope.PetitionSubDD = HDPetitionSubject;
           scope.VillageDD = HDVillage;
          
       

        //Save Data
           scope.save = function () {        

            if (Step1Validations() && Step2Validations() && Step3Validations() && scope.filesuplTable.length > 0) {
                scope.preloader = true;

                if (scope.ResDets == "Y") {
                    scope.PTdoorno = scope.doorno; 
                    scope.PTStreet = scope.Street;
                    scope.PTLocality = scope.Locality;
                    scope.PTLandMark = scope.LandMark;
                    scope.PTWard = scope.Ward;
                    scope.PTMandal = scope.Mandal;
                    scope.PTSelResidenceType = scope.SelResidenceType;
                    scope.PTSelVillage = scope.SelVillage;
                    scope.PTselcountry = scope.selcountry;
                    scope.PTselState = scope.selState;
                    scope.PTseldistrict = scope.seldistrict;
                    scope.PTselPS = scope.selPS;
                    scope.PTPincode = scope.Pincode;
                };

                var req = {}
                if (scope.RespondentTable.length > 0) {
                    req =
                        {
						"crime_pet_id": moment(new Date()).format('DDMMYYYYHHMMSS'),
						"gws_id": sessionStorage.getItem("TransID"),
						"pet_state_cd": scope.PetState,
						"pet_district_cd": scope.selPetdistrict,
						"pet_ps_cd": scope.selPetPS,
						"pet_application_date": moment(scope.PDate).format('DD/MM/YYYY'),
						"pet_received_type_cd": "12087",
						"pet_subject_cd": (scope.Psubject ? scope.Psubject : ""),
						"pet_description": (scope.PDesc ? scope.PDesc : ""),
						"applicant_first_name": (scope.Name ? scope.Name : ""),
						"applicant_middle_name": (scope.MiddleName ? scope.MiddleName : ""),
						"applicant_last_name": (scope.LastName ? scope.LastName : ""),
						"applicant_date_of_birth": moment(scope.dob).format('DD/MM/YYYY'),
						"applicant_age": (scope.age ? scope.age : ""),
						"applicant_relation_name": (scope.FHGName ? scope.FHGName : ""),
						"applicant_relation_type": (scope.RType ? scope.RType : ""),
						"applicant_occupation_cd": (scope.occupation ? scope.occupation : ""),
						"applicant_other_occupation_cd": (scope.OthOccupation ? scope.OthOccupation : ""),
						"applicant_gender_cd": (scope.selgender ? scope.selgender : ""),
						"applicant_caste_tribe_cd": (scope.caste ? scope.caste : ""),
						"applicant_sub_caste_cd": (scope.Subcaste ? scope.Subcaste : ""),
						"applicant_religion_cd": (scope.Religion ? scope.Religion : ""),
						"applicant_nationality_cd": (scope.Nationality ? scope.Nationality : ""),
						"applicant_edu_qual_cd": (scope.Education1 ? scope.Education1 : ""),
						"applicant_oth_edu_quali": (scope.Education2 ? scope.Education2 : ""),
						"applicant_telephone_off": (scope.OMobileNo ? scope.OMobileNo : ""),
						"applicant_telephone_residence": (scope.AMobileNo ? scope.AMobileNo : ""),
						"applicant_mobile_number": (scope.MobileNo ? scope.MobileNo : ""),
						"applicant_fax_no": (scope.Faxno ? scope.Faxno : ""),
						"applicant_email_id": (scope.EmailId ? scope.EmailId : ""),
						"applicant_alternative_email": (scope.AEmailId ? scope.AEmailId : ""),
						"applicant_full_name": (scope.ApFullName ? scope.ApFullName : ""),
						"applicant_marital_status_cd": (scope.MaritalStatus ? scope.MaritalStatus : ""),
						"applicant_alias_name": (scope.ApAliasName ? scope.ApAliasName : ""),
						"presnt_house_no": (scope.doorno ? scope.doorno : ""),
						"presnt_street_road_no": (scope.Street ? scope.Street : ""),
						"presnt_locality": (scope.Locality ? scope.Locality : ""),
						"presnt_landmark_milestone": (scope.LandMark ? scope.LandMark : ""),
						"presnt_ward_colony": (scope.Ward ? scope.Ward : ""),
						"presnt_area_mandal": (scope.Mandal ? scope.Mandal : ""),
						"presnt_residence_type": (scope.SelResidenceType ? scope.SelResidenceType : ""),
						"presnt_village": (scope.SelVillage ? scope.SelVillage : ""),
						"presnt_country_cd": (scope.selcountry ? scope.selcountry : ""),
						"presnt_state_cd": (scope.selState ? scope.selState : ""),
						"presnt_district_cd": (scope.seldistrict ? scope.seldistrict : ""),
						"presnt_ps_cd": (scope.selPS ? scope.selPS : ""),
						"presnt_pincode": (scope.Pincode ? scope.Pincode : ""),
						"present_other_country_cd": (scope.Pselcountry ? scope.Pselcountry : ""),
						"present_other_address_line_1": (scope.AddressL1 ? scope.AddressL1 : ""),
						"present_other_address_line_2": (scope.AddressL2 ? scope.AddressL2 : ""),
						"present_other_address_line_3": (scope.AddressL3 ? scope.AddressL3 : ""),
						"present_other_pincode": (scope.POPincode ? scope.POPincode : ""),
						"is_permanant_addr_same": (scope.ResDets ? scope.ResDets : ""),
						"permnt_house_no": (scope.PTdoorno ? scope.PTdoorno : ""),
						"permnt_street_road_no": (scope.PTStreet ? scope.PTStreet : ""),
						"permnt_locality": (scope.PTLocality ? scope.PTLocality : ""),
						"permnt_landmark_milestone": (scope.PTLandMark ? scope.PTLandMark : ""),
						"permnt_ward_colony": (scope.PTWard ? scope.PTWard : ""),
						"permnt_area_mandal": (scope.PTMandal ? scope.PTMandal : ""),
						"permnt_residence_type": (scope.PTSelResidenceType ? scope.PTSelResidenceType : ""),
						"permnt_village": (scope.PTSelVillage ? scope.PTSelVillage : ""),
						"permnt_country_cd": (scope.PTselcountry ? scope.PTselcountry : ""),
						"permnt_state_cd": (scope.PTselState ? scope.PTselState : ""),
						"permnt_district_cd": (scope.PTseldistrict ? scope.PTseldistrict : ""),
						"permnt_ps_cd": (scope.PTselPS ? scope.PTselPS : ""),
						"permnt_pincode": (scope.PTPincode ? scope.PTPincode : ""),
						"id_proof_type_cd": (scope.selIDProof ? scope.selIDProof : ""),
						"id_id_no": (scope.IDProof ? scope.IDProof : ""),
						"id_issue_date": (scope.IDIssueDate ? moment(scope.IDIssueDate).format('DD/MM/YYYY') : ""),
						"id_issue_place": (scope.IDIssuePlace ? scope.IDIssuePlace : ""),
						"id_issue_authority": (scope.IDIssueAuth ? scope.IDIssueAuth : ""),
						"id_issue_authority_other": (scope.IDIssueOAuth ? scope.IDIssueOAuth : ""),
						"address_proof_type_cd": (scope.selAddrProof ? scope.selAddrProof : ""),
						"addr_id_no": (scope.AddrProof ? scope.AddrProof : ""),
						"addr_issue_date": (scope.AddrIssueDate ? moment(scope.AddrIssueDate).format('DD/MM/YYYY') : ""),
						"addr_issue_place": (scope.AddrIssuePlace ? scope.AddrIssuePlace : ""),
						"addr_issue_authority": (scope.AddrIssueAuth ? scope.AddrIssueAuth : ""),
						"addr_issue_authority_other": (scope.AddrIssueOAuth ? scope.AddrIssueOAuth : ""),
                            "challan_ref_no": "",
                            "bank_name": "",
                            "branch_code": "",
                            "payment_date": "",
                            "challan_amount": "",
                            "journal_number": "",
                            "RespondentDetails": scope.RespondentTable,
                            "Files": scope.filesuplTable

                        }
                }
                else {
                    req =
                        {
						"crime_pet_id": moment(new Date()).format('DDMMYYYYHHMMSS'),
						"gws_id": sessionStorage.getItem("TransID"),
                            "pet_state_cd": scope.PetState,
                            "pet_district_cd": scope.selPetdistrict,
                            "pet_ps_cd": scope.selPetPS,
                            "pet_application_date": moment(scope.PDate).format('DD/MM/YYYY'),
                            "pet_received_type_cd": "12087",
                        "pet_subject_cd": (scope.Psubject ? scope.Psubject :""),
                        "pet_description": (scope.PDesc ? scope.PDesc : ""),
                        "applicant_first_name": (scope.Name ? scope.Name : ""),
                        "applicant_middle_name": (scope.MiddleName ? scope.MiddleName :""),
                        "applicant_last_name": (scope.LastName ? scope.LastName : ""),
                            "applicant_date_of_birth": moment(scope.dob).format('DD/MM/YYYY'),
                        "applicant_age": (scope.age ? scope.age : ""),
                        "applicant_relation_name": (scope.FHGName ? scope.FHGName : ""),
                        "applicant_relation_type": (scope.RType ? scope.RType : ""),
                        "applicant_occupation_cd": (scope.occupation ? scope.occupation : ""),
                        "applicant_other_occupation_cd": (scope.OthOccupation ? scope.OthOccupation : ""),
                        "applicant_gender_cd": (scope.selgender ? scope.selgender : ""),
                        "applicant_caste_tribe_cd": (scope.caste ? scope.caste : ""),
                        "applicant_sub_caste_cd": (scope.Subcaste ? scope.Subcaste : ""),
                        "applicant_religion_cd": (scope.Religion ? scope.Religion : ""),
                        "applicant_nationality_cd": (scope.Nationality ? scope.Nationality : ""),
                        "applicant_edu_qual_cd": (scope.Education1 ? scope.Education1 : ""),
                        "applicant_oth_edu_quali": (scope.Education2 ? scope.Education2 : ""),
                        "applicant_telephone_off": (scope.OMobileNo ? scope.OMobileNo : ""),
                        "applicant_telephone_residence": (scope.AMobileNo ? scope.AMobileNo : ""),
                        "applicant_mobile_number": (scope.MobileNo ? scope.MobileNo : ""),
                        "applicant_fax_no": (scope.Faxno ? scope.Faxno : ""),
                        "applicant_email_id": (scope.EmailId ? scope.EmailId : ""),
                        "applicant_alternative_email": (scope.AEmailId ? scope.AEmailId : ""),
                        "applicant_full_name": (scope.ApFullName ? scope.ApFullName : ""),
                        "applicant_marital_status_cd": (scope.MaritalStatus ? scope.MaritalStatus : ""),
                        "applicant_alias_name": (scope.ApAliasName ? scope.ApAliasName : ""),
                            "presnt_house_no": (scope.doorno ? scope.doorno : ""),
                        "presnt_street_road_no": (scope.Street ? scope.Street : ""),
                        "presnt_locality": (scope.Locality ? scope.Locality : ""),
                        "presnt_landmark_milestone": (scope.LandMark ? scope.LandMark : ""),
                        "presnt_ward_colony": (scope.Ward ? scope.Ward : ""),
                        "presnt_area_mandal": (scope.Mandal ? scope.Mandal : ""),
                        "presnt_residence_type": (scope.SelResidenceType ? scope.SelResidenceType : ""),
                        "presnt_village": (scope.SelVillage ? scope.SelVillage : ""),
                        "presnt_country_cd": (scope.selcountry ? scope.selcountry : ""),
                        "presnt_state_cd": (scope.selState ? scope.selState : ""),
                        "presnt_district_cd": (scope.seldistrict ? scope.seldistrict : ""),
                        "presnt_ps_cd": (scope.selPS ? scope.selPS : ""),
                        "presnt_pincode": (scope.Pincode ? scope.Pincode : ""),
                        "present_other_country_cd": (scope.Pselcountry ? scope.Pselcountry : ""),
                        "present_other_address_line_1": (scope.AddressL1 ? scope.AddressL1 : ""),
                        "present_other_address_line_2": (scope.AddressL2 ? scope.AddressL2 : ""),
                        "present_other_address_line_3": (scope.AddressL3 ? scope.AddressL3 : ""),
                        "present_other_pincode": (scope.POPincode ? scope.POPincode : ""),
                        "is_permanant_addr_same": (scope.ResDets ? scope.ResDets : ""),
                        "permnt_house_no": (scope.PTdoorno ? scope.PTdoorno : ""),
                        "permnt_street_road_no": (scope.PTStreet ? scope.PTStreet : ""),
                        "permnt_locality": (scope.PTLocality ? scope.PTLocality : ""),
                        "permnt_landmark_milestone": (scope.PTLandMark ? scope.PTLandMark : ""),
                        "permnt_ward_colony": (scope.PTWard ? scope.PTWard : ""),
                        "permnt_area_mandal": (scope.PTMandal ? scope.PTMandal : ""),
                        "permnt_residence_type": (scope.PTSelResidenceType ? scope.PTSelResidenceType : ""),
                        "permnt_village": (scope.PTSelVillage ? scope.PTSelVillage :""),
                        "permnt_country_cd": (scope.PTselcountry ? scope.PTselcountry : ""),
                        "permnt_state_cd": (scope.PTselState ? scope.PTselState : ""),
                        "permnt_district_cd": (scope.PTseldistrict ? scope.PTseldistrict : ""),
                        "permnt_ps_cd": (scope.PTselPS ? scope.PTselPS : ""),
                        "permnt_pincode": (scope.PTPincode ? scope.PTPincode :""),
                        "id_proof_type_cd": (scope.selIDProof ? scope.selIDProof : ""),
                        "id_id_no": (scope.IDProof ? scope.IDProof : ""),
                            "id_issue_date": (scope.IDIssueDate ? moment(scope.IDIssueDate).format('DD/MM/YYYY') : ""),
                        "id_issue_place": (scope.IDIssuePlace ? scope.IDIssuePlace : ""),
                        "id_issue_authority": (scope.IDIssueAuth ? scope.IDIssueAuth : ""),
                        "id_issue_authority_other": (scope.IDIssueOAuth ? scope.IDIssueOAuth : ""),
                        "address_proof_type_cd": (scope.selAddrProof ? scope.selAddrProof : ""),
                        "addr_id_no": (scope.AddrProof ? scope.AddrProof : ""),
                            "addr_issue_date": (scope.AddrIssueDate ? moment(scope.AddrIssueDate).format('DD/MM/YYYY') : ""),
                        "addr_issue_place": (scope.AddrIssuePlace ? scope.AddrIssuePlace :""),
                        "addr_issue_authority": (scope.AddrIssueAuth ? scope.AddrIssueAuth : ""),
                        "addr_issue_authority_other": (scope.AddrIssueOAuth ? scope.AddrIssueOAuth : ""),
                            "challan_ref_no": "",
                            "bank_name": "",
                            "branch_code": "",
                            "payment_date": "",
                            "challan_amount": "",
                            "journal_number": "",
                            "Files": scope.filesuplTable

                        }    
                }

                home_services.POSTENCRYPTAPI("SaveCrimePetition", req, token, function (value) {
                    if (value.data.statuscode == "200") {
                        scope.preloader = false;
                      //  swal("Success", "Petition Registered Successfully. \n Petition ID : " + value.data.cctns_pet_id, "success");
                        swal({
                            title: "Success!",
                            text: "Petition Registered Successfully.\n Petition ID: " + value.data.cctns_pet_id,
                            icon: "success",
                            buttons: true,
                            dangerMode: false,
                        }).then((willDelete) => {
                            if (willDelete) {
                                window.location.reload();
                            }
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
                        swal("info", "Data Insertion Failed", "error");
                    }
                });
            }
        }

        function LoadMasters() {
            var req = { TYPE: "1" }
            home_services.POSTENCRYPTAPI("LoadMasters", req, token, function (value) {
                var data = value.data[0];
                console.log(data);
                if (data.status == "Success") {
                    scope.MasterData = data.MasterDataDetails;
                    scope.DistrictDD = $(scope.MasterData).filter(function (i, n) { return n.distCd});
                }
                else {
                    swal("info", "Error occured while loading masters data", "info");
                }

            });
        }

           scope.addRespondent = function () {
                   //Add the new item to the Array.
                   if (scope.RespDets == "Y") {
                       scope.RespPTdoorno = scope.Respdoorno;
                       scope.RespPTStreet = scope.RespStreet;
                       scope.RespPTLocality = scope.RespLocality;
                       scope.RespPTLandMark = scope.RespLandMark;
                       scope.RespPTWard = scope.RespWard;
                       scope.RespPTMandal = scope.RespMandal;
                       scope.RespPTSelResidenceType = scope.RespSelResidenceType;
                       scope.RespPTSelVillage = scope.RespSelVillage;
                       scope.RespPTselcountry = scope.Respselcountry;
                       scope.RespPTselState = scope.RespselState;
                       scope.RespPTseldistrict = scope.Respseldistrict;
                       scope.RespPTselPS = scope.RespselPS;
                       scope.RespPTPincode = scope.RespPincode;
                   };
                   var respondent = {
                       "Respondent_first_name": (scope.RespName ? scope.RespName : ""),
                       "Respondent_middle_name": (scope.RespMiddleName ? scope.RespMiddleName : ""),
                       "Respondent_last_name": (scope.RespLastName ? scope.RespLastName : ""),
                       "Respondent_date_of_birth": (scope.Respdob ? moment(scope.Respdob).format('DD/MM/YYYY') : ""),
                       "Respondent_age": (scope.Respage ? scope.Respage : ""),
                       "Respondent_relation_name": (scope.RespFHGName ? scope.RespFHGName : ""),
                       "Respondent_relation_type": (scope.RespRType ? scope.RespRType : ""),
                       "Respondent_occupation_cd": (scope.Respoccupation ? scope.Respoccupation : ""),
                       "Respondent_other_occupation_cd": (scope.RespOthOccupation ? scope.RespOthOccupation : ""),
                       "Respondent_gender_cd": (scope.Respselgender ? scope.Respselgender : ""),
                       "Respondent_caste_tribe_cd": (scope.Respcaste ? scope.Respcaste : ""),
                       "Respondent_sub_caste_cd": (scope.RespSubcaste ? scope.RespSubcaste : ""),
                       "Respondent_religion_cd": (scope.RespReligion ? scope.RespReligion : ""),
                       "Respondent_nationality_cd": (scope.RespNationality ? scope.RespNationality : ""),
                       "Respondent_edu_qual_cd": (scope.RespEducation1 ? scope.RespEducation1 :""),
                       "Respondent_oth_edu_quali": (scope.RespEducation2 ? scope.RespEducation2 : ""),
                       "Respondent_telephone_off": (scope.RespOMobileNo ? scope.RespOMobileNo : ""),
                       "Respondent_telephone_residence": (scope.RespAMobileNo ? scope.RespAMobileNo : ""),
                       "Respondent_mobile_number": (scope.RespMobileNo ? scope.RespMobileNo : ""),
                       "Respondent_fax_no": (scope.RespFaxno ? scope.RespFaxno : ""),
                       "Respondent_email_id": (scope.RespEmailId ? scope.RespEmailId : ""),
                       "Respondent_alternative_email": (scope.RespAEmailId ? scope.RespAEmailId : ""),
                       "Respondent_full_name": (scope.RespApFullName ? scope.RespApFullName : ""),
                       "Respondent_marital_status_cd": (scope.RespMaritalStatus ? scope.RespMaritalStatus : ""),
                       "Respondent_alias_name": (scope.RespApAliasName ? scope.RespApAliasName : ""),
                       "Respondent_presnt_house_no": (scope.Respdoorno ? scope.Respdoorno : ""),
                       "Respondent_presnt_street_road_no": (scope.RespStreet ? scope.RespStreet : ""),
                       "Respondent_presnt_locality": (scope.RespLocality ? scope.RespLocality : ""),
                       "Respondent_presnt_landmark_milestone": (scope.RespLandMark ? scope.RespLandMark : ""),
                       "Respondent_presnt_ward_colony": (scope.RespWard ? scope.RespWard : ""),
                       "Respondent_presnt_area_mandal": (scope.RespMandal ? scope.RespMandal : ""),
                       "Respondent_presnt_residence_type": (scope.RespSelResidenceType ? scope.RespSelResidenceType : ""),
                       "Respondent_presnt_village": (scope.RespSelVillage ? scope.RespSelVillage : ""),
                       "Respondent_presnt_country_cd": (scope.Respselcountry ? scope.Respselcountry : ""),
                       "Respondent_presnt_state_cd": (scope.RespselState ? scope.RespselState : ""),
                       "Respondent_presnt_district_cd": (scope.Respseldistrict ? scope.Respseldistrict : ""),
                       "Respondent_presnt_ps_cd": (scope.RespselPS ? scope.RespselPS : ""),
                       "Respondent_presnt_pincode": (scope.RespPincode ? scope.RespPincode : ""),
                       "Respondent_present_other_country_cd": (scope.RespPselcountry ? scope.RespPselcountry : ""),
                       "Respondent_present_other_address_line_1": (scope.RespAddressL1 ? scope.RespAddressL1 : ""),
                       "Respondent_present_other_address_line_2": (scope.RespAddressL2 ? scope.RespAddressL2 : ""),
                       "Respondent_present_other_address_line_3": (scope.RespAddressL3 ? scope.RespAddressL3 : ""),
                       "Respondent_present_other_pincode": (scope.RespPOPincode ? scope.RespPOPincode : ""),
                       "Respondent_is_permanant_addr_same": (scope.RespDets ? scope.RespDets : ""),
                       "Respondent_permnt_house_no": (scope.RespPTdoorno ? scope.RespPTdoorno : ""),
                       "Respondent_permnt_street_road_no": (scope.RespPTStreet ? scope.RespPTStreet : ""),
                       "Respondent_permnt_locality": (scope.RespPTLocality ? scope.RespPTLocality : ""),
                       "Respondent_permnt_landmark_milestone": (scope.RespPTLandMark ? scope.RespPTLandMark : ""),
                       "Respondent_permnt_ward_colony": (scope.RespPTWard ? scope.RespPTWard : ""),
                       "Respondent_permnt_area_mandal": (scope.RespPTMandal ? scope.RespPTMandal : ""),
                       "Respondent_permnt_residence_type": (scope.RespPTSelResidenceType ? scope.RespPTSelResidenceType : ""),
                       "Respondent_permnt_village": (scope.RespPTSelVillage ? scope.RespPTSelVillage : ""),
                       "Respondent_permnt_country_cd": (scope.RespPTselcountry ? scope.RespPTselcountry : ""),
                       "Respondent_permnt_state_cd": (scope.RespPTselState ? scope.RespPTselState : ""),
                       "Respondent_permnt_district_cd": (scope.RespPTseldistrict ? scope.RespPTseldistrict : ""),
                       "Respondent_permnt_ps_cd": (scope.RespPTselPS ? scope.RespPTselPS : ""),
                       "Respondent_permnt_pincode": (scope.RespPTPincode ? scope.RespPTPincode : ""),
                       "Respondent_id_proof_type_cd": (scope.RespselIDProof ? scope.RespselIDProof : ""),
                       "Respondent_id_id_no": (scope.RespIDProof ? scope.RespIDProof : ""),
                       "Respondent_id_issue_date": (scope.RespIDIssueDate ? moment(scope.RespIDIssueDate).format('DD/MM/YYYY') : ""),
                       "Respondent_id_issue_place": (scope.RespIDIssuePlace ? scope.RespIDIssuePlace : ""),
                       "Respondent_id_issue_authority": (scope.RespIDIssueAuth ? scope.RespIDIssueAuth : ""),
                       "Respondent_id_issue_authority_other": (scope.RespIDIssueOAuth ? scope.RespIDIssueOAuth : ""),
                       "Respondent_address_proof_type_cd": (scope.RespselAddrProof ? scope.RespselAddrProof : ""),
                       "Respondent_addr_id_no": (scope.RespAddrProof ? scope.RespAddrProof : ""), 
                       "Respondent_addr_issue_date": (scope.RespAddrIssueDate ? moment(scope.RespAddrIssueDate).format('DD/MM/YYYY') : ""),
                       "Respondent_addr_issue_place": (scope.RespAddrIssuePlace ? scope.RespAddrIssuePlace : ""),
                       "Respondent_addr_issue_authority": (scope.RespAddrIssueAuth ? scope.RespAddrIssueAuth : ""),
                       "Respondent_addr_issue_authority_other": (scope.RespAddrIssueOAuth ? scope.RespAddrIssueOAuth : ""),
                       "Respondent_pet_district_cd": (scope.RespPetidist ? scope.RespPetidist : ""),
                       "Respondent_pet_ps_cd": (scope.RespPetiPS ? scope.RespPetiPS : ""),
                   };
                   scope.RespName = "";
                   scope.RespMiddleName = "";
                   scope.RespLastName = "";
                   scope.Respdob = null;
                   scope.Respage = "";
                   scope.RespFHGName = "";
                   scope.RespRType = "";
                   scope.Respoccupation = "";
                   scope.RespOthOccupation = "";
                   scope.Respselgender = "";
                   scope.Respcaste = "";
                   scope.RespSubcaste = "";
                   scope.RespReligion = "";
                   scope.RespNationality = "";
                   scope.RespEducation1 = "";
                   scope.RespEducation2 = "";
                   scope.RespOMobileNo = "";
                   scope.RespAMobileNo = "";
                   scope.RespMobileNo = "";
                   scope.RespFaxno = "";
                   scope.RespEmailId = "";
                   scope.RespAEmailId = "";
                   scope.RespApFullName = "";
                   scope.RespMaritalStatus = "";
                   scope.RespApAliasName = "";
                   scope.Respdoorno = "";
                   scope.RespStreet = "";
                   scope.RespLocality = "";
                   scope.RespLandMark = "";
                   scope.RespWard = "";
                   scope.RespMandal = "";
                   scope.RespSelResidenceType = "";
                   scope.RespSelVillage = "";
                   scope.Respselcountry = "";
                   scope.RespselState = "";
                   scope.Respseldistrict = "";
                   scope.RespselPS = "";
                   scope.RespPincode = "";
                   scope.RespPselcountry = "";
                   scope.RespAddressL1 = "";
                   scope.RespAddressL2 = "";
                   scope.RespAddressL3 = "";
                   scope.RespPOPincode = "";
                   scope.RespDets = null;
                   scope.RespPTdoorno = "";
                   scope.RespPTStreet = "";
                   scope.RespPTLocality = "";
                   scope.RespPTLandMark = "";
                   scope.RespPTWard = "";
                   scope.RespPTMandal = "";
                   scope.RespPTSelResidenceType = "";
                   scope.RespPTSelVillage = "";
                   scope.RespPTselcountry = "";
                   scope.RespPTselState = "";
                   scope.RespPTseldistrict = "";
                   scope.RespPTselPS = "";
                   scope.RespPTPincode = "";
                   scope.RespselIDProof = "";
                   scope.RespIDProof = "";
                   scope.RespIDIssueDate = null;
                   scope.RespIDIssuePlace = "";
                   scope.RespIDIssueAuth = "";
                   scope.RespIDIssueOAuth = "";
                   scope.RespselAddrProof = "";
                   scope.RespAddrProof = "";
                   scope.RespAddrIssueDate = null;
                   scope.RespAddrIssuePlace = "";
                   scope.RespAddrIssueAuth = "";
                   scope.RespAddrIssueOAuth = "";
                   scope.RespPetidist = "";
                   scope.RespPetiPS = "";

                   scope.RespondentTable.push(respondent);
               }

           scope.addFiles = function () {
               if (Step5Validations()) {
               var filesupl = {
                   "File_Name": filename1,
                   "file_Desc": (scope.DocsDesc ? scope.DocsDesc : ""),
                   "file_type_cd": (scope.FileType ? scope.FileType : ""),
                   "file_SubtypeCd": (scope.FileSubTy ? scope.FileSubTy : ""),
                   "image_url": FileUAM
                   }
                   $('#Doc1Upl').val('');
                   scope.DocsDesc = "";
                   scope.FileType = "";
                   scope.FileSubTy = "";
                   filename1 = "";
                   FileUAM = "";

               scope.filesuplTable.push(filesupl);
               }
           }

           scope.LoadDist = function () {
               scope.PetDistrictDD = [];
               if (scope.PetState) {
                   scope.DistrictDD = $(HDDistricts).filter(function (i, n) { return n.State_cd === scope.PetState });
               }
               if (scope.selState) {
                   scope.DistrictDD1 = $(HDDistricts).filter(function (i, n) { return n.State_cd === scope.selState });
               }
               if (scope.PTselState) {
                   scope.DistrictDD2 = $(HDDistricts).filter(function (i, n) { return n.State_cd === scope.PTselState });
               }
               if (scope.RespselState) {
                   scope.DistrictDD3 = $(HDDistricts).filter(function (i, n) { return n.State_cd === scope.RespselState });
               }
               if (scope.RespPTselState) {
                   scope.DistrictDD4 = $(HDDistricts).filter(function (i, n) { return n.State_cd === scope.RespPTselState });
               }
           }

           scope.LoadPS = function () {
               scope.PoliceStationDD = [];
               if (scope.selPetdistrict) {
                   scope.PoliceStationDD = $(HDPoliceStations).filter(function (i, n) { return n.District_cd === scope.selPetdistrict });
               }
               if (scope.seldistrict) {
                   scope.PoliceStationDD1 = $(HDPoliceStations).filter(function (i, n) { return n.District_cd === scope.seldistrict });
               }
               if (scope.PTseldistrict) {
                   scope.PoliceStationDD2 = $(HDPoliceStations).filter(function (i, n) { return n.District_cd === scope.PTseldistrict });
               }
               if (scope.Respseldistrict) {
                   scope.PoliceStationDD3 = $(HDPoliceStations).filter(function (i, n) { return n.District_cd === scope.Respseldistrict });
               }
               if (scope.RespPTseldistrict) {
                   scope.PoliceStationDD4 = $(HDPoliceStations).filter(function (i, n) { return n.District_cd === scope.RespPTseldistrict });
               }
               if (scope.RespPetidist) {
                   scope.PoliStatList = $(HDPoliceStations).filter(function (i, n) { return n.District_cd === scope.RespPetidist });
               }
           }
        //load PS
        //scope.LoadPS = function () {
        //    scope.PoliceStationDD = [];
        //    if (scope.seldistrict.distCd) {
        //        scope.PoliceStationDD = $(scope.MasterData).filter(function (i, n) { return n.distCd === parseInt(scope.seldistrict.distCd) });
        //    }
        //   }

        scope.LoadSubCaste = function () {
               scope.SubCasteDD = [];
               if (scope.caste) {
                   scope.SubCasteDD = $(HDSubCaste).filter(function (i, n) { return n.Caste_tribe_cd === scope.caste });
               }
               if (scope.Respcaste) {
                   scope.SubCasteList = $(HDSubCaste).filter(function (i, n) { return n.Caste_tribe_cd === scope.Respcaste });
               }
           }
        
        scope.getAge = function () {
            var dob1 = new Date(scope.dob);
            var today = new Date();
            var age1 = Math.floor((today - dob1) / (365.25 * 24 * 60 * 60 * 1000));
            scope.age = age1;

            var dob2 = new Date(scope.Respdob);
            var today1 = new Date();
            var age2 = Math.floor((today1 - dob2) / (365.25 * 24 * 60 * 60 * 1000));
            scope.Respage = age2;
            
        }

        scope.checkyes = function (e) {
            scope.detstable = e;
           }
        scope.checkyesResp = function (e) {
               scope.Respdetstable = e;
           }
           scope.checkyesRD = function (e) {
               scope.RespondentKnown = e;
           }

        scope.steps = {
            step1: true,
            step2: false,
            step3: false,
            step4: false,
            step5: false
        };

        scope.btn_get_tab = function (tab_id) {
            //scope.step_position = tab_id;
        };

        scope.$watch('step_position', function (e) {
            if (e == 1) {
                scope.steps.step1 = true;
                scope.finalsave = false;
            }
            else if (e == 2) {
                scope.steps.step2 = true;
                scope.finalsave = false;
            }
            else if (e == 3) {
                scope.steps.step3 = true;
                scope.finalsave = false;
            }
            else if (e==4) {
                scope.steps.step4 = true;
                scope.finalsave = false;
            }
            else if (e == 5) {
                scope.steps.step5 = true;
                scope.finalsave = true;
            }
           });

           scope.Next1 = function () {
               if (Step1Validations()) {
               scope.step_position = 2;
               }
           }
           scope.Previous2 = function () {
               scope.step_position = 1;
           }
           scope.Next2 = function () {
               if (Step2Validations()) {
                   scope.step_position = 3;
               }
           }
           scope.Previous3 = function () {
               scope.step_position = 2;
           }
           scope.Next3 = function () {
              if (Step3Validations()) {
                   scope.step_position = 4;
              }
           }
           scope.Previous4 = function () {
               scope.step_position = 3;
           }
           scope.Next4 = function () {
           scope.step_position = 5;
           }
           scope.Previous5 = function () {
               scope.step_position = 4;
           }
       
           function Step1Validations(){
               if (!scope.PDate) {
                   swal('Info', "Please select Petition Date", 'info');
                   return false;
               }
               else if (!scope.PetState) {
                   swal('info', "Please Select Petition State", 'info');
                   return false;
               }
               else if (!scope.selPetdistrict) {
                   swal('info', "Please select Petition District", 'info');
                   return false;
               }
               else if (!scope.selPetPS) {
                   swal('info', "Please select Petition PS", 'info');
                   return false;
               }
               else if (!scope.Psubject) {
                   swal('info', "Please Enter Petition Subject", 'info');
                   return false;
               }
               else if (!scope.PDesc) {
                   swal('info', "Please Enter Petition Description", 'info');
                   return false;
               }
               else if (!scope.Name) {
                   swal('Info', "Please Enter Applicant First Name", 'info');
                   return false;
               }
               else if (!scope.selgender) {
                   swal('info', "Please Select Applicant Gender", 'info');
                   return false;
               }
               else if (!scope.FHGName) {
                   swal('Info', "Please Enter Relative Name", 'info');
                   return false;
               }
               else if (!scope.RType) {
                   swal('info', "Please Select Relation Type", 'info');
                   return false;
               }
               else if (!scope.dob) {
                   swal('info', "Please Enter Applicant Date Of Birth", 'info');
                   return false;
               }             
               else if (!scope.MobileNo) {
                   swal('info', "Please Enter Mobile No", 'info');
                   return false;
               }
               else if (scope.MobileNo.length < 10) {
                   swal('info', "Mobile Number Should be 10 Digits", 'info');
                   return false;
               }
               else if (!scope.ApFullName) {
                   swal('info', "Please Enter Applicant Full Name", 'info');
                   return false;
               }
               return true;
           }
           function Step2Validations() {
               if (!scope.doorno) {
                   swal('Info', "Please Enter Door Number", 'info');
                   return false;
               }
               else if (!scope.Ward) {
                   swal('info', "Please Enter Ward", 'info');
                   return false;
               }
               else if (!scope.Mandal) {
                   swal('info', "Please Enter Mandal", 'info');
                   return false;
               }
               else if (!scope.selcountry) {
                   swal('info', "Please Select Country", 'info');
                   return false;
               } 
               else if (!scope.selState) {
                   swal('info', "Please Select State", 'info');
                   return false;
               }
               else if (!scope.seldistrict) {
                   swal('info', "Please Select District", 'info');
                   return false;
               }
               else if (!scope.selPS) {
                   swal('info', "Please select PS", 'info');
                   return false;
               }
               else if (!scope.ResDets) {
                   swal('info', "Is Permanent Address Same as Present Address? ", 'info');
                   return false;
               }
               else if (scope.ResDets == "N" && !scope.PTdoorno) {
                   swal('info', "Please Enter Permanent Address Door No.", 'info');
                   return false;
               }
               else if (scope.PTdoorno && !scope.PTWard) {
                   swal('info', "Please select Permanent Address Ward", 'info');
                   return false;
               }
               else if (scope.PTdoorno && !scope.PTMandal) {
                   swal('info', "Please Enter Permanent Address Mandal", 'info');
                   return false;
               }
               else if (scope.PTdoorno && !scope.PTselcountry) {
                   swal('info', "Please Select Country", 'info');
                   return false;
               }
               else if (scope.PTdoorno && !scope.PTselState) {
                   swal('info', "Please Select State", 'info');
                   return false;
               }
               else if (scope.PTdoorno && !scope.PTseldistrict) {
                   swal('info', "Please Select District", 'info');
                   return false;
               }
               else if (scope.PTdoorno && !scope.PTselPS) {
                   swal('info', "Please select Police Station", 'info');
                   return false;
               }
               return true;
           }
           function Step3Validations() {
               if (scope.selIDProof && !scope.IDProof) {
                   swal('Info', "Please Enter ID Proof Number", 'info');
                   return false;
               }
               else if (scope.selAddrProof && !scope.AddrProof) {
                   swal('info', "Please Enter Address Proof Number", 'info');
                   return false;
               }
               return true;
           }
           function Step5Validations() {
              if (!$("#Doc1Upl").val()) {
                   swal("Info", "Please Select File ", "info");
                   return false;
               }
              
               return true;
           }

           //function RespondentValidation() {
           //    if (!scope.RespName) {
           //        swal('Info', "Please enter Respondent Name", 'info');
           //        return false;
           //    }
           //    else if (!scope.Respselgender) {
           //        swal('info', "Please Select Respondent Gender", 'info');
           //        return false;
           //    }
           //    else if (!scope.RespFHGName) {
           //        swal('Info', "Please Enter Respondent Relative Name", 'info');
           //        return false;
           //    }
           //    else if (!scope.RespRType) {
           //        swal('info', "Please Select Respondent Relation Type", 'info');
           //        return false;
           //    }
           //    else if (!scope.Respdob) {
           //        swal('info', "Please Enter Respondent Date Of Birth", 'info');
           //        return false;
           //    }
           //    else if (!scope.RespMobileNo) {
           //        swal('info', "Please Enter Respondent Mobile No", 'info');
           //        return false;
           //    }
           //    else if (scope.RespMobileNo.length < 10) {
           //        swal('info', "Mobile Number Should be 10 Digits", 'info');
           //        return false;
           //    }
           //    else if (!scope.RespApFullName) {
           //        swal('info', "Please Enter Respondent Full Name", 'info');
           //        return false;
           //    }
           //    else if (!scope.Respdoorno) {
           //        swal('Info', "Please Enter Respondent Present Address Door Number", 'info');
           //        return false;
           //    }
           //    else if (!scope.RespWard) {
           //        swal('info', "Please Enter Respondent Present Address Ward", 'info');
           //        return false;
           //    }
           //    else if (!scope.RespMandal) {
           //        swal('info', "Please Enter Respondent Present Address Mandal", 'info');
           //        return false;
           //    }
           //    else if (!scope.Respselcountry) {
           //        swal('info', "Please Select Respondent Present Address Country", 'info');
           //        return false;
           //    }
           //    else if (!scope.RespselState) {
           //        swal('info', "Please Select Respondent Present Address State", 'info');
           //        return false;
           //    }
           //    else if (!scope.Respseldistrict) {
           //        swal('info', "Please Select Respondent Present Address District", 'info');
           //        return false;
           //    }
           //    else if (!scope.RespselPS) {
           //        swal('info', "Please select Respondent Present Address PoliceStation", 'info');
           //        return false;
           //    }
           //    else if (!scope.RespDets) {
           //        swal('info', "Please Select Is Permanent Address Same? ", 'info');
           //        return false;
           //    }
           //    else if (scope.RespDets == "N" && !scope.RespPTdoorno) {
           //        swal('info', "Please Enter Permanent Address Door No.", 'info');
           //        return false;
           //    }
           //    else if (scope.RespPTdoorno && !scope.RespPTWard) {
           //        swal('info', "Please select Respondent Permanent Address Ward", 'info');
           //        return false;
           //    }
           //    else if (scope.RespPTdoorno && !scope.RespPTMandal) {
           //        swal('info', "Please Enter Respondent Permanent Address Mandal", 'info');
           //        return false;
           //    }
           //    else if (scope.RespPTdoorno && !scope.RespPTselcountry) {
           //        swal('info', "Please Select Respondent Permanent Address Country", 'info');
           //        return false;
           //    }
           //    else if (scope.RespPTdoorno && !scope.RespPTselState) {
           //        swal('info', "Please Select Respondent Permanent Address State", 'info');
           //        return false;
           //    }
           //    else if (scope.RespPTdoorno && !scope.RespPTseldistrict) {
           //        swal('info', "Please Select Respondent Permanent Address District", 'info');
           //        return false;
           //    }
           //    else if (scope.RespPTdoorno && !scope.RespPTselPS) {
           //        swal('info', "Please select Respondent Permanent Address PS", 'info');
           //        return false;
           //    }
           //    else if (scope.RespselIDProof && !scope.RespIDProof) {
           //        swal('Info', "Please Enter ID Proof Number", 'info');
           //        return false;
           //    }
           //    else if (scope.RespselAddrProof && !scope.RespAddrProof) {
           //        swal('info', "Please Enter Address Proof Number", 'info');
           //        return false;
           //    }
           //    else if (!scope.RespPetidist) {
           //        swal('info', "Please select Respondent Petition District", 'info');
           //        return false;
           //    }
           //    else if (!scope.RespPetiPS) {
           //        swal('info', "Please select Respondent Petition PS", 'info');
           //        return false;
           //    }
           //    //else if (scope.RespondentTable.length < 0) {
           //    //    swal('info', "Please Add Respondent Details", 'info');
           //    //    return false;
           //    //}  
           //    return true;
           //}
         
    }
})();


