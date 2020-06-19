(function () {

    var app = angular.module("GSWS");
    app.config(["$stateProvider", App_config]);

    function App_config($stateProvider) {
        var web_site = location.hostname;
        var pre_path = "";
        if (web_site !== "localhost") {
            pre_path = "/" + location.pathname.split("/")[1] + "/";
        }
        $stateProvider
            .state("knowYourVolunteer", { url: "/knowYourVolunteer", templateUrl: pre_path + "Depts/knowYourVolunteer/UI/home.html", controller: "KYVController" })
            .state("uc.CitizenVolunteerMapping", { url: "/CitizenVolunteerMapping", templateUrl: pre_path + "Depts/knowYourVolunteer/UI/CitizenVolunteerMapping.html", controller: "CitizenVolunteerMappingController" });
    }

    app.service("KYVServices", ["network_service", KYVServices]);

    function KYVServices(ns, state) {

        var Internal_Services = this;
        var baseurl = "/api/KYV/";

        Internal_Services.post = function (methodname, input, callback) {

            ns.post(baseurl + methodname, input, function (data) {
                callback(data);

            }, function (error) {
                callback(error);
            });
        };

        Internal_Services.encrypt_post = function (methodname, input, token, callback) {

            ns.encrypt_post(baseurl + methodname, input, token, function (data) {
                callback(data);

            }, function (error) {
                callback(error);
            });
        };
    }

    app.controller("KYVController", ["$scope", "KYVServices", KYV_CTRL]);

    function KYV_CTRL(scope, KYVServices) {


        scope.loader = false;
        scope.personDetails = "";
        scope.GrievDetails = "";
        scope.grevReg = false;
        scope.inputEdit = true;
        scope.captchaDiv = false;

        scope.CaptchLoad = function () {
            scope.loader = true;
            scope.Fcaptcha = "";
            var obj = { ftype: "1" };
            KYVServices.post("GetCaptcha", obj, function (data) {
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
                    alert(res.Reason);
                }
                scope.loader = false;
            });
        };

        scope.CaptchLoad();

        scope.GetRefresh = function () {
            scope.CaptchLoad();
        };

        scope.GetRefreshGrev = function () {
            scope.loader = true;
            scope.subCaptcha = "";
            var obj = { ftype: "1" };
            KYVServices.post("GetCaptcha", obj, function (data) {
                var res = data.data;
                if (res.code == "100") {
                    scope.subCaptcha = "";
                    $("#capid").html(res.idval);
                    scope.subCaptchaId = res.idval;
                    $("#Gcaptchdis tr").remove();
                    var img = $("<img>", { "id": res.idval, "src": "data:image/Gif;base64," + res.imgurl, "width": "90px", "height": "40px" });
                    var rowget = $('<tr></tr>').append('<td></td>').html(img);
                    $("#Gcaptchdis tbody").append(rowget);
                }
                else {
                    alert(res.Reason);
                }
                scope.loader = false;
            }, function (error) {
                scope.loader = false;
                console.log(error);
            });
        };

        scope.loadDistricts = function () {

            var input = {
                type: 1
            };

            KYVServices.post("districtList", input, function (value) {
                var res = value.data;
                if (res.success) {
                    scope.districtsList = res.result;
                }
                else {
                    swal('info', res.result, 'info');
                    return;
                }
            }, function (error) {
                console.log(error);
            });
        };

        scope.volunteerDetails = function () {
            scope.personDetails = "";
            if (scope.uidNum == null || scope.uidNum == undefined || scope.uidNum == undefined) {
                alert("Please Enter aadhaar number");
                return;
            }

            if (scope.uidNum.length != 12) {
                alert("Please Enter valid aadhaar number");
                return;
            }

            if (!validateVerhoeff(scope.uidNum)) {
                alert("Please Enter valid aadhaar number");
                return;
            }
            else if (scope.Fcaptcha == "" || scope.Fcaptcha == undefined || scope.Fcaptcha == null) {
                swal('info', 'Please Enter Captcha', 'info');
                return;
            }

            scope.loader = true;
            var requestData = {
                uidNum: scope.uidNum,
                Captcha: scope.Fcaptcha,
                ConfirmCaptch: scope.capId
            };
            KYVServices.post("volunteerDetails", requestData, function (data) {
                var res = data.data;
                if (res.success) {
                    scope.personDetails = res.result[0];
                }
                else if (res.Status == "102") {
                    scope.CaptchLoad();
                    alert(res.result);
                }
                else {
                    scope.loadDistricts();
                    scope.inputEdit = false;
                    scope.grevReg = true;
                    scope.CaptchLoad();
                    alert(res.result);
                }
                scope.loader = false;
            }, function (error) {
                console.log(error);
            });
        };

        scope.ddDistrictChange = function () {
            scope.mandalId = "";
            scope.secId = "";

            if (scope.districtId == '' || scope.districtId == undefined || scope.districtId == null) {
                scope.mandalList = [];
                scope.secList = [];
                alert('Please Select District');
            }

            var input = {
                districtId: scope.districtId
            };

            scope.loader = true;
            KYVServices.post("mandalList", input, function (value) {

                var res = value.data;
                if (res.success) {
                    scope.mandalList = res.result;
                }
                else {
                    alert(res.result);
                }
                scope.loader = false;
            }, function (error) {
                scope.loader = false;
                console.log(error);
            });

        };

        scope.ddMandalChange = function () {

            scope.secId = "";
            if (scope.mandalId == '' || scope.mandalId == undefined || scope.mandalId == null) {
                scope.secList = [];
                alert('Please Select Mandal');
            }

            var input = {
                districtId: scope.districtId,
                mandalId: scope.mandalId
            };

            scope.loader = true;
            KYVServices.post("secList", input, function (value) {

                var res = value.data;
                if (res.success) {
                    scope.secList = res.result;

                }
                else {
                    alert(res.result);
                }
                scope.loader = false;
            }, function (error) {
                scope.loader = false;
                console.log(error);
            });
        };

        scope.ddSecChange = function () {
            scope.GetRefreshGrev();
            scope.captchaDiv = true;
        };

        scope.CreateGrievanceData = function () {
            scope.GrievDetails = '';
            if (ValidateGrievance()) {

                var districtname = $.grep(scope.districtsList, function (b) {
                    return b.LGD_DIST_CODE == scope.districtId;
                })[0].DISTRICT_NAME;
                var mandalname = $.grep(scope.mandalList, function (b) {
                    return b.LGD_MANDAL_CODE == scope.mandalId;
                })[0].MANDAL_NAME;
                var secname = $.grep(scope.secList, function (b) {
                    return b.SECRETARIAT_CODE == scope.secId;
                })[0].SECRETARIAT_NAME;

                var input = {
                    uidNum: scope.uidNum,
                    citizenName: scope.name,
                    gender: scope.gender,
                    districtId: scope.districtId,
                    districtName: districtname,
                    mandalId: scope.mandalId,
                    mandalName: mandalname,
                    secId: scope.secId,
                    secName: secname,
                    mobileNumber: scope.mobileno,
                    Captcha: scope.subCaptcha,
                    ConfirmCaptch: scope.subCaptchaId
                };

                scope.loader = true;
                KYVServices.post("citizenDetailsSub", input, function (value) {

                    var res = value.data;
                    if (res.success) {
                        scope.GrievDetails = res.result[0];
                        scope.grevReg = false;
                    }
                    else {
                        swal('info', res.result, 'info');
                    }
                    scope.loader = false;
                }, function (error) {
                    scope.loader = false;
                    console.log(error);
                });
            }
        };

        function ValidateGrievance() {
            if (scope.districtId == null || scope.districtId == undefined || scope.districtId == undefined) {
                alert("Please select District");
                return false;
            }
            if (scope.mandalId == null || scope.mandalId == undefined || scope.mandalId == undefined) {
                alert("Please select Mandal");
                return;
            }
            if (scope.secId == null || scope.secId == undefined || scope.secId == undefined) {
                alert("Please select Secretariat");
                return;
            }
            if (scope.name == null || scope.name == undefined || scope.name == undefined) {
                alert("Please Enter Citizen Name");
                return;
            }
            if (scope.gender == null || scope.gender == undefined || scope.gender == undefined) {
                alert("Please Select Gender");
                return;
            }
            if (scope.mobileno == "" || scope.mobileno == undefined || scope.mobileno == null || scope.mobileno.length < 10) {
                swal('info', 'Please Enter 10 Digit Mobile Number', 'info');
                return false;
            }
            var reg = new RegExp("^[6-9][0-9]{9}$");
            if (!reg.test(scope.mobileno)) {
                swal('info', 'Please Enter Valid Mobile Number', 'info');
                return false;
            }
            if (scope.mobileno == "1111111111" || scope.mobileno == "2222222222" || scope.mobileno == "3333333333" || scope.mobileno == "4444444444" || scope.mobileno == "5555555555" || scope.mobileno == "6666666666" || scope.mobileno == "7777777777" || scope.mobileno == "8888888888" || scope.mobileno == "9999999999") {
                swal('info', 'Please Enter Valid Mobile Number', 'info');
                return false;
            }
            if (scope.subCaptcha == "" || scope.subCaptcha == undefined || scope.subCaptcha == null) {
                swal('info', 'Please Enter Captcha', 'info');
                return false;
            }

            return true;
        };
    }
})();