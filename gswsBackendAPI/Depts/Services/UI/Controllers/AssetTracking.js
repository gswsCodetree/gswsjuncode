(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("AssetTrackCntrl", ["$scope", "$state", "$log", "Ser_Services", AssetTrackCntrl]);

	function AssetTrackCntrl(scope, state, log, ser_services) {
	//	laodallsecratariats();
		laoddistricts();
		scope.username = sessionStorage.getItem("user");
		scope.data_level = 0;
		scope.dataarray = [];

		//Save Data
		scope.save = function () {
			if (!scope.seldistrict) {
				swal("", "Please Select District", "error");
			}
			else if (!scope.selmandal) {
				swal("", "Please Select Mandal", "error");
			}
			else if (!scope.selVillage) {
				swal("", "Please Select Secratriat", "error");
			}

			//CPU 1
			else if (!scope.cpuserialno) {
				swal("", "Please Enter CPU Serial Number of System 1", "error");
			}
			else if (!scope.cpucon1) {
				swal("", "Please Select CPU Connected or Not for System 1", "error");
			}
			else if (!scope.cpuwok1) {
				swal("", "Please Select CPU Working or Not for System 1", "error");
			}
			else if ((scope.cpucon1 == "1") && !scope.cpuconrem1) {
				swal("", "Please Enter CPU Connection Remarks of System 1", "error");
			}
			else if ((scope.cpuwok1 == "1") && !scope.cpuworkrem1) {
				swal("", "Please Enter CPU Working Condition Remarks of System 1", "error");
			}

			//Monitor 1
			else if (!scope.monitorserialno) {
				swal("", "Please Enter Monitor Serial Number of System 1", "error");
			}
			else if (!scope.monitorcon1) {
				swal("", "Please Select Monitor Connected or Not for System 1", "error");
			}
			else if (!scope.monitorwok1) {
				swal("", "Please Select Monitor Working or Not for System 1", "error");
			}
			else if ((scope.monitorcon1 == "1") && !scope.monitorconrem1) {
				swal("", "Please Enter Monitor Connection Remarks of System 1", "error");
			}
			else if ((scope.monitorwok1 == "1") && !scope.monitorworkrem1) {
				swal("", "Please Enter Monitor Working Condition Remarks of System 1", "error");
			}


			//Keyboard 1
			else if (!scope.Keyboardserialno) {
				swal("", "Please Enter Keyboard Serial Number of System 1", "error");
			}
			else if (!scope.Keyboardcon1) {
				swal("", "Please Select Keyboard Connected or Not for System 1", "error");
			}
			else if (!scope.Keyboardwok1) {
				swal("", "Please Select Keyboard Working or Not for System 1", "error");
			}
			else if ((scope.Keyboardcon1 == "1") && !scope.Keyboardconrem1) {
				swal("", "Please Enter Keyboard Connection Remarks of System 1", "error");
			}
			else if ((scope.Keyboardwok1 == "1") && !scope.Keyboardworkrem1) {
				swal("", "Please Enter Monitor Working Condition Remarks of System 1", "error");
			}


			//Mouse 1
			else if (!scope.mouseserialno) {
				swal("", "Please Enter Mouse Serial Number of System 1", "error");
			}
			else if (!scope.mousecon1) {
				swal("", "Please Select Mouse Connected or Not for System 1", "error");
			}
			else if (!scope.mousewok1) {
				swal("", "Please Select Mouse Working or Not for System 1", "error");
			}
			else if ((scope.mousecon1 == "1") && !scope.mouseconrem1) {
				swal("", "Please Enter Mouse Connection Remarks of System 1", "error");
			}
			else if ((scope.mousewok1 == "1") && !scope.mouseworkrem1) {
				swal("", "Please Enter Mouse Working Condition Remarks of System 1", "error");
			}


			//Battery 1
			else if (!scope.Batteryserialno) {
				swal("", "Please Enter Battery Serial Number of System 1", "error");
			}
			else if (!scope.Batterycon1) {
				swal("", "Please Select Battery Connected or Not for System 1", "error");
			}
			else if (!scope.Batterywok1) {
				swal("", "Please Select Battery Working or Not for System 1", "error");
			}
			else if ((scope.Batterycon1 == "1") && !scope.Batteryconrem1) {
				swal("", "Please Enter Battery Connection Remarks of System 1", "error");
			}
			else if ((scope.Batterywok1 == "1") && !scope.Batteryworkrem1) {
				swal("", "Please Enter Battery Working Condition Remarks of System 1", "error");
			}



			//Invertor
			else if (!scope.Invertorserialno) {
				swal("", "Please Enter Invertor Serial Number", "error");
			}
			else if (!scope.Invertorcon1) {
				swal("", "Please Select Invertor Connected or Not", "error");
			}
			else if (!scope.Invertorwok1) {
				swal("", "Please Select Invertor Working or Not", "error");
			}
			else if ((scope.Invertorcon1 == "1") && !scope.Invertorconrem1) {
				swal("", "Please Enter Invertor Connection Remarks", "error");
			}
			else if ((scope.Invertorwok1 == "1") && !scope.Invertorworkrem1) {
				swal("", "Please Enter Invertor Working Condition Remarks", "error");
			}



			//Printer
			else if (!scope.Printerserialno) {
				swal("", "Please Enter Printer Serial Number", "error");
			}
			else if (!scope.Printercon1) {
				swal("", "Please Select Printer Connected or Not", "error");
			}
			else if (!scope.Printerwok1) {
				swal("", "Please Select Printer Working or Not", "error");
			}
			else if ((scope.Printercon1 == "1") && !scope.Printerconrem1) {
				swal("", "Please Enter Printer Connection Remarks", "error");
			}
			else if ((scope.Printerwok1 == "1") && !scope.Printerworkrem1) {
				swal("", "Please Enter Printer Working Condition Remarks", "error");
			}


			//Laminator
			else if (!scope.Laminatorserialno) {
				swal("", "Please Enter Laminator Serial Number", "error");
			}
			else if (!scope.Laminatorcon1) {
				swal("", "Please Select Laminator Connected or Not", "error");
			}
			else if (!scope.Laminatorwok1) {
				swal("", "Please Select Laminator Working or Not for", "error");
			}
			else if ((scope.Laminatorcon1 == "1") && !scope.Laminatorconrem1) {
				swal("", "Please Enter Laminator Connection Remarks", "error");
			}
			else if ((scope.Laminatorwok1 == "1") && !scope.Laminatorworkrem1) {
				swal("", "Please Enter Laminator Working Condition Remarks", "error");
			}


			//Biometric
			else if (!scope.Biometricserialno) {
				swal("", "Please Enter Biometric Serial Number", "error");
			}
			else if (!scope.Biometriccon1) {
				swal("", "Please Select Biometric Connected or Not for System 1", "error");
			}
			else if (!scope.Biometricwok1) {
				swal("", "Please Select Biometric Working or Not for System 1", "error");
			}
			else if ((scope.Biometriccon1 == "1") && !scope.Biometricconrem1) {
				swal("", "Please Enter Biometric Connection Remarks", "error");
			}
			else if ((scope.Biometricwok1 == "1") && !scope.Biometricworkrem1) {
				swal("", "Please Enter Biometric Working Condition Remarks", "error");
			}


			//MAC BATCH & MOD NO 1
			else if (!scope.macaddress1) {
				swal("", "Please Enter MAC Address of System 1", "error");
			}
			else if (!scope.modelno1) {
				swal("", "Please Enter Model No. of System 1", "error");
			}
			else if (!scope.batchno1) {
				swal("", "Please Enter Batch No. of System 1", "error");
			}




			//CPU 2
			else if (!scope.cpuserialno2) {
				swal("", "Please Enter CPU Serial Number of System 2", "error");
			}
			else if (!scope.cpucon2) {
				swal("", "Please Select CPU Connected or Not for System 2", "error");
			}
			else if (!scope.cpuwok2) {
				swal("", "Please Select CPU Working or Not for System 2", "error");
			}
			else if ((scope.cpucon2 == "1") && !scope.cpuconrem2) {
				swal("", "Please Enter CPU Connection Remarks of System 2", "error");
			}
			else if ((scope.cpuwok2 == "1") && !scope.cpuworkrem2) {
				swal("", "Please Enter CPU Working Condition Remarks of System 2", "error");
			}

			//Monitor 2
			else if (!scope.monitorserialno2) {
				swal("", "Please Enter Monitor Serial Number of System 2", "error");
			}
			else if (!scope.monitorcon2) {
				swal("", "Please Select Monitor Connected or Not for System 2", "error");
			}
			else if (!scope.monitorwok2) {
				swal("", "Please Select Monitor Working or Not for System 2", "error");
			}
			else if ((scope.monitorcon2 == "1") && !scope.monitorconrem2) {
				swal("", "Please Enter Monitor Connection Remarks of System 2", "error");
			}
			else if ((scope.monitorwok2 == "1") && !scope.monitorworkrem2) {
				swal("", "Please Enter Monitor Working Condition Remarks of System 2", "error");
			}


			//Keyboard 2
			else if (!scope.Keyboardserialno2) {
				swal("", "Please Enter Keyboard Serial Number of System 2", "error");
			}
			else if (!scope.Keyboardcon2) {
				swal("", "Please Select Keyboard Connected or Not for System 2", "error");
			}
			else if (!scope.Keyboardwok2) {
				swal("", "Please Select Keyboard Working or Not for System 2", "error");
			}
			else if ((scope.Keyboardcon2 == "1") && !scope.Keyboardconrem2) {
				swal("", "Please Enter Keyboard Connection Remarks of System 2", "error");
			}
			else if ((scope.Keyboardwok2 == "1") && !scope.Keyboardworkrem2) {
				swal("", "Please Enter Keyboard Working Condition Remarks of System 2", "error");
			}


			//Mouse 2
			else if (!scope.mouseserialno2) {
				swal("", "Please Enter Mouse Serial Number of System 2", "error");
			}
			else if (!scope.mousecon2) {
				swal("", "Please Select Mouse Connected or Not for System 2", "error");
			}
			else if (!scope.mousewok2) {
				swal("", "Please Select Mouse Working or Not for System 2", "error");
			}
			else if ((scope.mousecon2 == "1") && !scope.mouseconrem2) {
				swal("", "Please Enter Mouse Connection Remarks of System 2", "error");
			}
			else if ((scope.mousewok2 == "1") && !scope.mouseworkrem2) {
				swal("", "Please Enter Mouse Working Condition Remarks of System 2", "error");
			}

			//Battery 2
			else if (!scope.Batteryserialno2) {
				swal("", "Please Enter Battery Serial Number of System 2", "error");
			}
			else if (!scope.Batterycon2) {
				swal("", "Please Select Battery Connected or Not for System 2", "error");
			}
			else if (!scope.Batterywok2) {
				swal("", "Please Select Battery Working or Not for System 2", "error");
			}
			else if ((scope.Batterycon2 == "1") && !scope.Batteryconrem2) {
				swal("", "Please Enter Battery Connection Remarks of System 2", "error");
			}
			else if ((scope.Batterywok2 == "1") && !scope.Batteryworkrem2) {
				swal("", "Please Enter Battery Working Condition Remarks of System 2", "error");
			}


			//MAC BATCH & MOD NO 2
			else if (!scope.macaddress2) {
				swal("", "Please Enter MAC Address of System 2", "error");
			}
			else if (!scope.modelno2) {
				swal("", "Please Enter Model No. of System 2", "error");
			}
			else if (!scope.batchno2) {
				swal("", "Please Enter Batch No. of System 2", "error");
			}

			else {
				scope.dataarray.push({
					USERNAME: scope.username,
					DISTRICT: scope.seldistrict,
					MANDAL: scope.selmandal,
					SECRATARIAT: scope.selVillage,


					CPUSERIALNO: scope.cpuserialno,
					CPUCONN: scope.cpucon1,
					CPUWORKING: scope.cpuwok1,
					CPUCONREMARKS: scope.cpuconrem1,
					CPUWORREMARKS: scope.cpuworkrem1,


					MONITORSERIALNO: scope.monitorserialno,
					MONITORCONN: scope.monitorcon1,
					MONITORWORKING: scope.monitorwok1,
					MONITORCONREMARKS: scope.monitorconrem1,
					MONITORWORREMARKS: scope.monitorworkrem1,


					KEYBOARDSERIALNO: scope.Keyboardserialno,
					KEYBOARDCONN: scope.Keyboardcon1,
					KEYBOARDWORKING: scope.Keyboardwok1,
					KEYBOARDCONREMARKS: scope.Keyboardconrem1,
					KEYBOARDWORREMARKS: scope.Keyboardworkrem1,


					MOUSESERIALNO: scope.mouseserialno,
					MOUSECONN: scope.mousecon1,
					MOUSEWORKING: scope.mousewok1,
					MOUSECONREMARKS: scope.mouseconrem1,
					MOUSEWORREMARKS: scope.mouseworkrem1,


					INVERTORSERIALNO: scope.Invertorserialno,
					INVERTORCONN: scope.Invertorcon1,
					INVERTORWORKING: scope.Invertorwok1,
					INVERTORCONREMARKS: scope.Invertorconrem1,
					INVERTORWORREMARKS: scope.Invertorworkrem1,


					BATTERIESSERIALNO: scope.Batteryserialno,
					BATTERIESCONN: scope.Batterycon1,
					BATTERIESWORKING: scope.Batterywok1,
					BATTERIESCONREMARKS: scope.Batteryconrem1,
					BATTERIESWORREMARKS: scope.Batteryworkrem1,


					PRINTERSERIALNO: scope.Printerserialno,
					PRINTERCONN: scope.Printercon1,
					PRINTERWORKING: scope.Printerwok1,
					PRINTERCONREMARKS: scope.Printerconrem1,
					PRINTERWORREMARKS: scope.Printerworkrem1,


					LAMINATORSERIALNO: scope.Laminatorserialno,
					LAMINATORCONN: scope.Laminatorcon1,
					LAMINATORWORKING: scope.Laminatorwok1,
					LAMINATORCONREMARKS: scope.Laminatorconrem1,
					LAMINATORWORREMARKS: scope.Laminatorworkrem1,


					BIOMETRICSERIALNO: scope.Biometricserialno,
					BIOMETRICCONN: scope.Biometriccon1,
					BIOMETRICWORKING: scope.Biometricwok1,
					BIOMETRICCONREMARKS: scope.Biometricconrem1,
					BIOMETRICWORREMARKS: scope.Biometricworkrem1,


					MACADDRESS: scope.macaddress1,
					MODELNO: scope.modelno1,
					BATCHNO: scope.batchno1,
					SYSNO: "1"

				});
				scope.dataarray.push({

					USERNAME: scope.username,
					DISTRICT: scope.seldistrict,
					MANDAL: scope.selmandal,
					SECRATARIAT: scope.selVillage,


					CPUSERIALNO: scope.cpuserialno2,
					CPUCONN: scope.cpucon2,
					CPUWORKING: scope.cpuwok2,
					CPUCONREMARKS: scope.cpuconrem2,
					CPUWORREMARKS: scope.cpuworkrem2,


					MONITORSERIALNO: scope.monitorserialno2,
					MONITORCONN: scope.monitorcon2,
					MONITORWORKING: scope.monitorwok2,
					MONITORCONREMARKS: scope.monitorconrem2,
					MONITORWORREMARKS: scope.monitorworkrem2,


					KEYBOARDSERIALNO: scope.Keyboardserialno2,
					KEYBOARDCONN: scope.Keyboardcon2,
					KEYBOARDWORKING: scope.Keyboardwok2,
					KEYBOARDCONREMARKS: scope.Keyboardconrem2,
					KEYBOARDWORREMARKS: scope.Keyboardworkrem2,


					MOUSESERIALNO: scope.mouseserialno2,
					MOUSECONN: scope.mousecon2,
					MOUSEWORKING: scope.mousewok2,
					MOUSECONREMARKS: scope.mouseconrem2,
					MOUSEWORREMARKS: scope.mouseworkrem2,


					INVERTORSERIALNO: "",
					INVERTORCONN: "1",
					INVERTORWORKING: "1",
					INVERTORCONREMARKS: "",
					INVERTORWORREMARKS: "",


					BATTERIESSERIALNO: scope.Batteryserialno2,
					BATTERIESCONN: scope.Batterycon2,
					BATTERIESWORKING: scope.Batterywok2,
					BATTERIESCONREMARKS: scope.Batteryconrem2,
					BATTERIESWORREMARKS: scope.Batteryworkrem2,


					PRINTERSERIALNO: "",
					PRINTERCONN: "1",
					PRINTERWORKING: "1",
					PRINTERCONREMARKS: "",
					PRINTERWORREMARKS: "",


					LAMINATORSERIALNO: "",
					LAMINATORCONN: "1",
					LAMINATORWORKING: "1",
					LAMINATORCONREMARKS: "",
					LAMINATORWORREMARKS: "",


					BIOMETRICSERIALNO: "",
					BIOMETRICCONN: "1",
					BIOMETRICWORKING: "1",
					BIOMETRICCONREMARKS: "",
					BIOMETRICWORREMARKS: "",


					MACADDRESS: scope.macaddress2,
					MODELNO: scope.modelno2,
					BATCHNO: scope.batchno2,
					SYSNO: "2"

				});
				var api = "SaveSystemData";
				var input = {
					TYPE: "1",
					DATAARRAY: scope.dataarray
				};
				ser_services.POSTENCRYPTAPI(api, input, sessionStorage.getItem("Token"), function (data) {
					var res = data.data;
					if (res.Status == "Success") {
						swal({
							title: "Good job!",
							text: "Data Saved Successfully",
							icon: "success"
						})

							.then((value) => {
								if (value) {
									window.location.reload();
								}
							});
					}
					else {
						swal("", res.Reason, "error");
					}
				});
			}
		}

		//Load Districts
		function laoddistricts() {
			var api = "LoadDistricts";
			var input = {
				TYPE: "1"
			};
			ser_services.POSTENCRYPTAPI(api, input, sessionStorage.getItem("Token"), function (data) {
				var res = data.data;
				if (res.Status == "Success") {
					scope.DistricsDD = res.Details;
				}
				else {
					swal("", "Districts Loading Failed", "error");
				}
			});
		}

		// Load Madals's
		scope.LoadMadals = function () {
			scope.data_level = 0;
			var api = "LoadDistricts";
			var input = {
				TYPE: "2",
				DISTRICT: scope.seldistrict
			};
			ser_services.POSTENCRYPTAPI(api, input, sessionStorage.getItem("Token"), function (data) {
				var res = data.data;
				if (res.Status == "Success") {
					scope.MandalsDD = res.Details;
				}
				else {
					swal("", "Mandals Loading Failed", "error");
				}
			});
		}

		// Load Panchayats's
		scope.LoadVillages = function () {
			scope.data_level = 0;
			var api = "LoadDistricts";
			var input = {
				TYPE: "3",
				DISTRICT: scope.seldistrict,
				MANDAL: scope.selmandal
			};
			ser_services.POSTENCRYPTAPI(api, input, sessionStorage.getItem("Token"), function (data) {
				var res = data.data;
				if (res.Status == "Success") {
					scope.VillagesDD = res.Details;
					filldefaultvals();
				}
				else {
					swal("", "Villages Loading Failed", "error");
				}
			});
		}

		// Load seccdata
		scope.LoadSeccdata = function () {
			scope.data_level = 0;
			var api = "LoadSeccDetails";
			var input = {
				TYPE: "1",
				DISTRICT: scope.seldistrict,
				MANDAL: scope.selmandal,
				SECRATARIAT: scope.selVillage
			};
			ser_services.POSTENCRYPTAPI(api, input, sessionStorage.getItem("Token"), function (data) {
				var res = data.data;
				if (res.Status == "Success") {
					scope.seccDetails = res.Details;
					filldefaultvals();
					scope.data_level = 2;

				}
				else {
					scope.data_level = 1;

				}
			});
		};


		scope.btn_logout = function () {
			sessionStorage.clear();
			window.open('login.html', '_Self');
		};


		//Fill Default Values
		function filldefaultvals() {
		}

		scope.nexprev = function (e) {
			scope.data_level = e;
		}


		//Cpu Connection 1 Change
		scope.cpucon1change = function () {
			if (scope.cpucon1 == "0") {
				scope.divcpuconrem1 = false;
			}
			else if (scope.cpucon1 == "1") {
				scope.divcpuconrem1 = true;
			}
		}

		//Working 1 Change
		scope.cpuwor1change = function () {
			if (scope.cpuwok1 == "0") {
				scope.divcpuworkrem1 = false;
			}
			else if (scope.cpuwok1 == "1") {
				scope.divcpuworkrem1 = true;
			}
		}



		//Monitor Connection 1 Change
		scope.monitorcon1change = function () {
			if (scope.monitorcon1 == "0") {
				scope.divmonitorconrem1 = false;
			}
			else if (scope.monitorcon1 == "1") {
				scope.divmonitorconrem1 = true;
			}
		}

		//Monitor Working 1 Change
		scope.monitorwor1change = function () {
			if (scope.monitorwok1 == "0") {
				scope.divmmonitorworkrem1 = false;
			}
			else if (scope.monitorwok1 == "1") {
				scope.divmmonitorworkrem1 = true;
			}
		}



		//Keyboard Connection 1 Change
		scope.Keyboardcon1change = function () {
			if (scope.Keyboardcon1 == "0") {
				scope.divKeyboardconrem1 = false;
			}
			else if (scope.Keyboardcon1 == "1") {
				scope.divKeyboardconrem1 = true;
			}
		}

		//Keyboard Working 1 Change
		scope.Keyboardwor1change = function () {
			if (scope.Keyboardwok1 == "0") {
				scope.divKeyboardworkrem1 = false;
			}
			else if (scope.Keyboardwok1 == "1") {
				scope.divKeyboardworkrem1 = true;
			}
		}



		//mouse Connection 1 Change
		scope.mousecon1change = function () {
			if (scope.mousecon1 == "0") {
				scope.divmouseconrem1 = false;
			}
			else if (scope.mousecon1 == "1") {
				scope.divmouseconrem1 = true;
			}
		}

		//mouse Working 1 Change
		scope.mousewor1change = function () {
			if (scope.mousewok1 == "0") {
				scope.divmouseworkrem1 = false;
			}
			else if (scope.mousewok1 == "1") {
				scope.divmouseworkrem1 = true;
			}
		}



		//Invertor Connection 1 Change
		scope.Invertorcon1change = function () {
			if (scope.Invertorcon1 == "0") {
				scope.divInvertorconrem1 = false;
			}
			else if (scope.Invertorcon1 == "1") {
				scope.divInvertorconrem1 = true;
			}
		}

		//Invertor Working 1 Change
		scope.Invertorwor1change = function () {
			if (scope.Invertorwok1 == "0") {
				scope.divInvertorworkrem1 = false;
			}
			else if (scope.Invertorwok1 == "1") {
				scope.divInvertorworkrem1 = true;
			}
		}



		//Battery Connection 1 Change
		scope.Batterycon1change = function () {
			if (scope.Batterycon1 == "0") {
				scope.divBatteryconrem1 = false;
			}
			else if (scope.Batterycon1 == "1") {
				scope.divBatteryconrem1 = true;
			}
		}

		//Battery Working 1 Change
		scope.Batterywor1change = function () {
			if (scope.Batterywok1 == "0") {
				scope.divBatteryworkrem1 = false;
			}
			else if (scope.Batterywok1 == "1") {
				scope.divBatteryworkrem1 = true;
			}
		}



		//Printer Connection 1 Change
		scope.Printercon1change = function () {
			if (scope.Printercon1 == "0") {
				scope.divPrinterconrem1 = false;
			}
			else if (scope.Printercon1 == "1") {
				scope.divPrinterconrem1 = true;
			}
		}

		//Printer Working 1 Change
		scope.Printerwor1change = function () {
			if (scope.Printerwok1 == "0") {
				scope.divPrinterworkrem1 = false;
			}
			else if (scope.Printerwok1 == "1") {
				scope.divPrinterworkrem1 = true;
			}
		}



		//Laminator Connection 1 Change
		scope.Laminatorcon1change = function () {
			if (scope.Laminatorcon1 == "0") {
				scope.divLaminatorconrem1 = false;
			}
			else if (scope.Laminatorcon1 == "1") {
				scope.divLaminatorconrem1 = true;
			}
		}

		//Laminator Working 1 Change
		scope.Laminatorwor1change = function () {
			if (scope.Laminatorwok1 == "0") {
				scope.divLaminatorworkrem1 = false;
			}
			else if (scope.Laminatorwok1 == "1") {
				scope.divLaminatorworkrem1 = true;
			}
		}


		//Biometric Connection 1 Change
		scope.Biometriccon1change = function () {
			if (scope.Biometriccon1 == "0") {
				scope.divBiometricconrem1 = false;
			}
			else if (scope.Biometriccon1 == "1") {
				scope.divBiometricconrem1 = true;
			}
		}

		//Biometric Working 1 Change
		scope.Biometricwor1change = function () {
			if (scope.Biometricwok1 == "0") {
				scope.divBiometricworkrem1 = false;
			}
			else if (scope.Biometricwok1 == "1") {
				scope.divBiometricworkrem1 = true;
			}
		}


		//Cpu Connection 2 Change
		scope.cpucon2change = function () {
			if (scope.cpucon2 == "0") {
				scope.divcpuconrem2 = false;
			}
			else if (scope.cpucon2 == "1") {
				scope.divcpuconrem2 = true;
			}
		}

		//Cpu Working 2 Change
		scope.cpuwor2change = function () {
			if (scope.cpuwok2 == "0") {
				scope.divcpuworkrem2 = false;
			}
			else if (scope.cpuwok2 == "1") {
				scope.divcpuworkrem2 = true;
			}
		}



		//Monitor Connection 2 Change
		scope.monitorcon2change = function () {
			if (scope.monitorcon2 == "0") {
				scope.divmonitorconrem2 = false;
			}
			else if (scope.monitorcon2 == "1") {
				scope.divmonitorconrem2 = true;
			}
		}

		//Monitor Working 2 Change
		scope.monitorwor2change = function () {
			if (scope.monitorwok2 == "0") {
				scope.divmmonitorworkrem2 = false;
			}
			else if (scope.monitorwok2 == "1") {
				scope.divmmonitorworkrem2 = true;
			}
		}



		//Keyboard Connection 2 Change
		scope.Keyboardcon2change = function () {
			if (scope.Keyboardcon2 == "0") {
				scope.divKeyboardconrem2 = false;
			}
			else if (scope.Keyboardcon2 == "1") {
				scope.divKeyboardconrem2 = true;
			}
		}

		//Keyboard Working 2 Change
		scope.Keyboardwor2change = function () {
			if (scope.Keyboardwok2 == "0") {
				scope.divKeyboardworkrem2 = false;
			}
			else if (scope.Keyboardwok2 == "1") {
				scope.divKeyboardworkrem2 = true;
			}
		}



		//mouse Connection 2 Change
		scope.mousecon2change = function () {
			if (scope.mousecon2 == "0") {
				scope.divmouseconrem2 = false;
			}
			else if (scope.mousecon2 == "1") {
				scope.divmouseconrem2 = true;
			}
		}

		//mouse Working 2 Change
		scope.mousewor2change = function () {
			if (scope.mousewok2 == "0") {
				scope.divmouseworkrem2 = false;
			}
			else if (scope.mousewok2 == "1") {
				scope.divmouseworkrem2 = true;
			}
		}




		//Invertor Connection 2 Change
		scope.Invertorcon2change = function () {
			if (scope.Invertorcon2 == "0") {
				scope.divInvertorconrem2 = false;
			}
			else if (scope.Invertorcon2 == "1") {
				scope.divInvertorconrem2 = true;
			}
		}

		//Invertor Working 2 Change
		scope.Invertorwor2change = function () {
			if (scope.Invertorwok2 == "0") {
				scope.divInvertorworkrem2 = false;
			}
			else if (scope.Invertorwok2 == "1") {
				scope.divInvertorworkrem2 = true;
			}
		}




		//Battery Connection 2 Change
		scope.Batterycon2change = function () {
			if (scope.Batterycon2 == "0") {
				scope.divBatteryconrem2 = false;
			}
			else if (scope.Batterycon2 == "1") {
				scope.divBatteryconrem2 = true;
			}
		}

		//Battery Working 2 Change
		scope.Batterywor2change = function () {
			if (scope.Batterywok2 == "0") {
				scope.divBatteryworkrem2 = false;
			}
			else if (scope.Batterywok2 == "1") {
				scope.divBatteryworkrem2 = true;
			}
		}



		//Printer Connection 2 Change
		scope.Printercon2change = function () {
			if (scope.Printercon2 == "0") {
				scope.divPrinterconrem2 = false;
			}
			else if (scope.Printercon2 == "1") {
				scope.divPrinterconrem2 = true;
			}
		}

		//Printer Working 2 Change
		scope.Printerwor2change = function () {
			if (scope.Printerwok2 == "0") {
				scope.divPrinterworkrem2 = false;
			}
			else if (scope.Printerwok2 == "1") {
				scope.divPrinterworkrem2 = true;
			}
		}


		////Auto Complete Secratariat Name
		//scope.complete = function (string) {
		//	scope.hidethis = false;
		//	var output = [];
		//	angular.forEach(scope.countryList, function (country) {
		//		if (country.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
		//			output.push(country);
		//		}
		//	});
		//	scope.filterCountry = output;
		//}

		////After filling text box
		//scope.fillTextbox = function (string) {
		//	scope.country = string;
		//	scope.hidethis = true;
		//}  


		////Load all secratariat List
		//function laodallsecratariats() {
		//	var api = "LoadDistricts";
		//	var input = {
		//		TYPE: "4"
		//	};
		//	ser_services.POSTENCRYPTAPI(api, input, sessionStorage.getItem("Token"), function (data) {
		//		var res = data.data;
		//		if (res.Status == "Success") {
		//			scope.countryList = res.Details;
		//			console.log(scope.countryList);
		//		}
		//		else {
		//			swal("", "Districts Loading Failed", "error");
		//		}
		//	});
		//}
	}
})();