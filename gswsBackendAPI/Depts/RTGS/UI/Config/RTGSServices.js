(function () {

	var app = angular.module("GSWS");

	app.service("RTGS_Services", ["network_service", "$state", RTGS_Services]);

	function RTGS_Services(ns, state) {

		var RTGS_Services = this;
		var baseurl = "/api/RTGS/";

		var baserevurl = "/api/REVENUE/";

		RTGS_Services.DemoAPI = function (Method, input, callback) {
			ns.post(baseurl + Method, input, function (data) {
				callback(data);

			}, function (error) {
				callback(error);
			});
		};
		RTGS_Services.POSTENCRYPTAPI = function (methodname, input, token, callback) {

			ns.encrypt_post(baseurl + methodname, input, token, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};
		RTGS_Services.POSTREVENUEENCRYPTAPI = function (methodname, input, token, callback) {

			ns.encrypt_post("/api/REVENUE/" + methodname, input, token, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};
	}

})();