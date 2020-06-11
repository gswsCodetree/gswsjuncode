(function () {


	var app = angular.module("GSWS");
	app.controller("issueTrackingController", ["$scope", "$state", "$log", "Internal_Services", Internal_CTRL]);

	function Internal_CTRL(scope, state, log, Internal_Services) {

		scope.callcenter = true;
		scope.username = "user";

		var image_value = "";
		scope.preloader = true;
		
		var req =
		{
			SECRETARIAT_ID: "11090984"
		};
		Internal_Services.DemoAPI("IssuesTrackingReport", req, function (value) {
			if (value.data.Status == "Success") {
				scope.data = value.data.Details;
				//console.log("IssuesTrackingReport", scope.data);
			}
			else {
				alert("Report Loading Failed");
			}
			scope.preloader = false;
		});



		scope.loadComments = function (id) {

		
			scope.commentPreloader = true;
			var req =
			{
				REPORT_ID: id
			};
			Internal_Services.DemoAPI("IssuesTrackingComments", req, function (value) {
				scope.report_data = value.data.Details;
				$("#CommentsModel").modal('show');
				console.log('IssuesTrackingComments', scope.report_data);
				scope.commentPreloader = false;
			});
		};

		scope.showDetails = function (obj) {
			$("#CommentsModel").modal('show');
			scope.issue_data = {
				DEPT_NAME: obj.SD_NAME,
				HOD_NAME: obj.HOD_NAME,
				IMAGE_1: obj.IMAGE1_URL,
				IMAGE_2: obj.IMAGE2_URL,
				IMAGE_3: obj.IMAGE3_URL,
				REMARKS: obj.REMARKS
			};
			scope.loadComments(obj.REPORT_ID);
			scope.REPORT_ID = obj.REPORT_ID;

		};

		scope.imageView = function (image) {
			Swal.fire({
				//title: 'Sweet!',
				//text: 'Modal with a custom image.',
				imageUrl: image,
				imageWidth: 800,
				imageHeight: 400,
				imageAlt: 'Issue Image'
			});
		};


		scope.addComment = function (report_id) {

			if (scope.issue_cmt == "" || scope.issue_cmt == null || scope.issue_cmt == undefined) {
				Swal.fire("Info", "Please Enter Your Comment", "info");
				return;
			}

			if (scope.issue_status == "" || scope.issue_status == null || scope.issue_status == undefined) {
				Swal.fire("Info", "Please Select Status", "info");
				return;
			}


			var req = {
				comment: scope.issue_cmt,
				issue_status: scope.issue_status,
				image: image_value,
				report_id: report_id,
				username: scope.username,
				message_flag: scope.callcenter
			};

			Internal_Services.DemoAPI("commentAddition", req, function (value) {
				if (value.data.Status == 200) {
					scope.loadComments(report_id);	
					scope.issue_cmt = "";
					scope.issue_status = "";
					image_value = "";
					document.getElementById("img_path").value = "";
				}
				else {
					alert("Failed to comment, Please Try Again!!!");
				}
			});

			console.log("REQ : ",req);

		};



		scope.onLoad_image = function (e, reader, file, fileList, fileOjects, fileObj) {

			var file_size = fileObj.filesize;
			if (file_size < 2048000) {
				image_value = fileObj.base64;
			}
			else {
				alert("image file must be less than 2 MB");
				document.getElementById("img_path").value = "";
			}
		};



	}



})();