
(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("HHGreaterController", ["$scope", "$state", "HHGreaterServices", HHGreater_CTRL]);

	function HHGreater_CTRL(scope, state, HHGreaterServices) {

		scope.token = sessionStorage.getItem("Token");
		scope.secccode = sessionStorage.getItem("secccode");
		scope.Loader = false;
		scope.citizenDetails = '';

		scope.getDetails = function () {

			scope.clusters = '';
			scope.uidNum = '';
			var requestData = {
				gsws_code: scope.secccode
			};
			HHGreaterServices.POSTENCRYPTAPI("clusterList", requestData, scope.token, function (value) {
				if (value.data.status) {
					scope.clusters = value.data.result;
				}
				else {
					alert(value.data.result);
				}
			}, function (error) {
				console.log(error);
			});

		};
		scope.getDetails();

		scope.btnSearchAadhaar = function () {
			if (scope.uidNum == '' || scope.uidNum == undefined || scope.uidNum == null) {
				alert("Please enter aadhaar number");
				return;
			}
			if (scope.uidNum.length < 12) {
				alert("Aadhaar number should be 12 digits");
				return;
			}
			else {
				if (!validateVerhoeff(scope.uidNum)) {
					alert("Please enter a valid aadhaar number");
					return;
				}
			}
			scope.Loader = true;
			scope.citizenDetails = '';
			var requestData = {
				gsws_code: scope.secccode,
				uidNum: scope.uidNum
			};
			HHGreaterServices.POSTENCRYPTAPI("Citizendetails", requestData, scope.token, function (value) {
				if (value.data.status) {
					scope.citizenDetails = value.data.result;
					console.log(scope.citizenDetails);

				}
				else {
					alert(value.data.result);
				}
				scope.Loader = false;
			}, function (error) {
				scope.Loader = false;
				console.log(error);
			});


		};

		scope.btnValidateFamily = function () {
			scope.hofList = [];
			for (var i = 0; i < scope.citizenDetails.length; i++) {

				if (scope.citizenDetails[i].IS_LIVING_WITHFAMILY == "") {
					alert("You've not selected family status for " + scope.citizenDetails[i].CITIZEN_NAME);
					return;
				}

				if (scope.citizenDetails[i].AGE < 18 && scope.citizenDetails[i].AGE > 1) {
					if (scope.citizenDetails[i].FATHER_AADHAAR == "" || scope.citizenDetails[i].FATHER_AADHAAR == null || scope.citizenDetails[i].FATHER_AADHAAR == undefined) {
						alert("Father aadhaar mandatory for below 18 years person for " + scope.citizenDetails[i].CITIZEN_NAME);
						return;
					}
					else {
						if (scope.citizenDetails[i].FATHER_AADHAAR.length < 12) {
							alert("Father Aadhaar number should be 12 digits for " + scope.citizenDetails[i].CITIZEN_NAME);
							return;
						}
						else {
							if (!validateVerhoeff(scope.citizenDetails[i].FATHER_AADHAAR)) {
								alert("Please enter a valid father aadhaar number for " + scope.citizenDetails[i].CITIZEN_NAME);
								return;
							}
						}
					}
					if (scope.citizenDetails[i].MOTHER_AADHAAR == "" || scope.citizenDetails[i].MOTHER_AADHAAR == null || scope.citizenDetails[i].MOTHER_AADHAAR == undefined) {
						alert("Mother aadhaar mandatory for below 18 years person for " + scope.citizenDetails[i].CITIZEN_NAME);
						return;
					}
					else {
						if (scope.citizenDetails[i].MOTHER_AADHAAR.length < 12) {
							alert("Mother Aadhaar number should be 12 digits for " + scope.citizenDetails[i].CITIZEN_NAME);
							return;
						}
						else {
							if (!validateVerhoeff(scope.citizenDetails[i].MOTHER_AADHAAR)) {
								alert("Please enter a valid mother aadhaar number for " + scope.citizenDetails[i].CITIZEN_NAME);
								return;
							}
						}
					}
				}

				if (scope.citizenDetails[i].IS_MARRIED == "1") {

					if (scope.citizenDetails[i].SPOUSE_AADHAAR == "" || scope.citizenDetails[i].SPOUSE_AADHAAR == undefined || scope.citizenDetails[i].SPOUSE_AADHAAR == null) {
						alert("Please enter a spouse aadhaar for " + scope.citizenDetails[i].CITIZEN_NAME);
						return;
					}

					if (scope.citizenDetails[i].SPOUSE_AADHAAR.length < 12) {
						alert("Spouse Aadhaar number should be 12 digits for " + scope.citizenDetails[i].CITIZEN_NAME);
						return;
					}
					else {
						if (!validateVerhoeff(scope.citizenDetails[i].SPOUSE_AADHAAR)) {
							alert("Please enter a valid spouse aadhaar number for " + scope.citizenDetails[i].CITIZEN_NAME);
							return;
						}
					}
				}

				scope.hofList.push({ UID_NUM: scope.citizenDetails[i].UID_NUM, NAME: scope.citizenDetails[i].CITIZEN_NAME });

			}
			scope.otpFlag = false;
			$("#householdMappingModal").modal('show');

		};

		scope.btnAddHosuehold = function () {
			if (scope.cluster_id == "" || scope.cluster_id == null || scope.cluster_id == undefined) {
				alert("Please select cluster");
				return;
			}
			if (scope.DOOR_NO == "" || scope.DOOR_NO == null || scope.DOOR_NO == undefined) {
				alert("Please enter door no");
				return;
			}
			if (scope.hof == "" || scope.hof == null || scope.hof == undefined) {
				alert("Please select Head Of Family");
				return;
			}
			console.log(scope.cluster_id);
			for (var i = 0; i < scope.citizenDetails.length; i++) {
				scope.citizenDetails[i].DOOR_NO = scope.DOOR_NO;
				scope.citizenDetails[i].CLUSTER_ID = JSON.parse(scope.cluster_id).CLUSTER_ID;
				scope.citizenDetails[i].CLUSTER_NAME = JSON.parse(scope.cluster_id).CLUSTE;
				scope.citizenDetails[i].DISTRICT_CODE = sessionStorage.getItem("distcode");
				scope.citizenDetails[i].MANDAL_CODE = sessionStorage.getItem("mcode");
				scope.citizenDetails[i].SECRETARIAT_CODE = scope.secccode;

				if (scope.citizenDetails[i].UID_NUM == scope.hof) {
					scope.citizenDetails[i].IS_HOFAMILY = 1;
				}
			}

			if (scope.otp == "" || scope.otp == null || scope.otp == undefined) {
				alert("Please enter OTP");
				return;
			}
			if (scope.otp != scope.otpVal) {
				alert("Invalid OTP, Please try again !!!");
				window.location.reload();
			}
			scope.otpFlag = false;
			var requestData = {
				responseData: scope.citizenDetails,
				insertedBy: sessionStorage.getItem("uniqueid")
			};
			HHGreaterServices.POSTENCRYPTAPI("dataSubmission", requestData, scope.token, function (value) {
				if (value.data.status) {
					alert(value.data.result);
					window.location.reload();
				}
				else {
					alert(value.data.result);
				}
				scope.otpFlag = true;
			}, function (error) {
				scope.otpFlag = true;
				console.log(error);
			});
		};

		scope.btnOTPSend = function () {
			if (scope.cluster_id == "" || scope.cluster_id == null || scope.cluster_id == undefined) {
				alert("Please select cluster");
				return;
			}
			if (scope.DOOR_NO == "" || scope.DOOR_NO == null || scope.DOOR_NO == undefined) {
				alert("Please enter door no");
				return;
			}
			if (scope.hof == "" || scope.hof == null || scope.hof == undefined) {
				alert("Please select Head Of Family");
				return;
			}
			if (scope.mobileNum == "" || scope.mobileNum == null || scope.mobileNum == undefined) {
				alert("Please enter mobile number");
				return;
			}
			var requestData = {
				mobileNum: scope.mobileNum
			};
			HHGreaterServices.POSTENCRYPTAPI("sendOTP", requestData, scope.token, function (value) {
				if (value.data.status) {
					scope.otpVal = value.data.result;
					alert("OTP Sent Successfully !!!");
					scope.otpFlag = true;
				}
				else {
					alert(value.data.result);
				}
			}, function (error) {
				console.log(error);
			});


		};

		scope.btnAddMemberModal = function () {
			$("#addMemberModal").modal('show');
			scope.addMemberUid = "";
		};

		scope.btnAddMember = function () {
			if (scope.addMemberUid == '' || scope.addMemberUid == undefined || scope.addMemberUid == null) {
				alert("Please enter aadhaar number");
				return;
			}
			if (scope.addMemberUid.length < 12) {
				alert("Aadhaar number should be 12 digits");
				return;
			}
			else {
				if (!validateVerhoeff(scope.addMemberUid)) {
					alert("Please enter a valid aadhaar number");
					return;
				}
			}

			for (var i = 0; i < scope.citizenDetails.length; i++) {
				if (scope.citizenDetails[i].UID_NUM == scope.addMemberUid) {
					alert("Enter member is already in this list");
					return;
				}
			}

			var requestData = {
				uidNum: scope.addMemberUid
			};
			HHGreaterServices.POSTENCRYPTAPI("addMember", requestData, scope.token, function (value) {
				if (value.data.status) {
					scope.memberData = value.data.result[0];
					console.log(scope.memberData);
					scope.citizenDetails.push({
						HOUSEHOLD_ID: scope.memberData.HOUSEHOLD_ID,
						UID_NUM: scope.memberData.UID_NUM,
						CITIZEN_NAME: scope.memberData.CITIZEN_NAME,
						DOB_DT: convertDate(scope.memberData.DOB_DT),
						MOBILE_NUMBER: scope.memberData.MOBILE_NUMBER,
						GENDER: scope.memberData.GENDER,
						IS_LIVING_WITHFAMILY: "",
						IS_HOFAMILY: "",
						IS_MEMBERADDED: "1",
						IS_MEMBERDELETED: "",
						RELATION_WITHHOF: "",
						FATHER_AADHAAR: "",
						MOTHER_AADHAAR: "",
						SPOUSE_AADHAAR: "",
						IS_MARRIED: "0",
						TEMP_ID: "",
						NEW_HH_ID: "",
						DISTRICT_CODE: "",
						DISTRICT_NAME: "",
						MANDAL_CODE: "",
						MANDAL_NAME: "",
						SECRETARIAT_CODE: "",
						SECRETARIAT_NAME: "",
						DOOR_NO: "",
						HOUSE_IMAGE_PATH: "",
						CLUSTER_ID: "",
						CLUSTER_NAME: "",
						MAPPING_STATUS: scope.memberData.MAPPING_STATUS,
						AGE: scope.memberData.AGE
					});
					$("#addMemberModal").modal('hide');
					alert("person added successfully !!!");
				}
				else {
					alert(value.data.result);
				}
			}, function (error) {
				console.log(error);
			});

		};

		scope.btnRemovePerson = function (uidNum) {
			if (confirm("Are you sure want to remove the person")) {
				for (var i = 0; i < scope.citizenDetails.length; i++) {
					if (scope.citizenDetails[i].UID_NUM == uidNum) {
						scope.citizenDetails[i].IS_MEMBERDELETED = "1";
						alert("Person successfully removed from family !!!");
						console.log("updated results with delete flag", scope.citizenDetails);
						return;
					}
				}
				alert("Failed to remove Person from family !!!");

			}
		};

	}

	function convertDate(date) {
		var val = date.substr(0, 10);
		var splitVal = val.split("-");
		console.log(splitVal);
		return splitVal[2] + "-" + splitVal[1] + "-" + splitVal[0];
	}


})();