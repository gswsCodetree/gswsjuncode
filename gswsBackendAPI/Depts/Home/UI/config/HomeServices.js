(function () {

	var app = angular.module("GSWS");

	app.service("Home_Services", ["network_service", "$state", API_Services]);

	function API_Services(ns, state) {

		var API_Services = this;
	    var	baseurl = "/api/HomeWeb/";
		var masterurl = "/API/GSWSWEB/";

		API_Services.DemoAPI = function (methodname, input, callback) {

			ns.encrypt_post(baseurl + methodname, input, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};
		API_Services.MasterDemoAPI = function (methodname, input, callback) {

			ns.encrypt_post(masterurl + methodname, input, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};

		API_Services.POSTENCRYPTAPI = function (methodname, input, token, callback) {

			ns.encrypt_post(baseurl + methodname, input, token, function (data) {
				callback(data);

			}, function (data) {
				callback(data);
			});
		};




	}

})();