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
			.state("uc.seedsPayments", { url: "/seedsPayments", templateUrl: pre_path + "Depts/seedsPayments/UI/seedsPayments.html", controller: "seedsPaymentsController" })
			.state("uc.seedsPaymentStatus", { url: "/seedsPaymentStatus", templateUrl: pre_path + "Depts/seedsPayments/UI/seedsPaymentStatus.html", controller: "seedsPaymentStatusController" })
			.state("uc.PaymentChallan", { url: "/PaymentChallan", templateUrl: pre_path + "Depts/seedsPayments/UI/PaymentChallan.html", controller: "PaymentChallanController" })
			.state("uc.SeedsChakanGeneration", { url: "/SeedsChalanGeneration", templateUrl: pre_path + "Depts/seedsPayments/UI/SeedsChalanGeneration.html", controller: "SeedsChalanGenerationController" })
			.state("uc.seedPaymentResp", { url: "/seedPaymentResp", templateUrl: pre_path + "Depts/seedsPayments/UI/seedsPaymentResp.html", controller: "seedsPaymentRespController" })
			.state("uc.RBKChakanGeneration", { url: "/RBKChallanGeneration", templateUrl: pre_path + "Depts/seedsPayments/UI/RBKPaymentGeneration.html", controller: "RBKChalanGenerationController" })
			.state("uc.SeedsChallanPrint", { url: "/SeedsChallanPrint", templateUrl: pre_path + "Depts/seedsPayments/UI/SeedsChallanPrint.html", controller: "SeedsChallanPrintController" })
			.state("uc.RBKPaymentChallan", { url: "/RBKPaymentChallan", templateUrl: pre_path + "Depts/seedsPayments/UI/RBKPaymentChallan.html", controller: "RBKPaymentChallanController" });

	}

	app.service("seedsPaymentsServices", ["network_service", seedsPaymentsServices]);

	function seedsPaymentsServices(ns, state) {

		var Internal_Services = this;
		baseurl = "/api/seedsPayment/";

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