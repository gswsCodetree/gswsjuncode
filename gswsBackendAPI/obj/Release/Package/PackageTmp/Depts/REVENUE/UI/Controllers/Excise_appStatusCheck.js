(function () {
	var app = angular.module("GSWS", ["akFileUploader"]);

	app.controller("ExciseComplaintCheck_Controller", ["$scope", "$state", "$log", "REVENUE_Services", Excise_CTRL]);

	function Excise_CTRL(scope, state, log, REVENUE_Services, ) {

		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}
		scope.getdetails = function (cardNumber, type) {




			if (!(scope.txtcmplttid)) {
				alert('Please enter Complaint ID');
				return;
			}
			var req = {
				appcode: scope.txtcmplttid
			};
			REVENUE_Services.POSTREVENCRYPTAPI("CheckComplaintStatus", req, token,function (value) {
				var res = value.data;
				if (res.Status == "Success") {
					scope.detailsshow = true;
					if (!(res.Data)) {
						scope.detailsshow = false;
						scope.ResData = "";
						alert("No Data available for this complaint id");
						return;
					}
					else
						scope.ResData = res.Data;
				}
				else if (res.Status == "428") {
					swal('info', res.Reason, 'info');
					sessionStorage.clear();
					state.go("Login");
					return;
				}

				else {
					scope.detailsshow = false;
					scope.ResData = "";
					alert("No Data available for this complaint id");
					return;
				}
			});
		};

		scope.Refresh = function () {
			location.reload(true);
		};

		scope.ImageUpload = function (cardNumber, type, t) {
			if (type == undefined || type == "") {
				alert("Please choose image to upload");
				return;
			}
			var file = type.attachment;
			var fileexten = file.type;
			if (fileexten.split("/")[0] == "image") {

			}
			else {
				swal("", "Only Image Accepted", "error");
				return;
			}
			var prop = { AadharCardNumber: cardNumber, Attachment: type.attachment };
			entityService.saveTutorial(prop)
				.then(function (data) {
					scope.BridePersonalDetail.bankImageUrl = data.data;
				});
		};

	}



})();
