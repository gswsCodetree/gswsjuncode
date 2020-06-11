(function () {
	var app = angular.module("GSWS");

	app.controller("PensionApplication", ["$scope", "$state", "$log", "SERP_Services", PensionApplication_CTRL]);



	function PensionApplication_CTRL(scope, state, log, SERP_Services) {

		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}
		scope.aadharvalidation = true;
		scope.pensiondesable = false;
		scope.preloader = false;
		scope.FiveStepDD = [];
		scope.IsOtherPension = "no";
		scope.PensionToken = "";

		scope.ValidateUID = function () {

			scope.preloader = true;

			if (!scope.txtValidateUID) {
				scope.preloader = false; swal("info", 'Please enter Aadhar Number.', "info");
				return;
			}

			var card = scope.txtValidateUID;
			if (scope.txtValidateUID.length != 12) {
				scope.preloader = false; swal("info", 'Please Enter 12 Digit Aadhaar Number.', "info");
				return;
			}
			if (card == "111111111111" || card == "222222222222" || card == "333333333333" || card == "444444444444" || card == "555555555555" || card == "666666666666"
				|| card == "777777777777" || card == "888888888888" || card == "999999999999" || card == "000000000000") {
				scope.preloader = false; swal("info", "Please Enter 12 Digit Aadhaar Number", "info");
				return;
			}
			var status = validateVerhoeff(card);
			if (!status) {
				scope.preloader = false; swal("info", 'Enter Valid Aadhaar Number', "info");
				return;
			}

			var obj = { userName: sessionStorage.getItem("secccode") }
			SERP_Services.POSTENCRYPTAPI("ValidatePensionLogin", obj, token, function (value) {

				//scope.preloader = false;

				var res = value.data;


				if (res.Status == "Success") {
					if (res.data.response == "Logged In successfully") {
						scope.Status = "Available";

						scope.pensionTypeList = res.data.pensionTypeList;
						scope.panchayatList = res.data.panchayat;
						scope.distcode = res.data.distcode;
						scope.distname = res.data.distname;
						scope.mndcode = res.data.mndcode;
						scope.mndname = res.data.mndname;
						scope.secretariatName = res.data.secretariatName;
						scope.secretariatCode = res.data.secretariatCode;
						scope.pancode = res.data.pancode;
						//scope.panname = res.data.panname;
						scope.villagecode = "null";
						scope.vilalgename = "null";
						//scope.habcode = res.data.habcode;
						//scope.habname = res.data.habname;
						scope.volunteerName = res.data.volunteerName;
						scope.PensionToken = res.data.token;

						var objdata =
						{

							"aadhaarNumber": scope.txtValidateUID,
							"flag": "aadhar",
							"rationCardNumber": "",
							"sadaremId": " ",
							"pensionType": "",
							"loginId": sessionStorage.getItem("secccode"),
							"token": scope.PensionToken
						}

						SERP_Services.POSTENCRYPTAPI("ValidatePensionUID", objdata, token, function (value) {
							scope.FiveStepDD = [];
							var res = value.data;
							scope.preloader = false;
							if (res.Status == "Success") {
								var resdata = res.data.aadhaar[0];
								if (!resdata.response) {
									var aadharValidations = resdata.aadharValidations;

									scope.txtUID = scope.txtValidateUID;

									scope.aadhaarRationCardNumber = resdata.aadhaarRationCardNumber;
									scope.uidname = resdata.firstName + " " + resdata.middleName;
									scope.relationName = resdata.relationName;
									scope.caste = resdata.caste;
									scope.subCaste = resdata.subCaste;
									scope.houseHoldId = resdata.houseHoldId;
									scope.aadhaarDistrictId = resdata.aadhaarDistrictId;
									scope.aadhaarDistrictName = resdata.aadhaarDistrictName;
									scope.firstName = resdata.firstName;
									scope.middleName = resdata.middleName;
									scope.age = resdata.age;
									scope.dateOfBirth = resdata.dateOfBirth;
									scope.photo = "data:image/jpeg;base64," + resdata.photo;
									scope.gender = resdata.gender;

									//Fill the Applicant details

									scope.txtFName = scope.firstName;
									scope.txtMName = scope.middleName;
									scope.txtFHName = scope.relationName;
									scope.txtDOB = scope.dateOfBirth;

									scope.detailsuidshow = true;

									scope.aadharvalidation = false;

									if (aadharValidations.length > 6) {
										scope.aadharValidationsStatus = aadharValidations[0].replace("{", "").replace("}", "").replace("response=", "");

										$.each(aadharValidations, function (key, value) {
											if (key != 0) {
												var sobj = value.replace("{", "").replace("}", "").split(",");
												var sixobj1 = {};
												sixobj1.category = sobj[1].replace("category=", "").trim();
												sixobj1.status = sobj[2].replace("status=", "").trim();
												sixobj1.eligibility = sobj[3].replace("eligibility=", "").trim();
												sixobj1.givenValue = sobj[4].replace("givenValue=", "").trim();

												scope.FiveStepDD.push(sixobj1);
											}
										});
									}
								}
								else { scope.preloader = false; swal("info", res.data.aadhaar[0].response, "info"); scope.detailsuidshow = false; }
							}
							else if (value.data.Status == "428") {
								swal('info', value.data.Reason, 'info');
								sessionStorage.clear();
								state.go("Login");
								return;
							}
							else { scope.detailsuidshow = false; }
						});

					}
					else { scope.preloader = false; swal("info", 'Invalid Credientials', "info"); }
				}
				else {
					scope.Status = "Not Available";
					//scope.resdata = "";
					//scope.detailsshow = true;
					scope.preloader = false;
					swal("info", 'No Data Found', "info");
				}

			});
		}

		scope.ValidatePensionType = function () {
			scope.preloader = true;
			if (!scope.txtUID) {
				scope.preloader = false; swal("info", 'Please Enter UID Number.', "info");
				return;
			}
			else if (!scope.ddlType) {
				scope.preloader = false; swal("info", 'Please Select Type.', "info");
				return;
			}
			else if (!scope.txtNumber) {
				scope.preloader = false; swal("info", 'Please enter Ration Card Number.', "info");
				return;
			}
			var objdata =
			{
				"aadhaarNumber": scope.txtUID,
				"flag": "rationcard",
				"rationCardNumber": scope.txtNumber,
				"aadhaarRationCardNumber": scope.aadhaarRationCardNumber,
				"sadaremId": "",
				"pensionType": scope.ddlType,
				"loginId": sessionStorage.getItem("secccode"),
				"token": scope.PensionToken,
				"age": scope.age,
				"gender": scope.gender,
				"aadhaarDistrictId": scope.aadhaarDistrictId
			}

			SERP_Services.POSTENCRYPTAPI("ValidatePensionRation", objdata, token, function (value) {
				scope.preloader = false;
				var res = value.data;
				scope.sadaremSelection = "0";
				if (res.Status == "Success") {
					if (!res.data.response) {
						if (res.data.rationcardList != null) {
							scope.responsedata = res.data.rationcardList;
							//scope.txtFName = res.data.firstName;
							//scope.txtMName = res.data.middleName;
							//scope.txtFHName = res.data.relationName;
							scope.txtGender = res.data.gender;
							//scope.txtDOB = res.data.dateOfBirth;
							scope.txtAge = res.data.age;
							scope.txtMandal = res.data.s_mandalName;

							//scope.txtward = res.data.s_panchayatName;
							//scope.txtHabitation = res.data.s_habitationName;

							scope.uiddatashow = true;

							scope.pensiondesable = true;

							//if (scope.ddlType == "3") { scope.sadaramentry = true; scope.aadharentry = false; }
							//else { scope.aadharentry = true; scope.sadaramentry = false; }
						}
						else { scope.preloader = false; swal("info", "Enter Corect Ration Number Or Ration Not Mapped With Aadhhar", "info"); }
					}
					else { scope.preloader = false; swal("info", res.data.response, "info"); }
				}
				else if (value.data.Status == "428") {
					swal('info', value.data.Reason, 'info');
					sessionStorage.clear();
					state.go("Login");
					return;
				}
				else { scope.uiddatashow = false; }
			});

		}

		scope.ValidateSadaram = function () {

			scope.preloader = true;
			if (!scope.txtSadaram) {
				scope.preloader = false; swal("info", 'Please entervalid Sadaram Number.', "info");
				return;
			}

			var objdata =
			{
				"aadhaarNumber": "515772110629",
				"flag": "sadaremID",
				"rationCardNumber": scope.txtNumber,
				"sadaremId": scope.txtSadaram,
				"pensionType": scope.ddlType,
				"loginId": sessionStorage.getItem("secccode"),
				"token": scope.PensionToken
			}


			SERP_Services.POSTENCRYPTAPI("ValidateSadaram", objdata, token, function (value) {

				var res = value.data;
				scope.preloader = false;
				if (res.Status == "Success") {
					if (res.data.response == null || res.data.response == "") {
						if (res.data.aadhaarNumber != null && res.data.aadhaarNumber != "") {
							scope.sadaremUid = res.data.sadaremUid;
							scope.disabilityType = res.data.disabilityType;
							scope.percentage = res.data.percentage;
							scope.sadaremName = res.data.sadaremName;
							scope.disabilityStatus = res.data.disabilityStatus;
							scope.sadaremRationCard = res.data.sadaremRationCard;
							scope.sadaremSelection = "1";
							//scope.aadharentry = true;
							scope.sadaramaadharentry = true;
							scope.sadaramdetails = true;
							scope.sadaramentrydisabled = true;
						}
						else { scope.aadharentry = false; }
					}
					else { scope.preloader = false; swal("info", res.data.response, "info"); }
				}
				else if (value.data.Status == "428") {
					swal('info', value.data.Reason, 'info');
					sessionStorage.clear();
					state.go("Login");
					return;
				}
				else { scope.preloader = false; swal("info", res.Status, "info"); }
			});
		}

		scope.IsAadharexists = function (s) {
			scope.IsAadharRation = s;
			if (scope.IsAadharRation == "Yes" && scope.ddlType == "3") {
				scope.sadaramentry = true;

			}
			else if (scope.IsAadharRation == "Yes" && scope.ddlType != "3") {
				scope.aadharentry = true;
				scope.sadaramentry = false;

			}
			else {
				scope.aadharentry = false;
				scope.sadaramentry = false;
			}
		};

		scope.IsSaderemAadharexists = function (s) {
			scope.IsAadharRation = s;
			if (scope.IsSadaremAadhar == "Yes" && scope.ddlType == "3") {
				scope.aadharentry = true;
			}
			else {
				scope.aadharentry = false;
			}
		};


		scope.savedata = function () {
			scope.preloader = true;

			if (!scope.txtFName) {
				scope.preloader = false; swal("info", 'Please enter first name.', "info");
				return;
			}
			else if (!scope.txtMName) {
				scope.preloader = false; swal("info", 'Please enter Middle name.', "info");
				return;
			}
			else if (!scope.txtFHName) {
				scope.preloader = false; swal("info", 'Please enter Father / Husband Name.', "info");
				return;
			}
			else if (!scope.txtGender) {
				scope.preloader = false; swal("info", 'Please enter  Gender.', "info", "info");
				return;
			}
			else if (!scope.txtDOB) {
				scope.preloader = false; swal("info", 'Please enter  DOB.', "info", "info");
				return;
			}
			else if (!scope.txtAge) {
				scope.preloader = false; swal("info", 'Please enter  Age.', "info", "info");
				return;
			}
			else if (!scope.ddlCaste) {
				scope.preloader = false; swal("info", 'Please Select Caste.', "info", "info");
				return;
			}
			else if (!scope.txtMandal) {
				scope.preloader = false; swal("info", 'Please enter mandal.', "info");
				return;
			}
			else if (!scope.SelPanchayat) {
				scope.preloader = false; swal("info", 'Please Select Panchayath.', "info");
				return;
			}
			else if (!scope.SelHabitation) {
				scope.preloader = false; swal("info", 'Please Select Habitation.', "info");
				return;
			}
			else if (!scope.txtStreet) {
				scope.preloader = false; swal("info", 'Please enter street.', "info");
				return;
			}
			else if (!scope.txtHouseNo) {
				scope.preloader = false; swal("info", 'Please enter House no.', "info");
				return;
			}
			else if (!scope.txtPHNo) {
				scope.preloader = false; swal("info", 'Please enter Phnoe no.', "info");
				return;
			}
			else if (!scope.txtAppMonth) {
				scope.preloader = false; swal("info", 'Please enter Application Month.', "info");
				return;
			}
			else if (!base64) {
				scope.preloader = false; swal("info", 'Please upload certificate.', "info");
				return;
			}

			//else if (file.match(/\.([^\.]+)$/) == undefined || file.match(/\.([^\.]+)$/) == null) {
			//    swal("info",'Please Select Certificate.',"info");
			//    return;
			//}
			else if (!scope.txtIncome) {
				scope.preloader = false; swal("info", 'Please Enter Family Income.', "info");
				return;
			}
			else if (!scope.txtRemarks) {
				scope.preloader = false; swal("info", 'Please Enter Remarks.', "info");
				return;
			}
			else if (scope.IsOtherPension == "yes" && !scope.txtOherPName) {
				scope.preloader = false; swal("info", 'Please Enter Other Pension Name.', "info");
				return;
			}
			else if (!scope.txtVolID) {
				scope.preloader = false; swal("info", 'Please Enter Volunteer ID.', "info");
				return;
			}
			else if (!scope.txtVolName) {
				scope.preloader = false; swal("info", 'Please Enter Volunteer Name.', "info");
				return;
			}
			else if (!scope.txtVolMobile) {
				scope.preloader = false; swal("info", 'Please Enter Volunteer Mobile.', "info");
				return;
			}
			else if (scope.txtVolMobile.lenth < 10) {
				scope.preloader = false; swal("info", 'Volunteer Mobile Number Should be 10 Digits.', "info");
				return;
			}


			var strland = scope.FiveStepDD[5].givenValue.split(" and ");
			var wetland = strland.length > 0 ? strland[0].replace("Wet land :", "").trim() : "";
			var dryland = strland.length > 1 ? strland[1].replace("dry land :", "").trim() : "";

			var objdata =
			{
				"aadhaarNumber": (scope.txtUID ? scope.txtUID : ""),
				"rationCardNumber": (scope.txtNumber ? scope.txtNumber : ""),
				"aadhaarRationCardNumber": (scope.aadhaarRationCardNumber ? scope.aadhaarRationCardNumber : ""),
				"dateOfBirth": (scope.dateOfBirth ? scope.dateOfBirth : ""),
				"age": (scope.age ? scope.age : ""),
				"firstName": (scope.firstName ? scope.firstName : ""),
				"middleName": (scope.middleName ? scope.middleName : ""),
				"relationName": (scope.relationName ? scope.relationName : ""),
				"pensionType": (scope.ddlType ? scope.ddlType : ""),
				"gender": (scope.gender ? scope.gender : ""),
				"houseHoldId": (scope.houseHoldId ? scope.houseHoldId : ""),
				"webland": "101",
				"s_districtId": (scope.distcode ? scope.distcode : ""),
				"s_districtName": (scope.distname ? scope.distname : ""),
				"s_mandalId": (scope.mndcode ? scope.mndcode : ""),
				"s_mandalName": (scope.mndname ? scope.mndname : ""),
				"s_panchayatId": (scope.SelPanchayat.pancode ? scope.SelPanchayat.pancode : ""),
				"s_panchayatName": (scope.SelPanchayat.panname ? scope.SelPanchayat.panname : ""),
				"s_villageId": (scope.villagecode ? scope.villagecode : ""),
				"s_villageName": (scope.vilalgename ? scope.vilalgename : ""),
				"s_habitationId": (scope.SelHabitation.habcode ? scope.SelHabitation.habcode : ""),
				"s_habitationName": (scope.SelHabitation.habname ? scope.SelHabitation.habname : ""),
				"sadaremId": (scope.txtSadaram ? scope.txtSadaram : ""),
				"disabilityType": (scope.disabilityType ? scope.disabilityType : ""),
				"percentage": (scope.percentage ? scope.percentage : ""),
				"disabilityStatus": (scope.disabilityStatus ? scope.disabilityStatus : ""),
				"fileSize": (docFileSize ? docFileSize : ""),
				"imeiNumber": "",
				"appliedMonth": moment(scope.txtAppMonth).format('DD/MM/YYYY'),
				"hNo": (scope.txtHouseNo ? scope.txtHouseNo : ""),
				"i_caste": (scope.ddlCaste ? scope.ddlCaste : ""),
				"sadaremSelection": (scope.sadaremSelection ? scope.sadaremSelection : ""),
				"street": (scope.txtStreet ? scope.txtStreet : ""),
				"loginId": sessionStorage.getItem("secccode"),
				"token": (scope.PensionToken ? scope.PensionToken : ""),
				"remarks": (scope.txtRemarks ? scope.txtRemarks : ""),
				"rationSelection": "0",
				"phoneNumber": (scope.txtPHNo ? scope.txtPHNo : ""),
				"caste": (scope.ddlCaste ? scope.ddlCaste : ""),
				"subCaste": (scope.ddlCaste ? scope.ddlCaste : ""),
				"uploadDocument": base64.replace("data:image/jpeg;base64,", "").replace("data:application/pdf;base64,", ""),
				"fourwheeler": scope.FiveStepDD[0].givenValue,
				"incometax": scope.FiveStepDD[1].givenValue,
				"electricity": scope.FiveStepDD[2].givenValue,
				"govtemp": scope.FiveStepDD[3].givenValue,
				"propertyTax": scope.FiveStepDD[4].givenValue,
				"wetland": (wetland ? wetland : ""),
				"dryland": (dryland ? dryland : ""),
				"income": (scope.txtIncome ? scope.txtIncome : ""),
				"otherpensions": (scope.IsOtherPension ? scope.IsOtherPension : ""),
				"otherpensionsRemarks": (scope.txtOherPName ? scope.txtOherPName : ""),
				"base64incomedocument": "",
				"base64photo": "",
				"eligibleInEligibleflag": (scope.aadharValidationsStatus == "ELIGIBLE" ? "Eligible" : "InEligible"),
				"volunteerId": (scope.txtVolID ? scope.txtVolID : ""),
				"volunteerName": (scope.txtVolName ? scope.txtVolName : ""),
				"volunteerMobileNumber": (scope.txtVolMobile ? scope.txtVolMobile : ""),
				"gsws_Id": sessionStorage.getItem("TransID")
			}



			SERP_Services.POSTENCRYPTAPI("savePensiondata", objdata, token, function (value) {
				scope.preloader = false;
				var res = value.data;
				if (res.Status == "Success") {
					if (res.data.success) {
						swal("info", res.data.success, "info")
						window.location.reload();
					}
					else { scope.preloader = false; swal("info", res.data.ErrorMsg, "info") }
				}
				else if (value.data.Status == "428") {
					swal('info', value.data.Reason, 'info');
					sessionStorage.clear();
					state.go("Login");
					return;
				}
				else { swal('info', value.data.data + " " + value.data.Reason, 'info'); }
			});

		}

		// Be aware! We are handling only the first <input type="file" /> element
		// To avoid errors, it should be placed before this piece of code
		var input = document.querySelector('input[type=file]');
		var base64 = "";
		var docFileSize = "";
		// You will receive the Base64 value every time a user selects a file from his device
		// As an example I selected a one-pixel red dot GIF file from my computer
		input.onchange = function () {

			var file = input.files[0];
			var ext = file.name.split(".").pop().toLowerCase();
			if ($.inArray(ext, ["JPEG", "jpg", "png", "PNG", "PDF", "pdf"]) == -1) {
				base64 = ""; document.getElementById("txtUploadCert").value = "";
				docFileSize = ""; scope.preloader = false;
				swal("info", "Please Upload only jpg,png or pdf Files", "info");
				return;
			}

			else {
				docFileSize = file.size;
				reader = new FileReader();
				reader.onloadend = function () {
					// Since it contains the Data URI, we should remove the prefix and keep only Base64 string
					base64 = reader.result;
				};
				reader.readAsDataURL(file);
			}

		};

	}

	app.directive('numbersOnly', function () {
		return {
			require: 'ngModel',
			link: function (scope, element, attr, ngModelCtrl) {
				function fromUser(text) {
					if (text) {
						var transformedInput = text.replace(/[^0-9]/g, '');

						if (transformedInput !== text) {
							ngModelCtrl.$setViewValue(transformedInput);
							ngModelCtrl.$render();
						}
						return transformedInput;
					}
					return undefined;
				}
				ngModelCtrl.$parsers.push(fromUser);
			}
		};
	});
})();

