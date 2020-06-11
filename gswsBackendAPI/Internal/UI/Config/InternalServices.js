(function () {

	var app = angular.module("GSWS");

	app.service("Internal_Services", ["network_service", "$state", Internal_Services]);

	function Internal_Services(ns, state) {

		var Internal_Services = this;
	  	baseurl = "/api/Internal/";

		Internal_Services.DemoAPI = function (methodname, input, callback) {

			ns.post(baseurl + methodname, input, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};

		Internal_Services.POSTENCRYPTAPI = function (methodname, input, token, callback) {

			ns.encrypt_post(baseurl + methodname, input, token, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};
	}

})();