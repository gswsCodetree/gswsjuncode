(function () {

	var app = angular.module("GSWS");

	app.service("Ser_Services", ["network_service", "$state", API_Services]);

	function API_Services(ns, state) {

		var API_Services = this;
	  var 	baseurl = "/api/Services/";
		var disurl = "/api/Internal/";
		var masterurl = "/api/GSWSWEB/";
		API_Services.DemoAPI = function (methodname, input, callback) {

			ns.post(baseurl + methodname, input, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
				});
		};

		API_Services.DemoInternalAPI = function (methodname, input, callback) {

			ns.post(disurl + methodname, input, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
        };

        API_Services.POSTENCRYPTAPI = function (methodname, input, token, callback) {
            ns.encrypt_post(baseurl + methodname, input, token, function (data) {
                callback(data);

            }, function (error) {
                callback(data);
            });
		};
		API_Services.POSTENCRYPTGSWSAPI = function (methodname, input, token, callback) {
			ns.encrypt_post(masterurl + methodname, input, token, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};
		API_Services.POSTINTERNALENCRYPTAPI = function (methodname, input, token, callback) {
			ns.encrypt_post(disurl + methodname, input, token, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};
		//Integrated and Income Certificates Download
		API_Services.CertificatesDwnlodAPI = function (input, callback) {

			ns.post(baseurl + "CertificatesDownload", input, function (data) {
				callback(data);

			}, function (error) {
				callback(error);
			});
		};

	}

})();