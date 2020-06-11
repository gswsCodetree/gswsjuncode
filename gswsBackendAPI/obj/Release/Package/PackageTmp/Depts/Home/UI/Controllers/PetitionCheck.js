(function () {
	/* eslint-disable */
	var app = angular.module("GSWS");
	app.controller("Home_PetitionStatusCntrl", ["$scope", "Home_Services", '$sce', '$http', "$state", Cert_CTRL]);
	function Cert_CTRL(scope, home_services, sce, $http,state) {
		scope.Preloader = false;
		var token = sessionStorage.getItem("Token"); var user = sessionStorage.getItem("user");
		if (!(token) || !(user)) {
			alert('Session expired..!');
			state.go("Login");
		}

		scope.detailsshow = false;

		scope.getdetails = function () {

			if (!(scope.txtpetitionid)) {
				alert('Please enter Petition ID.');
				return;
			}
			if (!(scope.txtPetname)) {
				alert('Please enter Petitioner Name.');
				return;
			}

			var req = {
				petitionId: scope.txtpetitionid, petName: scope.txtPetname
			};
			scope.Preloader = true;
			home_services.POSTENCRYPTAPI("CheckPetitionStatus", req, token, function (value) {
				scope.Preloader = false;
				if (value.status == 200) {
					var res = value.data;

					if (res.status == "Success") {
						if (res.Data.length == 0) {
							alert('No Data Found');
							scope.RData = "";
							scope.detailsshow = false;
						}
						else {
							scope.detailsshow = true;
							scope.RData = res.Data[0];

						}
					}
					else {
						alert('No Data Found');
						scope.RData = "";
						scope.detailsshow = false;
					}
				} else {
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


