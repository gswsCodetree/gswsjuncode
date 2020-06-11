(function () {

	var umempApp = angular.module('MyApp', ["akFileUploader"]);
	umempApp.controller('UnEMPRegistrationCtrl', ['$scope', 'datacontext', '$interval', 'entityService', UnEMPRegistrationCtrl]);
	function UnEMPRegistrationCtrl(scope, datacontext, interval, entityService) {
		scope.failurecount = 0;
		scope.lisendotp = true;
		scope.disableaadhaar = false;
		scope.disableaadhaarbtn = false;
		scope.checkdeclarationataadhaar = true;
		scope.disablemobilenum = true;
		scope.editaddressdetails = false;
		scope.rdcnf = false;
		scope.agreedisagree = false;
		scope.result9 = '1';
		uid_mask("#mainuid");
		scope.applicationsubmission = false;
		scope.dicanothermatch = false;


		//scope.agrrechange = function () {
		//	if (scope.agreeuniinfo) {
		//		swal("Failure", "Please check the checkbox", 'error');
		//	}
		//}


		//agree university data 
		scope.agreeuinversityinfo = function () {
			if (scope.Imageurl == "" || scope.Imageurl == undefined) {
				swal("info", "Please Upload Certificate", 'error');
				return;
			}
			var url = "/API/UnEmployee/SeedregisterAadhaarDetails"
			scope.umempuid = chk_uid();
			if (scope.halltilnum == null || scope.halltilnum == "" || scope.halltilnum == undefined) {
				swal("Failure", "Please Enter Hallticket Number", 'error');
				scope.applicationsubmission = true;
			}
			else {
				var val = { Aadhaar: scope.umempuid, hallticktno: scope.halltilnum }
				datacontext.post(url, val, function (data) {
					var resdis = data.data;
					if (resdis.Status == "Success") {
						Geteligibledata();
						scope.applicationsubmission = true;
						swal("Success", "Data Updated Successfully", 'success');
					}
					else {
						swal("Failure", resdis.Reason, 'error');
						scope.applicationsubmission = true;
					}
				});
			}
		}

		//Get District Codes and Names]
		function district() {
			var disurl = "/API/UnEmployee/GetDistrictDetails"
			datacontext.getById(disurl, "1", function (data) {
				var resdis = data.data;
				if (resdis.Status == "Success") {
					scope.districtsarray = resdis.DistMaster;
				}
				else {
					swal(res.Reason);
				}
			});
		}

		function mandal(district) {
			var manurl = "/API/UnEmployee/GetMandalDetails"

			var manval = district + "/" + scope.pssruralurbanflag
			datacontext.getById(manurl, manval, function (data) {
				var resman = data.data;
				if (resman.Status == "Success") {
					scope.mandalsarray = resman.MandalMaster;
				}
				else {
					swal(res.Reason);
				}
			});
		}

		function village(district, mandal) {
			var vilurl = "/API/UnEmployee/GetvillageDetails"

			var vilval = district + "/" + mandal + "/" + scope.pssruralurbanflag
			datacontext.getById(vilurl, vilval, function (data) {
				var resvil = data.data;
				if (resvil.Status == "Success") {
					scope.villagesarray = resvil.VTMaster;
				}
				else {
					swal(res.Reason);
				}
			});
		}


		//check declaration click
		scope.clickdeclarationataadhaar = function () {
			if (scope.checkdeclarationataadhaar == true) {
				scope.umempuid = chk_uid();
				var uid = scope.umempuid;
				if (uid == undefined || uid == null || uid.length < 12) {

					swal("Failure", "Please Enter 12 Digit Aadhaar Number", "Error");
					scope.disableaadhaar = false;
					scope.disableaadhaarbtn = false;
					return;
				}

				scope.disableaadhaar = true;
				scope.checkdeclarationataadhaar = false;
				scope.disableaadhaarbtn = true;

			}
			else {
				swal("Please Check the checkbox");
			}
		}

		scope.resenddesable = true; var promise;
		var click = 0;
		var increaseCounter = function () {
			if (scope.counter > 0) {
				scope.counter = scope.counter - 1;
				if (scope.counter == 0) { scope.resenddesable = false; interval.cancel(promise); }// scope.btnresend = true;
			}
		}



		//send otp
		scope.SendOTP = function (value) {
			scope.disableaadhaarbtn = false;
			scope.umempuid = chk_uid();
			scope.counter = 120;

			var uid = scope.umempuid;
			if (uid == undefined || uid == null || uid.length < 12) {

				swal("Failure", "Please Enter 12 Digit Aadhaar Number", "Error");
				scope.disableaadhaarbtn = true;
				return;
			}
			else {

				var status = validateVerhoeff(uid);
				if (!status) {
					scope.disableaadhaar = false;
					scope.checkdeclarationataadhaar = true;
					return status
				}

				var url = "/API/UnEmployee/SendOTP";			// var cpid = "CPK2004003712";

				datacontext.getByLogin(url, uid, function (data) {
					var res = data.data;
					if (res.Status == "Success") {

						if (value == 2) { click++; scope.resenddesable = true; } else { click = 1; scope.counter = 60; }
						if (value == 2) {

							if (click == 2) {
								scope.counter = 120;
							}
							if (click == 3) {
								scope.counter = 180;
							}
							if (click == 4) {
								scope.counter = 240;
							}
							if (click == 5) {
								scope.counter = 300;
							}
							if (click == 6) {
								scope.counter = 360;
							}
							if (click == 7) {
								scope.counter = 420;
							}
							if (click == 8) {
								scope.counter = 480;
							}
							if (click == 9) {
								scope.counter = 540;
							}
							if (click == 10) {
								scope.counter = 600;
							}
							if (click > 10) {
								window.location = "index.html";
							}
						}

						promise = interval(increaseCounter, 1000);

						scope.showsecondsmsg = true;
						scope.disableaadhaar = true;
						scope.checkdeclarationataadhaar = false;
						scope.liotp = true;
						//scope.lisendotp = false;
						sessionStorage.setItem("TokenId", res.token_id);
						swal('success', res.Reason, 'success');
					}

					else {
						scope.liotp = false;
						scope.lisendotp = true;
						scope.disableaadhaarbtn = false;
						swal("Failure", res.Reason, "error");
					}
				}, function (error) {
					alert(error);
				});
			}

		}

		scope.VerifyOTP = function () {

			//var uid = scope.umempuid;
			var otpval = scope.pwdOTP;

			if (otpval == undefined || otpval == null || otpval.length < 6) {
				swal('Failure', 'Please Enter 6 Digit OTP Number');
				return;
			}

			else {
				district();
				var url = "/API/UnEmployee/VerifyOTP";
				var input = scope.umempuid + "/" + scope.pwdOTP;
				datacontext.getById(url, input, function (data) {
					var res = data.data;
					if (res.Status == "Success") {
						//scope.lisendotp = false;
						scope.showsecondsmsg = false;
						scope.liotp = false;
						scope.disableaadhaarbtn = false;
						scope.lipersonal = true;
						scope.showresendotp = false;

						//scope.applicatename = res.Name;
						//scope.gender = res.Gender;
						//scope.imagebase64 = res.strbase64img;
						//scope.dob = res.Dateofbirth;
						//scope.villagename = res.villagename;
						//scope.mandalname = res.Mandalname;
						//scope.districtname = res.districtname;
						//scope.pincode = res.pincode;


						sessionStorage.setItem("username", res.pssName);
						sessionStorage.setItem("strimg", res.strbase64img);
						sessionStorage.setItem("userid", scope.umempuid);
						scope.applicatename = res.pssName;
						scope.gender = res.pssGender;
						scope.imagebase64 = res.strbase64img;
						scope.dob = res.pssDateofbirth;
						scope.villagename = res.pssvillagename;
						scope.mandalname = res.pssMandalname;
						scope.districtname = res.pssdistrictname;
						scope.pincode = res.psspincode;
						scope.selecteddistrict = res.pssdistrictid;
						mandal(scope.selecteddistrict);
						scope.selectedmandal = res.pssmandalid;
						village(scope.selecteddistrict, scope.selectedmandal);
						scope.selectedvillage = res.pssvillagecode;
						scope.doornumber = res.pssDoorno;
						scope.streetname = res.pssStreetname;

						scope.pssapplicatename = res.pssName;
						scope.pssgender = res.pssGender;
						scope.pssdob = res.pssDateofbirth;
						scope.pssdistrictname = res.pssdistrictname;
						scope.pssruralurbanflag = res.pssruralurbanflag;
						sessionStorage.setItem("ruflag", res.pssruralurbanflag);
						mandal(scope.selecteddistrict);
						scope.pssmandalname = res.pssMandalname;
						village(scope.selecteddistrict, scope.selectedmandal);
						scope.pssvillagename = res.pssvillagename;
						scope.psspincode = res.psspincode;
						scope.pssdoornumber = res.pssDoorno;
						scope.pssstreetname = res.pssStreetname;
						scope.mobilenum = res.pssmobilenumber;

						if (res.Unempstatus == "01") {
							window.location = "StudentProfile.html";
						}
						if (res.Unempstatus == "02") {
							window.location = "GrievanceDetails.html";
						}
					}

					else {
						//scope.lisendotp = false;
						scope.liotp = true;
						scope.lipersonal = false;
						swal("Failure", res.Reason, "error");
					}
				}, function (error) {
					alert(error);
				});

			}


		}

		scope.mandal = function (distcode) {
			scope.selectedmandal = "";
			scope.selectedvillage = "";
			scope.mandalsarray = "";
			mandal(distcode);
		}

		scope.village = function (distcode, mandal) {
			scope.selectedvillage = "";
			scope.villagesarray = "";
			village(distcode, mandal);

		}

		scope.GetEligibleData = function () {

			var IndNum = /^[0]?[6789]\d{9}$/;
			if (scope.mobilenum == undefined || scope.mobilenum == null || scope.mobilenum.length < 10) {

				swal("Failure!", "Please Enter 10 digit Mobile Number", "error");
				return;
			}
			if (IndNum.test(scope.mobilenum)) {

			} else {
				swal("Failure!", "Invalid Mobile Number", "error");
				return; 7
			}

			Geteligibledata();
			$("html, body").animate({ scrollTop: 1800 }, 1000);
		}

		function Geteligibledata() {
			var url = "/API/UnEmployee/GetEligibleWeb";
			var input = scope.umempuid;
			var education = "";
			datacontext.getById(url, input, function (data) {
				var res = data.data;
				if (res.Status == "Success") {
					scope.liotp = false;

					scope.lieligible = true;
					scope.lieducation = true;
					scope.lieducationmapped = true;
					if (res.Unempstatus == "02") {

						scope.libankdetails = false;
						scope.divgrievance = true;
						scope.Checkedeligiblelist = res.checkEligiblelist;
						//education = res.checkEligiblelist[6].STATUS;
					}
					else {
						scope.divgrievance = false;
						scope.libankdetails = true;
						scope.Checkedeligiblelist = res.checkEligiblelist;
						
					}
					var failurecount = 0;

					for (var index = 0; index < res.checkEligiblelist.length; index++) {

						if (res.checkEligiblelist[index].CODE == "201514877") {
							education = res.checkEligiblelist[index].STATUS;
							if (education == "Eligible") {
								scope.educationmapped = false;
								scope.divstudetails = true;
							}
							else {
								scope.educationmapped = true;
								scope.divstudetails = false;
							}
						
						}
						if (res.checkEligiblelist[index].STATUS != 'Eligible') {
							failurecount++;
						}
					}

					if (failurecount == 0) {

						scope.finalstatus = 'success';
						scope.agreedisagree = false;
					} else {
						scope.finalstatus = 'not success';
						scope.agreedisagree = true;
					}

					if (education == "Eligible") {
						var url = "/API/UnEmployee/GetMappedEducationDetails";
						var input = scope.umempuid;
						datacontext.getById(url, input, function (data) {
							debugger;
							var resedu = data.data;
							if (resedu.Status == "Success") {
								scope.lieducationmapped = false;

								scope.divstudetails = true;
								scope.divnotstu = false;
								scope.stuname = resedu.Studentdetails[0].STUDENT_NAME;
								scope.stuunversity = resedu.Studentdetails[0].UNIVERSITY;
								scope.stuyear = resedu.Studentdetails[0].YEAR;
								scope.stucourse = resedu.Studentdetails[0].COURSE;
								scope.stufathername = resedu.Studentdetails[0].FATHER_NAME;
								scope.halltilnum = resedu.Studentdetails[0].HALLTICKET_NO;
								scope.namecomerror = '';
							}
							else {
								scope.educationmapped = true;
								scope.lieducationmapped = false;
							}

						}, function (error) {
							alert(error);
						});

						//education Details

					}

				}

				else {
					//scope.lisendotp = false;
					scope.liotp = false;
					scope.lipersonal = true;
					scope.lieligible = false;
					scope.libankdetails = false;
					swal("Failure", res.Reason, "error");
				}
			}, function (error) {
				alert(error);
			});

		}

		scope.checkeditadress = function () {
			if (scope.chckeditdetails) {
				scope.editaddressdetails = true;
			}
			else {
				scope.editaddressdetails = false;
			}
		}

		//Send mobile otp for mobile verification
		scope.sendmobileotp = function () {
			if (scope.mobilenumpopup == "" || scope.mobilenumpopup == undefined || scope.mobilenumpopup == null) {
				swal("Please Enter Mobile Number");
				return;
			}
			if (scope.mobilenumpopup.length < 10) {
				swal("Mobile number should be 10 digits ");
				return;
			}
			if (scope.mobilenumpopup == "1111111111" || scope.mobilenumpopup == "2222222222" || scope.mobilenumpopup == "3333333333" ||
				scope.mobilenumpopup == "4444444444" || scope.mobilenumpopup == "5555555555" || scope.mobilenumpopup == "6666666666" ||
				scope.mobilenumpopup == "7777777777" || scope.mobilenumpopup == "8888888888" || scope.mobilenumpopup == "9999999999") {
				swal("Please enter valid mobile number");
				return;
			}
			else {
				var mobileverifyotpurl = "/API/UnEmployee/SendMobileOTP"

				var mobileverifyotpval = scope.mobilenumpopup;
				datacontext.getById(mobileverifyotpurl, mobileverifyotpval, function (data) {
					var res = data.data;
					if (res.Status == "Success") {
						scope.sentmobileotp = res.OTP;
						swal("OTP will be sent to entered mobile number");
					}
					else {
						scope.sentmobileotp = "123";
						swal(res.Reason);
					}

				});
			}

		}

		//Check Otp Verification
		scope.verifymobileotp = function () {
			if (scope.mobilenumpopup == "" || scope.mobilenumpopup == undefined || scope.mobilenumpopup == null) {
				swal("Please Enter Mobile Number");
				return;
			}
			if (scope.mobileverifyotp == "" || scope.mobileverifyotp == undefined) {
				swal("Please enter OTP sent to your mobile number");
				return;
			}
			else {
				if (scope.mobileverifyotp == scope.sentmobileotp) {

					var mobileverifyotpurl = "/API/UnEmployee/UpdateMobileNumber";
					var input = scope.umempuid + "/" + scope.mobilenumpopup;
					//var mobileverifyotpval = 
					datacontext.getById(mobileverifyotpurl, input, function (data) {
						var res = data.data;
						if (res.Status == "Success") {
							scope.mobilenumpopup = '';
							scope.mobileverifyotp = '';
							swal({
								title: "success",
								text: "Your Mobile Number changed Successfully",
								type: "success"
							},
								function () {

									$('#mobilemodalpopup').modal('hide');
								});
						}
						else {
							//scope.sentmobileotp = "123";
							swal(res.Reason);
						}

					});

					

					//swal("Your Mobile Number changed Successfully");
					scope.mobilenum = scope.mobilenumpopup;


					//$('.mobilemodalpopup').css('display', 'none');
				}
				else {
					swal("You Have Entered Wrong OTP");
				}
			}
		}

		scope.optout = function () {
			swal({
				title: "Are you sure?",
				text: "Do You Want to Optout",
				type: "warning",
				showCancelButton: true,
				confirmButtonColor: "#DD6B55", confirmButtonText: "Yes",
				cancelButtonText: "No",
				closeOnConfirm: false,
				closeOnCancel: false
			},
				function (isConfirm) {
					if (isConfirm) {
						swal("Success!", "Thank You," + '\n' + "We really appreciate your action of give it up.It truly demonstrates your care and concern and surely motivate other beneficiaries.", 'success');
						Saveunemployeefinalsuccess(4);
					} else {
						swal("Cancelled", "You have cancelled from opting out this scheme.", "error");
					}
				});
		}


		scope.validateIfsc = function () {

			var url = "/API/UnEmployee/GetBankIFSC";
			var input = scope.bankifsc;
			datacontext.getById(url, input, function (data) {
				var res = data.data;
				if (res.Status == "Success") {

					scope.len = res.acclength;
					scope.bankname = res.bankname;
					scope.bankbranch = res.branch;

				}

				else {

					swal("Failure", res.Reason, "error");
				}
			}, function (error) {
				alert(error);
			});
		}

		//Agree aboce mentioned details are true
		scope.GetChangesurvey = function (value) {
			if (value == 1) {
				scope.savewithougrievancereg = true;
				scope.savegrievancereg = false;
			}
			else {
				scope.savewithougrievancereg = false;
				scope.savegrievancereg = true;
			}
		}


		scope.SaveUnEmployeeData = function (value) {

			Saveunemployeefinalsuccess(value);

		}

		function Saveunemployeefinalsuccess(value) {
			if (scope.halltilnum == undefined || scope.halltilnum == null || scope.halltilnum == '') {
				swal('Failure', 'Please validate Education Details', 'error');
				return;

			}

			if (scope.stuname == undefined || scope.stuname == null || scope.stuname == '') {
				swal('Failure', 'Please validate Education Details', 'error');
				return;
			}

			if (scope.stuunversity == undefined || scope.stuunversity == null || scope.stuunversity == '') {
				swal('Failure', 'Please validate Education Details', 'error');
				return;
			}
			//if (scope.Bankaccnum == undefined || scope.Bankaccnum == null) {
			//	swal('Failure', 'Please enter Bank Account Number', 'error');
			//	return;

			//}

			//if (scope.Bankconfirmaccnum == undefined || scope.Bankconfirmaccnum == null) {
			//	swal('Failure', 'Please enter Bank confirm Account Number', 'error');
			//	return;
			//}

			//if (scope.Bankaccnum!= scope.Bankconfirmaccnum) {
			//	// alert("Bank Account Number and  Confirm Account Number  are MisMatch");
			//	swal("Failure!", "Bank Account Number and  Confirm Account Number  are MisMatch", "error");
			//	 return;
			//}

			//var response = { UID: scope.umempuid, mobilenum: scope.mobilenum, emailid: scope.emailid, gender:scope.Gender, name: scope.applicatename, ifsccode: scope.bankifsc, bankacc: scope.Bankaccnum, confirmbankacc: scope.Bankconfirmaccnum, bankname: scope.bankname, bankbranch: scope.bankbranch };


			var response = { UID: scope.umempuid, mobilenum: scope.mobilenum, emailid: scope.emailid, gender: scope.Gender, name: scope.applicatename, ifsccode: null, bankacc: null, confirmbankacc: null, bankname: null, bankbranch: null, HallTicketnum: scope.halltilnum, CertificateName: scope.stuname, Course: scope.stucourse, YearofPass: scope.stuyear, University: scope.stuunversity, RequestType: 2, UnempStatus: 1, FATHER_NAME: scope.stufathername }; //1-mobile,2-web

			var url = "/API/UnEmployee/SaveUnEmployeeData";

			datacontext.post(url, response, function (data) {
				var res = data.data;
				if (res.Status == "Success") {

					//swal('Success', res.Reason + ":" + res.ApplicationID, 'error');
					//swal('info', res.Reason + ":" + res.ApplicationID, 'success');
					swal({
						title: "success",
						text: res.Reason + ":" + "Your Application RequestID is  :-" + res.ApplicationID,
						type: "success"
					});
					//swal("Your Grievance Submitted Successfully");
					//window.location.reload());
					interval(function () { window.location = "StudentProfile.html"; }, 7000);


					return;
				}

				else {

					swal("Failure", res.Reason, "error");
				}
			}, function (error) {
				alert(error);
			});
		}

		scope.submitgrievance = function () {

			if (scope.umempuid == undefined || scope.umempuid == null) {
				swal("Please Enter AdharNumber");
				return;
			}

			//if (scope.district == undefined || scope.district == null) {
			//	swal("Please Select District Name");
			//	return;
			//}
			//if (scope.mandal == undefined || scope.mandal == null) {
			//	swal("Please Select Mandal Name");
			//	return;
			//}
			//if (scope.village == undefined || scope.village == null) {
			//	swal("Please Select Village Name");
			//	return;
			//}
			scope.grievancelist = [];
			//if (scope.textareafeedback == undefined || scope.textareafeedback == null) {

			//	swal("Please Enter Remarks about the grievance");
			//	return;
			//}
			var IndNum = /^[0]?[6789]\d{9}$/;
			if (scope.mobilenum == undefined || scope.mobilenum == null || scope.mobilenum.length < 10) {

				swal("Failure!", "Please Enter 10 digit Mobile Number", "error");
				return;
			}
			if (IndNum.test(scope.mobilenum)) {

			} else {
				swal("Failure!", "Invalid Mobile Number", "error");
				return;
			}
			if (scope.gender == undefined || scope.gender == null) {
				swal("Please Select Gender");
				return;
			}
			if (scope.applicatename == undefined || scope.applicatename == null) {
				swal("Please Enter Name");
				return;
			}

			var codes = '';
			var gricount = 0;
			for (var index = 0; index < scope.Checkedeligiblelist.length; index++) {

				if (scope.Checkedeligiblelist[index].STATUS != 'Eligible') {
					//failurecount++;
					codes += scope.Checkedeligiblelist[index].CODE + ",";

					//Ration
					if (scope.Checkedeligiblelist[index].CODE == "201514876") {

						if (scope.chkrationgri) {
							scope.rationgristatus = 1;
							gricount++;
							if (scope.Rationmum == "" || scope.Rationmum == undefined || scope.Rationmum == null) {

								swal('info', 'Please Enter Ration Number', 'error');
								return;
							}
						}
						else {
							scope.rationgristatus = 0;
						}
						var val = { Deptid: scope.rationdepartId, SubSbujectcode: '201514876', Otherproblem: 'RationId:' + scope.Rationmum + ",RationAttachmentfile:122,REMARKS:" + scope.textarearationfeedback, GrievanceStatus: scope.rationgristatus };
						scope.grievancelist.push(val);

					}
					//dob age
					if (scope.Checkedeligiblelist[index].CODE == "201514883") {
						if (scope.chkagegri) {
							scope.agegristatus = 1;
							gricount++;
							if (scope.dobcertificte == "" || scope.dobcertificte == undefined || scope.dobcertificte == null) {

								swal('info', 'Please Enter Dob Certficate Number', 'error');
								return;
							}
						}
						else {
							scope.agegristatus = 0;
						}

						var val2 = { Deptid: scope.agedepartId, SubSbujectcode: '201514883', Otherproblem: 'DobCertificatenum:' + scope.dobcertificte + ',DOBAttachmentfile:122,REMARKS:' + scope.textareavehiclefeedback, GrievanceStatus: scope.agegristatus };
						scope.grievancelist.push(val2);
					}

					//education
					if (scope.Checkedeligiblelist[index].CODE == "201514877") {

						if (scope.chkeducationgri) {
							gricount++;
							if (scope.edurollno == "" || scope.edurollno == undefined || scope.edurollno == null) {

								swal('info', 'Please Enter Roll Number', 'error');
								return;
							}
							if (scope.eduhallticketno == "" || scope.eduhallticketno == undefined || scope.eduhallticketno == null) {

								swal('info', 'Please Enter Hall Ticket Number', 'error');
								return;
							}
							if (scope.eduyearpass == "" || scope.eduyearpass == undefined || scope.eduyearpass == null) {

								swal('info', 'Please Enter Year of Pass', 'error');
								return;
							}
							if (scope.selectedstate == "" || scope.selectedstate == undefined || scope.selectedstate == null) {

								swal('info', 'Please Select State', 'error');
								return;
							}

							if (scope.selectedunversity == "" || scope.selectedunversity == undefined || scope.selectedunversity == null) {

								swal('info', 'Please Select University', 'error');
								return;
							}

							if (scope.selectedcourse == "" || scope.selectedcourse == undefined || scope.selectedcourse == null) {

								swal('info', 'Please Select Course', 'error');
								return;
							}
							scope.edugristatus = 1;
						}
						else {
							scope.edugristatus = 0;
						}

						var val3 = { Deptid: scope.agedepartId, SubSbujectcode: '201514883', Otherproblem: 'Rollno:' + scope.edurollno + ',HallTikcetnum:' + scope.halltilnum + ',Course:' + scope.selectedcourse + ',Year:' + scope.eduyearpass + ',InstituteName:' + scope.selectedunversity + ',state:' + scope.selectedstate + ',REMARKS:' + scope.textareaedufeedback };
						scope.grievancelist.push(val3);
					}

					//employee 
					if (scope.Checkedeligiblelist[index].CODE == "201514880") {
						if (scope.chkempgri) {
							scope.empgristatus = 1;
							gricount++;
						}
						else {
							scope.empgristatus = 0;
						}
						var val4 = { Deptid: scope.empdepartId, SubSbujectcode: '201514880', Otherproblem: 'REMARKS:' + scope.textareaempfeedback, GrievanceStatus: scope.empgristatus };
						scope.grievancelist.push(val4);

					}
					//employee private employee

					if (scope.Checkedeligiblelist[index].CODE == "201514898") {
						if (scope.chkempgri) {
							scope.empprigristatus = 1;
							gricount++;
						}
						else {
							scope.empprigristatus = 0;
						}
						var val4 = { Deptid: scope.empdepartId, SubSbujectcode: '201514898', Otherproblem: 'REMARKS:' + scope.textareaempfeedback, GrievanceStatus: scope.empprigristatus };
						scope.grievancelist.push(val4);
					}

					//obmms 
					if (scope.Checkedeligiblelist[index].CODE == "201514879") {

						if (scope.chkobmsgri) {
							scope.obmmsgristatus = 1;
							gricount++;
						}
						else {
							scope.obmmsgristatus = 0;
						}
						var val5 = { Deptid: scope.obmmdepartId, SubSbujectcode: '201514879', Otherproblem: 'REMARKS:' + scope.textareaobmsfeedback, GrievanceStatus: scope.obmmsgristatus };
						scope.grievancelist.push(val5);
					}



					//land
					if (scope.Checkedeligiblelist[index].CODE == "201514882") {

						if (scope.chklandgri) {
							scope.landgristatus = 1;
							gricount++;
						}
						else {
							scope.landgristatus = 0;
						}

						var val5 = { Deptid: scope.landdepartId, SubSbujectcode: '201514882', Otherproblem: 'REMARKS:' + scope.textarealandfeedback, GrievanceStatus: scope.landgristatus };
						scope.grievancelist.push(val5);
					}

					//Vehicle
					if (scope.Checkedeligiblelist[index].CODE == "201514885") {
						if (scope.chkvehigri) {
							scope.vehgristatus = 1;
							gricount++;
						}
						else {
							scope.vehgristatus = 0;
						}
						var val5 = { Deptid: scope.vehicledepartId, SubSbujectcode: '201514885', Otherproblem: 'REMARKS:' + scope.textareavehiclefeedback, GrievanceStatus: scope.vehgristatus };
						scope.grievancelist.push(val5);
					}

					//Psscode
					if (scope.Checkedeligiblelist[index].CODE == "201514897") {
						if (scope.chkpsssurveygri) {
							scope.psssurveygristatus = 1;
							gricount++;
						}
						else {
							scope.psssurveygristatus = 0;
						}
						var val5 = { Deptid: scope.vehicledepartId, SubSbujectcode: '201514897', Otherproblem: 'REMARKS:' + scope.textareavehiclefeedback, GrievanceStatus: scope.psssurveygristatus };
						scope.grievancelist.push(val5);
					}
					//bio metric in pss
					if (scope.Checkedeligiblelist[index].CODE == "201514896") {
						if (scope.chkpssbiogri) {
							scope.pssbiogristatus = 1;
							gricount++;
						}
						else {
							scope.pssbiogristatus = 0;
						}
						var val5 = { Deptid: scope.vehicledepartId, SubSbujectcode: '201514896', Otherproblem: 'REMARKS:' + scope.textareavehiclefeedback, GrievanceStatus: scope.pssbiogristatus };
						scope.grievancelist.push(val5);
					}
					//scholarship
					if (scope.Checkedeligiblelist[index].CODE == "201514905" || scope.Checkedeligiblelist[index].CODE == "201514906" || scope.Checkedeligiblelist[index].CODE == "201514907" || scope.Checkedeligiblelist[index].CODE == "201514908") {
						if (scope.chkscholargri) {
							scope.schlrgristatus = 1;
							gricount++;
						}
						else {
							scope.schlrgristatus = 0;
						}
						var val5 = { Deptid: scope.vehicledepartId, SubSbujectcode: scope.Checkedeligiblelist[index].CODE, Otherproblem: 'REMARKS:' + scope.textareavehiclefeedback, GrievanceStatus: scope.schlrgristatus };
						scope.grievancelist.push(val5);
					}

					//pension
					if (scope.Checkedeligiblelist[index].CODE == "201514900") {
						if (scope.chkpensiongri) {
							scope.pensiongristatus = 1;
							gricount++;
						}
						else {
							scope.pensiongristatus = 0;
						}
						var val5 = { Deptid: scope.vehicledepartId, SubSbujectcode: '201514900', Otherproblem: 'REMARKS:' + scope.textareavehiclefeedback, GrievanceStatus: scope.pensiongristatus };
						scope.grievancelist.push(val5);
					}

				}
			}

			if (scope.Checkedeligiblelist.length != gricount) {

				swal('info', 'You have total of ' + scope.Checkedeligiblelist.length + ' grievances,You are raising only :' + gricount + 'among them');
			}
			codes = codes.slice(0, -1);
			if (codes == null || codes == undefined) {
				swal("Please check no eligible");
				return;
			}

			else {
				var response = { ADHAR_NO: scope.umempuid, MOBILE_NUMBER: scope.mobilenum, APPLICATENT_NAME: scope.applicatename, PROBLEM_DESC: scope.textareafeedback, grievancecode: codes, GENDER: scope.pssgender, pssVILLAGECODE: scope.selectedvillage, pssMANDAL: scope.selectedmandal, pssDISTRIC_ID: scope.selecteddistrict, RUFlag: scope.pssruralurbanflag, grievancelist: scope.grievancelist };
				var url = "/API/UnEmployee/SaveGrievanceDetails";
				datacontext.post(url, response, function (data) {
					var res = data.data;
					if (res.Status == "Success") {
						//swal('Success', res.Reason + ":" + res.ApplicationID, 'error');
						//swal('info', res.Reason, 'success');
						swal({
							title: "success",
							text: res.Reason,
							type: "success"
						});
						//swal("Your Grievance Submitted Successfully");
						//window.location.reload());
						interval(function () { window.location.reload(); }, 5000);

						return;
					}
					else {

						swal("Failure", res.Reason, "error");
					}
				}, function (error) {
					alert(error);
				});
				//swal("Your Grievance Submitted Successfully");
				//document.location.reload();
			}
		}


		scope.GetUniversity = function () {
			univeritylist();
		}

		scope.SavewithoutGrievanceRegistration = function () {

			var response = { UID: scope.umempuid, mobilenum: scope.mobilenum, emailid: scope.emailid, gender: scope.Gender, name: scope.applicatename, district: scope.selecteddistrict, rural_urban: scope.pssruralurbanflag, mandal: scope.selectedmandal, village: scope.selectedvillage, requesttype: '2' }; //1-mobile,2-web

			var url = "/API/UnEmployee/SavewithoutGrievanceRegistration"

			datacontext.post(url, response, function (data) {
				var res = data.data;
				if (res.Status == "Success") {
					swal({
						title: "success",
						text: "Thank You!",
						type: "success"
					});
					interval(function () { window.location.reload(); }, 5000);
					return;
				}

				else {

					swal("Failure", res.Reason, "error");
				}
			}, function (error) {
				alert(error);
			});

		}

		scope.GetEducationDetails = function () {

			EducationDetails();
		}
		//Modal pop code for grievance

		function EducationDetails() {
			var vilurl = "/API/UnEmployee/GetstudentData";

			var vilval = scope.halltilnum + "/" + scope.umempuid + "/" + scope.pssgender;
			datacontext.getById(vilurl, vilval, function (data) {
				var resedu = data.data;
				if (resedu.Status == "Success") {
					scope.divstudetails = true;
					scope.divnotstu = false;
					scope.dicanothermatch = false;
					scope.applicationsubmission = false;
					scope.stuname = resedu.Studentdetails[0].STUDENT_NAME;
					scope.stuunversity = resedu.Studentdetails[0].UNIVERSITY;
					scope.stuyear = resedu.Studentdetails[0].YEAR;
					scope.stucourse = resedu.Studentdetails[0].COURSE;
					scope.stufathername = resedu.Studentdetails[0].FATHER_NAME;
					scope.namecomerror = '';
					
				}
				else if (resedu.Status == "Success1") {
					scope.divstudetails = true;
					scope.divnotstu = false;
					scope.dicanothermatch = false;
					scope.stuname = resedu.Studentdetails[0].STUDENT_NAME;
					scope.stuunversity = resedu.Studentdetails[0].UNIVERSITY;
					scope.stuyear = resedu.Studentdetails[0].YEAR;
					scope.stucourse = resedu.Studentdetails[0].COURSE;
					scope.stufathername = resedu.Studentdetails[0].FATHER_NAME;

					scope.namecomerror = resedu.Reason;
				}
				else if (resedu.Status == "NotMatched") {
					scope.educationmapped = true;
					scope.divstudetails = false;
					scope.divnotstu = false;
					scope.dicanothermatch = true;
					scope.applicationsubmission = true;
				}
				else if (resedu.Status == "DemoAuthFailed") {
					scope.educationmapped = true;
					scope.divstudetails = true;
					scope.divnotstu = false;
					scope.dicanothermatch = false;
					scope.applicationsubmission = true;
					scope.stuname = resedu.Studentdetails[0].STUDENT_NAME;
					scope.stuunversity = resedu.Studentdetails[0].UNIVERSITY;
					scope.stuyear = resedu.Studentdetails[0].YEAR;
					scope.stucourse = resedu.Studentdetails[0].COURSE;
					scope.stufathername = resedu.Studentdetails[0].FATHER_NAME;
					swal("Failure", resedu.Reason, 'error');
				}
				else {
					
					scope.STUERROR = resedu.Reason;
					scope.divstudetails = false;
					scope.dicanothermatch = false;
					scope.divnotstu = true;
					scope.applicationsubmission = true;
					//Statecourselist();
				}
			});
		}

		function Statecourselist() {

			var stateurl = "/API/UnEmployee/GetUnivercityData";

			var val = "1/2";
			datacontext.getById(stateurl, val, function (data) {
				var resedu = data.data;
				if (resedu.Status == "Success") {
					scope.statelist = resedu.statelist
					scope.courslist = resedu.courslist;
				}
				else {
					swal(resedu.Reason);
				}
			});

		}

		function univeritylist() {

			var stateurl = "/API/UnEmployee/GetUnivercityData";

			var val = "2/" + scope.selectedstate;
			datacontext.getById(stateurl, val, function (data) {
				var resedu = data.data;
				if (resedu.Status == "Success") {
					scope.Universitylist = resedu.Universitylist;

				}
				else {
					swal(resedu.Reason);
				}
			});

		}
		function distroload() {
			var url = "/API/UnEmployee/GetMeekosamDistrict"

			datacontext.getById(url, "1", function (data) {
				var databind = data.data;
				if (databind.Status == "Success") {
					scope.distlist = databind.DistMaster;
				}
				else {
					alert("District Loading is Failed");
				}
			});
		}
		scope.loadmandal = function (disid) {

			var url = "/API/UnEmployee/GetMeekosamMandal"

			datacontext.getById(url, disid, function (data) {
				var databind = data.data;
				if (databind.Status == "Success") {
					scope.bindsmandal = databind.MandalMaster;
				}
				else {
					alert("erorr");
				}
			});
		}
		scope.loadvillage = function (mandid) {
			var val = scope.district + "/" + mandid;//  scope.mandalid = mandid;
			var url = "/API/UnEmployee/GetMeekosamVillage"

			datacontext.getById(url, val, function (data) {
				var databind = data.data;
				if (databind.Status == "Success") {
					scope.bindsvillage = databind.VTMaster;
				}
				else {
					alert("erorr");
				}
			});
		}

		scope.SaveGrievanceRegistration = function () {
			distroload();
			var codes = '';
			for (var index = 0; index < scope.Checkedeligiblelist.length; index++) {

				if (scope.Checkedeligiblelist[index].STATUS != 'Eligible') {
					//failurecount++;
					codes += scope.Checkedeligiblelist[index].CODE + ",";
				}
			}
			codes = codes.slice(0, -1);


			var url = "/API/UnEmployee/GetGrievanceDepartment";
			var input = scope.pssruralurbanflag+"?griid=" + codes;
			datacontext.getById(url, input, function (data) {
				var databind = data.data;
				if (databind.Status == "Success") {
					scope.Deptlist = databind.GrievanceDepartList;

					for (var i = 0; i < scope.Deptlist.length; i++) {

						if (scope.Deptlist[i].SUBSUBJECTCODE == "201514880") {

							scope.divemp = true;
							scope.empdepart = scope.Deptlist[i].ENAME;
							scope.empdepartId = scope.Deptlist[i].PETCODE;
							scope.empsubsubject = scope.Deptlist[i].SUBSUBJECT + "/" + scope.Deptlist[i].T_SUBSUBJECT;

						}
						if (scope.Deptlist[i].SUBSUBJECTCODE == "201514885") {

							scope.divvehicle = true;
							scope.vehicledepart = scope.Deptlist[i].ENAME;
							scope.vehicledepartId = scope.Deptlist[i].PETCODE;
							scope.vehiclesubsubject = scope.Deptlist[i].SUBSUBJECT + "/" + scope.Deptlist[i].T_SUBSUBJECT;

						}
						if (scope.Deptlist[i].SUBSUBJECTCODE == "201514879") {

							scope.divobmms = true;
							scope.obmmsdepart = scope.Deptlist[i].ENAME;
							scope.obmmdepartId = scope.Deptlist[i].PETCODE;
							scope.obmmssubsubject = scope.Deptlist[i].SUBSUBJECT + "/" + scope.Deptlist[i].T_SUBSUBJECT;

						}
						if (scope.Deptlist[i].SUBSUBJECTCODE == "201514883") {

							scope.divage = true;
							scope.agedepart = scope.Deptlist[i].ENAME;
							scope.agedepartId = scope.Deptlist[i].PETCODE;
							scope.agesubsubject = scope.Deptlist[i].SUBSUBJECT + "/" + scope.Deptlist[i].T_SUBSUBJECT;

						}

						if (scope.Deptlist[i].SUBSUBJECTCODE == "201514882") {

							scope.divland = true;
							scope.landdepart = scope.Deptlist[i].ENAME;
							scope.landdepartId = scope.Deptlist[i].PETCODE;
							scope.landsubsubject = scope.Deptlist[i].SUBSUBJECT + "/" + scope.Deptlist[i].T_SUBSUBJECT;

						}

						if (scope.Deptlist[i].SUBSUBJECTCODE == "201514877") {

							scope.divedu = true;
							scope.edudepart = scope.Deptlist[i].ENAME;
							scope.edudepartId = scope.Deptlist[i].PETCODE;
							scope.edusubsubject = scope.Deptlist[i].SUBSUBJECT + "/" + scope.Deptlist[i].T_SUBSUBJECT;

						}
						if (scope.Deptlist[i].SUBSUBJECTCODE == "201514876") {

							scope.divration = true;
							scope.rationdepart = scope.Deptlist[i].ENAME;
							scope.rationdepartId = scope.Deptlist[i].PETCODE;
							scope.rationsubsubject = scope.Deptlist[i].SUBSUBJECT + "/" + scope.Deptlist[i].T_SUBSUBJECT;

						}

						if (scope.Deptlist[i].SUBSUBJECTCODE == "201514897") {

							scope.divpsssurvey = true;
							scope.pssdepart = scope.Deptlist[i].ENAME;
							scope.pssdepartId = scope.Deptlist[i].PETCODE;
							scope.psssubsubject = scope.Deptlist[i].SUBSUBJECT + "/" + scope.Deptlist[i].T_SUBSUBJECT;

						}
						if (scope.Deptlist[i].SUBSUBJECTCODE == "201514896") {

							scope.divpssbio = true;
							scope.pssbiodepart = scope.Deptlist[i].ENAME;
							scope.pssbiodepartId = scope.Deptlist[i].PETCODE;
							scope.pssbiosubsubject = scope.Deptlist[i].SUBSUBJECT + "/" + scope.Deptlist[i].T_SUBSUBJECT;

						}
						if (scope.Deptlist[i].SUBSUBJECTCODE == "201514905" || scope.Deptlist[i].SUBSUBJECTCODE == "201514906" || scope.Deptlist[i].SUBSUBJECTCODE == "201514907" || scope.Deptlist[i].SUBSUBJECTCODE == "201514908") {

							scope.divscholar = true;
							scope.scholardepart = scope.Deptlist[i].ENAME;
							scope.scholardepartId = scope.Deptlist[i].PETCODE;
							scope.scholarsubsubject = scope.Deptlist[i].SUBSUBJECT + "/" + scope.Deptlist[i].T_SUBSUBJECT;

						}
						if (scope.Deptlist[i].SUBSUBJECTCODE == "201514900") {

							scope.divpension = true;
							scope.pensiondepart = scope.Deptlist[i].ENAME;
							scope.pensiondepartId = scope.Deptlist[i].PETCODE;
							scope.pensionsubsubject = scope.Deptlist[i].SUBSUBJECT + "/" + scope.Deptlist[i].T_SUBSUBJECT;

						}

						Statecourselist();
					}
				}
				else {
					alert(databind.Reason);
				}
			});

		}

		//Image Upload
		scope.ImageUpload = function (cardNumber, type, filename) {
			if (cardNumber == undefined || cardNumber == "") {
				swal("Please Select Image To Upload");
				return;
			}
			var file = type.attachment;
			var fileexten = file.type;
			if (fileexten.split("/")[0] == "image") {

			}
			else {
				swal("Only Image Accepted");
				return;
			}
			var prop = { AadharCardNumber: cardNumber, Attachment: type.attachment };
			entityService.saveTutorial(prop)
				.then(function (data) {
					scope.Imageurl = data.data;
					swal("Image Uploaded Successfully");
					console.log(data);
				});
		}

		scope.uplopadrationimage = function () {
			swal("success", "File Uploaded Successfully", 'success');
		}

		scope.uplopaddobimage = function () {
			swal("success", "File Uploaded Successfully", 'success');
		}


		scope.submitgrievance = function () {

			if (scope.umempuid == undefined || scope.umempuid == null) {
				swal("Please Enter AdharNumber");
				return;
			}

			//if (scope.district == undefined || scope.district == null) {
			//	swal("Please Select District Name");
			//	return;
			//}
			//if (scope.mandal == undefined || scope.mandal == null) {
			//	swal("Please Select Mandal Name");
			//	return;
			//}
			//if (scope.village == undefined || scope.village == null) {
			//	swal("Please Select Village Name");
			//	return;
			//}
			scope.grievancelist = [];
			//if (scope.textareafeedback == undefined || scope.textareafeedback == null) {

			//	swal("Please Enter Remarks about the grievance");
			//	return;
			//}
			var IndNum = /^[0]?[6789]\d{9}$/;
			if (scope.mobilenum == undefined || scope.mobilenum == null || scope.mobilenum.length < 10) {

				swal("Failure!", "Please Enter 10 digit Mobile Number", "error");
				return;
			}
			if (IndNum.test(scope.mobilenum)) {

			} else {
				swal("Failure!", "Invalid Mobile Number", "error");
				return;
			}
			if (scope.gender == undefined || scope.gender == null) {
				swal("Please Select Gender");
				return;
			}
			if (scope.applicatename == undefined || scope.applicatename == null) {
				swal("Please Enter Name");
				return;
			}

			var codes = '';
			var gricount = 0;
			for (var index = 0; index < scope.Checkedeligiblelist.length; index++) {

				if (scope.Checkedeligiblelist[index].STATUS != 'Eligible') {
					//failurecount++;
					codes += scope.Checkedeligiblelist[index].CODE + ",";

					//Ration
					if (scope.Checkedeligiblelist[index].CODE == "201514876") {

						if (scope.chkrationgri) {
							scope.rationgristatus = 1;
							gricount++;
							if (scope.Rationmum == "" || scope.Rationmum == undefined || scope.Rationmum == null) {

								swal('info', 'Please Enter Ration Number', 'error');
								return;
							}
						}
						else {
							scope.rationgristatus = 0;
						}
						var val = { Deptid: scope.rationdepartId, SubSbujectcode: '201514876', Otherproblem: 'RationId:' + scope.Rationmum + ",RationAttachmentfile:122,REMARKS:" + scope.textarearationfeedback, GrievanceStatus: scope.rationgristatus };
						scope.grievancelist.push(val);

					}
					//dob age
					if (scope.Checkedeligiblelist[index].CODE == "201514883") {
						if (scope.chkagegri) {
							scope.agegristatus = 1;
							gricount++;
							if (scope.dobcertificte == "" || scope.dobcertificte == undefined || scope.dobcertificte == null) {

								swal('info', 'Please Enter Dob Certficate Number', 'error');
								return;
							}
						}
						else {
							scope.agegristatus = 0;
						}

						var val2 = { Deptid: scope.agedepartId, SubSbujectcode: '201514883', Otherproblem: 'DobCertificatenum:' + scope.dobcertificte + ',DOBAttachmentfile:122,REMARKS:' + scope.textareavehiclefeedback, GrievanceStatus: scope.agegristatus };
						scope.grievancelist.push(val2);
					}

					//education
					if (scope.Checkedeligiblelist[index].CODE == "201514877") {

						if (scope.chkeducationgri) {
							gricount++;
							if (scope.edurollno == "" || scope.edurollno == undefined || scope.edurollno == null) {

								swal('info', 'Please Enter Roll Number', 'error');
								return;
							}
							if (scope.eduhallticketno == "" || scope.eduhallticketno == undefined || scope.eduhallticketno == null) {

								swal('info', 'Please Enter Hall Ticket Number', 'error');
								return;
							}
							if (scope.eduyearpass == "" || scope.eduyearpass == undefined || scope.eduyearpass == null) {

								swal('info', 'Please Enter Year of Pass', 'error');
								return;
							}
							if (scope.selectedstate == "" || scope.selectedstate == undefined || scope.selectedstate == null) {

								swal('info', 'Please Select State', 'error');
								return;
							}

							if (scope.selectedunversity == "" || scope.selectedunversity == undefined || scope.selectedunversity == null) {

								swal('info', 'Please Select University', 'error');
								return;
							}

							if (scope.selectedcourse == "" || scope.selectedcourse == undefined || scope.selectedcourse == null) {

								swal('info', 'Please Select Course', 'error');
								return;
							}
							scope.edugristatus = 1;
						}
						else {
							scope.edugristatus = 0;
						}

						var val3 = { Deptid: scope.edudepartId, SubSbujectcode: '201514877', Otherproblem: 'Rollno:' + scope.edurollno + ',HallTikcetnum:' + scope.halltilnum + ',Course:' + scope.selectedcourse + ',Year:' + scope.eduyearpass + ',InstituteName:' + scope.selectedunversity + ',state:' + scope.selectedstate + ',REMARKS:' + scope.textareaedufeedback, GrievanceStatus: scope.edugristatus };
						scope.grievancelist.push(val3);
					}

					//employee 
					if (scope.Checkedeligiblelist[index].CODE == "201514880") {
						if (scope.chkempgri) {
							scope.empgristatus = 1;
							gricount++;
						}
						else {
							scope.empgristatus = 0;
						}
						var val4 = { Deptid: scope.empdepartId, SubSbujectcode: '201514880', Otherproblem: 'REMARKS:' + scope.textareaempfeedback, GrievanceStatus: scope.empgristatus };
						scope.grievancelist.push(val4);

					}
					//employee private employee

					if (scope.Checkedeligiblelist[index].CODE == "201514898") {
						if (scope.chkempgri) {
							scope.empprigristatus = 1;
							gricount++;
						}
						else {
							scope.empprigristatus = 0;
						}
						var val4 = { Deptid: scope.empdepartId, SubSbujectcode: '201514898', Otherproblem: 'REMARKS:' + scope.textareaempfeedback, GrievanceStatus: scope.empprigristatus };
						scope.grievancelist.push(val4);
					}

					//obmms 
					if (scope.Checkedeligiblelist[index].CODE == "201514879") {

						if (scope.chkobmsgri) {
							scope.obmmsgristatus = 1;
							gricount++;
						}
						else {
							scope.obmmsgristatus = 0;
						}
						var val5 = { Deptid: scope.obmmdepartId, SubSbujectcode: '201514879', Otherproblem: 'REMARKS:' + scope.textareaobmsfeedback, GrievanceStatus: scope.obmmsgristatus };
						scope.grievancelist.push(val5);
					}



					//land
					if (scope.Checkedeligiblelist[index].CODE == "201514882") {

						if (scope.chklandgri) {
							scope.landgristatus = 1;
							gricount++;
						}
						else {
							scope.landgristatus = 0;
						}

						var val5 = { Deptid: scope.landdepartId, SubSbujectcode: '201514882', Otherproblem: 'REMARKS:' + scope.textarealandfeedback, GrievanceStatus: scope.landgristatus };
						scope.grievancelist.push(val5);
					}

					//Vehicle
					if (scope.Checkedeligiblelist[index].CODE == "201514885") {
						if (scope.chkvehigri) {
							scope.vehgristatus = 1;
							gricount++;
						}
						else {
							scope.vehgristatus = 0;
						}
						var val5 = { Deptid: scope.vehicledepartId, SubSbujectcode: '201514885', Otherproblem: 'REMARKS:' + scope.textareavehiclefeedback, GrievanceStatus: scope.vehgristatus };
						scope.grievancelist.push(val5);
					}

					//Psscode
					if (scope.Checkedeligiblelist[index].CODE == "201514897") {
						if (scope.chkpsssurveygri) {
							scope.psssurveygristatus = 1;
							gricount++;
						}
						else {
							scope.psssurveygristatus = 0;
						}
						var val5 = { Deptid: scope.pssdepartId, SubSbujectcode: '201514897', Otherproblem: 'REMARKS:' + scope.textareavehiclefeedback, GrievanceStatus: scope.psssurveygristatus };
						scope.grievancelist.push(val5);
					}
					//bio metric in pss
					if (scope.Checkedeligiblelist[index].CODE == "201514896") {
						if (scope.chkpssbiogri) {
							scope.pssbiogristatus = 1;
							gricount++;
						}
						else {
							scope.pssbiogristatus = 0;
						}
						var val5 = { Deptid: scope.pssbiodepartId, SubSbujectcode: '201514896', Otherproblem: 'REMARKS:' + scope.textareavehiclefeedback, GrievanceStatus: scope.pssbiogristatus };
						scope.grievancelist.push(val5);
					}
					//scholarship
					if (scope.Checkedeligiblelist[index].CODE == "201514905" || scope.Checkedeligiblelist[index].CODE == "201514906" || scope.Checkedeligiblelist[index].CODE == "201514907" || scope.Checkedeligiblelist[index].CODE == "201514908") {
						if (scope.chkscholargri) {
							scope.schlrgristatus = 1;
							gricount++;
						}
						else {
							scope.schlrgristatus = 0;
						}
						var val5 = { Deptid: scope.scholardepartId, SubSbujectcode: scope.Checkedeligiblelist[index].CODE, Otherproblem: 'REMARKS:' + scope.textareavehiclefeedback, GrievanceStatus: scope.schlrgristatus };
						scope.grievancelist.push(val5);
					}

					//pension
					if (scope.Checkedeligiblelist[index].CODE == "201514900") {
						if (scope.chkpensiongri) {
							scope.pensiongristatus = 1;
							gricount++;
						}
						else {
							scope.pensiongristatus = 0;
						}
						var val5 = { Deptid: scope.pensiondepartId, SubSbujectcode: '201514900', Otherproblem: 'REMARKS:' + scope.textareavehiclefeedback, GrievanceStatus: scope.pensiongristatus };
						scope.grievancelist.push(val5);
					}

				}
			}

			if (gricount < 1) {

				swal('info', 'Please check minimum one grievance checkbox', 'error');
				return;
			}
			codes = codes.slice(0, -1);
			var cval = codes.split(',').length;

			if (cval != gricount) {

				swal('info', 'You have total of ' + cval + ' grievances,You are raising only :' + gricount + 'among them', 'error');
			}

			if (codes == null || codes == undefined) {
				swal("Please check no eligible");
				return;
			}

			else {
				var response = { ADHAR_NO: scope.umempuid, MOBILE_NUMBER: scope.mobilenum, APPLICATENT_NAME: scope.applicatename, PROBLEM_DESC: scope.textareafeedback, grievancecode: codes, GENDER: scope.pssgender, pssVILLAGECODE: scope.selectedvillage, pssMANDAL: scope.selectedmandal, pssDISTRIC_ID: scope.selecteddistrict, RUFlag: scope.pssruralurbanflag, grievancelist: scope.grievancelist };
				var url = "/API/UnEmployee/SaveGrievanceDetails";
				datacontext.post(url, response, function (data) {
					var res = data.data;
					if (res.Status == "Success") {
						//swal('Success', res.Reason + ":" + res.ApplicationID, 'error');
						//swal('info', res.Reason, 'success');
						swal({
							title: "success",
							text: res.Reason,
							type: "success"
						});
						//swal("Your Grievance Submitted Successfully");
						//window.location.reload());
						interval(function () { window.location.reload(); }, 5000);

						return;
					}
					else {

						swal("Failure", res.Reason, "error");
					}
				}, function (error) {
					alert(error);
				});
				//swal("Your Grievance Submitted Successfully");
				//document.location.reload();
			}
		}

	}

	$(document).keydown(function (event) {
		if (event.keyCode == 123) { // Prevent F12
			return false;
		} else if (event.ctrlKey && event.shiftKey && event.keyCode == 73) { // Prevent Ctrl+Shift+Iagreeuinversityinfo
			return false;
		}
	});
	$(document).on("contextmenu", function (e) {
		e.preventDefault();
	});

	$(document).onkeydown = function (e) {
		if (e.ctrlKey &&
			(e.keyCode === 67 ||
				e.keyCode === 86 ||
				e.keyCode === 85 ||
				e.keyCode === 117)) {
			//alert('not allowed');
			return false;
		} else {
			return true;
		}
	};

	angular.module("MyApp")
		.factory("entityService", ["akFileUploaderService", function (akFileUploaderService) {
			var saveTutorial = function (tutorial) {
				return akFileUploaderService.saveModel(tutorial, "/CMyuvaNapp/Rakshana/POSTData");
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