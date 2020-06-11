(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("YSRNavodayamOTRRegController", ["$scope", "$state", "$log", "Industries_Services", Industries_CTRL]);

    function Industries_CTRL(scope, state, log, ind_services) {
        var token = sessionStorage.getItem("Token");
        scope.pagename = "YSR Navodayam OTR Registration";
        scope.preloader = false;

        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

        LoadDistricts();
        LoadSectors();

        scope.LoadMandals = function () {
            if (!scope.seldistrict) { }
            else {
                var req = {
                    username: scope.seldistrict
                };
                ind_services.POSTENCRYPTAPI("LoadMandals", req, token, function (value) {
                    if (value.data.Status == 100) {
                        if (value.data.Data.length > 0)
                            scope.MandalsDD = value.data.Data;
                        else
                            swal("Info", "No Mandal Data Found", "info");
                    }
                    else if (value.data.Status == "428") {
                        sessionStorage.clear();
                        swal("info", "Session Expired !!!", "info");
                        state.go("Login");
                        return;
                    }
                    else
                        swal("Info", value.data.Reason, "info");
                });
            }
        }

        scope.SearchIFSC = function () {
            if (!scope.IFSCCode) { swal("Info", "Please Enter IFSC Code ", "info"); return false; }
            else {
                var req = {
                    username: scope.IFSCCode
                };
                ind_services.POSTENCRYPTAPI("SearchIFSCCode", req, token, function (value) {
                    if (value.data.Status == 100) {
                        if (value.data.Data.length > 0) {
                            scope.selloandistrict = value.data.Data[0]["District_ID"];
                            scope.selbankname = value.data.Data[0]["Bank_Name"];
                            scope.selbranchname = value.data.Data[0]["Branch_Name"];
                        }
                        else {
                            scope.selloandistrict = "";
                            scope.selbankname = "";
                            scope.selbranchname = "";
                            swal("Info", "Invalid IFSC Code ", "info");
                        }
                    }
                    else if (value.data.Status == "428") {
                        sessionStorage.clear();
                        swal("info", "Session Expired !!!", "info");
                        state.go("Login");
                        return;
                    }
                    else
                        swal("Info", value.data.Reason, "info");
                });
            }
        };

        scope.SubmitOTR = function () {
            if (Validation()) {
                var obj = {
                    "Unit_Name": scope.UnitName,
                    "Unit_District_ID": scope.seldistrict,
                    "Unit_Address": scope.UnitLocation,
                    "Mobile_No": scope.selemobileno,
                    "Plant_Machinery_Investment": scope.InvAmount,
                    "Other_Investment": scope.OtherInvAmount,
                    "DOCP": moment(scope.docpdate).format('DD-MMM-YYYY'),
                    "Total_Employment": scope.EmpTotal,
                    "Unit_Category": scope.selcatunit,
                    "Caste": scope.selsoccat,
                    "Sector": scope.selsector,
                    "LOA": scope.lineofactivity,
                    "UAM_No": scope.uamno,
                    "Mandal_ID": scope.selmandal,
                    "EMail_ID": scope.selemail,
                    "Government_Scheme": scope.govscheme,
                    "Monthly_Income": scope.monthlyincome,
                    "IFSC_Code": scope.IFSCCode,
                    "Bank_Name": scope.selbankname,
                    "Branch_Name": scope.selbranchname,
                    "Bank_District_ID": scope.selloandistrict,
                    "Loan_Account": scope.loanacno,
                    "Loan_Amount": scope.loanAmount,
                    "Loan_Date": moment(scope.loandate).format('DD-MMM-YYYY'),
                    "Loan_Type": scope.loantype,
                    "PAN_No": scope.userpan,
                    "UAM_Enclosure": FileUAM,
                    "Other_Enclosure": FileOther,
                }

                ind_services.POSTENCRYPTAPI("SaveOTRReg", obj, token, function (value) {
                    if (value.data.Status == 100) {
                        var datalst = value.data.Data.split("@");
                        if (datalst[0] == "SUCCESS") {
                            swal("Success", "OTR Registered successfully. OTR Number : " + datalst[1], "info");
                            setTimeout(function () { window.location.reload(); }, 3000);
                        }
                        else
                            swal("Failed", value.data.Data, "info");
                    }
                    else if (value.data.Status == "428") {
                        sessionStorage.clear();
                        swal("info", "Session Expired !!!", "info");
                        state.go("Login");
                        return;
                    }
                    else
                        swal("Failed", value.data.Reason, "info");
                });
            }
        }
        
        function LoadDistricts() {
            var req = {};
            ind_services.POSTENCRYPTAPI("LoadDistricts", req, token, function (value) {
                if (value.data.Status == 100) {
                    if (value.data.Data.length > 0)
                        scope.DistrictsDD = value.data.Data;
                    else
                        swal("Info", "No Districts Data Found", "info");
                }
                else if (value.data.Status == "428") {
                    sessionStorage.clear();
                    swal("info", "Session Expired !!!", "info");
                    state.go("Login");
                    return;
                }
                else
                    swal("Info", value.data.Reason, "info");
                
            });
        }

        
        function LoadSectors() {
            var req = {};
            ind_services.POSTENCRYPTAPI("LoadSectors", req, token, function (value) {
                if (value.data.Status == 100) {
                    if (value.data.Data.length > 0)
                    {
                        scope.SectorsDD = value.data.Data;
                        scope.selsector = "0";
                    }
                    else
                        swal("Info", "No Sectors Data Found", "info");
                }
                else if (value.data.Status == "428") {
                    sessionStorage.clear();
                    swal("info", "Session Expired !!!", "info");
                    state.go("Login");
                    return;
                }
                else
                    swal("Info", value.data.Reason, "info");

                
            });
        }

        function Validation() {
            var emailRegex = new RegExp("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");
            if (!scope.UnitName) {
                swal("Info", "Please Enter Name Of Unit ", "info");
                return false;
            }
            else if (!scope.UnitLocation) {
                swal("Info", "Please Enter Location Of Unit ", "info");
                return false;
            }
            else if (!scope.seldistrict) {
                swal("Info", "Please Select District ", "info");
                return false;
            }
            else if (!scope.selmandal) {
                swal("Info", "Please Select Mandal ", "info");
                return false;
            }
            else if (!scope.selemail) {
                swal("Info", "Please Enter Email ", "info");
                return false;
            }
            else if (!(emailRegex.test(scope.selemail))) {
                swal("Info", "Please Enter Valid Email.", "info");
                return false;
            }
            else if (!scope.selemobileno) {
                swal("Info", "Please Enter Mobile Number ", "info");
                return false;
            }
            else if (scope.selemobileno.length < 10) {
                swal("Info", "Mobile Number Should be 10 Digits", "info");
                return false;
            }
            else if (!scope.loanacno) {
                swal("Info", "Please Enter Loan Account Number ", "info");
                return false;
            }
            else if (!scope.IFSCCode) {
                swal("Info", "Please Enter IFSC Code ", "info");
                return false;
            }
            else if (!scope.selloandistrict) {
                swal("Info", "Please Select Bank District ", "info");
                return false;
            }
            else if (!scope.selbankname) {
                swal("Info", "Please Enter Bank Name ", "info");
                return false;
            }
            else if (!scope.selbranchname) {
                swal("Info", "Please Enter Bank Branch ", "info");
                return false;
            }
            else if (!scope.loanAmount) {
                swal("Info", "Please Enter Loan Amount ", "info");
                return false;
            }
            else if (!scope.loandate) {
                swal("Info", "Please Select Loan Date ", "info");
                return false;
            }
            else if (!scope.InvAmount) {
                swal("Info", "Please Enter Invest Amount ", "info");
                return false;
            }
            else if (!scope.OtherInvAmount) {
                swal("Info", "Please Enter Other Investments ", "info");
                return false;
            }
            else if (!scope.docpdate) {
                swal("Info", "Please Select DOCP ", "info");
                return false;
            }
            else if (!scope.EmpTotal) {
                swal("Info", "Please Enter Total Employment ", "info");
                return false;
            }
            else if (!scope.selcatunit) {
                swal("Info", "Please Select Category Of Unit ", "info");
                return false;
            }
            else if (!scope.selsoccat) {
                swal("Info", "Please Select Social Category ", "info");
                return false;
            }
            else if (scope.selsector == "0") {
                swal("Info", "Please Select Sector ", "info");
                return false;
            }
            else if (!scope.lineofactivity) {
                swal("Info", "Please Enter Line OF Activity ", "info");
                return false;
            }
            else if (!scope.uamno) {
                swal("Info", "Please UAM Number ", "info");
                return false;
            }
            else if (!scope.govscheme) {
                swal("Info", "Please Select Govt. Scheme ", "info");
                return false;
            }
            else if (!scope.monthlyincome) {
                swal("Info", "Please Enter Monthly Net Income ", "info");
                return false;
            }

            else if (!scope.userpan) {
                swal("Info", "Please Enter PAN Number ", "info");
                return false;
            }
            else if (!$("#UamUpl").val()) {
                swal("Info", "Please Upload UAM Document ", "info");
                return false;
            }
            //else if (!scope.lineofactivity) {
            //    swal("Info", "Please Enter Line OF Activity ", "info");
            //    return false;
            //}
            //else if (!scope.lineofactivity) {
            //    swal("Info", "Please Enter Line OF Activity ", "info");
            //    return false;
            //}

            return true;
        }
    }
})();