(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("SadaremCertificate", ["$scope", "$state", "$log", "Health_Services","$sce", SadaremCertificateCall]);

    function SadaremCertificateCall(scope, state, log, health_services, sce) {

        scope.preloader = false;
		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}
        scope.GetStatus = function () {
            if (scope.sadaremid == "" || scope.sadaremid == null || scope.sadaremid == undefined || scope.sadaremid == "") {
                alert("Please Enter Sadarem Id.");
                return;
            }

            else {
                scope.preloader = true;
                scope.sadaremdetails = true;
                var jsonodata = { personcode: scope.sadaremid };

				health_services.POSTENCRYPTAPI("GetSadaremCertificate", jsonodata, token,function (value) {
                    scope.preloader = false;
                    var response = value.data;

                    if (response.Status == "Success") {
                        if (response.data.list.DEVICEDATA.Sadaremcode != "" && response.data.list.DEVICEDATA.Sadaremcode != "0") {
                            scope.Sadaremcode = response.data.list.DEVICEDATA.Sadaremcode;
                            scope.PERSONNAME = response.data.list.DEVICEDATA.PERSONNAME;
                            scope.gender = response.data.list.DEVICEDATA.gender;
                            scope.date_of_birth = response.data.list.DEVICEDATA.date_of_birth;
                            scope.PensionID = response.data.list.DEVICEDATA.PensionID;
                            scope.rationcard_number = response.data.list.DEVICEDATA.rationcard_number;
                            scope.AadharNumber = response.data.list.DEVICEDATA.AadharNumber;
                            scope.Address = response.data.list.DEVICEDATA.Address;
                            scope.TYPEOFDISABILITY = response.data.list.DEVICEDATA.TYPEOFDISABILITY;

                            scope.percentage = response.data.list.DEVICEDATA.percentage;
                            scope.DateOfIssue = response.data.list.DEVICEDATA.DateOfIssue;
                            scope.reasonforpersonstatus = response.data.list.DEVICEDATA.reasonforpersonstatus;
                            scope.ARstatus = response.data.list.DEVICEDATA.ARstatus;

                            //scope.CertHashMessage = response.data.list.DEVICEDATA.CertHashMessage;


                            var b = s_sd(response.data.list.DEVICEDATA.CertHashMessage, 'application/pdf');
                            var l = document.createElement('a');
                            scope.CertHashMessage = window.URL.createObjectURL(b);

                            var b = s_sd(response.data.list.DEVICEDATA.IDCardHashMessage, 'application/pdf');
                            var l = document.createElement('a');
                            scope.IDCardHashMessage = window.URL.createObjectURL(b);

                        }
                        else { scope.sadaremdetails = false; alert(response.data.list.DEVICEDATA.reasonforpersonstatus) }
					}
					else if (value.data.Status == "428") {
						sessionStorage.clear();
						swal('info', response.Reason, 'info');
						state.go("Login");
						return;
					}
                    else { scope.sadaremdetails = false; alert(response.Reason) }

                    
                });

            }
        }
    };


    function s_sd(content, contentType) {
        contentType = contentType || '';
        var sliceSize = 512;
        var byteCharacters = window.atob(content);
        var byteArrays = [
        ];
        for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
            var slice = byteCharacters.slice(offset, offset + sliceSize);
            var byteNumbers = new Array(slice.length);
            for (var i = 0; i < slice.length; i++) {
                byteNumbers[i] = slice.charCodeAt(i);
            }
            var byteArray = new Uint8Array(byteNumbers);
            byteArrays.push(byteArray);
        }
        var blob = new Blob(byteArrays, {
            type: contentType
        });
        return blob;
    }

    app.config(['$compileProvider', function ($compileProvider) {
        $compileProvider.aHrefSanitizationWhitelist(/^\s*(|blob|):/);
    }]);
   
})();