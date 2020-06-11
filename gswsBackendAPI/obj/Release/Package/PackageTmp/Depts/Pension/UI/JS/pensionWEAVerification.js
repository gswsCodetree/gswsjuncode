(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("pensionWEAVerificationController", ["$scope", "pensionDeptServices", "$stateParams", "$state", pensionWEAVerification_CTRL]);


	function pensionWEAVerification_CTRL(scope, ps, stateParams, $state) {

		scope.token = sessionStorage.getItem("Token");
		scope.personDetails = '';
		scope.txnId = stateParams.txnId; // getParameterByName('txnId');
		scope.grevId = stateParams.grevId; // getParameterByName('grevId');

		if (scope.txnId != "" && scope.txnId != null && scope.txnId != undefined || scope.grevId != "" && scope.grevId != null && scope.grevId != undefined) {
			console.log(scope.txnId, scope.grevId);
		}
		else {
			alert("Invalid Request !!!");
			window.open("https://gramawardsachivalayam.ap.gov.in/GSWS/Home/Main", "_Self");
			return;
		}


		scope.loadGrevences = function () {
			scope.loader = true;
			var requestData = {
				secretariatId: sessionStorage.getItem("secccode"),
				transid: scope.txnId,
				grievanceId: scope.grevId
			};
			ps.encrypt_post("personGrevDetails", requestData, scope.token, function (data) {
				var res = data.data;
				if (res.status) {
					scope.personDetails = res.result;
				}
				else {
					alert(res.result);
					$state.go('ui.pensionGrevinanceList');
					//var url = "/#!/pensionGrevinanceList";
					//window.open(url, "_Self");
				}
				scope.loader = false;
			}, function (error) {
				scope.loader = false;
				console.log(error);
			});

		};

		scope.loadGrevences();

		scope.btnView = function (FileString, FileName) {
			scope.certificateType = FileName;
			scope.certificateString = FileString;
			if (scope.certificateString == undefined || scope.certificateString == '' || scope.certificateString == null) {
				alert('No File to Show');
				return;
			} else {
				$("#certificateModal").modal('show');
			}
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

		scope.btnSubmit = function () {

			if (scope.familyIncomeforMonth == undefined || scope.familyIncomeforMonth == '' || scope.familyIncomeforMonth == null) {
				alert('Please Enter Family Income For Month');
				return;
			}
			if (scope.familyDryLand == undefined || scope.familyDryLand == '' || scope.familyDryLand == null) {
				alert('Please Enter Family Dry Land ');
				return;
			}
			if (scope.familyWetLand == undefined || scope.familyWetLand == '' || scope.familyWetLand == null) {
				alert('Please Enter Family Wet Land ');
				return;
			}
			if (scope.fourWheelerFamilyYesOrNo == undefined || scope.fourWheelerFamilyYesOrNo == '' || scope.fourWheelerFamilyYesOrNo == null) {
				alert('Please Select Family Four Wheeler Status');
				return;
			}
			if (scope.fourWheelerFamilyYesOrNo == "YES") {
				if (scope.fourWheelerFamilyDEtails == undefined || scope.fourWheelerFamilyDEtails == '' || scope.fourWheelerFamilyDEtails == null) {
					alert('Please Enter Family Four Wheeler Details');
					return;
				}
				if (scope.fourWheelerFamilyDEtails.replace(' ', '').length == 0) {
					alert('Please Enter Valid Family Four Wheeler Details');
					return;
				}
			}
			if (scope.govtEmployeeYesOrNo == undefined || scope.govtEmployeeYesOrNo == '' || scope.govtEmployeeYesOrNo == null) {
				alert('Please Select Family Govt. Employee Status');
				return;
			}
			if (scope.govtEmployeeYesOrNo == "YES") {
				if (scope.govtEmployeeDetails == undefined || scope.govtEmployeeDetails == '' || scope.govtEmployeeDetails == null) {
					alert('Please Enter Family Govt. Employee Details');
					return;
				}
				if (scope.govtEmployeeDetails.replace(' ', '').length == 0) {
					alert('Please Enter Valid Family Govt. Employee Details');
					return;
				}
			}
			if (scope.electricConsumptionUnits == undefined || scope.electricConsumptionUnits == '' || scope.electricConsumptionUnits == null) {
				alert('Please Enter Electricity Consumption Units in month ');
				return;
			}
			if (scope.otherPensionInFamilyYesOrNo == undefined || scope.otherPensionInFamilyYesOrNo == '' || scope.otherPensionInFamilyYesOrNo == null) {
				alert('Please Select Other Pensioner in Family Status');
				return;
			}
			if (scope.incomeTaxPayeeYesOrNo == undefined || scope.incomeTaxPayeeYesOrNo == '' || scope.incomeTaxPayeeYesOrNo == null) {
				alert('Please Select Income Tax Payee in Family Status');
				return;
			}
			if (scope.incomeTaxPayeeYesOrNo == "YES") {
				if (scope.incomeTaxDetails == undefined || scope.incomeTaxDetails == '' || scope.incomeTaxDetails == null) {
					alert('Please Enter Income Tax Payee in Family Details');
					return;
				}
				if (scope.incomeTaxDetails.replace(' ', '').length == 0) {
					alert('Please Enter Valid Income Tax Payee in Family Details');
					return;
				}
			}
			if (scope.otherPensionInFamilyYesOrNo == "YES") {
				if (scope.anyOtherPensionType == "" || scope.anyOtherPensionType == '' || scope.anyOtherPensionType == null) {
					alert('Please Enter Other Pensioner in Family Type');
					return;
				} else if (scope.anyOtherPensionType == "OTHERS") {
					if (scope.otherPensionFamilyDetails == undefined || scope.otherPensionFamilyDetails == '' || scope.otherPensionFamilyDetails == null) {
						alert('Please Enter Other Pensioner in Family Details');
						return;
					}
					if (scope.otherPensionFamilyDetails.replace(' ', '').length == 0) {
						alert('Please Enter Valid Other Pensioner in Family Details');
						return;
					}
				}
			}
			if (scope.residentPropMuniInSqFt == undefined || scope.residentPropMuniInSqFt == '' || scope.residentPropMuniInSqFt == null) {
				alert('Please Enter Resident Property in Square Feet ');
				return;
			}
			if (scope.remarks == undefined || scope.remarks == '' || scope.remarks == null) {
				alert('Please Enter Field observation Remarks');
				return;
			}
			if (scope.aadharFile != undefined && scope.aadharFile != '' && scope.aadharFile != null) {
				if (ageCertificate == undefined || ageCertificate == '' || ageCertificate == null) {
					alert('Please Select Age Proof Certificate  to be upload');
					return;
				}
			}

			var req = {
				txnId: scope.txnId,
				data: {
					loginId: sessionStorage.getItem("username"),
					aadharNumber: scope.personDetails.details.aadhar,
					familyIncomeforMonth: scope.familyIncomeforMonth,
					familyDryLand: scope.familyDryLand,
					familyWetLand: scope.familyWetLand,
					fourWheelerFamilyYesOrNo: scope.fourWheelerFamilyYesOrNo,
					fourWheelerFamilyDEtails: scope.fourWheelerFamilyDEtails,
					govtEmployeeYesOrNo: scope.govtEmployeeYesOrNo,
					govtEmployeeDetails: scope.govtEmployeeDetails,
					electricConsumptionUnits: scope.electricConsumptionUnits,
					incomeTaxPayeeYesOrNo: scope.incomeTaxPayeeYesOrNo,
					incomeTaxDetails: scope.incomeTaxDetails,
					residentPropMuniInSqFt: scope.residentPropMuniInSqFt,
					otherPensionInFamilyYesOrNo: scope.otherPensionInFamilyYesOrNo,
					otherPensionFamilyDetails: scope.anyOtherPensionType == 'OTHERS' ? scope.otherPensionFamilyDetails : scope.anyOtherPensionType,
					aadharFileName: scope.aadharProof,
					aadharProof: ageCertificate,
					schemeFileName: scope.personDetails.details.category,
					schemeProof: schemeProofCertificate,
					remarks: scope.remarks,
					grievanceId: scope.grevId
				}
			};

			scope.loader = true;
			ps.encrypt_post("pensionAppWEASub", req, scope.token, function (data) {
				var res = data.data;
				if (res.status) {
					alert(res.result);
					//var url = "/#!/pensionGrevinanceList";
					//window.open(url, "_Self");
					$state.go('ui.pensionGrevinanceList');
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