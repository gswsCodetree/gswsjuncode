(function () {
    var app = angular.module("GSWS");
    app.controller("ImamAndMouzans", ["$scope", "Minority_Services", '$sce', ImamAndMouzansCall]);
    function ImamAndMouzansCall(scope, Minority_Services, sce) {
		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}
        //scope.detailsshow = false;
        scope.getdetails = function () {
            
            if (scope.txtBCode == undefined || scope.txtBCode == null) {
                alert('Please enter BeneficiaryCode .');
                return;
            }

            var objCert = {
                BeneficiaryCode: scope.txtBCode,
            };
			
			Minority_Services.POSTENCRYPTAPI("GetHonorariumToImamAndMouzansDetails", objCert,token, function (value) {
                var res = value.data;

                if (res.Status == "Success") {
                    scope.Status = "Available";
                    scope.resdata = res.data;
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
                    scope.detailsshow = false;
                    alert('No Data Found');
                }

            });
        };

    }
})();