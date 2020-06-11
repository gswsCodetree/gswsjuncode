(function () {
	var status = false;
	var app = angular.module("GSWS");

	app.controller("SecretariatFormController", ["$scope", "$state", "$log", "Login_Services", 'entityService', SecretariatFormController]);

	function SecretariatFormController(scope, state, log, Login_Services, entityService) {
		scope.preloader = false;
		//alert(1);

		scope.DistName = sessionStorage.getItem("distname");
		scope.RUFlag = sessionStorage.getItem("RUFlag");
		scope.MName = sessionStorage.getItem("mname");
		scope.SeccName = sessionStorage.getItem("secname");

		var token = sessionStorage.getItem("Token");
		var req = { FTYPE: "1", DISTCODE: sessionStorage.getItem("distcode"), MCODE: sessionStorage.getItem("mcode"), RUFLAG:sessionStorage.getItem("RUFlag"),GPCODE: sessionStorage.getItem("secccode") }
		Login_Services.POSTENCRYPTAPI("GetSecretariatVolunteerData", req, token, function (value) {
			if (value.data.Status == "100") {
				scope.preloader = false;
				scope.Secformlist = value.data.DataList;
				scope.Vollist = value.data.VolList;
				console.log(value.data);
				scope.tab = 3;
				if (scope.Vollist.length > 0)
					scope.tab = 4;
			}
			else if (value.data.Status == 428) {
				state.go(Login);
			}
			else {
				scope.tab = 1;
				scope.secdisable = false;
				scope.preloader = false;
				//swal('info', value.data.Reason, 'info');
			}
		});
		

		scope.setTab = function (newTab) {
			scope.tab = newTab;
		};

		scope.isSet = function (tabNum) {
			return scope.tab === tabNum;
		};

		scope.GetEdit = function () {
			scope.divedit = true;
			scope.UCrbnRecievSmartPhone=scope.Secformlist[0].IS_SEC_REC_SMART_PHONE+"";
			scope.UCSelSmartPhMake = scope.Secformlist[0].SMART_PHONE_MAKER;
			scope.UCOtherSmartphmake = scope.Secformlist[0].SMART_PHONE_MAKER;
			scope.UCIMEISIMOne = scope.Secformlist[0].IMEI_SIM_1;
			scope.UCIMEISIMTwo = scope.Secformlist[0].IMEI_SIM_2;
			scope.UCrbnRecievMobileNum=scope.Secformlist[0].IS_SEC_REC_OFFICIAL_MOB_NO+"";
			scope.UCSelMobileOperator = scope.Secformlist[0].SEC_MOBILE_OPERATOR;
			scope.UCSecMobile = scope.Secformlist[0].SEC_MOBILE_NO;
			scope.UCSIMSerialNo = scope.Secformlist[0].SEC_SIM_SERIAL_NO;
			scope.UCrbnFPScanner = scope.Secformlist[0].IS_SEC_REC_FPS+"";
			scope.UCSelFPS = scope.Secformlist[0].FPS_MAKER;
			scope.UCFPSModelNum = scope.Secformlist[0].FPS_MODEL_NO;
			scope.UCFPSSerialNum = scope.Secformlist[0].FPS_SERIAL_NO;
			scope.UCrbnPrinter = scope.Secformlist[0].IS_SEC_REC_PRINTER_SCANNER+"";
			scope.UCSelPrinterMake = scope.Secformlist[0].PRINTER_SCANNER_MAKER;
			scope.UCPrinterModelNum = scope.Secformlist[0].PRINTER_SCANNER_MODEL_NO;
			scope.UCPrinterSerialNum = scope.Secformlist[0].PRINTER_SCANNER_SERIAL_NO;
			scope.UCSelAvailbleDesktop = scope.Secformlist[0].DESKTOP_COMPUTER_AVAILABLE+"";
			scope.UCSelDesktoponeMake = scope.Secformlist[0].MONITOR_1_MAKER;
			scope.UCDesktopSerialNumone = scope.Secformlist[0].MONITOR_1_SERIAL_NO;
			scope.UCSelCPUoneMake = scope.Secformlist[0].CPU_1_MAKER;
			scope.UCCPUSerialNumone = scope.Secformlist[0].CPU_1_SERIAL_NO;
			scope.UCSelMouseoneMake = scope.Secformlist[0].MOUSE_1_MAKER;
			scope.UCMouseSerialNumone = scope.Secformlist[0].MOUSE_1_SERIAL_NO;
			scope.UCSelKeyboardoneMake = scope.Secformlist[0].KEYBOARD_1_MAKER;
			scope.UCKeyboardSerialNumone = scope.Secformlist[0].KEYBOARD_1_SERIAL_NO;

			scope.UCSelDesktoptwoMake = scope.Secformlist[0].MONITOR_2_MAKER;
			scope.UCDesktopSerialNumtwo = scope.Secformlist[0].MONITOR_2_SERIAL_NO;
			scope.UCSelCPUtwoMake = scope.Secformlist[0].CPU_2_MAKER;
			scope.UCCPUSerialNumtwo = scope.Secformlist[0].CPU_2_SERIAL_NO;
			scope.UCSelMousetwoMake = scope.Secformlist[0].MOUSE_2_MAKER;
			scope.UCMouseSerialNumtwo = scope.Secformlist[0].MOUSE_2_SERIAL_NO;
			scope.UCSelKeyboardtwoMake = scope.Secformlist[0].KEYBOARD_2_MAKER;
			scope.UCKeyboardSerialNumtwo = scope.Secformlist[0].KEYBOARD_2_SERIAL_NO;
			scope.UCSelAvailbleUPS = scope.Secformlist[0].NO_OF_UPS_AVAILABLE+"";
			scope.UCUPSMakeone = scope.Secformlist[0].UPS_1_MAKER;
			scope.UCUPSSerialnumone = scope.Secformlist[0].UPS_1_SERIAL_NO;
			scope.UCUPSMaketwo = scope.Secformlist[0].UPS_2_MAKER;
			scope.UCUPSSerialnumtwo = scope.Secformlist[0].UPS_2_SERIAL_NO;
			scope.UCrbnRecievMahilaSmartPhone = scope.Secformlist[0].IS_MP_REC_OFF_SMART_PHONE+"";
			scope.UCSelMahilaSmartPhMake = scope.Secformlist[0].MP_SMART_PHONE_MAKER;
			scope.UCOtherMahilaSmartphmake = scope.Secformlist[0].MP_SMART_PHONE_MAKER;
			scope.UCMahilaIMEISIMOne = scope.Secformlist[0].MP_IMEI_SIM_1;
			scope.UCMahilaIMEISIMTwo = scope.Secformlist[0].MP_IMEI_SIM_2;
			scope.UCrbnRecievMahilaMobileNum = scope.Secformlist[0].IS_MP_REC_OFFICIAL_MOB_NO+"";
				scope.UCSelMahilaMobileOperator = scope.Secformlist[0].MP_MOBILE_OPERATOR;
				scope.UCMahilaPoliceMobile = scope.Secformlist[0].MP_MOBILE_NO;
				scope.UCMahilaSIMSerialNo = scope.Secformlist[0].MP_SIM_SERIAL_NO;

				scope.UCVolSmartpohe = scope.Secformlist[0].NO_OF_SMART_PHONES_REC;
				scope.UCVolSIMCard = scope.Secformlist[0].NO_OF_SIM_CARDS_REC;

				scope.UCselinternetconnectvity = scope.Secformlist[0].IS_INTERNET_FAC_AVAILABLE;
				scope.UCmdminstall = scope.Secformlist[0].NO_OF_MOBILE_INSTALL_MDM;
				scope.UCnoofArogyasethuapp = scope.Secformlist[0].NO_OF_VOL_INST_AROGYASETHU;
				scope.UCHighsecuritystationery = scope.Secformlist[0].HIGH_SECUR_STATIONERY_REC;
				scope.UCHighstationerystockAvailable = scope.Secformlist[0].HIGH_SECUR_STAT_STOCK_AVAI;
				scope.UCseleligibility = scope.Secformlist[0].IS_ELIG_CRITERIA_POSTER_DIS;
				scope.eligibiltyimg = scope.Secformlist[0].ELIGI_CRITERIA_LAT_LONGS;
				scope.UCselbeneficiarylist = scope.Secformlist[0].IS_BENEFI_LIST_DISPLAYED;
				scope.beneficieryimg = scope.Secformlist[0].BEN_LIST_LAT_LONGS;

				scope.UCSecTables = scope.Secformlist[0].NO_OF_TABLES;
				scope.UCSecplasticchairs = scope.Secformlist[0].NO_OF_PLASTIC_CHAIRS;
				scope.UCSecStypechairs = scope.Secformlist[0].NO_OF_S_TYPE_CHAIRS
			    scope.UCSecnoticeboard = scope.Secformlist[0].NO_OF_NOTICE_BOARDS;
				scope.UCSecironsafes = scope.Secformlist[0].NO_OF_IRON_SAFES;
				scope.UCSecIronracks = scope.Secformlist[0].NO_OF_IRON_RACKS;
				scope.UCVoltrafirstphase = scope.Secformlist[0].NO_OF_VOL_TRAINED_1_PHASE;
				scope.UCVoltrasecondphase = scope.Secformlist[0].NO_OF_VOL_TRAINED_2_PHASE;
			scope.UCFunctionariesphase = scope.Secformlist[0].FUNCTIONARIES_TRAINED;
				scope.UCVolVacant = scope.Secformlist[0].VOLUNTEER_POST_VACANCIES;
				scope.UCrbnsecBuildingPaint = scope.Secformlist[0].IS_SEC_BUILDING_PAINTED+"";
				scope.UCrbnsecnameboard = scope.Secformlist[0].IS_NAME_BOARD_AVAILABLE+"";
				scope.UCrbnseccelectrification = scope.Secformlist[0].IS_SEC_ELECTRIFICATION+"";
				scope.UCrbnseccDrinkingWater = scope.Secformlist[0].IS_DRINKING_WATER_AVAILABLE+"";
				scope.UCrbnsecctoilets = scope.Secformlist[0].IS_TOILETS_AVAILABLE+"";
				scope.UCrbnseccstationary = scope.Secformlist[0].IS_STATIONARY_AVAILABLE+"";
				scope.UCrbnseccspandana = scope.Secformlist[0].IS_SPANDHANA_CENT_AVAILABLE+"";
			scope.UCrbnseccbilling = scope.Secformlist[0].IS_BILLING_CENTER_AVAILABLE + "";
			scope.UCrbnMcassActive = scope.Secformlist[0].IS_MCASS_ACTIVATED + "";
			scope.UCSeccMcassUserId = scope.Secformlist[0].MCASS_USER_ID + "";
					
		}

		scope.GetVolEdit = function (obj)
		{
			scope.divvoledit = true;
			scope.UVolunteerName = obj.VOLUNTEER_NAME;
			scope.UVolunteerUID = obj.VOLUNTEER_AADHAR;
			scope.UVolunteerCFMSID = obj.VOL_CFMS_ID,
			scope.UVolClusterName = obj.CLUSTER_NAME;
			scope.UVolunteertype = obj.VOLUNTEER_TYPE;
			scope.USelVolSmartPhMake = obj.VOL_SMART_PHONE_MAKER+"";
			scope.UVolIMEISIMOne = obj.VOL_IMEI_SIM_1;
			scope.UVolIMEISIMTwo = obj.VOL_IMEI_SIM_2;
			scope.USelVolMobileOperator = obj.VOL_OFF_MOBILE_OPERATOR;
			scope.UVolOffcialMobile = obj.VOL_OFFICIAL_MOB_NO;
			scope.UVolPersonalMobile = obj.VOL_PERSONAL_MOB_NO;
			scope.UVolSIMSerialNo = obj.VOL_OFFICAIL_SNO;
			scope.USelVolFPS = obj.VOL_FPS_MAKER+"";
			scope.UVolFPSModelNum = obj.VOL_FPS_MODEL_NO;
			scope.UVolFPSSerialNum = obj.VOL_FPS_SERIAL_NO;
			scope.UrbnvolSmartPhone = obj.IS_VOL_REC_SMART_PHONE+"";
			scope.UrbnvolMobilenumber = obj.IS_VOL_REC_MOB_NO+"";
			scope.Urbnvolprintercumscanner = obj.IS_VOL_REC_FPS+"";
			scope.Urbnvoltelegram = obj.IS_VOL_INSTALL_TELEGRAM+"";
			scope.Uvolwtgroupcluster = obj.VOL_WTSAPP_GROUP_CLUSTER+"";
			scope.UrbnVolMcassActive = obj.IS_MCASS_ACTIVATED+"";
			scope.UVolMcassUserId = obj.MCASS_USER_ID;
		}
		scope.SaveFormData = function () {
			if (!validdate()) {
				return;
			}
			else {
				scope.secdisable = true;
				scope.preloader = true;
				var obj = {
					Ftype:1,DistrictCode: sessionStorage.getItem("distcode"), MandalCode: sessionStorage.getItem("mcode"), RUFlag: sessionStorage.getItem("RUFlag"), SecretariatCode: sessionStorage.getItem("secccode"),
					SecterataitSmartPhone: scope.rbnRecievSmartPhone, SecSmartPhoneMake: scope.SelSmartPhMake, SecSmartPhoneMakeOthers: scope.OtherSmartphmake,
					SecSIMOneIMEI: scope.IMEISIMOne, SecIMEITwoSIM: scope.IMEISIMTwo, SecRecieveMobileNum: scope.rbnRecievMobileNum, SecMobileOperator: scope.SelMobileOperator,
					SecMobileNum: scope.SecMobile, SecSIMSerialNum: scope.SIMSerialNo, FPSScannerAviaiable: scope.rbnFPScanner, FPSMake: scope.SelFPS, FPSModelNum: scope.FPSModelNum,
					FPSSerialNum: scope.FPSSerialNum, PrinterAvailable: scope.rbnPrinter, PrinterMake: scope.SelPrinterMake, PrinterModelNum: scope.PrinterModelNum, PrinterSerialNum: scope.PrinterSerialNum,
					NumofDesktop: scope.SelAvailbleDesktop,
					DesktoponeMake: scope.SelDesktoponeMake,
					DesktoponeSerial: scope.DesktopSerialNumone,
					CPUoneMake: scope.SelCPUoneMake,
					CPUoneSerialNum: scope.CPUSerialNumone,
					MouseoneMake: scope.SelMouseoneMake,
					MouseoneSerialNum: scope.MouseSerialNumone,
					keyboardoneMake: scope.SelKeyboardoneMake,
					keyboardoneSerialNum: scope.KeyboardSerialNumone,

					DesktoptwoMake: scope.SelDesktoptwoMake,
					DesktoptwoSerial: scope.DesktopSerialNumtwo,
					CPUtwoMake: scope.SelCPUtwoMake,
					CPUtwoSerialNum: scope.CPUSerialNumtwo,
					MousetwoMake: scope.SelMousetwoMake,
					MousetwoSerialNum: scope.MouseSerialNumtwo,
					keyboardtwoMake: scope.SelKeyboardtwoMake,
					keyboardtwoSerialNum: scope.KeyboardSerialNumtwo,
					UPSAvailable: scope.SelAvailbleUPS,
					UPSMakeone: scope.UPSMakeone,
					UPSoneSerialnum: scope.UPSSerialnumone,
					UPSMaketwo: scope.UPSMaketwo,
					UPStwoSerialnum: scope.UPSSerialnumtwo,
					RecieveMahilePoliceSmartPhone: scope.rbnRecievMahilaSmartPhone,
					MahilaSmartPhMake: scope.SelMahilaSmartPhMake,
					MahileOtherSmartMake: scope.OtherMahilaSmartphmake,
					MahilaIMEINumone: scope.MahilaIMEISIMOne,
					MahilaIMEINumtwo: scope.MahilaIMEISIMTwo,
					RecieveMobileNumber: scope.rbnRecievMahilaMobileNum,
					MahilaMobileOperator: scope.SelMahilaMobileOperator,
					MahileMobileNumber: scope.MahilaPoliceMobile,
					MahilaSIMSerialNum: scope.MahilaSIMSerialNo,
					VolunteerNoofSmartPh: scope.VolSmartpohe,
					VolunteerNoofSIMCard: scope.VolSIMCard,
					InternetAvailble: scope.selinternetconnectvity,
					MobileMdmInstall: scope.mdminstall,
					NoofArogyasetuapp: scope.noofArogyasethuapp,
					HighSecurityStationeryRec: scope.Highsecuritystationery,
					HighSecurityStationeryStock_Availoable: scope.HighstationerystockAvailable,
					EligibilityCriteria: scope.seleligibility,
						EligibilityImage:scope.eligibiltyimg,
					Beneficiarylist: scope.selbeneficiarylist,
						BeneficiarylistImage:scope.beneficieryimg,

					SecTables: scope.SecTables,
					SecPlasticchairs: scope.Secplasticchairs,
					SecStypechairs: scope.SecStypechairs,
					Secnoticeboard: scope.Secnoticeboard,
					SecIronsafes: scope.Secironsafes,
					SecIronracks: scope.SecIronracks,
					VolunteerPhaseone: scope.Voltrafirstphase,
					VolunteerPhasetwo: scope.Voltrasecondphase,
					FunctionariesTrained: scope.Functionariesphase,
					VolunteerVacant: scope.VolVacant,
					SecBuildingPaint: scope.rbnsecBuildingPaint,
					Secnameboard: scope.rbnsecnameboard,
					SecElectrification: scope.rbnseccelectrification,
					SecDrinkwater: scope.rbnseccDrinkingWater,
					SecToilets: scope.rbnsecctoilets,
					SecStationary: scope.rbnseccstationary,
					SecSpandana: scope.rbnseccspandana,
					SecBillingCounter: scope.rbnseccbilling,
					MCassActive: scope.rbnMcassActive,
					MCassUserId: scope.SeccMcassUserId,
					Insertby: sessionStorage.getItem("user")

				}

				var token = sessionStorage.getItem("Token");
				//var req = { FType: "1", DistCode: sessionStorage.getItem("distcode"), MCode: sessionStorage.getItem("mcode"), SecCode: sessionStorage.getItem("secccode"), Loginuser: sessionStorage.getItem("user"), FilePath: pdffile }
				Login_Services.POSTENCRYPTAPI("SaveSecretariatForm", obj, token, function (value) {
					if (value.data.Status == "100") {
						scope.preloader = false;
						swal('info', value.data.Reason, 'success');
						window.location.reload();
					}
					else {
						scope.secdisable = false;
						scope.preloader = false;
						swal('info', value.data.Reason, 'info');
					}
				});
			}
		}
		function validdate() {
			var IndNum = /^[0]?[6789]\d{9}$/;
			if (!scope.rbnRecievSmartPhone) {
				swal('info', 'Whether Secteratait received Official Smart Phone?', 'info');
				return false;
			}
			if (scope.rbnRecievSmartPhone == '1') {
				
				if (!scope.SelSmartPhMake) {
					swal('info', 'Please Select  Smart Phone Make', 'info');
					return false;
				}
				if (scope.SelSmartPhMake == 'Other') {
					if (!scope.OtherSmartphmake) {
						swal('info', 'Please Select  Smart Phone Make', 'info');
						return false;
					}
					else { }
				}
				if (!scope.IMEISIMOne) {
					swal('info', 'IMEI(SIM Slot 1) Number', 'info');
					return false;
				}
				if (!scope.IMEISIMTwo) {
					swal('info', 'IMEI(SIM Slot 2) Number', 'info');
					return false;
				}
				else { }
			}
			if (!scope.rbnRecievMobileNum) {
				swal('info', 'Please Check,Whether Secteratait received Official Mobile Number?', 'info');
				return false;
			}
			if (scope.rbnRecievMobileNum == '1') {
				if (!scope.SelMobileOperator) {
					swal('info', 'Please Select Mobile Operator', 'info');
					return false;
				}
				if (!scope.SecMobile) {
					swal('info', 'Please Enter Secretariat Mobile Number', 'info');
					return false;
				}
				if (!IndNum.test(scope.SecMobile)) {
					swal('info', 'Please Enter Valid Secretariat Mobile Number', 'info');
					return false;
				}
				if (!scope.SIMSerialNo) {
					swal('info', 'Please Enter SIM Serial No', 'info');
					return false;
				}
			}
			if (!scope.rbnFPScanner) {
				swal('info', 'Please Check,Whether Secteratait Received Finger Print Scanner?', 'info');
				return false;
			}
			if (scope.rbnFPScanner == '1') {
				if (!scope.SelFPS) {
					swal('info', 'Please Select FPS', 'info');
					return false;
				}
				if (!scope.FPSModelNum) {
					swal('info', 'Please Enter FPS Model Number', 'info');
					return false;
				}
				if (!scope.FPSSerialNum) {
					swal('info', 'Please FPS Serial Number', 'info');
					return false;
				}
			}
			if (!scope.rbnPrinter) {
				swal('info', 'Please Check,Whether Secteratait Received Printer cum Scanner?', 'info');
				return false;
			}
			if (scope.rbnPrinter == '1') {
				if (!scope.SelPrinterMake) {
					swal('info', 'Please Select Printer cum Scanner', 'info');
					return false;
				}
				if (!scope.PrinterModelNum) {
					swal('info', 'Please Enter Printer cum Scanner Model Number', 'info');
					return false;
				}
				if (!scope.PrinterSerialNum) {
					swal('info', 'Please Printer cum  Serial Number', 'info');
					return false;
				}
			}
			if (!scope.SelAvailbleDesktop) {
				swal('info', 'Select Desktop Computers available ', 'info');
				return false;
			}
			if (scope.SelAvailbleDesktop == '1' || scope.SelAvailbleDesktop == '2') {
				if (!scope.SelDesktoponeMake) {
					swal('info', 'Please Select Desktop Computer - 1 Make', 'info');
					return false;
				}
				if (!scope.DesktopSerialNumone) {
					swal('info', 'Please Enter Desktop Computer - 1 Serial Number', 'info');
					return false;
				}
				if (!scope.SelCPUoneMake) {
					swal('info', 'Please Select CPU - 1 Make', 'info');
					return false;
				}
				if (!scope.CPUSerialNumone) {
					swal('info', 'Please Enter CPU - 1 Serial Number', 'info');
					return false;
				}
				if (!scope.SelMouseoneMake) {
					swal('info', 'Please Select Mouse - 1 Make', 'info');
					return false;
				}
				if (!scope.MouseSerialNumone) {
					swal('info', 'Please Enter Mouse - 1 Serial Number', 'info');
					return false;
				}
				if (!scope.SelKeyboardoneMake) {
					swal('info', 'Please Select Keyboard - 1 Make', 'info');
					return false;
				}
				if (!scope.KeyboardSerialNumone) {
					swal('info', 'Please Enter keyboard - 1 Serial Number', 'info');
					return false;
				}
			}

			if (scope.SelAvailbleDesktop == '2') {
				if (!scope.SelDesktoptwoMake) {
					swal('info', 'Please Select Desktop Computer - 2 Make', 'info');
					return false;
				}
				if (!scope.DesktopSerialNumtwo) {
					swal('info', 'Please Enter Desktop Computer - 2 Serial Number', 'info');
					return false;
				}
				if (!scope.SelCPUtwoMake) {
					swal('info', 'Please Select CPU - 2 Make', 'info');
					return false;
				}
				if (!scope.CPUSerialNumtwo) {
					swal('info', 'Please Enter CPU - 2 Serial Number', 'info');
					return false;
				}
				if (!scope.SelMousetwoMake) {
					swal('info', 'Please Select Mouse - 2 Make', 'info');
					return false;
				}
				if (!scope.MouseSerialNumtwo) {
					swal('info', 'Please Enter Mouse - 2 Serial Number', 'info');
					return false;
				}
				if (!scope.SelKeyboardtwoMake) {
					swal('info', 'Please Select Keyboard - 2 Make', 'info');
					return false;
				}
				if (!scope.KeyboardSerialNumtwo) {
					swal('info', 'Please Enter keyboard - 2 Serial Number', 'info');
					return false;
				}
			}

			if (!scope.SelAvailbleUPS) {
				swal('info', 'Please select How Many UPS available at Secretariat', 'info');
				return false;
			}
			if (scope.SelAvailbleUPS == '1' || scope.SelAvailbleUPS == '2') {
				if (!scope.UPSMakeone) {
					swal('info', 'Please Enter UPS-1 Make', 'info');
					return false;
				}
				if (!scope.UPSSerialnumone) {
					swal('info', 'Please Enter UPS-1 Serial Number', 'info');
					return false;
				}

			}
			if (scope.SelAvailbleUPS == '2') {
				if (!scope.UPSMaketwo) {
					swal('info', 'Please Enter UPS-2 Make', 'info');
					return false;
				}
				if (!scope.UPSSerialnumtwo) {
					swal('info', 'Please Enter UPS-2 Serial Number', 'info');
					return false;
				}

			}

			if (!scope.rbnRecievMahilaSmartPhone) {
				swal('info', 'Please Whether Mahila Police received Official Smart Phone', 'info');
				return false;
			}
			if (scope.rbnRecievMahilaSmartPhone == '1') {

				if (!scope.SelMahilaSmartPhMake) {
					swal('info', 'Please Select  Smart Phone Make(Mahila Police)', 'info');
					return false;
				}
				if (scope.SelMahilaSmartPhMake == 'Other') {
					if (!scope.OtherMahilaSmartphmake) {
						swal('info', 'Please  Enter Other  Smart Phone Make(Mahila Police)', 'info');
						return false;
					}
					else { }
				}
				if (!scope.MahilaIMEISIMOne) {
					swal('info', 'IMEI(SIM Slot 1) Number Mahila Police', 'info');
					return false;
				}
				if (!scope.MahilaIMEISIMTwo) {
					swal('info', 'IMEI(SIM Slot 2) Number Mahila Police', 'info');
					return false;
				}
				else { }
			}
			if (!scope.rbnRecievMahilaMobileNum) {
				swal('info', 'Please Check,Whether Secteratait received Official Mobile Number Mahila Police?', 'info');
				return false;
			}
			if (scope.rbnRecievMahilaMobileNum == '1') {
				if (!scope.SelMahilaMobileOperator) {
					swal('info', 'Please Select Mobile Operator(Mahila Police)', 'info');
					return false;
				}
				if (!scope.MahilaPoliceMobile) {
					swal('info', 'Please Enter Mahila Police Mobile Number', 'info');
					return false;
				}
				if (!IndNum.test(scope.MahilaPoliceMobile)) {
					swal('info', 'Please Enter Valid Mahila Police Mobile Number', 'info');
					return false;
				}
				if (!scope.MahilaSIMSerialNo) {
					swal('info', 'Please Enter Mahila Police SIM Serial No', 'info');
					return false;
				}
			}
			if (!scope.VolSmartpohe) {
				swal('info', 'Please Enter Number of smart phones', 'info');
				return false;
			}
			if (!scope.VolSIMCard) {
				swal('info', 'Please Enter Number of SIM Cards', 'info');
				return false;
			}
			if (!scope.selinternetconnectvity) {
				swal('info', 'Please Select Internet Connectvity', 'info');
				return false;
			}
			if (!scope.mdminstall) {
				swal('info', 'Please Enter No of MDM Install ', 'info');
				return false;
			}

			if (!scope.noofArogyasethuapp) {
				swal('info', 'Please Enter no of Volunteer install Arogyasethu app', 'info');
				return false;
			}
			if (!scope.Highsecuritystationery) {
				swal('info', 'Please Enter No of high security stationery Received', 'info');
				return false;
			}
			if (!scope.HighstationerystockAvailable) {
				swal('info', 'Please Enter No of High Security stationery stock available', 'info');
				return false;
			}
			if (!scope.seleligibility) {
				swal('info', 'Please Select Eligibility Criteria Posters Displayed ', 'info');
				return false;
			}
			if (scope.seleligibility == '1') {
				if (!scope.eligibiltyimg) {
					swal('info', 'Please Upload eligible criteria Image ', 'info');
					return false;
				}
			}
			if (!scope.selbeneficiarylist) {
				swal('info', 'Please Select Beneficiary list Displayed ', 'info');
				return false;
			}
			if (scope.selbeneficiarylist == '1') {
				if (!scope.beneficieryimg) {
					swal('info', 'Please Upload Beneficiary list Image ', 'info');
					return false;
				}
			}
			
			if (!scope.SecTables) {
				swal('info', 'Please Enter Number of Tables', 'info');
				return false;
			}
			if (!scope.Secplasticchairs) {
				swal('info', 'Please Enter Number of Plastic Chairs', 'info');
				return false;
			}
			if (!scope.SecStypechairs) {
				swal('info', 'Please Enter Number of S type Chairs', 'info');
				return false;
			}
			if (!scope.Secnoticeboard) {
				swal('info', 'Please Enter Number of notice board', 'info');
				return false;
			}
			if (!scope.Secironsafes) {
				swal('info', 'Please Enter Number of Iron Safes', 'info');
				return false;
			}
			if (!scope.Secironracks) {
				swal('info', 'Please Enter Number of Iron racks', 'info');
				return false;
			}
			if (!scope.Voltrafirstphase) {
				swal('info', 'Please Enter Number of Volunteer Trained Phase one', 'info');
				return false;
			}
			if (!scope.Voltrasecondphase) {
				swal('info', 'Please Enter Number of Volunteer Trained Phase two', 'info');
				return false;
			}
			if (!scope.Functionariesphase) {
				swal('info', 'Please Enter Number of Functionaries Trained phase one and phase 2', 'info');
				return false;
			}
			if (!scope.VolVacant) {
				swal('info', 'Please Enter Number of Volunteer Post Vacant', 'info');
				return false;
			}

			if (!scope.rbnsecBuildingPaint) {
				swal('info', 'Please check Whether Building Paints', 'info');
				return false;
			}

			if (!scope.rbnsecnameboard) {
				swal('info', 'Please check Whether Name Board', 'info');
				return false;
			}
			if (!scope.rbnseccelectrification) {
				swal('info', 'Please check Whether Electrification?', 'info');
				return false;
			}
			
			if (!scope.rbnseccDrinkingWater) {
				swal('info', 'Please check Whether Drinking Water Facility?', 'info');
				return false;
			}
			if (!scope.rbnsecctoilets) {
				swal('info', 'Please check Whether Toilets Facility', 'info');
				return false;
			}

			if (!scope.rbnseccstationary) {
				swal('info', 'Please check Stationary Availability', 'info');
				return false;
			}
			if (!scope.rbnseccspandana) {
				swal('info', 'Please check Whether Spandana Counter?', 'info');
				return false;
			}

			if (!scope.rbnseccbilling) {
				swal('info', 'Please check  Billing Counter', 'info');
				return false;
			}
			if (!scope.rbnMcassActive) {
				swal('info', 'Please check Whether Mcass activcated', 'info');
				return false;
			}
			if (scope.rbnMcassActive == '1') {
				if (!scope.SeccMcassUserId) {
					swal('info', 'Please Enter MCass User ID', 'info');
					return false;
				}
				else {
					return true;
				}
			}
			else {
				return true;
			}
		}

		scope.uploadfile = function (type, category) {

			
			var file = type.attachment;
			var fileexten = file.type;
			var FileSize = file.size;
			if (FileSize > 1000000) {
				swal('info', 'File size exceeded 1 MB', 'error');
				scope.preloader = false;
				angular.element("input[type='file']").val('');
				return;

			}
			if (fileexten.split("/")[1] == "jpg" || fileexten.split("/")[1] == "JPG" || fileexten.split("/")[1] == "JPEG" || fileexten.split("/")[1] == "jpeg") {

			}
			else {
				scope.preloader = false;
				swal("info", "Only JPG Image Accepted", "info");
				return;
			}
			var prop = { AadharCardNumber: sessionStorage.getItem("secccode"), Attachment: type.attachment, CertificateCategory: category };
			entityService.saveTutorial(prop, "/Home/SecImageData")
				.then(function (data) {
					scope.imagedata = data.data;
					if (scope.imagedata.match("Failure")) {
						swal('info', scope.imagedata, 'error');
						scope.preloader = false;
						angular.element("input[type='file']").val('');
						return;
					}
					if (category == "eligibility") {
						scope.eligibiltyimg = data.data;
						swal('info', "file Uploaded Successfully", 'success');
					}
					else if (category == "Beneficiary") {
						scope.beneficieryimg = data.data;
						swal('info', "file Uploaded Successfully", 'success');
					}
					




					//getCertificateDetails();
					console.log(data);
				});
		}

		scope.SaveVolunteer = function () {
			if (!Volunteervalidate()) {
				return;
			}
			else {
				scope.voldisable = true;
				scope.preloader = true;
				var obj = {
					Ftype: 1, DistrictCode: sessionStorage.getItem("distcode"), MandalCode: sessionStorage.getItem("mcode"), RUFlag: sessionStorage.getItem("RUFlag"), SecretariatCode: sessionStorage.getItem("secccode"),
					VolunteerName: scope.VolunteerName, VolunteerUID: scope.VolunteerUID, VolunteerCFMSID: scope.VolunteerCFMSID,
					VolunteerClusterName: scope.VolClusterName, VolunteertType: scope.Volunteertype, VolunteerSmartPhone: scope.SelVolSmartPhMake,
					VolunteerSIMIMEIOne: scope.VolIMEISIMOne, VolunteerSIMIMEITwo: scope.VolIMEISIMTwo, VolunteerMobileOperator: scope.SelVolMobileOperator,
					VolunteerOfficialMobile: scope.VolOffcialMobile,
					VolunteerPersonalMobile: scope.VolPersonalMobile, VolunteerSerialNumber: scope.VolSIMSerialNo,
					VolunteerFPS: scope.SelVolFPS, VolunteerFPSModelNumber: scope.VolFPSModelNum,
					VolunteerFPSSerialNumber: scope.VolFPSSerialNum,
					VolunteerSmartPhoneRecieve: scope.rbnvolSmartPhone,
					VolunteerSIMMobileNumbers: scope.rbnvolMobilenumber,
					VolunteerFingerprintcumscanner: scope.rbnvolprintercumscanner,
					VolunteerinstallTelgram: scope.rbnvoltelegram,
					VolunteerWtsclustergroup: scope.volwtgroupcluster,
					VolMCassActive: scope.rbnVolMcassActive,
					VolMCassUserId: scope.VolMcassUserId,
					Insertby: sessionStorage.getItem("user")

				}

				var token = sessionStorage.getItem("Token");
				//var req = { FType: "1", DistCode: sessionStorage.getItem("distcode"), MCode: sessionStorage.getItem("mcode"), SecCode: sessionStorage.getItem("secccode"), Loginuser: sessionStorage.getItem("user"), FilePath: pdffile }
				Login_Services.POSTENCRYPTAPI("SaveValunteerForm", obj, token, function (value) {
					if (value.data.Status == "100") {
						scope.preloader = false;
						swal('info', value.data.Reason, 'success');
						window.location.reload();
					}
					else {
						scope.preloader = false;
						scope.voldisable = false;
						swal('info', value.data.Reason, 'info');
					}
				});
			}
		}
		function Volunteervalidate() {
			var IndNum = /^[0]?[6789]\d{9}$/;
			if (!scope.VolunteerName) {
				swal('info', 'Please Enter Volunteer Name', 'info');
				return false;
			}
			if (!scope.VolunteerUID) {
				swal('info', 'Please Enter Volunteer Aadhaar Number', 'info');
				return false;
			}
			if (!validateVerhoeff(scope.VolunteerUID)) {
				swal('info', 'Please Enter Valid Volunteer Aadhar Number', 'info');
				return false;
			}
			if (!scope.VolunteerCFMSID) {
				swal('info', 'Please Enter Valid Volunteer  CFMS ID', 'info');
				return false;
			}
			if (!scope.VolClusterName) {
				swal('info', 'Please Enter Volunteer Cluster Name', 'info');
				return false;
			}

			if (!scope.Volunteertype) {
				swal('info', 'Please Select Volunteer Type', 'info');
				return false;
			}
			if (!scope.rbnvolSmartPhone) {
				swal('info', 'Please Check Whether Volunteer received an Official SmartPhone?', 'info');
				return false;
			}

			if (scope.rbnvolSmartPhone == '1') {
				if (!scope.SelVolSmartPhMake) {
					swal('info', 'Please Select Volunteer Smart Phone Make', 'info');
					return false;
				}

				if (scope.SelVolSmartPhMake == "Other") {
					if (!scope.OtherVolSmartphmake) {
						swal('info', 'Please Enter Other Volunteer Smart Phone Make', 'info');
						return false;
					}
				}
				if (!scope.VolIMEISIMOne) {
					swal('info', 'Please Enter Volunteer IMEI SIM-1  Number', 'info');
					return false;
				}
				if (!scope.VolIMEISIMTwo) {
					swal('info', 'Please Enter Volunteer IMEI SIM-2  Number', 'info');
					return false;
				}
			}
			if (!scope.rbnvolMobilenumber) {
				swal('info', 'Please Check Whether Volunteer received an Official Mobile Number?', 'info');
				return false;
			}
			if (scope.rbnvolMobilenumber == '1') {
				
				if (!scope.SelVolMobileOperator) {
					swal('info', 'Please Select Mobile Operator', 'info');
					return false;
				}
				if (!scope.VolOffcialMobile) {
					swal('info', 'Please Enter Volunter Official Mobile  Number', 'info');
					return false;
				}
				if (!IndNum.test(scope.VolOffcialMobile)) {

					swal('info', 'Please Enter Valid Volunter Official Mobile  Number', 'info');
					return false;
				}
				if (!scope.VolPersonalMobile) {
					swal('info', 'Please Enter Volunteer Personal Mobile Number', 'info');
					return false;
				}
				if (!scope.VolSIMSerialNo) {

					swal('info', 'Please Enter Valid Volunter SIM Serial Number', 'info');
					return false;
				}
			}

			if (!scope.rbnvolprintercumscanner) {

				swal('info', 'Please Check Whether Volunteer received a Printer cum Scanner?', 'info');
				return false;
			}
			if (scope.rbnvolprintercumscanner == '1') {
				if (!scope.SelVolFPS) {

					swal('info', 'Please Select Volunteer Fingerprint Scanner', 'info');
					return false;
				}
				if (!scope.VolFPSModelNum) {

					swal('info', 'Please Enter FPS Model Number', 'info');
					return false;
				}

				if (!scope.VolFPSSerialNum) {

					swal('info', 'Please Enter FPS Serial Number', 'info');
					return false;
				}
			}
			if (!scope.rbnvoltelegram) {
				swal('info', 'Please Check Whether Volunteer installed Telegram?', 'info');
				return false;
			}
			if (!scope.volwtgroupcluster) {
				swal('info', 'Please Check Whether volunteer created WhatsApp group for their cluster?', 'info');
				return false;
			}
			if (!scope.rbnVolMcassActive) {
				swal('info', 'Please check Whether Mcass activcated', 'info');
				return false;
			}
			if (scope.rbnVolMcassActive == '1') {
				if (!scope.VolMcassUserId) {
					swal('info', 'Please Enter Volu MCass User ID', 'info');
					return false;
				}
				else {
					return true;
				}
			}
			else {
				return true;
			}
		}
		function UpdateVolunteervalidate() {
			var IndNum = /^[0]?[6789]\d{9}$/;
			if (!scope.UVolunteerName) {
				swal('info', 'Please Enter Volunteer Name', 'info');
				return false;
			}
			if (!scope.UVolunteerUID) {
				swal('info', 'Please Enter Volunteer Aadhaar Number', 'info');
				return false;
			}
			if (!validateVerhoeff(scope.UVolunteerUID)) {
				swal('info', 'Please Enter Valid Volunteer Aadhar Number', 'info');
				return false;
			}
			if (!scope.UVolunteerCFMSID) {
				swal('info', 'Please Enter Valid Volunteer  CFMS ID', 'info');
				return false;
			}
			if (!scope.UVolClusterName) {
				swal('info', 'Please Enter Volunteer Cluster Name', 'info');
				return false;
			}

			if (!scope.UVolunteertype) {
				swal('info', 'Please Select Volunteer Type', 'info');
				return false;
			}
			if (!scope.UrbnvolSmartPhone) {
				swal('info', 'Please Check Whether Volunteer received an Official SmartPhone?', 'info');
				return false;
			}

			if (scope.UrbnvolSmartPhone == '1') {
				if (!scope.USelVolSmartPhMake) {
					swal('info', 'Please Select Volunteer Smart Phone Make', 'info');
					return false;
				}

				if (scope.USelVolSmartPhMake == "Other") {
					if (!scope.UOtherVolSmartphmake) {
						swal('info', 'Please Enter Other Volunteer Smart Phone Make', 'info');
						return false;
					}
				}
				if (!scope.UVolIMEISIMOne) {
					swal('info', 'Please Enter Volunteer IMEI SIM-1  Number', 'info');
					return false;
				}
				if (!scope.UVolIMEISIMTwo) {
					swal('info', 'Please Enter Volunteer IMEI SIM-2  Number', 'info');
					return false;
				}
			}
			if (!scope.UrbnvolMobilenumber) {
				swal('info', 'Please Check Whether Volunteer received an Official Mobile Number?', 'info');
				return false;
			}
			if (scope.UrbnvolMobilenumber == '1') {

				if (!scope.USelVolMobileOperator) {
					swal('info', 'Please Select Mobile Operator', 'info');
					return false;
				}
				if (!scope.UVolOffcialMobile) {
					swal('info', 'Please Enter Volunter Official Mobile  Number', 'info');
					return false;
				}
				if (!IndNum.test(scope.UVolOffcialMobile)) {

					swal('info', 'Please Enter Valid Volunter Official Mobile  Number', 'info');
					return false;
				}
				if (!scope.UVolPersonalMobile) {
					swal('info', 'Please Enter Volunteer Personal Mobile Number', 'info');
					return false;
				}
				if (!scope.UVolSIMSerialNo) {

					swal('info', 'Please Enter Valid Volunter SIM Serial Number', 'info');
					return false;
				}
			}

			if (!scope.Urbnvolprintercumscanner) {

				swal('info', 'Please Check Whether Volunteer received a Printer cum Scanner?', 'info');
				return false;
			}
			if (scope.Urbnvolprintercumscanner == '1') {
				if (!scope.USelVolFPS) {

					swal('info', 'Please Select Volunteer Fingerprint Scanner', 'info');
					return false;
				}
				if (!scope.UVolFPSModelNum) {

					swal('info', 'Please Enter FPS Model Number', 'info');
					return false;
				}

				if (!scope.UVolFPSSerialNum) {

					swal('info', 'Please Enter FPS Serial Number', 'info');
					return false;
				}
			}
			if (!scope.Urbnvoltelegram) {
				swal('info', 'Please Check Whether Volunteer installed Telegram?', 'info');
				return false;
			}
			if (!scope.Uvolwtgroupcluster) {
				swal('info', 'Please Check Whether volunteer created WhatsApp group for their cluster?', 'info');
				return false;
			}
			if (!scope.UrbnVolMcassActive) {
				swal('info', 'Please check Whether Mcass activcated', 'info');
				return false;
			}
			if (scope.UrbnVolMcassActive == '1') {
				if (!scope.UVolMcassUserId) {
					swal('info', 'Please Enter Volu MCass User ID', 'info');
					return false;
				}
				else {
					return true;
				}
			}
			else {
				return true;
			}
		}

		//update seccform
		scope.UpdateSeccFormData = function () {
			if (!Updatevaliddate()) {
				return;
			}
			else {
				scope.ucsecdisable = true;
				scope.preloader = true;
				var obj = {
					Ftype: 2, DistrictCode: sessionStorage.getItem("distcode"), MandalCode: sessionStorage.getItem("mcode"), RUFlag: sessionStorage.getItem("RUFlag"), SecretariatCode: sessionStorage.getItem("secccode"),
					SecterataitSmartPhone: scope.UCrbnRecievSmartPhone, SecSmartPhoneMake: scope.UCSelSmartPhMake, SecSmartPhoneMakeOthers: scope.UCOtherSmartphmake,
					SecSIMOneIMEI: scope.UCIMEISIMOne, SecIMEITwoSIM: scope.UCIMEISIMTwo, SecRecieveMobileNum: scope.UCrbnRecievMobileNum, SecMobileOperator: scope.UCSelMobileOperator,
					SecMobileNum: scope.UCSecMobile, SecSIMSerialNum: scope.UCSIMSerialNo, FPSScannerAviaiable: scope.UCrbnFPScanner, FPSMake: scope.UCSelFPS, FPSModelNum: scope.UCFPSModelNum,
					FPSSerialNum: scope.UCFPSSerialNum, PrinterAvailable: scope.UCrbnPrinter, PrinterMake: scope.UCSelPrinterMake, PrinterModelNum: scope.UCPrinterModelNum, PrinterSerialNum: scope.UCPrinterSerialNum,
					NumofDesktop: scope.UCSelAvailbleDesktop,
					DesktoponeMake: scope.UCSelDesktoponeMake,
					DesktoponeSerial: scope.UCDesktopSerialNumone,
					CPUoneMake: scope.UCSelCPUoneMake,
					CPUoneSerialNum: scope.UCCPUSerialNumone,
					MouseoneMake: scope.UCSelMouseoneMake,
					MouseoneSerialNum: scope.UCMouseSerialNumone,
					keyboardoneMake: scope.UCSelKeyboardoneMake,
					keyboardoneSerialNum: scope.UCKeyboardSerialNumone,

					DesktoptwoMake: scope.UCSelDesktoptwoMake,
					DesktoptwoSerial: scope.UCDesktopSerialNumtwo,
					CPUtwoMake: scope.UCSelCPUtwoMake,
					CPUtwoSerialNum: scope.UCCPUSerialNumtwo,
					MousetwoMake: scope.UCSelMousetwoMake,
					MousetwoSerialNum: scope.UCMouseSerialNumtwo,
					keyboardtwoMake: scope.UCSelKeyboardtwoMake,
					keyboardtwoSerialNum: scope.UCKeyboardSerialNumtwo,
					UPSAvailable: scope.UCSelAvailbleUPS,
					UPSMakeone: scope.UCUPSMakeone,
					UPSoneSerialnum: scope.UCUPSSerialnumone,
					UPSMaketwo: scope.UCUPSMaketwo,
					UPStwoSerialnum: scope.UCUPSSerialnumtwo,
					RecieveMahilePoliceSmartPhone: scope.UCrbnRecievMahilaSmartPhone,
					MahilaSmartPhMake: scope.UCSelMahilaSmartPhMake,
					MahileOtherSmartMake: scope.UCOtherMahilaSmartphmake,
					MahilaIMEINumone: scope.UCMahilaIMEISIMOne,
					MahilaIMEINumtwo: scope.UCMahilaIMEISIMTwo,
					RecieveMobileNumber: scope.UCrbnRecievMahilaMobileNum,
					MahilaMobileOperator: scope.UCSelMahilaMobileOperator,
					MahileMobileNumber: scope.UCMahilaPoliceMobile,
					MahilaSIMSerialNum: scope.UCMahilaSIMSerialNo,
					VolunteerNoofSmartPh: scope.UCVolSmartpohe,
					VolunteerNoofSIMCard: scope.UCVolSIMCard,
					InternetAvailble: scope.UCselinternetconnectvity,
					MobileMdmInstall: scope.UCmdminstall,
					NoofArogyasetuapp: scope.UCnoofArogyasethuapp,
					HighSecurityStationeryRec: scope.UCHighsecuritystationery,
					HighSecurityStationeryStock_Availoable: scope.UCHighstationerystockAvailable,
					EligibilityCriteria: scope.UCseleligibility,
					EligibilityImage: scope.UCeligibiltyimg,
					Beneficiarylist: scope.UCselbeneficiarylist,
					BeneficiarylistImage: scope.beneficieryimg,

					SecTables: scope.UCSecTables,
					SecPlasticchairs: scope.UCSecplasticchairs,
					SecStypechairs: scope.UCSecStypechairs,
					Secnoticeboard: scope.UCSecnoticeboard,
					SecIronsafes: scope.UCSecironsafes,
					SecIronracks: scope.UCSecIronracks,
					VolunteerPhaseone: scope.UCVoltrafirstphase,
					VolunteerPhasetwo: scope.UCVoltrasecondphase,
					FunctionariesTrained: scope.UCFunctionariesphase,
					VolunteerVacant: scope.UCVolVacant,
					SecBuildingPaint: scope.UCrbnsecBuildingPaint,
					Secnameboard: scope.UCrbnsecnameboard,
					SecElectrification: scope.UCrbnseccelectrification,
					SecDrinkwater: scope.UCrbnseccDrinkingWater,
					SecToilets: scope.UCrbnsecctoilets,
					SecStationary: scope.UCrbnseccstationary,
					SecSpandana: scope.UCrbnseccspandana,
					SecBillingCounter: scope.UCrbnseccbilling,
					MCassActive: scope.UCrbnMcassActive,
					MCassUserId: scope.UCSeccMcassUserId,
					Insertby: sessionStorage.getItem("user")

				}

				var token = sessionStorage.getItem("Token");
				//var req = { FType: "1", DistCode: sessionStorage.getItem("distcode"), MCode: sessionStorage.getItem("mcode"), SecCode: sessionStorage.getItem("secccode"), Loginuser: sessionStorage.getItem("user"), FilePath: pdffile }
				Login_Services.POSTENCRYPTAPI("SaveSecretariatForm", obj, token, function (value) {
					if (value.data.Status == "100") {
						scope.preloader = false;
						swal('info', value.data.Reason, 'success');
						window.location.reload();
					}
					else {
						scope.secdisable = false;
						scope.preloader = false;
						swal('info', value.data.Reason, 'info');
					}
				});
			}
		}

		//update Volunteer Foem
		scope.UpdateVolunteer = function () {
			if (!UpdateVolunteervalidate()) {
				return;
			}
			else {
				scope.Uvoldisable = true;
				scope.preloader = true;
				var obj = {
					Ftype: 2, DistrictCode: sessionStorage.getItem("distcode"), MandalCode: sessionStorage.getItem("mcode"), RUFlag: sessionStorage.getItem("RUFlag"), SecretariatCode: sessionStorage.getItem("secccode"),
					VolunteerName: scope.UVolunteerName, VolunteerUID: scope.UVolunteerUID, VolunteerCFMSID: scope.UVolunteerCFMSID,
					VolunteerClusterName: scope.UVolClusterName, VolunteertType: scope.UVolunteertype, VolunteerSmartPhone: scope.USelVolSmartPhMake,
					VolunteerSIMIMEIOne: scope.VolIMEISIMOne, VolunteerSIMIMEITwo: scope.VolIMEISIMTwo, VolunteerMobileOperator: scope.USelVolMobileOperator,
					VolunteerOfficialMobile: scope.UVolOffcialMobile,
					VolunteerPersonalMobile: scope.UVolPersonalMobile, VolunteerSerialNumber: scope.UVolSIMSerialNo,
					VolunteerFPS: scope.USelVolFPS, VolunteerFPSModelNumber: scope.UVolFPSModelNum,
					VolunteerFPSSerialNumber: scope.UVolFPSSerialNum,
					VolunteerSmartPhoneRecieve: scope.UrbnvolSmartPhone,
					VolunteerSIMMobileNumbers: scope.UrbnvolMobilenumber,
					VolunteerFingerprintcumscanner: scope.Urbnvolprintercumscanner,
					VolunteerinstallTelgram: scope.Urbnvoltelegram,
					VolunteerWtsclustergroup: scope.Uvolwtgroupcluster,
					VolMCassActive: scope.UrbnVolMcassActive,
					VolMCassUserId: scope.UVolMcassUserId,
					Insertby: sessionStorage.getItem("user")

				}

				var token = sessionStorage.getItem("Token");
				//var req = { FType: "1", DistCode: sessionStorage.getItem("distcode"), MCode: sessionStorage.getItem("mcode"), SecCode: sessionStorage.getItem("secccode"), Loginuser: sessionStorage.getItem("user"), FilePath: pdffile }
				Login_Services.POSTENCRYPTAPI("SaveValunteerForm", obj, token, function (value) {
					if (value.data.Status == "100") {
						scope.preloader = false;
						swal('info', value.data.Reason, 'success');
						window.location.reload();
					}
					else {
						scope.preloader = false;
						scope.voldisable = false;
						swal('info', value.data.Reason, 'info');
					}
				});
			}
		}
		function Updatevaliddate() {
			var IndNum = /^[0]?[6789]\d{9}$/;
			if (!scope.UCrbnRecievSmartPhone) {
				swal('info', 'Whether Secteratait received Official Smart Phone?', 'info');
				return false;
			}
			if (scope.UCrbnRecievSmartPhone == '1') {

				if (!scope.UCSelSmartPhMake) {
					swal('info', 'Please Select  Smart Phone Make', 'info');
					return false;
				}
				if (scope.UCSelSmartPhMake == 'Other') {
					if (!scope.UCOtherSmartphmake) {
						swal('info', 'Please Select  Smart Phone Make', 'info');
						return false;
					}
					else { }
				}
				if (!scope.UCIMEISIMOne) {
					swal('info', 'IMEI(SIM Slot 1) Number', 'info');
					return false;
				}
				if (!scope.UCIMEISIMTwo) {
					swal('info', 'IMEI(SIM Slot 2) Number', 'info');
					return false;
				}
				else { }
			}
			if (!scope.UCrbnRecievMobileNum) {
				swal('info', 'Please Check,Whether Secteratait received Official Mobile Number?', 'info');
				return false;
			}
			if (scope.UCrbnRecievMobileNum == '1') {
				if (!scope.UCSelMobileOperator) {
					swal('info', 'Please Select Mobile Operator', 'info');
					return false;
				}
				if (!scope.UCSecMobile) {
					swal('info', 'Please Enter Secretariat Mobile Number', 'info');
					return false;
				}
				if (!IndNum.test(scope.UCSecMobile)) {
					swal('info', 'Please Enter Valid Secretariat Mobile Number', 'info');
					return false;
				}
				if (!scope.UCSIMSerialNo) {
					swal('info', 'Please Enter SIM Serial No', 'info');
					return false;
				}
			}
			if (!scope.UCrbnFPScanner) {
				swal('info', 'Please Check,Whether Secteratait Received Finger Print Scanner?', 'info');
				return false;
			}
			if (scope.UCrbnFPScanner == '1') {
				if (!scope.UCSelFPS) {
					swal('info', 'Please Select FPS', 'info');
					return false;
				}
				if (!scope.UCFPSModelNum) {
					swal('info', 'Please Enter FPS Model Number', 'info');
					return false;
				}
				if (!scope.UCFPSSerialNum) {
					swal('info', 'Please FPS Serial Number', 'info');
					return false;
				}
			}
			if (!scope.UCrbnPrinter) {
				swal('info', 'Please Check,Whether Secteratait Received Printer cum Scanner?', 'info');
				return false;
			}
			if (scope.UCrbnPrinter == '1') {
				if (!scope.UCSelPrinterMake) {
					swal('info', 'Please Select Printer cum Scanner', 'info');
					return false;
				}
				if (!scope.UCPrinterModelNum) {
					swal('info', 'Please Enter Printer cum Scanner Model Number', 'info');
					return false;
				}
				if (!scope.UCPrinterSerialNum) {
					swal('info', 'Please Printer cum  Serial Number', 'info');
					return false;
				}
			}
			if (!scope.UCSelAvailbleDesktop) {
				swal('info', 'Select Desktop Computers available ', 'info');
				return false;
			}
			if (scope.UCSelAvailbleDesktop == '1' || scope.UCSelAvailbleDesktop == '2') {
				if (!scope.UCSelDesktoponeMake) {
					swal('info', 'Please Select Desktop Computer - 1 Make', 'info');
					return false;
				}
				if (!scope.UCDesktopSerialNumone) {
					swal('info', 'Please Enter Desktop Computer - 1 Serial Number', 'info');
					return false;
				}
				if (!scope.UCSelCPUoneMake) {
					swal('info', 'Please Select CPU - 1 Make', 'info');
					return false;
				}
				if (!scope.UCCPUSerialNumone) {
					swal('info', 'Please Enter CPU - 1 Serial Number', 'info');
					return false;
				}
				if (!scope.UCSelMouseoneMake) {
					swal('info', 'Please Select Mouse - 1 Make', 'info');
					return false;
				}
				if (!scope.UCMouseSerialNumone) {
					swal('info', 'Please Enter Mouse - 1 Serial Number', 'info');
					return false;
				}
				if (!scope.UCSelKeyboardoneMake) {
					swal('info', 'Please Select Keyboard - 1 Make', 'info');
					return false;
				}
				if (!scope.UCKeyboardSerialNumone) {
					swal('info', 'Please Enter keyboard - 1 Serial Number', 'info');
					return false;
				}
			}

			if (scope.UCSelAvailbleDesktop == '2') {
				if (!scope.UCSelDesktoptwoMake) {
					swal('info', 'Please Select Desktop Computer - 2 Make', 'info');
					return false;
				}
				if (!scope.UCDesktopSerialNumtwo) {
					swal('info', 'Please Enter Desktop Computer - 2 Serial Number', 'info');
					return false;
				}
				if (!scope.UCSelCPUtwoMake) {
					swal('info', 'Please Select CPU - 2 Make', 'info');
					return false;
				}
				if (!scope.UCCPUSerialNumtwo) {
					swal('info', 'Please Enter CPU - 2 Serial Number', 'info');
					return false;
				}
				if (!scope.UCSelMousetwoMake) {
					swal('info', 'Please Select Mouse - 2 Make', 'info');
					return false;
				}
				if (!scope.UCMouseSerialNumtwo) {
					swal('info', 'Please Enter Mouse - 2 Serial Number', 'info');
					return false;
				}
				if (!scope.UCSelKeyboardtwoMake) {
					swal('info', 'Please Select Keyboard - 2 Make', 'info');
					return false;
				}
				if (!scope.UCKeyboardSerialNumtwo) {
					swal('info', 'Please Enter keyboard - 2 Serial Number', 'info');
					return false;
				}
			}

			if (!scope.UCSelAvailbleUPS) {
				swal('info', 'Please select How Many UPS available at Secretariat', 'info');
				return false;
			}
			if (scope.UCSelAvailbleUPS == '1' || scope.UCSelAvailbleUPS == '2') {
				if (!scope.UCUPSMakeone) {
					swal('info', 'Please Enter UPS-1 Make', 'info');
					return false;
				}
				if (!scope.UCUPSSerialnumone) {
					swal('info', 'Please Enter UPS-1 Serial Number', 'info');
					return false;
				}

			}
			if (scope.UCSelAvailbleUPS == '2') {
				if (!scope.UCUPSMaketwo) {
					swal('info', 'Please Enter UPS-2 Make', 'info');
					return false;
				}
				if (!scope.UCUPSSerialnumtwo) {
					swal('info', 'Please Enter UPS-2 Serial Number', 'info');
					return false;
				}

			}

			if (!scope.UCrbnRecievMahilaSmartPhone) {
				swal('info', 'Please Whether Mahila Police received Official Smart Phone', 'info');
				return false;
			}
			if (scope.UCrbnRecievMahilaSmartPhone == '1') {

				if (!scope.UCSelMahilaSmartPhMake) {
					swal('info', 'Please Select  Smart Phone Make(Mahila Police)', 'info');
					return false;
				}
				if (scope.UCSelMahilaSmartPhMake == 'Other') {
					if (!scope.UCOtherMahilaSmartphmake) {
						swal('info', 'Please  Enter Other  Smart Phone Make(Mahila Police)', 'info');
						return false;
					}
					else { }
				}
				if (!scope.UCMahilaIMEISIMOne) {
					swal('info', 'IMEI(SIM Slot 1) Number Mahila Police', 'info');
					return false;
				}
				if (!scope.UCMahilaIMEISIMTwo) {
					swal('info', 'IMEI(SIM Slot 2) Number Mahila Police', 'info');
					return false;
				}
				else { }
			}
			if (!scope.UCrbnRecievMahilaMobileNum) {
				swal('info', 'Please Check,Whether Secteratait received Official Mobile Number Mahila Police?', 'info');
				return false;
			}
			if (scope.UCrbnRecievMahilaMobileNum == '1') {
				if (!scope.UCSelMahilaMobileOperator) {
					swal('info', 'Please Select Mobile Operator(Mahila Police)', 'info');
					return false;
				}
				if (!scope.UCMahilaPoliceMobile) {
					swal('info', 'Please Enter Mahila Police Mobile Number', 'info');
					return false;
				}
				if (!IndNum.test(scope.UCMahilaPoliceMobile)) {
					swal('info', 'Please Enter Valid Mahila Police Mobile Number', 'info');
					return false;
				}
				if (!scope.UCMahilaSIMSerialNo) {
					swal('info', 'Please Enter Mahila Police SIM Serial No', 'info');
					return false;
				}
			}
			if (!scope.UCVolSmartpohe) {
				swal('info', 'Please Enter Number of smart phones', 'info');
				return false;
			}
			if (!scope.UCVolSIMCard) {
				swal('info', 'Please Enter Number of SIM Cards', 'info');
				return false;
			}
			if (!scope.UCselinternetconnectvity) {
				swal('info', 'Please Select Internet Connectvity', 'info');
				return false;
			}
			if (!scope.UCmdminstall) {
				swal('info', 'Please Enter No of MDM Install ', 'info');
				return false;
			}

			if (!scope.UCnoofArogyasethuapp) {
				swal('info', 'Please Enter no of Volunteer install Arogyasethu app', 'info');
				return false;
			}
			if (!scope.UCHighsecuritystationery) {
				swal('info', 'Please Enter No of high security stationery Received', 'info');
				return false;
			}
			if (!scope.UCHighstationerystockAvailable) {
				swal('info', 'Please Enter No of High Security stationery stock available', 'info');
				return false;
			}
			if (!scope.UCseleligibility) {
				swal('info', 'Please Select Eligibility Criteria Posters Displayed ', 'info');
				return false;
			}
			if (scope.UCseleligibility == '1') {
				if (!scope.eligibiltyimg) {
					swal('info', 'Please Upload eligible criteria Image ', 'info');
					return false;
				}
			}
			if (!scope.UCselbeneficiarylist) {
				swal('info', 'Please Select Beneficiary list Displayed ', 'info');
				return false;
			}
			if (scope.UCselbeneficiarylist == '1') {
				if (!scope.beneficieryimg) {
					swal('info', 'Please Upload Beneficiary list Image ', 'info');
					return false;
				}
			}
			
			if (!scope.UCSecTables) {
				swal('info', 'Please Enter Number of Tables', 'info');
				return false;
			}
			if (!scope.UCSecplasticchairs) {
				swal('info', 'Please Enter Number of Plastic Chairs', 'info');
				return false;
			}
			if (!scope.UCSecStypechairs) {
				swal('info', 'Please Enter Number of S type Chairs', 'info');
				return false;
			}
			if (!scope.UCSecnoticeboard) {
				swal('info', 'Please Enter Number of notice board', 'info');
				return false;
			}
			if (!scope.UCSecironsafes) {
				swal('info', 'Please Enter Number of Iron Safes', 'info');
				return false;
			}
			if (!scope.UCSecIronracks) {
				swal('info', 'Please Enter Number of Iron racks', 'info');
				return false;
			}
			if (!scope.UCVoltrafirstphase) {
				swal('info', 'Please Enter Number of Volunteer Trained Phase one', 'info');
				return false;
			}
			if (!scope.UCVoltrasecondphase) {
				swal('info', 'Please Enter Number of Volunteer Trained Phase two', 'info');
				return false;
			}
			if (!scope.UCFunctionariesphase) {
				swal('info', 'Please Enter Number of Functionaries Trained phase one and phase 2', 'info');
				return false;
			}
			if (!scope.UCVolVacant) {
				swal('info', 'Please Enter Number of Volunteer Post Vacant', 'info');
				return false;
			}

			if (!scope.UCrbnsecBuildingPaint) {
				swal('info', 'Please check Whether Building Paints', 'info');
				return false;
			}

			if (!scope.UCrbnsecnameboard) {
				swal('info', 'Please check Whether Name Board', 'info');
				return false;
			}
			if (!scope.UCrbnseccelectrification) {
				swal('info', 'Please check Whether Electrification?', 'info');
				return false;
			}

			if (!scope.UCrbnseccDrinkingWater) {
				swal('info', 'Please check Whether Drinking Water Facility?', 'info');
				return false;
			}
			if (!scope.UCrbnsecctoilets) {
				swal('info', 'Please check Whether Toilets Facility', 'info');
				return false;
			}

			if (!scope.UCrbnseccstationary) {
				swal('info', 'Please check Stationary Availability', 'info');
				return false;
			}
			if (!scope.UCrbnseccspandana) {
				swal('info', 'Please check Whether Spandana Counter?', 'info');
				return false;
			}

			if (!scope.UCrbnseccbilling) {
				swal('info', 'Please check  Billing Counter', 'info');
				return false;
			}
			if (!scope.UCrbnMcassActive) {
				swal('info', 'Please check Whether Mcass activcated', 'info');
				return false;
			}
			if (scope.UCrbnMcassActive == '1') {
				if (!scope.UCMcassUserId) {
					swal('info', 'Please Enter Volu MCass User ID', 'info');
					return false;
				}
				else {
					return true;
				}
			}
			else {
				return true;
			}
		}
	}

	angular.module("GSWS")
		.factory("entityService", ["akFileUploaderService", function (akFileUploaderService) {
			var saveTutorial = function (tutorial, url) {
				return akFileUploaderService.saveModel(tutorial, "/GSWS" + url);
				//return akFileUploaderService.saveModel(tutorial,"/GRAMAVAPP"+url);
				//return akFileUploaderService.saveModel(tutorial, "/Rakshana/POSTData");
			};
			return {
				saveTutorial: saveTutorial
			};
		}]);

	angular.module("akFileUploader", [])
		.factory("akFileUploaderService", ["$q", "$http",
			function ($q, $http) {
				var getModelAsFormData = function (data) {
					var dataAsFormData = new FormData();
					angular.forEach(data, function (value, key) {
						dataAsFormData.append(key, value);
					});
					return dataAsFormData;
				};

				var saveModel = function (data, url) {
					var deferred = $q.defer();
					$http({
						url: url,
						method: "POST",
						data: getModelAsFormData(data),
						transformRequest: angular.identity,
						headers: { 'Content-Type': undefined }
					}).then(function (result) {

						deferred.resolve(result);
					}, function (result, status) {
						deferred.reject(status);
					});
					return deferred.promise;
				};

				return {
					saveModel: saveModel
				}

			}]).directive("akFileModel", ["$parse",
				function ($parse) {
					return {
						restrict: "A",
						link: function (scope, element, attrs) {
							var model = $parse(attrs.akFileModel);
							var modelSetter = model.assign;
							element.bind("change", function () {
								scope.$apply(function () {
									modelSetter(scope, element[0].files[0]);
								});
							});
						}
					};
				}]);
})();
