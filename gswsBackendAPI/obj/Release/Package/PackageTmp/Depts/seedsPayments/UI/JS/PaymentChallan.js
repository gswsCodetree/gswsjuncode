(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("PaymentChallanController", ["$scope", "$state", "$log", '$interval', "seedsPaymentsServices", 'DTOptionsBuilder', PaymentChallanController]);

	function PaymentChallanController(scope, state, log, interval, seedsPaymentsService, DTOptionsBuilder) {
		scope.preloader = false;
		scope.secretariat = sessionStorage.getItem("secccode");
		scope.token = sessionStorage.getItem("Token");
		//scope.secretariat = "11290048";
		if (!scope.secretariat || !scope.token) {
			window.location = "https://gramawardsachivalayam.ap.gov.in/GSWS/Home/Main#";
			return;
		}
		scope.type = "2";
		GetData();

		scope.dtOptions = DTOptionsBuilder.newOptions().withOption('lengthMenu', [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]])


		scope.GetData = function () {
			scope.chalantxnid = "";
			scope.bankrefno = "";
			scope.banktxndate = "";
			if (scope.reporttype == "C") {
				scope.type = "3";
			}
			else if (scope.reporttype == "") {
				scope.type = "2";
			}
			GetData();
		}

		//Get Data
		function GetData() {
			scope.preloader = true;
			var input = {
				P_TYPE: scope.type,
				P_DISTRICT: scope.district,
				P_MANDAL: scope.mandal,
				P_SEC: scope.secretariat,
				P_CHALAN_TXN_ID: scope.chalantxnid,
				P_BANK_REF_NO: scope.bankrefno,
				P_BANK_TXN_DATE: scope.banktxndate
			};

			seedsPaymentsService.encrypt_post("SeedbankDepositModule", input, scope.token, function (data) {
				scope.preloader = false;
				var res = data.data;
				console.log(res);
				if (res.Status == "100") {
					if (scope.type == "3" || scope.type == "2") {
						scope.preloader = false;
						scope.pendingdata = res.DataList;
						scope.table = "PENDING";
					}
					else if (scope.type == "4" && res.DataList[0]["STATUS"] == "1") {
						scope.preloader = false;
						swal({
							title: "",
							text: "Data Updated Succesfully",
							type: ""
						}).then(function () {
							scope.type = "3";
							scope.chalantxnid = "";
							scope.bankrefno = "";
							scope.banktxndate = "";
							scope.reporttype = "C";
							$('#myModal').modal('toggle');
							GetData();
						});
					}
					else if (scope.type == "4" && res.DataList[0]["STATUS"] == "0") {
						scope.preloader = false;
						swal("", "Data Updation Failed.Please try again Later", "");
					}
					else if (scope.type == "6") {
						scope.preloader = false;
						scope.detaildata = res.DataList;
						scope.table = "DETAILS";
					}
					else if (scope.type == "5") {
						scope.preloader = false;
						scope.pendetaildata = res.DataList;
						scope.table = "PENDINGDETAILS";
					}
				}
				else {
					scope.preloader = false;
					scope.pendingdata = "";
					swal("", "No Data Found", "");
					scope.table = "PENDING";
				}
			});
		}

		scope.ClickCount = function (type, pen) {
			 if (type == "5") {
				scope.type = type;
				scope.viewgeneratebtn = pen;
			}
			else if (type == "6") {
				scope.type = type;
				scope.chalantxnid = pen.CHALLAN_TXN_ID;
			}
			GetData();
		}

		scope.remove = function (type) {
			scope.type = type;
			GetData();
		}

		scope.Updateclick = function (pen) {
			scope.chalantxnid = pen.CHALLAN_TXN_ID;
		}

		scope.Updatemodalclick = function () {
			if (!scope.bankrefno) {
				swal("", "Please enter Bank Reference Number", "");
				return;
			}
			else if (!scope.banktxndate) {
				swal("", "Please Select Bank Transaction Date", "");
				return;
			}
			else {
				scope.type = "4";
				scope.banktxndate = moment(scope.banktxndate).format('DD-MMM-YY');
				GetData();
			}
		}

		scope.GenerateChalan = function () {
			sessionStorage.setItem("secccode", scope.secretariat);
			window.open("../GSWS/#!/SeedsChalanGeneration",'_blank');
			//window.open("http://localhost:3831/#!/SeedsChalanGeneration", '_blank');
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	}
})();