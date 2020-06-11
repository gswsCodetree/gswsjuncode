(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("SeedsChallanPrintController", ["$scope", "$state", "$log", '$interval', "seedsPaymentsServices", 'DTOptionsBuilder', SeedsChallanPrintController]);

	function SeedsChallanPrintController(scope, state, log, interval, seedsPaymentsService, DTOptionsBuilder) {
		scope.preloader = false;
		scope.secretariat = sessionStorage.getItem("secccode");
		scope.token = sessionStorage.getItem("Token");
		if (!scope.secretariat || !scope.token) {
			window.location = "https://gramawardsachivalayam.ap.gov.in/GSWS/Home/Main#";
			return;
		}
		scope.secretariat = "10390291";
		
		scope.dtOptions = DTOptionsBuilder.newOptions().withOption('lengthMenu', [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]])
		scope.GetSeedsChallanDetails = function () {
			if (!scope.seedschallan) {
				swal('info', 'Please Enter Seeds Challan Number', 'info');
				return;
			}
			else {
				scope.type = "13";
				GetData();

			}


		}
	
		//Get Data
		function GetData() {
			scope.preloader = true;
			var input = {
				P_TYPE: scope.type,
				P_DISTRICT: scope.district,
				P_MANDAL: scope.mandal,
				P_SEC: scope.secretariat,
				P_CHALAN_TXN_ID: scope.seedschallan,
				P_BANK_REF_NO: scope.bankrefno,
				P_BANK_TXN_DATE: scope.banktxndate
			};

			seedsPaymentsService.encrypt_post("SeedbankDepositModule", input, scope.token, function (data) {
				scope.preloader = false;
				var res = data.data;
				console.log(res);
				if (res.Status == "100") {
					scope.divbtn = true;
					if (res.DataList[0]["STATUS"] == "1") {
						scope.preloader = false;

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
					}
					else if (res.DataList[0]["STATUS"] == "0") {
						scope.preloader = false;
						swal("", "Chalan Generation Failed", "");
					}
					else if (res.DataList[0]["STATUS"] == "5") {
						scope.preloader = false;
						swal("", "Chalan Already Generated for today.Please try again tomorrow.", "");
					}
					else if (res.DataList[0]["STATUS"] == "10") {
						scope.preloader = false;
						swal("", "Challan ID cannot be generated due to Secretariat Bank details not found", "");
					}
				}
				else {
					scope.preloader = false;
					swal("", "Something went wrong.Please try again later.", "");
				}
			});
		}


		scope.PrintReceipt = function () {

			//	scope.divbutton = true;
			var divprint = document.getElementById("printdiv").innerHTML;

			var popupWinindow = window.open('', 'Print-Window');
			popupWinindow.document.open();
			popupWinindow.document.write('<html><head><link href="assets/css/bootstrap.css" rel="stylesheet" type="text/css"><link rel="stylesheet" type="text/css" href="assets/css/printcss.css" /></head><body onload="window.print()">' + divprint + '</html>');
			popupWinindow.document.close();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	}
})();