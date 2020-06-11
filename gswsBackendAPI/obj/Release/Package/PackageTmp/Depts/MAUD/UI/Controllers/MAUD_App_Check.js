(function () {
	/* eslint-disable */
	var app = angular.module("GSWS");
	app.controller("MAUD_ApplicationCheck", ["$scope", "$state","MAUD_Services", '$sce', Cert_CTRL]);
	function Cert_CTRL(scope,state,services, sce) {
		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}
		scope.detailsshow = false;
		scope.getdetails = function () {

			if (!(scope.ddlfiletype)) {
				alert('Please select Application type.');
				return;
			}
			if (!(scope.txtcertid)) {
				alert('Please enter Applicant No / Temp No.');
				return;
			}
			var input;
			if (scope.ddlfiletype == "Temp") {
				input = "Temp/" + scope.txtcertid;
			}
			else
				input = scope.txtcertid;
			var req = {
				AppID: input
			};

			services.Application_Status(req,token, function (value) {
				var res = value.data;

				if (res.Status == "success") {
					scope.resdata = res.Data[0];
					scope.detailsshow = true;
					//alert(scope.resdata.FileNo);
				}
				else {
					scope.Status = "Not Available";
					scope.resdata = "";
					scope.detailsshow = false;
					alert(res.Reason);
				}

			});
		};

		scope.Refresh = function () {
			location.reload(true);
		}


	}
})();


