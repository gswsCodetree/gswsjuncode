
(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("unMappedRcController", ["$scope", "$state", "unMappedPensionersMappingServices", "unMappedRcServices", pension_CTRL]);

	function pension_CTRL(scope, state, Pension_Services, unMappedRcServices) {

		scope.token = sessionStorage.getItem("Token");
		scope.secccode = sessionStorage.getItem("secccode");
		scope.Loader = false;


		scope.loadriceCards = function () {
			scope.Loader = true;
			var requestData = {
				gsws_code: scope.secccode
			};
			unMappedRcServices.POSTENCRYPTAPI("loadRiceCards", requestData, scope.token, function (value) {
				if (value.data.status == 200) {
					scope.riceCards = value.data.result;
					console.log("loadRiceCards	 : ", scope.riceCards);
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
					scope.loadriceCards();
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

		scope.btnSubmit = function () {
			if (scope.cluster_id == "" || scope.cluster_id == null || scope.cluster_id == undefined) {
				alert("Please select cluster");
				return;
			}
			if (scope.personUid == "" || scope.personUid == null || scope.personUid == undefined) {
				alert("Please select Rice Cards");
				return;
			}

			if (confirm("are you sure want to add rice card to the " + JSON.parse(scope.cluster_id).CLUSTER_NAME + " cluster ?")) {

				scope.Loader = true;
				var url = 'assignClusterToRc';
				var requestData = {
					gsws_code: scope.secccode,
					uid: scope.personUid,
					cluster_id: JSON.parse(scope.cluster_id).CLUSTER_ID
				};
				unMappedRcServices.POSTENCRYPTAPI(url, requestData, scope.token, function (value) {
					if (value.data.status == 200) {
						alert(value.data.result);
						scope.loadriceCards();
					}
					else {
						alert(value.data.result);
						scope.loadriceCards();
					}
					scope.Loader = false;
				}, function (error) {
					scope.Loader = false;
					console.log(error);
					scope.loadriceCards();
				});
			}
		};

	}


})();