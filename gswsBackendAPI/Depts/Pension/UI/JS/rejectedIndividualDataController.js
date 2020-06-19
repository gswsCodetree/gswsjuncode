(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("rejectedIndividualDataController", ["$scope", "pensionDeptServices", "$stateParams", "$state", individualData_CTRL]);


    function individualData_CTRL(scope, ps, stateParams, $state) {

        scope.token = sessionStorage.getItem("Token");
        scope.personDetails = '';
        scope.txnId = stateParams.txnId; // getParameterByName('txnId');
        scope.grevId = stateParams.grevId; // getParameterByName('grevId');

        if (scope.txnId != "" && scope.txnId != null && scope.txnId != undefined || scope.grevId != "" && scope.grevId != null && scope.grevId != undefined) {
            console.log(scope.txnId, scope.grevId);
        }
        else {
            alert("Invalid Request !!!");
            window.open("https://gramawardsachivalayam.ap.gov.in/GSWS/Home/Main", "_Self");
            return;
        }


        scope.loadIndividualGrevences = function () {
            scope.loader = true;
            var requestData = {
                secretariatCode: sessionStorage.getItem("secccode"),
                transactionId: scope.txnId,
                grievanceId: scope.grevId
            };
            ps.encrypt_post("individualGrevDetails", requestData, scope.token, function (data) {
                var res = data.data;
                if (res.status) {
                    scope.individualDetails = res.result;
                  
                }
                else {
                    alert(res.result);
                    $state.go('uc.MpdoRejectedListForAppeal');
                }
                scope.loader = false;
            }, function (error) {
                scope.loader = false;
                console.log(error);
            });

        };

        scope.loadIndividualGrevences();

        scope.btnView = function (FileString, FileName) {
            scope.certificateType = FileName;
            scope.certificateString = FileString;
            if (scope.certificateString == undefined || scope.certificateString == '' || scope.certificateString == null) {
                alert('No File to Show');
                return;
            } else {
                $("#certificateModal").modal('show');
            }
        };

        var Proof1 = "";
        scope.onLoad_doc1 = function (e, reader, file, fileList, fileOjects, fileObj) {
            if (fileObj.filetype != 'image/jpeg' && fileObj.filetype != 'image/png') {
                alert("Please select Image only !!!");
                return;
            }
            if (fileObj.filesize > 153601) {
                alert("Image should be less than 150 KB !!!");
                return;
            }
            Proof1 = fileObj.base64;
        };

        var Proof2 = "";
        scope.onLoad_doc2 = function (e, reader, file, fileList, fileOjects, fileObj) {
            if (fileObj.filetype != 'image/jpeg' && fileObj.filetype != 'image/png') {
                alert("Please select Image only !!!");
                return;
            }
            if (fileObj.filesize > 153601) {
                alert("Image should be less than 150 KB !!!");
                return;
            }
            Proof2 = fileObj.base64;
        };

        var Proof3 = "";
        scope.onLoad_doc3 = function (e, reader, file, fileList, fileOjects, fileObj) {
            if (fileObj.filetype != 'image/jpeg' && fileObj.filetype != 'image/png') {
                alert("Please select Image only !!!");
                return;
            }
            if (fileObj.filesize > 153601) {
                alert("Image should be less than 150 KB !!!");
                return;
            }
            Proof3 = fileObj.base64;
        };

        scope.btnSubmit = function () {
            if (scope.remarks == undefined || scope.remarks == '' || scope.remarks == null) {
                alert('Please Enter MPDO Remarks');
                return;
            }
            if (Proof1 == undefined || Proof1 == '' || Proof1 == null) {
                alert('Please Select Certificate to upload');
                return;
            }
            
            var req = {
                transactionId: scope.txnId,
                grievanceId: scope.grevId,
                loginId: sessionStorage.getItem("user"),
                status: "",
                remarks: scope.remarks,
                doc1: Proof1,
                doc2: Proof2,
                doc3: Proof3
            };

            scope.loader = true;
            ps.encrypt_post("insertWEAEnteredIndividualDetails", req, scope.token, function (data) {
                var res = data.data;
                if (res.status) {
                    alert(res.result);
                    $state.go('uc.MpdoRejectedListForAppeal');
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

    }

    app.directive('numbersDotOnly', function () {
        return {
            require: '?ngModel',
            link: function (scope, element, attrs, ngModelCtrl) {
                if (!ngModelCtrl) {
                    return;
                }

                ngModelCtrl.$parsers.push(function (val) {
                    if (angular.isUndefined(val)) {
                        val = '';
                    }
                    var clean = val.replace(/[^0-9\.]/g, '');
                    var decimalCheck = clean.split('.');

                    if (!angular.isUndefined(decimalCheck[1])) {
                        decimalCheck[1] = decimalCheck[1].slice(0, 2);
                        clean = decimalCheck[0] + '.' + decimalCheck[1];
                    }

                    if (val !== clean) {
                        ngModelCtrl.$setViewValue(clean);
                        ngModelCtrl.$render();
                    }
                    return clean;
                });

                element.bind('keypress', function (event) {
                    if (event.keyCode === 32) {
                        event.preventDefault();
                    }
                });
            }
        };
    });

    app.directive('numbersOnly', function () {
        return {
            require: 'ngModel',
            restrict: 'A',
            link: function (scope, element, attr, ctrl) {
                function inputValue(val) {
                    if (val) {
                        var digits = val.replace(/[^0-9]/g, '');
                        if (digits !== val) {
                            ctrl.$setViewValue(digits);
                            ctrl.$render();
                        }
                        return digits;//ParseInt(digits,10);
                    }
                    return undefined;
                }
                ctrl.$parsers.push(inputValue);
            }
        };
    });


})();