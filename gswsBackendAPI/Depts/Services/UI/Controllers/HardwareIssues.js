(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("HardwareIssuesCntrl", ["$scope", "$state", "$log", "Ser_Services", 'entityService', HardwareIssuesCntrl]);

	function HardwareIssuesCntrl(scope, state, log, ser_services, entityService) {
		scope.preloader = true;
		laoddistricts();
		loadhwcomponent();
		scope.username = sessionStorage.getItem("user");

		scope.seldistrictname = sessionStorage.getItem("distname");
		scope.selmandalname = sessionStorage.getItem("mname");
		scope.selVillagename = sessionStorage.getItem("secname");

		scope.seldistrict = sessionStorage.getItem("distcode");
		scope.selmandal = sessionStorage.getItem("mcode");
		scope.selVillage = sessionStorage.getItem("secccode");

		scope.MAINDIV = 'RAISEISSUES';
		scope.ResolvedIssuesclick = function (e, k) {
			scope.ResolvedIssuesData = "";
			scope.MAINDIV = e;
			scope.statustype = k;
			scope.headshow = k;
			if (e == "SOLVEDISSUES") {
				GetSolvedIssuesData();
			}
		}

		function GetSolvedIssuesData() {
			var req = {
				TYPE: "2", //Software
				UpdatedBy: scope.username,
				SECRETARIAT: scope.selVillage,
				ACTIVE_STATUS: scope.statustype
			};
			ser_services.DemoInternalAPI("GetHWSWResolvedIssues", req, function (value) {
				if (value.data.Status == "Success") {
					scope.ResolvedIssuesData = value.data.Details;
					console.log(scope.ResolvedIssuesData);
				}
				else {
					swal("", "No Data Found", "error");
				}
			});
		}

		//Save Data
		scope.submit = function () {
			if (!scope.hwcomponent) {
				swal("", "Please Select Hardware Component", "error");
			}
			else if (!scope.hwissue) {
				swal("", "Please Select Reason", "error");
			}
			else if (!scope.hwremarks) {
				swal("", "Please Enter Remarks", "error");
			}
			else {
				scope.preloader = true;
				var api = "SaveHardwareIssue";
				var input = {
					TYPE: "3",
					USERNAME: scope.username,

					DISTRICT: scope.seldistrict,
					MANDAL: scope.selmandal,
					SECRATARIAT: scope.selVillage,

					HWCOMPONENT: scope.hwcomponent,
					HWISSUE: scope.hwissue,
					IMAGEURL: scope.img1,
					REMARKS: scope.hwremarks,
					SOURCE:"1"
					
				};
				ser_services.POSTENCRYPTAPI(api, input, sessionStorage.getItem("Token"), function (data) {
					scope.preloader = false;
					var res = data.data;
					if (res.Status == "Success") {

						swal({
							title: "Good job!",
							text: res.Reason,
							icon: "success"
						})

							.then((value) => {
								if (value) {
									window.location.reload();
								}
							});
					}
				});
			}
		}

		//Load Districts
		function laoddistricts() {
			var api = "LoadDistricts";
			var input = {
				TYPE: "1"
			};
			ser_services.POSTENCRYPTAPI(api, input, sessionStorage.getItem("Token"), function (data) {
				scope.preloader = false;
				var res = data.data;
				if (res.Status == "Success") {
					scope.DistricsDD = res.Details;
				}
				else {
					swal("", "Districts Loading Failed", "error");
				}
			});
		}

		// Load Madals's
		scope.LoadMadals = function () {
			scope.preloader = true;
			scope.data_level = 0;
			var api = "LoadDistricts";
			var input = {
				TYPE: "2",
				DISTRICT: scope.seldistrict
			};
			ser_services.POSTENCRYPTAPI(api, input, sessionStorage.getItem("Token"), function (data) {
				scope.preloader = false;
				var res = data.data;
				if (res.Status == "Success") {
					scope.MandalsDD = res.Details;
				}
				else {
					swal("", "Mandals Loading Failed", "error");
				}
			});
		}

		// Load Panchayats's
		scope.LoadVillages = function () {
			scope.preloader = true;
			scope.data_level = 0;
			var api = "LoadDistricts";
			var input = {
				TYPE: "3",
				DISTRICT: scope.seldistrict,
				MANDAL: scope.selmandal
			};
			ser_services.POSTENCRYPTAPI(api, input, sessionStorage.getItem("Token"), function (data) {
				scope.preloader = false;
				var res = data.data;
				if (res.Status == "Success") {
					scope.VillagesDD = res.Details;
				}
				else {
					swal("", "Villages Loading Failed", "error");
				}
			});
		}

		//Load Hardware Component
		function loadhwcomponent() {
			scope.preloader = true;
			var api = "Loadhwcomponent";
			var input = {
				TYPE: "1"
			};
			ser_services.POSTENCRYPTAPI(api, input, sessionStorage.getItem("Token"), function (data) {
				scope.preloader = false;
				var res = data.data;
				if (res.Status == "Success") {
					scope.HWComponentDD = res.Details;
				}
				else {
					swal("", "Hardware Components Loading Failed", "error");
				}
			});
		}

		//Change in hardware Component 
		scope.hwcomponentchange = function () {
			scope.preloader = true;
			scope.hwissue = "";
			scope.hwremarks = "";
			var api = "Loadhwcomponent";
			var input = {
				TYPE: "2",
				COMPONENTID: scope.hwcomponent
			};
			ser_services.POSTENCRYPTAPI(api, input, sessionStorage.getItem("Token"), function (data) {
				scope.preloader = false;
				var res = data.data;
				if (res.Status == "Success") {
					scope.HWIssueDD = res.Details;
				}
				else {
					swal("", "Hardware Issues Loading Failed", "error");
				}
			});
		}

		//Image Upload
		scope.uploadfile = function (type, category, categoryid, imgname) {

			if (!scope.seldistrict) {
				swal('info', 'Please Select District', 'info');
				return;
			}
			if (!type) {
				swal('info', 'Please Select File to Upload', 'info');
				return;
			}
			if (!scope.hwcomponent) {
				swal('info', 'Please Select H/W Component', 'info');
				return;
			}
			if (!scope.hwissue) {
				swal('info', 'Please Select H/W Issue', 'info');
				return;
			}
			categoryid = scope.hwcomponent.toString() + scope.hwissue.toString();
			var file = type.attachment;
			var fileexten = file.type;
			var FileSize = file.size;
			if (FileSize > 1000000) {
				swal('info', 'File size exceeded 1 MB', 'error');
				scope.preloader = false;
				angular.element("input[type='file']").val('');
				return;

			}
			if (fileexten.split("/")[1] == "jpg" || fileexten.split("/")[1] == "JPG" || fileexten.split("/")[1] == "JPEG" || fileexten.split("/")[1] == "jpeg") {

			}
			else {
				scope.preloader = false;
				swal("info", "Only JPG Image Accepted", "info");
				return;
			}
			scope.preloader = true;
			var prop = { AadharCardNumber: imgname, Attachment: type.attachment, CertifcateID: categoryid, CertificateCategory: category };
			entityService.saveTutorial(prop, "/Home/POSTImageData")
				.then(function (data) {
					scope.preloader = false;
					scope.imagedata = data.data;
					if (scope.imagedata.match("Failure")) {
						swal('info', scope.imagedata, 'error');
						scope.preloader = false;
						angular.element("input[type='file']").val('');
						return;
					}
					if (imgname == "Image1") {
						scope.img1 = data.data;
						swal('info', "file Uploaded Successfully", 'success');
					}
					else if (imgname == "Image2") {
						scope.img2 = data.data;
						swal('info', "file Uploaded Successfully", 'success');
					}
					else if (imgname == "Image3") {
						scope.img3 = data.data;
						swal('info', "file Uploaded Successfully", 'success');
					}
					console.log(data);
				});
		}
	}


	angular.module("GSWS")
		.factory("entityService", ["akFileUploaderService", function (akFileUploaderService) {
			var saveTutorial = function (tutorial, url) {
				return akFileUploaderService.saveModel(tutorial, "/GSWS"+url);
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