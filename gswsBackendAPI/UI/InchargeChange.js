(function () {
    var app = angular.module("GSWS");

    app.controller("InchargeChange", ["$scope", "$state", "$log", "Login_Services", Main_CTRL]);

    function Main_CTRL(scope, state, log, Login_Services) {
        scope.isRole = false;
        scope.Preloader = false;
        var username = sessionStorage.getItem("user");
        var secid = sessionStorage.getItem("secccode");
        var token = sessionStorage.getItem("Token");
        var desingation = sessionStorage.getItem("desinagtion");
        if (!username || !token) {

            state.go("Login");
            return;
        }

		if (desingation == "101" || desingation == "1") {
            scope.UserID = secid + "-DA";
            scope.USERTYPE = "1";
            scope.DisplayUserID = scope.UserID;
        }
        else if (desingation == "201") {
            scope.UserID = secid + "-WEDS";
            scope.USERTYPE = "2";
            scope.DisplayUserID = scope.UserID;
        }

        if (scope.UserID) {
            LoadRoles();
        }
        
        //LoadReceivedData();

        scope.SendPassword = function () {
            scope.isRole = false;
            if (scope.SelRole) {
                var input = { P_TYPE: 2, ROLL_ID: scope.SelRole, SCRT_ID: secid, USERTYPE: scope.USERTYPE }
                Login_Services.POSTENCRYPTAPI("InchargeChangeAPI", input, token, function (value) {
                    var res = value.data;
                    console.log(res);
                    if (res.Status == 100) {
                        scope.DisplayMobileNumber = res.DataList;
                        scope.MobileNumber = scope.DisplayMobileNumber;
                        scope.Password = res.Password;
                        scope.isRole = true;
                        swal('info', res.Reason, 'info');
                    }
                    else if (res.Status == "428") {
                        sessionStorage.clear();
                        swal('info', res.Reason, 'info');
                        state.go("Login");
                        return;
                    }
                    else {
                        swal('info', res.Reason, 'info');
                    }

                });
            }
        }

        scope.SaveDetails = function () {
           
            if (!scope.UserID) {
                swal('info', "Please Enter User ID", 'info'); return false;
            }
            else if (!scope.SelRole) {
                swal('info', "Please Select Role", 'info'); return false;
            }
            else if (!scope.MobileNumber) {
                swal('info', "Please Enter Mobile Number", 'info'); return false;
            }
            else if (!scope.FromDate) {
                swal('info', "Please Select From Date", 'info'); return false;
            }
            else if (!scope.ToDate) {
                swal('info', "Please select To Date", 'info'); return false;
            }
            else {
                scope.Preloader = true;
                var haspwd = sha256_digest(scope.Password);

                var input = { P_TYPE: 3, USER_ID: scope.UserID, PASSWORD: haspwd, MOBILE_NO: scope.MobileNumber, FROMDATE: moment(scope.FromDate).format('DD-MMM-YYYY'), TODATE: moment(scope.ToDate).format('DD-MMM-YYYY'), ROLL_ID: scope.SelRole, ENTRY_BY: scope.username, SCRT_ID: secid }
                Login_Services.POSTENCRYPTAPI("InchargeChangeAPI", input, token, function (value) {
                    scope.Preloader = false;
                    var res = value.data;
                    console.log(res);
                    if (res.Status == 100) {
                        swal('info', "Request Inserted Successfully.", 'info');
                        window.location.reload();
                    }
                    else if (res.Status == "428") {
                        sessionStorage.clear();
                        swal('info', res.Reason, 'info');
                        state.go("Login");
                        return;
                    }
                    else {
                        swal('info', res.Reason, 'info');
                    }
                    
                });
            }
            
        }

        scope.getclose = function () {
            $("#myModal12").modal('hide');
        }

        scope.AddRemarks = function () {
            if (!scope.remarks) {
                swal('info', "Please Enter Remarks", 'info');
                return false;
            }
            else {
                var input = { type: 4, transid: sessionStorage.getItem("rtransid"), remarks: scope.remarks, savetype: sessionStorage.getItem("rsavetype") }
                Login_Services.POSTENCRYPTAPI("SaveReceiveAction", input, token, function (value) {
                    var res = value.data;
                    if (res.Status == 100) {
                        swal('Info', "Remarks Added Successfully.", 'info');
                        $("#myModal12").modal('hide');
                    }
                    else {
                        swal('info', res.Reason, 'info');
                    }

                });
            }
        }

        function LoadRoles() {

            var input = { P_TYPE: 1, USER_ID: username }
            Login_Services.POSTENCRYPTAPI("InchargeChangeAPI", input, token, function (value) {
                var res = value.data;
                console.log(res);
                if (res.Status == 100) {
                    scope.RolesDD = res.DataList;
                }
                else if (res.Status == "428") {
                    sessionStorage.clear();
                    swal('info', res.Reason, 'info');
                    state.go("Login");
                    return;
                }
                else {
                    swal('info', res.Reason, 'info');
                }

            });
        }

    }
})();