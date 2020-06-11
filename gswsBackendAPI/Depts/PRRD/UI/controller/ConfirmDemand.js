(function () {
    var app = angular.module("GSWS");
	app.controller("ConfirmDemand", ["$scope", "$state", "PRRD_Services", '$sce', ConfirmDemandCall]);

    function ConfirmDemandCall(scope,state, PRRD_Services, sce) {
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

        scope.detailsshow = false;

        scope.getdetails = function () {

            if (scope.txrtNumber == undefined || scope.txrtNumber == null) {
                alert('Please enter Sequnce UID.');
                return;
            }
            
            var objdata = {
                SeqID: scope.txrtNumber,
                ReceiptNo: scope.txrtNumber,
                Flag:"Y"
            };

            PRRD_Services.POSTENCRYPTAPI("ConfirmDemand", objdata, token, function (value) {
                var res = value.data;

                if (res.Status == "Success") {
                    scope.Status = "Available";
                    scope.status = res.data.Remarks;
                    scope.detailsshow = true;
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

    app.filter('unique', function () {
        return function (collection, keyname) {
            var output = [],
                keys = []
            found = [];

            if (!keyname) {

                angular.forEach(collection, function (row) {
                    var is_found = false;
                    angular.forEach(found, function (foundRow) {

                        if (foundRow == row) {
                            is_found = true;
                        }
                    });

                    if (is_found) { return; }
                    found.push(row);
                    output.push(row);

                });
            }
            else {

                angular.forEach(collection, function (row) {
                    var item = row[keyname];
                    if (item === null || item === undefined) return;
                    if (keys.indexOf(item) === -1) {
                        keys.push(item);
                        output.push(row);
                    }
                });
            }

            return output;
        };
    });
})();