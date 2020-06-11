(function () {
    var app = angular.module("GSWS");
    app.controller("WomenDivorced", ["$scope", "Minority_Services", '$sce', WomenDivorcedCall]);
    function WomenDivorcedCall(scope, Minority_Services, sce) {
		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}
        //scope.detailsshow = false;
        scope.getdetails = function () {
            
            if (scope.txtmcno == undefined || scope.txtmcno == null) {
                alert('Please enter MCNO.');
                return;
            }

            var objCert = {
                MCNO: scope.txtmcno,
            };

			Minority_Services.POSTENCRYPTAPI("GetWomenDivorcedDetails",objCert,token, function (value) {
                var res = value.data;

                if (res.Status == "Success") {
                    scope.Status = "Available";
                    scope.resdata = res.data[0];
                    scope.detailsshow = true;
				}
				else if (res.Status == "428") {
					swal('info', res.Reason, 'info');
					sessionStorage.clear();
					state.go("Login");
					return;
				}
                else {
                    scope.Status = "Not Available";
                    scope.resdata = "";
                    scope.detailsshow = false;
                    alert('No Data Found');
                }

            });
        };

    }
})();