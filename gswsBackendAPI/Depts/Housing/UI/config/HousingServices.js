(function () {

    var app = angular.module("GSWS");

    app.service("Housing_Services", ["network_service", "$state", Hou_Services]);

    function Hou_Services(ns, state) {

        var Hou_Services = this;
        baseurl = "/api/Housing/";

        Hou_Services.DemoAPI = function (methodname, input, callback) {

            ns.post(baseurl + methodname, input, function (data) {
                callback(data);

            }, function (error) {
                callback(data);
            });
        };

        Hou_Services.POSTENCRYPTAPI = function (methodname, input, token, callback) {
            ns.encrypt_post(baseurl + methodname, input, token, function (data) {
                callback(data);

            }, function (error) {
                callback(data);
            });
        };
    }

})();