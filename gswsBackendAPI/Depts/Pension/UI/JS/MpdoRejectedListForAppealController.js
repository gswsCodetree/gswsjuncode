(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("MpdoRejectedListForAppealController", ["$scope", "pensionDeptServices", "$state", rejectedList_CTRL]);


    function rejectedList_CTRL(scope, ps, $state) {

        scope.token = sessionStorage.getItem("Token");
        scope.RejectList = '';
      
        scope.loadRejectedList = function () {
            scope.loader = true;
            var requestData = {

                loginId: sessionStorage.getItem("user"),
                secretariatCode: sessionStorage.getItem("secccode")

            };
            ps.encrypt_post("MpdoRejectedList", requestData, scope.token, function (data) {
                var res = data.data;
                if (res.status) {
                    scope.RejectList = res.result;
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
        scope.loadRejectedList();

        scope.btnViewAppealList = function (txnId, grevId) {
            $state.go('uc.rejectedIndividualData', { txnId: txnId, grevId: grevId });
        };
       }

})();