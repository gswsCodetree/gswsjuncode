(function () {
    var app = angular.module("GSWS");
    app.controller("TaxAppStatus", ["$scope", "$state", "PRRD_Services", '$sce', TaxAppStatusCall]);

    function TaxAppStatusCall(scope, state, PRRD_Services, sce) {
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

        LoadPanchayats();

        scope.detailsshow = false;
        scope.getdetails = function () {
            if (!scope.ddlPanchayat) {
                swal("info", 'Please Select Panchayat.', "info");
                return;
            }
            else if (!scope.ddlAppType) {
                swal("info", 'Please Select Application Type.', "info");
                return;
            }
            else if (!scope.AppNO) {
                swal("info", 'Please Enter Application Number.', "info");
                return;
            }

            var objCert = {
                Secccode: scope.ddlPanchayat,
                AppType: scope.ddlAppType,
                AppNo: scope.AppNO,
            };

            scope.detailsshow = false;

            PRRD_Services.POSTENCRYPTAPI("GetTaxAppStatus", objCert, token, function (value) {
                var res = value.data;

                if (res.Status == "Success") {

                    if (res.data.status == 200) {
                        scope.detailsshow = true;

                        var currdata = res.data.details;

                        scope.dept_application_id = currdata.dept_application_id;
                        scope.service_name = currdata.service_name;
                        scope.message = currdata.message;
                        scope.citizen_name = currdata.citizen_name;
                        scope.gender = currdata.gender;
                        scope.district = currdata.district;
                        scope.mandal = currdata.mandal;
                        scope.panchayat = currdata.panchayat;
                        scope.closing_date = currdata.closing_date;
                    }
                    else {
                        swal("info", res.data.msg, "info");
                    }
                }
                else if (res.Status == "428") {
                    sessionStorage.clear();
                    swal("info", "Session Expired !!!", "info");
                    state.go("Login");
                    return;
                }
                else {
                    swal("info", res.data, "info");
                    scope.detailsshow = false;
                }

            });
        };

        function LoadPanchayats() {
            var objCert = {
                Secccode: sessionStorage.getItem("secccode"),
                UserId: sessionStorage.getItem("user"),
                SacId: sessionStorage.getItem("secccode"),
                DesignId: sessionStorage.getItem("desinagtion")
            };

            PRRD_Services.POSTENCRYPTAPI("fetchPanchayats", objCert, token, function (value) {
                var res = value.data;
                if (res.Status == "Success") {
                    var data = res.data;
                    if (data.status == "200") { scope.PanchayatList = data.data; }
                    else { swal("info", data.msg, "info"); }
                }
                else if (res.Status == "428") {
                    sessionStorage.clear();
                    swal("info", "Session Expired !!!", "info");
                    state.go("Login");
                    return;
                }
                else {
                    alert('No Panchayat Data Found');
                }

            });
        }

    }

})();