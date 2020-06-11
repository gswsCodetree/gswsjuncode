(function () {

	var app = angular.module("GSWS");

	app.service("REVENUE_Services", ["network_service", "$state", REVENUE_Services]);

	function REVENUE_Services(ns, state) {

		var REVENUE_Services = this;
		baseurl = "/api/REVENUE/";
		var masterurl ="/api/GSWSWEB/"
	

		REVENUE_Services.DemoAPI = function (Method, input, callback) {
			ns.post(baseurl + Method, input, function (data) {
				callback(data);

			}, function (error) {
				callback(error);
			});
		};

		REVENUE_Services.POSTENCRYPTAPI = function (methodname, input, token, callback) {

			ns.encrypt_post(masterurl + methodname, input, token, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
        };

        REVENUE_Services.Excise_complant = function (Method, input, callback) {
            ns.post(baseurl + Method, input, function (data) {
                callback(data);

            }, function (error) {
                callback(error);
            });
        };
	}

})();