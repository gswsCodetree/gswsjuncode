(function () {
	var app = angular.module("GSWS");

	app.controller("CumilativeDashboardController", ["$scope", "$state", "$log", "Login_Services", CumilativeDashboardController]);

	function CumilativeDashboardController(scope, state, log, Login_Services) {
		scope.token = sessionStorage.getItem("Token");
		Checksessionexpire();
		getdata("11",null,null);

		//Get Data
		function getdata(type, fromdate, todate) {
			if (type == "12" && (!scope.fromdate)) {
				swal("", "Please Select From Date to Get Data");
				return;
			}
			else if (type == "12" && (!scope.todate)) {
				swal("", "Please Select TO Date to Get Data");
				return;
			}
			else {
				scope.preloader = true;
				var req = {
					TYPE: type,
					FROMDATE: fromdate,
					TODATE: todate
				}
				Login_Services.POSTENCRYPTAPI("GetCumilativeDashboardData", req, scope.token, function (data) {
					scope.preloader = false;
					var res = data.data;
					if (res.Status == '100') {
						scope.distransactiontotal =0;
						scope.disdeliveredtotal = 0;
						scope.disapprovaltotal = 0;
						scope.disresponsetotal = 0;
						scope.deptransactiontotal = 0;
						scope.depdeliveredtotal = 0;
						scope.depapprovaltotal = 0;
						scope.depresponsetotal = 0;
						scope.statedata=[];
						scope.DistrictData=[];
						scope.DepartmentData=[];
						for (var i = 0; i <= res.DataList.length; i++) {
							if (res.DataList[i]["ROLE"] == "0") {
								scope.statedata.push(res.DataList[i]);
								scope.NO_OF_TXNS = scope.statedata[0]["NO_OF_TXNS"];
								scope.SERVICE_DELIVERED = scope.statedata[0]["SERVICE_DELIVERED"];
								scope.PEND_FOR_APPROVAL = scope.statedata[0]["PEND_FOR_APPROVAL"];
								scope.RESPONSE_REC = scope.statedata[0]["RESPONSE_REC"];
							}
							else if (res.DataList[i]["ROLE"] == "1") {
								scope.DistrictData.push(res.DataList[i]);
								scope.distransactiontotal += parseInt(res.DataList[i]["NO_OF_TXNS"]);
								scope.disdeliveredtotal += parseInt(res.DataList[i]["SERVICE_DELIVERED"]);
								scope.disapprovaltotal += parseInt(res.DataList[i]["PEND_FOR_APPROVAL"]);
								scope.disresponsetotal += parseInt(res.DataList[i]["RESPONSE_REC"]);
							}
							else if (res.DataList[i]["ROLE"] == "2") {
								scope.DepartmentData.push(res.DataList[i]);
								scope.deptransactiontotal += parseInt(res.DataList[i]["NO_OF_TXNS"]);
								scope.depdeliveredtotal += parseInt(res.DataList[i]["SERVICE_DELIVERED"]);
								scope.depapprovaltotal += parseInt(res.DataList[i]["PEND_FOR_APPROVAL"]);
								scope.depresponsetotal += parseInt(res.DataList[i]["RESPONSE_REC"]);
							}
						}
					}
					else {
						swal("", res.Reason, "success");
					}
				});
			}
		}

		//Check Session Expire
		function Checksessionexpire() {
			if (!(scope.token)) {
				swal({
					title: "OOPS!",
					text: "Session expired..!",
					icon: "error"
				})

					.then((value) => {
						if (value) {
							state.go("Login");
						}
					});
			}
		}
	}
})();