(function () {
    var app = angular.module("GSWS");
	app.controller("LayoutPlanInformation", ["$scope", "$state", "PRRD_Services", '$sce', LayoutPlanInformationCall]);

    function LayoutPlanInformationCall(scope, state, PRRD_Services, sce) {

        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

        scope.detailsshow = false;

        scope.getdetails = function () {

            if (scope.ddlSearchby == undefined || scope.ddlSearchby == null || scope.ddlSearchby == "") {
                alert('Please enter Search by');
                return;
            }
            else if (scope.txrtNumber == undefined || scope.txrtNumber == null || scope.txrtNumber == "") {
                alert('Please enter Application ID.');
                return;
            }
            var card = scope.txrtNumber;
            if (scope.txrtNumber.length != 12 && scope.ddlSearchby == "1") {
                scope.preloader = false; alert('Please Enter 12 Digit Aadhaar Number.');
                return;
            }
            if ((card == "111111111111" || card == "222222222222" || card == "333333333333" || card == "444444444444" || card == "555555555555" || card == "666666666666"
                || card == "777777777777" || card == "888888888888" || card == "999999999999" || card == "000000000000") && scope.ddlSearchby == "1") {
                scope.preloader = false; alert("Please Enter 12 Digit Aadhaar Number");
                return;
            }
            if (scope.ddlSearchby == "1") {
                var status = validateVerhoeff(card);
                if (status) {

                }
                else {
                    scope.preloader = false; alert('Enter Valid Aadhaar Number');
                    return;
                }
            }
            var objdata = {
                search: scope.ddlSearchby,
                search_val: scope.txrtNumber,
            };
            PRRD_Services.POSTENCRYPTAPI("LayoutPlanApplicationInformation", objdata, token, function (value) {
                var res = value.data;

                if (res.Status == "Success") {

                    scope.Status = "Available";

                    if (res.data.details[0] != null && res.data.details.length > 0) {

                        scope.id = res.data.details[0].personal_data.id;
                        scope.surname = res.data.details[0].personal_data.surname;
                        scope.name = res.data.details[0].personal_data.name;
                        scope.relation = res.data.details[0].personal_data.relation;
                        scope.surname1 = res.data.details[0].personal_data.surname1;
                        scope.name1 = res.data.details[0].personal_data.name1;
                        scope.aadhar = res.data.details[0].personal_data.aadhar;
                        scope.email = res.data.details[0].personal_data.email;
                        scope.mobile = res.data.details[0].personal_data.mobile;
                        scope.address = res.data.details[0].personal_data.address;
                        scope.application_no = res.data.details[0].personal_data.application_no;
                        scope.timestamp = res.data.details[0].personal_data.timestamp;


                        scope.Locationid = res.data.details[0].location_data.id;
                        scope.Locationuid = res.data.details[0].location_data.uid;
                        scope.Locationextent = res.data.details[0].location_data.extent;
                        scope.Locationlayout_acres = res.data.details[0].location_data.layout_acres;
                        scope.Locationsurvey_no = res.data.details[0].location_data.survey_no;
                        scope.Locationlocation = res.data.details[0].location_data.location;
                        scope.Locationward_no = res.data.details[0].location_data.ward_no;
                        scope.Locationblock_no = res.data.details[0].location_data.block_no;
                        scope.Locationroad_width = res.data.details[0].location_data.road_width;
                        scope.Locationno_of_plots = res.data.details[0].location_data.no_of_plots;
                        scope.Locationarea_for_openspace_sq = res.data.details[0].location_data.area_for_openspace_sq;
                        scope.Locationarea_for_openspace = res.data.details[0].location_data.area_for_openspace;
                        scope.Locationarea_for_roads = res.data.details[0].location_data.area_for_roads;
                        scope.Locationplotarea_percentage_sq = res.data.details[0].location_data.plotarea_percentage_sq;
                        scope.Locationplotarea_percentage = res.data.details[0].location_data.plotarea_percentage;
                        scope.Locationarea_for_amenties_sq = res.data.details[0].location_data.area_for_amenties_sq;
                        scope.Locationarea_for_amenties = res.data.details[0].location_data.area_for_amenties;
                        scope.Locationlayout_type = res.data.details[0].location_data.layout_type;
                        scope.Locationcourt_case = res.data.details[0].location_data.court_case;
                        scope.Locationcourt_case_remarks = res.data.details[0].location_data.court_case_remarks;
                        scope.Locationapplication_no = res.data.details[0].location_data.application_no;
                        scope.Locationstage = res.data.details[0].location_data.stage;
                        scope.Locationstatus = res.data.details[0].location_data.status;
                        scope.Locationpayment_status = res.data.details[0].location_data.payment_status;
                        scope.Locationtimestamp = res.data.details[0].location_data.timestamp;

                        scope.ownerdata = res.data.details[0].owner_data;


                        scope.Landid = res.data.details[0].land_data.id;
                        scope.Landuid = res.data.details[0].land_data.uid;
                        scope.Landsitearea = res.data.details[0].land_data.sitearea;
                        scope.Landencumbrance_no = res.data.details[0].land_data.encumbrance_no;
                        scope.Landproceeding_date = res.data.details[0].land_data.proceeding_date;
                        scope.Landother_remarks = res.data.details[0].land_data.other_remarks;
                        scope.Landtimestamp = res.data.details[0].land_data.timestamp;
                        scope.Landproceeding_no = res.data.details[0].land_data.proceeding_no;
                        scope.Landencumbrance_date = res.data.details[0].land_data.encumbrance_date;


                        scope.topographicalid = res.data.details[0].topographical_data.id;
                        scope.topographicaluid = res.data.details[0].topographical_data.uid;
                        scope.topographicalsite_abutting = res.data.details[0].topographical_data.site_abutting;
                        scope.topographicalsite_abutting_desc = res.data.details[0].topographical_data.site_abutting_desc;
                        scope.topographicalnoc = res.data.details[0].topographical_data.noc;
                        scope.topographicalnoc_desc = res.data.details[0].topographical_data.noc_desc;
                        scope.topographicalsite_line = res.data.details[0].topographical_data.site_line;
                        scope.topographicalsite_line_desc = res.data.details[0].topographical_data.site_line_desc;
                        scope.topographicalfield_numbers = res.data.details[0].topographical_data.field_numbers;
                        scope.topographicalsite_acquited = res.data.details[0].topographical_data.site_acquited;
                        scope.topographicalsite_acquired_desc = res.data.details[0].topographical_data.site_acquired_desc;
                        scope.topographicalsite_reference = res.data.details[0].topographical_data.site_reference;
                        scope.topographicalsite_reference_desc = res.data.details[0].topographical_data.site_reference_desc;
                        scope.topographicaltimestamp = res.data.details[0].topographical_data.timestamp;

                        scope.doc_details = res.data.details[0].doc_details;


                        scope.certificatesid = res.data.details[0].required_certificates.id;
                        scope.certificatesuid = res.data.details[0].required_certificates.uid;
                        scope.certificatescert1 = res.data.details[0].required_certificates.cert1;
                        scope.certificatesremarks1 = res.data.details[0].required_certificates.remarks1;
                        scope.certificatescert2 = res.data.details[0].required_certificates.cert2;
                        scope.certificatesremarks2 = res.data.details[0].required_certificates.cert3;
                        scope.certificatescert3 = res.data.details[0].required_certificates.site_line;
                        scope.certificatesremarks3 = res.data.details[0].required_certificates.remarks3;
                        scope.certificatescert4 = res.data.details[0].required_certificates.cert4;
                        scope.certificatesremarks4 = res.data.details[0].required_certificates.remarks4;
                        scope.certificatescert5 = res.data.details[0].required_certificates.cert5;
                        scope.certificatesremarks5 = res.data.details[0].required_certificates.remarks5;
                        scope.certificatescert6 = res.data.details[0].required_certificates.cert6;
                        scope.certificatesremarks6 = res.data.details[0].required_certificates.remarks6;
                        scope.certificatescert7 = res.data.details[0].required_certificates.cert7;
                        scope.certificatesremarks7 = res.data.details[0].required_certificates.remarks7;
                        scope.certificatescert8 = res.data.details[0].required_certificates.cert8;
                        scope.certificatesremarks8 = res.data.details[0].required_certificates.remarks8;
                        scope.certificatescert9 = res.data.details[0].required_certificates.cert9;
                        scope.certificatesremarks9 = res.data.details[0].required_certificates.remarks9;
                        scope.certificatescert10 = res.data.details[0].required_certificates.cert10;
                        scope.certificatesremarks10 = res.data.details[0].required_certificates.remarks10;
                        scope.certificatescert11 = res.data.details[0].required_certificates.cert11;
                        scope.certificatesremarks11 = res.data.details[0].required_certificates.remarks11;
                        scope.certificatescert12 = res.data.details[0].required_certificates.cert12;
                        scope.certificatesremarks12 = res.data.details[0].required_certificates.remarks12;
                        scope.certificatestimestamp = res.data.details[0].required_certificates.timestamp;


                        scope.Paymentid = res.data.details[0].payment_data.id;
                        scope.Paymentuid = res.data.details[0].payment_data.uid;
                        scope.Paymentlayout_fee = res.data.details[0].payment_data.layout_fee;
                        scope.Paymentinspection_fee = res.data.details[0].payment_data.inspection_fee;
                        scope.Paymentsecurity_deposit = res.data.details[0].payment_data.security_deposit;
                        scope.Paymenttotal = res.data.details[0].payment_data.total;
                        scope.Paymenttimestamp = res.data.details[0].payment_data.timestamp;

                        scope.detailsshow = true;
                    }
                    else { scope.detailsshow = false; alert('Invalid Application/Aadhar Number'); }
                }
                else if (res.Status == "428") {
                    sessionStorage.clear();
                    swal("info", "Session Expired !!!", "info");
                    state.go("Login");
                    return;
                }

                else {
                    scope.Status = "Not Available";
                    scope.detailsshow = false;
                    alert('No Data Found');
                }

            });
        };
    }
})();