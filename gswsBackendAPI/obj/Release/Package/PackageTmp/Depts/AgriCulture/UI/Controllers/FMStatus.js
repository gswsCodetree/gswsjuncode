(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("FMStatus", ["$scope", "$state", "$log", "AgriCulture_Services", Agri_CTRL]);

    function Agri_CTRL(scope, state, log, fm_services) {
        scope.pagename = "Former Mechanization";
        scope.preloader = false;
        scope.dttable = false;
        scope.StatusCodes = FMStatusCodes;
        var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			state.go("Login");
			return;
		}
        scope.GetStatus = function () {
            if (!scope.app_no) {
                swal('info', "Please Enter Application Number.", 'info');
                return;
            }
            else {
                scope.preloader = true;
                scope.dttable = false;
				var req = { Application: scope.app_no, DistrictCode: sessionStorage.getItem("distcode"), MandalCode: sessionStorage.getItem("mcode"), SceretriatCode: sessionStorage.getItem("secccode"), UrlId: sessionStorage.getItem("urlid"), Loginid:user };
                fm_services.POSTENCRYPTAPIAGRI("GetFMAppStatus", req, token, function (value) {
                    //scope.StudentDetails = [];
                    if (value.data.Status == 100) {
                        scope.FarmerName = value.data.Details.FarmerName;
                        scope.FatherHusbandName = value.data.Details.FatherHusbandName;
                        scope.ImplementSubsidyName = value.data.Details.ImplementSubsidyName;
                        scope.ApplicationRegistrationDate = value.data.Details.ApplicationRegistrationDate;
                        scope.FinYear = value.data.Details.FinYear;
                        scope.ReasonforRejection = value.data.Details.ReasonforRejection;
                        var statusCode = value.data.Details.AplicationStatus.trim();

                        var obj = $(scope.StatusCodes).filter(function (i, n) { return n.Status_Code === statusCode });

                        scope.Status = obj[0].Status_Name;

                        //scope.StudentDetails.push(value.data.Details);
                        
                        scope.dttable = true;
					}
					else if (value.data.Status == "428") {
						sessionStorage.clear();
						swal('info', value.data.Reason, 'info');
						state.go("Login");
						return;
					}
                    else { swal('info', value.data.Reason, 'info'); }

                    scope.preloader = false;

                });

            }
        }

    }
})();