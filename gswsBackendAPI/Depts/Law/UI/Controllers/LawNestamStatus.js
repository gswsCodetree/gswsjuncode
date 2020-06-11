(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("LawNestamStatus", ["$scope", "$state", "$log", "Law_Services", Law_CTRL]);

    function Law_CTRL(scope, state, log, law_services) {
        scope.pagename = "Law Nestam";
        scope.preloader = false;
        scope.dttable = false;
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

        scope.GetStatus = function () {
            if (!scope.userinput) {
                swal('info', "Please Enter Application Number.", 'info');
                return false;
            }

            else {
                scope.preloader = true;
                scope.dttable = false;
                var req = { ref_no: scope.userinput };
                law_services.POSTENCRYPTAPI("GetAppStatus", req, token, function (value) {
                    console.log(value);

                    if (value.data.Status == 100) {
                        if (!value.data.Details.pyNote) {

                            scope.ApplicantName = value.data.Details.ApplicantName;
                            scope.mandalname = value.data.Details.mandalname;
                            scope.MunicipalityName = value.data.Details.MunicipalityName;
                            scope.distctname = value.data.Details.distctname;
                            scope.ApplicationID = value.data.Details.ApplicationID;
                            scope.pyStatusWork = value.data.Details.pyStatusWork;
                            console.log(value.data.Details);
                            scope.dttable = true;
                        }

                        else
                            swal('info', value.data.Details.pyNote, 'info');

                    }
                    else if (value.data.Status == "428") {
                        sessionStorage.clear();
                        swal("info", "Session Expired !!!", "info");
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