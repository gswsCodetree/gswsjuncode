(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("KnowYourVolunteer", ["$scope", "$state", "$log", "PRRD_Services", Vol_CTRL]);

    function Vol_CTRL(scope, state, log, vol_services) {
        scope.pagename = "Know Your Volunteer";
        scope.preloader = false;
        scope.dttable = false;
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

        scope.validateaadhaar = function () {
            status = true;
            //if ((scope.student_aadhaar + "").toString().length == "12") {
            //    var aadhaar = scope.student_aadhaar;
            //    status = validateVerhoeff(aadhaar);
            //    if (!status) {
            //        //jQuery("#refresh").trigger("click");
            //        return status;
            //    }
            //}
        }

        scope.GetStatus = function () {
            if (!scope.uidno) {
                swal('info', "Please Enter Aadhaar Number.", 'info');
                return;
            }
            if (scope.uidno.toString().length != "12") {
                swal('info', "Aadhaar Number should be 12 digits.", 'info');
                return;
            }
            else if (scope.uidno == "000000000000" || scope.uidno == "111111111111" || scope.uidno == "222222222222" || scope.uidno == "333333333333" || scope.uidno == "444444444444" || scope.uidno == "555555555555" || scope.uidno == "666666666666" || scope.uidno == "777777777777" || scope.uidno == "888888888888" || scope.uidno == "999999999999") {
                swal('info', "Please Enter Valid Aadhaar Number", 'info');
                return;
            }
            //else if (status != true) {
            //    swal('info',"Please Enter a valid Aadhaar Number");
            //    return;
            //}
            else {
                scope.preloader = true;
                scope.dttable = false;
                var req = { ftype: 2, fuid_num: scope.uidno };
                vol_services.POSTENCRYPTAPI("GetValunteerDetails", req, token, function (value) {
                    scope.StudentDetails = [];
                    if (value.data.Status == 100) {
                        scope.StudentDetails = value.data.Details;
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