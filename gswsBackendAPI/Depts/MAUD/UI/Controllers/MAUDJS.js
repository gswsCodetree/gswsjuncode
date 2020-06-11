(function () {
	/* eslint-disable */
	var app = angular.module("GSWS");
	app.controller("MAUDController", ["$scope", "$state", "MAUD_Services", '$sce', Cert_CTRL]);
	function Cert_CTRL(scope, state, services, sce) {

		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}
		LoadDistrics();

		function LoadDistrics() {
			var req = {
				TYPE: "4"
			};
			services.DemoAPI("LoadDepartments", req, function (value) {
				if (value.data.Status == "Success") {
					scope.DistricsDD = value.data.Details;
				}
				else {
					alert("Districs Loading Failed");
				}
			});
		}

		scope.GetSubmit = function () {

			if (scope.seldistrict == "" || scope.seldistrict == undefined || scope.seldistrict == null) {
				swal('info', 'Please Select District', 'info');
				return;
			}
			if (scope.selmandal == "" || scope.selmandal == undefined || scope.selmandal == null) {
				swal('info', 'Please Select Mandal', 'info');
				return;
			}
			var req = {FTYPE:2, DISTCODE: scope.seldistrict, MCODE: scope.selmandal}

			services.DemoAPI2("GetAllURLList", req, function (value) {
				if (value.data.Status == "100") {
					//scope.DistricsDD = value.data.Details;
					scope.maindata = value.data.DataList;
					console.log(scope.maindata);
				}
				else {
					alert("Districs Loading Failed");
				}
			});

		}


		scope.process_url = function (obj) {

			gettransIntitate(obj)
		};


		function gettransIntitate(obj) {


			scope.distcode = localStorage.getItem("distcode");
			scope.mcode = localStorage.getItem("mcode");
			scope.gpcode = localStorage.getItem("gpcode");
			scope.secratariat_id = localStorage.getItem("secccode");
			var username = localStorage.getItem("user");
			var token = sessionStorage.getItem("Token");
			var input = { DEPT_ID: obj.SD_ID, HOD_ID: obj.HOD_ID, SERVICE_ID: obj.SD_ID, DISTRICT_ID: scope.distcode, MANDAL_ID: scope.mcode, GP_WARD_ID: scope.gpcode, LOGIN_USER: username, TYPE_OF_REQUEST: obj.TYPE_OF_SERVICE, URL_ID: obj.URL_ID, SECRETRAINT_CODE: scope.secratariat_id };

			services.POSTTRANSCRYPTAPI("initiateTransaction", input, token, function (value) {
				var res = value.data;
				if (res.status == "200") {

					scope.translist = res.Translist;
					var input = "";
					if (obj.TYPE_OF_SERVICE == "1") {
						if (obj.HOD_ID == "1601") {
							input = { USERNAME: "admin", PASSWORD: "admin@123", PS_TXN_ID: res.Translist[0].TRANSACTION_ID, RETURN_URL: "http://prajasachivalayam.ap.gov.in/PSTESTAPP/#!/Login" }
						}
						else if (obj.HOD_ID == "2201") {
							input = { USERNAME: "VS010202501", PASSWORD: "f542540f719f46833b51ce88d8cc5112", PS_TXN_ID: res.Translist[0].TRANSACTION_ID, RETURN_URL: "http://prajasachivalayam.ap.gov.in/PSTESTAPP/#!/Login" }
						}
						else
							input = { USERNAME: username, PASSWORD: username + 1, PS_TXN_ID: res.Translist[0].TRANSACTION_ID, RETURN_URL: "http://prajasachivalayam.ap.gov.in/PSTESTAPP/#!/Login" }

						//var input = { USERNAME: username, PASSWORD: username + 1, PS_TXN_ID: res.Translist[0].TNSId, RETURN_URL: "http://prajasachivalayam.ap.gov.in/PSTESTAPP/#!/Login" }
						services.POSTENCRYPTAPI("GetEncryptthirdparty", input, token, function (value) {
							var res = value.data;
							if (res.Status == "100") {
								var url = "";
								if (obj.HOD_ID == "2201") {
									url = obj.URL + "?Id=" + res.encrypttext + "&iv=" + res.key;
								}
								else
									url = obj.URL + "?Id=" + res.encrypttext + "^" + res.key;

								window.open(url, '_blank');
								//window.open(url, "", "scrollbars=yes,resizable=yes,top=50,left=200,width=1000,height=500");
								//	window.open( "_blank");	
								return;
							}
							else {
								swal('info', 'Invalid Request', 'info');
							}


						});
					}
					else {
						window.open(obj.URL, "_blank");
					}
					//window.open(url, "", "scrollbars=yes,resizable=yes,top=50,left=200,width=1000,height=500");
					//	
					return;
				}
				else {
					swal('info', 'Invalid Request', 'info');
				}


			});
		}

		// Load Madals's
		scope.LoadMadals = function () {
			var req = {
				TYPE: "5",
				DISTRICT: scope.seldistrict,
				RUFLAG:'U'
			};
			services.DemoAPI("LoadDepartments", req, function (value) {
				if (value.data.Status == "Success") {
					scope.MandalsDD = value.data.Details;
				}
				else {
					alert("Mandals Loading Failed");
				}
			});
		}


	}
})();


