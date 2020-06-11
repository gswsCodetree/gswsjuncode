(function () {
    var app = angular.module("GSWS");

    app.controller("BLSVerificationController", ["$scope", "$state", "$log", "SERP_Services", BLSVerification_CTRL]);

    function BLSVerification_CTRL(scope, state, log, SERP_Services) {
		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}
        scope.inputradio = "option1";
        scope.benstatus = [];
        scope.preloader = false;
        scope.divtable = false;
        scope.divvillage = true;
        scope.divsbaccount = false;
        scope.divloanaccount = false;
        LoadDistrics();
        ResetData();
        LoadBanks();

        // Load District's
        function LoadDistrics() {
            scope.DistricsDD = [];
            scope.MandalsDD = [];
            scope.PanchayatDD = [];
            scope.VillageDD = [];
            scope.VillageOrgDD = [];
            scope.SelfHelpDD = [];

            var req = {};
            SERP_Services.POSTENCRYPTAPI("GetAllDistricts", req, token, function (value) {
                if (value.data.Status == 100)
					scope.DistricsDD = value.data.Details.District_DETAILS;
				else if (value.data.Status == "428") {
					swal('info', value.data.Reason, 'info');
					sessionStorage.clear();
					state.go("Login");
					return;
				}
                else
                    swal("Info", value.data.Reason, "info");
            });
        }

        // Load Madals's
        scope.LoadMadals = function () {
            scope.MandalsDD = [];
            scope.PanchayatDD = [];
            scope.VillageDD = [];
            scope.VillageOrgDD = [];
            scope.SelfHelpDD = [];
            if (scope.seldistrict) {
                var req = {
                    District_id: scope.seldistrict
                };
                SERP_Services.POSTENCRYPTAPI("GetAllMandals", req, token, function (value) {
                    if (value.data.Status == 100)
						scope.MandalsDD = value.data.Details.MANDAL_DETAILS;
					else if (value.data.Status == "428") {
						swal('info', value.data.Reason, 'info');
						sessionStorage.clear();
						state.go("Login");
						return;
					}
                    else
                        swal("Failed", value.data.Reason, "info");
                });
            }
        }

        // Load Panchayats's
        scope.LoadPachayats = function () {
            var req = {
                TYPE: "6",
                DISTRICT: scope.seldistrict,
                MANDAL: scope.selmandal,
                RUFLAG: scope.selruflag
            };
            SERP_Services.POSTENCRYPTAPI("LoadDepartments", req, token, function (value) {
                if (value.data.Status == 100) {
                    scope.PanchayatDD = value.data.Details.Details;
				}
				else if (value.data.Status == "428") {
					swal('info', value.data.Reason, 'info');
					sessionStorage.clear();
					state.go("Login");
					return;
				}
                else {
                    swal("Info", value.data.Reason, "info");
                }
            });
        }

        // Load Villages's
        scope.LoadVillages = function () {
            var req = {
                TYPE: "6",
                DISTRICT: scope.seldistrict,
                MANDAL: scope.selmandal,
                RUFLAG: scope.selruflag
            };
            SERP_Services.POSTENCRYPTAPI("LoadDepartments", req, token, function (value) {
                if (value.data.Status == 100) {
                    scope.VillageDD = value.data.Details.Details;
				}
				else if (value.data.Status == "428") {
					swal('info', value.data.Reason, 'info');
					sessionStorage.clear();
					state.go("Login");
					return;
				}
                else {
                    swal("Info", value.data.Reason, "info");
                }
            });
        }

        // Load VillageOrg's
        scope.LoadVillageOrg = function () {
            scope.SelfHelpDD = [];
            if (scope.selmandal) {
                var req = {
                    village_id: scope.seldistrict + '' + scope.selmandal
                };
                SERP_Services.POSTENCRYPTAPI("GetVillageOrganisation", req, token, function (value) {
                    if (value.data.Status == 100)
						scope.VillageOrgDD = value.data.Details.VO_DETAILS;
					else if (value.data.Status == "428") {
						swal('info', value.data.Reason, 'info');
						sessionStorage.clear();
						state.go("Login");
						return;
					}
                    else
                        swal("Info", value.data.Reason, "info");
                });
            }
        }

        // Load HelpGroups's
        scope.LoadHelpGroups = function () {
            if (scope.selvillorg) {
                var req = {
                    Vo_id: scope.selvillorg
                };
                SERP_Services.POSTENCRYPTAPI("LoadHelpGroups", req,token, function (value) {
                    if (value.data.Status == 100)
						scope.SelfHelpDD = value.data.Details.SHG_DETAILS;
					else if (value.data.Status == "428") {
						swal('info', value.data.Reason, 'info');
						sessionStorage.clear();
						state.go("Login");
						return;
					}
                    else
                        swal("Info", value.data.Reason, "info");
                });
            }
        }

        // Load Bank's
        function LoadBanks() {
            var req = {};
            SERP_Services.POSTENCRYPTAPI("LoadaAllBanks", req, token, function (value) {
                if (value.data.Status == 100) {
                    scope.BanksDD = value.data.Details.BANK_DETAILS;
				}
				else if (value.data.Status == "428") {
					swal('info', value.data.Reason, 'info');
					sessionStorage.clear();
					state.go("Login");
					return;
				}
                else {
                    swal("Info", "Banks Loading Failed", "info");
                }
            });
        }

        scope.changeOption = function (value) {
            ResetData();
            if (value == "option1") {
                scope.divvillage = true;
                scope.divsbaccount = false;
                scope.divloanaccount = false;
            }
            else if (value == "option2") {
                scope.divvillage = false;
                scope.divsbaccount = true;
                scope.divloanaccount = false;
            }
            else if (value == "option3") {
                scope.divvillage = false;
                scope.divsbaccount = false;
                scope.divloanaccount = true;
            }
            else {
                scope.divvillage = false;
                scope.divsbaccount = false;
                scope.divloanaccount = false;
            }
        }

        // Get Bank linkage status & verification
        scope.GetLoans = function (gtype) {
            if (gtype == 1) {
                if (!(scope.seldistrict)) {
                    swal("Info", "Please Select District", "info");
                    return false;
                }
                else if (!(scope.selmandal)) {
                    swal("Info", "Please Select Mandal", "info");
                    return false;
                }
                //else if (!(scope.selpanchayath)) {
                //    swal("Info","Please Select Panchayat", "info");
                //    return;
                //} else if (!(scope.selvillage)) {
                //    swal("Info","Please Select Village", "info");
                //    return;
                //}
                else if (!(scope.selvillorg)) {
                    swal("Info", "Please Select Village Organization", "info");
                    return false;
                }
                else if (!(scope.selselfhelp)) {
                    swal("Info", "Please Self Help Group", "info");
                    return false;
                }
            }
            else if (gtype == 2) {
                if (!(scope.selsbbank)) {
                    swal("Info", "Please Select Bank Name", "info");
                    return false;
                }
                else if (!(scope.sbacno)) {
                    swal("Info", "Please Enter Loan Account Number", "info");
                    return false;
                }
            }
            else if (gtype == 3) {
                if (!(scope.selloanbank)) {
                    swal("Info", "Please Select Bank Name", "info");
                    return false;
                }
                else if (!(scope.loanacno)) {
                    swal("Info", "Please Enter Loan Account Number", "info");
                    return false;
                }
            }

            scope.divtable = false;
            scope.preloader = true;


            if (true) {
                scope.preloader = true;
                var req = {}
                if (gtype == 1) {
                    req = { Shg_Id: scope.selselfhelp, SB_Account_No: "", Loan_Account_No: "", Bank_Code: "" }
                }
                else if (gtype == 2) {
                    req = { Shg_Id: "", SB_Account_No: scope.sbacno, Loan_Account_No: "", Bank_Code: scope.selsbbank }
                }
                else if (gtype == 3) {
                    req = { Shg_Id: "", SB_Account_No: "", Loan_Account_No: scope.loanacno, Bank_Code: scope.selloanbank }
                }

                SERP_Services.POSTENCRYPTAPI("GetAllBLLoans", req, token, function (value) {
                    scope.preloader = false;
                    if (value.data.Status == 100) {
                        scope.benstatus = value.data.Details.BLLoansLst;
                        scope.divtable = true;
					}
					else if (value.data.Status == "428") {
						swal('info', value.data.Reason, 'info');
						sessionStorage.clear();
						state.go("Login");
						return;
					}
                    else
                        swal("Info", value.data.Reason, "info");
                });
            }
        }

        // Get YSR Asara Status & Verification
        scope.GetAsaraDetails = function (gtype) {
            if (gtype == 1) {
                if (!(scope.seldistrict)) {
                    swal("Info", "Please Select District", "info");
                    return false;
                }
                else if (!(scope.selmandal)) {
                    swal("Info", "Please Select Mandal", "info");
                    return false;
                }
                //else if (!(scope.selpanchayath)) {
                //    swal("Info","Please Select Panchayat", "info");
                //    return;
                //} else if (!(scope.selvillage)) {
                //    swal("Info","Please Select Village", "info");
                //    return;
                //}
                else if (!(scope.selvillorg)) {
                    swal("Info", "Please Select Village Organization", "info");
                    return false;
                }
                else if (!(scope.selselfhelp)) {
                    swal("Info", "Please Self Help Group", "info");
                    return false;
                }
            }
            else if (gtype == 2) {
                if (!(scope.selsbbank)) {
                    swal("Info", "Please Select Bank Name", "info");
                    return false;
                }
                else if (!(scope.sbacno)) {
                    swal("Info", "Please Enter Loan Account Number");
                    return false;
                }
            }
            else if (gtype == 3) {
                if (!(scope.selloanbank)) {
                    swal("Info", "Please Select Bank Name", "info");
                    return false;
                }
                else if (!(scope.loanacno)) {
                    swal("Info", "Please Enter Loan Account Number", "info");
                    return false;
                }
            }

            scope.divtable = false;
            scope.preloader = true;


            if (true) {
                scope.preloader = true;
                var req = {}
                if (gtype == 1) {
                    req = { Shg_Id: scope.selselfhelp, SB_Account_No: "", Loan_Account_No: "", Bank_Code: "" }
                }
                else if (gtype == 2) {
                    req = { Shg_Id: "", SB_Account_No: scope.sbacno, Loan_Account_No: "", Bank_Code: scope.selsbbank }
                }
                else if (gtype == 3) {
                    req = { Shg_Id: "", SB_Account_No: "", Loan_Account_No: scope.loanacno, Bank_Code: scope.selloanbank }
                }

                SERP_Services.POSTENCRYPTAPI("GetAllYSRLoans", req, token, function (value) {
                    scope.preloader = false;
                    if (value.data.Status == 100) {
                        scope.benstatus = value.data.Details.YSRAssara;
                        scope.divtable = true;
					}
					else if (value.data.Status == "428") {
						swal('info', value.data.Reason, 'info');
						sessionStorage.clear();
						state.go("Login");
						return;
					}
                    else
                        swal("Info", value.data.Reason, "info");
                });
            }
        }

        // Get VLR Status & Verification
        scope.GetVLRDetails = function (gtype) {
            if (gtype == 1) {
                if (!(scope.seldistrict)) {
                    swal("Info", "Please Select District", "info");
                    return false;
                }
                else if (!(scope.selmandal)) {
                    swal("Info", "Please Select Mandal", "info");
                    return false;
                }
                //else if (!(scope.selpanchayath)) {
                //    swal("Info","Please Select Panchayat", "info");
                //    return;
                //} else if (!(scope.selvillage)) {
                //    swal("Info","Please Select Village", "info");
                //    return;
                //}
                else if (!(scope.selvillorg)) {
                    swal("Info", "Please Select Village Organization", "info");
                    return false;
                }
                else if (!(scope.selselfhelp)) {
                    swal("Info", "Please Self Help Group", "info");
                    return false;
                }
            }
            else if (gtype == 2) {
                if (!(scope.selsbbank)) {
                    swal("Info", "Please Select Bank Name", "info");
                    return false;
                }
                else if (!(scope.sbacno)) {
                    swal("Info", "Please Enter Loan Account Number", "info");
                    return false;
                }
            }
            else if (gtype == 3) {
                if (!(scope.selloanbank)) {
                    swal("Info", "Please Select Bank Name", "info");
                    return false;
                }
                else if (!(scope.loanacno)) {
                    swal("Info", "Please Enter Loan Account Number", "info");
                    return false;
                }
            }

            scope.divtable = false;
            scope.preloader = true;


            if (true) {
                scope.preloader = true;
                var req = {}
                if (gtype == 1) {
                    req = { Shg_Id: scope.selselfhelp, SB_Account_No: "", Loan_Account_No: "", Bank_Code: "" }
                }
                else if (gtype == 2) {
                    req = { Shg_Id: "", SB_Account_No: scope.sbacno, Loan_Account_No: "", Bank_Code: scope.selsbbank }
                }
                else if (gtype == 3) {
                    req = { Shg_Id: "", SB_Account_No: "", Loan_Account_No: scope.loanacno, Bank_Code: scope.selloanbank }
                }

                SERP_Services.POSTENCRYPTAPI("GetAllVLRLoans", req, token, function (value) {
                    scope.preloader = false;
                    if (value.data.Status == 100) {
                        scope.benstatus = value.data.Details.VLRLoanslst;
                        scope.divtable = true;
					}
					else if (value.data.Status == "428") {
						swal('info', value.data.Reason, 'info');
						sessionStorage.clear();
						state.go("Login");
						return;
					}
                    else
                        swal("Info", value.data.Reason, "info");
                });
            }
        }

        function ResetData() {
            scope.seldistrict = "";
            scope.selsbbank = "";
            scope.sbacno = "";
            scope.selloanbank = "";
            scope.loanacno = "";
            scope.benstatus = [];
            scope.divtable = false;
            scope.MandalsDD = [];
            scope.PanchayatDD = [];
            scope.VillageDD = [];
            scope.VillageOrgDD = [];
            scope.SelfHelpDD = [];

        }
    }
})();