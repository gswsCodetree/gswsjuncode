(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("AmmavodiController", ["$scope", "$state", "$log", "Education_Services", Education_CTRL]);

	function Education_CTRL(scope, state, log, edu_services) {
		scope.pagename = "Ammavodi";
		scope.preloader = false;
		scope.dttable = false;
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

		scope.validateaadhaar = function () {
			status = true;
			//if ((scope.student_aadhaar + "").toString().length == "12") {
			//    var aadhaar = scope.student_aadhaar;
			//    status = validateVerhoeff(aadhaar);
			//    if (!status) {
			//        //jQuery("#refresh").trigger("click");
			//        return status;
			//    }
			//}
		}

		scope.GetStatus = function () {
			if (scope.student_aadhaar == "" || scope.student_aadhaar == null || scope.student_aadhaar == undefined) {
				swal('info', "Please Enter Student Aadhaar Number.", 'info');
				return;
			}
			if (scope.student_aadhaar.toString().length != "12") {
				swal('info', "Student Aadhaar Number should be 12 digits.", 'info');
				return;
			}
			else if (scope.student_aadhaar == "000000000000" || scope.student_aadhaar == "111111111111" || scope.student_aadhaar == "222222222222" || scope.student_aadhaar == "333333333333" || scope.student_aadhaar == "444444444444" || scope.student_aadhaar == "555555555555" || scope.student_aadhaar == "666666666666" || scope.student_aadhaar == "777777777777" || scope.student_aadhaar == "888888888888" || scope.student_aadhaar == "999999999999") {
				swal('info', "Please Enter a valid Aadhaar Number", 'info');
				return;
			}
			//else if (status != true) {
			//    swal('info',"Please Enter a valid Aadhaar Number");
			//    return;
			//}
			else {
				scope.preloader = true;
				scope.dttable = false;
				var req = { ftype: 2, fdpart_id: null, fadhar_no: scope.student_aadhaar };
				edu_services.POSTENCRYPTAPI("GetAmmavodiAppStatus", req, token, function (value) {
					scope.StudentDetails = [];
					var res = value.data;
					if (res.errorMsg!="No Data Found") {
						scope.StudentDetails.push(value.data);
						console.log(value.data);
						scope.dttable = true;
                    }
                    else if (value.data.Status == "428") {
                        sessionStorage.clear();
                        swal("info", "Session Expired !!!", "info");
                        //state.go("Login");
                        return;
                    }
					else { swal('info', value.data.errorMsg, 'info'); }

					scope.preloader = false;

				});

			}
		}
	}
})();