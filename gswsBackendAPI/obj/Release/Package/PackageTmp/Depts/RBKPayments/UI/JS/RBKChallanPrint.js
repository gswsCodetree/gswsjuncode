(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("RBKChallanPrintController", ["$scope", "$state", "$log", '$interval', "seedsPaymentsServices", 'DTOptionsBuilder', RBKChallanPrintController]);

	function RBKChallanPrintController(scope, state, log, interval, seedsPaymentsService, DTOptionsBuilder) {
		scope.preloader = false;
		scope.secretariat = sessionStorage.getItem("secccode");
		scope.token = sessionStorage.getItem("Token");
		if (!scope.secretariat || !scope.token) {
			window.location = "https://gramawardsachivalayam.ap.gov.in/GSWS/Home/Main#";
			return;
		}
		scope.secretariat = "11090058";
	
		scope.dtOptions = DTOptionsBuilder.newOptions().withOption('lengthMenu', [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]])


		scope.Amountlist = [{ AmountVal: 2000 }, { AmountVal: 500 }, { AmountVal: 200 }, { AmountVal: 100 }, { AmountVal: 50 }, { AmountVal: 20 }, { AmountVal: 10 }, { AmountVal: 5 }, { AmountVal: 2 }, { AmountVal: 1 }]
		//Get Data
		scope.GetChallanDetails = function () {
			if (!scope.rbkchallan) {
				swal('info', 'Please Enter RBK Challan Number', 'info');
				return;
			}
			else {
				scope.type = "14";
				GetData();

			}


		}
		function GetData() {
			scope.preloader = true;
			var input = {
				P_TYPE: scope.type,
				P_DISTRICT: scope.district,
				P_MANDAL: scope.mandal,
				P_SEC: scope.secretariat,
				P_CHALAN_TXN_ID: scope.rbkchallan,
				P_BANK_REF_NO: scope.bankrefno,
				P_BANK_TXN_DATE: scope.banktxndate
			};

			seedsPaymentsService.encrypt_post("SeedbankDepositModule", input, scope.token, function (data) {
				scope.preloader = false;
				var res = data.data;
				console.log(res);
				if (res.Status == "100") {
					if (res.DataList[0]["STATUS"] == "1") {
						scope.preloader = false;
						scope.divprint = true;
						scope.divbtn = true;
						scope.secretariatname = res.DataList[0]["SECRETARIAT_NAME"];
						scope.secretariatactno = res.DataList[0]["SEC_BANK_ACCOUNT_NO"];
						scope.secretariatifsccode = res.DataList[0]["SEC_IFSC_CODE"];
						scope.secretariatacttype = res.DataList[0]["SEC_ACCOUNT_TYPE"];
						scope.secretariatbranch = res.DataList[0]["SEC_BRANCH"];

						scope.neftactno = res.DataList[0]["ACT_NO"];
						scope.neftbankbranch = res.DataList[0]["BANK_AND_BRANCH_NAME"];
						scope.neftifsccode = res.DataList[0]["IFSC_CODE"];
						scope.neftdistrictname = res.DataList[0]["DISTRICT_NAME"];
						scope.totalamnt = res.DataList[0]["TOTAL_AMOUNT"];
						scope.nefttransctndate = res.DataList[0]["CHALLAN_DATE"];
						scope.nefttransctnid = res.DataList[0]["CHALLAN_TXN_ID"];
						scope.BenName = res.DataList[0]["ACT_NAME"];
						scope.BenAccnum = res.DataList[0]["BENEFICIARY_ACT_NO"];
						scope.Nameofvsact = res.DataList[0]["NAME_OF_VS_ACT"];
						scope.amountwords = res.DataList[0]["AMT_IN_WORDS"];

					}
					else if (res.DataList[0]["STATUS"] == "0") {
						scope.preloader = false;
						scope.divprint = false;
						scope.errormsg = "Chalan Generation Failed";
						swal("info", "Chalan Generation Failed", "info");
					}
					else if (res.DataList[0]["STATUS"] == "5") {
						scope.preloader = false;
						scope.divprint = false;
						scope.errormsg = "Chalan Already Generated for today.Please try again tomorrow";
						swal("info", "Chalan Already Generated for today.Please try again tomorrow.", "info");
					}
					else if (res.DataList[0]["STATUS"] == "10") {
						scope.preloader = false;
						scope.divprint = false;
						scope.errormsg = "Challan ID cannot be generated due to Secretariat Bank details not found";
						swal("info", "Challan ID cannot be generated due to Secretariat Bank details not found", "info");
					}
				}
				else {
					scope.preloader = false;
					scope.divprint = false;

					swal("info", "Something went wrong.Please try again later.", "info");
				}
			});
		}

		scope.BankChallan = function () {

			scope.divprint = true;
			scope.divprintbank = false;
		}
		scope.BankDepositForm = function () {
			scope.divprint = false;
			scope.divprintbank = true;

		}

		scope.PrintReceipt = function () {

			//	scope.divbutton = true;
			var divprint = document.getElementById("printdiv").innerHTML;

			var popupWinindow = window.open('', 'Print-Window');
			popupWinindow.document.open();
			popupWinindow.document.write('<html><head><link href="assets/css/bootstrap.css" rel="stylesheet" type="text/css"><link rel="stylesheet" type="text/css" href="assets/css/printcss.css" /></head><body onload="window.print()">' + divprint + '</html>');
			popupWinindow.document.close();
		}

		scope.PrintReceiptBank = function () {

			//	scope.divbutton = true;
			var divprint = document.getElementById("printdivbank").innerHTML;

			var popupWinindow = window.open('', 'Print-Window');
			popupWinindow.document.open();
			popupWinindow.document.write('<html><head><link href="assets/css/bootstrap.css" rel="stylesheet" type="text/css"><link rel="stylesheet" type="text/css" href="assets/css/printcss.css" /></head><body onload="window.print()">' + divprint + '</html>');
			popupWinindow.document.close();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	}
})();