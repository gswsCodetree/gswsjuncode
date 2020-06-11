
(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("unMappedPensionersMappingController", ["$scope", "$state", "unMappedPensionersMappingServices", pension_CTRL]);

	function pension_CTRL(scope, state, Pension_Services) {

		scope.token = sessionStorage.getItem("Token");
		scope.secccode = sessionStorage.getItem("secccode");
		scope.Loader = false;


		scope.loadPensioners = function () {
			scope.Loader = true;
			var requestData = {
				gsws_code: scope.secccode
			};
			Pension_Services.POSTENCRYPTAPI("loadPensioners", requestData, scope.token, function (value) {
				if (value.data.status == 200) {
					scope.pensioners = value.data.result;
					console.log("pensioners : ", scope.pensioners);
				}
				else {
					alert(value.data.result);
				}
				scope.Loader = false;
			}, function (error) {
				console.log(error);
				scope.Loader = false;
			});
		};


		scope.getDetails = function () {
			scope.Loader = true;
			scope.clusters = '';
			scope.pensioners = '';
			var requestData = {
				gsws_code: scope.secccode
			};
			Pension_Services.POSTENCRYPTAPI("loadClusters", requestData, scope.token, function (value) {
				if (value.data.status == 200) {
					scope.clusters = value.data.result;
					console.log("clusters : ", scope.clusters);
					scope.loadPensioners();
				}
				else {
					alert(value.data.result);
				}
				scope.Loader = false;
			}, function (error) {
				scope.Loader = false;
				console.log(error);
			});


		};

		scope.getDetails();

		scope.subFunc = function () {
			if (scope.cluster_id == "" || scope.cluster_id == null || scope.cluster_id == undefined) {
				scope.submission = false;
				return;
			}
			if (scope.pensionerUid == "" || scope.pensionerUid == null || scope.pensionerUid == undefined) {
				scope.submission = false;
				return;
			}
			scope.submission = true;
		};

		scope.ddClusterChange = function () {
			scope.subFunc();
		};

		scope.ddPensionerChange = function () {
			scope.subFunc();
		};

		scope.btnSubmit = function () {

			if (confirm("are you sure want to add pensioner to the cluster ?")) {
				if (scope.cluster_id == "" || scope.cluster_id == null || scope.cluster_id == undefined) {
					alert("Please select cluster");
					return;
				}
				if (scope.pensionerUid == "" || scope.pensionerUid == null || scope.pensionerUid == undefined) {
					alert("Please select pensioner");
					return;
				}

				var url = 'assignClusterToPensioner';
				var requestData = {
					gsws_code: scope.secccode,
					uid: scope.pensionerUid,
					cluster_id: scope.cluster_id
				};
				Pension_Services.POSTENCRYPTAPI(url, requestData, scope.token, function (value) {
					if (value.data.status == 200) {
						alert(value.data.result);
						scope.loadPensioners();
						return;
					}
					else {
						alert(value.data.result);
						scope.loadPensioners();
						return;
					}
				}, function (error) {
					console.log(error);
					scope.loadPensioners();
				});
			}
		};

	}


})();