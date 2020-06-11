(function () {

	var app = angular.module("GSWS");

	app.service("SERP_Services", ["network_service", "$state", SERP_Services]);

	function SERP_Services(ns, state) {

		var SERP_Services = this;
		var baseurl = "/api/SERP/";


		SERP_Services.DemoAPI = function (Method, input, callback) {
			ns.post(baseurl + Method, input, function (data) {
				callback(data);

			}, function (error) {
				callback(error);
			});
		};

		SERP_Services.POSTENCRYPTAPI = function (methodname, input, token, callback) {
			ns.encrypt_post(baseurl + methodname, input, token, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};
	}

})();