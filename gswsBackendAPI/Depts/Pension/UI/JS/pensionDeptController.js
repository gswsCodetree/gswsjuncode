(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("pensionDeptController", ["$scope", "pensionDeptServices", pensionDept_CTRL]);


	function pensionDept_CTRL(scope, ps) {

		scope.token = sessionStorage.getItem("Token");
		scope.personDetails = '';
		scope.encrypted_data = getParameterByName('Id');
		scope.iv = getParameterByName('IV');

		if (scope.encrypted_data != "" && scope.encrypted_data != null && scope.encrypted_data != undefined || scope.iv != "" && scope.iv != null && scope.iv != undefined) {
			console.log(scope.encrypted_data, scope.iv);
		}
		else {
			alert("Invalid Request !!!");
			window.open("https://gramawardsachivalayam.ap.gov.in/GSWS/Home/Main", "_Self");
			return;
		}

		scope.getAge = function (birthDate) {
			var today = new Date();
			var age = today.getFullYear() - birthDate.getFullYear();
			var m = today.getMonth() - birthDate.getMonth();
			if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
				age = age - 1;
			}

			return age;
		};


		scope.dobChange = function () {
			scope.age = scope.getAge(scope.dob);
		};

		scope.btnValidatepersonDetails = function () {
			if (scope.uid == undefined || scope.uid == '' || scope.uid == null) {
				alert('Please Enter Aadhaar Number ');
				return;
			}
			if (!validateVerhoeff(scope.uid)) {
				alert('Please Enter Valid Aadhaar Number ');
				return;
			}
			if (scope.category == undefined || scope.category == '' || scope.category == null) {
				alert('Please Select Category');
				return;
			}
			if (scope.category == "3") {
				if (scope.saradem == undefined || scope.saradem == '' || scope.saradem == null) {
					alert('Please Enter SADARAM ID');
					return;
				}
			}

			if (scope.category == '4') {
				scope.certificateType = 'Death certificate of Husband.';
			} else if (scope.category == '2') {
				scope.certificateType = 'Society issued certificate';
			} else if (scope.category == '5') {
				scope.certificateType = 'Society issued certificate';
			} else if (scope.category == '9') {
				scope.certificateType = 'Medical certificate';
			} else if (scope.category == '11') {
				scope.certificateType = 'Thasildar issued certificate';
			} else if (scope.category == '10') {
				scope.certificateType = 'Society issued marine fishermen certificate';
			}

			scope.loader = true;
			var requestData = {
				secId: sessionStorage.getItem("secccode"),
				uid: scope.uid,
				pensionType: scope.category,
				sadaremId: scope.saradem
			};
			ps.encrypt_post("personDetails", requestData, scope.token, function (data) {
				var res = data.data;
				if (res.status) {
					scope.personDetails = res.result;
					scope.PanchayatList = res.PanchayatList;
					scope.casteList = res.casteList;
					var dateParts = scope.personDetails.details.DOB_DT.substring(0, 10).split('-');
					scope.dob = new Date(dateParts[2], dateParts[1] - 1, dateParts[0]);
					scope.age = scope.getAge(scope.dob);
					scope.gender = scope.personDetails.details.GENDER;
				}
				else {
					alert(res.result);
				}
				scope.loader = false;
			}, function (error) {
				scope.loader = false;
				console.log(error);
			});
		};


		//scope.category = "1";
		//scope.uid = "645736782830";
		//scope.saradem = "";
		//scope.btnValidatepersonDetails();



		scope.gpChange = function () {
			scope.HabitationList = '';
			if (scope.gpId == '') {
				return;
			}

			scope.loader = true;
			var requestData = {
				secretariatId: sessionStorage.getItem("secccode"),
				panchayatId: scope.gpId
			};
			ps.encrypt_post("habitationList", requestData, scope.token, function (data) {
				var res = data.data;
				if (res.status) {
					scope.HabitationList = res.result;
				}
				else {
					alert(res.result);
				}
				scope.loader = false;
			}, function (error) {
				scope.loader = false;
				console.log(error);
			});
		};

		scope.casteChange = function () {
			scope.subCasteList = [];
			if (scope.caste == "") {
				return;
			}
			scope.loader = true;
			var requestData = {
				casteId: scope.caste
			};
			ps.encrypt_post("subCasteList", requestData, scope.token, function (data) {
				var res = data.data;
				if (res.status) {
					scope.subCasteList = res.result;
				}
				else {
					alert(res.result);
				}
				scope.loader = false;
			}, function (error) {
				scope.loader = false;
				console.log(error);
			});
		};

		var ageCertificate = "";
		scope.onLoad_ageCertificate = function (e, reader, file, fileList, fileOjects, fileObj) {
			if (fileObj.filetype != 'image/jpeg' && fileObj.filetype != 'image/png') {
				alert("Please select Image only !!!");
				return;
			}
			if (fileObj.filesize > 153601) {
				alert("Image should be less than 150 KB !!!");
				return;
			}
			ageCertificate = fileObj.base64;
		};

		var schemeProofCertificate = "";
		scope.onLoad_proofCertificate = function (e, reader, file, fileList, fileOjects, fileObj) {
			if (fileObj.filetype != 'image/jpeg' && fileObj.filetype != 'image/png') {
				alert("Please select Image only !!!");
				return;
			}
			if (fileObj.filesize > 153601) {
				alert("Image should be less than 150 KB !!!");
				return;
			}
			schemeProofCertificate = fileObj.base64;
		};

		scope.getCasteName = function (caste) {
			for (var i = 0; i < scope.casteList.length; i++) {
				if (caste == scope.casteList[i].CASTE_CAT_ID.toString()) {
					return scope.casteList[i].CASTE_CATEGORY;
				}
			}
			return '';
		};

		scope.btnSubmitDetails = function () {
			if (scope.gpId == undefined || scope.gpId == '' || scope.gpId == null) {
				alert('Please Select Panchayat / MC  ');
				return;
			}
			if (scope.habitationId == undefined || scope.habitationId == '' || scope.habitationId == null) {
				alert('Please Select Habitation / Ward ');
				return;
			}
			if (scope.relationType == undefined || scope.relationType == '' || scope.relationType == null) {
				alert('Please Select Relation Type ');
				return;
			}
			if (scope.relationName == undefined || scope.relationName == '' || scope.relationName == null) {
				alert('Please Enter Relation Name ');
				return;
			}
			if (scope.age == undefined || scope.age == '' || scope.age == null) {
				alert('Please Enter AGE ');
				return;
			}
			if (scope.dob == undefined || scope.dob == '' || scope.dob == null) {
				alert('Please Select Date Of Birth ');
				return;
			}
			if (scope.caste == undefined || scope.caste == '' || scope.caste == null) {
				alert('Please Select Caste ');
				return;
			}
			if (scope.subcaste == undefined || scope.subcaste == '' || scope.subcaste == null) {
				alert('Please Select Sub Caste');
				return;
			}
			if (scope.gender == undefined || scope.gender == '' || scope.gender == null) {
				alert('Please Select Gender ');
				return;
			}
			if (scope.maritalStatus == undefined || scope.maritalStatus == '' || scope.maritalStatus == null) {
				alert('Please Select Marital Status ');
				return;
			}
			if (scope.address1 == undefined || scope.address1 == '' || scope.address1 == null) {
				alert('Please Enter Address 1 ');
				return;
			}
			if (scope.mobileNumber == undefined || scope.mobileNumber == '' || scope.mobileNumber == null) {
				alert('Please Enter Mobile Number ');
				return;
			}
			if (scope.income == undefined || scope.income == '' || scope.income == null) {
				alert('Please Enter Family Income ');
				return;
			}
			if (scope.dryland == undefined || scope.dryland == '' || scope.dryland == null) {
				alert('Please Enter Dry Land holding of Family (In acers) ');
				return;
			}
			if (scope.wetland == undefined || scope.wetland == '' || scope.wetland == null) {
				alert('Please Enter Wet Land  holding of Family (In acers) ');
				return;
			}
			if (scope.fourWheeler == undefined || scope.fourWheeler == '' || scope.fourWheeler == null) {
				alert('Please Select Four wheeler in Family ');
				return;
			} else if (scope.fourWheeler == '1') {
				if (scope.fourWheelerDetails == undefined || scope.fourWheelerDetails == '' || scope.fourWheelerDetails == null) {
					alert('Please Enter Four Wheeler Details ');
					return;
				}
			}
			if (scope.govtEmployee == undefined || scope.govtEmployee == '' || scope.govtEmployee == null) {
				alert('Please Select Govt. Employee in Family ');
				return;
			} else if (scope.govtEmployee == '1') {
				if (scope.govtEmployeeDetails == undefined || scope.govtEmployeeDetails == '' || scope.govtEmployeeDetails == null) {
					alert('Please Enter govt Employee Details ');
					return;
				}
			}
			if (scope.electricityConsumption == undefined || scope.electricityConsumption == '' || scope.electricityConsumption == null) {
				alert('Please Enter Electricity consumption (In Units) ');
				return;
			}
			if (scope.pilaniArea == undefined || scope.pilaniArea == '' || scope.pilaniArea == null) {
				alert('Please Enter Property in Municipal / Plinth area (In Sq Ft) ');
				return;
			}
			if (scope.incomeTaxPayee == undefined || scope.incomeTaxPayee == '' || scope.incomeTaxPayee == null) {
				alert('Please Select Income Tax payee in family ');
				return;
			} else if (scope.incomeTaxPayee == '1') {
				if (scope.incomeTaxPayeeDetails == undefined || scope.incomeTaxPayeeDetails == '' || scope.incomeTaxPayeeDetails == null) {
					alert('Please Enter Income-Tax Payee Details');
					return;
				}
			}
			if (scope.anyOtherPension == undefined || scope.anyOtherPension == '' || scope.anyOtherPension == null) {
				alert('Please Select Any Other Pension in family');
				return;
			} else if (scope.anyOtherPension == '1') {
				if (scope.anyOtherPensionDetails == undefined || scope.anyOtherPensionDetails == '' || scope.anyOtherPensionDetails == null) {
					alert('Please Enter Any Other Pension Details ');
					return;
				}
			}
			if (scope.ageEntered == undefined || scope.ageEntered == '' || scope.ageEntered == null) {
				alert('Please Select Age entered is as per ');
				return;
			}
			if (ageCertificate == undefined || ageCertificate == '' || ageCertificate == null) {
				alert('Please Select AGE Certificate to Upload ');
				return;
			}
			if (scope.category != '1' && scope.category != '3') {
				if (scope.certificateType == undefined || scope.certificateType == '' || scope.certificateType == null) {
					alert('Please Select Category ');
					return;
				}
				if (schemeProofCertificate == undefined || schemeProofCertificate == '' || schemeProofCertificate == null) {
					alert('Please Select Certificate Proof to Upload ');
					return;
				}
			}


			var req = {
				aadharNumber: scope.uid,
				panchayatOrMcid: scope.gpId,
				habitationOrWardId: scope.habitationId,
				address: scope.address1,
				anotherAddress: scope.address2,
				age: scope.age,
				dateOfBirth: convertDate(scope.dob),
				applicantContactNum: scope.mobileNumber,
				familyIncomeforMonth: scope.income,
				familyDryLand: scope.dryland,
				familyWetLand: scope.wetland,
				fourWheelerFamilyYesOrNo: scope.fourWheeler,
				fourWheelerFamilyDEtails: scope.fourWheelerDetails,
				govtEmployeeYesOrNo: scope.govtEmployee,
				govtEmployeeDetails: scope.govtEmployeeDetails,
				electricConsumptionUnits: scope.electricityConsumption,
				incomeTaxPayeeYesOrNo: scope.incomeTaxPayee,
				incomeTaxDetails: scope.incomeTaxPayeeDetails,
				residentPropMuniInSqFt: scope.pilaniArea,
				otherPensionInFamilyYesOrNo: scope.anyOtherPension,
				otherPensionFamilyDetails: scope.anyOtherPensionType == 'OTHERS' ? scope.anyOtherPensionDetails : scope.anyOtherPensionType,
				disabilityId: scope.saradem,
				aadharFileName: scope.ageEntered,
				aadharProof: ageCertificate,
				schemeFileName: scope.certificateType,
				schemeProof: schemeProofCertificate,
				pensionType: scope.category,
				maritalStatus: scope.maritalStatus,
				caste: scope.getCasteName(scope.caste),
				subCaste: scope.subcaste,
				encrypted_data: scope.encrypted_data,
				iv: scope.iv,
				relationName: scope.relationName,
				relationType: scope.relationType
			};

			scope.loader = true;
			ps.encrypt_post("pensionAppSub", req, scope.token, function (data) {
				var res = data.data;
				if (res.status) {
					alert(res.result);
					window.location.reload();
				}
				else {
					alert(res.result);
				}
				scope.loader = false;
			}, function (error) {
				scope.loader = false;
				console.log(error);
			});


		};
	}



	function getParameterByName(name, url) {
		if (!url) url = window.location.href;
		name = name.replace(/[\[\]]/g, '\\$&');
		var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
			results = regex.exec(url);
		if (!results) return null;
		if (!results[2]) return '';
		return decodeURIComponent(results[2].replace(/\+/g, ' '));
	}

	function convertDate(date) {
		let month = String(date.getMonth() + 1).padStart(2, '0');
		let day = String(date.getDate()).padStart(2, '0');
		let year = date.getFullYear();
		var dateString = day + "-" + month + "-" + year;
		return dateString;
	}

	app.directive('numbersDotOnly', function () {
		return {
			require: '?ngModel',
			link: function (scope, element, attrs, ngModelCtrl) {
				if (!ngModelCtrl) {
					return;
				}

				ngModelCtrl.$parsers.push(function (val) {
					if (angular.isUndefined(val)) {
						val = '';
					}
					var clean = val.replace(/[^0-9\.]/g, '');
					var decimalCheck = clean.split('.');

					if (!angular.isUndefined(decimalCheck[1])) {
						decimalCheck[1] = decimalCheck[1].slice(0, 2);
						clean = decimalCheck[0] + '.' + decimalCheck[1];
					}

					if (val !== clean) {
						ngModelCtrl.$setViewValue(clean);
						ngModelCtrl.$render();
					}
					return clean;
				});

				element.bind('keypress', function (event) {
					if (event.keyCode === 32) {
						event.preventDefault();
					}
				});
			}
		};

	});

	app.directive('numbersOnly', function () {
		return {
			require: 'ngModel',
			restrict: 'A',
			link: function (scope, element, attr, ctrl) {
				function inputValue(val) {
					if (val) {
						var digits = val.replace(/[^0-9]/g, '');
						if (digits !== val) {
							ctrl.$setViewValue(digits);
							ctrl.$render();
						}
						return digits;//ParseInt(digits,10);
					}
					return undefined;
				}
				ctrl.$parsers.push(inputValue);
			}
		};
	});

})();