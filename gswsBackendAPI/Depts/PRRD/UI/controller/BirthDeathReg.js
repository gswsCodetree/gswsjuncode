(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("BirthDeathReg", ["$scope", "$state", "$log", "PRRD_Services", Birth_CTRL]);

    function Birth_CTRL(scope, state, log, Birth_Services) {
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

        scope.otpdiv = true;
        scope.preloader = false;

        LoadDistrics();

        //Submit
        scope.SendOtp = function () {

            if (!(scope.seldistrict)) {
                swal('info', "Please Select District Name", 'info');
                return false;
            }
            else if (!(scope.selmandal)) {
                swal('info', "Please Select Mandal Name", 'info');
                return false;
            }
            else if (!scope.selvillage) {
                swal('info', "Please Select Panchayath Name", 'info');
                return false;
            }
            else if (!(scope.entname)) {
                swal('info', "Please Enter Name", 'info');
                return false;
            }
            else if (!(scope.entmobile)) {
                swal('info', "Please Enter Mobile Number", 'info');
                return false;
            }
            else if (scope.entmobile.lenght < 10) {
                swal('info', "Mobile Number Should be 10 Digits", 'info');
                return false;
            }
            else {
                //scope.preloader = true;
                scope.otpdiv = false;
                swal('Success', "OTP Sent to Mobile Number Successfully", 'success');
                //var req =
                //{

                //}
                //Birth_Services.POSTENCRYPTAPI("SaveServicesURLData", req,token, function (value) {
                //    if (value.data.Status == "Success") {
                //        scope.preloader = false;
                //        scope.benstatus = value.data["Details"];
                //        alert("Data Inserted Successfully");
                //        window.location.reload();
                //    }
                //    else {
                //        scope.preloader = false;
                //        swal('info',"Data Submission Failed");
                //    }
                //});
            }
        }

        scope.VerifyOtp = function () {
            if (!scope.entotp) {
                swal('info', "Please Enter OTP", 'info');
                return false;
            }
            else
            {
                swal('Success', "OTP Verified Successfully", 'success');
                var obj = { DistrictID: scope.seldistrict, RuralFlag: scope.selruflag, MandalID: scope.selmandal, PanchayatID: scope.selvillage, Name: scope.entname, MobileNO: scope.entmobile };
				var encryptdata = "ukBVhopaDWgRGWsoGTfR18C9IkA1yf6QfIXu8ttyCrGCPWNnZxZ2ASbfskBLF0HbPcWcINF2ZqWiVv0ArBXQHxf03yre4SuyXs/sphyqMC629Wtd4/QuZrDxLPIWBxHlwHfXneGfez+dcC/toZz3ckKzdMQxUbkh7qqEhkyCan6W7mic9cWuUBk+n3W0y6GcubDERcz4EupcHO04+4HxAA==&iv=3hfQumfC6jRSodjP";
				var url = "http://prajaasachivalayam.ap.gov.in/Pstestapp/#!/BDReg?ID=" + encryptdata;
                window.open(url, '_blank');
                //state.go("ui.NOCApplication?ID=" + encryptdata);
            }
        }


        // Load District's
        function LoadDistrics() {
            var req = {
                TYPE: "4"
            };
            Birth_Services.POSTENCRYPTAPI("LoadDistricts", req, token, function (value) {
                scope.DistricsDD = [];
                if (value.data.Status == 100) {
                    scope.DistricsDD = value.data.Details;
                }
                else if (value.data.Status == "428") {
                    sessionStorage.clear();
                    swal("info", "Session Expired !!!", "info");
                    state.go("Login");
                    return;
                }
                else {
                    swal('info', "Districs Loading Failed", 'info');
                }
            });
        }

        // Load Madals's
        scope.LoadMadals = function () {
            var req = {
                TYPE: "5",
                DISTRICT: scope.seldistrict,
                RUFLAG: scope.selruflag
            };
            Birth_Services.POSTENCRYPTAPI("LoadDistricts", req, token, function (value) {
                if (value.data.Status == 100) {
                    scope.MandalsDD = value.data.Details;
                }
                else if (value.data.Status == "428") {
                    sessionStorage.clear();
                    swal("info", "Session Expired !!!", "info");
                    state.go("Login");
                    return;
                }
                else {
                    swal('info', "Mandals Loading Failed", 'info');
                }
            });
        }

        // Load Panchayats's
        scope.LoadPachayats = function () {
            var req = {
                TYPE: "6",
                DISTRICT: scope.seldistrict,
                MANDAL: scope.selmandal,
                RUFLAG: scope.selruflag
            };
            Birth_Services.POSTENCRYPTAPI("LoadDistricts", req, token, function (value) {
                if (value.data.Status == 100) {
                    scope.PanchayatDD = value.data.Details;
                }
                else if (value.data.Status == "428") {
                    sessionStorage.clear();
                    swal("info", "Session Expired !!!", "info");
                    state.go("Login");
                    return;
                }
                else {
                    swal('info', "Panchayats Loading Failed", 'info');
                }
            });
        }


    };
})();