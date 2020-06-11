(function () {

    var app = angular.module("GSWS");

    app.service("AgriCulture_Services", ["network_service", "$state", API_Services]);

    function API_Services( ns, state) {

        var API_Services = this;
        baseurl = "/api/Agriculture/";

        API_Services.DemoAPI = function (input, callback) {

            ns.post(baseurl + "DemoAPI", input, function (data) {
                callback(data);

            }, function (error) {
                    callback(data);
            });
        };

    }

})();