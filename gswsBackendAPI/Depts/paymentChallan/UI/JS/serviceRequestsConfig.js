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
			.state("uc.serviceRequests", { url: "/serviceRequests", templateUrl: pre_path + "Depts/paymentChallan/UI/serviceRequests.html", controller: "serviceRequestsController" });
	}

	app.service("cfmsPaymentServices", ["network_service", cfmsPaymentServices]);

	function cfmsPaymentServices(ns, state) {

		var Internal_Services = this;
		baseurl = "/api/cfms/";

		Internal_Services.post = function (methodname, input, callback) {

			ns.post(baseurl + methodname, input, function (data) {
				callback(data);

			}, function (error) {
				callback(error);
			});
		};

		Internal_Services.encrypt_post = function (methodname, input, token, callback) {

			ns.encrypt_post(baseurl + methodname, input, token, function (data) {
				callback(data);

			}, function (error) {
				callback(error);
			});
		};
	}


})();