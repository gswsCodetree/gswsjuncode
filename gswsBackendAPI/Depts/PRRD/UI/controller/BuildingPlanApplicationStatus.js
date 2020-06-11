(function () {
    var app = angular.module("GSWS");
	app.controller("BuildingPlanApplicationStatus", ["$scope", "$state", "PRRD_Services", '$sce', BuildingPlanApplicationStatusCall]);

    function BuildingPlanApplicationStatusCall(scope,state, PRRD_Services, sce) {

        scope.detailsshow = false;


        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

        scope.getdetails = function () {

            if (scope.txrtNumber == undefined || scope.txrtNumber == null) {
                alert('Please enter Application ID.');
                return;
            }
            
            var objdata = {
                getStatus: "1",
                id: scope.txrtNumber,
            };

            PRRD_Services.POSTENCRYPTAPI("BuildingPlanApplicationStatus", objdata, token, function (value) {
                var res = value.data;

                if (res.Status == "Success") {
                    if (res.data.error == null) {
                        scope.Status = "Available";
                        scope.applicationSubmittedDate = res.data.applicationSubmittedDate;
                        scope.applicationStage = res.data.applicationStage;
                        scope.applicationStatus = res.data.applicationStatus;
                        scope.paymentStatus = res.data.paymentStatus;
                        scope.downloadPlan = res.data.downloadPlan;
                        scope.downloadProceedings = res.data.downloadProceedings;
                        scope.detailsshow = true;
                    }
                    else { alert('Invalid Application ID'); scope.detailsshow = false; }
                }
                else if (res.Status == "428") {
                    sessionStorage.clear();
                    swal("info", "Session Expired !!!", "info");
                    state.go("Login");
                    return;
                }
                else {
                    scope.Status = "Not Available";
                    scope.detailsshow = false;
                    alert('No Data Found');
                }

            });
        };
    }

    
})();