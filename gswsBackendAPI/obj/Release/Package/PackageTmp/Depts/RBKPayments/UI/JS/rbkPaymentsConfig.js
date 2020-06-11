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
			.state("ui.rbkPayments", { url: "/rbkPayments", templateUrl: pre_path + "Depts/rbkPayments/UI/rbkPayments.html", controller: "rbkPaymentsController" })
			.state("uc.rbkPaymentStatus", { url: "/rbkPaymentStatus", templateUrl: pre_path + "Depts/rbkPayments/UI/rbkPaymentStatus.html", controller: "rbkPaymentStatusController" })
			.state("uc.rbkPaymentResp", { url: "/rbkPaymentResp", templateUrl: pre_path + "Depts/rbkPayments/UI/rbkPaymentResp.html", controller: "rbkPaymentRespController" })
			.state("uc.rbkChallanprint", { url: "/RBKChallanPrint", templateUrl: pre_path + "Depts/rbkPayments/UI/RBKChallanPrint.html", controller: "RBKChallanPrintController" });
			
	}

	app.service("rbkPaymentsServices", ["network_service", rbkPaymentsServices]);

	function rbkPaymentsServices(ns, state) {

		var Internal_Services = this;
		baseurl = "/api/rbkPayments/";

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