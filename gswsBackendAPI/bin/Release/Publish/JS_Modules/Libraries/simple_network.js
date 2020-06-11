/*
 Author  : Venkatesh Nelli
 Version : 1.0.0
 License : GNU General Public License v3.0
 */

(function () {
    'use strict';

    var app = angular.module('Network', []);
    app.service('network_service', ['$http', network]);

    function network(http) {

        
        var pre_path = "";
        //var web_site = location.hostname;
        /*
        if (web_site != "localhost") {
            pre_path = "/" + location.pathname.split("/")[1];
        }
        */


        var post = function (url_path, data_object, success_callback,failure_callback) {
            http({
                method: 'POST',
                url: pre_path + url_path,
                data: JSON.stringify(data_object)
            }).then(function (success) {
                success_callback(success);
            }, function (error) {
                failure_callback(error);
                console.log(error.data);
            });
        }

        var get = function (url_path, success_callback,failure_callback) {
            http({
                method: 'GET',
                url: pre_path + url_path
            }).then(function (success) {
                success_callback(success);
            }, function (error) {
                failure_callback(error);
                console.log(error.data);
            });
        }

        var token_post = function (url_path, data_object, success_callback, failure_callback) {
            http({
                method: 'POST',
                url: pre_path + url_path,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: data_object,
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).then(function (success) {
                success_callback(success);
            }, function (error) {
                failure_callback(error);
                console.log(error.data);
            });
        }

        var token_get = function (url_path, success_callback, failure_callback) {
            http({
                method: 'GET',
                url: pre_path + url_path,
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).then(function (success) {
                success_callback(success);
            }, function (error) {
                failure_callback(error);
                console.log(error.data);
            });
        }

        var access_post = function (url_path, data_object, token, success_callback, failure_callback) {
            http({
                method: 'POST',
                url: pre_path + url_path,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data_object),
                headers: {
                    'Authorization': 'Bearer ' + token,
                    'contentType': 'application/json'
                }
            }).then(function (success) {
                success_callback(success);
            }, function (error) {
                failure_callback(error);
                console.log(error.data);
            });
        }

        var access_get = function (url_path, token, success_callback, failure_callback) {
            http({
                method: 'GET',
                url: pre_path + url_path,
                headers: {
                    'Authorization': 'Bearer ' + token,
                    'contentType': 'application/json'
                }
            }).then(function (success) {
                success_callback(success);
            }, function (error) {
                failure_callback(error);
                console.log(error.data);
            });
        }

        var custom_post = function (url_path, data_object, headers, success_callback, failure_callback) {
            http({
                method: 'POST',
                url: pre_path + url_path,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data_object),
                headers: headers
            }).then(function (success) {
                success_callback(success);
            }, function (error) {
                failure_callback(error);
                console.log(error.data);
            });
        }

        var custom_get = function (url_path, headers, success_callback, failure_callback) {
            http({
                method: 'GET',
                url: pre_path + url_path,
                headers: headers
            }).then(function (success) {
                success_callback(success);
            }, function (error) {
                failure_callback(error);
                console.log(error.data);
            });
        }



        var encrypt_post = function (url_path, data_object,key, token, success_callback, failure_callback) {

            


            http({
                method: 'POST',
                url: pre_path + url_path,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data_object),
                headers: {
                    'Authorization': 'Bearer ' + token,
                    'contentType': 'application/json'
                }
            }).then(function (success) {
                success_callback(success);
            }, function (error) {
                failure_callback(error);
                console.log(error.data);
            });
        }

        var encrypt_get = function (url_path, data_object,key, token,success_callback, failure_callback) {

            http({
                method: 'GET',
                url: pre_path + url_path,
                headers: {
                    'Authorization': 'Bearer ' + token,
                    'contentType': 'application/json'
                }
            }).then(function (success) {
                success_callback(success);
            }, function (error) {
                failure_callback(error);
                console.log(error.data);
            });
        }


        network = {
            post: post,
            get: get,
            token_post: token_post,
            token_get: token_get,
            access_post: access_post,
            access_get: access_get,
            custom_post: custom_post,
            custom_get: custom_get
        };
        return network;
    }

}());

