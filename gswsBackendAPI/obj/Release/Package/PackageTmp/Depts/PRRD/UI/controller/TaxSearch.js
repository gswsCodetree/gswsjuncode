(function () {
	var app = angular.module("GSWS");
	app.controller("TaxAppSearch", ["$scope", "$state", "PRRD_Services", '$sce', TaxAppSearchCall]);

	function TaxAppSearchCall(scope, state, PRRD_Services, sce) {
		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return false;
		}

		LoadPanchayats();

		scope.TaxTypeChange = function () {
			scope.ddlRequest = "";
		}

		//scope.detailsshow = false;
		scope.getdetails = function () {
			if (!scope.ddlPanchayat) {
				alert('Please Select Panchayat.');
				return;
			}
			else if (!scope.ddlType) {
				alert('Please Select Tax Type.');
				return;
			}
			else if (!scope.ddlSearchby) {
				alert('Please enter Search By.');
				return;
			}
			else if (!scope.txtValue) {
				alert('Please enter Value.');
				return;
			}
			else if (!scope.ddlRequest) {
				alert('Please enter Request Type.');
				return;
			}
			scope.currGPCode = scope.ddlPanchayat;
			if (scope.ddlSearchby == "1") {
				var card = scope.txtValue;
				if (scope.txtValue.length != 12) {
					alert('Please Enter 12 Digit Aadhaar Number.');
					return;
				}
				if (card == "111111111111" || card == "222222222222" || card == "333333333333" || card == "444444444444" || card == "555555555555" || card == "666666666666"
					|| card == "777777777777" || card == "888888888888" || card == "999999999999" || card == "000000000000") {
					alert("Please Enter 12 Digit Aadhaar Number");
					return;
				}
				var status = validateVerhoeff(card);
				if (!status) {
					alert('Enter Valid Aadhaar Number');
					return;
				}
			}
			if (scope.ddlSearchby == "5") {
				if (scope.txtValue.length != 10) {
					alert('Please Enter 10 Digit Mobile Number.');
					return;
				}
			}

			var objCert = {
				taxType: scope.ddlType,
				searchBy: scope.ddlSearchby,
				value: scope.txtValue,
				Secccode: scope.currGPCode,
				UserId: sessionStorage.getItem("user"),
				SacId: sessionStorage.getItem("secccode"),
				DesignId: sessionStorage.getItem("desinagtion"),
				TranId: scope.ddlType
			};

			scope.mutation = false;
			scope.trade = false;
			scope.tap = false;
			scope.property = false;

			PRRD_Services.POSTENCRYPTAPI("fetchTransactionData", objCert, token, function (value) {
				var res = value.data;

				if (res.Status == "Success") {
					scope.Status = "Available";
					if (res.data.status == 100) {
						if (scope.ddlRequest == "1") { scope.mutation = true; }
						else if (scope.ddlRequest == "2") { scope.trade = true; }
						else if (scope.ddlRequest == "3") { scope.tap = true; }
						else if (scope.ddlRequest == "4") { scope.property = true; }

						scope.resdata = res.data;
						scope.housedata = res.data.housedata;

						scope.detailsshow = true;

						scope.uniqueno = res.data.housedata[0].unique;
						scope.eLTransactionId = res.data.eLTransactionId;
					}
					else {
						scope.detailsshow = false; scope.resdata = "";
						scope.housedata = ""; alert(res.data.msg);
					}
				}
				else if (res.Status == "428") {
					sessionStorage.clear();
					swal("info", "Session Expired !!!", "info");
					state.go("Login");
					return;
				}
				else {
					scope.Status = "Not Available";
					scope.resdata = "";
					scope.detailsshow = false;
					alert('No Data Found');
				}

			});
		};

		scope.sendRequest = function (value) {

			var objRequest = {
				UserId: sessionStorage.getItem("user"),
				SacId: sessionStorage.getItem("secccode"),
				DesignId: sessionStorage.getItem("desinagtion"),
				TranId: "",
				Secccode: scope.currGPCode,
				GSWS_ID: sessionStorage.getItem("TransID")
			};

			if (value == 1) {
				if (!scope.txtMutationUID) {
					alert('Please enter Aadhar.');
					return;
				}

				//var val = scope.txtcertid.length;
				var card = scope.txtMutationUID;
				if (scope.txtMutationUID.length < 12) {
					alert('Please Enter 12 Digit Aadhaar Number.');
					return;
				}
				if (card == "111111111111" || card == "222222222222" || card == "333333333333" || card == "444444444444" || card == "555555555555" || card == "666666666666"
					|| card == "777777777777" || card == "888888888888" || card == "999999999999" || card == "000000000000") {
					alert("Please Enter 12 Digit Aadhaar Number");
					return;
				}
				var status = validateVerhoeff(card);
				if (!status) {
					alert('Enter Valid Aadhaar Number');
					return;
					//return status;
				}

				if (!scope.txtMutationName) {
					alert('Please enter Owner Name.');
					return;
				}
				else if (!scope.txtMutationFirstName) {
					alert('Please enter First Name.');
					return;
				}
				else if (!scope.txtMutationDOB) {
					alert('Please enter Date of Birth.');
					return;
				}
				else if (!scope.txtMutationMobileNo) {
					alert('Please enter Mobile Number.');
					return;
				}
				else if (!scope.ddlMutationGender) {
					alert('Please enter Gender.');
					return;
				}

				objRequest.aadhar = scope.txtMutationUID;
				objRequest.owner_name = scope.txtMutationName;
				objRequest.fname = scope.txtMutationFirstName;
				objRequest.dob = scope.txtMutationDOB;
				objRequest.mobile = scope.txtMutationMobileNo;
				objRequest.gender = scope.ddlMutationGender;
				objRequest.unique = scope.uniqueno;
				objRequest.RequestType = 1;


			}
			else if (value == 2) {
				if (!scope.txtTradeName) {
					alert('Please enter Trade Name.');
					return;
				}
				else if (!scope.txtTradeOwnerName) {
					alert('Please enter Trade Owner Name.');
					return;
				}
				else if (!scope.txtTradeMobileNo) {
					alert('Please enter Mobile Number');
					return;
				}

				else if (!scope.txtTradeUID) {
					alert('Please enter Aadhar');
					return;
				}
				var card = scope.txtTradeUID;
				if (scope.txtTradeUID.length < 12) {
					alert('Please Enter 12 Digit Aadhaar Number.');
					return;
				}
				if (card == "111111111111" || card == "222222222222" || card == "333333333333" || card == "444444444444" || card == "555555555555" || card == "666666666666"
					|| card == "777777777777" || card == "888888888888" || card == "999999999999" || card == "000000000000") {
					alert("Please Enter 12 Digit Aadhaar Number");
					return;
				}
				var status1 = validateVerhoeff(card);
				if (!status1) {
					alert('Enter Valid Aadhaar Number');
					return;
					//return status;
				}
				if (!scope.txtTradeAssenssment) {
					alert('Please enter Assenssment Number.');
					return;
				}
				else if (!scope.ddlTradeType) {
					alert('Please enter Trade Type.');
					return;
				}

				objRequest.trade_name = scope.txtTradeName;
				objRequest.owner_name = scope.txtTradeOwnerName;
				objRequest.mobile = scope.txtTradeMobileNo;
				objRequest.aadhar = scope.txtTradeUID;
				objRequest.assessment_no = scope.txtTradeAssenssment;
				objRequest.trade_type = scope.ddlTradeType;
				objRequest.RequestType = 2;

			}
			else if (value == 3) {
				if (!scope.txtTapTapSize) {
					alert('Please enter Tap Size.');
					return;
				}
				else if (!scope.ddlTapUsage) {
					alert('Please Select Tap Usage.');
					return;
				}
				else if (!scope.txtTapAssessment) {
					alert('Please enter Tap Assessment Number.');
					return;
				}

				objRequest.tap_size = scope.txtTapTapSize;
				objRequest.usage = scope.ddlTapUsage;
				objRequest.noofconn = scope.txtTapAssessment;
				objRequest.unique = scope.uniqueno;
				objRequest.RequestType = 3;


			}
			else if (value == 4) {

				objRequest.elTransactionId = scope.eLTransactionId;
				objRequest.unique = scope.uniqueno;
				objRequest.RequestType = 4;

			}

			objRequest.UserId = sessionStorage.getItem("user");
			objRequest.SacId = sessionStorage.getItem("secccode");
			objRequest.DesignId = sessionStorage.getItem("desinagtion");
			objRequest.TranId = scope.ddlType;
			objRequest.Secccode = scope.currGPCode;
			objRequest.GSWS_ID = sessionStorage.getItem("TransID");


			PRRD_Services.POSTENCRYPTAPI("SendTransactionRequest", objRequest, token, function (value) {
				var res = value.data;

				if (res.Status == "Success") {
					scope.Status = "Available";
					if (res.data.status == 0) {

						scope.mutation = false;
						scope.trade = false;
						scope.tap = false;
						scope.property = false;

						swal('info', res.data.msg, 'info');
						setTimeout(function () { window.open('', '_self', '').close(); }, 3000);

					}
					else {
						alert(res.data.msg);
					}
				}
				else if (res.Status == "428") {
					sessionStorage.clear();
					swal("info", "Session Expired !!!", "info");
					state.go("Login");
					return;
				}
				else {
					scope.Status = "Not Available";
					scope.resdata = "";
					scope.detailsshow = false;
					alert('No Data Found');
				}

			});

		};

		function LoadPanchayats() {
			var objCert = {
				Secccode: sessionStorage.getItem("secccode"),
				UserId: sessionStorage.getItem("user"),
				SacId: sessionStorage.getItem("secccode"),
				DesignId: sessionStorage.getItem("desinagtion")
			};

			PRRD_Services.POSTENCRYPTAPI("fetchPanchayats", objCert, token, function (value) {
				var res = value.data;
				if (res.Status == "Success") {
					var data = res.data;
					if (data.status == "200") { scope.PanchayatList = data.data; }
					else { swal("info", data.msg, "info"); }
				}
				else if (res.Status == "428") {
					sessionStorage.clear();
					swal("info", "Session Expired !!!", "info");
					state.go("Login");
					return;
				}
				else {
					alert('No Panchayat Data Found');
				}

			});
		}

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