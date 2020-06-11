(function () {
	/* eslint-disable */
	var app = angular.module("GSWS");
	app.controller("Home_MissedKidnappedCntrl", ["$scope", "Home_Services", '$sce', '$http', "$state", Cert_CTRL]);
	function Cert_CTRL(scope, home_services, sce, $http,state) {
		scope.Preloader = false;
		var token = sessionStorage.getItem("Token"); var user = sessionStorage.getItem("user");
		if (!(token) || !(user)) {
			alert('Session expired..!');
			state.go("Login");
		}
		scope.detailsshow = false;

		scope.getdetails = function () {
			if (!(scope.ddlgender)) {
				swal('Error', 'Please select gender', 'error');
				return;
			}
			if (!(scope.fromdate)) {
				swal('Error', 'Please select from date', 'error');
				return;
			}
			if (!(scope.todate)) {
				swal('Error', 'Please select to date', 'error');
				return;
			}
			var fdate = new Date($('#txtfromdate').val());
			//var year = date.getFullYear();
			var day = fdate.getDate();
			var month = fdate.getMonth() + 1;
			var year = fdate.getFullYear();
			var fromdate = [day, month, year].join('/');

			var tdate = new Date($('#txttodate').val());
			//var year = date.getFullYear();
			var day1 = tdate.getDate();
			var month1 = tdate.getMonth() + 1;
			var year1 = tdate.getFullYear();
			var todate = [day1, month1, year1].join('/');

			var req = {
				age: scope.txtage, gender: scope.ddlgender, fromDt: fromdate, toDt:
					todate, name: scope.txtname, missingPlace: scope.txtPOM, heightFeet: scope.txtfeet, heightInch: scope.txtinch
			};
			scope.Preloader = true;
			home_services.POSTENCRYPTAPI("CheckMissedPersons", req, token, function (value) {
				scope.Preloader = false;
				if (value.status == 200) {
					var res = value.data[0];
                    if (value.data.status != "Failure") {
                        if (res.status == "Success") {
                            if (res.MissingPersonDetails.length == 0) {
                                alert('No Data Found');
                                scope.RData = "";
                                scope.detailsshow = false;
                            }
                            else {
                                scope.detailsshow = true;
                                scope.RData = res.MissingPersonDetails;

                            }
                        }
                        else {
                            alert('No Data Found');
                            scope.RData = "";
                            scope.detailsshow = false;
                        }
                    }
                    else {
                        alert(value.data.Reason);
                        scope.RData = "";
                        scope.detailsshow = false;
                    }
				}
				else {
					scope.Preloader = false;
					swal('Exception!', 'Something went wrong', 'error');
					return;
				}
			});
		};


		scope.Refresh = function () {
			location.reload(true);
		}
	}
})();


