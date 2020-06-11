(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("VolunteerMapping", ["$scope", "$state", "$log", "PRRD_Services", Vol_CTRL]);

    function Vol_CTRL(scope, state, log, vol_services) {
        scope.pagename = "Volunteer Mapping Details";
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
            if (!scope.volunteerid) {
                swal('info', "Please Enter Volunteer ID.", 'info');
                return false;
            }
            else {
                scope.preloader = true;
                scope.dttable = false;
                var req = { ftype: 1, fvv_id: scope.volunteerid };
                vol_services.POSTENCRYPTAPI("GetValunteerDetails", req, token, function (value) {
                    scope.StudentDetails = [];
                    if (value.data.Status == 100) {
                        scope.VolDetails = value.data.Details;
                        console.log(value.data.Details);
                        scope.dttable = true;
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