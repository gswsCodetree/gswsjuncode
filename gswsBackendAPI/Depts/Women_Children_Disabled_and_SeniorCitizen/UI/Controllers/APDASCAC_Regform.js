(function () {
	var app = angular.module("GSWS");

	app.controller("APDASCACREGFORM_Controller", ["$scope", "$state", "$log", "Women_Services", Women_CTRL]);

	function Women_CTRL(scope, state, log, Women_Services) {
        sessionStorage.setItem("WCDWToken", "");
		scope.Preloader = false;
		var token = sessionStorage.getItem("Token"); var user = sessionStorage.getItem("user");
		if (!(token) || !(user)) {
			alert('Session expired..!');
			state.go("Login");
		}

		scope.getdetails = function () {
			if (!(scope.txtname)) {
				alert('Please enter User Name');
				return;
			}
			if (!(scope.txtemaid)) {
				alert('Please enter Email ID');
				return;
			}
			if (!(scope.txtpswd)) {
				alert('Please enter Password');
				return;
			}
			if (!(scope.txtcnpswd)) {
				alert('Please enter Confirm Password');
				return;
			}
			if (scope.txtpswd != scope.txtcnpswd) {
				alert('Mismatch! Both entered passwords not matched.');
				return;
			}
			if (!(scope.txtmob)) {
				alert('Please enter Mobile Number');
				return;
			}
			if (scope.txtmob.length != "10") {
				alert('Please enter 10 digit Mobile Number');
				return;
			}
			if (!(scope.txtaadhaar)) {
				alert('Please enter Aadhaar Number');
				return;
			}
			if (scope.txtaadhaar.length != "12") {
				alert('Please enter 12 digit Aadhaar Number');
				return;
			}

			var val = scope.txtaadhaar.length;
			var card = scope.txtaadhaar;
			if (val < 12) {
				alert('Please Enter 12 Digit Aadhaar Number.');
				return;
			}
			if (card == "111111111111" || card == "222222222222" || card == "333333333333" || card == "444444444444" || card == "555555555555" || card == "666666666666"
				|| card == "777777777777" || card == "888888888888" || card == "999999999999" || card == "000000000000") {
				alert("Please Enter Valid Aadhaar Number");
				return;
			}

			var status = validateVerhoeff(card);

			if (status) {

			}
			else {
				alert('Enter Valid Aadhaar Number');
				return;
			}
            var req = {
                name: scope.txtname, email: scope.txtemaid, password: scope.txtpswd, confirmpassword: scope.txtcnpswd, mobile: scope.txtmob, aadhaar: scope.txtaadhaar, GSWS_ID: sessionStorage.getItem("TransID"), gsws_user_email: "", gsws_user_password: ""
            };
			if (!(token) || !(user)) {
				alert('Session expired..!');
				state.go("Login");
			}
			scope.Preloader = true;
			Women_Services.POSTENCRYPTAPI("Registration_form", req, token, function (value) {
                scope.Preloader = false;
                var res = value.data;
                if (res.Status == 100) {
                    swal('Success', "Registration Sucessfully", 'success');
                    sessionStorage.setItem("WCDWToken", res.Token);
                    state.go("ui.San_2wheeler");
                }
                else if (value.data.Status == "428") {
                    sessionStorage.clear();
                    swal("info", "Session Expired !!!", "info");
                    state.go("Login");
                    return;
                }
                else { swal("Info", value.data.Reason, "info"); }
			});
		};

		scope.Refresh = function () {
			location.reload(true);
		};
		function Reset() {
			scope.txtname = "";
			scope.txtemaid = "";
			scope.txtpswd = "";
			scope.txtcnpswd = "";
			scope.txtmob = "";
			scope.txtaadhaar = "";
		}

	}
})();
