(function () {
	var app = angular.module("GSWS");
	app.controller("JobcardDetails", ["$scope", "$state", "PRRD_Services", '$sce', JobCard_CTRL]);
	function JobCard_CTRL(scope, state, PRRD_Services, sce) {
		var token = sessionStorage.getItem("Token");
		var userid = sessionStorage.getItem("user");
		if (!token || !userid) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}
		scope.registrationdate = new Date();
		scope.panchayatlist = [];
		scope.habitationlist = [];
		scope.banklist = [];
		scope.bankbranchlist = [];
		scope.addmultiplejobcard = [];


		scope.districtname = sessionStorage.getItem("distname");
		scope.mandalname = sessionStorage.getItem("mname");
		var distcode = sessionStorage.getItem("distcode");
		var mcode = sessionStorage.getItem("mcode");

		var sachivalaymid = sessionStorage.getItem("secccode");
		var usertype = userid.split('-')[1].toString();
		if (usertype != "DA") {
			scope.IsDisabled = false;
			scope.IsView = true;
			GetloadJobCarddetailsforPA();
		} else {
			scope.IsDisabled = true;
			scope.IsView = false;
			GetloadJobCarddetailsforDA();
		}
		if (mcode != null) {

			scope.villagecode = "";
			loadpanchayatmaster();
		}
		scope.GetHabitationload = function () {
			LoadHabitation();
		}



		function GetloadJobCarddetailsforPA() {

			var input = {
				P_TYPE: 1,
				P_STATUS: "",
				UserId: sessionStorage.getItem("user"),
				SacId: sessionStorage.getItem("secccode"),
				DesignId: sessionStorage.getItem("desinagtion"),
				TranId: "",
				P_LGD_MANDAL_CODE: mcode,
				P_SACHIVALAYAM_ID: sachivalaymid

			};

			PRRD_Services.POSTENCRYPTAPI("GetJobcardetails", input, token, function (value) {
				var res = value.data;
				if (res.Status == "100") {

					scope.getJobcarddetails = res.Jobcarddetails;
				}
				else if (res.Status == "428") {

					swal('info', res.Reason, 'info');
					sessionStorage.clear();
					state.go("Login");
					return;
				}
				else {
					swal('info', res.Reason, 'info');
					return;
				}
			});
		}

		function GetloadJobCarddetailsforDA() {

			var input = {
				P_TYPE: 7,
				P_STATUS: "",
				UserId: sessionStorage.getItem("user"),
				SacId: sessionStorage.getItem("secccode"),
				DesignId: sessionStorage.getItem("desinagtion"),
				TranId: "",
				P_LGD_MANDAL_CODE: mcode,
				P_SACHIVALAYAM_ID: sachivalaymid

			};

			PRRD_Services.POSTENCRYPTAPI("GetJobcardetails", input, token, function (value) {
				var res = value.data;
				if (res.Status == "100") {
					scope.getJobcarddetails = res.Jobcarddetails;
				}
				else if (res.Status == "428") {
					swal('info', res.Reason, 'info');
					sessionStorage.clear();
					state.go("Login");
					return;
				}
				else {

					swal('info', res.Reason, 'info');
					return;
				}
			});
		}
		scope.clicked = function (JC_ID, Type) {
			sessionStorage.setItem("JC_ID", JC_ID);
			sessionStorage.setItem("Type", Type);
			// var url = state.href("ui.JobcardReg");
			state.go("ui.JobcardReg");
			//window.open(url, "_blank");  
		}

		function loadpanchayatmaster() {

			var input = {
				FTYPE: 1, DISTCODE: distcode, MandalCode: mcode,
				UserId: sessionStorage.getItem("user"),
				SacId: sessionStorage.getItem("secccode"),
				DesignId: sessionStorage.getItem("desinagtion"),
				TranId: ""
			};

			PRRD_Services.POSTENCRYPTAPI("GetHabitationCode", input, token, function (value) {

				var res = value.data;
				if (res.Status == "100") {
					scope.panchayatlist = res.Details;

				}
				else if (res.Status == "428") {
					swal('info', res.Reason, 'info');
					sessionStorage.clear();
					state.go("Login");
					return;
				}
				else {

					swal('info', res.Reason, 'info');
					return;
				}
			});
		}
		function LoadHabitation() {

			var input = {
				FTYPE: 2, DISTCODE: sessionStorage.getItem("distcode"), MandalCode: sessionStorage.getItem("mcode"), GPCODE: scope.panchayatcode,
				UserId: sessionStorage.getItem("user"),
				SacId: sessionStorage.getItem("secccode"),
				DesignId: sessionStorage.getItem("desinagtion"),
				TranId: ""
			};

			PRRD_Services.POSTENCRYPTAPI("GetHabitationCode", input, token, function (value) {

				var res = value.data;
				if (res.Status == "100") {
					scope.habitationlist = res.Details;

				}
				else if (res.Status == "428") {
					swal('info', res.Reason, 'info');
					sessionStorage.clear();
					state.go("Login");
					return;
				}
				else {

					swal('info', res.Reason, 'info');
					return;
				}
			});
		}
	}
})();

