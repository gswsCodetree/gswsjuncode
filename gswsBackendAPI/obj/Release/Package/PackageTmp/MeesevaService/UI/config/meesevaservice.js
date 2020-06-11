(function () {

	var app = angular.module("GSWS");

    app.service("Meeseva_Services", ["network_service", "$state", Meeseva_Services]);

    function Meeseva_Services(ns, state) {

        var Meeseva_Services = this;
        baseurl = "/api/MeesevaWeb/";
		var masterurl ="/api/GSWSWEB/"
	

        Meeseva_Services.DemoAPI = function (Method, input, callback) {
			ns.post(baseurl + Method, input, function (data) {
				callback(data);

			}, function (error) {
				callback(error);
			});
		};

        Meeseva_Services.POSTENCRYPTAPI = function (methodname, input, token, callback) {

			ns.encrypt_post(masterurl + methodname, input, token, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};
	}

})();