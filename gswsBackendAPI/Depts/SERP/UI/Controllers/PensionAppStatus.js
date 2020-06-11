(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("PensionAppStatus", ["$scope", "$state", "$log", "SERP_Services", PEN_CTRL]);

    function PEN_CTRL(scope, state, log, ho_services) {
        scope.pagename = "Pension Application Status";
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
            if (!scope.districtId) {
                swal('info', "Please Select District.", 'info');
                return false;
            }
            else if (!scope.applicationId) {
                swal('info', "Please Enter Application Number.", 'info');
                return false;
            }

            else {
                scope.preloader = true;
                scope.dttable = false;
                var req = { districtId: scope.districtId, applicationId: scope.applicationId};
                ho_services.POSTENCRYPTAPI("GetPensionAppStatus", req, token, function (value) {
                    console.log(value);
                    scope.preloader = false;
                    if (value.data.Status == 100) {
                        var data = value.data.Details;

                        if (!data.Error) {

                            scope.ApplicationID = data.ApplicationID;
                            scope.CitizenName = data.CitizenName;
                            scope.RelationName = data.RelationName;
                            scope.PensionType = data.PensionType;
                            scope.Gender = data.Gender;
                            scope.Statusmessage = data.Statusmessage;
                            scope.District = data.District;
                            scope.Mandal = data.Mandal;
                            scope.Panchayath = data.Panchayath;
                            scope.dttable = true;
                        }
                        else
                            swal('info', data.Error, 'info');
                    }
                    else if (value.data.Status == "428") {
                        sessionStorage.clear();
                        swal("info", "Session Expired !!!", "info");
                        state.go("Login");
                        return;
                    }
                    else { swal('info', value.data.Reason, 'info'); }

                   

                });

            }
        }
    }
})();