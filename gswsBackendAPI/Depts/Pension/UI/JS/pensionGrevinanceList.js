(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("pensionGrevinanceListController", ["$scope", "pensionDeptServices", "$state", pensionGrevinanceList_CTRL]);


	function pensionGrevinanceList_CTRL(scope, ps, $state) {

		scope.token = sessionStorage.getItem("Token");
		scope.greList = '';

		scope.loadGrevences = function () {
			scope.loader = true;
			var requestData = {
				secretariatId: sessionStorage.getItem("secccode")
			};
			ps.encrypt_post("grevianceList", requestData, scope.token, function (data) {
				var res = data.data;
				if (res.status) {
					scope.greList = res.result;
				}
				else {
					alert(res.result);
				}
				scope.loader = false;
			}, function (error) {
				scope.loader = false;
				console.log(error);
			});

		};

		scope.loadGrevences();

		scope.btnViewGrevinance = function (txnId, grevId) {
			$state.go('ui.pensionWEAVerification', { txnId: txnId, grevId: grevId });
			//var url = "/#!/pensionWEAVerification?txnId=" + txnId + "&grevId=" + grevId;
			//window.open(url, "_Self");
			//return;
		};
	}


})();