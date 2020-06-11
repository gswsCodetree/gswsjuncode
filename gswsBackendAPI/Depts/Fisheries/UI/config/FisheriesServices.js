(function () {

    var app = angular.module("GSWS");

    app.service("Fisheries_Services", ["network_service", "$state", Api_Services]);

    function Api_Services(ns, state) {

        var Api_Services = this;
        baseurl = "/api/Fisheries/";

        Api_Services.DemoAPI = function (methodname, input, callback) {

            ns.post(baseurl + methodname, input, function (data) {
                callback(data);

            }, function (error) {
                callback(data);
            });
        };

        Api_Services.POSTENCRYPTAPI = function (methodname, input, token, callback) {
            ns.encrypt_post(baseurl + methodname, input, token, function (data) {
                callback(data);

            }, function (error) {
                callback(data);
            });
        };
    }

})();