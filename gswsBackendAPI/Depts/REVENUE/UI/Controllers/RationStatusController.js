(function () {
	var app = angular.module("GSWS");

	app.controller("RationStatusController", ["$scope", "$state", "$log", "REVENUE_Services", Ration_CTRL]);

	function Ration_CTRL(scope, state, log, REVENUE_Services) {
		scope.preloader = false;
		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}

		scope.getdata = function () {

			scope.divtable = false;
			if (!scope.uid) {
				alert("Please Enter Document ID");
				return;
			}
			else {
				scope.preloader = true;
				var req = {
					UID: scope.uid,
				};

				REVENUE_Services.POSTREVENCRYPTAPI("GetApplicantStatus", req, token, function (value) {
					scope.preloader = false;
					var cdata = value.data;
					if (cdata.Status == 100) {
						if (cdata.Details.Data.length > 0) {
							scope.divtable = true;
							var rdata = cdata.Details.Data;

							scope.DOC_NO = rdata[0].DOC_NO;
							scope.FAMILY_ID = rdata[0].FAMILY_ID;
							scope.SUBJECT = rdata[0].SUBJECT;
							scope.APP_STATUS = rdata[0].APP_STATUS;

							scope.bentable = rdata;

						}
						else {
							swal('info', "No Data Found", 'info');
						}

					}
					else if (cdata.Status == "428") {
						swal('info', value.data.Reason, 'info');
						sessionStorage.clear();
						state.go("Login");
						return;
					}

					else {
						swal('info', value.data.Reason, 'info');
					}
				});
			}
		}
	}
})();

