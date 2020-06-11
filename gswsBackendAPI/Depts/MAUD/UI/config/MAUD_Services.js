(function () {

	var app = angular.module("GSWS");

	app.service("MAUD_Services", ["network_service", "$state", API_Services]);

	function API_Services(ns, state) {

		var API_Services = this;
		baseurl = "/api/MAUDServies/";

		baseurl2 = "/api/Internal/";

		API_Services.DemoAPI = function (methodname, input, callback) {

			ns.post(baseurl2 + methodname, input, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};

		API_Services.DemoAPI2 = function (methodname, input, callback) {

			ns.post("/api/GSWSWEB/" + methodname, input, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};
		var transurl = "/api/transaction/";
		API_Services.POSTTRANSCRYPTAPI = function (methodname, input, token, callback) {

			ns.encrypt_post(transurl + methodname, input, token, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};
		API_Services.Application_Status = function (input, token,callback) {

			ns.encrypt_post(baseurl + "CheckMaudAppStatus", input, token, function (data) {
				callback(data);

			}, function (error) {
				callback(error);
			});
		};

		API_Services.POSTENCRYPTAPI = function (methodname, input, token, callback) {

			ns.encrypt_post("/api/GSWSWEB/" + methodname, input, token, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};

	}

})();