(function () {
    var app = angular.module("GSWS");

    app.controller("TransResponseController", ["$scope", "$state", "$log", "Login_Services", Main_CTRL]);

    function Main_CTRL(scope, state, log, Login_Services) {
        //alert(rs.username);
		var username = sessionStorage.getItem("user");
		var secid = sessionStorage.getItem("secccode");
		var desingation = sessionStorage.getItem("desinagtion")
        scope.username = username;
        scope.ldate = new Date().toLocaleDateString();

        var token = sessionStorage.getItem("Token");
		if (username == null || username == undefined || token == null || token == undefined) {
			
			state.go("Login");
			return;
		}
        LoadTransResponse();

        function LoadTransResponse() {
            var input = { type: 2, user: username }
           // var input = { type: 2, secid: secid }
            Login_Services.POSTENCRYPTAPI("GetTransactionResponse", input, token, function (value) {
                var res = value.data;
                if (res.Status == "100") {
                    //$('#myTable').dataTable().fnClearTable();
                    //$('#myTable').dataTable().fnDestroy();
                    scope.TranDetals = res.DataList;
                    //$('#myTable').DataTable();
                }
				else if (res.Status == "428") {
					sessionStorage.clear();
					swal('info', res.Reason, 'info');
					state.go("Login");
					return;

				}
                else {
                    swal('info', res.Reason, 'info');
                }

            });
        }

		scope.GetPrint = function (data) {

			sessionStorage.setItem("pData", JSON.stringify(data));
			
           var  url=state.href("Print")
			//state.go("ui.TransResponse");
			window.open(url,"_blank");
			//state.go("Print");

		}
		scope.GetCheckStatus=function()
		{
			
			window.open("http://gramawardsachivalayam.ap.gov.in/PS1/#!/ArogyarakshaStatus","_blank");
		}

    }
})();