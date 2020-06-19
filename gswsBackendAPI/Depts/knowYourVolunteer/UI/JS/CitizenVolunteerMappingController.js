(function () {

    var app = angular.module("GSWS");

    app.controller("CitizenVolunteerMappingController", ["$scope", "KYVServices", CVM_CTRL]);

    function CVM_CTRL(scope, KYVServices) {

        scope.token = sessionStorage.getItem("Token");

        scope.loader = false;
        scope.personDetails = "";

        scope.loadCitizenDetails = function () {

            scope.cluster_id = '';
            scope.status = '';
            scope.citizenDetails = '';

            scope.loader = true;
            var requestData = {
                secId: sessionStorage.getItem("secccode")
            };
            KYVServices.encrypt_post("citizenDetails", requestData, scope.token, function (data) {
                var res = data.data;
                if (res.success) {
                    scope.citizenDetails = res.result;
                    console.log(scope.citizenDetails);
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

        scope.loadCitizenDetails();

        scope.btncitizenChange = function () {
            scope.status = '';
            scope.clusterId = '';
            if (scope.uidNum == '') {
                return;
            }
        }

        scope.loadClusters = function () {

            scope.clusters = '';

            var requestData = {
                gsws_code: sessionStorage.getItem("secccode")
            };
            KYVServices.encrypt_post("loadClusters", requestData, scope.token, function (value) {
                if (value.data.status == 200) {
                    scope.clusters = value.data.result;
                    console.log(scope.clusters);
                }
                else {
                    alert(value.data.result);
                }
            }, function (error) {
                console.log(error);
            });


        };

        scope.btnStatusChange = function () {
            scope.clusterId = '';
            if (scope.status == '') {
                return;
            }
            scope.loadClusters();
        }
        
        scope.citizenUpdate = function () {
            if (scope.uidNum == '' || scope.uidNum == undefined || scope.uidNum == null) {
                alert('Please select Citizen'); return;
            }
            if (scope.status == '' || scope.status == undefined || scope.status == null) {
                alert('Please select action Taken'); return;
            } else {
                if (scope.status == '1') {
                    if (scope.clusterId == '' || scope.clusterId == undefined || scope.clusterId == null) {
                        alert('Please select Cluster'); return;
                    }
                }
                else {
                    scope.clusterId = '';
                }
            }
            var requestData = {
                uidNum: scope.uidNum,
                clusterName: JSON.parse(scope.clusterId).CLUSTE,
                clusterId: JSON.parse(scope.clusterId).CLUSTER_ID,
                status: scope.status,
                updatedBy : sessionStorage.getItem("uniqueid")
            };
            KYVServices.encrypt_post("citizenUpdate", requestData, scope.token, function (value) {
                if (value.data.success) {
                    alert(value.data.result);
                    scope.loadCitizenDetails();
                }
                else {
                    alert(value.data.result);
                }
            }, function (error) {
                console.log(error);
            });
        }
    }

})();