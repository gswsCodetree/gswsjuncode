(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("LaborAppStatus", ["$scope", "$state", "$log", "Ser_Services", Lab_CTRL]);

    function Lab_CTRL(scope, state, log, ho_services) {
        scope.pagename = "Labor Application Status";
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
            if (!scope.applicationId) {
                swal('info', "Please Enter Registration Number/Old/Legacy Registration Number/Temp ID.", 'info');
                return false;
            }

            else {
                scope.preloader = true;
                scope.dttable = false;
                var req = { BenID: scope.applicationId };
                ho_services.POSTENCRYPTAPI("GetLaborAppStatus", req, token, function (value) {
                    console.log(value);
                    scope.preloader = false;
                    if (value.data.Status == 100) {
                        var data = value.data.Details;

                        if (data.status == "1") {

                            scope.reg_no = data.reg_no;
                            scope.trade_name = data.trade_name;
                            scope.worker_name = data.worker_name;
                            scope.father_name = data.father_name;
                            scope.genderName = data.genderName;
                            scope.worker_photo = "data:image/jpeg;base64," + data.worker_photo;
                            scope.approvalStatus = data.approvalStatus;
                            scope.dttable = true;
                        }
                        else
                            swal('info', data.msg, 'info');
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