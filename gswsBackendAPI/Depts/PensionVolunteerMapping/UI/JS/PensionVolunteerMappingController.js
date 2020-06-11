
(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("PensionVolunteerMappingServices", ["$scope", "$state", "PensionVolunteerMappingServices", pension_CTRL]);

	function pension_CTRL(scope, state, Pension_Services) {

		scope.token = sessionStorage.getItem("Token");
		scope.secccode = sessionStorage.getItem("secccode");
		scope.tableData = false;
		scope.Loader = false;
		scope.user = {
			data: []
		};


		scope.loadPensioners = function () {
			scope.user.data = [];
			scope.tableData = false;
			scope.Loader = true;
			var requestData = {
				gsws_code: scope.secccode
			};
			Pension_Services.POSTENCRYPTAPI("loadPensioners", requestData, scope.token, function (value) {
				if (value.data.status == 200) {
					scope.pensioners = value.data.result;
					console.log(scope.pensioners);
				}
				else {
					alert(value.data.result);
				}
				scope.Loader = false;
				scope.tableData = true;
			}, function (error) {
				console.log(error);
				scope.Loader = false;
				scope.tableData = true;
			});
		};

		scope.getDetails = function () {

			scope.clusters = '';
			scope.pensioners = '';
			var requestData = {
				gsws_code: scope.secccode
			};
			Pension_Services.POSTENCRYPTAPI("loadClusters", requestData, scope.token, function (value) {
				if (value.data.status == 200) {
					scope.clusters = value.data.result;
				}
				else {
					alert(value.data.result);
				}
			}, function (error) {
				console.log(error);
			});


		};

		scope.btnAddPensioner = function () {
			$("#pensionerMappingModal").modal('show');
			scope.pensionId = '';
			scope.pensioner = '';
			scope.pensionModel = false;
		};

		scope.btnListPensioners = function () {
			scope.pensionersList = '';
			var requestData = {
				gsws_code: scope.secccode
			};
			Pension_Services.POSTENCRYPTAPI("assignedPensionerData", requestData, scope.token, function (value) {
				if (value.data.status == 200) {
					scope.pensionersList = value.data.result;
					$("#pensionerListModal").modal('show');
				}
				else {
					alert(value.data.result);
				}
			}, function (error) {
				console.log(error);
			});
		};

		scope.btnUnAssign = function (pension_id, gsws_code) {
			var requestData = {
				gsws_code: gsws_code,
				user_data: [{ PENSIONID: pension_id }],
				updated_by: sessionStorage.getItem("uniqueid")
			};
			Pension_Services.POSTENCRYPTAPI("unassignPensioner", requestData, scope.token, function (value) {
				alert(value.data.result);
				$("#pensionerMappingModal").modal('hide');
				scope.btnListPensioners();
				return;
			}, function (error) {
				console.log(error);
				scope.btnListPensioners();
			});

		};

		scope.btnSearchPensioner = function () {
			scope.pensioner = '';
			var requestData = {
				pension_id: scope.pensionId
			};
			Pension_Services.POSTENCRYPTAPI("individualPensionData", requestData, scope.token, function (value) {
				if (value.data.status == 200) {
					scope.pensioner = value.data.result[0];
					scope.pensionModel = true;
				}
				else {
					alert(value.data.result);
				}
			}, function (error) {
				console.log(error);
			});
		};

		scope.btnAssignPensioner = function () {
			if (scope.pensionId == null && scope.pensionId == undefined || scope.pensionId == '') {
				alert("Please Enter Pensioner ID");
				return;
			}
			if (scope.pensioner == null && scope.pensioner == undefined || scope.pensioner == '') {
				alert("Pensioner Data Not Available");
				return;
			}
            var url = 'assignSecretariat';

			var requestData = {
				gsws_code: scope.secccode,
				pension_id: scope.pensionId,
				updated_by: sessionStorage.getItem("uniqueid")
			};
			Pension_Services.POSTENCRYPTAPI(url, requestData, scope.token, function (value) {
				alert(value.data.result);
				$("#pensionerMappingModal").modal('hide');
				scope.pensioner = '';
				scope.pensionModel = false;
				return;

			}, function (error) {
				console.log(error);
				scope.loadPensioners();
			});

		};

		scope.ddActionChange = function () {
			scope.tableData = false;
			if (scope.actionId == '' || scope.actionId == undefined || scope.actionId == null) {
				scope.pensioners = '';
				scope.clusters = '';
				scope.vvDropdown = false;
				return;
			}
			if (scope.actionId == '1') {
				scope.vvDropdown = true;
				scope.getDetails();
			}
			else {
				scope.clusters = '';
				scope.vvDropdown = false;
				scope.loadPensioners();
			}
		};

		scope.ddClusterChange = function () {
			if (scope.cluster_id == '' || scope.cluster_id == undefined || scope.cluster_id == null) {
				scope.pensioners = '';
				return;
			}
			scope.cluster_data = JSON.parse(scope.cluster_id);
			console.log(scope.cluster_data);
			scope.loadPensioners();
		};

		scope.btnSubmit = function () {

			var url = 'unassignVounteer';
			if (scope.actionId == '' || scope.actionId == undefined || scope.actionId == null) {
				Swal.fire('Info', 'Please select Action to be taken', 'info');
				return;
			}
			if (scope.actionId == "1") {
				if (scope.cluster_id == '' || scope.cluster_id == undefined || scope.cluster_id == null) {
					Swal.fire('Info', 'Please select Cluster', 'info');
					return;
				}
                url = 'assignVounteer';
			}

			var requestData = {
				gsws_code: scope.secccode,
				user_data: scope.user.data,
				vv_id: null,
				vv_name: scope.cluster_data.VOLUNTEER_NAME,
				cluster_id: scope.cluster_data.CLUSTER_ID,
				cluster_name: scope.cluster_data.CLUSTE,
				updated_by: sessionStorage.getItem("uniqueid")
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

		};

	}


})();