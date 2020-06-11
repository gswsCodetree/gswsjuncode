(function () {
	var app = angular.module("GSWS");

	app.controller("AppStatusController", ["$scope", "$state", "$log", "Login_Services", AppStatusController]);

	function AppStatusController(scope, state, log, Login_Services) {
		//alert(rs.username);
		var username = sessionStorage.getItem("user");
		var secid = sessionStorage.getItem("secccode");
		var desingation = sessionStorage.getItem("desinagtion")
		scope.username = username;
        scope.ldate = new Date().toLocaleDateString();
        scope.dttable = false;
		var token = sessionStorage.getItem("Token");
		if (username == null || username == undefined || token == null || token == undefined) {
			
			state.go("Login");
			return;
		}

		scope.GetStatus=function()
        {
            if (!scope.RequestId) {
                swal('info', "Please Enter Request Number", 'info');
                return false;
            }
            else {
                scope.dttable = false;
                var input = { type: 5, secid: scope.RequestId }
                Login_Services.POSTENCRYPTAPI("GetTransactionResponse", input, token, function (value) {
                    var res = value.data;
                    if (res.Status == "100") {
                        scope.dttable = true;
                        scope.ApplicationDetails = res.DataList;
                        console.log(res.DataList);
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
		
		}

        scope.GetPrint = function (tid) {
            sessionStorage.setItem("ptransid", tid);
            var url = state.href("Print")
            window.open(url, "_blank");
		}

	}
})();