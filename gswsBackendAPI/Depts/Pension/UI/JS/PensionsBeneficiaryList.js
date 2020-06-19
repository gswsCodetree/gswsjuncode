(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("PensionsBeneficiaryListController", ["$scope", "pensionDeptServices", "$state", beneficiaryList_CTRL]);

    function beneficiaryList_CTRL(scope, ps, $state) {

        scope.token = sessionStorage.getItem("Token");
        scope.BeneficiaryList = '';

        //scope.MonthsDD = [{ 'Code': '01', 'Value': 'January' }, { 'Code': '02', 'Value': 'February' }, { 'Code': '03', 'Value': 'March' }, { 'Code': '04', 'Value': 'April' }, { 'Code': '05', 'Value': 'May' }, { 'Code': '06', 'Value': 'June' },
        //{ 'Code': '07', 'Value': 'July' }, { 'Code': '08', 'Value': 'August' }, { 'Code': '09', 'Value': 'September' }, { 'Code': '10', 'Value': 'October' }, { 'Code': '11', 'Value': 'November' }, { 'Code': '12', 'Value': 'December' }];

        scope.mlist = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

        scope.btnBeneficiaryDetails = function () {
            if (scope.Month == undefined || scope.Month == '' || scope.Month == null) {
                alert('Please Select Month');
                return;
            }
            scope.loader = true;
            var requestData = {

                secId: sessionStorage.getItem("secccode"),
                month: scope.Month

            };
            scope.BeneficiaryList1 = [];
            scope.BeneficiaryList2 = [];
            ps.encrypt_post("PensionBeneficiaryList", requestData, scope.token, function (data) {
                var res = data.data;
                if (res.status) {
                    scope.BeneficiaryList = res.result.details;
                    scope.demoDetails = res.result;
                    for (var i = 0; i < scope.BeneficiaryList.length; i++) {
                        if (i < scope.BeneficiaryList.length / 2) {
                            scope.BeneficiaryList1.push(scope.BeneficiaryList[i]);
                        } else {
                            scope.BeneficiaryList2.push(scope.BeneficiaryList[i]);
                        }
                    }
                    console.log('BeneficiaryList1', scope.BeneficiaryList1);
                    console.log('BeneficiaryList2', scope.BeneficiaryList2);
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

    };

})();