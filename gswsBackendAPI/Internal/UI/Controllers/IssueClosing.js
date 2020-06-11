(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("IssueClosingController", ["$scope", "$state", "$log", "Internal_Services", 'entityService', IssueClosingController]);

    function IssueClosingController(scope, state, log, Internal_Services, entityService) {
        scope.preloader = false;

        var username = sessionStorage.getItem("user");
        var distcode = sessionStorage.getItem("distcode");
        var mcode = sessionStorage.getItem("mcode");
        var seccode = sessionStorage.getItem("secccode");
        var token = sessionStorage.getItem("Token");

        username = "user";

        if (!username) {
            state.go("Login");
        }
        scope.ddlCategory = true;
        scope.btnGetData = true;
        scope.tableDisplay = true;

        // //Type of Issue Change Event
        scope.ChangeIssueType = function () {
            scope.tableDisplay = true;
            if (scope.selIssueType == "" || scope.selIssueType == undefined || scope.selIssueType == null) {
                scope.CategoryData = "";
                scope.ddlCategory = true;
                scope.btnGetData = true;
                swal('info', 'Please select type of Issue.', 'info');
                return;
            }
            if (distcode == "" || distcode == undefined || distcode == null) {
                state.go("Login");
            }

            var req = {
                Type: scope.selIssueType,
                DID: distcode,
                CategoryID: 0,
                UpdatedBy: "",
                Reason: "",
                UniqueID: 0
            };
            Internal_Services.POSTENCRYPTAPI("GetCategoriesData", req, token, function (value) {
                if (value.data.Status == "Success") {
                    scope.CategoryData = value.data.Details;
                    scope.ddlCategory = false;
                }
                else {
                    alert("Categories Loading Failed");
                }
            });
        }

        //Category Change Event
        scope.CategoryChange = function () {
            scope.tableDisplay = true;
            scope.btnGetData = false;           
        }

        //Load Active Issues Data
        //2-Sowftware,1-Hardware,3-Software Details,4-Hardware details
        scope.GetActiveIssuesData = function () {
            if (scope.selIssueType == "" || scope.selIssueType == undefined || scope.selIssueType == null) {
                scope.CategoryData = "";
                scope.ddlCategory = true;
                scope.btnGetData = true;
                swal('info', 'Please select type of Issue.', 'info');
                return;
            }
            if (distcode == "" || distcode == undefined || distcode == null) {
                state.go("Login");
            }
            if (scope.selCategory == "" || scope.selCategory == undefined || scope.selCategory == null) {
                scope.btnGetData = true;
                swal('info', 'Please select Category.', 'info')
                return;
            }

            IssuedetailsCalling();

        }

        function IssuedetailsCalling() {
            var req = {
                Type: (scope.selIssueType == 2 ? 3 : (scope.selIssueType == 1 ? 4 : 0)),
                DID: distcode,
                CategoryID: scope.selCategory,
                UpdatedBy: "",
                Reason: "",
                UniqueID: 0
            };


            Internal_Services.POSTENCRYPTAPI("GetCategoriesData", req, token, function (value) {
                if (value.data.Status == "Success") {
                    scope.ActiveIssuesData = value.data.Details;
                    scope.tableDisplay = false;
                }
                else {
                    ActiveIssuesData = "";
                    scope.tableDisplay = true;
                    swal('info', 'No Data Found.', 'info');
                }
            });
        }

        //Reason Capture
        scope.CaptureReasonClick = function (obj) {
            $("#ReasonCaptureModel").modal('show');
            scope.TrackID = obj.REPORT_ID;
            scope.txtReason = "";
        };

        //Reason Save
        scope.ReasonSaveClick = function (TrackID, ReasonData) {

            if (ReasonData == null || ReasonData == undefined || ReasonData == "") {
                swal('info', 'Enter Reason.', 'info');
                return;
            } if (TrackID == null || TrackID == undefined || TrackID == "" || TrackID == 0) {
                state.go("Login");
                return;
            }

            // alert(TrackID + "," + ReasonData);
            var Final_Data = TrackID + "," + ReasonData;

            var req = {
                Type: (scope.selIssueType == 2 ? 5 : (scope.selIssueType == 1 ? 6 : 0)),
                DID: distcode,
                CategoryID: scope.selCategory,
                UpdatedBy: username,
                Reason: ReasonData,
                UniqueID: TrackID
            };
            Internal_Services.POSTENCRYPTAPI("GetCategoriesData", req, token, function (value) {
                if (value.data.Status == "Success") {
                    var UpdateStatus = value.data.Details;
                    if (UpdateStatus == "TRUE") {
                        scope.txtReason = "";
                        IssuedetailsCalling();//Loading data Again
                        $("#ReasonCaptureModel").modal('hide');
                        alert('Reason updated successfully.');
                        return;
                    }
                    scope.txtReason = "";
                    $("#ReasonCaptureModel").modal('show');
                    alert('Failed to update reasaon. Try agian.');
                }
                else {
                    swal('info', 'Failed to update reasaon. Try agian.', 'fail');
                }
            });
        };






    };


})();