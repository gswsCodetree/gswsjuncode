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
			.state("ui.payment", { url: "/payment", templateUrl: pre_path + "Payments/UI/home.html", controller: "paymentController" });
	}


	app.service("Payment_Services", ["network_service", "$state", Payment_Services]);

	function Payment_Services(ns, state) {

		var Internal_Services = this;
		baseurl = "/api/payment/";

		Internal_Services.GetData = function (methodname, input, callback) {
			var url = baseurl + methodname;
			ns.get(url,  function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};

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