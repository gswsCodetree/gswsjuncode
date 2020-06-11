(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("WCDWAppStatus", ["$scope", "$state", "$log", "Women_Services", WCDW_CTRL]);

    function WCDW_CTRL(scope, state, log, ho_services) {
        scope.pagename = "Women Child";
        scope.preloader = false;
        scope.dttable = false;
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        scope.TransDetails = [];

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

        scope.GetStatus = function () {
            if (!scope.AppNo) {
                swal('info', "Please Enter Application Number/Mobile Number/Aadhaar Number.", 'info');
                return false;
            }

            else {
                scope.preloader = true;
                scope.dttable = false;
                var req = { input: scope.AppNo };
                ho_services.POSTENCRYPTAPI("GetWCDWAppStatus", req, token, function (value) {
                    console.log(value);

                    if (value.data.Status == 100) {
                        var data = value.data.Reason;
                        if (data.GENDER) {
                            scope.TransDetails = [];
                            scope.APPLICATIONID = data["APPLICATION ID"];
                            scope.CITIZENNAME = data["CITIZEN NAME"];
                            scope.GENDER = data["GENDER"];
                            scope.VILLAGE = data["VILLAGE"];
                            scope.MANDAL = data["MANDAL"];
                            scope.DISTRICT = data["DISTRICT"];

                            $.each(data.transactions, function (key, value) {
                                var nobj = { "SERVICENAME": value["SERVICE NAME"], "STATUSMESSAGE": value["STATUS MESSAGE"], "TransactionDate": value["Transaction Date"] }
                                scope.TransDetails.push(nobj);
                            });

                            scope.dttable = true;
                        }
                        else
                            swal('info', data, 'info');
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