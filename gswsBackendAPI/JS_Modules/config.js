(function () {

    var app = angular.module("GSWS", [
        'ui.router',
		'Network',
		'input_masking',
		'datatables',
		'akFileUploader',
		'checklist-model',
		'naif.base64'
        //'Secure',
        //
        //'naif.base64'
    ]);

    app.config(["$stateProvider", "$urlRouterProvider", '$logProvider', '$locationProvider', App_config]);

    function App_config($stateProvider, $urlRouterProvider, $logProvider, $locationProvider) {
        var web_site = location.hostname;
        var pre_path = "";
        if (web_site !== "localhost") {
            pre_path = "/" + location.pathname.split("/")[1] + "/";
        }
		$urlRouterProvider.otherwise("/Login");
        $logProvider.debugEnabled(true);
		$stateProvider
			.state("ue", { url: "", templateUrl: pre_path + "UI/Headerfoter.html", controller: "UIController" })
			.state("ui", { url: "", templateUrl: pre_path + "UI/CommonHeader.html", controller: "UIController" })
			.state("uc", { url: "", templateUrl: pre_path + "UI/PublicHeader.html", controller: "PublicHController" })
			.state("ut", { url: "", templateUrl: pre_path + "UI/Teluguheader.html", controller: "UITController" })
			.state("Login", { url: "/Login", templateUrl: pre_path + "UI/Login.html", controller: "LoginController" })
			.state("DistrictLogin", { url: "/DistrictLogin", templateUrl: pre_path + "UI/DistrictLogin.html", controller: "DistrictLoginController" })
			.state("404", { url: "/errorpage", templateUrl: pre_path + "UI/404.html", controller: "UIController" })
			.state("500", { url: "/errorpage2", templateUrl: pre_path + "UI/500.html", controller: "UIController" })
			.state("BDReg", { url: "/BDReg", templateUrl: pre_path + "UI/birth-registration.html", controller: "UIController" })
			.state("Print", { url: "/PrintReceipt", templateUrl: pre_path + "UI/meesava-print.html", controller: "PrintReceipt" })
			.state("TestMain", { url: "/TestMainPage", templateUrl: pre_path + "UI/TestMain.html", controller: "TestMainController" })
			.state("ue.Dashboard", { url: "/MainDashboard", templateUrl: pre_path + "UI/MainDashboard.html", controller: "DaashboardController" })
			.state("ui.GVDashboard", { url: "/GVDashboard", templateUrl: pre_path + "UI/dashboard.html", controller: "GVDaashboardController" })
			.state("ui.TransResponse", { url: "/TransResponse", templateUrl: pre_path + "UI/TransactionResponse.html", controller: "TransResponseController" })
			.state("ui.ReceivedData", { url: "/ReceivedData", templateUrl: pre_path + "UI/ReceivedData.html", controller: "ReceivedController" })
			.state("ui.ApplicationStatus", { url: "/ApplicationStatus", templateUrl: pre_path + "UI/ApplicationStatus.html", controller: "AppStatusController" })
			.state("ui.Upward-downward", { url: "/Upward-downward", templateUrl: pre_path + "UI/upward-downward.html", controller: "UIController" })
			.state("ui.ProfileUpdate", { url: "/ProfileUpdate", templateUrl: pre_path + "UI/ProfileUpdate.html", controller: "ProfileUpdateController" })
			.state("ui.InchargeChange", { url: "/InchargeChange", templateUrl: pre_path + "UI/InchargeChange.html", controller: "InchargeChange" })
			.state("ui.PhysicalForms", { url: "/PhysicalForms", templateUrl: pre_path + "UI/PhysicalForms.html", controller: "PhysicalForms" })

			.state("AudioForms", { url: "/AudioFileForm", templateUrl: pre_path + "UI/AudioPage.html", controller: "AudioForms" })

			.state("uc.MailMobile", { url: "/UpdateMailMobile", templateUrl: pre_path + "UI/MailMobileUpdate.html", controller: "MailMobileController" })
			.state("ForgotPassword", { url: "/ForgotPassword", templateUrl: pre_path + "UI/ForgotPassword.html", controller: "ForgotPasswordController" })
			.state("ui.ChangePassword", { url: "/ChangePassword", templateUrl: pre_path + "UI/ChangePassword.html", controller: "ChangePasswordController" })
			.state("ui.PublicLogin", { url: "/PublicLogin", templateUrl: pre_path + "UI/PublicLogin.html", controller: "PublicLoginController" })
			.state("ui.CumilativeDashboard", { url: "/CumilativeDashboard", templateUrl: pre_path + "UI/CumilativeDashboard.html", controller: "CumilativeDashboardController" })
			.state("ui.SPandanaGrievance", { url: "/SPandanaGrievance", templateUrl: pre_path + "UI/SPandanaGrievance.html", controller: "SpandanaGrievanceCTRL" })
			.state("ui.Affidvit", { url: "/Affidvit", templateUrl: pre_path + "UI/Affidavit.html", controller: "AffidavitController" })
			.state("ut.TDashboard", { url: "/TMainDashboard", templateUrl: pre_path + "UI/TeluguDashboard.html", controller: "DaashboardController" })
			.state("uc.SecretariatForm", { url: "/SecretariatForm", templateUrl: pre_path + "UI/SecretariatForm.html", controller: "SecretariatFormController" })
			.state("uc.CFMSPayment", { url: "/CFMSChallanGeneration", templateUrl: pre_path + "UI/CFMSChallanGeneration.html", controller: "CFMSChallanGenerateCTRL" })
			.state("ui.SecretariatDashBoard", { url: "/SecretariatDashBoard", templateUrl: pre_path + "UI/SecretariatDashBoard.html", controller: "SecretariatDashBoardController" });		
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

	app.directive('alphaBets', function () {
		return {
			require: 'ngModel',
			restrict: 'A',
			link: function (scope, element, attr, ctrl) {
				function inputValue(val) {
					if (val) {
						var digits = val.replace(/[^a-zA-Z ]/g, '');
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

	app.directive('alphaNumeric', function () {
		return {
			require: 'ngModel',
			restrict: 'A',
			link: function (scope, element, attr, ctrl) {
				function inputValue(val) {
					if (val) {
						var digits = val.replace(/[^a-zA-Z0-9&,/ -]/g, '');
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

	app.directive('alphaNumerics', function () {
		return {
			require: 'ngModel',
			restrict: 'A',
			link: function (scope, element, attr, ctrl) {
				function inputValue(val) {
					if (val) {
						var digits = val.replace(/[^a-zA-Z0-9/ -]/g, '');
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

	app.directive('alphaNumbers', function () {
		return {
			require: 'ngModel',
			restrict: 'A',
			link: function (scope, element, attr, ctrl) {
				function inputValue(val) {
					if (val) {
						var digits = val.replace(/[^a-zA-Z0-9]/g, '');
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



    app.filter('highlight', function ($sce) {
        return function (text, phrase) {
            if (phrase) text = text.replace(new RegExp('(' + phrase + ')', 'gi'),
                '<span class="highlighted">$1</span>');
            return $sce.trustAsHtml('<a class="sub-service" style="cursor:pointer">' + text + '</a>');
        };
	});

	app.run(function ($http) {
		//
		//sessionStorage.setItem('SEC_KEY', Date.now());

			$http.defaults.headers.common['X-XSRF-Token'] = angular.element('input[name="__RequestVerificationToken"]').attr('value');
			//$http.defaults.headers.common['SEC_KEY'] = sessionStorage.getItem('SEC_KEY');
		
	});


	$(document).keydown(function (event) {
		if (event.keyCode == 123) { // Prevent F12
			return false;
		} else if (event.ctrlKey && event.shiftKey && event.keyCode == 73) { // Prevent Ctrl+Shift+I        
			return false;
		}
		else if (event.ctrlKey && event.shiftKey && event.keyCode == 74) { // Prevent Ctrl+Shift+J        
			return false;
		}
	});
	$(document).on("contextmenu", function (e) {
		e.preventDefault();
	});

})();