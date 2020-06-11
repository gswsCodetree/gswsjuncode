(function () {
	var app = angular.module("GSWS", ["Network"]);

	app.controller("LoginMainController", ["$scope", "network_service", "$interval", Login_CTRL]);

	function Login_CTRL(scope, ns,int) {
		var baseurl = "/api/GSWSWeb/";
		scope.navaratnaluImage = 0;
		scope.isPaused = false;
		scope.virusClick = function (id) {
			scope.navaratnaluImage = id;
		}
		int(function () {
			if (!scope.isPaused) {
				if (scope.navaratnaluImage == 9) {
					scope.navaratnaluImage = 0;
					return;
				}
				scope.navaratnaluImage++;
				return;
			}
		}, 5000);
		$('.navaratnalu-area').hover(function () {
			scope.isPaused = true;
		}, function () {
			scope.isPaused = false;
		});

		CaptchLoad();
		GetCountsLoad();

		function CaptchLoad() {
			scope.apploader = true;
			sessionStorage.setItem("hskey", "admin");
			var obj = { ftype: "1" }
			ns.post(baseurl+"GetCaptcha", obj, function (data) {
				var res = data.data;
				if (res.code == "100") {
					scope.capatchval = "";
					$("#capid").html(res.idval);
					scope.capId = res.idval;
					$("#captchdis tr").remove();
					var img = $("<img>", { "id": res.idval, "src": "data:image/Gif;base64," + res.imgurl, "width": "90px", "height": "40px" });
					var rowget = $('<tr></tr>').append('<td></td>').html(img);
					$("#captchdis tbody").append(rowget);
				}

				else {
					scope.apploader = false;

					CaptchLoad();
				}

			});
		}

		function GetCountsLoad() {
			scope.apploader = true;
			sessionStorage.setItem("hskey", "admin");
			var obj = { Ftype: "1" }
			ns.post(baseurl + "GetCountsData", obj, function (data) {
				var res = data.data;
				if (res.Status == "100") {
					if (res.DataList.length > 0) {
						var cntdata = res.DataList[0];
						scope.GSWS_STAFF = cntdata.GSWS_STAFF;
						scope.VOLUNTEERS = cntdata.VOLUNTEERS;
						scope.SERVICES = cntdata.SERVICES;
						scope.SERVICES_DELIVERED = cntdata.SERVICES_DELIVERED;
						scope.SPANDANA_GRIVEANCE = cntdata.SPANDANA_GRIVEANCE;
					}
				}
				else {
					scope.apploader = false;
				}

			});
		}

		scope.GetRefresh = function () {
			CaptchLoad();

		}
		scope.GetLogin = function () {

			if (scope.userid == "" || scope.userid == undefined) {

				swal('info', 'Please Enter Username', 'info')
				return;
			}
			if (scope.gswspwd == "" || scope.gswspwd == undefined) {

				swal('info', 'Please Enter Password', 'info')
				return;
			}
			if (scope.Fcaptcha == "" || scope.Fcaptcha == undefined || scope.Fcaptcha == null) {
				swal('info', 'Please Enter Captcha', 'info')
				return;
			}
			else {
				LOADlOGIN();


			}


		}

		scope.GetCitezenLoagin = function () {

			swal('info', 'Cooming Soon', 'info');
			return;
		}
		function LOADlOGIN() {
			scope.isdisabled = true;
			scope.divloader = true;

			//haspwd = sha256_digest(scope.gswspwd);

			var token = sessionStorage.getItem("Token");
			var input = { Ftype: 1, FUsername: scope.userid, Newpassword: scope.gswspwd, Captcha: scope.Fcaptcha, ConfirmCaptch: scope.capId }

			ns.post(baseurl+"GetPSLogin", input, function (value) {

				var res = value.data;
				
				scope.divloader = false;
				scope.isdisabled = false;

				if (res.Status == '100') {
					console.log(res);
					scope.logindetails = JSON.parse(res.Details);
					sessionStorage.setItem("username", scope.logindetails[0].EMP_NAME);
					sessionStorage.setItem("user", scope.logindetails[0].USER_ID);
					sessionStorage.setItem("uniqueid", scope.logindetails[0].UNIQUE_ID);
					sessionStorage.setItem("usermobileno", scope.logindetails[0].MOBILE_NUMBER);
					sessionStorage.setItem("usermailid", scope.logindetails[0].EMP_MAIL_ID);

					sessionStorage.setItem("distname", scope.logindetails[0].DISTRICT_NAME);
					sessionStorage.setItem("distcode", scope.logindetails[0].DISTRICT_ID);

					sessionStorage.setItem("mname", scope.logindetails[0].MANDAL_NAME);
					sessionStorage.setItem("mcode", scope.logindetails[0].MANDAL_ID);
					sessionStorage.setItem("RUFlag", scope.logindetails[0].RURAL_URBAN_FLAG);

					sessionStorage.setItem("gpcode", scope.logindetails[0].GP_WARD_ID);
					sessionStorage.setItem("secccode", scope.logindetails[0].GSWS_ID);
					sessionStorage.setItem("secname", scope.logindetails[0].SECRETARIAT_NAME);

					sessionStorage.setItem("desinagtion", scope.logindetails[0].DESIG_ID);
					sessionStorage.setItem("desinagtionname", scope.logindetails[0].DESIGNATION);
					sessionStorage.setItem("pwdupdateddtatus", scope.logindetails[0].PWD_STATUS);


					sessionStorage.setItem("Token", res.access_token);
					sessionStorage.setItem("hskey", res.SKey);
					localStorage.setItem('logout-event', 'logout' + Math.random());

					
					//state.go("ue.Dashboard");
					if (scope.logindetails[0].PWD_STATUS == 0) {
						//state.go("ui.ProfileUpdate");
						window.location.href = "../#!/ProfileUpdate";
					}
					else if (scope.logindetails[0].PWD_STATUS == 1) {
					//	state.go("ue.Dashboard");
						window.location.href = "../#!/MainDashboard";
					}



					//state.go("ue.Dashboard");
				}
				else if (res.Status == "428") {
					sessionStorage.clear();
					window.location.href.reload();
					return;
				}
				else {
					scope.Fcaptcha = "";
					//CaptchLoad();
					swal('info', res.Reason, 'info');
				}
			});


		}

	}
	app.run(function ($http) {
		//sessionStorage.setItem('SEC_KEY', Date.now());
		$http.defaults.headers.common['X-XSRF-Token'] = angular.element('input[name="__RequestVerificationToken"]').attr('value');
		//$http.defaults.headers.common['SEC_KEY'] = sessionStorage.getItem('SEC_KEY');
	});
})();