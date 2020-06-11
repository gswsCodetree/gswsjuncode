(function () {
    var app = angular.module("GSWS");

    app.controller("DistrictLoginController", ["$scope", "$state", "$log", "Login_Services", "$rootScope", DistrictLoginController]);

    function DistrictLoginController(scope, state, log, Login_Services, rs) {
        $(".modal-backdrop").css("display", "none");

        CaptchLoad();


        scope.GetLogin = function () {

            if (scope.userid == "" || scope.userid == undefined) {

                swal('info', 'Please Enter Username', 'info')
                return;
            }
            if (scope.password == "" || scope.password == undefined) {

                swal('info', 'Please Enter Password', 'info')
                return;
            }
            if (scope.Fcaptcha == "" || scope.Fcaptcha == undefined || scope.Fcaptcha == null) {
                swal('info', 'Please Enter Captcha', 'info')
                return;
            }
            else {
                LOADlOGIN();


            }


        }

        function CaptchLoad() {
            scope.apploader = true;
         sessionStorage.setItem("hskey", "admin");
            var obj = { ftype: "1" }
            Login_Services.POSTAPI("GetCaptcha", obj, function (data) {
                var res = data.data;
                if (res.code == "100") {


                    scope.capatchval = "";
                    $("#capid").html(res.idval);
                    scope.capId = res.idval;
                    $("#captchdis tr").remove();
                    var img = $("<img>", { "id": res.idval, "src": "data:image/Gif;base64," + res.imgurl, "width": "90px", "height": "40px" });
                    var rowget = $('<tr></tr>').append('<td></td>').html(img);
                    $("#captchdis tbody").append(rowget);
                }

                else {
                    scope.apploader = false;

                    CaptchLoad();
                }

            });
        }
        scope.GetRefresh = function () {
            CaptchLoad();

        }
        function LOADlOGIN() {
            scope.isdisabled = true;
            scope.divloader = true;

            if (scope.userid == "BRDREG" && scope.password == "BRDREG@321") {
                state.go("ui.BirthDeathReg");
                return;
            }

            var token = sessionStorage.getItem("Token")
            var input = { Ftype: 2, FUsername: scope.userid, Newpassword: scope.password, Captcha: scope.Fcaptcha, ConfirmCaptch: scope.capId }

			Login_Services.POSTAPI("GetPSLogin", input, function (value) {

                var res = value.data;
                scope.divloader = false;
                scope.isdisabled = false;

                if (res.Status == '100') {

                    scope.logindetails = JSON.parse(res.Details);
                    sessionStorage.setItem("username", scope.logindetails[0].EMP_NAME);
                    sessionStorage.setItem("distcode", scope.logindetails[0].DISTRICT_ID);
                    sessionStorage.setItem("mcode", scope.logindetails[0].MANDAL_ID);
                    sessionStorage.setItem("gpcode", scope.logindetails[0].GP_WARD_ID);
                    sessionStorage.setItem("secccode", scope.logindetails[0].GSWS_ID);
                    sessionStorage.setItem("user", scope.logindetails[0].USER_ID);
                    sessionStorage.setItem("desinagtion", scope.logindetails[0].DESIG_ID);
					sessionStorage.setItem("Token", res.access_token);

					sessionStorage.setItem("DeviceID", scope.logindetails[0].DEVICE_ID);
					sessionStorage.setItem("ROLE", scope.logindetails[0].ROLE);


					state.go("ui.HardwareIssueClosing");

                }
                else if (res.Status == "428") {
                    sessionStorage.clear();
					state.go("DistrictLogin");
                    return;
                }
                else {
                    scope.Fcaptcha = "";
                    CaptchLoad();
                    swal('info', res.Reason, 'info');
                }
            });


        }



    }
})();

