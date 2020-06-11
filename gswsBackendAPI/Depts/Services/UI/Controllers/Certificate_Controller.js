(function () {
	var app = angular.module("GSWS");
	app.controller("CertificateDownlaod", ["$scope", "$state", "Ser_Services", '$sce', Cert_CTRL]);
	function Cert_CTRL(scope, state, Ser_Services, sce) {
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

		scope.detailsshow = false;
		scope.getdetails = function () {
			if (scope.ddltype == undefined || scope.ddltype == null || scope.ddltype == "") {
				alert('Please select certificate type (Income / Integrated).');
				return;
			}
			if (scope.txtcertid == undefined || scope.txtcertid == null) {
				alert('Please enter Certificate ID.');
				return;
			}

			var req = {
				strIntegratedID: scope.txtcertid,
				CertType: scope.ddltype
			};

			Ser_Services.CertificatesDwnlodAPI(req, function (value) {
				var res = value.data;
				scope.Attachment = "";
				if (res.Status == "Success") {
					scope.Status = "Available";
					scope.detailsshow = true;
					var att = res.Data;
					sce.trustAsUrl(att);
					scope.Attachment = att.replace('https','http');
                }
                else if (res.Status == "428") {
                    sessionStorage.clear();
                    swal("info", "Session Expired !!!", "info");
                    state.go("Login");
                    return;
                }
				else {
					scope.Status = "Not Available";
					scope.Attachment = "";
					scope.detailsshow = false;
					alert('No Data Found');
				}

			});
		};

		scope.Refresh = function () {
			location.reload(true);
		}

	}
})();