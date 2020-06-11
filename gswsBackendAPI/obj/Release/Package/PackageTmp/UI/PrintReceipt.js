(function () {
    var app = angular.module("GSWS");

	app.controller("PrintReceipt", ["$scope", "$state", "$log", "Login_Services", AppStatusController]);

    function AppStatusController(scope, state, log, Login_Services) {
        //alert(rs.username);
        var username = sessionStorage.getItem("user");
        var secid = sessionStorage.getItem("secccode");
        var desingation = sessionStorage.getItem("desinagtion");
		    
        var token = sessionStorage.getItem("Token");
        if (!username || !token) {

            state.go("Login");
            return;
		}
		else
			LoadPrint();

		function LoadPrint() {
			var input = "";
		  var objdata=JSON.parse(sessionStorage.getItem("pData"));
			var ftype = objdata.SOURCE_TYPE;
			if (ftype == "Mee-Seva")
				input = { type: 6, secid: objdata.BEN_TRANS_ID }			
			else
				input = { type: 5, secid: objdata.TRANSACTION_ID }
            Login_Services.POSTENCRYPTAPI("GetTransactionResponse", input, token, function (value) {
                var res = value.data;
                if (res.Status == "100") {
					var data = res.DataList[0];
					console.log(data);
                    scope.TRANSACTION_ID = data.TRANSACTION_ID;
                    scope.URL_DESCRIPTION = data.URL_DESCRIPTION;
                    scope.DISTRICT_NAME = data.DISTRICT_NAME;
                    scope.SECRETARIAT_NAME = data.SECRETARIAT_NAME;
                    scope.INSERTED_DATE = data.INSERTED_DATE;
					scope.SLA = data.SLA.replace('Days','');
                    scope.CurrDate = moment(new Date()).format('DD-MM-YYYY hh:mm A');
                    scope.DEPT_TRANS_ID = data.DEPT_TRANS_ID;
                    scope.MANDAL_NAME = data.MANDAL_NAME;
					scope.username = username;
					scope.Amount = data.AMOUNT=='0'?'15': data.AMOUNT;
					scope.CITIZEN_NAME = data.CITIZEN_NAME;
					scope.Mobile = data.MOBILE_NUMBER;
                    //scope.TRANSACTION_ID = data.TRANSACTION_ID;
                    //scope.TRANSACTION_ID = data.TRANSACTION_ID;
                    //scope.TRANSACTION_ID = data.TRANSACTION_ID;
                    //scope.TRANSACTION_ID = data.TRANSACTION_ID;
                }
                else if (res.Status == "428") {
                    sessionStorage.clear();
                    swal('info', res.Reason, 'info');
                    state.go("Login");
                    return;

                }
                else {
                    swal('info', res.Reason, 'info');
                }
            });
		}

		scope.PrintReceipt = function () {
			
			var divprint = document.getElementById("printdiv").innerHTML;

			var popupWinindow = window.open('', 'Print-Window');
			popupWinindow.document.open();
			popupWinindow.document.write('<html><head><link href="assets/css/bootstrap.css" rel="stylesheet" type="text/css"><link rel="stylesheet" type="text/css" href="assets/css/printcss.css" /></head><body onload="window.print()">' + divprint + '</html>');
			popupWinindow.document.close();
		}
    }
})();