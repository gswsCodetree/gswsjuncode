(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("ArogyaRakshaController", ["$scope", "$state", "$log", "Health_Services", Health_CTRL]);

    function Health_CTRL(scope, state, log, health_services) {
        scope.pagename = "Arogya Raksha";
        scope.preloader = false;
        scope.dttable = false;
		var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			sessionStorage.clear();
			state.go("Login");
			return;
		}
        scope.validateaadhaar = function () {
            status = true;
            //if ((scope.aadhaar + "").toString().length == "12") {
            //    var aadhaar = scope.aadhaar;
            //    status = validateVerhoeff(aadhaar);
            //    if (!status) {
            //        //jQuery("#refresh").trigger("click");
            //        return status;
            //    }
            //}
        }

        scope.GetStatus = function () {
            if (scope.aadhaar == "" || scope.aadhaar == null || scope.aadhaar == undefined) {
                swal('info', "Please Enter Aadhaar Number.", 'info');
                return;
            }
            if (scope.aadhaar.toString().length != "12") {
                swal('info', "Aadhaar Number should be 12 digits.", 'info');
                return;
            }
            else if (scope.aadhaar == "000000000000" || scope.aadhaar == "111111111111" || scope.aadhaar == "222222222222" || scope.aadhaar == "333333333333" || scope.aadhaar == "444444444444" || scope.aadhaar == "555555555555" || scope.aadhaar == "666666666666" || scope.aadhaar == "777777777777" || scope.aadhaar == "888888888888" || scope.aadhaar == "999999999999") {
                swal('info',"Please Enter a valid Aadhaar Number", 'info');
                return;
            }
            //else if (status != true) {
            //    alert("Please Enter a valid Aadhaar Number");
            //    return;
            //}
            else {
                scope.preloader = true;
                scope.dttable = false;
                var req = { ftype: 1, fdpart_id: null, fadhar_no: scope.aadhaar };
                health_services.POSTENCRYPTAPI("GetArogyaRakshaStatus", req, token, function (value) {
                    scope.StudentDetails = [];
					if (value.data.Status == 100) {
						scope.dttable = true;
                        scope.StudentDetails = value.data.Details;
                        //console.log(value.data.Details);
					}
					else if (value.data.Status == "428") {
						sessionStorage.clear();
						swal('info', value.data.Reason, 'info');
						state.go("Login");
						return;
					}
					else {
						scope.dttable = false;
						swal('info',value.data.Reason,'info');
					}

                    scope.preloader = false;
                   
                });

            }
        }
    };
})();