
(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("riceCardsInchargeMappingController", ["$scope", "riceCardsInchargeMappingServices", riceCardsInchargeMapping_CTRL]);


	function riceCardsInchargeMapping_CTRL(scope, rs) {


		scope.token = sessionStorage.getItem("Token");
		scope.userId = sessionStorage.getItem("username");

		scope.tableData = false;
		scope.inChargesList = "";


		scope.secChange = function () {
			scope.tableData = false;
			scope.inChargesList = "";
			if (scope.secList == "") {
				return;
			}
			scope.secName = sessionStorage.getItem("secname");
			scope.loader = true;

			var requestData = {
				userId: scope.userId,
				secId: sessionStorage.getItem("secccode")
			};
			rs.POSTENCRYPTAPI("inChargesList", requestData, scope.token, function (data) {
				var res = data.data;
				if (res.success) {
					scope.inChargesList = res.result;
				}
				else {
					alert(res.result);
				}
				scope.tableData = true;
				scope.loader = false;
			}, function (error) {
				scope.tableData = true;
				scope.loader = false;
				console.log(error);
			});
		};

		scope.secChange();

		scope.inchargeTypeChange = function () {
			scope.volunteerList = "";
			if (scope.isVolunteer == "1") {
				var requestData = {
					userId: scope.userId,
					secId: sessionStorage.getItem("secccode")
				};
				rs.POSTENCRYPTAPI("volunteersList", requestData, scope.token, function (data) {
					var res = data.data;
					if (res.success) {
						scope.volunteerList = res.result;
					}
					else {
						alert(res.result);
					}
					scope.tableData = true;
					scope.loader = false;
				}, function (error) {
					scope.tableData = true;
					scope.loader = false;
					console.log(error);
				});
			}
		};

		scope.btnChangeIncharge = function (obj) {
			console.log(obj);
			scope.isVolunteer = "";
			$("#changeInchargeModal").modal('show');
			scope.InchargeName = "";
			scope.InchargeUid = "";
			scope.InchargeMobile = "";
			scope.Inchargedesignation = "";
			scope.InchargeId = "";
			scope.clusterId = obj.CLUSTER_ID;
		};

		scope.mobileNumCheck = function (data) {
			var response = data.match('[6-9]{1}[0-9]{9}');
			if (response) {
				return true;
			}
			else {
				return false;
			}
		};

		scope.btnAddInchargeData = function () {
			var requestData;
			if (scope.isVolunteer == "1") {
				if (scope.volunteerDetails == "" || scope.volunteerDetails == null || scope.volunteerDetails == undefined) {
					alert("Please select Volunteer");
					return;
				}
				scope.InchargeUid = JSON.parse(scope.volunteerDetails).UID_NUM;
				scope.InchargeMobile = JSON.parse(scope.volunteerDetails).VOLUNTEER_MOBILE;
				scope.Inchargedesignation = "VOLUNTEER";
				scope.InchargeName = JSON.parse(scope.volunteerDetails).VOLUNTEER_NAME;
				scope.InchargeId = JSON.parse(scope.volunteerDetails).CFMS;

			} else {
				if (scope.InchargeName == "" || scope.InchargeName == undefined || scope.InchargeName == null) {
					alert("Please enter Incharge name ");
					return;
				}
				if (scope.InchargeUid == "" || scope.InchargeUid == undefined || scope.InchargeUid == null) {
					alert("Please enter Incharge aadhaar number  ");
					return;
				}
				if (!validateVerhoeff(scope.InchargeUid)) {
					alert("Please enter valid Incharge aadhaar number ");
					return;
				}
				if (scope.InchargeId == "" || scope.InchargeId == undefined || scope.InchargeId == null) {
					alert("Please enter Incharge CFMS ID ");
					return;
				}
				if (scope.InchargeId.length != 8) {
					alert("Please enter Valid Incharge CFMS ID ");
					return;
				}

				if (!scope.mobileNumCheck(scope.InchargeMobile)) {
					alert("Please enter valid Incharge mobile number ");
					return;
				}
				if (scope.Inchargedesignation == "" || scope.Inchargedesignation == undefined || scope.Inchargedesignation == null) {
					alert("Please enter Incharge Designation ");
					return;
				}

			}


			requestData = {
				userId: scope.userId,
				secId: sessionStorage.getItem("secccode"),
				clusterId: scope.clusterId,
				volUid: scope.InchargeUid,
				volMobile: scope.InchargeMobile,
				volType: scope.Inchargedesignation,
				volName: scope.InchargeName,
				volCfmsId: scope.InchargeId,
				reason: scope.isVolunteer
			};


			rs.POSTENCRYPTAPI("inChargeUpdate", requestData, scope.token, function (data) {
				var res = data.data;
				if (res.success) {
					alert("inCharge changed successfully !!!");
					$("#changeInchargeModal").modal('hide');
					scope.secChange();
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