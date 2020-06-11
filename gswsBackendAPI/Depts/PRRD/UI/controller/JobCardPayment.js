(function () {
    var app = angular.module("GSWS");
	app.controller("JobCardPaymentDetails", ["$scope", "$state", "PRRD_Services", '$sce', JobCardPaymentDetailsCall]);

    function JobCardPaymentDetailsCall(scope,state, PRRD_Services, sce) {
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

        //scope.detailsshow = false;
        scope.getdetails = function () {

            if (scope.txtjobCardNumber == undefined || scope.txtjobCardNumber == null) {
                alert('Please enter Job Card Number.');
                return;
            }
            if (scope.ddlSearchby == undefined || scope.ddlSearchby == null) {
                alert('Please Select Search By.');
                return;
            }
            
            if (scope.ddlSearchby == 1) {

                var objCert = {
                    jobCardNo: scope.txtjobCardNumber

                };

                PRRD_Services.POSTENCRYPTAPI("PostData_Basic_Auth_PaymentDetails", objCert, token, function (value) {
                    var res = value.data;

                    if (res.Status == "Success") {
                        scope.Status = "Available";
                        scope.resdata = res.data.paymentDetails;
                        scope.detailsshow = true;
                        scope.detailsshowUID = false;
                    }
                    else if (res.Status == "428") {
                        sessionStorage.clear();
                        swal("info", "Session Expired !!!", "info");
                        state.go("Login");
                        return;
                    }

                    else {
                        scope.Status = "Not Available";
                        scope.resdata = "";
                        scope.detailsshow = false;
                        alert('No Data Found');
                    }

                });
            }
            else {
                var objCert = {
                    uidNumber: scope.txtjobCardNumber
                    
                };

                PRRD_Services.POSTENCRYPTAPI("PostData_Basic_Auth_PaymentDetails_BY_UID", objCert, token, function (value) {
                    var res = value.data;

                    if (res.Status == "Success") {
                        scope.Status = "Available";
                        scope.resdata = res.data.uidDetails;
                        scope.detailsshowUID = true;
                        scope.detailsshow = false;
                    }
                    else if (res.Status == "428") {
                        sessionStorage.clear();
                        swal("info", "Session Expired !!!", "info");
                        state.go("Login");
                        return;
                    }

                    else {
                        scope.Status = "Not Available";
                        scope.resdata = "";
                        scope.detailsshow = false;
                        alert('No Data Found');
                    }

                });
            }
        };



    }
})();