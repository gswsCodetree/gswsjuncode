(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("FeedBackController", ["$scope", "$state", "$log", "Internal_Services", 'entityService', FeedBackController]);

    function FeedBackController(scope, state, log, Internal_Services, entityService) {
        scope.preloader = false;

        var username = sessionStorage.getItem("user");
        var distcode = sessionStorage.getItem("distcode");
        var mcode = sessionStorage.getItem("mcode");
        var seccode = sessionStorage.getItem("secccode");
        var token = sessionStorage.getItem("Token");
        var desinagtion = sessionStorage.getItem("desinagtion");

        if (!username) {
            state.go("Login");
        }
        scope.MAINDIV = "RAISEISSUES";
        scope.statustype = "3";
        LoadDepartments();
        GetSolvedIssuesData();
        //Submit
        scope.Savedata = function () {
            
            if (!scope.selDept) {
                swal('info', 'Please Select Department', 'info');
                return;
            }

            else if (!scope.selHOD) {
                swal('info', 'Please Select HOD', 'info');
                return;
            }

            else if (!scope.selurlid) {
                swal('info', 'Please Select Url Name', 'info');
                return;
            }

            else if (!scope.selsubject) {
                swal('info', 'Please Select Subject', 'info');
                return;
            }

            else if (!scope.selsubsubject) {
                swal('info', 'Please Select Sub-Subject', 'info');
                return;
            }

            else if (!scope.Remarks) {
                swal('info', 'Please Enter Remarks', 'info');
                return;
            }

            else if (!scope.img1 || !scope.img2 || !scope.img3) {
                swal('info', 'Please Upload File', 'info');
                return;
            }
            $("#prev-modal").modal('show');

        }

        scope.SubmitData = function () {


            //var req = { DEPTID: scope.selDept.PS_SD_CODE, HODID: scope.selHOD.PS_HOD_CODE, URL_ID: scope.selurlid.URL_ID, USERMAUALID: scope.userlink, USER: username, REMARKS: scope.Remarks, IMAGE1URL: scope.img1, IMAGE2URL: scope.img2, IMAGE3URL: scope.img3, DISTRICTID: distcode, MANDALID: mcode, SECRETRIATID: seccode }
            var req = { DEPTID: scope.selDept.PS_SD_CODE, HODID: scope.selHOD.PS_HOD_CODE, URL_ID: scope.selurlid.URL_ID, USERMAUALID: scope.userlink, USER: username, REMARKS: scope.Remarks, IMAGE1URL: scope.img1, IMAGE2URL: scope.img2, IMAGE3URL: scope.img3, DISTRICTID: distcode, MANDALID: mcode, SECRETRIATID: seccode, SUBJECT: scope.selsubject.SUBJECT_NAME, SUBSUBJECT: scope.selsubsubject.SUBSUBJECT_CODE, SOURCE: "1", DESIGNATION: desinagtion }
            Internal_Services.POSTENCRYPTAPI("SaveFeedBackReport", req, token, function (value) {
                if (value.data.Status == "100") {
                    swal('info', value.data.Reason, 'info');
                    window.location.reload();
                    return;
                }
                else {
                    swal('info', value.data.Reason, 'error');
                }
            });
        }

        scope.uploadfile = function (type, category, categoryid, imgname) {

            if (scope.selHOD == null || scope.selHOD == undefined || scope.selHOD == "") {
                swal('info', 'Please Select HOD', 'info');
                return;
            }
            if (scope.selurlid == null || scope.selurlid == undefined || scope.selurlid == "" || categoryid == undefined) {
                swal('info', 'Please Select Url Description', 'info');
                return;
            }
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
            var prop = { AadharCardNumber: imgname, Attachment: type.attachment, CertifcateID: categoryid, CertificateCategory: category };
            entityService.saveTutorial(prop, "/Home/POSTImageData")
                .then(function (data) {
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
                    LoadSubjects();
                }
                else {
                    alert("Departmets Loading Failed");
                }
            });
        }

        //Load HOD's
        scope.LoadHODs = function () {
            scope.HODDD = [];
            var req = {
                TYPE: "2",
                DEPARTMENT: scope.selDept.PS_SD_CODE
            };
            Internal_Services.DemoAPI("LoadDepartments", req, function (value) {
                if (value.data.Status == "Success") {
                    scope.HODDD = value.data.Details;
                }
                else {

                    alert("Hod Data Not Found.");
                }
            });
        }

        //Load Services
        scope.LoadServices = function () {
            var req = {
                TYPE: "11",
                DEPARTMENT: scope.selDept.PS_SD_CODE,
                HOD: scope.selHOD.PS_HOD_CODE
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

        //Load Services
        scope.LoadServicesurls = function () {
            var req = {
                TYPE: "10",
                DEPARTMENT: scope.selDept.PS_SD_CODE,
                HOD: scope.selHOD.PS_HOD_CODE,
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

        //Load Subject's
        function LoadSubjects() {
            var req = {
                TYPE: "14"
            };
            Internal_Services.DemoAPI("LoadDepartments", req, function (value) {
                if (value.data.Status == "Success") {
                    scope.SubjectsDD = value.data.Details;
                    //LoadDesignations();
                }
                else {

                }
            });
        }

        //Load Sub Subjects
        scope.LoadSubSubjects = function () {
            var req = {
                TYPE: "15",
                DEPARTMENT: scope.selsubject.SUBJECT_CODE
            };
            Internal_Services.DemoAPI("LoadDepartments", req, function (value) {
                if (value.data.Status == "Success") {
                    scope.SubSubjectsDD = value.data.Details;
                }
                else {

                }
            });
        }

        scope.IssuesCntclick = function (ctype) {
            sessionStorage.setItem("IssueType", ctype);
            var url = state.href("ui.SoftwareIssuesTracking")
            window.open(url, "_blank");

        }



        function GetSolvedIssuesData() {
            var req = {
                TYPE: "1", //Software
                UpdatedBy: username,
                SECRETARIAT: seccode,
                ACTIVE_STATUS: scope.statustype
            };
            Internal_Services.DemoAPI("GetHWSWResolvedIssues", req, function (value) {
                if (value.data.Status == "Success") {
                    var cdata = value.data.Details[0];
                    scope.ISSUES_TOTAL = cdata.TOTAL;
                    scope.ISSUES_OPEN = cdata.OPEN_COUNTS;
                    scope.ISSUES_CLOSED = cdata.CLOSED_COUNTS;

                    scope.ResolvedIssuesData = value.data.Details;
                    console.log(scope.ResolvedIssuesData);
                }
                else {
                    swal("", "No Data Found", "error");
                }
            });
        }

    };

    angular.module("GSWS")
        .factory("entityService", ["akFileUploaderService", function (akFileUploaderService) {
            var saveTutorial = function (tutorial, url) {
                //return akFileUploaderService.saveModel(tutorial,url);
                return akFileUploaderService.saveModel(tutorial, "/GSWS" + url);
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