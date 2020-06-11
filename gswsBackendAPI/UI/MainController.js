(function () {
	var app = angular.module("GSWS");

	app.controller("MainController", ["$scope", "$state", "$log", "Login_Services", Main_CTRL]);

	function Main_CTRL(scope, state, log, Login_Services) {
		//alert(rs.username);
		var username = localStorage.getItem("username")
		var desingation = localStorage.getItem("desinagtion")
		scope.username = username;
		scope.ldate = new Date().toLocaleDateString();

		var token = sessionStorage.getItem("Token");

		scope.GetYSRVerification = function () {

		 var input={ Ftype: 1, Fusername:"YSRRB" }

			Login_Services.POSTENCRYPTAPI("GetRBEncrypt", input, token, function (value) {
				var res = value.data;
				if (res.Status == "100") {

					var url = "http://ysrrythubharosa.ap.gov.in/RBPSApp/RB/decryptPage??Id=" + res.encrypttext + "^" + res.key ;
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


		scope.apedcl_redirection = function () {
			var req = {
				secratariat_id: "785512"
			};
			Login_Services.POSTENCRYPTAPI("apEpdcl_EncryptionData", req, token, function (value) {
				var res = value.data;
				if (res.Status == "100") {
                    var base_url = "";
                    if (type == 1) {
                        base_url = "http://59.144.184.77:8085/EPDCL_GSWS/newConnectionService?Id=";
                    }
                    else if (type == 2) {
                        base_url = "http://59.144.184.77:8085/EPDCL_GSWS/loadChangeService?Id=";
                    } else if (type == 3) {
                        base_url = "http://59.144.184.77:8085/EPDCL_GSWS/categoryChangeNewService?Id=";
                    } else if (type == 4) {
                        base_url = "http://59.144.184.77:8085/EPDCL_GSWS/nameChangeNewService?Id=";
                    } else if (type == 5) {
                        base_url = "http://59.144.184.77:8085/EPDCL_GSWS/applConsumerServicesService?Id=";
                    }
                    var url = base_url + res.encrypttext + "^" + res.key;
                    window.open(url, '_blank');
                    return;
				}
				else {
					swal('info', 'Invalid Request', 'info');
				}


			});
		};



        function gettransIntitate(obj) {

            var input = { DEPT_ID: obj.DEPT_ID, HOD_ID:obj.HOD_ID, SERVICE_ID:obj.SERVICE_ID, DISTRICT_ID:scope.distcode, MANDAL_ID:scope.mcode, GP_WARD_ID:scope.gpcode, LOGIN_USER:username, TYPE_OF_REQUEST:scope.reqtype, URL_ID:obj.URL_ID, SECRETRAINT_CODE:scope.secratariat_id,};

            Login_Services.POSTTRANSCRYPTAPI("initiateTransaction", input, token, function (value) {
                var res = value.data;
                if (res.Status == "200") {

                    scope.translist = res.Translist;

                    var url = "http://ysrrythubharosa.ap.gov.in/RBPSApp/RB/decryptPage?Id=" + res.encrypttext + "^" + res.key;
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
	//	alert(2);
		// scope.pagename = "Login";
		scope.GetLogout = function ()
		{
			localStorage.clear();
			sessionStorage.clear();
			state.go("Login")
		}

	}
})();

