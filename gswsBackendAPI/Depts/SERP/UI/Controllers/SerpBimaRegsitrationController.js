(function () {
    var app = angular.module("GSWS");

    app.controller("SerpBimaRegsitrationController", ["$scope", "$state", "$log", "SERP_Services", SerpBimaRegsitration_CTRL]);

    function SerpBimaRegsitration_CTRL(scope, state, log, SERP_Services) {
        scope.pagename = "SERPBIMA";
		scope.preloader = false;
		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}
        scope.Incidentdayformat = "AM";
        scope.Accidentdayformat = "AM";
        scope.Incidentdayhh = "HH";
        scope.Incidentdaymm = "MM";
        scope.Accidentdayhh = "HH";
        scope.Accidentdaymm = "MM";
        scope.divaccidentdate = true;
        scope.divaccidenttime = true;
        scope.divaccidentplace = true;
        var status = true;
        Districts();
        Claimtypes();
        Incidenttypes();
        EntryBy();

        function Districts() {
            var req = { claimuid: null };
			SERP_Services.POSTENCRYPTAPI("GetDistricts", req, token,function (value) {
                scope.Districtsarray = [];
                if (value.data.Status == "Success") {
                    scope.ddlselectdistrict = value.data.Details;
				}
				else if (value.data.Status == "428") {
					swal('info', value.data.Reason, 'info');
					sessionStorage.clear();
					state.go("Login");
					return;
				}
                else { alert(value.data.Reason); }
            });
        }


        function Claimtypes() {
            var req = { claimuid: null };
			SERP_Services.POSTENCRYPTAPI("GetClaimtypes", req,token, function (value) {
                if (value.data.Status == "Success") {
                    scope.ddlClaimtypes = value.data.Details;
				}
				else if (value.data.Status == "428") {
					swal('info', value.data.Reason, 'info');
					sessionStorage.clear();
					state.go("Login");
					return;
				}
                else { alert(value.data.Reason); }
            });
        }

        function Incidenttypes() {
            var req = { claimuid: null };
			SERP_Services.POSTENCRYPTAPI("GetIncidenttypes", req, token,function (value) {
                if (value.data.Status == "Success") {
                    scope.ddlIncidenttypes = value.data.Details;
				}
				else if (value.data.Status == "428") {
					swal('info', value.data.Reason, 'info');
					sessionStorage.clear();
					state.go("Login");
					return;
				}
                else { alert(value.data.Reason); }
            });
        }

        function EntryBy() {
            var req = { claimuid: null };
			SERP_Services.POSTENCRYPTAPI("GetEntryBy", req, token,function (value) {
                if (value.data.Status == "Success") {
                    scope.ddlEntryby = value.data.Details;
				}
				else if (value.data.Status == "428") {
					swal('info', value.data.Reason, 'info');
					sessionStorage.clear();
					state.go("Login");
					return;
				}
                else { alert(value.data.Reason); }
            });
        }

        scope.districtchange = function () {
            scope.mandal = "";
            scope.grampanchayat = "";
            scope.villagename = "";
            scope.ddlMandals = null;
            scope.ddlgrampanchayt = null;
            scope.ddlvillages = null;
            var req = { DISTRICT_ID: scope.selectdistrict };
			SERP_Services.POSTENCRYPTAPI("GetMandals", req,token, function (value) {
                if (value.data.Status == "Success") {
                    scope.ddlMandals = value.data.Details;
				}
				else if (value.data.Status == "428") {
					swal('info', value.data.Reason, 'info');
					sessionStorage.clear();
					state.go("Login");
					return;
				}
                else { alert(value.data.Reason); }
            });
        }
        scope.changemandal = function () {
            scope.grampanchayat = "";
            scope.villagename = "";
            scope.ddlgrampanchayt = null;
            scope.ddlvillages = null;
            Mandals();
            Villages();
        }

        function Mandals() {
            var req = { DISTRICT_ID: scope.selectdistrict, MANDAL_ID: scope.mandal };
			SERP_Services.POSTENCRYPTAPI("GetPanchayats", req,token, function (value) {
                if (value.data.Status == "Success") {
                    scope.ddlgrampanchayt = value.data.Details;
				}
				else if (value.data.Status == "428") {
					swal('info', value.data.Reason, 'info');
					sessionStorage.clear();
					state.go("Login");
					return;
				}
                else { alert(value.data.Reason); }
            });
        }

        function Villages() {
            var req = { DISTRICT_ID: scope.selectdistrict, MANDAL_ID: scope.mandal };
			SERP_Services.POSTENCRYPTAPI("GetVillages", req, token,function (value) {
                if (value.data.Status == "Success") {
                    scope.ddlvillages = value.data.Details;
				}
				else if (value.data.Status == "428") {
					swal('info', value.data.Reason, 'info');
					sessionStorage.clear();
					state.go("Login");
					return;
				}
                else { alert(value.data.Reason); }
            });
        }

        function AllFieldsclear() {
            scope.Incidentdayformat = "AM";
            scope.Accidentdayformat = "AM";
            scope.Incidentdayhh = "HH";
            scope.Incidentdaymm = "MM";
            scope.Accidentdayhh = "HH";
            scope.Accidentdaymm = "MM";
            scope.divaccidentdate = true;
            scope.divaccidenttime = true;
            scope.divaccidentplace = true;
            scope.claim_aadhaar = "";
            scope.selectdistrict = "";
            scope.claimtype = "";
            scope.cause = "";
            scope.causereason = "";
            scope.Incidentdate = null;
            scope.Incidenttype = "";
            scope.Accidentdate = null;
            scope.Incidentplace = "";
            scope.Accidentplace = "";
            scope.NomPhone = "";
            scope.NomName = "";
            scope.Nominee_aadhaar = "";
            scope.infoby = "";
            scope.infoph = "";
            scope.mandal = "";
            scope.grampanchayat = "";
            scope.villagename = "";
            scope.street = "";
            scope.ward = "";
            scope.doorno = "";
            scope.vdregid = "";
            scope.remarks = "";
            scope.eid = "";
            scope.PHP = "";
            scope.neid = "";
            scope.nomineepin = "";
            scope.vvname = "";
            scope.vvnumber = "";
            scope.vvsid = "";
            scope.vvsloc = "";
            scope.vvspanc = "";
            scope.entryby = "";
        }

        scope.claimtypechange = function () {
            if (scope.claimtype == 2) {
                scope.divaccidentdate = false;
                scope.divaccidenttime = false;
                scope.divaccidentplace = false;
            }
            else {
                scope.divaccidentdate = true;
                scope.divaccidenttime = true;
                scope.divaccidentplace = true;
            }
            scope.cause = "";
            scope.ddlcauses = null;
            var req = { Code: scope.claimtype };
			SERP_Services.POSTENCRYPTAPI("GetCauses", req, token,function (value) {
                if (value.data.Status == "Success") {
                    scope.ddlcauses = value.data.Details;
				}
				else if (value.data.Status == "428") {
					swal('info', value.data.Reason, 'info');
					sessionStorage.clear();
					state.go("Login");
					return;
				}
                else { alert(value.data.Reason); }
            });
        }

        scope.Getsubmit = function () {
            if (scope.claim_aadhaar == "" || scope.claim_aadhaar == null || scope.claim_aadhaar == undefined) {
                alert("Please Enter Aadhaar Number.");
                return;
            }
            else if (scope.selectdistrict == "" || scope.selectdistrict == null || scope.selectdistrict == undefined) {
                alert("Please Select District Name.");
                return;
            }
            else if (scope.claimtype == "" || scope.claimtype == null || scope.claimtype == undefined) {
                alert("Please Select Claim Type.");
                return;
            }
            else if (scope.cause == "" || scope.cause == null || scope.cause == undefined) {
                alert("Please Select Cause.");
                return;
            }
            else if (scope.causereason == "" || scope.causereason == null || scope.causereason == undefined) {
                alert("Please Enter Cause Reason.");
                return;
            }
            else if (scope.Incidentdate == "" || scope.Incidentdate == null || scope.Incidentdate == undefined) {
                alert("Please Select Incident Date.");
                return;
            }
            else if (scope.Incidentdayhh == "" || scope.Incidentdayhh == null || scope.Incidentdayhh == undefined || scope.Incidentdayhh == "HH"
                || scope.Incidentdaymm == "" || scope.Incidentdaymm == null || scope.Incidentdaymm == undefined || scope.Incidentdaymm == "MM"
            ) {
                alert("Please Select Incident Time.");
                return;
            }
            else if (scope.Incidenttype == "" || scope.Incidenttype == null || scope.Incidenttype == undefined) {
                alert("Please Select Incident Type.");
                return;
            }
            else if ((scope.claimtype != 2 && (scope.Accidentdate == "" || scope.Accidentdate == null || scope.Accidentdate == undefined))) {
                alert("Please Select Accident Date.");
                return;
            }
            else if ((scope.claimtype != 2 && (scope.Accidentdayhh == "" || scope.Accidentdayhh == null || scope.Accidentdayhh == undefined || scope.Accidentdayhh == "HH"
                || scope.Accidentdaymm == "" || scope.Accidentdaymm == null || scope.Accidentdaymm == undefined || scope.Accidentdaymm == "MM"))) {
                alert("Please Select Accident Time.");
                return;
            }
            else if (scope.Accidentplace == "" || scope.Accidentplace == null || scope.Accidentplace == undefined) {
                alert("Please Select Accident Place.");
                return;
            }
            else if (scope.Incidentplace == "" || scope.Incidentplace == null || scope.Incidentplace == undefined) {
                alert("Please Select Incident Place.");
                return;
            }
            else if (scope.NomPhone == "" || scope.NomPhone == null || scope.NomPhone == undefined) {
                alert("Please Enter Nominee Phone.");
                return;
            }
            else if (scope.NomName == "" || scope.NomName == null || scope.NomName == undefined) {
                alert("Please Enter Nominee Name.");
                return;
            }
            else if (scope.Nominee_aadhaar == "" || scope.Nominee_aadhaar == null || scope.Nominee_aadhaar == undefined) {
                alert("Please Enter Nominee Aadhaar Number.");
                return;
            }
            else if (scope.infoby == "" || scope.infoby == null || scope.infoby == undefined) {
                alert("Please Enter Informed By.");
                return;
            }
            else if (scope.infoph == "" || scope.infoph == null || scope.infoph == undefined) {
                alert("Please Enter Informed Phone No.");
                return;
            }
            else if (scope.street == "" || scope.street == null || scope.street == undefined) {
                alert("Please Enter Policy holder Street.");
                return;
            }
            else if (scope.ward == "" || scope.ward == null || scope.ward == undefined) {
                alert("Please Enter Policy holder Ward.");
                return;
            }
            else if (scope.doorno == "" || scope.doorno == null || scope.doorno == undefined) {
                alert("Please Enter Policy holder Door No.");
                return;
            }
            else if (scope.vdregid == "" || scope.vdregid == null || scope.vdregid == undefined) {
                alert("Please Enter Village volunteerID/ DigitalAsst ID.");
                return;
            }
            else if (scope.vvname == "" || scope.vvname == null || scope.vvname == undefined) {
                alert("Please Enter Villagevolunteer/DigitalAsst Name.");
                return;
            }
            else if (scope.vvnumber == "" || scope.vvnumber == null || scope.vvnumber == undefined) {
                alert("Please Enter Villagevolunteer/DigitalAsst MobileNo.");
                return;
            }
            else if (scope.vvsid == "" || scope.vvsid == null || scope.vvsid == undefined) {
                alert("Please Enter Village secretariat Id.");
                return;
            }
            else if (scope.vvsloc == "" || scope.vvsloc == null || scope.vvsloc == undefined) {
                alert("Please Enter Village secretariat Location.");
                return;
            }
            else if (scope.vvspanc == "" || scope.vvspanc == null || scope.vvspanc == undefined) {
                alert("Please Enter Village secretariat Panchayat.");
                return;
            }
            else if (scope.entryby == "" || scope.entryby == null || scope.entryby == undefined) {
                alert("Please Select Entry By.");
                return;
            }
            else {
                var req = {
                    Uid: scope.claim_aadhaar, Did: scope.selectdistrict, Incident_Date: scope.Incidentdate, Incident_Time: scope.Incidentdayhh + ':' + scope.Incidentdaymm + ' ' + scope.Incidentdayformat, Accident_Date: scope.Accidentdate, Accident_Time: scope.Accidentdayhh + ':' + scope.Accidentdaymm + ' ' + scope.Accidentdayformat, Claimtype: scope.claimtype, Cause: scope.cause,
                    CauseSubReason: scope.causereason, Nominee_Phone: scope.NomPhone, Nominee_Name: scope.NomName, Nom_Uid: scope.Nominee_aadhaar, Incident_Type: scope.Incidenttype, incident_Place: scope.Incidentplace, Accident_Place: scope.Accidentplace, Informer_Phone: scope.infoph, Informed_By: scope.infoby,
                    Reg_User: scope.vdregid, Remarks: scope.remarks, Street: scope.street, Door_Num: scope.doorno, Ward: scope.ward, Village_Name: scope.villagename, Gram_Panchayat: scope.grampanchayat, Mandal_Name: scope.mandal, EID: scope.eid, Pol_Pincode: scope.PHP,
					Nom_EID: scope.neid, Nom_Pincode: scope.nomineepin, VV_Name: scope.vvname, VV_Phone: scope.vvnumber, VS_id: scope.vvsid, VS_Location: scope.vvsloc, VS_Panchayat: scope.vvspanc, EntryBy: scope.entryby, GSWS_ID: sessionStorage.getItem("TransID")
                };
				SERP_Services.POSTENCRYPTAPI("BimaRegistration", req,token, function (value) {
                    if (value.data.Status == "Success") {
                        if (value.data.Registrationsts == "118") {
                            AllFieldsclear();
                            alert(value.data.Reason);

						}
						else if (value.data.Status == "428") {
							swal('info', value.data.Reason, 'info');
							sessionStorage.clear();
							state.go("Login");
							return;
						}
                        else {
                            alert(value.data.Reason);
                        }
                    }
                    else { alert(value.data.Reason); }
                });
            }
        }

    }
})();