(function () {

    var app = angular.module("GSWS");

    app.service("Law_Services", ["network_service", "$state", Law_Services]);

    function Law_Services(ns, state) {

        var law_Services = this;
        baseurl = "/api/Law/";

        law_Services.DemoAPI = function (methodname, input, callback) {

            ns.post(baseurl + methodname, input, function (data) {
                callback(data);

            }, function (error) {
                callback(data);
            });
        };

        law_Services.POSTENCRYPTAPI = function (methodname, input, token, callback) {
            ns.encrypt_post(baseurl + methodname, input, token, function (data) {
                callback(data);

            }, function (error) {
                callback(data);
            });
        };
    }

})();