(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("SanctionOrderPrintController", ["$scope", "pensionDeptServices",  SanctionOrder_CTRL]);


    function SanctionOrder_CTRL(scope, ps) {

        let dateObj = new Date();
        let month = String(dateObj.getMonth() + 1).padStart(2, '0');;
        let day = String(dateObj.getDate()).padStart(2, '0');
        let year = dateObj.getFullYear();
        scope.todayDate = day + "-" + month + '-' + year;

        scope.token = sessionStorage.getItem("Token");
        scope.EndorsList = '';
        scope.sanctionOrder = false;

        scope.transid = getParameterByName('txnId');
        scope.grievanceId = getParameterByName('grevId');

        if (scope.transid != "" && scope.transid != null && scope.transid != undefined || scope.grievanceId != "" && scope.grievanceId != null && scope.grievanceId != undefined) {
            console.log(scope.transid, scope.grievanceId);
        }
        else {
            alert("Invalid Request !!!");
            window.open("https://gramawardsachivalayam.ap.gov.in/GSWS/Home/Main", "_Self");
            return;
        }

        scope.loadSanctionOrder = function () {
           scope.sanctionOrder = false;
            scope.negativeEndorse = false;
            scope.loader = true;
            var requestData = {
                grievanceId: scope.grievanceId,
                transid: scope.transid
            };
            ps.encrypt_post("SanctionOrderList", requestData, scope.token, function (data) {
                var res = data.data;
                if (res.status) {
                    scope.resultSOdata = res.result;
                    scope.sanctionOrder = true;
                }
                else {
                    alert(res.resultData);
                }
                scope.loader = false;
            }, function (error) {
                scope.loader = false;
                console.log(error);
            });
        };
        scope.loadSanctionOrder();

        scope.btnPrint = function () {
            window.print();
        }
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

})();