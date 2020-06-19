(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("PensionEndorsementController", ["$scope", "pensionDeptServices", "$state", pensionEndorsement_CTRL]);


    function pensionEndorsement_CTRL(scope, ps, $state) {

        let dateObj = new Date();
        let month = String(dateObj.getMonth() + 1).padStart(2, '0');;
        let day = String(dateObj.getDate()).padStart(2, '0');
        let year = dateObj.getFullYear();
        scope.todayDate = day + "-" + month + '-' + year;

        scope.token = sessionStorage.getItem("Token");
        scope.EndorsList = '';
        scope.negativeEndorse = false;
        scope.sanctionOrder = false;


        scope.loadEndorsements = function () {
            scope.loader = true;
            var requestData = {

                loginId: sessionStorage.getItem("user"),
                secretariatCode: sessionStorage.getItem("secccode")

            };
            ps.encrypt_post("EndorsementList", requestData, scope.token, function (data) {
                var res = data.data;
                if (res.status) {
                    scope.EndorsList = res.result;
                }
                else {
                    alert(res.resultData);
                }
                scope.negativeEndorse = false;
                scope.loader = false;
            }, function (error) {
                scope.loader = false;
                console.log(error);
            });
        };
        scope.loadEndorsements();

        scope.btnReject = function (obj) {
            var url = $state.href('uc.NegativeEndorsementPrint');
            window.open(url + "?txnId=" + obj.transaction_Id + "&grevId=" + obj.grievance_Id, '_blank');
        };

        scope.btnSanction = function (obj) {
            var url = $state.href('uc.SanctionOrderPrint');
            window.open(url + "?txnId=" + obj.transaction_Id + "&grevId=" + obj.grievance_Id, '_blank');
        };
    }

})();