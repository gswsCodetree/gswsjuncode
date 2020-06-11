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
			.state("ui.pensionNewApplication", { url: "/pensionNewApplication", templateUrl: pre_path + "Depts/Pension/UI/pensionNewApplication.html", controller: "pensionDeptController" })
			.state("ui.pensionGrevinanceList", { url: "/pensionGrevinanceList", templateUrl: pre_path + "Depts/Pension/UI/pensionGrevinanceList.html", controller: "pensionGrevinanceListController" })
			.state("ui.pensionWEAVerification", { url: "/pensionWEAVerification", templateUrl: pre_path + "Depts/Pension/UI/pensionWEAVerification.html", controller: "pensionWEAVerificationController", params: { txnId: null, grevId: null } });
	}

	app.service("pensionDeptServices", ["network_service", pensionDeptServices]);

	function pensionDeptServices(ns, state) {

		var Internal_Services = this;
		baseurl = "/api/pensionDept/";

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