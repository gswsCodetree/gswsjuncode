(function () {
	var app = angular.module("GSWS");

	app.controller("HomePoliceController", ["$scope", "$state", "$log", "Home_Services", HomePolice_CTRL]);

	function HomePolice_CTRL(scope, state, log, Home_Services) {
		var token = sessionStorage.getItem("Token"); var user = sessionStorage.getItem("user");
		if (!(token) || !(user)) {
			alert('Session expired..!');
			state.go("Login");
		}
		var input = { Ftype: 1 }

		Home_Services.POSTENCRYPTAPI("LHMSAPPoliceDashbooard", input, token, function (value) {

			var res = value.data;
			if (res.success == true) {

				scope.distlist = res.data;
				scope.todayreguser = res.data.todayRegisteredUser;
				scope.totuser = res.data.totalUsers;
				scope.todaydeploy = res.data.todayRegisteredUser;
				scope.todaywatch = res.data.todayWatchRequest;
				scope.totalwatch = res.data.totalWatchRequest;
				scope.Failurewatch = res.data.failureWatchRequest;
				scope.sucpreWatch = res.data.successfulPreventionWatchRequest;
				scope.realprewatch = res.data.realTimePreventionWatchRequest;

				scope.lhmsmandallist = [];
				for (var i = 0; i < res.data.district.length; i++) {
					for (var j = 0; j < res.data.district[i].towns.length; j++) {
						scope.lhmsmandallist.push(res.data.district[i].towns[j]);
					}
				}
			}
			else {

				swal('info', res.Reason, 'info');
				return;
			}
		});

		scope.GetDateData = function () {

			var input = { Ftype: 2, Startdate: scope.startdate, enddate: scope.enddate }

			Home_Services.POSTENCRYPTAPI("LHMSAPPoliceDashbooard", input, token, function (value) {

				var res = value.data;
				if (res.success == true) {

					scope.distlist = res.data;
					scope.todayreguser = res.data.todayRegisteredUser;
					scope.totuser = res.data.totalUsers;
					scope.todaydeploy = res.data.todayRegisteredUser;
					scope.todaywatch = res.data.todayWatchRequest;
					scope.totalwatch = res.data.totalWatchRequest;
					scope.Failurewatch = res.data.failureWatchRequest;
					scope.sucpreWatch = res.data.successfulPreventionWatchRequest;
					scope.realprewatch = res.data.realTimePreventionWatchRequest;

					scope.lhmsmandallist = [];
					for (var i = 0; i < res.data.district.length; i++) {
						for (var j = 0; j < res.data.district[i].towns.length; j++) {
							scope.lhmsmandallist.push(res.data.district[i].towns[j]);
						}
					}
				}
				else {

					swal('info', res.Reason, 'info');
					return;
				}
			});
		}
	}
})();

