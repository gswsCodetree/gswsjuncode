(function () {
	var app = angular.module("GSWS");

	app.controller("DaashboardController", ["$scope", "$state", "$log", "Login_Services", "$filter", "$sce", "$timeout", Main_CTRL]);

	function Main_CTRL(scope, state, log, Login_Services, $filter, sce, timeout) {

		var username = sessionStorage.getItem("user");
		var token = sessionStorage.getItem("Token");

		scope.ldate = new Date().toLocaleDateString();
		scope.final_data = [];

		if (username == null || username == undefined || token == null || token == undefined) {
			$(".modal").modal('hide');
			$(".modal-backdrop").css("display", "none");
			//state.go("Login");
			state.go("Login");
			return;
		}
		scope.username = username;

		scope.ploader = true;
		scope.TransReps = function () {

			var url = state.href("ui.TransResponse")
			//state.go("ui.TransResponse");
			window.open(url, "_blank");
		}

		var role = sessionStorage.getItem("desinagtion");
		scope.role = role;
		var input = "";
		if (role != "1") {
			input = { Ftype: 5, ROLE: role }

		}
		else {
			input = { Ftype: 3 }
		}
		scope.DISTRICTNAME = DISTRICTNAME;
		scope.ULBNAMES = ULBNAMES;

		Login_Services.POSTENCRYPTAPI("GetAllURLList", input, token, function (value) {
			var res = value.data;

			if (res.Status == "100") {
				// scope.divloader=false;
				scope.main_data = res.DataList;
				//console.log(scope.main_data);
				// scope.main_data = url_data;
				scope.url_data = scope.main_data;
				scope.ploader = false;
				scope.final_data = [];
				scope.hods = [];
				loadalladata();
				LoadSideMenuDashboard();
				LoadSpandanaDashboard();
				WalletAmount();
			}
			else if (res.Status == "428") {
				$("#mobile-modal").modal('hide');
				sessionStorage.clear();
				swal('info', res.Reason, 'info');
				//state.go("Login");
				state.go("Login");
				return;

			}

			else {
				scope.ploader = false;
				swal('info', res.Reason, 'info');
			}


		});


		scope.process_url = function (obj) {
			scope.pmobile = "";
			scope.Citizenname = "";
			scope.Suid = "";
			scope.uidnum = "";
			scope.popurlname = obj.URL_DESCRIPTION;
			sessionStorage.setItem('urldata', JSON.stringify(obj));

			if (obj.URL_ID == "200200101" || obj.URL_ID =="310300702") {
				gettransIntitate(JSON.stringify(obj), "");
			}
			else if (obj.TYPE_OF_SERVICE == "1") {
				$("#mobile-modal").modal('show');
			}
			
			else if (obj.TYPE_OF_SERVICE == "0" && obj.TYPE_OF_REQUEST == "1") {
				$("#mobile-modal").modal('show');
			}
			else {
				gettransIntitate(JSON.stringify(obj), "");
			}
			//gettransIntitate(obj)
		};

		scope.GetUpward = function () {
			var url = state.href("ui.Upward-downward")
			//state.go("ui.TransResponse");
			window.open(url, "_blank");

		}
		scope.GetDownWard = function () {
			var url = state.href("ui.Upward-downward")
			//state.go("ui.TransResponse");
			window.open(url, "_blank");

		}
		scope.GetInitiate = function () {

			if (scope.pmobile == "" || scope.pmobile == undefined || scope.pmobile == null) {

				swal('info', 'Please Enter Mobile Number', 'info');
				return;
			}
			if (scope.Citizenname == "" || scope.Citizenname == undefined || scope.Citizenname == null) {

				swal('info', 'Please Enter Name', 'info');
				return;
			}
			if (scope.pmobile.length < 10) {

				swal('info', 'Please Enter 10 Digit Mobile Number', 'info');
				return;
			}
			var reg = new RegExp("^[6-9][0-9]{9}$");

			if (!reg.test(scope.pmobile)) {
				swal('info', 'Please Enter Valid Mobile Number', 'info');
				return;
			}
			if (scope.pmobile == "1111111111" || scope.pmobile == "2222222222" || scope.pmobile == "3333333333" || scope.pmobile == "4444444444" || scope.pmobile == "5555555555" || scope.pmobile == "6666666666" || scope.pmobile == "7777777777" || scope.pmobile == "8888888888"   || scope.pmobile == "9999999999" ) {
				swal('info', 'Please Enter Valid Mobile Number', 'info');
				return;
			}
			if (scope.uidnum.length < 12) {

				swal('info', 'Please Enter 12 Digit Aadhaar Number', 'info');
				return;
			}
			if (scope.uidnum == "111111111111" || scope.uidnum == "222222222222" || scope.uidnum == "333333333333" || scope.uidnum == "44444444444444" || scope.uidnum == "555555555555" || scope.uidnum == "666666666666" || scope.uidnum == "777777777777" || scope.uidnum == "888888888888" || scope.uidnum == "999999999999") {
				swal('info', 'Please Enter Valid Aadhaar Number', 'info');
				return;
			}
			var val = validateVerhoeff(scope.uidnum);
			if (!val) {
				swal('info', 'Please Enter Valid Aadhaar Number', 'info');
				return;
			}
			gettransIntitate(sessionStorage.getItem("urldata"), scope.pmobile);
		}

		//Recharge Button

		scope.GetRecharge = function () {
			$("#PaymentId").html('');
			var distcode = sessionStorage.getItem("distcode");
			var secccode = sessionStorage.getItem("secccode");

			var req = { distCode: distcode, gwsCode: secccode, operatorId: username }

			Login_Services.POSTENCRYPTAPI("WalletRecharge", req, token, function (value) {
				var res = value.data;
				if (res.Status == "100") {

					window.open(res.Returnurl, "_blank");
					return;
				}
				else {
					swal('info', res.Reason, 'info');
				}
			}, function (error) {
				swal('info', error.data, 'info');
				return;
			});
		}

		function WalletAmount() {

		scope.distcode = sessionStorage.getItem("distcode");
			scope.secratariat_id = sessionStorage.getItem("secccode");
			var req = { district_code: scope.distcode, gsws_code: scope.secratariat_id }

			Login_Services.POSTENCRYPTAPI("WalletAmount", req, token, function (value) {
				var res = value.data;
				if (res.Status == "100") {

					scope.WalletAmount = res.Amount;
				}
				else {
					scope.WalletAmount = "0.0";
				}
			}, function (error) {
				//swal('info', error.data, 'info');
				return;
			});


		}
		function gettransIntitate(obj, mobile) {


			obj = JSON.parse(obj);
			scope.distcode = sessionStorage.getItem("distcode");
			scope.mcode = sessionStorage.getItem("mcode");
			scope.gpcode = sessionStorage.getItem("gpcode");
			scope.secratariat_id = sessionStorage.getItem("secccode");
			var username = sessionStorage.getItem("user");
			var role = sessionStorage.getItem("desinagtion");
			var token = sessionStorage.getItem("Token");
			var input = "";
			if (obj.ACTIVE_STATUS == '2') {
				input = { DEPT_ID: obj.SD_ID, HOD_ID: obj.HOD_ID, SERVICE_ID: obj.SD_ID, DISTRICT_ID: obj.DISTRICT_ID, MANDAL_ID: obj.MANDAL_ID, LOGIN_USER: username, TYPE_OF_REQUEST: obj.TYPE_OF_REQUEST, TYPE_OF_SERVICE: obj.TYPE_OF_SERVICE, URL_ID: obj.URL_ID, GSWS_ID: scope.secratariat_id, SECRETRAINT_CODE: scope.secratariat_id, DESIGNATION_ID: role, CITIZENNAME: scope.Citizenname, MOBILENUMBER: scope.pmobile, BEN_ID:scope.uidnum };
			}
			else {
				input = { DEPT_ID: obj.SD_ID, HOD_ID: obj.HOD_ID, SERVICE_ID: obj.SD_ID, DISTRICT_ID: scope.distcode, MANDAL_ID: scope.mcode, LOGIN_USER: username, TYPE_OF_REQUEST: obj.TYPE_OF_REQUEST, TYPE_OF_SERVICE: obj.TYPE_OF_SERVICE, URL_ID: obj.URL_ID, GSWS_ID: scope.secratariat_id, SECRETRAINT_CODE: scope.secratariat_id, DESIGNATION_ID: role, CITIZENNAME: scope.Citizenname, MOBILENUMBER: scope.pmobile, BEN_ID: scope.uidnum };
			}
			Login_Services.POSTTRANSCRYPTAPI("initiateTransaction", input, token, function (value) {
				var res = value.data;
				$("#mobile-modal").modal('hide');
				if (res.status == "200") {
					var url = "";
					if (obj.TYPE_OF_SERVICE == "0" && (obj.TYPE_OF_REQUEST == "1" || obj.TYPE_OF_REQUEST == "4")) {
						sessionStorage.setItem("TransID", res.TransId);
						sessionStorage.setItem("HodId", obj.HOD_ID);
					}

					if (obj.TYPE_OF_SERVICE == "1") {
						if (obj.HOD_ID == "2201" || obj.HOD_ID == "3702" || obj.HOD_ID == "3703") {
							url = obj.URL + "?Id=" + res.encrypttext + "&iv=" + res.key;
						}
						else if (obj.URL_ID == "110102401") {
							url = obj.URL + "?id=" + res.encrypttext + "&iv=" + res.key;
						}
						else if (obj.HOD_ID == "1601") {
							url = obj.URL + "Id=" + res.encrypttext + "^" + res.key;
						}
						else if (obj.HOD_ID == "1202" || obj.URL_ID=="110100103") {
							url = obj.URL + "&Id=" + res.encrypttext + "&IV=" + res.key;
						}
						else if (obj.HOD_ID == "2701" || obj.HOD_ID == "2703") {
							url = obj.URL + "?id=" + res.encrypttext + "&iv=" + res.key;
						}

						else if (obj.URL_ID == "370300101" || obj.URL_ID == "330104101" || obj.URL_ID == "200300101") {
							url = obj.URL + "?Id=" + res.encrypttext + "&IV=" + res.key;;
						}
						else if (obj.SD_ID == "13" || obj.SD_ID == "20" || obj.SD_ID == "28" || obj.SD_ID == "36" || obj.SD_ID == "38") {
							url = obj.URL + "Id=" + res.encrypttext + "&iv=" + res.key;
						}
						else if (obj.HOD_ID == "3901") {
							url = obj.URL + "&Id=" + res.encrypttext + "&IV=" + res.key;
						}
						else if (obj.HOD_ID == "4001") {
							url = obj.URL + "&Id=" + res.encrypttext + "&IV=" + res.key;
						}
						else
							url = obj.URL + "?Id=" + res.encrypttext + "^" + res.key;

						window.open(url, '_blank');
						//scope.translist = res.Translist;
					}
					else if (obj.URL_ID == "110401301" || obj.URL_ID == "110401401" || obj.URL_ID == "110102501" || obj.URL_ID =="310300104") {
						//url = obj.URL + "?Id=" + res.encrypttext + "&IV=" + res.key;
						var localurl = window.location.href.split("#!/")[0];
						var secondurl = obj.URL.split("#!/")[1] + "?Id=" + res.encrypttext + "&IV=" + res.key;
						window.open(localurl + "#!/" + secondurl, "_blank");
					}
					else {
						sessionStorage.setItem("urlid", obj.URL_ID);
						if (obj.HOD_ID == "1801") {
							input = { USERNAME: username, PASSWORD: username + 1, PS_TXN_ID: res.TransId, RETURN_URL: "http://prajaasachivalayam.ap.gov.in/PSTESTAPP/#!/Login" }

							//var input = { USERNAME: username, PASSWORD: username + 1, PS_TXN_ID: res.Translist[0].TNSId, RETURN_URL: "http://prajasachivalayam.ap.gov.in/PSTESTAPP/#!/Login" }
							Login_Services.POSTENCRYPTAPI("GetEncryptthirdparty", input, token, function (value) {
								var res = value.data;
								if (res.Status == "100") {

									//sessionStorage.setItem("spanecncypt", "encryptId=" + res.encrypttext + "&IV=" + res.key);

									$("#mobile-modal").modal('hide');
									//var localurl = window.location.href.split("#!/")[0];
									var secondurl = res.url + "accessToken=" + res.SToken+"&AadhaarNo=" + scope.uidnum + "&encryptId=" + res.encrypttext + "&IV=" + res.key + "&ServiceType=" + obj.URL_ID;
									window.open(secondurl, "_blank");
								}
								else if (res.Status == "428") {
									$("#mobile-modal").modal('hide');
									sessionStorage.clear();
									swal('info', res.Reason, 'info');
									//state.go("Login");
									state.go("Login");
									return;

								}
								else {
									swal('info', res.Reason, 'info');
									//sessionStorage.removeItem("spanecncypt");
								}
							});
						}
						else if (obj.URL_ID == "200301401" || obj.URL_ID == "360201701" || obj.URL_ID == "360201401" || obj.URL_ID == "130101101" || obj.URL_ID == "280101201" || obj.URL_ID == "280101401" || obj.URL_ID == "280101301" || obj.URL_ID == "360201801" || obj.URL_ID == "3602018501" || obj.URL_ID == "170100102" || obj.URL_ID == "130101401" || obj.URL_ID == "130101501" || obj.URL_ID == "240200101" )
						{

							$("#mobile-modal").modal('hide');
							var localurl1 = window.location.href.split("#!/")[0];
							var secondurl1 = obj.URL.split("#!/")[1];
							window.open(localurl1 + "#!/" + secondurl1 + "&enc=" + res.encrypttext + "&iv=" + res.key, "_blank");
						}
						else {
							$("#mobile-modal").modal('hide');
							var localurl = window.location.href.split("#!/")[0];
							var secondurl = obj.URL.split("#!/")[1];
							window.open(localurl + "#!/" + secondurl, "_blank");
						}
						//window.location=obj.URL;
						//window.open();

					}
					//window.open(url, "", "scrollbars=yes,resizable=yes,top=50,left=200,width=1000,height=500");
					//	
					return;
				}
				else if (res.Status == "428") {
					$("#mobile-modal").modal('hide');
					sessionStorage.clear();
					swal('info', res.Reason, 'info');
					//state.go("Login");
					state.go("Login");
					return;

				}
				else {
					swal('info', res.Reason, 'info');
				}


			});
		}


		scope.dept_checker = function (dept_id) {
			for (var i = 0; i < scope.final_data.length; i++) {
				if (scope.final_data[i].DEPT_ID == dept_id) {
					return false;
				}
			}
			return true;
		};

		scope.hod_checker = function (hod_id) {
			for (var i = 0; i < scope.hods.length; i++) {
				if (scope.hods[i].HOD_ID == hod_id) {
					return false;
				}
			}
			return true;
		};

		function loadalladata() {
			for (var i = 0; i < scope.url_data.length; i++) {
				if (scope.dept_checker(scope.url_data[i].SD_ID)) {
					scope.final_data.push({
						DEPT_NAME_TEL: scope.url_data[i].SD_NAME_TEL,
						DEPT_NAME: scope.url_data[i].SD_NAME,
						DEPT_ID: scope.url_data[i].SD_ID,
						HOD_LIST: null
					});
				}
			}

			for (var i = 0; i < scope.url_data.length; i++) {
				if (scope.hod_checker(scope.url_data[i].HOD_ID)) {
					var temp_data = {
						HOD_ID: scope.url_data[i].HOD_ID,
						HOD_NAME: scope.url_data[i].HOD_NAME,
						HOD_NAME_TEL: scope.url_data[i].HOD_NAME_TEL,
						DEPT_NAME: scope.url_data[i].SD_NAME,
						DEPT_NAME_TEL: scope.url_data[i].SD_NAME_TEL,
						DEPT_ID: scope.url_data[i].SD_ID
					};
					scope.hods.push(temp_data);
				}
			}

			scope.dept_count = 0;
			scope.HOD_LIST = [];
			for (var k = 0; k < scope.hods.length; k++) {
				scope.url_list = [];

				for (var i = 0; i < scope.url_data.length; i++) {
					if (scope.hods[k].HOD_ID == scope.url_data[i].HOD_ID) {
						scope.url_list.push({

							SD_ID: scope.url_data[i].SD_ID,
							SD_NAME: scope.url_data[i].SD_NAME,
							SD_NAME_TEL: scope.url_data[i].SD_NAME_TEL,
							HOD_ID: scope.url_data[i].HOD_ID,
							HOD_NAME: scope.url_data[i].HOD_NAME,
							HOD_NAME_TEL: scope.url_data[i].HOD_NAME_TEL,
							URL_ID: scope.url_data[i].URL_ID,
							URL: scope.url_data[i].URL,
							URL_DESCRIPTION: scope.url_data[i].URL_DESCRIPTION,
							URL_TEL_DESCRIPTION: scope.url_data[i].URL_DESC_TEL,
							TYPE_OF_SERVICE: scope.url_data[i].TYPE_OF_SERVICE,
							ACTIVE_STATUS: scope.url_data[i].ACTIVE_STATUS,
							TYPE_OF_REQUEST: scope.url_data[i].TYPE_OF_REQUEST,
							USER_MANUAL_URL: scope.url_data[i].USER_MANUAL_URL
						});
					}
				}

				scope.HOD_LIST.push(
					{
						DEPT_NAME: scope.hods[k].DEPT_NAME,
						DEPT_NAME_TEL: scope.hods[k].DEPT_NAME_TEL,
						DEPT_ID: scope.hods[k].DEPT_ID,
						HOD_ID: scope.hods[k].HOD_ID,
						HOD_NAME_TEL: scope.hods[k].HOD_NAME_TEL,
						HOD_NAME: scope.hods[k].HOD_NAME,
						URL_LIST: scope.url_list
					});
			}

			for (var i = 0; i < scope.final_data.length; i++) {
				scope.temp_hod_list = [];
				for (var j = 0; j < scope.HOD_LIST.length; j++) {
					if (scope.final_data[i].DEPT_ID == scope.HOD_LIST[j].DEPT_ID) {
						scope.temp_hod_list.push(scope.HOD_LIST[j]);
					}
				}

				scope.final_data[scope.dept_count].HOD_LIST = scope.temp_hod_list;
				scope.dept_count += 1;

			}

			scope.result = scope.final_data;

		}
		scope.get_data = function () {

			if (scope.search.URL_DESCRIPTION == "") {
				scope.viewDropdown = false;
			}
			else {
				scope.viewDropdown = true;
			}

			scope.url_data = $filter('filter')(scope.main_data, { URL_DESCRIPTION: scope.search.URL_DESCRIPTION });


			scope.final_data = [];
			scope.hods = [];

			for (var i = 0; i < scope.url_data.length; i++) {
				if (scope.dept_checker(scope.url_data[i].SD_ID)) {
					scope.final_data.push({
						DEPT_NAME: scope.url_data[i].SD_NAME,
						DEPT_ID: scope.url_data[i].SD_ID,
						HOD_LIST: null
					});
				}
			}

			for (var i = 0; i < scope.url_data.length; i++) {
				if (scope.hod_checker(scope.url_data[i].HOD_ID)) {
					var temp_data = {
						HOD_ID: scope.url_data[i].HOD_ID,
						HOD_NAME: scope.url_data[i].HOD_NAME,
						DEPT_NAME: scope.url_data[i].SD_NAME,
						DEPT_ID: scope.url_data[i].SD_ID
					};
					scope.hods.push(temp_data);
				}
			}

			scope.dept_count = 0;
			scope.HOD_LIST = [];
			for (var k = 0; k < scope.hods.length; k++) {
				scope.url_list = [];

				for (var i = 0; i < scope.url_data.length; i++) {
					if (scope.hods[k].HOD_ID == scope.url_data[i].HOD_ID) {
						scope.url_list.push({

							SD_ID: scope.url_data[i].SD_ID,
							SD_NAME: scope.url_data[i].SD_NAME,
							HOD_ID: scope.url_data[i].HOD_ID,
							HOD_NAME: scope.url_data[i].HOD_NAME,
							URL_ID: scope.url_data[i].URL_ID,
							URL: scope.url_data[i].URL,
							URL_DESCRIPTION: scope.url_data[i].URL_DESCRIPTION,
							TYPE_OF_SERVICE: scope.url_data[i].TYPE_OF_SERVICE,
							ACTIVE_STATUS: scope.url_data[i].ACTIVE_STATUS,
							TYPE_OF_REQUEST: scope.url_data[i].TYPE_OF_REQUEST,
							USER_MANUAL_URL: scope.url_data[i].USER_MANUAL_URL
						});
					}
				}

				scope.HOD_LIST.push(
					{
						DEPT_NAME: scope.hods[k].DEPT_NAME,
						DEPT_ID: scope.hods[k].DEPT_ID,
						HOD_ID: scope.hods[k].HOD_ID,
						HOD_NAME: scope.hods[k].HOD_NAME,
						URL_LIST: scope.url_list
					});
			}



			for (var i = 0; i < scope.final_data.length; i++) {
				scope.temp_hod_list = [];
				for (var j = 0; j < scope.HOD_LIST.length; j++) {
					if (scope.final_data[i].DEPT_ID == scope.HOD_LIST[j].DEPT_ID) {
						scope.temp_hod_list.push(scope.HOD_LIST[j]);
					}
				}

				scope.final_data[scope.dept_count].HOD_LIST = scope.temp_hod_list;
				scope.dept_count += 1;

			}


		};
		scope.get_Telugudata = function () {

			if (scope.search.URL_DESCRIPTION == "") {
				scope.viewDropdown = false;
			}
			else {
				scope.viewDropdown = true;
			}

			scope.url_data = $filter('filter')(scope.main_data, { URL_DESCRIPTION: scope.search.URL_DESCRIPTION });


			scope.final_data = [];
			scope.hods = [];

			for (var i = 0; i < scope.url_data.length; i++) {
				if (scope.dept_checker(scope.url_data[i].SD_ID)) {
					scope.final_data.push({
						DEPT_NAME_TEL: scope.url_data[i].SD_NAME_TEL,
						DEPT_NAME: scope.url_data[i].SD_NAME,
						DEPT_ID: scope.url_data[i].SD_ID,
						HOD_LIST: null
					});
				}
			}

			for (var i = 0; i < scope.url_data.length; i++) {
				if (scope.hod_checker(scope.url_data[i].HOD_ID)) {
					var temp_data = {
						HOD_ID: scope.url_data[i].HOD_ID,
						HOD_NAME: scope.url_data[i].HOD_NAME,
						HOD_NAME_TEL: scope.url_data[i].HOD_NAME_TEL,
						DEPT_NAME: scope.url_data[i].SD_NAME,
						DEPT_NAME_TEL: scope.url_data[i].SD_NAME_TEL,
						DEPT_ID: scope.url_data[i].SD_ID
					};
					scope.hods.push(temp_data);
				}
			}

			scope.dept_count = 0;
			scope.HOD_LIST = [];
			for (var k = 0; k < scope.hods.length; k++) {
				scope.url_list = [];

				for (var i = 0; i < scope.url_data.length; i++) {
					if (scope.hods[k].HOD_ID == scope.url_data[i].HOD_ID) {
						scope.url_list.push({

							SD_ID: scope.url_data[i].SD_ID,
							SD_NAME: scope.url_data[i].SD_NAME,
							SD_NAME_TEL: scope.url_data[i].SD_NAME_TEL,
							HOD_ID: scope.url_data[i].HOD_ID,
							HOD_NAME: scope.url_data[i].HOD_NAME,
							HOD_NAME_TEL: scope.url_data[i].HOD_NAME_TEL,
							URL_ID: scope.url_data[i].URL_ID,
							URL: scope.url_data[i].URL,
							URL_DESCRIPTION: scope.url_data[i].URL_DESCRIPTION,
							URL_TEL_DESCRIPTION: scope.url_data[i].URL_DESC_TEL,
							TYPE_OF_SERVICE: scope.url_data[i].TYPE_OF_SERVICE,
							ACTIVE_STATUS: scope.url_data[i].ACTIVE_STATUS,
							TYPE_OF_REQUEST: scope.url_data[i].TYPE_OF_REQUEST,
							USER_MANUAL_URL: scope.url_data[i].USER_MANUAL_URL
						});
					}
				}

				scope.HOD_LIST.push(
					{
						DEPT_NAME: scope.hods[k].DEPT_NAME,
						DEPT_ID: scope.hods[k].DEPT_ID,
						HOD_ID: scope.hods[k].HOD_ID,
						HOD_NAME: scope.hods[k].HOD_NAME,
						HOD_NAME_TEL: scope.hods[k].HOD_NAME_TEL,
						URL_LIST: scope.url_list
					});
			}



			for (var i = 0; i < scope.final_data.length; i++) {
				scope.temp_hod_list = [];
				for (var j = 0; j < scope.HOD_LIST.length; j++) {
					if (scope.final_data[i].DEPT_ID == scope.HOD_LIST[j].DEPT_ID) {
						scope.temp_hod_list.push(scope.HOD_LIST[j]);
					}
				}

				scope.final_data[scope.dept_count].HOD_LIST = scope.temp_hod_list;
				scope.dept_count += 1;

			}


		};


		scope.onMouseHold = function (e, obj) {
			scope.hint_frame = true;
			e = e || window.event; //window.event for IE
			// alert("Keycode of key pressed: " + (e.keyCode || e.which));
			e.preventDefault();
			$("body").on("contextmenu", function (e) {
				return false;
			});
			if (e.which == 3) {

				scope.MTitle = obj.URL_DESCRIPTION;
				scope.urldes = obj.USER_MANUAL_URL;
				if (scope.urldes == null || scope.urldes == undefined) {

					swal('info', 'UserManual Not Available To This Servcie', 'info');
					return
				}
				else {
					scope.urldes = sce.trustAsResourceUrl(scope.urldes);
					//alert(scope.urldes);
					$("#myModal12").modal('show');
				}

			}
			// alert(obj);

		};

		scope.getclose = function () {
			$("#myModal12").modal('hide');
		}
		scope.onMouseRelease = function () {
			scope.hint_frame = false;
		};



		scope.GetCheck = function (val) {

			scope.viewDropdown = true;
			scope.url_data = $filter('filter')(scope.main_data, { URL_DESCRIPTION: val });
			//scope.search.URL_DESCRIPTION = val;
		}

		//navaratanalu
		function loadmuncipality() {
			var req = {
				TYPE: "9"
			};
			Login_Services.DEMOAPI("LoadDepartments", req, function (value) {
				if (value.data.Status == "Success") {
					scope.MandalsDD = value.data.Details;
				}
				else {
					alert("Mandals Loading Failed");
				}
			});

		}
		scope.LoadEnergy = function () {
			var req = { FTYPE: 4, DISTCODE: scope.seldistrict }

			Login_Services.POSTENCRYPTAPI("GetAllURLList", req, token, function (value) {
				if (value.data.Status == "100") {
					//scope.DistricsDD = value.data.Details;
					scope.mainEnergydata = value.data.DataList;
					//console.log(scope.maindata);
				}
				else {
					alert("Districs Loading Failed");
				}
			});
		}
		function LoadDistrics() {
			var req = {
				TYPE: "4"
			};
			Login_Services.DEMOAPI("LoadDepartments", req, function (value) {
				if (value.data.Status == "Success") {
					scope.DistricsDD = value.data.Details;
				}
				else {
					alert("Districs Loading Failed");
				}
			});
		}
		scope.GetSubmit = function () {


			if (scope.selmandal == "" || scope.selmandal == undefined || scope.selmandal == null) {
				swal('info', 'Please Select Mandal', 'info');
				return;
			}
			//alert(scope.selmandal);
			scope.seldistrict1 = scope.selmandal.split('_')[0];
			var req = { FTYPE: 2, DISTCODE: scope.seldistrict1, MCODE: scope.selmandal.split('_')[1] }

			Login_Services.POSTENCRYPTAPI("GetAllURLList", req, token, function (value) {
				if (value.data.Status == "100") {
					//scope.DistricsDD = value.data.Details;
					scope.maindata = value.data.DataList;
					//console.log(scope.maindata);

					scope.muncount = 0, scope.towncount = 0, scope.phealthcount = 0;

					for (var i = 0; i < scope.maindata.length; i++) {


						if (scope.maindata[i].HOD_ID == '2701') {

							scope.muncount++;
						}
						if (scope.maindata[i].HOD_ID == '2702') {

							scope.towncount++
						}
						if (scope.maindata[i].HOD_ID == '2703') {

							scope.phealthcount++;
						}
					}
				}
				else {
					alert("Districs Loading Failed");
				}
			});

		}
		loadHousing();
		function loadHousing() {
			scope.Housesitelist = [];

			scope.mflag = sessionStorage.getItem("mcode");
			//scope.navalist = scope.url_data;

			input = { Ftype: 8, ROLE: sessionStorage.getItem("desinagtion"), MCODE: sessionStorage.getItem("mcode") }
			Login_Services.POSTENCRYPTAPI("GetAllURLList", input, token, function (value) {
				var res = value.data;

				if (res.Status == "100") {
					scope.Housesitelist = res.DataList;
					console.log(scope.Housesitelist);
				}
				else {
					swal('info', res.Reason, 'error');
					return;
				}
			});

		}

		scope.GetNavaRatnalu = function (title, Id) {
			scope.navalist = [];
			scope.Ntitle = title;
			scope.ID = Id;
			//scope.navalist = scope.url_data;

			input = { Ftype: 7, DISTCODE: Id }
			Login_Services.POSTENCRYPTAPI("GetAllURLList", input, token, function (value) {
				var res = value.data;

				if (res.Status == "100") {
					scope.navalist = res.DataList;
				}
				else {
					swal('info', res.Reason, 'error');
					return;
				}
			});

			console.log(scope.navalist);
		}

		scope.filteredChoices = [];
		scope.filtereddist = [];

		scope.isVisible = {
			suggestions: false
		};
		scope.isVisible1 = {
			suggestions: false
		};

		scope.filterItems = function () {
			//alert(scope.enteredtext);
			if (scope.enteredtext.length >= 1) {
				scope.filteredChoices = querySearch(scope.enteredtext);
				console.log(scope.filteredChoices.length);
				scope.isVisible.suggestions = scope.filteredChoices.length > 0 ? true : false;
			}
			else {
				scope.isVisible.suggestions = false;
			}
		};

		scope.filterdistItems = function () {
			//alert(scope.enteredtext);
			if (scope.enteredtext1.length >= 1) {
				scope.filtereddist = querydistSearch(scope.enteredtext1);
				//console.log(scope.filteredChoices.length);
				scope.isVisible1.suggestions = scope.filtereddist.length > 0 ? true : false;
			}
			else {
				scope.isVisible.suggestions = false;
			}
		};
		/**
		 * Takes one based index to save selected choice object
		 */
		scope.selectItem = function (item) {
			//scope.selected = scope.ULBNAMES[index - 1];
			scope.enteredtext = item.ULBNAME;
			scope.ID = item.ULBID;
			//	alert(scope.ID);
			scope.isVisible.suggestions = false;

			var req = { FTYPE: 2, DISTCODE: item.DISTRICTID, MCODE: item.ULBID }

			Login_Services.POSTENCRYPTAPI("GetAllURLList", req, token, function (value) {
				if (value.data.Status == "100") {
					//scope.DistricsDD = value.data.Details;
					scope.maindata = value.data.DataList;
					//console.log(scope.maindata);

					scope.muncount = 0, scope.towncount = 0, scope.phealthcount = 0;

					for (var i = 0; i < scope.maindata.length; i++) {


						if (scope.maindata[i].HOD_ID == '2701') {

							scope.muncount++;
						}
						if (scope.maindata[i].HOD_ID == '2702') {

							scope.towncount++
						}
						if (scope.maindata[i].HOD_ID == '2703') {

							scope.phealthcount++;
						}
					}
				}
				else {
					alert("muncipality url Loading Failed");
				}
			});
		};

		scope.selectdistItem = function (item) {
			//scope.selected = scope.ULBNAMES[index - 1];
			scope.enteredtext1 = item.DISTRICTNAME;
			scope.ID = item.DISTRICTID;
			//	alert(scope.ID);
			scope.isVisible1.suggestions = false;

			var req = { FTYPE: 4, DISTCODE: item.DISTRICTID }

			Login_Services.POSTENCRYPTAPI("GetAllURLList", req, token, function (value) {
				if (value.data.Status == "100") {
					//scope.DistricsDD = value.data.Details;
					scope.mainEnergydata = value.data.DataList;
					//console.log(scope.maindata);
				}
				else {
					alert("energy url Loading Failed");
				}
			});
		};
		/**
		 * Search for states... use $timeout to simulate
		 * remote dataservice call.
		 */
		function querySearch(query) {
			// returns list of filtered items
			return query ? scope.ULBNAMES.filter(createFilterFor(query, 1)) : [];
		}

		function querydistSearch(query) {
			// returns list of filtered items
			return query ? scope.DISTRICTNAME.filter(createFilterFor(query, 2)) : [];
		}
		/**
		 * Create filter function for a query string
		 */

		scope.GetNavaSakamDetails = function (val) {
			NavaSakam(val);
		}
		function NavaSakam(type) {
			if (username == "brsreddy" || username == "ramsaid" || username == "chandu") {
				scope.nuser = "11290586-WEA";
			}
			else {
				scope.nuser = username;
			}
			var req = { secr_user: scope.nuser }

			Login_Services.POSTENCRYPTAPI("NavaSakamaData", req, token, function (value) {
				if (value.data.Status == "100") {

					window.open(value.data.Returnurl + "&service_id=" + type, "_blank");
				}
				else {
					swal('info'.res.Reason, 'info');
				}
			});
		}
		function createFilterFor(query, val) {

			var lowercaseQuery = angular.lowercase(query);

			return function filterFn(item) {
				//alert(item);
				var label = '';
				// Check if the given item matches for the given query
				if (val == 1) {
					label = angular.lowercase(item.ULBNAME);
				}
				else
					label = angular.lowercase(item.DISTRICTNAME);

				return (label.indexOf(lowercaseQuery) === 0);
			};
		}
		scope.items = scope.ULBNAMES;

		/* start side menu register and receieve	 */
		function LoadSideMenuDashboard() {

			var input = { type: 1, designationId: username };
			Login_Services.POSTENCRYPTAPI("SideMenuDashboardData", input, token, function (value) {
				var res = value.data;
				if (res.Status == "100") {

					scope.SideMenuDashboard = res.DataList;

					scope.servicereq = scope.SideMenuDashboard[0].SERVICE_REGIS;
					scope.servicedelivert = scope.SideMenuDashboard[0].SERVICE_DELIVERED;
					scope.pendingforapproval = scope.SideMenuDashboard[0].PENDING_FOR_APPROVAL;
					scope.responserecieved = scope.SideMenuDashboard[0].RESPONSE_RECEIVED;

					scope.RecievedInstr = scope.SideMenuDashboard[0].RECEIVED_INSTRUC;
					scope.ResponseSent = scope.SideMenuDashboard[0].RESPONSE_SENT;

					scope.Desigationaname = sessionStorage.getItem("desinagtionname");
				}
				else {
					//swal('info', res.Reason, 'info');
				}


			});
		}

		/* end side menu reg*/

		/* Meeseva Link enable */


		scope.GetMeesevaData = function (val, description) {

			if (description) {
				$(".meesevaname").html(description);
				$("#meesevaframe").attr("src", "#");
				$("#meesevaframe").attr("src", "http://gramawardsachivalayam.ap.gov.in/PSTESTAPP/MeesevaService/Images/" + description.replace("/", "_") + ".jpg");
			}

			var start = new Date;
			start = start.setSeconds(start.getSeconds() + 5)

			timeinterid = setInterval(function () {
				var time = Math.round((start - new Date) / 1000);
				if (time > 0)
					$('.Timer').text(time + " Seconds");
				else
					$('.Timer').text("")
			}, 1000);

			$("#Meesevamodel").modal('show');
			timeoutid = setTimeout(function () { SkipMeesevaPopUp(val); }, 5000);


		}

		scope.SkipInterval = function () {
			SkipMeesevaPopUp();
		}


		function MeesevaVroLogin() {
			//http://uat.meeseva.gov.in/ASDCDeptPortal/Userinterface/VSWSRedirection.aspx
			scope.muser = username;// "CTR-VRO1-25004";
			var req = { OPERATORID: scope.muser, SECRETARIATCODE: sessionStorage.getItem("secccode") }
			Login_Services.DEMOMeesevaAPI("MeesevaVROEncrypt", req, function (value) {
				$("#Meesevamodel").modal('hide');
				var res = value.data;
				if (res.STATUS == "100") {
					scope.divmeeseva = true;

					$("#DYNAMICDATA").html('');
					$("#DYNAMICDATA").append($('<form id="member_signup1"  action="https://apdept.meeseva.gov.in/APSDCDeptPortal/Userinterface/VSWSRedirection.aspx" name="member_signup1" target="_blank" method="POST" >').append(

						$('<input />', { name: 'TOKEN', value: res.TOKEN, type: 'hidden' }),
						$('<input />', { name: 'LANDINGID', value: res.LANDINGID, type: 'hidden' }),
						$('<input />', { name: 'USERID', value: scope.muser, type: 'hidden' }),
						$('<input />', { name: 'ENCDATA', value: res.ENCDATA, type: 'hidden' }),
						$('<br />'),
						$('<input />', { id: 'savebutton', style: 'display:none', type: 'submit', value: 'Save' })), '</form>');

					document.forms['member_signup1'].submit();



				}
				else {
					scope.divmeeseva = false;
					swal('info', res.Message, 'info');
					return;
				}
			});
		}

		scope.HousesitesVro = function () {
			HosueSitesVroLogin();
		}
		function HosueSitesVroLogin() {
			//http://uat.meeseva.gov.in/ASDCDeptPortal/Userinterface/VSWSRedirection.aspx
			scope.muser = "ANA-ANA-VRO-1"; //username;// "CTR-VRO1-25004";
			var req = { OPERATORID: scope.muser, SECRETARIATCODE: sessionStorage.getItem("secccode") }
			Login_Services.DEMOMeesevaAPI("VROApproval", req, function (value) {
				$("#Meesevamodel").modal('hide');
				var res = value.data;
				if (res.STATUS == "100") {
					scope.divmeeseva = true;

					$("#DYNAMICDATA").html('');
					$("#DYNAMICDATA").append($('<form id="member_signup1"  action="' + res.RedirectUrl+'" name="member_signup1" target="_blank" method="POST" >').append(

						$('<input />', { name: 'TOKEN', value: res.TOKEN, type: 'hidden' }),
						$('<input />', { name: 'LANDINGID', value: res.LANDINGID, type: 'hidden' }),
						$('<input />', { name: 'USERID', value: scope.muser, type: 'hidden' }),
						$('<input />', { name: 'ENCDATA', value: res.ENCDATA, type: 'hidden' }),
						$('<br />'),
						$('<input />', { id: 'savebutton', style: 'display:none', type: 'submit', value: 'Save' })), '</form>');

					document.forms['member_signup1'].submit();



				}
				else {
					scope.divmeeseva = false;
					swal('info', res.Message, 'info');
					return;
				}
			});
		}
		function SkipMeesevaPopUp() {
			clearTimeout(timeoutid);
			clearInterval(timeinterid);
			//var val = "";
			//var req = { SERVICEID: val, SECRETARIATCODE: sessionStorage.getItem("secccode") }
			scope.divmeeseva = true;
			$("#Meesevamodel").modal('hide');
			//if (username == "brsreddy" || username == "ramsaid" || username == "chandu" || username == "meeseva") {
			//	window.open("../GSWS/meesevaTest.aspx?Seccode=10690567&userid=10690567-DA", "_blank");
			//}
			//else {
			//	window.open("../GSWS/meesevaTest.aspx?Seccode=" + sessionStorage.getItem("secccode") + "&userid=" + username, "_blank");
			//}
			$("#DYNAMICDATA").append($('<form id="member_signup"  action="../UrlRedirection.aspx" name="member_signup" target="_blank" method="POST" >').append(

				$('<input />', { name: 'Seccode', value: sessionStorage.getItem("secccode"), type: 'hidden' }),
				$('<input />', { name: 'userid', value: username, type: 'hidden' }),
			
						$('<input />', { id: 'savebutton', style: 'display:none', type: 'submit', value: 'Save' })), '</form>');

				document.forms['member_signup'].submit();

			//Login_Services.DEMOMeesevaAPI("MeesevaEncrypt", req, function (value) {
			//	$("#Meesevamodel").modal('hide');
			//	var res = value.data;
			//	if (res.STATUS == "100") {
			//		scope.divmeeseva = true;

			//		scope.mtoken = res.TOKEN;
			//		//alert(scope.mtoken);
			//		scope.mlandingid = res.LANDINGID;
			//		scope.mscaid = res.SCAID;
			//		scope.mchannelid = res.CHANNELID;
			//		scope.moperatorid = res.OPERATORID;
			//		scope.moperator_uniqueno = res.OPERATOR_UNIQUENO;
			//		scope.mserviceid = res.SERVICEID;
			//		scope.mencdata = res.ENCDATA;
			//		$("#DYNAMICDATA").html('');
			//		$("#DYNAMICDATA").append($('<form id="member_signup"  action="http://meeseva.gov.in/GSVWIMeeseva/UserInterface/DC/VSWSRedirection.aspx" name="member_signup" target="_blank" method="POST" >').append(

			//			$('<input />', { name: 'TOKEN', value: res.TOKEN, type: 'hidden' }),
			//			$('<input />', { name: 'LANDINGID', value: res.LANDINGID, type: 'hidden' }),
			//			$('<input />', { name: 'SCAID', value: res.SCAID, type: 'hidden' }),
			//			$('<input />', { name: 'CHANNELID', value: res.CHANNELID, type: 'hidden' }),
			//			$('<input />', { name: 'OPERATORID', value: res.OPERATORID, type: 'hidden' }),
			//			$('<input />', { name: 'OPERATOR_UNIQUENO', value: res.OPERATOR_UNIQUENO, type: 'hidden' }),
			//			//$('<input />', { name: 'SERVICEID', value: res.SERVICEID, type: 'hidden' }),
			//			$('<input />', { name: 'ENCDATA', value: res.ENCDATA, type: 'hidden' }),
			//			$('<br />'),
			//			$('<input />', { id: 'savebutton', style: 'display:none', type: 'submit', value: 'Save' })), '</form>');

			//		document.forms['member_signup'].submit();

			//		// var newForm=jQuery('<form>',{ 'action':'http://uat.meeseva.gov.in/GSVWIMeeseva/UserInterface/DC/VSWSRedirection.aspx'}).
			//		// append(jQuery('<input>', { 'name': 'TOKEN', 'value': res.TOKEN, 'type': 'hidden' },
			//		// '<input>', { 'name': 'LANDINGID', 'value': res.LANDINGID, 'type': 'hidden' },
			//		// '<input>', { 'name': 'SCAID', 'value': res.SCAID, 'type': 'hidden' },
			//		// '<input>', { 'name': 'CHANNELID', 'value': res.CHANNELID, 'type': 'hidden' },
			//		// '<input>', { 'name': 'OPERATORID', 'value': res.OPERATORID, 'type': 'hidden' },
			//		// '<input>', { 'name': 'OPERATOR_UNIQUENO', 'value': res.OPERATOR_UNIQUENO, 'type': 'hidden' },
			//		// '<input>', { 'name': 'SERVICEID', 'value': res.SERVICEID, 'type': 'hidden' }));

			//		// newForm.submit();


			//	}
			//	else {
			//		scope.divmeeseva = false;
			//		swal('info', res.Message, 'info');
			//		return;
			//	}
			//});
		}
		/* end Meeseva */

		//spandana integration code

		//scope.SMasterlist = spandamster;
		scope.GetLoadSpandana = function () {
			scope.divspandareg = true;
		}
		scope.GetSPandanaDetails = function () {

			if (scope.spandanauid == "" || scope.spandanauid == undefined || scope.spandanauid == null) {
				swal('info', 'Please Enter Aadhaar Numner', 'info');
				return;
			}
			if (scope.spandanauid.length < 12) {
				swal('info', 'Please Enter 12 Digit Aadhaar Numner', 'info');
				return;
			}

			var input = { INPUT: 1, UID: scope.spandanauid };
			var token = sessionStorage.getItem("Token");
			Login_Services.POSTREVENUEENCRYPTAPI("GetSpandanaUIDDetails", input, token, function (value) {
				var res = value.data;
				if (res.Status == "100") {

					scope.spandanalist = res.Details;
					console.log(scope.spandanalist);

					scope.SdistList = [{
						DISTRICT: scope.spandanalist[0].DISTRICT_NAME,
						LGD_DISTRICT_CODE: scope.spandanalist[0].DISTRICT_ID,
					}];
					scope.SmandalList = [{
						MANDAL_NAME: scope.spandanalist[0].TEHSIL_NAME,
						LGD_MANDAL_CODE: scope.spandanalist[0].MANDAL_ID
					}];

					scope.SvillageList = [{

						VILLAGE: scope.spandanalist[0].VT_NAME,
						LGD_VILLAGE_CODE: scope.spandanalist[0].VT_CODE
					}];
					scope.Sruflag = scope.spandanalist[0].RURAL_URBAN_FLAG;
					scope.Sname = scope.spandanalist[0].CITIZEN_NAME;
					scope.Sfathername = scope.spandanalist[0].CARE_OF;
					scope.Sage = scope.spandanalist[0].AGE;
					scope.Sdoorno = scope.spandanalist[0].DOOR_NO;
					scope.Shabitation = scope.spandanalist[0].STREET;
					scope.Sgender = scope.spandanalist[0].GENDER;
					scope.Smob = scope.spandanalist[0].MOBILE_NUMBER;
					scope.Soccuption = scope.spandanalist[0].OCCUPATION_CATEGORY;
					scope.Sincome = scope.spandanalist[0].ANNUAL_INCOME;
					scope.Shhid = scope.spandanalist[0].HH_CODE;
					scope.Spincode = scope.spandanalist[0].PINCODE;
					scope.divmain = true;
					scope.divgrievance = true;
					loadtoken();
					//scope.Sname = scope.spandanalist[0].
				}


				else {
					scope.divmain = false;
					scope.divgrievance = false;
					swal('info', res.Reason, 'info');
				}
			});
		}

		scope.getgrievancetype = function (type) {

			if (type == "1") {
				scope.gsubject = true;
				scope.gkeyword = false;
				scope.gdept = false;
				scope.subjectcode = "";
				scope.subjectlist = [];
				scope.subsubjectcode = "";
				scope.subsubjectlist = [];

				loadDepartments(2);
			}
			else if (type == "2") {
				scope.subjectcode = "";
				scope.subjectlist = [];
				scope.subsubjectcode = "";
				scope.subsubjectlist = [];
				scope.gsubject = false;
				scope.gkeyword = true;
				scope.gdept = false;
				loadDepartments(4);
			}
			else {
				scope.subjectcode = "";
				scope.subjectlist = [];
				scope.subsubjectcode = "";
				scope.subsubjectlist = [];
				scope.gsubject = false;
				scope.gkeyword = false;
				scope.gdept = true;
				loadDepartments(1);
			}
		}

		scope.getSpandanaGrievance = function (val) {
			if (val == "1") {
				scope.divservice = true;
				scope.divspandareg = false;
				scope.divspanstatus = false;
				scope.distcode = "";
				scope.mcode = "";
				scope.gpcode = "";
				scope.ruflag = "";
				loaddistmaster();
			}
			else {
				scope.divspandareg = true;
				scope.divservice = false;
				scope.divspanstatus = false;
				scope.distcode = "";
				scope.ruflag = "";
				scope.mcode = "";
				scope.gpcode = "";
				loaddistmaster();
			}
		}
		scope.GetGrievanceStatus = function () {
			scope.divservice = false;
			scope.divspanstatus = true;
			scope.divspandareg = false;

		}

		//service Reguest
		scope.ServiceRequest = function () {

			if (scope.spandanauid == "" || scope.spandanauid == undefined || scope.spandanauid == null || scope.spandanauid.length < 12) {

				swal('info', 'Please Enter 12 Digit Aadhaar Number', 'info')
				return;
			}
			if (scope.distcode == "" || scope.distcode == undefined || scope.distcode == null) {

				swal('info', 'Please Select District', 'info')
				return;
			}
			if (scope.ruflag == "" || scope.ruflag == undefined || scope.ruflag == null) {

				swal('info', 'Please Select rural/urban flag', 'info')
				return;
			}
			if (scope.mcode == "" || scope.mcode == undefined || scope.mcode == null) {

				swal('info', 'Please Select Mandal', 'info')
				return;
			}
			if (scope.gpcode == "" || scope.gpcode == undefined || scope.gpcode == null) {

				swal('info', 'Please Please Select Pachayat', 'info')
				return;
			}
			else {
				GetSpandanaData('3402001');

			}
		}

		//service Grievance
		scope.GrievanceRequest = function () {

			if (scope.spandanauid1 == "" || scope.spandanauid1 == undefined || scope.spandanauid1 == null || scope.spandanauid1.length < 12) {

				swal('info', 'Please Enter 12 Digit Aadhaar Number', 'info')
				return;
			}
			if (scope.distcode == "" || scope.distcode == undefined || scope.distcode == null) {

				swal('info', 'Please Select District', 'info')
				return;
			}
			if (scope.ruflag == "" || scope.ruflag == undefined || scope.ruflag == null) {

				swal('info', 'Please Select rural/urban flag', 'info')
				return;
			}
			if (scope.mcode == "" || scope.mcode == undefined || scope.mcode == null) {

				swal('info', 'Please Select Mandal', 'info')
				return;
			}
			if (scope.gpcode == "" || scope.gpcode == undefined || scope.gpcode == null) {

				swal('info', 'Please Please Select Pachayat', 'info')
				return;
			}
			else {
				GetSpandanaData('3402001');

			}
		}
		function GetSpandanaData(sid) {
			scope.distcode1 = sessionStorage.getItem("distcode");
			scope.mcode1 = sessionStorage.getItem("mcode");
			scope.gpcode1 = sessionStorage.getItem("gpcode");
			scope.secratariat_id = sessionStorage.getItem("secccode");
			var username = sessionStorage.getItem("user");
			var role = sessionStorage.getItem("desinagtion");
			var token = sessionStorage.getItem("Token");
			var input = { DEPT_ID: 34, HOD_ID: "3402", SERVICE_ID: sid, DISTRICT_ID: scope.distcode1, MANDAL_ID: scope.mcode1, LOGIN_USER: username, TYPE_OF_REQUEST: 1, URL_ID: "340200101", GSWS_ID: scope.secratariat_id, SECRETRAINT_CODE: scope.secratariat_id, DESIGNATION_ID: role, CITIZENNAME: scope.Citizenname, MOBILENUMBER: scope.pmobile, Sdistcode: scope.distcode, Smcode: scope.mcode, Svtcode: scope.gpcode, SRuflag: scope.ruflag, UID: scope.spandanauid };

			Login_Services.POSTTRANSCRYPTAPI("SpandanainitiateTransaction", input, token, function (value) {
				var res = value.data;
				if (res.status == "200") {

					window.open(res.URL, "_blank");

				}
				else {
					swal('info', res.Reason, 'info');
					return;
				}
			});
		}


		scope.GetSpandanaUrl = function (obj) {
			$("#spandana-modal").modal('show');
			scope.Suid = "";
			scope.SCitizenname = "";
			scope.Smobile = "";
			scope.popurlname = obj.URL_DESCRIPTION;
			scope.Sobj = JSON.stringify(obj);
		}

		scope.GetSpandanaRequest = function () {

			if (scope.Suid == "" || scope.Suid == undefined || scope.Suid == null || scope.Suid.length < 12) {

				swal('info', 'Please Enter 12 Digit Aadhaar Number', 'info')
				return;
			}
			if (!scope.SCitizenname) {

				swal('info', 'Please Enter Citizen Name', 'info')
				return;
			}
			if (!scope.Smobile || scope.Smobile.length < 10) {

				swal('info', 'Please Enter 10 Digit Mobile Number', 'info');
				return;
			}
			var reg = new RegExp("^[6-9][0-9]{9}$");

			if (!reg.test(scope.Smobile)) {
				swal('info', 'Please Enter Valid Mobile Number', 'info');
				return;
			}
			if (scope.Smobile == "1111111111" || scope.Smobile == "2222222222" || scope.Smobile == "3333333333" || scope.Smobile == "4444444444" || scope.Smobile == "5555555555" || scope.Smobile == "6666666666" || scope.Smobile == "7777777777" || scope.Smobile == "8888888888" || scope.Smobile == "9999999999") {
				swal('info', 'Please Enter Valid Mobile Number', 'info');
				return;
			}
			else {
				GetSpandanaDataURL();
			}
		}
		function GetSpandanaDataURL() {
			scope.distcode1 = sessionStorage.getItem("distcode");
			scope.mcode1 = sessionStorage.getItem("mcode");
			scope.gpcode1 = sessionStorage.getItem("gpcode");
			scope.secratariat_id = sessionStorage.getItem("secccode");
			var username = sessionStorage.getItem("user");
			var role = sessionStorage.getItem("desinagtion");
			var token = sessionStorage.getItem("Token");
			var obj = JSON.parse(scope.Sobj);
			
			var input = { DEPT_ID: obj.SD_ID, HOD_ID: obj.HOD_ID, SERVICE_ID: obj.SD_ID, SERVICE_CODE: obj.URL.split("?HodId=")[1], DISTRICT_ID: scope.distcode1, MANDAL_ID: scope.mcode1, LOGIN_USER: username, TYPE_OF_REQUEST: 1, URL_ID: obj.URL_ID, GSWS_ID: scope.secratariat_id, SECRETRAINT_CODE: scope.secratariat_id, DESIGNATION_ID: role, CITIZENNAME: scope.SCitizenname, MOBILENUMBER: scope.Smobile, Sdistcode: scope.distcode1, Smcode: scope.mcode1, Svtcode: scope.gpcode1, SRuflag: scope.ruflag, UID: scope.Suid };

			Login_Services.POSTTRANSCRYPTAPI("SpandanainitiateTransaction", input, token, function (value) {
				var res = value.data;
				$("#spandana-modal").modal('hide');
				if (res.status == "200") {

					window.open(res.URL, "_blank");

				}
				else {
					$("#spandana-modal").modal('hide');
					swal('info', res.Reason, 'info');
					return;
				}
			});
		}
		scope.RecievedResp = function (val) {
			//MeesevaVroLogin();
			sessionStorage.setItem("RType",val);
			var url = state.href("ui.ReceivedData");
			//state.go("ui.TransResponse");
			window.open(url, "_blank");
			//state.go('ui.ReceivedData');
		}

		scope.MeesevaVRO = function () {
			MeesevaVroLogin();
			}
		function loadtoken() {


			Login_Services.DEMREVOAPI("GetSpandaGrievanceToken", "", function (value) {

				var res = value.data;
				if (res.StatusCode == "200" && res.Status == "Success") {
					scope.token = res.Token;


				}
				else {
					swal('info', res.Message, 'error');
				}
			});
		}


		scope.GetMandalload = function () {

			scope.mcode = "";
			scope.gpcode = "";
			loadmandalmaster();
		}

		scope.GPLoad = function () {
			scope.gpcode = "";
			loadPanchayatmaster();
		}

		function loaddistmaster() {

			var input = { FTYPE: 4 }

			Login_Services.POSTENCRYPTAPI("GetLGDMaster", input, token, function (value) {

				var res = value.data;
				if (res.Status == "100") {

					scope.distlist = res.DataList;
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

		function loadmandalmaster() {

			var input = { FTYPE: 5, DISTCODE: scope.distcode, RUFLAG: scope.ruflag }

			Login_Services.POSTENCRYPTAPI("GetLGDMaster", input, token, function (value) {

				var res = value.data;
				if (res.Status == "100") {

					scope.mandallist = res.DataList;
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

		function loadPanchayatmaster() {

			var input = { FTYPE: 6, DISTCODE: scope.distcode, RUFLAG: scope.ruflag, MCODE: scope.mcode }

			Login_Services.POSTENCRYPTAPI("GetLGDMaster", input, token, function (value) {

				var res = value.data;
				if (res.Status == "100") {

					scope.gplist = res.DataList;
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


		scope.GetSubject = function () {
			loadDepartments(2);
		}

		scope.GetSpandanaStatus = function () {

			if (scope.sdocnum == "" || scope.sdocnum == undefined || scope.sdocnum == null) {
				swal('info', 'Please Enter Grievance Doc Number', 'info');
				return;
			}
			Login_Services.DEMREVOAPI("GetSpandaGrievanceToken", "", function (value) {

				var res = value.data;
				if (res.StatusCode == "200" && res.Status == "Success") {
					scope.token = res.Token;


					var input = { Statusid: 1, grievancid: scope.sdocnum, token: scope.token };

					Login_Services.POSTREVENUEENCRYPTAPI("GetSpandanaStatusCheck", input, token, function (value) {

						var res = value.data;
						console.log(res);
						if (res.length > 0) {

							scope.divstatus = true;
							scope.SStatusList = res;
							//scope.SpandanaList=
						}
						else {
							scope.divstatus = false;
							swal('info', 'No Data Found', 'info');
							return;
						}
					});
					//loadDepartments(1);
				}
				else {
					swal('info', res.Message, 'error');
				}
			});

		}
		scope.GetSubSubject = function () {
			loadDepartments(3);
		}
		function loadDepartments(val) {
			var token = sessionStorage.getItem("Token");
			var req = {
				ftype: val, ruFlag: scope.Sruflag, hodId: 0, token: scope.token, subjectId: scope.subjectcode
			}

			Login_Services.POSTREVENUEENCRYPTAPI("GetSpandanaDepartments", req, token, function (value) {

				var res = value.data;
				//console.log(res);
				if (res.StatusCode == "200") {
					if (val == "1") {

						scope.departmentlist = res.Data

					}
					else if (val == "2") {
						scope.subjectlist = res.Data
					}
					else if (val == "3") {
						scope.subsubjectlist = res.Data
					}
					else if (val == "4") {
						scope.Searchkeywordlist = res.Data
					}

				}
				else if (res.Status == "428") {
					sessionStorage.clear();
					swal('info', res.Reason, 'info');
					state.go("Login");
					//state.go("Login");
					return;

				}
				else {

					swal('info', res.Message, 'info');
					return;
				}
			});
		}

		scope.GetSeccMandal = function () {
			LoadSeccmaster(2);
		}
		scope.GetSeccVillage = function () {
			LoadSeccmaster(3);
		}
		LoadSeccmaster(1);
		function LoadSeccmaster(ftype) {

			var token = sessionStorage.getItem("Token");
			var req = {
				Ftype: ftype, Fdistrict: scope.mdist, Fruflag: scope.mruflag, Fmandal: scope.mmandal, Fvillage: scope.mvill
			}

			Login_Services.POSTREVENUEENCRYPTAPI("GetSpandanaMaster", req, token, function (value) {

				var res = value.data;
				console.log(res);
				if (res.Status == "100") {
					if (ftype == "1") {

						scope.seccdistlist = res.Details;

					}
					else if (ftype == "2") {
						scope.seccmandallist = res.Details;
					}
					else if (ftype == "3") {
						scope.seccvillagelist = res.Details;
					}

				}
				else if (res.Status == "428") {
					sessionStorage.clear();
					swal('info', res.Reason, 'info');
					state.go("Login");
					//state.go("Login");
					return;

				}
				else {

					swal('info', res.Reason, 'info');
					return;
				}
			});
		}

		scope.GetSameMaster = function () {

			scope.Samedistlist = $.grep(scope.seccdistlist, function (p) { return p.DISTRICT_CODE == scope.sdist; })
				.map(function (p) { return p });

			//LoadSeccmaster(2);


			scope.SameMandallist = $.grep(scope.seccmandallist, function (p) { return p.TEHSIL_CODE == scope.SMandal; })
				.map(function (p) { return p });

			//LoadSeccmaster(3);

			scope.Samevilllist = $.grep(scope.seccvillagelist, function (p) { return p.VT_CODE == scope.Svillage; })
				.map(function (p) { return p });
		}
		scope.SpandanaSubmit = function () {

			if (scope.spandanauid == "" || scope.spandanauid == null || scope.spandanauid == undefined) {
				swal('info', 'Please Enter Aadhaar Number', 'info');
				return;
			}
			if (scope.spandanauid.length < 12) {
				swal('info', 'Please Enter 12 Digit Aadhaar Number', 'info');
				return;
			}

			if (scope.Sname == "" || scope.Sname == null || scope.Sname == undefined) {
				swal('info', 'Please Enter Name', 'info');
				return;
			}

			if (scope.Sfathername == "" || scope.Sfathername == null || scope.Sfathername == undefined) {
				swal('info', 'Please Enter Father Name', 'info');
				return;
			}


			if (scope.Sage == "" || scope.Sage == null || scope.Sage == undefined) {
				swal('info', 'Please Enter Age', 'info');
				return;
			}

			if (scope.Sdoorno == "" || scope.Sdoorno == null || scope.Sdoorno == undefined) {
				swal('info', 'Please Enter Door Number', 'info');
				return;
			}
			if (scope.Shabitation == "" || scope.Shabitation == null || scope.Shabitation == undefined) {
				swal('info', 'Please Enter Habitation', 'info');
				return;
			}


			if (scope.sdist == "" || scope.sdist == null || scope.sdist == undefined) {
				swal('info', 'Please Select District', 'info');
				return;
			}
			if (scope.Sruflag == "" || scope.Sruflag == null || scope.Sruflag == undefined) {
				swal('info', 'Please Select Rural/Urban', 'info');
				return;
			}

			if (scope.SMandal == "" || scope.SMandal == null || scope.SMandal == undefined) {
				swal('info', 'Please Select Mandal', 'info');
				return;
			}
			if (scope.Svillage == "" || scope.Svillage == null || scope.Svillage == undefined) {
				swal('info', 'Please Select Village', 'info');
				return;
			}


			if (scope.Sgender == "" || scope.Sgender == null || scope.Sgender == undefined) {
				swal('info', 'Please Enter Gender', 'info');
				return;
			}
			if (scope.Smob == "" || scope.Smob == null || scope.Smob == undefined) {
				swal('info', 'Please Enter Mobile Number', 'info');
				return;
			}

			if (scope.Smob.length < 10) {
				swal('info', 'Please Enter 10 Digit Mobile Number', 'info');
				return;
			}

			if (scope.Soccuption == "" || scope.Smob == null || scope.Smob == undefined) {
				swal('info', 'Please Enter 10 Digit Mobile Number', 'info');
				return;
			}

			if (scope.mdist == "" || scope.mdist == null || scope.mdist == undefined) {
				swal('info', 'Please Select District', 'info');
				return;
			}
			if (scope.mruflag == "" || scope.mruflag == null || scope.mruflag == undefined) {
				swal('info', 'Please Select Rural/Urban', 'info');
				return;
			}

			if (scope.mmandal == "" || scope.mmandal == null || scope.mmandal == undefined) {
				swal('info', 'Please Select Mandal', 'info');
				return;
			}
			if (scope.mvill == "" || scope.mvill == null || scope.mvill == undefined) {
				swal('info', 'Please Select Village', 'info');
				return;
			}
			if (scope.rdntype == "1") {
				if (scope.subjectcode == "" || scope.subjectcode == null || scope.subjectcode == undefined) {
					swal('info', 'Please Select Subject', 'info');
					return;
				}
				if (scope.subsubjectcode == "" || scope.subsubjectcode == null || scope.subsubjectcode == undefined) {
					swal('info', 'Please Select Sub Subject', 'info');
					return;
				}
				else {
				}

			}
			if (scope.rdntype == "2") {

				if (scope.subsubjectcode == "" || scope.subsubjectcode == null || scope.subsubjectcode == undefined) {
					swal('info', 'Please Select Sub Subject', 'info');
					return;
				}
				else {
				}

			}
			if (scope.rdntype == "3") {
				if (scope.hodid == "" || scope.hodid == null || scope.hodid == undefined) {
					swal('info', 'Please Select Department', 'info');
					return;
				}
				if (scope.subjectcode == "" || scope.subjectcode == null || scope.subjectcode == undefined) {
					swal('info', 'Please Select Subject', 'info');
					return;
				}
				if (scope.subsubjectcode == "" || scope.subsubjectcode == null || scope.subsubjectcode == undefined) {
					swal('info', 'Please Select Sub Subject', 'info');
					return;
				}
				else {
				}

			}
			var username = sessionStorage.getItem("user");
			var input = {

				AadhaarNumber: scope.spandanauid, HouseHoldId: scope.Shhid, ApplicantName: scope.Sname, CareOf: scope.Sfathername,
				Age: scope.Sage, Gender: scope.Sgender, PinCode: scope.Spincode, Mobile: scope.Smob, Email: scope.Semail, Income: scope.Sincome,
				Occupation: scope.Soccuption, AppTypeInfo: scope.apptype, AppCreator: username, MkmDistCode: scope.mdist, MkmMandalCode: scope.mmandal, MkmVillageCode: scope.mvill,
				PssDistCode: scope.sdist, PssMandalCode: scope.SMandal, PssVillageCode: scope.Svillage, HodId: scope.hodid, FormID: scope.subsubjectcode,
				ProblemDetails: scope.SRemarks, Habitation: scope.Shabitation, PresentAddress: scope.Sdoorno, token: scope.token, PinCode: scope.Spincode, Loginuser: username
			}

			var token = sessionStorage.getItem("Token");


			Login_Services.POSTREVENUEENCRYPTAPI("SapandaSubmit", input, token, function (value) {
				var res = value.data;
				console.log(res);
				if (res.StatusCode == "200") {
					swal('info', res.Message, 'info');
				}
				else if (res.Status == "428") {
					sessionStorage.clear();
					swal('info', res.Reason, 'info');
					//state.go("Login");
					state.go("Login");
					return;

				}
				else {

					swal('info', res.Message, 'info');
				}
			});

		}

		function LoadSpandanaDashboard() {
			scope.SpandanaRegcount = 0; scope.SpandanaResolvedcount = 0; scope.SpandanaPendingcount = 0;
			Login_Services.DEMREVOAPI("GetSpandaGrievanceToken", "", function (value) {

				var res = value.data;
				if (res.StatusCode == "200" && res.Status == "Success") {
					scope.token = res.Token;

					var token = sessionStorage.getItem("Token");
					var input = { Statusid: 1, SeccCode: sessionStorage.getItem("secccode"), token: scope.token };

					Login_Services.POSTREVENUEENCRYPTAPI("GetSpandanaSideDashboard", input, token, function (value) {

						var res = value.data;
						if (res.StatusCode == "200" && res.Status == "Success") {
							scope.SpandanaRegcount = res.Data[0].TOTAL;
							scope.SpandanaResolvedcount = res.Data[0].RESOLVED;
							scope.SpandanaPendingcount = res.Data[0].PENDING;

						}
						else {

							//swal('info', res.Message, 'info');
							return;
						}
					});
					//loadDepartments(1);
				}
				else {
					//swal('info', res.Message, 'error');
				}
			});
		}

		scope.GetSpandanaDashboard = function (val) {

			sessionStorage.setItem("Spandanatype", val);
			var url = state.href("ui.SPandanaGrievance");
			//state.go("ui.TransResponse");
			window.open(url, "_blank");
		}

	}
		app.filter('unique', function () {
			return function (arr, field) {
				return _.uniq(arr, function (a) { return a[field]; });
			};
		});


	
})();

