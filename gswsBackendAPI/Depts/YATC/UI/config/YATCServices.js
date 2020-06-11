(function () {

    var app = angular.module("GSWS");

    app.service("YATC_Services", ["network_service", "$state", YATC_Services]);

    function YATC_Services(ns, state) {

        var YATC_Services = this;
        baseurl = "/api/YATC/";

        YATC_Services.DemoAPI = function (methodname, input, callback) {

            ns.post(baseurl + methodname, input, function (data) {
                callback(data);

            }, function (error) {
                callback(data);
            });
        };

        YATC_Services.POSTENCRYPTAPI = function (methodname, input, token, callback) {
            ns.encrypt_post(baseurl + methodname, input, token, function (data) {
                callback(data);

            }, function (error) {
                callback(data);
            });
        };

    }

})();