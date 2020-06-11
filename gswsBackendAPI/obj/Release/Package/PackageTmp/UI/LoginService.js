(function () {

	var app = angular.module("GSWS");

	app.service("Login_Services", ["network_service", "$state", Login_Services]);

	function Login_Services(ns, state) {

		var Login_Services = this;
		var baseurl = "/api/GSWSWeb/";
        var transurl = "/api/transaction/";
		Login_Services.POSTAPI = function (methodname, input, callback) {

			ns.post(baseurl + methodname, input, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};
		Login_Services.POSTCUSTOMAPI = function (methodname, input, callback) {

			ns.custom_post(baseurl + methodname,"", input, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};
		Login_Services.DEMOAPI = function (methodname, input, callback) {

			ns.post("/api/Internal/" + methodname, input, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};

		Login_Services.DEMOMeesevaAPI = function (methodname, input, callback) {

			ns.post("/api/MeesevaWeb/" + methodname, input, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};

		Login_Services.POSTENCRYPTAPI = function (methodname, input, token, callback) {

			ns.encrypt_post(baseurl + methodname, input, token, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
        };

		Login_Services.POSTREVENUEENCRYPTAPI = function (methodname, input, token, callback) {

			ns.encrypt_post("/api/REVENUE/" + methodname, input, token, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};
		Login_Services.DEMREVOAPI = function (methodname, input, callback) {

			ns.post("/api/REVENUE/" + methodname, input, function (data) {
				callback(data);

			}, function (error) {
				callback(data);
			});
		};
        Login_Services.POSTTRANSCRYPTAPI = function (methodname, input, token, callback) {

            ns.encrypt_post(transurl + methodname, input, token, function (data) {
                callback(data);

            }, function (error) {
                callback(data);
            });
        };
	}

})();