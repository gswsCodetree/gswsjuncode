(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("HSitesAppStatus", ["$scope", "$state", "$log", "Housing_Services", House_CTRL]);

    function House_CTRL(scope, state, log, ho_services) {
        scope.pagename = "Housing Sites";
        scope.preloader = false;
        scope.dttable = false;
        scope.StatusList = HSStatus;
        scope.Genders = [{ "ID": "M", "Name": "Male" }, { "ID": "F", "Name": "Female" }, { "ID": "O", "Name": "Others" }]

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

                var req = { AppNo: scope.appno };
                ho_services.POSTENCRYPTAPI("GetHSitesAppStatus", req, token, function (value) {
                    console.log(value);
                    if (value.data.Status == 100) {
                        console.log(value.data.Details);
                        var data = jQuery.parseJSON(value.data.Details);
                        if (data.status == "1") {
                            swal('info', data.remarks, 'info');
                        }

                        else {
                            scope.ApplicantName = data[0].ApplicantName;
                            scope.Address = data[0].Address;
                            scope.RelativeName = data[0].RelativeName;
                            scope.Religion = data[0].Religion;
							scope.Status = data[0].APPStatus;
                            var obj3 = $(scope.Genders).filter(function (i, n) { return n.ID === data[0].Gender });
                            if (obj3[0])
                                scope.Gender = obj3[0]["Name"];

                           // var obj2 = $(scope.StatusList).filter(function (i, n) { return n.Status_Type_ID === data[0].AppStatus.toString() });
                            //if (obj2[0])
                              //  scope.Status = obj2[0]["Status_Type_Name"];

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