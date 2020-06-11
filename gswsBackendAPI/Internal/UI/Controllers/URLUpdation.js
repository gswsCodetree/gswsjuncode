(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("InternalUpdateController", ["$scope", "$state", "$log", "Internal_Services",'entityService', Internal_CTRL]);

	function Internal_CTRL(scope, state, log, Internal_Services, entityService) {
		scope.preloader = false;
		LoadDepartments();
		var username = localStorage.getItem("user");
		//Submit
		scope.Savedata = function () {

			if (scope.selDept == null || scope.selDept == undefined || scope.selDept == "") {
				swal('info', 'Please Select Department', 'info');
				return;
			}

			if (scope.selHOD == null || scope.selHOD == undefined || scope.selHOD == "") {
				swal('info', 'Please Select HOD', 'info');
				return;
			}

			if (scope.selservice == null || scope.selservice == undefined || scope.selservice == "") {
				swal('info', 'Please Select Service name', 'info');
				return;
			}

			if (scope.selurlid == null || scope.selurlid == undefined || scope.selurlid == "") {
				swal('info', 'Please Select Url Description', 'info');
				return;
			}

			if (scope.userlink == undefined || scope.userlink == null || scope.userlink == "") {

				swal('info', 'Please Upload File', 'info');
				return;
			}

			var req = { DEPTID: scope.selDept, HODID: scope.selHOD, SERVICEID: scope.selservice, URL_ID: scope.selurlid, USERMAUALID: scope.userlink, USER:username}
			Internal_Services.DemoAPI("UpdateUserManual", req, function (value) {
				if (value.data.Status == "100") {
					swal('info', value.data.Reason, 'info');
					return;
				}
				else {
					swal('info', value.data.Reason, 'error');
				}
			});
		}

		scope.uploadfile = function (type, category, categoryid) {

			if (scope.selHOD == null || scope.selHOD == undefined || scope.selHOD == "") {
				swal('info', 'Please Select HOD', 'info');
				return;
			}
			if (scope.selurlid == null || scope.selurlid == undefined || scope.selurlid == "" || categoryid==undefined) {
				swal('info', 'Please Select Url Description', 'info');
				return;
			}
			var file = type.attachment;
			var fileexten = file.type;
			var FileSize = file.size;
			if (FileSize > 5000000) {
				swal('info', 'File size exceeded 5 MB', 'error');
				scope.preloader = false;
				angular.element("input[type='file']").val('');
				return;

			}
			if (fileexten.split("/")[1] == "pdf" || fileexten.split("/")[1] == "PDF") {

			}
			else {
				scope.preloader = false;
				swal("Only PDF Image Accepted");
				return;
			}
			var prop = { AadharCardNumber: scope.Eduaadhaar, Attachment: type.attachment, CertifcateID: categoryid, CertificateCategory: category };
			entityService.saveTutorial(prop, "/Home/POSTData")
				.then(function (data) {
					scope.imagedata = data.data;
					if (scope.imagedata.match("Failure")) {
						swal('info', scope.imagedata, 'error');
						scope.preloader = false;
						angular.element("input[type='file']").val('');
						return;
					}
					scope.userlink = data.data;

					swal('info',"file Uploaded Successfully",'success');

					//getCertificateDetails();
					console.log(data);
				});
		}

		//Load Department's
		function LoadDepartments() {
			var req = {
				TYPE: "1"
			};
			Internal_Services.DemoAPI("LoadDepartments", req, function (value) {
				if (value.data.Status == "Success") {
					scope.DepartmentDD = value.data.Details;
				}
				else {
					alert("Departmets Loading Failed");
				}
			});
		}

		//Load HOD's
		scope.LoadHODs = function () {
			var req = {
				TYPE: "2",
				DEPARTMENT: scope.selDept
			};
			Internal_Services.DemoAPI("LoadDepartments", req, function (value) {
				if (value.data.Status == "Success") {
					scope.HODDD = value.data.Details;
				}
				else {
					alert("Departmets Loading Failed");
				}
			});
		}

		//Load Services
		scope.LoadServices = function () {
			var req = {
				TYPE: "7",
				DEPARTMENT: scope.selDept,
				HOD: scope.selHOD
			};
			Internal_Services.DemoAPI("LoadDepartments", req, function (value) {
				if (value.data.Status == "Success") {
					scope.ServicesDD = value.data.Details;
				}
				else {
					alert("Departmets Loading Failed");
				}
			});
		}

		//Load Services
		scope.LoadServicesurls = function () {
			var req = {
				TYPE: "10",
				DEPARTMENT: scope.selDept,
				HOD: scope.selHOD,
				DISTRICT: scope.selservice
			};
			Internal_Services.DemoAPI("LoadDepartments", req, function (value) {
				if (value.data.Status == "Success") {
					scope.URLLIST = value.data.Details;
				}
				else {
					alert("Departmets Loading Failed");
				}
			});
		}

	};

	angular.module("GSWS")
		.factory("entityService", ["akFileUploaderService", function (akFileUploaderService) {
			var saveTutorial = function (tutorial, url) {
				return akFileUploaderService.saveModel(tutorial, url);
				//return akFileUploaderService.saveModel(tutorial,"/GRAMAVAPP"+url);
				//return akFileUploaderService.saveModel(tutorial, "/Rakshana/POSTData");
			};
			return {
				saveTutorial: saveTutorial
			};
		}]);

	angular.module("akFileUploader", [])
		.factory("akFileUploaderService", ["$q", "$http",
			function ($q, $http) {
				var getModelAsFormData = function (data) {
					var dataAsFormData = new FormData();
					angular.forEach(data, function (value, key) {
						dataAsFormData.append(key, value);
					});
					return dataAsFormData;
				};

				var saveModel = function (data, url) {
					var deferred = $q.defer();
					$http({
						url: url,
						method: "POST",
						data: getModelAsFormData(data),
						transformRequest: angular.identity,
						headers: { 'Content-Type': undefined }
					}).then(function (result) {

						deferred.resolve(result);
					}, function (result, status) {
						deferred.reject(status);
					});
					return deferred.promise;
				};

				return {
					saveModel: saveModel
				}

			}]).directive("akFileModel", ["$parse",
				function ($parse) {
					return {
						restrict: "A",
						link: function (scope, element, attrs) {
							var model = $parse(attrs.akFileModel);
							var modelSetter = model.assign;
							element.bind("change", function () {
								scope.$apply(function () {
									modelSetter(scope, element[0].files[0]);
								});
							});
						}
					};
				}]);
})();