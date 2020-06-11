
(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("RationVolunteerMappingController", ["$scope", "$state", "Ration_Services", "$filter", Ration_CTL]);


	function Ration_CTL(scope, state, Ration_Services, filter) {

		//Pagination Code
		scope.curPage = 1;
		scope.itemsPerPage = 50;
		scope.filteredItems = [];
		scope.paginationPages = [];

		scope.token = sessionStorage.getItem("Token");
		scope.secccode = sessionStorage.getItem("secccode");

		scope.tableData = false;
		scope.Loader = false;
		scope.user = {
			data: []
		};

		scope.loadClusters = function () {
			scope.clusters = '';
			scope.rationers = '';
			var requestData = {
				psec_id: scope.secccode
			};
			Ration_Services.POSTENCRYPTAPI("loadClusters", requestData, scope.token, function (data) {
				var res = data.data;
				console.log(res);
				if (res.status == 200) {
					scope.clusters = res.result;

				}
				else {
					alert(res.result);
				}
			}, function (error) {
				console.log(error);
			});
		};
		scope.loadClusters();

		scope.loadRationMembers = function () {
			scope.user.data = [];
			scope.tableData = false;
			scope.Loader = true;
			var requestData = {
				psec_id: scope.secccode
			};
			Ration_Services.POSTENCRYPTAPI("loadRationMembers", requestData, scope.token, function (data) {
				var res = data.data;
				console.log(res);
				if (res.status == 200) {
					scope.rationers = res.result;
					console.log(scope.rationers);
					console.log(scope.numOfPages());

					scope.tempVal = 5;
					scope.totalPages = scope.numOfPages();
					for (var i = 0; i < 5; i++) {
						scope.paginationPages.push(i + 1);
					}
					scope.DataSlice();

				}
				else {
					alert(res.result);
				}
				scope.Loader = false;
				scope.tableData = true;
			}, function (error) {
				console.log(error);
				scope.Loader = false;
				scope.tableData = true;
			});
		};


		scope.ddClusterChange = function () {
			scope.riceCardDetails = '';
			scope.riceCardId = '';
			if (scope.cluster_id == '' || scope.cluster_id == undefined || scope.cluster_id == null) {
				scope.rationers = '';
				return;
			}
			scope.cluster_data = JSON.parse(scope.cluster_id);
			console.log(scope.cluster_data);
			scope.loadRationMembers();
		};

		scope.btnSubmit = function () {

			var url = 'assignRationToCluster';
			if (scope.cluster_id == '' || scope.cluster_id == undefined || scope.cluster_id == null) {
				Swal.fire('Info', 'Please select Cluster', 'info');
				return;
			}
			var requestData;
			if (scope.actionId == "2") {
				if (scope.newSecId == '' || scope.newSecId == undefined || scope.newSecId == null) {
					Swal.fire('Info', 'Please select Secretariat', 'info');
					return;
				}
				url = 'unassignRationToCluster';
				requestData = {
					psec_id: scope.newSecId,
					user_data: scope.user.data,
					pvv_id: null,
					pvv_name: scope.cluster_data.VOLUNTEER_NAME,
					pCLUSTER_ID: scope.cluster_data.CLUSTER_ID,
					pCLUSTER_NAME: scope.cluster_data.CLUSTE,
					pUPDATED_BY: sessionStorage.getItem("uniqueid")
				};
			}
			else {
				requestData = {
					psec_id: scope.secccode,
					user_data: scope.user.data,
					pvv_id: null,
					pvv_name: scope.cluster_data.VOLUNTEER_NAME,
					pCLUSTER_ID: scope.cluster_data.CLUSTER_ID,
					pCLUSTER_NAME: scope.cluster_data.CLUSTE,
					pUPDATED_BY: sessionStorage.getItem("uniqueid")
				};
			}

			Ration_Services.DemoAPI(url, requestData, function (value) {
				if (value.data.status == 200) {
					alert(value.data.result);
					$("#rationerListModal").modal('hide');
					scope.confirmCheckBox = false;
					scope.loadRationMembers();
					return;
				}
				else {
					alert(value.data.result);
					$("#rationerListModal").modal('hide');
					scope.confirmCheckBox = false;
					scope.loadRationMembers();
					return;
				}
			}, function (error) {
				console.log(error);
				$("#rationerListModal").modal('hide');
				scope.confirmCheckBox = false;
				scope.loadRationMembers();
			});

		};

		scope.btnPreview = function () {
			scope.actionId = '';
			scope.gswsDropdown = false;
			scope.checkBoxFlag = false;
			$("#rationerListModal").modal('show');
		};

		scope.loadsecList = function () {
			scope.secList = '';
			var requestData = {
				psec_id: scope.secccode
			};
			Ration_Services.POSTENCRYPTAPI("secList", requestData, scope.token, function (data) {
				var res = data.data;
				console.log(res);
				if (res.status == 200) {
					scope.secList = res.result;

				}
				else {
					alert(res.result);
				}
			}, function (error) {
				console.log(error);
			});
		};

		scope.ddActionChange = function () {
			if (scope.actionId == '' || scope.actionId == undefined || scope.actionId == null) {
				scope.secList = '';
				scope.gswsDropdown = false;
				scope.checkBoxFlag = false;
				return;
			}
			scope.confirmCheckBox = false;
			scope.checkStatus = true;
			if (scope.actionId == '1') {
				scope.gswsDropdown = false;
				scope.checkBoxFlag = true;
			}
			else {
				scope.gswsDropdown = true;
				scope.checkBoxFlag = false;
				scope.loadsecList();
			}
		};

		scope.ddnewSecIdChange = function () {
			scope.confirmCheckBox = false;
			if (scope.newSecId == '' || scope.newSecId == undefined || scope.newSecId == null) {
				scope.secList = '';
				scope.checkBoxFlag = false;
				return;
			}
			scope.checkBoxFlag = true;
		};
		scope.btnAddRiceCard = function () {
			scope.riceCardDetails = '';
			scope.riceCardId = '';
			$("#addRiceCardModal").modal('show');
		};

		scope.btnSearchRiceCard = function () {
			if (scope.riceCardId == '' || scope.riceCardId == null || scope.riceCardId == undefined) {
				alert('Please Enter Rice Card ID');
				return;
			}
			var requestData = {
				psec_id: scope.secccode,
				user_data: [{ EXISTING_RC_NUMBER: scope.riceCardId }]
			};
			Ration_Services.POSTENCRYPTAPI("SearchRiceCard", requestData, scope.token, function (data) {
				var res = data.data;
				console.log(res);
				if (res.status == 200) {
					scope.riceCardDetails = res.result;
				}
				else {
					alert(res.result);
				}
			}, function (error) {
				console.log(error);
			});
		};

		scope.btnSubmitRiceCard = function () {
			var requestData;
			if (scope.riceCardDetails[0].DISTRICT_STATUS == "1") {
				if (confirm("You're adding rice card from another district, Rice card will be added to the cluster after the approved by the JOINT COLLECTOR")) {
					requestData = {
						pCLUSTER_ID: JSON.parse(scope.cluster_id).CLUSTER_ID,
						psec_id: scope.secccode,
						user_data: scope.riceCardDetails,
						pUPDATED_BY: sessionStorage.getItem("uniqueid")
					};

					Ration_Services.POSTENCRYPTAPI("reqRiceCardToCluster", requestData, scope.token, function (value) {
						if (value.data.status == 200) {
							alert(value.data.result);
							$("#addRiceCardModal").modal('hide');
							return;
						}
						else {
							alert(value.data.result);
							$("#addRiceCardModal").modal('hide');
							return;
						}
					}, function (error) {
						console.log(error);
						$("#rationerListModal").modal('hide');
						return;
					});
				}

			}
			else {
				requestData = {
					pCLUSTER_ID: JSON.parse(scope.cluster_id).CLUSTER_ID,
					psec_id: scope.secccode,
					user_data: scope.riceCardDetails,
					pUPDATED_BY: sessionStorage.getItem("uniqueid")
				};

				Ration_Services.POSTENCRYPTAPI("assignRiceCardToCluster", requestData, scope.token, function (value) {
					if (value.data.status == 200) {
						alert(value.data.result);
						$("#addRiceCardModal").modal('hide');
						return;
					}
					else {
						alert(value.data.result);
						$("#addRiceCardModal").modal('hide');
						return;
					}
				}, function (error) {
					console.log(error);
					$("#rationerListModal").modal('hide');
					return;
				});
			}



		};

		scope.searchChange = function (val) {
			if (val.length < 3) {
				var begin = ((scope.curPage - 1) * scope.itemsPerPage);
				end = begin + scope.itemsPerPage;
				scope.filteredItems = scope.rationers.slice(begin, end);
				return;
			}
			scope.filteredItems = filter('filter')(scope.rationers, val);
			console.log(scope.filteredItems);
		};


		//Pagination Code


		//Total No Of Pages the data is
		scope.numOfPages = function () {
			return Math.ceil(scope.rationers.length / scope.itemsPerPage);

		};

		//for showing the current page
		scope.DataSlice = function () {
			var begin = ((scope.curPage - 1) * scope.itemsPerPage);
			end = begin + scope.itemsPerPage;
			scope.filteredItems = scope.rationers.slice(begin, end);
		};

		scope.Selectedindex = function (i) {
			scope.curPage = i;
			scope.DataSlice();
		};

		//For Next Page if current page is less than maximum number
		scope.nextpage = function () {
			var pages = scope.numOfPages();
			if (scope.curPage >= pages)
				return;



			//for pagination values in html chagnes 
			if (scope.curPage == scope.tempVal) {

				scope.paginationPages.splice(0, 1);
				scope.paginationPages.push(scope.curPage + 1);
				scope.tempVal = scope.curPage + 1;
			}


			scope.curPage += 1;
			scope.DataSlice();
		};

		//For Previous Page if current page is greater than 1
		scope.Previousepage = function () {
			if (scope.curPage <= 1)
				return;


			scope.curPage -= 1;
			var leastValue = scope.paginationPages[0];

			//for pagination values in html chagnes 
			if (scope.curPage < leastValue) {

				scope.paginationPages = [];
				for (var i = scope.curPage; i < scope.curPage + 5; i++) {
					scope.paginationPages.push(i);
				}
			}

			scope.tempVal = scope.paginationPages[scope.paginationPages.length - 1];

			scope.DataSlice();
		};



	}

})();