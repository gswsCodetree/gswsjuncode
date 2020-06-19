(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("SoftwareIssuesTrackingController", ["$scope", "$state", "$log", "Internal_Services", 'entityService', SoftwareIssuesTrackingController]);

    function SoftwareIssuesTrackingController(scope, state, log, Internal_Services, entityService) {
        scope.preloader = false;

        var username = sessionStorage.getItem("user");
        var distcode = sessionStorage.getItem("distcode");
        var mcode = sessionStorage.getItem("mcode");
        var seccode = sessionStorage.getItem("secccode");
        var token = sessionStorage.getItem("Token");
        var statustype = sessionStorage.getItem("IssueType");

        if (!username) {
            state.go("Login");
        }
       
        GetSolvedIssuesData();

        scope.IssueDetails = function (obj) {
            scope.currticketid = obj.REPORT_ID;
            scope.DEPARTMENT_NAME = obj.DEPARTMENT_NAME;
            scope.REPORT_ID = obj.REPORT_ID;
            scope.SUBJECT_NAME = obj.SUBJECT_NAME;
            scope.SUBSUBJECT_NAME = obj.SUBSUBJECT_NAME;
            scope.MOBILE = obj.MOBILE;
            scope.SOURCE = obj.SOURCE;
            scope.REMARKS = obj.REMARKS;
            scope.IMAGE_URL = obj.IMAGE_URL;
            scope.STATUS = obj.STATUS;
            scope.INSERTED_DATE = obj.INSERTED_DATE;
            scope.CLOSED_DATE = obj.CLOSED_DATE;
            scope.UPDATED_REMARKS = obj.UPDATED_REMARKS;


            $("#issue-modal").modal('show');
        }

        scope.ReopenIssue = function () {
            $("#reopen-modal").modal('show');
        }

        scope.SubmitReopenIssue = function () {
            if (!scope.ReRemarks) {
                swal("info", "Please Enter Re-open Remarks.", "info");
                return false;
            }

            var req = { USER: username, REMARKS: scope.ReRemarks, SOURCE: "3", REPORTID: scope.REPORT_ID }
            Internal_Services.POSTENCRYPTAPI("SaveReOpenTicket", req, token, function (value) {
                if (value.data.Status == "100") {
                    swal('info', value.data.Reason, 'info');
                    window.location.reload();
                    return;
                }
                else {
                    swal('info', value.data.Reason, 'error');
                }
            });
        }
        
        function GetSolvedIssuesData() {
            var req = {
                TYPE: "1", //Software
                UpdatedBy: username,
                SECRETARIAT: seccode,
                ACTIVE_STATUS: statustype
            };
            Internal_Services.DemoAPI("GetHWSWResolvedIssues", req, function (value) {
                if (value.data.Status == "Success") {
                    scope.ResolvedIssuesData = value.data.Details;
                }
                else {
                    swal("info", "No Data Found", "info");
                }
            });
        }

        //Excel Download

        scope.ExcelDownload = function (e) {
            $("#ISSUESTBL").table2excel({
                name: "Worksheet Name",
                exclude: ".image",
                filename: scope.table
            });
        }

    };
})();