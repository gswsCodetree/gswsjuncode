(function () {

	var app = angular.module("GSWS");

	app.service("Socialwelfare_Services", ["network_service", "$state", API_Services]);

	function API_Services(ns, state) {

		var API_Services = this;
		baseurl = "/api/SocialWelfare/";



		API_Services.MrgCertDwnldAPI = function (input, callback) {

			ns.post(baseurl + "MrgCert_Download", input, function (data) {
				callback(data);

			}, function (error) {
				callback(error);
			});
		};

		

	}

})();