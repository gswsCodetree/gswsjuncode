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
			.state("knowYourVolunteer", { url: "/knowYourVolunteer", templateUrl: pre_path + "Depts/knowYourVolunteer/UI/home.html", controller: "KYVController" });
	}

	app.service("KYVServices", ["network_service", KYVServices]);

	function KYVServices(ns, state) {

		var Internal_Services = this;
		baseurl = "/api/KYV/";

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

	app.controller("KYVController", ["$scope", "KYVServices", KYV_CTRL]);


	function KYV_CTRL(scope, KYVServices) {


		scope.token = sessionStorage.getItem("Token");

		scope.loader = false;
		scope.personDetails = "";

		scope.volunteerDetails = function () {

			scope.personDetails = "";
			if (scope.uidNum == null || scope.uidNum == undefined || scope.uidNum == undefined) {
				alert("Please Enter aadhaar number");
				return;
			}

			if (scope.uidNum.length != 12) {
				alert("Please Enter valid aadhaar number");
				return;
			}

			if (!validateVerhoeff(scope.uidNum)) {
				alert("Please Enter valid aadhaar number");
				return;
			}


			scope.loader = true;
			var requestData = {
				uidNum: scope.uidNum
			};
			KYVServices.post("volunteerDetails", requestData, scope.token, function (data) {
				var res = data.data;
				if (res.success) {
					scope.personDetails = res.result[0];
				}
				else {
					alert(res.result);
				}
				scope.loader = false;
			}, function (error) {
				scope.loader = false;
				console.log(error);
			});
		};


	}

})();