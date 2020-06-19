using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Dynamic;
using Newtonsoft.Json;
using gswsBackendAPI.DL.DataConnection;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Xml;
using System.IO;
using System.Threading.Tasks;
using gswsBackendAPI.Depts.paymentChallan.BackEnd;

namespace gswsBackendAPI.DL.CommonHel
{
	public class LoginHelper: LoginSPHelper
	{
		dynamic _ResultObj = new ExpandoObject();
		ResponseModel _ResObj = new ResponseModel();
		public dynamic GetLogin(LoginModel Lobj)
		{
			try
			{
				string captchaverify =new captchahelper().GetCaptchVerify(Lobj);
				if (captchaverify == "Success")
				{
					DataTable dtLogin = GetLogin_SP(Lobj);
					if (dtLogin != null && dtLogin.Rows.Count > 0)
					{
						HeadertokenModel obk = new HeadertokenModel();
						obk.Ftype = "1";
						obk.UserId = Lobj.FUsername;
						obk.Token = ComputeSha256Hash(Guid.NewGuid().ToString());
						 DataTable dt1= GetLogin_TokenSave(obk);
						if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "1")
						{
							token_gen.initialize();
							token_gen.expiry_minutes = 120;
							token_gen.addClaim("admin");
							token_gen.PRIMARY_MACHINE_KEY = "10101010101010101010101010101010";
							token_gen.SECONDARY_MACHINE_KEY = "1010101010101010";
							token_gen.addResponse("Status", "100");
							token_gen.addResponse("Details", JsonConvert.SerializeObject(dtLogin));
							token_gen.addResponse("SKey", obk.Token);
							return token_gen.generate_token();
						}
						else
						{
							_ResObj.Status = 102;
							_ResObj.Reason = "Invalid Username and Password";
						}
					}
					else
					{
						_ResObj.Status = 102;
						_ResObj.Reason = "Invalid Username and Password";
					}
				}
				else
				{
					_ResObj.Status = 102;
					_ResObj.Reason = "Invalid Captcha";
				}
			}
			catch (Exception ex)
			{
				_ResObj.Status = 102;
				_ResObj.Reason = ErrorMessage;

			}
			return _ResObj;
		}


		public string SaveToken_Login(HeadertokenModel objl)
		{
			ResponseModel obj = new ResponseModel();
			try
			{
				DataTable dtl = new DataTable();				
					dtl = GetLogin_TokenSave(objl);
				if (dtl != null && dtl.Rows.Count > 0 && dtl.Rows[0][0].ToString() == "1")
				{
					return "Success";
				}
				else
				{
					return "Failure";
				}
				
				
			}
			catch (Exception ex)
			{
				throw ex;
				
			}
		}
		public dynamic GetLgdmaster(LGDMasterModel Lobj)
		{
			try
			{
				DataTable dtLgd = GetLgdMaster_SP(Lobj);
				if (dtLgd != null && dtLgd.Rows.Count > 0)
				{
					_ResObj.Status = 100;
					_ResObj.Reason = "Your Login Successfully";
					_ResObj.DataList = dtLgd;
				}
				else
				{
					_ResObj.Status = 102;
					_ResObj.Reason = "No Data Found";
				}
			}
			catch (Exception ex)
			{
				_ResObj.Status = 102;
				_ResObj.Reason = ErrorMessage;

			}
			return _ResObj;
		}

		public dynamic GetgswsSeccmaster(LGDMasterModel Lobj)
		{
			try
			{
				DataTable dtLgd = GetGSWS_SecretariatMaster_SP(Lobj);
				if (dtLgd != null && dtLgd.Rows.Count > 0)
				{
					_ResObj.Status = 100;
					_ResObj.Reason = "Your Login Successfully";
					_ResObj.DataList = dtLgd;
				}
				else
				{
					_ResObj.Status = 102;
					_ResObj.Reason = "No Data Found";
				}
			}
			catch (Exception ex)
			{
				_ResObj.Status = 102;
				_ResObj.Reason = ErrorMessage;

			}
			return _ResObj;
		}

		public dynamic getAllUrlData(LGDMasterModel Lobj)
        {
            try
            {
                DataTable dtLgd = GetAllUrls_SP(Lobj);
                if (dtLgd != null && dtLgd.Rows.Count > 0)
                {
                    _ResObj.Status = 100;
                    _ResObj.Reason = "Your Login Successfully";
                    _ResObj.DataList = dtLgd;
                }
                else
                {
                    _ResObj.Status = 102;
                    _ResObj.Reason = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                _ResObj.Status = 102;
                _ResObj.Reason = ErrorMessage;

            }
            return _ResObj;
        }

		public dynamic getGVDashboard(LGDMasterModel Lobj)
		{
			dynamic objdash = new ExpandoObject();
			try
			{
				DataTable dtLgd = Get_GVDashboard_SP(Lobj);
				Lobj.FTYPE = "2";
				DataTable dtdist = Get_GVDashboard_SP(Lobj);

				if (dtLgd != null && dtLgd.Rows.Count > 0)
				{
					objdash.Status = 100;
					objdash.Reason = "Your Login Successfully";
					objdash.DataList = dtLgd;
					objdash.DataDistList = dtdist;
				}
				else
				{
					objdash.Status = 102;
					objdash.Reason = "No Data Found";
				}
			}
			catch (Exception ex)
			{
				objdash.Status = 102;
				objdash.Reason = ErrorMessage;

			}
			return objdash;
		}

		public dynamic GetTransResponse_helper(TransRes Lobj)
		{
			try
			{
				DataTable dttrans = GetTransactionResponse_SP(Lobj);


				if (dttrans != null && dttrans.Rows.Count > 0)
				{
					_ResObj.Status = 100;
					_ResObj.Reason = "Data Getting Successfully";
					_ResObj.DataList = dttrans;
				}
				else
				{
					_ResObj.Status = 102;
					_ResObj.Reason = "No Data Found";
				}
			}
			catch (Exception ex)
			{
				_ResObj.Status = 102;
				_ResObj.Reason = ErrorMessage;

			}
			return _ResObj;
		}

		public dynamic GetRegRecDashboard(TransRes Lobj)
		{
			try
			{
				DataTable dttrans = GetRegReceivedDashboardCount(Lobj);


				if (dttrans != null && dttrans.Rows.Count > 0)
				{
					_ResObj.Status = 100;
					_ResObj.Reason = "Data Getting Successfully";
					_ResObj.DataList = dttrans;
				}
				else
				{
					_ResObj.Status = 102;
					_ResObj.Reason = "No Data Found";
				}
			}
			catch (Exception ex)
			{
				_ResObj.Status = 102;
				_ResObj.Reason = ErrorMessage;

			}
			return _ResObj;
		}

		public dynamic GetStatusTrackingDetails(StatusTrackingModel objstatus)
		{
			dynamic objdata=new ExpandoObject();
			try
			{
				string Status = SaveStatusTracking(objstatus);
				if (Status == "Success")
				{
					objdata.Status = "100";
					objdata.Reason = "Data Submitted Successfully";
				}
				else
				{
					objdata.Status = "102";
					objdata.Reason = "Data Not Submitted";
				}
			}
			catch (Exception ex)
			{
				objdata.Status = "102";
				objdata.Reason = ErrorMessage;
			}
			return objdata;
		}

		public dynamic SaveReceiveAction_helper(TransRes Lobj)
		{
			try
			{
				DataTable dttrans = SaveReceiveAction_SP(Lobj);


				//if (dttrans != null && dttrans.Rows.Count > 0)
				//{
				//    _ResObj.Status = 100;
				//    _ResObj.Reason = "Data Getting Successfully";
				//    _ResObj.DataList = dttrans;
				//}
				//else
				//{
				//    _ResObj.Status = 102;
				//    _ResObj.Reason = "No Data Found";
				//}

				_ResObj.Status = 100;
				_ResObj.Reason = "Data Getting Successfully";
				_ResObj.DataList = dttrans;

			}
			catch (Exception ex)
			{
				_ResObj.Status = 102;
				_ResObj.Reason = ErrorMessage;

			}
			return _ResObj;
		}

		#region Profile Update

		//Send OTP
		public dynamic ProfileSendOTP_helper(Profilemodel Lobj)
		{
			try
			{
				Random random = new Random();
				string r = "";
				int i;
				for (i = 0; i < 6; i++)
				{
					r += random.Next(0, 6).ToString();
				}
				Lobj.OTP = r;

				string k = ProfileSendOTP_SP(Lobj);

				if (k == "Success")
				{
					if (Lobj.TYPE == "0")
					{
						SMSService.SMSService objsms = new SMSService.SMSService();
						string Status = objsms.SendSMS(Lobj.MOBILE, "Your OTP to update mobile number in Prajasachivalayam is : " + Lobj.OTP);
						if (Status.Contains("402"))
						{
							_ResObj.Status = 100;
							_ResObj.Reason = "OTP Sent Successfully To the entered Mobile Number";
						}
						else
						{
							_ResObj.Status = 102;
							_ResObj.Reason = "OTP Sending Failed";
						}
						return _ResObj;
					}
					else if (Lobj.TYPE == "1")
					{
						string Status = SendOTPforEmail(Lobj);
						if (Status.Contains("402"))
						{
							_ResObj.Status = 100;
							_ResObj.Reason = "OTP Sent Successfully To the entered Mail Address";
						}
						else
						{
							_ResObj.Status = 102;
							_ResObj.Reason = "OTP Sending Failed";
						}
						return _ResObj;
					}
				}
				else
				{
					_ResObj.Status = 102;
					_ResObj.Reason = "OTP Sending Failed";
				}
			}
			catch (Exception ex)
			{
				_ResObj.Status = 102;
				_ResObj.Reason = ErrorMessage;

			}
			return _ResObj;
		}

		//Verify OTP
		public dynamic ProfileVerifyOTP_helper(Profilemodel Lobj)
		{
			try
			{
				bool k = ProfileVerifyOTP_SP(Lobj);

				if (k == true)
				{
					_ResObj.Status = 100;
					_ResObj.Reason = "OTP Verified Successfully";
				}
				else
				{
					_ResObj.Status = 102;
					_ResObj.Reason = "OTP Verification Failed";
				}
			}
			catch (Exception ex)
			{
				_ResObj.Status = 102;
				_ResObj.Reason = ErrorMessage;

			}
			return _ResObj;
		}

		//Save Password
		public dynamic ProfileUpdate_helper(Profilemodel Lobj)
		{
			try
			{
				Lobj.PASSWORD = ComputeSha256Hash(Lobj.PASSWORD);
				bool k = ProfileUpdate_SP(Lobj);


				if (k == true)
				{
					_ResObj.Status = 100;
					_ResObj.Reason = "Data Updated Successfully";

				}
				else
				{
					_ResObj.Status = 102;
					_ResObj.Reason = "Data Updation Failed";
				}
			}
			catch (Exception ex)
			{
				_ResObj.Status = 102;
				_ResObj.Reason = ErrorMessage;

			}
			return _ResObj;
		}

		//Save Secretartiat Form
		public dynamic GetSaveSeccformDat(SecretariatFormObject Lobj)
		{
			try
			{
				if (Lobj.Ftype == "1")
				{
					string validateval = GetValidateHardware(Lobj);
					if (validateval != "Success")
					{
						_ResObj.Status = 102;
						_ResObj.Reason = validateval;

						return _ResObj;
					}

				}
				string status = SaveSecretariatForm_SP(Lobj);


				if (status.Equals("Success"))
				{
					_ResObj.Status = 100;
					_ResObj.Reason = "Data Submitted Successfully";

				}
				else
				{
					_ResObj.Status = 102;
					_ResObj.Reason = status;
				}
			}
			catch (Exception ex)
			{
				_ResObj.Status = 102;
				_ResObj.Reason = ErrorMessage;

			}
			return _ResObj;
		}

		public dynamic GetVolunteerFormData(VolunteerFormObject Lobj)
		{
			try
			{
				string validateval = GetValidateVolunteer(Lobj);
				if (validateval != "Success")
				{
					_ResObj.Status = 102;
					_ResObj.Reason = validateval;

					return _ResObj;
				}

				string status = SaveVolunteerForm_SP(Lobj);


				if (status.Equals("Success"))
				{
					_ResObj.Status = 100;
					_ResObj.Reason = "Data Submitted Successfully";

				}
				else
				{
					_ResObj.Status = 102;
					_ResObj.Reason = status;
				}
			}
			catch (Exception ex)
			{
				_ResObj.Status = 102;
				_ResObj.Reason = ErrorMessage;

			}
			return _ResObj;
		}

		public dynamic GetSeccVolunteerData(LGDMasterModel Lobj)
		{
			dynamic objdata = new ExpandoObject();
			try
			{
				DataTable dtsecc = GetSeccandVolunteerdetail_SP(Lobj);
				Lobj.FTYPE = "2";
				DataTable dtvol = GetSeccandVolunteerdetail_SP(Lobj);


				if (dtsecc != null && dtsecc.Rows.Count > 0)
				{
					objdata.Status = 100;
					objdata.DataList = dtsecc;
					objdata.VolList = dtvol;
					objdata.Reason = "";
				}
				else
				{
					objdata.Status = 102;
					objdata.DataList = dtsecc;
					objdata.Reason = "This Secretariat Data is Not Available.";
				}
			}
			catch (Exception ex)
			{
				objdata.Status = 102;
				objdata.Reason = ErrorMessage;

			}
			return objdata;
		}
		public dynamic GetGeoTrackingMailUpdate(LoginMailModel Lobj)
		{
			try
			{
				
				string status = UpdateMailMobileForm_SP(Lobj);


				if (status.Equals("Success"))
				{
					_ResObj.Status = 100;
					_ResObj.Reason = "Data Submitted Successfully";

				}
				else
				{
					_ResObj.Status = 102;
					_ResObj.Reason = status;
				}
			}
			catch (Exception ex)
			{
				_ResObj.Status = 102;
				_ResObj.Reason = ErrorMessage;

			}
			return _ResObj;
		}

		//Send Email
		public dynamic SendOTPforEmail(Profilemodel obj)
		{
			try
			{
				MailMessage message = new MailMessage();
				SmtpClient smtp = new SmtpClient();
				message.From = new MailAddress("gowtham.codetree@gmail.com");
				message.To.Add(new MailAddress(obj.EMAILID));
				message.Subject = "Test";
				message.IsBodyHtml = true; //to make message body as html  
				message.Body = "Your OTP to update Email ID in Prajasachivalayam is : " + obj.OTP;
				smtp.Port = 587;
				smtp.Host = "smtp.gmail.com"; //for gmail host  
				smtp.EnableSsl = true;
				smtp.UseDefaultCredentials = false;
				smtp.Credentials = new NetworkCredential("gowtham.codetree@gmail.com", "nagchaitu");
				smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
				smtp.Send(message);
				return "402";
			}

			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		//Generate sha256 hash for a string
		public static string ComputeSha256Hash(string rawData)
		{
			// Create a SHA256   
			using (SHA256 sha256Hash = SHA256.Create())
			{
				// ComputeHash - returns byte array  
				byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

				// Convert byte array to a string   
				StringBuilder builder = new StringBuilder();
				for (int i = 0; i < bytes.Length; i++)
				{
					builder.Append(bytes[i].ToString("x2"));
				}
				return builder.ToString();
			}
		}

		public string GetValidateHardware(SecretariatFormObject objsecc)
		{
			HardwareValidateModel objwh = new HardwareValidateModel();

			try
			{
				if (!string.IsNullOrEmpty(objsecc.SecSIMOneIMEI))
				{
					objwh.FTYPE = "1";
					objwh.IMEINUM = objsecc.SecSIMOneIMEI;
					DataTable dt1 = GetHardwareValidate_SP(objwh);
					//if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "0")
					//{
					//	return "Invalid IMEI-1 Number for Secretariat";
					//}
					if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "1")
					{
						return "Already Submitted IMEI-1 Number for Secretariat";
					}

				}
				 if (!string.IsNullOrEmpty(objsecc.SecIMEITwoSIM))
				{
					objwh.FTYPE = "2";
					objwh.IMEINUM = objsecc.SecIMEITwoSIM;
					DataTable dt1 = GetHardwareValidate_SP(objwh);
					//if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "0")
					//{
					//	return "Invalid IMEI-2 Number for Secretariat";
					//}
					if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "1")
					{
						return "Already Submitted IMEI-2 Number for Secretariat";
					}
				}
				 if (!string.IsNullOrEmpty(objsecc.DesktoponeSerial))
				{
					objwh.FTYPE = "3";
					objwh.SERIALNUM = objsecc.DesktoponeSerial;
					DataTable dt1 = GetHardwareValidate_SP(objwh);
					if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "0")
					{
						return "Invalid Desktop-1 Serial Number.Please Enter Correct Desktop-1 Serial Number";
					}
					if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "2")
					{
						return "Already Submitted Desktop-1 Serial Number";
					}
				}
				 if (!string.IsNullOrEmpty(objsecc.DesktoptwoSerial))
				{
					objwh.FTYPE = "3";
					objwh.SERIALNUM = objsecc.DesktoptwoSerial;
					DataTable dt1 = GetHardwareValidate_SP(objwh);
					if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "0")
					{
						return "Invalid Desktop-2 Serial Number.Please Enter Correct Desktop-2 Serial Number";
					}
					if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "2")
					{
						return "Already Submitted Desktop-2 Serial Number";
					}
				}
				 if (!string.IsNullOrEmpty(objsecc.UPSoneSerialnum))
				{
					objwh.FTYPE = "4";
					objwh.SERIALNUM = objsecc.UPSoneSerialnum;
					DataTable dt1 = GetHardwareValidate_SP(objwh);
					if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "0")
					{
						return "Invalid UPS-1 Serial Number.Please Enter Correct UPS-1 Serial Number";
					}
					if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "2")
					{
						return "Already Submitted UPS-1 Serial Number";
					}
				
				}
				 if (!string.IsNullOrEmpty(objsecc.UPStwoSerialnum))
				{
					objwh.FTYPE = "4";
					objwh.SERIALNUM = objsecc.UPStwoSerialnum;
					DataTable dt1 = GetHardwareValidate_SP(objwh);
					if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "0")
					{
						return "Invalid UPS-2 Serial Number.Please Enter Correct UPS-2 Serial Number";
					}
					if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "2")
					{
						return "Already Submitted UPS-2 Serial Number";
					}
				}
				if (!string.IsNullOrEmpty(objsecc.PrinterSerialNum))
				{
					objwh.FTYPE = "5";
					objwh.SERIALNUM = objsecc.PrinterSerialNum;
					objwh.MODELNUM = objsecc.PrinterModelNum;
					DataTable dt1 = GetHardwareValidate_SP(objwh);
					if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "0")
					{
						return "Invalid Printer Model or Serial Number.Please Enter Correct Printer cum Scanner Serial Number";
					}
					if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "2")
					{
						return "Already Submitted Printer Model or Serial Number";
					}
				}
				if (!string.IsNullOrEmpty(objsecc.SecMobileNum))
				{
					objwh.FTYPE = "6";
					objwh.SERIALNUM = objsecc.SecSIMSerialNum;
					objwh.MODELNUM = objsecc.SecMobileNum;
					DataTable dt1 = GetHardwareValidate_SP(objwh);
					//if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "0")
					//{
					//	return "Invalid Mobile or Serial Number.Please Enter Correct Mobile or Serial Number";
					//}
					if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "1")
					{
						return "Already Submitted Mobile  or Serial Number";
					}
				}
				if (!string.IsNullOrEmpty(objsecc.MahileMobileNumber))
				{
					objwh.FTYPE = "6";
					objwh.SERIALNUM = objsecc.MahilaSIMSerialNum;
					objwh.MODELNUM = objsecc.MahileMobileNumber;
					DataTable dt1 = GetHardwareValidate_SP(objwh);
					//if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "0")
					//{
					//	return "Invalid Mahila Police Mobile or Serial Number.Please Enter Correct Mahila Police Mobile or Serial Number";
					//}
					if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "1")
					{
						return "Already Submitted Mahila Police Mobile Number  or Serial Number";
					}
				}
				else
				{
					return "Success";
				}

				return "Success";

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public string GetValidateVolunteer(VolunteerFormObject objsecc)
		{
			HardwareValidateModel objwh = new HardwareValidateModel();

			try
			{
				if (!string.IsNullOrEmpty(objsecc.VolunteerSIMIMEIOne))
				{
					objwh.FTYPE = "1";
					objwh.IMEINUM = objsecc.VolunteerSIMIMEIOne;
					DataTable dt1 = GetHardwareValidate_SP(objwh);
					//if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "0")
					//{
					//	return "Invalid IMEI-1 Number for Secretariat";
					//}
					if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "1")
					{
						return "Already Submitted IMEI-1 Number for Secretariat";
					}

				}
				if (!string.IsNullOrEmpty(objsecc.VolunteerSIMIMEITwo))
				{
					objwh.FTYPE = "2";
					objwh.IMEINUM = objsecc.VolunteerSIMIMEITwo;
					DataTable dt1 = GetHardwareValidate_SP(objwh);
					//if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "0")
					//{
					//	return "Invalid IMEI-2 Number for Secretariat";
					//}
					if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "1")
					{
						return "Already Submitted IMEI-2 Number for Secretariat";
					}
				}
				
				if (!string.IsNullOrEmpty(objsecc.VolunteerOfficialMobile))
				{
					objwh.FTYPE = "6";
					objwh.SERIALNUM = objsecc.VolunteerSerialNumber;
					objwh.MODELNUM = objsecc.VolunteerOfficialMobile;
					DataTable dt1 = GetHardwareValidate_SP(objwh);
					//if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "0")
					//{
					//	return "Invalid Mobile or Serial Number.Please Enter Correct Mobile or Serial Number";
					//}
					if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0][0].ToString() == "1")
					{
						return "Already Submitted Mobile  or Serial Number";
					}
				}
				
				else
				{
					return "Success";
				}

				return "Success";

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		#endregion

		#region Navasakam

		public dynamic GetNavasakamToken(Navakasammodel obj)
		{
			dynamic objns = new ExpandoObject();
			try
			{
				var response = new EncryptDecrypt().SpandanaPostData("http://nskservices.apcfss.in/rest/Nsk/genToken", obj, "Basic bmF2YXNha2FtOk5AdmEka2FtQDEyMw==");

				dynamic res = JsonConvert.DeserializeObject<dynamic>(response);
				if (res.response == "success")
				{
					objns.Status = "100";
					objns.Returnurl = "https://navasakam5.apcfss.in/GWSecrLogin.do?token=" + res.token;
				}
				else
				{
					objns.Status = "102";
					objns.Reason = "Invalid Token is generated";
				}


			}
			catch (Exception ex)
			{
				objns.Status = "102";
				objns.Reason = ErrorMessage;
			}
			return objns;
		}

		public dynamic GetWalletRecharge(WalletModel obj)
		{
			dynamic objns = new ExpandoObject();
			try
			{
				obj.userName = "GSWS";
				obj.password = "GSWS@123";
				var response = new EncryptDecrypt().PostData("https://gramawardsachivalayam.ap.gov.in/GSWSAPI/api/web/walletOneRecharge", obj);

				dynamic res = JsonConvert.DeserializeObject<dynamic>(response);
				if (res.status == "200")
				{
					objns.Status = "100";
					objns.TransId = res.result;
				}
				else
				{
					objns.Status = "102";
					objns.Reason = "Some Issue on Top up Service";
				}


			}
			catch (Exception ex)
			{

				objns.Status = "102";
				objns.Reason = ErrorMessage;
			}
			return objns;
		}


		public dynamic GetWalletAmount(WalletAmountmodel obj)
		{
			dynamic objns = new ExpandoObject();
			try
			{
				//RamInfoService.ServicesRWMS objram = new RamInfoService.ServicesRWMS();
				//RamInfoService.WalletReqBean objwallet = new RamInfoService.WalletReqBean();
				RaminfoprodServce.ServicesRWMS objram = new RaminfoprodServce.ServicesRWMS();
				RaminfoprodServce.WalletReqBean objwallet = new RaminfoprodServce.WalletReqBean();
				objwallet.distCode = obj.district_code;
				objwallet.strGWSCode = obj.gsws_code;
				objwallet.strUserId = "GWSRWO";
				objwallet.strPassWord = "GWSRWO@123";
				RaminfoprodServce.WalletResBean wallResponse = objram.RWMSDisplayWallet(objwallet);
				
				if (wallResponse.errorCode== "0")
				{
					objns.Status = "100";
					objns.Amount = wallResponse.topupAmount; 
				}
				else
				{
					objns.Status = "102";
					objns.Reason = wallResponse.msg;

				}


			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("RaminfoExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetWalletAmount:" + ex.Message.ToString()));

				objns.Status = "102";
				objns.Reason = ErrorMessage;
			}
			return objns;
		}
		#endregion
		#region Forgotpassword

		//Sending OTP
		public dynamic ForgotpasswordsendOTP_helper(LoginModel Lobj)
		{
			try
			{
				DataTable dt = GetLogin_SP(Lobj);

				if (dt != null && dt.Rows.Count > 0)
				{
					_ResObj.Status = 100;
					_ResObj.Reason = "Success";
					_ResObj.DataList = dt;

					Random random = new Random();
					string r = "";
					int i;
					for (i = 0; i < 6; i++)
					{
						r += random.Next(0, 6).ToString();
					}
					Lobj.OTP = r;

					Lobj.MOBILE = dt.Rows[0]["MOBILE_NUMBER"].ToString();
					Lobj.UNIQUEID = dt.Rows[0]["UNIQUE_ID"].ToString();

					Profilemodel proobj = new Profilemodel();
					proobj.OTP = Lobj.OTP;
					proobj.UNIQUEID = Lobj.UNIQUEID;
					proobj.TYPE = "0";

					string k = ProfileSendOTP_SP(proobj);

					if (k == "Success")
					{
						if (!string.IsNullOrEmpty(Lobj.MOBILE))
						{
							SMSService.SMSService objsms = new SMSService.SMSService();
							string Status = objsms.SendSMS(Lobj.MOBILE, "Your OTP to Reset Password in Prajasachivalayam is : " + Lobj.OTP);
							if (Status.Contains("402"))
							{
								string hidemobile = "";
								hidemobile = "XXXXXX" + Lobj.MOBILE.Substring(6, 4);
								_ResObj.Status = 100;
								_ResObj.Reason = "OTP Sent Successfully To " + hidemobile;
							}
							else
							{
								_ResObj.Status = 102;
								_ResObj.Reason = "OTP Sending Failed";
							}
						}
						else
						{
							_ResObj.Status = 102;
							_ResObj.Reason = "No Mobile Number Found for this username";
						}
					}
					else
					{
						_ResObj.Status = 102;
						_ResObj.Reason = "Something Went Wrong.Please try again!!!";
					}
				}
				else
				{
					_ResObj.Status = 102;
					_ResObj.Reason = "Not a Registered UserName";
				}
			}
			catch (Exception ex)
			{
				_ResObj.Status = 102;
				_ResObj.Reason = ex.Message;
			}
			return _ResObj;
		}

		#endregion

		public dynamic GetCountsData_Helper(LoginModel Lobj)
		{
			try
			{
				DataTable dtLogin = GetCountsData_Helper_SP(Lobj);
				_ResObj.Status = 100;
				_ResObj.DataList = dtLogin;
			}
			catch (Exception ex)
			{
				_ResObj.Status = 102;
				_ResObj.Reason = ErrorMessage;

			}
			return _ResObj;
		}


		#region Aadhaar OTP Login

		//Aadhar Based Login Send OTP
		public dynamic AadhaarSendOTP_helper(string Aadhaarnum)
		{
			AadharOTP obj = new AadharOTP();
			try
			{
				WRServiceAadhaarOTP83.WREKYCOTP _objservice = new WRServiceAadhaarOTP83.WREKYCOTP();
				string response = _objservice.YUVSendAuth(Aadhaarnum, "GSWS", "2/106");  // OTPGenerationBySRDHSecuredeKYC(Aadhaarno, "smart pulse survey", "1/1022");
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(response);
				string status = string.Empty;
				string a = @"/java/object[class='com.ecentric.bean.otpgenration']";
				XmlNodeList nodeList = doc.SelectNodes(a);
				XmlNodeList elemList = doc.GetElementsByTagName("void");
				if (elemList[0].ChildNodes[0].InnerText == "100")
				{
					obj.Status = "100";
					obj.Reason = "OTP generation Successful";
					obj.Result = "";
				}
				else
				{
					if (elemList[1].ChildNodes[0].InnerText.Contains("996") || elemList[1].ChildNodes[0].InnerText.Contains("997"))
					{
						obj.Status = "102";
						obj.Reason = "This Aadhaar is not linked  properly.Please visit nearest enrolment center";
						obj.Result = "";
					}
					else if (elemList[1].ChildNodes[0].InnerText.Contains("112") || elemList[1].ChildNodes[0].InnerText.Contains("111") || elemList[1].ChildNodes[0].InnerText.Contains("110"))
					{
						obj.Status = "102";
						obj.Reason = "Your Aadhaar is not linked with any Mobile number.Please visit nearest enrolment center to update mobile number and eMailID";
						obj.Result = "";
					}
					else if (elemList[2].ChildNodes[0].InnerText.Contains("112") || elemList[2].ChildNodes[0].InnerText.Contains("111") || elemList[2].ChildNodes[0].InnerText.Contains("110"))
					{
						obj.Status = "102";
						obj.Reason = "Your Aadhaar is not linked with any Mobile number.Please visit nearest enrolment center to update mobile number and eMailID";
						obj.Result = "";
					}
					else if (elemList[1].ChildNodes[0].InnerText.Contains("521"))
					{
						obj.Status = "102";
						obj.Reason = "Your Aadhaar is linked with invalid mobile number,Please visit nearest enrolment center to update mobile number and eMailID";
						obj.Result = "";
					}
					else if (elemList[1].ChildNodes[0].InnerText.Contains("952"))
					{
						obj.Status = "102";
						obj.Reason = "OTP generation Failed,Please try again  after 10 minutes.";
						obj.Result = "";
					}
					else if (elemList[1].ChildNodes[0].InnerText.Contains("997"))
					{
						obj.Status = "102";
						obj.Reason = ".Your Aadhaar Number is not linked Properly.";
						obj.Result = "";
					}
					else
					{
						obj.Status = "102";
						obj.Reason = elemList[1].ChildNodes[0].InnerText + "," + elemList[0].ChildNodes[0].InnerText;
						obj.Result = "";
					}
				}

			}
			catch (Exception ex)
			{
				//string mappath = HttpContext.Current.Server.MapPath("CatchExceptionLogs");
				//Task WriteTask = Task.Factory.StartNew(() => logshel.Write_Log_Exception(mappath, "AadhaarSendOTPHelper:" + ex.Message));
				if (ex.Message == "Data at the root level is invalid. Line 1, position 1.")
				{
					obj.Status = "102";
					obj.Reason = "Aadhaar server is not responding,Please try again after sometime";
				}
				else
				{
					obj.Status = "Failure";
					obj.Reason = ex.Message;
				}
			}
			return obj;
		}

		//Aadhar Based Login Verify OTP
		public dynamic AadhaarVerifyOTP_helper(string UID_NUM, string OTPNUM)
		{
			Aadhardetails obj = new Aadhardetails();
			try
			{
				WRServiceAadhaarOTP83.WREKYCOTP objOTP = new WRServiceAadhaarOTP83.WREKYCOTP();
				var result = objOTP.YuvaOtpVerifyAuth(OTPNUM, UID_NUM, "2/106", "GSWS");
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(result);
				string status = string.Empty;
				string a = @"/java/object[@ class='com.ecentric.bean.BiometricresponseBean']";
				XmlNodeList nodeList = doc.SelectNodes(a);
				XmlNodeList elemList = doc.GetElementsByTagName("void");

				if (elemList[2].ChildNodes[0].InnerText == "100")
				{
					foreach (XmlNode MemberObjN1 in nodeList)
					{
						foreach (XmlNode MemberObjN in MemberObjN1.ChildNodes)
						{
							string strPropvalue = MemberObjN.Attributes["property"].Value.ToString();
							string strValue = MemberObjN.InnerText.ToString();

							if (strPropvalue == "base64file")
							{
								obj.STRBASE64IMG = "data:image/png;base64," + strValue;
							}
							else if (strPropvalue == "district")
							{
								obj.DISTRICTNAME = strValue;
							}
							else if (strPropvalue == "dob")
							{
								obj.DATEOFBIRTH = strValue;
							}
							else if (strPropvalue == "name")
							{
								obj.NAME = strValue;
							}
							else if (strPropvalue == "lc")
							{
								obj.MANDALNAME = strValue;
							}
							else if (strPropvalue == "village")
							{
								obj.VILLAGENAME = strValue;
							}
							else if (strPropvalue == "pincode")
							{
								obj.PINCODE = strValue;
							}
							else if (strPropvalue == "gender")
							{
								obj.GENDER = strValue;
							}
						}
					}
					obj.Status = "100";
					obj.Reason = "";

					token_gen.initialize();
					token_gen.expiry_minutes = 60;
					token_gen.addClaim("admin");
					token_gen.PRIMARY_MACHINE_KEY = "10101010101010101010101010101010";
					token_gen.SECONDARY_MACHINE_KEY = "1010101010101010";
					token_gen.addResponse("Status", "100");
					token_gen.addResponse("Details", "");
					token_gen.addResponse("Name", obj.NAME);
					return token_gen.generate_token();

				}
				else if (elemList[2].ChildNodes[0].InnerText == "K-100")
				{
					obj.Status = "102";
					obj.Reason = "Please enter valid OTP number,Try Again!";
				}
				else if (elemList[2].ChildNodes[0].InnerText == "997")
				{
					obj.Status = "102";
					obj.Reason = "Your Aadhaar Number is not linked Properly.";
				}
				else
				{
					obj.Status = "102";
					obj.Reason = "Please enter valid OTP number,Try Again!";
				}
			}
			catch (Exception ex)
			{
				//string mappath = HttpContext.Current.Server.MapPath("CatchExceptionLogs");
				//Task WriteTask = Task.Factory.StartNew(() => logshel.Write_Log_Exception(mappath, "AadhaarVerifyOTPHelper:" + ex.Message));

				obj.Status = "102";
				obj.Reason = ex.Message;
			}
			return obj;
		}
		#region Cumilative Dashboard

		//Sending OTP
		public dynamic GetCumilativeDashboardData_helper(Profilemodel Lobj)
		{
			try
			{
				DataTable dt = GetCumilativeDashboardData_SP(Lobj);

				if (dt != null && dt.Rows.Count > 0)
				{
					_ResObj.Status = 100;
					_ResObj.Reason = "Data Loaded Successfully";
					_ResObj.DataList = dt;
				}
				else
				{
					_ResObj.Status = 102;
					_ResObj.Reason = "Data Loading Failed";
				}
			}
			catch (Exception ex)
			{
				_ResObj.Status = 102;
				_ResObj.Reason = ex.Message;
			}
			return _ResObj;
		}

		#endregion
		#endregion

		#region Incharge Change
		public dynamic InchargeChange_helper(Digitalmodels obj)
		{
			try
			{
				DataTable dt = InchargeChange_data_helper(obj);

				if (dt != null && dt.Rows.Count > 0)
				{
					if (obj.P_TYPE == "1")
					{
						_ResultObj.Status = 100;
						_ResultObj.Reason = "Data Loaded Successfully";
						_ResultObj.DataList = dt;
					}
					else if (obj.P_TYPE == "2")
					{
						var mobile = dt.Rows[0]["MOBILE_NUMBER"].ToString();
						if (!string.IsNullOrEmpty(mobile) && mobile != "0")
						{
							var Password = "";
							int result = SendPassword(mobile, obj.USERTYPE, ref Password);
							if (result == 1)
							{
								_ResultObj.Status = 100;
								_ResultObj.Reason = "Sent Password for Mobile Number Successfully.";
								_ResultObj.DataList = mobile;
								_ResultObj.Password = Password;
							}
							else
							{
								_ResultObj.Status = 102;
								_ResultObj.Reason = "Error Occured while send the password to mobile.";
							}
						}
						else
						{
							_ResultObj.Status = 103;
							_ResultObj.Reason = "Mobile no not exist for selected user. Please update the profile for selected user.";
						}
					}
					else
					{
						_ResultObj.Status = 100;
						_ResultObj.Reason = "Data Inserted Successfully";
						_ResultObj.DataList = dt;
					}
				}
				else
				{
					_ResultObj.Status = 102;
					_ResultObj.Reason = "Data Loading Failed";
				}
			}
			catch (Exception ex)
			{
				_ResultObj.Status = 102;
				_ResultObj.Reason = "Error Occured";
			}
			return _ResultObj;
		}

		public dynamic SaveAffidavitDat(Affidavitmodel objaff)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				bool status = Affidavit_Sp(objaff);
				if (status)
				{
					obj.Status = "100";
					obj.Reason = "Your File Uploaded Successfully";
				}
				else
				{
					obj.Status = "102";
					obj.Reason = "File Not Uploaded.Please Try Again";
				}
			}
			catch (Exception ex)
			{
				obj.Status = "102";
				obj.Reason = ErrMessage;
			}
			return obj;
		}

		#endregion

		public dynamic GetencryptData(RaminfoWalletModel objram)
		{
			dynamic objurl = new ExpandoObject();
			try
			{//distCode=01&gwsCode=11290300&agencyType=F&reqId=121212&userId=User01&password=121212&operatorId=11290300-DA
				string rqval = DateTime.Now.ToString("yyyyMMddHHmmssfff") +new Random().Next(1000, 9999).ToString();
				string inputdata = "distCode=" + objram.distCode + "&gwsCode=" + objram.gwsCode + "&agencyType=F&reqId=" + rqval + "&userId=User01&password=121212&operatorId=" + objram.operatorId;
				string encdata = EncryptText(inputdata, "R@!W@!!et");
				objurl.Status = 100;
				objurl.Returnurl = "http://43.241.39.112:8086/Home/GswsLoginurl?GReqid=" + encdata;
				objurl.Reeason = "";

			}
			catch (Exception ex)
			{
				objurl.Status = 102;
				objurl.Returnurl = "";
				objurl.Reason = ex.Message.ToString();
				return objurl;
			}
			return objurl;
		}

		

		public static string EncryptText(string input, string password)
		{

			// Get the bytes of the string
			byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
			byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

			// Hash the password with SHA256
			passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

			byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

			string result = Convert.ToBase64String(bytesEncrypted);

			return result;
		}

		public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
		{
			byte[] encryptedBytes = null;

			// Set your salt here, change it to meet your flavor:
			// The salt bytes must be at least 8 bytes.
			byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

			using (MemoryStream ms = new MemoryStream())
			{
				using (RijndaelManaged AES = new RijndaelManaged())
				{
					AES.KeySize = 256;
					AES.BlockSize = 128;

					var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
					AES.Key = key.GetBytes(AES.KeySize / 8);
					AES.IV = key.GetBytes(AES.BlockSize / 8);

					AES.Mode = CipherMode.CBC;

					using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
						cs.Close();
					}
					encryptedBytes = ms.ToArray();
				}
			}

			return encryptedBytes;
		}

		//GSWS Dashboard
		public dynamic SecretariatReport_helper(Profilemodel Lobj)
		{
			try
			{
				DataTable dt = SecretariatReport_sphel(Lobj);

				if (dt != null && dt.Rows.Count > 0)
				{
					_ResObj.Status = 100;
					_ResObj.Reason = "Data Loaded Successfully";
					_ResObj.DataList = dt;
				}
				else
				{
					_ResObj.Status = 102;
					_ResObj.Reason = "No Data Found";
				}
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("ORA-12520:"))
				{
					_ResObj.Status = 102;
					_ResObj.Reason = "Server busy.Please try again after sometime";

				}
				else
				{
					_ResObj.Status = 102;
					_ResObj.Reason = ex.Message;
				}
			}
			return _ResObj;
		}


		//GSWS Dashboard
		public dynamic GetCFMSPaymentService(CFMSPAYMENTMODEL Lobj)
		{
			try
			{
				DataTable dt = CFMSGATEWAY_Save_Data_SP(Lobj);

				if (dt != null && dt.Rows.Count > 0 )
				{
					_ResObj.Status = 100;
					_ResObj.Reason = "Data Loaded Successfully";
					_ResObj.DataList = dt;
				}
				else
				{
					_ResObj.Status = 102;
					_ResObj.Reason = "No Data Found";
				}
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("ORA-12520:"))
				{
					_ResObj.Status = 102;
					_ResObj.Reason = "Server busy.Please try again after sometime";

				}
				else
				{
					_ResObj.Status = 102;
					_ResObj.Reason = ex.Message;
				}
			}
			return _ResObj;
		}

		public dynamic GetCFMSPaymentGeneration(CFMSPAYMENTMODEL Lobj)
		{
			dynamic objdata = new ExpandoObject();
			try
			{
				DataTable dt = CFMSGATEWAY_Save_Data_SP(Lobj);

				if (dt != null && dt.Rows.Count > 0)
				{
					RootCfMSResponse objresponse = new RootCfMSResponse();
					var data="";
					try
					{
						 data = cfmsHelper.Send_CFMS_Payment_Response(dt);

						 objresponse = JsonConvert.DeserializeObject<RootCfMSResponse>(data);

					}
					catch (Exception ex)
					{
						List<RootCfMSResponse> list = JsonConvert.DeserializeObject<List<RootCfMSResponse>>(data);
						objresponse.Response.Message=list[0].Response.Message;
						objresponse.Response.DeptTransID = list[0].Response.DeptTransID;
					}
					if (string.IsNullOrEmpty(objresponse.Response.Message))
					{
						serviceRequestModel obj = new serviceRequestModel();

						obj.type = "5";
						obj.transaction_status = objresponse.Response.Transaction_Status;
						obj.challanId = objresponse.Response.CFMS_ID.ToString();
						obj.ifsc_code = objresponse.Response.IFSC_Code;
						obj.valid_upto = objresponse.Response.Valid_Upto.ToString();
						obj.deptTxnId = objresponse.Response.DeptTransID.ToString();
						DataTable dtcfms =cfmsHelper.cfmsChallanProc(obj);
						if (dtcfms != null && dtcfms.Rows.Count > 0 && dtcfms.Rows[0][0].ToString() == "1")
						{
							objdata.status = 100;
							objdata.Reason = objresponse.Response.Message;
							objdata.Returnurl = "https://devcfms.apcfss.in:44300/sap/bc/ui5_ui5/sap/zfi_rcp_cstatus/index.html?sap-client=150&DeptID=" + objresponse.Response.DeptTransID.ToString();
						}
						else
						{

							DataTable dtcfms1 =cfmsHelper.cfmsChallanProc(obj);
							if (dtcfms1 != null && dtcfms1.Rows.Count > 0 && dtcfms1.Rows[0][0].ToString() == "1")
							{
								objdata.status = 100;
								objdata.Reason = objresponse.Response.Message;
								objdata.Returnurl = "https://devcfms.apcfss.in:44300/sap/bc/ui5_ui5/sap/zfi_rcp_cstatus/index.html?sap-client=150&DeptID=" + objresponse.Response.DeptTransID.ToString();
							}
						}

					}
					else
					{
						serviceRequestModel obj1 = new serviceRequestModel();
						if (objresponse.Response.Message.ToString().Contains("already exist"))
						{
							
							obj1.type = "6";
							obj1.transaction_status = "2"; //already updated in cfms						
							obj1.valid_upto = objresponse.Response.Message.ToString();
							obj1.deptTxnId = objresponse.Response.DeptTransID;
							DataTable dtcfms1 = cfmsHelper.cfmsChallanProc(obj1);
							objdata.status = 102;
							objdata.Reason = objresponse.Response.Message;
						}
						else
						{
							obj1.type = "6";
							obj1.transaction_status = "3"; //already updated in cfms						
							obj1.valid_upto = objresponse.Response.Message.ToString();
							obj1.deptTxnId = objresponse.Response.DeptTransID;
							DataTable dtcfms1 = cfmsHelper.cfmsChallanProc(obj1);
							objdata.status = 102;
							objdata.Reason = objresponse.Response.Message;
						}
					}

				}
				else
				{
					_ResObj.Status = 102;
					_ResObj.Reason = "No Data Found";
				}
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("ORA-12520:"))
				{
					_ResObj.Status = 102;
					_ResObj.Reason = "Server busy.Please try again after sometime";

				}
				else
				{
					_ResObj.Status = 102;
					_ResObj.Reason = ex.Message;
				}
			}
			return _ResObj;
		}
	}
}