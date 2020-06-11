(function () {

    var app = angular.module("GSWS");

    app.service("AgriCulture_Services", ["network_service", "$state", API_Services]);

    function API_Services(ns, state) {

        var API_Services = this;
        var baseurl = "/api/Agriculture/";
        var masterurl = "/api/GSWSWEB/";
        API_Services.DemoAPI = function (methodname, input, callback) {

            ns.post(baseurl + methodname, input, function (data) {
                callback(data);

            }, function (error) {
                callback(error);
            });
        };

        API_Services.POSTENCRYPTAPIAGRI = function (methodname, input, token, callback) {

            ns.encrypt_post(baseurl + methodname, input, token, function (data) {
                callback(data);

            }, function (error) {
                callback(data);
            });
        };

        API_Services.POSTENCRYPTAPI = function (methodname, input, token, callback) {

            ns.encrypt_post(masterurl + methodname, input, token, function (data) {
                callback(data);

            }, function (error) {
                callback(data);
            });
        };

          }

})();