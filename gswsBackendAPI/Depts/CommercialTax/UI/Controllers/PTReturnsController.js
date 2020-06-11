(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("PTReturnsController", ["$scope", "$state", "$log", "CT_Services", PTRet_CTRL]);

    function PTRet_CTRL(scope, state, log, ptret_services) {

        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

        scope.TaxPeriodDD = [];
        scope.ProfessionDD = [];
        scope.PayRangeDD = [];
        scope.TotalMTax = 0;
        scope.TotalYTax = 0;
        scope.TaxMTable = [];
        scope.TaxYTable = [];

        scope.preloader = false;
        scope.dttable = false;
        scope.divrettbl = false;
        scope.divmonth = true;

        scope.GetDetails = function () {
            scope.dttable = false;
            if (!scope.entertin) {
                swal('info', "Please Enter TIN Number.", 'info');
                return false;
            }

            else {
                ClearData();
                scope.preloader = true;
                var req = { action: "GETRET", tin: scope.entertin }

                ptret_services.POSTENCRYPTAPI("GetReturnsData", req, token, function (value) {


                    if (value.data.Status == 100) {
                        if (value.data.Details.status_cd == "1") {
                            var decriptdata = window.atob(value.data.Details.data);
                            var JsonData = JSON.parse(decriptdata);
                            //scope.StaDetails = JsonData;
                            console.log(JsonData);

                            var profdata = JsonData.profession_dtls;
                            scope.prof_tin = profdata.prof_tin;
                            scope.enterprise_name = profdata.enterprise_name;
                            scope.circlename = profdata.circle;
                            scope.division = profdata.division;
                            scope.tax_period_type = profdata.tax_period_type;
                            scope.profession_type = profdata.profession_type;

                            if (JsonData.tax_period) {
                                var taxperiod = { tax_periodid: JsonData.tax_period.tax_period, tax_periodname: JsonData.tax_period.tax_period }
                                scope.TaxPeriodDD.push(taxperiod);
                            }

                            if (JsonData.prof_tax_dtls) {
                                //var taxdata = $(ptret_services.ProfessionTypes).filter(function (i, n) { return n.profession_type_code === JsonData.prof_tax_dtls.profession_type_code });
                                var profession = { profession_type_code: JsonData.prof_tax_dtls.profession_type_code, profession_type_name: profdata.profession_type, profession_tax_rate: JsonData.prof_tax_dtls.profession_tax_rate }
                                scope.ProfessionDD.push(profession);
                            }

                            var slabs = JsonData.slab_dtls;
                            if (slabs) {
                                for (i = 0; i < slabs.length; i++) {
                                    var slab = { slab: slabs[i].slab, slab_code: slabs[i].slab_code, slab_taxrate: slabs[i].slab_taxrate }
                                    scope.PayRangeDD.push(slab);
                                }
                            }

                            if (scope.tax_period_type == "Year")
                                scope.divmonth = false;
                            else
                                scope.divmonth = true;

                            scope.dttable = true;
                        }
                        else {
                            swal("Info", value.data.Details.error[0].message, "info");
                        }
                        console.log(value.data);
                    }
                    else if (value.data.Status == "428") {
                        sessionStorage.clear();
                        swal("info", "Session Expired !!!", "info");
                        state.go("Login");
                        return;
                    }
                    else { swal("Info", value.data.Reason, "info"); }

                    scope.preloader = false;

                });
            }

        }

        scope.CalPayble = function () {
            if (scope.EnterEmployees && scope.selpayrange) {
                if (Number(scope.EnterEmployees) > 0) {
                    var obj = $(scope.PayRangeDD).filter(function (i, n) { return n.slab_code === scope.selpayrange });
                    if (obj) {
                        scope.ptaxrate = obj[0].slab_taxrate;
                        scope.ptaxpayble = (scope.EnterEmployees * obj[0].slab_taxrate);
                    }
                    else {
                        scope.ptaxrate = "";
                        scope.ptaxpayble = "";
                    }
                }
                else {
                    scope.ptaxrate = "";
                    scope.ptaxpayble = "";
                }
            }
            else {
                scope.ptaxrate = "";
                scope.ptaxpayble = "";
            }
        }

        scope.GetRate = function () {
            if (scope.selprofession) {
                var obj = $(scope.ProfessionDD).filter(function (i, n) { return n.profession_type_code === scope.selprofession });
                scope.profession_type_rate = obj[0].profession_tax_rate;
            }
            else { scope.profession_type_rate = ''; }
        }

        scope.AddPay = function () {
            if (!scope.selpayrange) {
                swal('info', "Please Select Pay Range.", 'info');
                return false;
            }
            else if (!scope.EnterEmployees) {
                swal('info', "Please Enter No of Employees.", 'info');
                return false;
            }
            else {
                var obj = $(scope.PayRangeDD).filter(function (i, n) { return n.slab_code === scope.selpayrange });
                var payobj = { slab_code: obj[0].slab_code, slab_taxrate: obj[0].slab_taxrate, slab: obj[0].slab, nof_employees: scope.EnterEmployees, tot_payable: scope.ptaxpayble }
                scope.TaxMTable.push(payobj);
                scope.divrettbl = true;
                scope.selpayrange = '';
                scope.ptaxrate = '';
                scope.EnterEmployees = '';
                scope.ptaxpayble = '';

                scope.TotalMTax = scope.TotalMTax + payobj.tot_payable;
            }
        }

        scope.AddProfession = function () {
            if (!scope.selprofession) {
                swal('info', "Please Select Profession.", 'info');
                return false;
            }

            else {
                var obj = $(scope.ProfessionDD).filter(function (i, n) { return n.profession_type_code === scope.selprofession });
                var payobj = { profession_type_code: obj[0].profession_type_code, profession_type_name: obj[0].profession_type_name, profession_tax_rate: obj[0].profession_tax_rate }
                scope.TaxYTable.push(payobj);
                scope.divrettbl = true;
                scope.selprofession = '';
                scope.profession_type_rate = '';

                scope.TotalYTax = Number(scope.TotalYTax) + Number(payobj.profession_tax_rate);
            }
        }

        // Remove Pay Row to table
        scope.removeMPay = function (index) {
            //Find the record using Index from Array.

            if (window.confirm("Do you want to delete record ")) {
                //Remove the item from Array using Index.
                scope.TotalMTax = scope.TotalMTax - scope.TaxMTable[index].tot_payable;
                scope.TaxMTable.splice(index, 1);
            }

            if (scope.TaxMTable.length == 0) {
                scope.divrettbl = false;
                scope.TotalMTax = 0;
            }
        }

        // Remove Pay Row to table
        scope.removeYPay = function (index) {
            //Find the record using Index from Array.

            if (window.confirm("Do you want to delete record ")) {
                //Remove the item from Array using Index.
                scope.TotalYTax = scope.TotalYTax - scope.TaxYTable[index].profession_tax_rate;
                scope.TaxYTable.splice(index, 1);
            }

            if (scope.TaxYTable.length == 0) {
                scope.divrettbl = false;
                scope.TotalYTax = 0;
            }
        }

        scope.SubmitMDetails = function () {
            if (!scope.seltaxperiod) {
                swal('info', "Please Select Tax Period.", 'info');
                return false;
            }
            else if (scope.TaxMTable.length == 0) {
                swal('info', "Please Add Atleast One Paytax Details.", 'info');
                return false;
            }
            else {
                scope.preloader = true;
                var ReturnsData = {
                    "prd_ty": (scope.tax_period_type == "Year" ? "Y" : "M"),
                    "prof_tin": scope.prof_tin,
                    "ret_month": scope.seltaxperiod,
                    "tot_payable": scope.TotalMTax,
                    "empid": "0951357"
                }

                var ReturnsIns = [];

                for (i = 0; i < TaxMTable.length; i++) {
                    var Obj = {};

                    Obj.sal_slab_code = TaxMTable[i].slab_code;
                    Obj.nof_employees = TaxMTable[i].nof_employees;
                    Obj.tot_payable = TaxMTable[i].tot_payable;
                    ReturnsIns.push(Obj);
                }

                var req = {
                    "ret_com": ReturnsData,
                    "retu_ins": ReturnsIns
                };

                var obj = {
                    "action": "INSRET",
                    "data": window.btoa(JSON.stringify(req))
                }

                ptret_services.POSTENCRYPTAPI("SubmitReturnsData", obj, token, function (value) {

                    if (value.data.Status == 100) {
                        if (value.data.Details.status_cd == "1") {

                            swal("Success", "Return ID ." + value.data.Details.return_id, "success");
                            setTimeout(function () { window.location.reload(); }, 5000);

                        }
                        else {
                            var Message = "";
                            for (i = 0; i < value.data.Details.error.length; i++) {
                                Message += value.data.Details.error[i].message + "\n";
                            }
                            swal("Info", Message, "info");
                        }
                        console.log(value.data);
                    }
                    else if (value.data.Status == "428") {
                        sessionStorage.clear();
                        swal("info", "Session Expired !!!", "info");
                        state.go("Login");
                        return;
                    }
                    else { swal("Info", value.data.Reason, "info"); }

                    scope.preloader = false;

                });
            }
        }

        scope.SubmitYDetails = function () {
            if (!scope.seltaxperiod) {
                swal('info', "Please Select Tax Period.", 'info');
                return false;
            }
            else if (scope.TaxYTable.length == 0) {
                swal('info', "Please Add Atleast One Paytax Details.", 'info');
                return false;
            }
            else {
                scope.preloader = true;
                var ReturnsData = {
                    "prd_ty": (scope.tax_period_type == "Year" ? "Y" : "M"),
                    "prof_tin": scope.prof_tin,
                    "ret_month": scope.seltaxperiod,
                    "tot_payable": scope.TotalYTax,
                    "empid": "0951357"
                }


                var req = {
                    "ret_com": ReturnsData
                };

                var obj = {
                    "action": "INSRET",
                    "data": window.btoa(JSON.stringify(req))
                }

                ptret_services.POSTENCRYPTAPI("SubmitReturnsData", obj, token, function (value) {

                    if (value.data.Status == 100) {
                        if (value.data.Details.status_cd == "1") {

                            swal("Success", "Return ID ." + value.data.Details.return_id, "success");
                            setTimeout(function () { window.location.reload(); }, 5000);

                        }
                        else {
                            var Message = "";
                            for (i = 0; i < value.data.Details.error.length; i++) {
                                Message += value.data.Details.error[i].message + "\n";
                            }
                            swal("Info", Message, "info");
                        }
                        console.log(value.data);
                    }
                    else if (value.data.Status == "428") {
                        sessionStorage.clear();
                        swal("info", "Session Expired !!!", "info");
                        state.go("Login");
                        return;
                    }
                    else { swal("Info", value.data.Reason, "info"); }

                    scope.preloader = false;

                });
            }
        }

        function ClearData() {
            scope.TaxPeriodDD = [];
            scope.PayRangeDD = [];
            scope.ProfessionDD = [];
            scope.TotalMTax = 0;
            scope.TotalYTax = 0;
            scope.TaxMTable = [];
            scope.TaxYTable = [];
        }

    }
})();

