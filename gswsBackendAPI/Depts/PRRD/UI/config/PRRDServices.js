(function () {

    var app = angular.module("GSWS");

    app.service("PRRD_Services", ["network_service", "$state", API_Services]);

    function API_Services(ns, state) {

        var API_Services = this;
		var baseurl = "/api/PRRD/";
		var weburl = "API/GSWSWEB/";

        API_Services.DemoAPI = function (methodname, input, callback) {

            ns.post(baseurl + methodname, input, function (data) {
                callback(data);

            }, function (error) {
                callback(data);
            });
        };
		API_Services.POSTENCRYPTAPI = function (methodname, input, token, callback) {

			ns.encrypt_post(baseurl + methodname, input, token, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};
		API_Services.WebPOSTENCRYPTAPI = function (methodname, input, token, callback) {

			ns.encrypt_post(weburl + methodname, input, token, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};
    }

})();