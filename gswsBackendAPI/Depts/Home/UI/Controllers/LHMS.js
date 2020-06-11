(function () {
	var app = angular.module("GSWS");
	app.controller("Home_LHMSCntrl", ["$scope", "Home_Services", '$sce', '$http', LHMS_CTRL]);
	function LHMS_CTRL(scope, home_services, sce, $http) {
		scope.preloader = false;
		scope.municipality = [];
		scope.slide = "1";

		var token = sessionStorage.getItem("Token"); var user = sessionStorage.getItem("user");
		if (!(token) || !(user)) {
			alert('Session expired..!');
			state.go("Login");
		}

		LoadDistrictData();

		//Registration 
		scope.submitregisterreq = function () {
			if (!scope.regname) {
				swal("", "Please Enter Your Name", "error");
			}
			else if (!scope.regmobile) {
				swal("", "Please Enter Your Mobile Number", "error");
			}
			else if (!scope.regdistrict) {
				swal("", "Please Select Your District", "error");
			}
			else if (!scope.regmunicipality) {
				swal("", "Please Select Your Municipality", "error");
			}
			else if (!scope.regdoorno) {
				swal("", "Please Enter Your Door Number", "error");
			}
			else if (!scope.regaddress) {
				swal("", "Please Enter Your Full Address", "error");
			}
			else if (!scope.reglatitude) {
				swal("", "Please Enter Your latitude", "error");
			}
			else if (!scope.reglongitude) {
				swal("", "Please Enter Your Longitude", "error");
			}
			else {
				scope.preloader = true;
				var req = {
					name: scope.regname,
					address: scope.regdoorno + "," + scope.regaddress,
					mobile_number: scope.regmobile,
					latitude: scope.reglatitude,
					longitude: scope.reglongitude,
					town_id: scope.regdistrict,
					municipal_id: scope.regmunicipality,
					token: "IU56jhgu567JGJH7",
					GSWS_ID: sessionStorage.getItem("TransID")
				};

				home_services.POSTENCRYPTAPI("SaveLHMSData", req, token, function (data) {
					var res = data.data;
					scope.preloader = false;
					if (res.success == true) {
						swal({
							title: "Good job!",
							text: res.message,
							icon: "success"
						})

							.then((value) => {
								if (value) {
									window.location.reload();
								}
							});
					}
					else {
						swal("", res.message, "error");
					}
				});
			}
		}

		//Watch Request 
		scope.submitwatchreq = function () {
			if (!scope.watchuserid) {
				swal("", "Please Enter Your User ID", "error");
			}
			else if (!scope.watchstartdate) {
				swal("", "Please Enter Start Date", "error");
			}
			else if (!scope.watchstarttime) {
				swal("", "Please Enter Start Time", "error");
			}
			else if (!scope.watchenddate) {
				swal("", "Please Enter End Date", "error");
			}
			else if (!scope.watchendtime) {
				swal("", "Please Enter End Time", "error");
			}
			else {
				scope.preloader = true;
				var req = {
					token: "IU56jhgu567JGJH7",
					owner_id: scope.watchuserid,
					start_date: scope.watchstartdate,
					start_time: scope.watchstarttime,
					end_date: scope.watchenddate,
					end_time: scope.watchendtime,
					GSWS_ID: sessionStorage.getItem("TransID")
				};

				home_services.POSTENCRYPTAPI("SaveLHMSWatchRequestData", req, token, function (data) {
					scope.preloader = false;
					var res = data.data;
					if (res.success == true) {
						swal({
							title: "Good job!",
							text: res.message,
							icon: "success"
						})

							.then((value) => {
								if (value) {
									window.location.reload();
								}
							});
					}
					else {
						swal("", res.message, "error");
					}
				});
			}
		}

		//Selected
		scope.selected = function (s) {
			scope.slide = s;
		}

		//Back
		scope.back = function (s) {
			scope.slide = s;
		}

		//Get Location
		scope.GetLocation = function () {
			//swal("","Unable to get your current location.Please enter Manually","error");
			if (navigator.geolocation) {
				navigator.geolocation.getCurrentPosition(function (position) {

					scope.reglatitude = position.coords.latitude;
					scope.reglongitude = position.coords.longitude;
				});

			}
		}

		//Load Districts Data
		function LoadDistrictData() {
			scope.preloader = true;
			var obj = {}
			home_services.POSTENCRYPTAPI("GetLHMSDistrictData", obj, token, function (data) {
				scope.preloader = false;
				var res = data.data;

				if (res.success == true) {
					scope.districts = res.district;
					for (var i = 0; i < scope.districts.length; i++) {
						for (var j = 0; j < scope.districts[i]["municipal"].length; j++) {
							var val = {
								id: scope.districts[i]["municipal"][j]["id"],
								name: scope.districts[i]["municipal"][j]["name"],
								status: scope.districts[i]["municipal"][j]["status"],
								pincode: scope.districts[i]["municipal"][j]["pincode"],
								discode: scope.districts[i]["id"]
							};
							scope.municipality.push(val);
						}

					}
					scope.preloader = false;
				}
				else {
					scope.preloader = false;
					swal("", "District Data Loading Failed", "error");
				}
			});
		}

	}
})();