(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("HWIssueClosingCntrl", ["$scope", "$state", "$log", "Internal_Services", 'entityService', HWIssueClosingCntrl]);

	function HWIssueClosingCntrl(scope, state, log, Internal_Services, entityService) {
		scope.preloader = false;

		scope.username = sessionStorage.getItem("user");
		scope.distcode = sessionStorage.getItem("distcode");
		scope.mcode = sessionStorage.getItem("mcode");
		scope.seccode = sessionStorage.getItem("secccode");
		scope.token = sessionStorage.getItem("Token");

		scope.deviceid = sessionStorage.getItem("DeviceID");
		scope.role = sessionStorage.getItem("ROLE");

		Checksessionexpire();
		GetData();

		//Get Data
		function GetData() {
			var input = {
				ROLE: scope.role,
				ASSET: scope.deviceid,
				DID: scope.distcode
			}
			Internal_Services.POSTENCRYPTAPI("GetHWCategoriesData", input, scope.token, function (data) {
				var res = data.data;
				if (res.Status == "Success") {
					scope.ActiveIssuesData = res.Details;
				}
				else {
					swal("", res.Reason,"error");
				}
			});
		}

		//Check Session Expire
		function Checksessionexpire() {
			if (!(scope.token)) {
				swal({
					title: "OOPS!",
					text: "Session expired..!",
					icon: "error"
				})

					.then((value) => {
						if (value) {
							state.go("DistrictLogin");
						}
					});
			}
		}

		//Reason Capture to close an issue
		scope.CaptureReasonClick = function (obj) {
			$("#ReasonCaptureModel").modal('show');
			scope.TrackID = obj.REPORT_ID;
			scope.txtReason = "";
		};

		//Click on Report id click
		scope.ClickReportID = function (obj) {
			$("#ReportIDModal").modal('show');
			scope.TrackID = obj.REPORT_ID;
			scope.ModelRemarks = obj.REMARKS;
			scope.ModelImage = obj.IMAGE_URL;
			scope.txtReason = "";
		};

		//Close Issue
		scope.ReasonSaveClick = function (TrackID, ReasonData) {

			if (ReasonData == null || ReasonData == undefined || ReasonData == "") {
				swal('info', 'Enter Reason.', 'info');
				return;
			} if (TrackID == null || TrackID == undefined || TrackID == "" || TrackID == 0) {
				state.go("DistrictLogin");
				return;
			}

			// alert(TrackID + "," + ReasonData);
			var Final_Data = TrackID + "," + ReasonData;

			var req = {
				Type: "6",
				DID: scope.distcode,
				CategoryID: "",
				UpdatedBy: scope.username,
				Reason: ReasonData,
				UniqueID: TrackID
			};
			Internal_Services.POSTENCRYPTAPI("GetCategoriesData", req, scope.token, function (value) {
				if (value.data.Status == "Success") {
					var UpdateStatus = value.data.Details;
					if (UpdateStatus == "TRUE") {
						scope.txtReason = "";
						GetData();//Loading data Again
						$("#ReasonCaptureModel").modal('hide');
						swal("",'Reason updated successfully.',"");
						return;
					}
					scope.txtReason = "";
					$("#ReasonCaptureModel").modal('show');
					swal("",'Failed to update reasaon. Try agian.',"");
				}
				else {
					swal('info', 'Failed to update reasaon. Try agian.', 'fail');
				}
			});
		};

	};


})();