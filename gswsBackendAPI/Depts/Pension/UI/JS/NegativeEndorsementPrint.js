(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("NegativeEndorsementPrintController", ["$scope", "pensionDeptServices",  NegativeEndorsement_CTRL]);


    function NegativeEndorsement_CTRL(scope, ps) {

        scope.token = sessionStorage.getItem("Token");
        scope.EndorsList = '';
        scope.negativeEndorse = true;

        scope.transactionId = getParameterByName('txnId');
        scope.grievanceId = getParameterByName('grevId');

        if (scope.transactionId != "" && scope.transactionId != null && scope.transactionId != undefined || scope.grievanceId != "" && scope.grievanceId != null && scope.grievanceId != undefined) {
            console.log(scope.transactionId, scope.grievanceId);
        }
        else {
            alert("Invalid Request !!!");
            window.open("https://gramawardsachivalayam.ap.gov.in/GSWS/Home/Main", "_Self");
            return;
        }

        scope.loadNegativeEndorsements = function () {
           // scope.negativeEndorse = false;
            scope.loader = true;
            var requestData = {
                grievanceId: scope.grievanceId,
                transactionId: scope.transactionId
            };
            ps.encrypt_post("NegativeEndorsementList", requestData, scope.token, function (data) {
                var res = data.data;
                if (res.status) {
                    scope.resultdata = res.result;
                    scope.negativeEndorse = true;
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
        scope.loadNegativeEndorsements();

        scope.btnNePrint = function () {
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