(function () {
	var app = angular.module("GSWS");
	app.controller("MarriageApplication", ["$scope", "$state",  "PRRD_Services", '$sce', MarriageApplicationCall]);

	function MarriageApplicationCall(scope,state, PRRD_Services, sce) {


		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return false;
		}

		scope.ddlPanchayat = "";

		if (sessionStorage.getItem("RUFlag") == "U") {
			swal("info", "This Service is not working in Ward Sachivalayam Logins", 'info');
			return;
		}
		else {
			scope.preloader = true;
			var objCert = {
				seccode: sessionStorage.getItem("secccode")
			};

			PRRD_Services.POSTENCRYPTAPI("LoadNICPanchayats", objCert, token, function (value) {
				var res = value.data;
				scope.preloader = false;
				if (res.Status == "Success") {
					scope.PanchayatList = res.data;
				}
				else if (res.Status == "428") {
					sessionStorage.clear();
					swal("info", "Session Expired !!!", "info");
					state.go("Login");
					return;
				}
				else {
					swal("info", 'No Panchayats Data Found', "info");
				}
			});
		}

		scope.SubmitData = function () {
			if (!scope.ddlPanchayat) {
				swal('info', "Please Select Panchayat", 'info');
				return false;
			}
			scope.lgdcode = scope.ddlPanchayat;
			scope.preloader = true;

			var jsonData = {
				granttype: "A",
				servicetype: "M",
				seccode: sessionStorage.getItem("secccode")
			}

			PRRD_Services.POSTENCRYPTAPI("PRRDMarriageRegistration", jsonData, token, function (value) {
				var res = value.data;
				$("#DYNAMICDATA").html('');
				if (res.Status == "Success") {

					$("#DYNAMICDATA").append($('<form id="member_signup_marriage"  action="https://mpanchayat.ap.gov.in/PIM/createMcertGs.do" enctype="multipart/form-data" name="member_signup_marriage" method="POST" >').append(

						$('<input />', { name: 'token', value: res.data[0].token, type: 'hidden' }),
						$('<input />', { name: 'username', value: res.username, type: 'hidden' }),
						$('<input />', { name: 'gs_applid', value: res.gs_applid, type: 'hidden' }),
						$('<input />', { name: 'token_sno', value: res.data[0].token_sno, type: 'hidden' }),
						$('<input />', { name: 'servicetype', value: "M", type: 'hidden' }),
						$('<input />', { name: 'lgdcode', value: scope.lgdcode, type: 'hidden' }), //"196940"
						$('<input />', { id: 'savebutton', type: 'submit', value: 'Save' })), '</form>');

					document.forms['member_signup_marriage'].submit();
				}
				else if (res.Status == "428") {
					sessionStorage.clear();
					swal("info", "Session Expired !!!", "info");
					state.go("Login");
					return;
				}
			});
		}
	}
})();