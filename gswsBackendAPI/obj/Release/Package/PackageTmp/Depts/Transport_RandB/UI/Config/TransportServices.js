(function () {

	var app = angular.module("GSWS");

	app.service("Transport_RandB_Services", ["network_service", "$state", Transport_Services]);

	function Transport_Services(ns, state) {

		var Transport_Services = this;
	var	baseurl = "/api/Transport/";


		Transport_Services.DemoAPI = function (Method, input, callback) {
			ns.post(baseurl + Method, input, function (data) {
				callback(data);

			}, function (error) {
				callback(error);
			});
		};
		Transport_Services.POSTENCRYPTAPI = function (methodname, input, token, callback) {
			ns.encrypt_post(baseurl + methodname, input, token, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};

	}

})();