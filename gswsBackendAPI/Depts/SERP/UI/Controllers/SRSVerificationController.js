(function () {
    var app = angular.module("GSWS");

    app.controller("SRSVerificationController", ["$scope", "$state", "$log", "SERP_Services", SRSVerification_CTRL]);

    function SRSVerification_CTRL(scope, state, log, SERP_Services) {
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
        scope.divreg = false;
        scope.eligible = "";

        LoadDistrics();
        LoadProjectCategory();
        LoadRequestType();

        // Load District's
        function LoadDistrics() {
            scope.DistricsDD = [];
            var req = {};
            SERP_Services.POSTENCRYPTAPI("GetSRSDistricts", req, token, function (value) {
                if (value.data.Status == 100)
					scope.DistricsDD = value.data.Details.DISTRICT_DETAILS;
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

        function LoadProjectCategory() {
            var req = {};
            SERP_Services.POSTENCRYPTAPI("GetProjectCategory", req, token, function (value) {
                if (value.data.Status == 100)
					scope.ProCatDD = value.data.Details.ProjectCategory;
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

        function LoadRequestType() {
            var req = {};
            SERP_Services.POSTENCRYPTAPI("GetRequestType", req, token, function (value) {
                if (value.data.Status == 100)
					scope.ReqTypeDD = value.data.Details.RequestType;
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
            ResetData('district');

            if (scope.seldistrict) {
                var req = {
                    DISTRICT_ID: scope.seldistrict
                };
                SERP_Services.POSTENCRYPTAPI("GetSRSMandals", req, token, function (value) {
                    if (value.data.Status == 100)
						scope.MandalsDD = value.data.Details.MANDAL_DETAILS;
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

        // Load VillageOrg's
        scope.LoadVillageOrg = function () {
            ResetData('mandal');
            if (scope.selmandal) {
                var req = {
                    DISTRICT_ID: scope.seldistrict,
                    MANDAL_ID: scope.selmandal
                };
                SERP_Services.POSTENCRYPTAPI("GetSRSVillageOrganisation", req, token, function (value) {
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
            ResetData('villageorg');
            if (scope.selvillorg) {
                var req = {
                    VO_ID: scope.selvillorg
                };
                SERP_Services.POSTENCRYPTAPI("GetSRSHelpGroups", req, token, function (value) {
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

        // Load Members's
        scope.LoadMembers = function () {
            ResetData('helpgroup');
            if (scope.selselfhelp) {
                var req = {
                    SHG_ID: scope.selselfhelp
                };
                SERP_Services.POSTENCRYPTAPI("GetMemberDetails", req, token, function (value) {
                    if (value.data.Status == 100)
						scope.MembersDD = value.data.Details.MEMBER_DETAILS;
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

        // Load Major Activities's
        scope.LoadMajorAct = function () {
            ResetData('procat');
            if (scope.selprocat) {
                var req = {
                    MAJOR_ACTIVITY_TYPE: "Y",
                    PROJECT_CATEGORY: scope.selprocat,
                    MAJOR_ACTIVITY: ""
                };
                SERP_Services.POSTENCRYPTAPI("GetActivityDetails", req, token, function (value) {
                    if (value.data.Status == 100)
						scope.MajorActDD = value.data.Details.Activity;
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

        // Load Minor Activities's
        scope.LoadMinorAct = function () {
            if (scope.selmajoract) {
                var req = {
                    MAJOR_ACTIVITY_TYPE: "N",
                    PROJECT_CATEGORY: scope.selprocat,
                    MAJOR_ACTIVITY: scope.selmajoract
                };
                SERP_Services.POSTENCRYPTAPI("GetActivityDetails", req, token, function (value) {
                    if (value.data.Status == 100)
						scope.MinorActDD = value.data.Details.Activity;
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

        scope.ChangeMember = function () {
            ResetData('member');
        }

        scope.GetStatus = function () {
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
            else if (!(scope.selmembers)) {
                swal("Info", "Please Select Member", "info");
                return false;
            }
            else if (!(scope.selprocat)) {
                swal("Info", "Please Select Project Category", "info");
                return false;
            }
            //else if (!(scope.selmajoract)) {
            //    swal("Info","Please Select Major Activity", "info");
            //    return false;
            //}
            //else if (!(scope.selminoract)) {
            //    swal("Info","Please Select Minor Activity", "info");
            //    return false;
            //}

            scope.divtable = false;
            scope.divreg = false;
            scope.preloader = true;

            var req = { SHG_ID: scope.selselfhelp, MEMBER_ID: scope.selmembers, PROJECT_CATEGORY: scope.selprocat }

            SERP_Services.POSTENCRYPTAPI("GetEligibleDetails", req, token, function (value) {
                console.log(value.data);
                if (value.data.Status == 100) {
                    scope.eligible = value.data.Details.ELIGIBLE_DETAILS["Eligible"];
                    if (scope.eligible == "Eligible") {
                        scope.divreg = true;
                    }
                    //swal("Info",value.data.ELIGIBLE_DETAILS["Eligible"], "info");
                    //scope.benstatus = value.data.BLLoansLst;
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

                scope.preloader = false;
            });
        }

        scope.LoanRequest = function () {
            if (!(scope.seldistrict)) {
                swal("Info", "Please Select District", "info");
                return false;
            }
            else if (!(scope.selmandal)) {
                swal("Info", "Please Select Mandal", "info");
                return false;
            }
            else if (!(scope.selvillorg)) {
                swal("Info", "Please Select Village Organization", "info");
                return false;
            }
            else if (!(scope.selselfhelp)) {
                swal("Info", "Please Self Help Group", "info");
                return false;
            }
            else if (!(scope.selmembers)) {
                swal("Info", "Please Select Member", "info");
                return false;
            }
            else if (!(scope.selprocat)) {
                swal("Info", "Please Select Project Category", "info");
                return false;
            }
            else if (!(scope.selmajoract)) {
                swal("Info", "Please Select Major Activity", "info");
                return false;
            }
            else if (!(scope.selminoract)) {
                swal("Info", "Please Select Minor Activity", "info");
                return false;
            }
            else if (!(scope.loanamount)) {
                swal("Info", "Please Enter Loan Amount", "info");
                return false;
            }
            else if (!(scope.mobileno)) {
                swal("Info", "Please Enter Mobile Number", "info");
                return false;
            }

            scope.preloader = true;

            var req = { VO_ID: scope.selselfhelp, SHG_ID: scope.selmembers, MEMBER_ID: scope.selprocat, PROJECT_CATEGORY: scope.selprocat, ACTIVITY_CODE: scope.selprocat, ACTIVITY_NAME: scope.selprocat, LOAN_AMOUNT: scope.selprocat, MOBILE_NO: scope.selprocat, CREATED_BY: "Prajja Sachivalayam", REGISTRATION_CODE: "010101" }

            SERP_Services.POSTENCRYPTAPI("LoanRequestInsert", req, token, function (value) {
                scope.preloader = false;
                if (value.data.Status == 100) {
                    console.log(value.data.Details.LoanStatus);
                    if (value.data.Details.LoanStatus["Status"] == "Success!") {
                        window.location.reload();
                        swal("Info", "Loan Request Successfully.", "info");
                    }
                    else
                        swal("Info", value.data.Details.LoanStatus["Message"], "info");
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

        function ResetData(type) {
            scope.divtable = false;
            scope.divreg = false;
            scope.loanamount = "";
            scope.mobileno = "";
            scope.MinorActDD = [];

            if (type == "district") {
                scope.selmandal = "";
                scope.selvillorg = "";
                scope.selselfhelp = "";
                scope.selmembers = "";
                scope.selprocat = "";
                scope.selmajoract = "";
                scope.selminoract = "";

                scope.MandalsDD = [];
                scope.VillageOrgDD = [];
                scope.SelfHelpDD = [];
                scope.MembersDD = [];
                scope.MajorActDD = [];
            }
            else if (type == "mandal") {
                scope.selvillorg = "";
                scope.selselfhelp = "";
                scope.selmembers = "";
                scope.selprocat = "";
                scope.selmajoract = "";
                scope.selminoract = "";

                scope.VillageOrgDD = [];
                scope.SelfHelpDD = [];
                scope.MembersDD = [];
                scope.MajorActDD = [];
            }
            else if (type == "villageorg") {
                scope.selselfhelp = "";
                scope.selmembers = "";
                scope.selprocat = "";
                scope.selmajoract = "";
                scope.selminoract = "";

                scope.SelfHelpDD = [];
                scope.MembersDD = [];
                scope.MajorActDD = [];
            }
            else if (type == "helpgroup") {
                scope.selmembers = "";
                scope.selprocat = "";
                scope.selmajoract = "";
                scope.selminoract = "";

                scope.MembersDD = [];
                scope.MajorActDD = [];
            }
            else if (type == "procat") {
                scope.selmajoract = "";
                scope.selminoract = "";

                scope.MajorActDD = [];
            }
            else if (type == "member") {
                scope.selprocat = "";
                scope.selmajoract = "";
                scope.selminoract = "";
            }
        }
    }
})();