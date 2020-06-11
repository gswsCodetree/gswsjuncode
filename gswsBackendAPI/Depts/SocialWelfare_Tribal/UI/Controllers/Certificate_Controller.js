(function () {
	var app = angular.module("GSWS");
	app.controller("CertificateDownlaod", ["$scope", "Socialwelfare_Services", '$sce', Cert_CTRL]);
	function Cert_CTRL(scope, socialwelfare_services, sce) {

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

			socialwelfare_services.DemoAPI(req, function (value) {
				var res = value.data;

				if (res.Status == "Success") {
					scope.Status = "Available";
					scope.detailsshow = true;
					var att = res.Data;
					sce.trustAsUrl(att);
					scope.Attachment = att;
				}
				else {
					scope.Status = "Not Available";
					scope.detailsshow = false;
					alert('No Data Found');
				}

			});
		};
		scope.name = "vamsi";

	}
})();