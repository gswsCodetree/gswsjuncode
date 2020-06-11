(function () {
	var app = angular.module("GSWS");

	app.controller("GVDaashboardController", ["$scope", "$state", "$log", "Login_Services", "$filter", Main_CTRL]);

	function Main_CTRL(scope, state, log, Login_Services, $filter) {

		var input = { FTYPE: 1 }
		var username = sessionStorage.getItem("user");		
		var token = sessionStorage.getItem("Token");
		if (username == null || username == undefined || token == null || token == undefined) {

			state.go("Login");
			return;
		}
		Login_Services.POSTENCRYPTAPI("GVDashboardData", input,token, function (value) {
			var res = value.data;
			if (res.Status == "100") {
				// scope.divloader=false;
				scope.main_data = res.DataList;
				scope.totvregph1 = scope.main_data[0].PH1_TOT_REGIS;		
				//alert(scope.totvregph1);
				scope.totvregph2 = scope.main_data[0].PH2_TOT_REGIS
				scope.totvsph1 = scope.main_data[0].PH1_INTERVIEW_SELECTED;
				scope.totvsph2 = scope.main_data[0].PH2_INTERVIEW_SELECTED;
				scope.totcluster = scope.main_data[0].TOT_CLUSTERS;
				scope.tothhtocluster = scope.main_data[0].HH_MAPPED;
				scope.totmemtocluster = scope.main_data[0].MEMBERS_MAPP;
				scope.tothhNmcluster = scope.main_data[0].PENDING_HH;
				scope.totvtocluster = scope.main_data[0].VV_MAPP_CLUS;
				scope.Totvnmtocluster = scope.main_data[0].VV_NOT_MAPP_CLUS;
					
				//console.log(scope.main_data);
				scope.DistDashlist = res.DataDistList;
				scope.ftotreguserph1 = 0, scope.ftotreguserph2 = 0, scope.ftotselctuserph1 = 0, scope.ftotselctuserph2 = 0,
					scope.ftothh = 0, scope.ftothhmapc = 0, scope.ftotmemmapc = 0, scope.ftotcluster = 0, scope.ftothhnotmapcluster = 0, scope.ftotncluster = 0;
				//console.log(scope.DistDashlist);
				for (var i = 0; i < scope.DistDashlist.length; i++) {
					scope.ftotreguserph1 += parseInt(scope.DistDashlist[i].PH1_TOT_REGIS);
					scope.ftotreguserph2 += parseInt(scope.DistDashlist[i].PH2_TOT_REGIS);
					scope.ftotselctuserph1 += parseInt(scope.DistDashlist[i].PH1_INTERVIEW_SELECTED);
					scope.ftotselctuserph2 += parseInt(scope.DistDashlist[i].PH2_INTERVIEW_SELECTED);
					scope.ftothh += parseInt(scope.DistDashlist[i].TOT_HH);
					scope.ftothhmapc += parseInt(scope.DistDashlist[i].HH_MAPPED);
					scope.ftotmemmapc += parseInt(scope.DistDashlist[i].MEMBERS_MAPP);
					scope.ftotcluster += parseInt(scope.DistDashlist[i].VV_MAPP_CLUS);				
					scope.ftotncluster += parseInt(scope.DistDashlist[i].VV_NOT_MAPP_CLUS);
					scope.ftothhnotmapcluster += parseInt(scope.DistDashlist[i].PENDING_HH);
				}
				
			}
			else {
				swal('info', 'Invalid Request', 'info');
			}


		});
	}
})();