(function () {
    var app = angular.module("GSWS");
    app.controller("TransportDrinkingWater", ["$scope", "PRRD_Services", '$sce', TransportDrinkingWaterCall]);

    function TransportDrinkingWaterCall(scope, PRRD_Services, sce) {

        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

        scope.detailsshow = false;

        var obj = {}
        PRRD_Services.POSTENCRYPTAPI("TransportDrinkingWater", obj, token, function (value) {
            var res = value.data;

            if (res.Status == "Success") {
                scope.Status = "Available";
                if (res.data.DATA != null && res.data.DATA.length != 0) {
                    scope.resdata = res.data.DATA;
                    scope.detailsshow = true;
                }
                else {
                    scope.Status = "Not Available";
                    scope.detailsshow = false;
                    alert('Today No Transport provided for drinking water');
                }
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



})();