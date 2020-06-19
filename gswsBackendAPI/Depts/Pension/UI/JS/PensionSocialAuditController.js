(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("PensionSocialAuditController", ["$scope", "pensionDeptServices", "$state", socialAuditList_CTRL]);

    function socialAuditList_CTRL(scope, ps, $state) {

        scope.token = sessionStorage.getItem("Token");
        scope.SocialList = '';


        scope.dataChange = function () {
            scope.SocialList = '';
        }

        scope.btnSocialAuditDetails = function () {
            if (scope.FrmDate == undefined || scope.FrmDate == '' || scope.FrmDate == null) {
                alert('Please Select From Date');
                return;
            }
            if (scope.ToDate == undefined || scope.ToDate == '' || scope.ToDate == null) {
                alert('Please Select To Date');
                return;
            }
            if (scope.Draft == undefined || scope.Draft == '' || scope.Draft == null) {
                alert('Please Select if Draft is eligible/Ineligible');
                return;
            }
            scope.loader = true;
            var requestData = {

                fromDate: scope.getDate(scope.FrmDate),
                toDate: scope.getDate(scope.ToDate),
                secId: sessionStorage.getItem("secccode"),
                eligibleORIneligibleFlag: scope.Draft

            };
            ps.encrypt_post("PensionSocialAuditList", requestData, scope.token, function (data) {
                var res = data.data;
                if (res.status) {
                    scope.SocialList = res.result;
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

        scope.btnPrint = function () {
            window.print();
        };

        scope.getDate = function (dateObj) {
            let month = String(dateObj.getMonth() + 1).padStart(2, '0');;
            let day = String(dateObj.getDate()).padStart(2, '0');
            let year = dateObj.getFullYear();
            return day + "/" + month + '/' + year;
        };

    };

})();