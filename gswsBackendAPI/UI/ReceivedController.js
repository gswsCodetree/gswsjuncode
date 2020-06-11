(function () {
    var app = angular.module("GSWS");

    app.controller("ReceivedController", ["$scope", "$state", "$log", "Login_Services", Main_CTRL]);

    function Main_CTRL(scope, state, log, Login_Services) {
        //alert(rs.username);
        var username = sessionStorage.getItem("user");
		var secid = sessionStorage.getItem("secccode");
		var token = sessionStorage.getItem("Token");
		var desingation = sessionStorage.getItem("desinagtion");
		if (username == null || username == undefined || token == null || token == undefined) {
			
			state.go("Login");
			return;
		}
        scope.username = username;
        scope.ldate = new Date().toLocaleDateString();

       

        LoadReceivedData();

        scope.MainAction = function (transid, type) {
            sessionStorage.setItem("rtransid", transid);
            sessionStorage.setItem("rsavetype", type);
            $("#myModal12").modal('show');
            //swal('info', transid + " " + type, 'info');
        }

        scope.getclose = function () {
            $("#myModal12").modal('hide');
        }

        scope.AddRemarks = function () {
            if (!scope.remarks) {
                swal('info', "Please Enter Remarks", 'info');
                return false;
            }
            else {
                var input = { type: 4, transid: sessionStorage.getItem("rtransid"), remarks: scope.remarks, savetype: sessionStorage.getItem("rsavetype")}
                Login_Services.POSTENCRYPTAPI("SaveReceiveAction", input, token, function (value) {
                    var res = value.data;
                    if (res.Status == 100) {
                        swal('Info', "Remarks Added Successfully.", 'info');
                        $("#myModal12").modal('hide');
                    }
                    else {
                        swal('info', res.Reason, 'info');
                    }

                });
            }
        }

        function LoadReceivedData() {
			var ftype=sessionStorage.getItem("RType");
			scope.servicetype=ftype;
			var input ="";
            //var input = { type: 1, user: username }
			if(ftype=="1")
			 input = { type: 8, user: username } //secid
		else
			 input = { type: 7, user: username } //secid
            Login_Services.POSTENCRYPTAPI("GetTransactionResponse", input, token, function (value) {
                var res = value.data;
                if (res.Status == 100) {
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

			var url = state.href("Print")
			//state.go("ui.TransResponse");
			window.open(url, "_blank");
			//state.go("Print");

		}
    }
})();