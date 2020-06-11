(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("AffidavitController", ["$scope", "$state", "$log", "Login_Services", 'entityService', Affidavit_CTRL]);

	function Affidavit_CTRL(scope, state, log, Internal_Services, entityService) {
		scope.preloader = false;
		
		scope.uploadfile = function (type) {

			if (type == null) {
				swal('info', 'please upload file', 'info');
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
			var prop = { AadharCardNumber: "222", Attachment: type.attachment, CertifcateID: sessionStorage.getItem("secccode"), CertificateCategory: "Affidvit" };
			entityService.saveTutorial(prop, "/Home/POSTAffidvitData")
				.then(function (data) {
					scope.imagedata = data.data;
					if (scope.imagedata.match("Failure")) {
						swal('info', scope.imagedata, 'error');
						scope.preloader = false;
						angular.element("input[type='file']").val('');
						return;
					}
					if (scope.imagedata.match(".pdf") || scope.imagedata.match(".PDF")) {
						scope.userlink = data.data;
						uploadFileData(scope.userlink);
						//swal('info', "file Uploaded Successfully", 'success');
					}
					else {

						swal('info', 'Pdf File is not updated properly.please try again', 'error');
						scope.preloader = false;
						angular.element("input[type='file']").val('');
						return;
					}

					//getCertificateDetails();
					console.log(data);
				});
		}

		function uploadFileData(pdffile) {
			var token = sessionStorage.getItem("Token");
			var req = { FType: "1", DistCode: sessionStorage.getItem("distcode"), MCode: sessionStorage.getItem("mcode"), SecCode: sessionStorage.getItem("secccode"), Loginuser: sessionStorage.getItem("user"), FilePath: pdffile}
			Internal_Services.POSTENCRYPTAPI("AffidavitPost", req, token, function (value) {
				if (value.data.Status == "100") {
					swal('info', "file Uploaded Successfully", 'success');
					angular.element("input[type='file']").val('');
				}
				else {
					swal('info', value.data.Reason, 'info');
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