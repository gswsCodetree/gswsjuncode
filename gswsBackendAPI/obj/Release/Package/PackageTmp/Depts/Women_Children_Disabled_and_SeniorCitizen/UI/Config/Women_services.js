(function () {

	var app = angular.module("GSWS");

	app.service("Women_Services", ["network_service", "$state", API_Services]);

	function API_Services(ns, state) {

		var API_Services = this;
		baseurl = "/api/WomenServices/";

		API_Services.DemoAPI = function (Method, input, callback) {
			ns.post(baseurl + Method, input, function (data) {
				callback(data);

			}, function (error) {
				callback(error);
			});
		};

		API_Services.POSTENCRYPTAPI = function (methodname, input, token, callback) {

			ns.encrypt_post(baseurl + methodname, input, token, function (data) {
				callback(data);

			}, function (data) {
				callback(data);
			});
		};



		API_Services.MrgCertDwnldAPI = function (input, callback) {

			ns.post(baseurl + "MrgCert_Download", input, function (data) {
				callback(data);

			}, function (error) {
				callback(error);
			});
		};

		API_Services.StatusCheck = function (input, callback) {

			ns.post(baseurl + "StatusCheck", input, function (data) {
				callback(data);

			}, function (error) {
				callback(error);
			});
		};

		API_Services.Education_Scholarship_StatusCheck = function (input, callback) {

			ns.post(baseurl + "Education_ApplicationCheck", input, function (data) {
				callback(data);

			}, function (error) {
				callback(error);
			});
		};

		API_Services.LoadAcadamicyears = function (input, callback) {

			ns.post(baseurl + "GetAnnaualyears", input, function (data) {
				callback(data);

			}, function (error) {
				callback(error);
			});
		};

	}

})();