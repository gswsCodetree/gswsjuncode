(function () {

	var app = angular.module("GSWS");

	app.service("Ration_Services", ["network_service", "$state", Ration_Services]);

	function Ration_Services(ns, state) {

		var Ration_Services = this;
		baseurl = "/api/RationVolunteer/";

		Ration_Services.DemoAPI = function (methodname, input, callback) {

			ns.post(baseurl + methodname, input, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};

		Ration_Services.POSTENCRYPTAPI = function (methodname, input, token, callback) {

			ns.encrypt_post(baseurl + methodname, input, token, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};
	}

})();