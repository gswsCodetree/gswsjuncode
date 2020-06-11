(function () {

    var app = angular.module("GSWS", [
        'ui.router',
        'Network'
        //'Secure',
        //'input_masking',
        //'naif.base64'
    ]);

    app.config(["$stateProvider", "$urlRouterProvider", '$logProvider', '$locationProvider', App_config]);

    function App_config($stateProvider, $urlRouterProvider, $logProvider, $locationProvider) {
        var web_site = location.hostname;
        var pre_path = "";
        if (web_site !== "localhost") {
            pre_path = "/" + location.pathname.split("/")[1] + "/";
        }
        $urlRouterProvider.otherwise("/");
        $logProvider.debugEnabled(true);
        $stateProvider
            .state("loginPage", { url: "/", templateUrl: pre_path + "UI/login.html", controller: "LoginController" })
            .state("AgriCulture", { url: "/AgriCulture", templateUrl: pre_path + "UI/AgriCulture/Home.html", controller: "HomeController" })
            .state("AgriCultureMainPage", { url: "/AgriCultureMainPage", templateUrl: pre_path + "UI/AgriCulture/Main.html", controller: "MainController" });
          
        //$locationProvider.html5Mode({
        //    enabled: true,
        //    requireBase: false
        //});
    }

    app.directive('onlyDigits', function () {
        return {
            require: 'ngModel',
            restrict: 'A',
            link: function (scope, element, attr, ctrl) {
                function inputValue(val) {
                    if (val) {
                        var digits = val.replace(/[^0-9.-]/g, '');
                        if (digits !== val) {
                            ctrl.$setViewValue(digits);
                            ctrl.$render();
                        }
                        return digits;//ParseInt(digits,10);
                    }
                    return undefined;
                }
                ctrl.$parsers.push(inputValue);
            }
        };
    });

    app.directive('numbersOnly', function () {
        return {
            require: 'ngModel',
            restrict: 'A',
            link: function (scope, element, attr, ctrl) {
                function inputValue(val) {
                    if (val) {
                        var digits = val.replace(/[^0-9]/g, '');
                        if (digits !== val) {
                            ctrl.$setViewValue(digits);
                            ctrl.$render();
                        }
                        return digits;//ParseInt(digits,10);
                    }
                    return undefined;
                }
                ctrl.$parsers.push(inputValue);
            }
        };
    });


})();