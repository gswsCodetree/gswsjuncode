(function () {

	var app = angular.module("GSWS");
	app.config(["$stateProvider", App_config]);

	function App_config($stateProvider) {
		var web_site = location.hostname;
		var pre_path = "";
		if (web_site !== "localhost") {
			pre_path = "/" + location.pathname.split("/")[1] + "/";
		}
		$stateProvider
			.state("ui.unMappedPensionersMapping", { url: "/unMappedPensionersMapping", templateUrl: pre_path + "Depts/unMappedPensioners/UI/home.html", controller: "unMappedPensionersMappingController" });
	}

	app.service("unMappedPensionersMappingServices", ["network_service", unMappedPensionersMappingServices]);

	function unMappedPensionersMappingServices(ns, state) {

		var Internal_Services = this;
		var baseurl = "/api/unMappedPensioners/";
		
		Internal_Services.DemoAPI = function (methodname, input, callback) {

			ns.post(baseurl + methodname, input, function (data) {
				callback(data);

			}, function (error) {
					callback(error);
			});
		};

		Internal_Services.POSTENCRYPTAPI = function (methodname, input, token, callback) {

			ns.encrypt_post(baseurl + methodname, input, token, function (data) {
				callback(data);

			}, function (error) {
					callback(error);
			});
		};
	}


})();