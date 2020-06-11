(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("MeesevaAppStatus", ["$scope", "$state", "$log", "Ser_Services", Services_CTRL]);

    function Services_CTRL(scope, state, log, me_services) {
        scope.pagename = "Meeseva Application Status";
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

            scope.dttable = false;
            if (!scope.appno) {
                swal('info', "Please Enter Application Number.", 'info');
                return false;
            }

            else {
                scope.preloader = true;
                scope.dttable = false;

                var req = { application_number: scope.appno };
                me_services.POSTENCRYPTAPI("GetMeesevaAppStatus", req, token, function (value) {
                    console.log(value);
                    if (value.data.Status == 100) {
                        console.log(value.data.Details);
                        var data = value.data.Details;
                        if (!value.data.Details) { swal('info', "No Data Found", 'info'); }
                        else {
                            scope.APPLICANTNAME = data[0].APPLICANTNAME;
                            scope.REGISTRED_DATE = data[0].REGISTRED_DATE;
                            scope.SERVICE_NAME = data[0].SERVICE_NAME;
                            scope.SECRETARIAT_NAME = data[0].SECRETARIAT_NAME;
                            scope.MANDAL_NAME = data[0].MANDAL_NAME;
                            scope.DISTRICT_NAME = data[0].DISTRICT_NAME;
                            scope.STATUS_MSG = data[0].STATUS_MSG;
                            scope.dttable = true;
                        }
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