using gswsBackendAPI.DL.CommonHel;
using gswsBackendAPI.DL.DataConnection;
using gswsBackendAPI.Support;
using gswsBackendAPI.transactionModule;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace gswsBackendAPI.Controllers
{
	[requestFilter]
   [AuthorizationFilter]
	[RoutePrefix("API/GSWSWEB")]
    public class GSWSWebController : ApiController
    {
		ResponseModel _response = new ResponseModel();
		LoginHelper _Loginhel = new LoginHelper();

		ServerSideValidations _valid = new ServerSideValidations();
		Logdatafile logshel = new Logdatafile();


		[HttpPost]
		[Route("Token")]
		public IHttpActionResult Token()
		{
			token_gen.initialize();
			token_gen.expiry_minutes = 60;
			token_gen.addClaim("admin");
			token_gen.PRIMARY_MACHINE_KEY = "10101010101010101010101010101010";
			token_gen.SECONDARY_MACHINE_KEY = "1010101010101010";
			token_gen.addResponse("Status", "100");

			return Ok(token_gen.generate_token());
		}

		[HttpPost]
		[Route("GetRoleAccess")]
		public dynamic GetRoleAccess(dynamic data)
		{
			captchahelper _chel = new captchahelper();
			string jsondata =token_gen.Authorize_aesdecrpty(data);
			try
			{
				captch val = JsonConvert.DeserializeObject<captch>(jsondata);


				return Ok(_chel.GetroleAccess(val));
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}
		[HttpPost]
		[Route("GetCaptcha")]
		public dynamic GetCaptcha(dynamic data)
		{
			
			captchahelper _chel = new captchahelper();
			string jsondata = JsonConvert.SerializeObject(data);
			try
			{
				captch val = JsonConvert.DeserializeObject<captch>(jsondata);


				return Ok(_chel.check_s_captch(val));
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = "Something Went wrong Please Try again";
				return Ok(_response);
			}
		}
		[HttpPost]
		[Route("GetPSLogin")]
		public dynamic GetPSLogin(dynamic data)
		{

			string jsondata = JsonConvert.SerializeObject(data); //token_gen.Authorize_aesdecrpty(data);
			try
			{
				System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;

				//if (headers.Contains("Ftype") || headers.Contains("FUsername") || headers.Contains("Newpassword") || headers.Contains("Captcha") || headers.Contains("ConfirmCaptch"))
				//{
				//	val.Ftype = headers.GetValues("Ftype").First();
				//	val.FUsername = headers.GetValues("FUsername").First();
				//	val.Newpassword = headers.GetValues("Newpassword").First();
				//	val.Captcha = headers.GetValues("Captcha").First();
				//	val.ConfirmCaptch = headers.GetValues("ConfirmCaptch").First();
				//	val.Ftype = headers.GetValues("Ftype").First();

				//	return Ok(_Loginhel.GetLogin(val));
				//}
				string mappath = HttpContext.Current.Server.MapPath("LoginLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, jsondata));

				LoginModel root = JsonConvert.DeserializeObject<LoginModel>(jsondata);
				return Ok(_Loginhel.GetLogin(root));
				
				
				

				//LoginModel val = JsonConvert.DeserializeObject<LoginModel>(jsondata);

			
			//	return "Success";
				
			}
			catch (Exception ex)
			{
				_response.Status =102;
				_response.Reason = _Loginhel.ErrorMessage;
				return Ok(_response);
			}
		}

		[HttpPost]
		[Route("GetPSLogOut")]
		public dynamic GetPSLogOut(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			//string jsondata = JsonConvert.SerializeObject(data); //token_gen.Authorize_aesdecrpty(data);
			try
			{
				HeadertokenModel val = JsonConvert.DeserializeObject<HeadertokenModel>(jsondata);

				return Ok(_Loginhel.SaveToken_Login(val));

			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = _Loginhel.ErrorMessage;
				return Ok(_response);
			}
		}

		[HttpPost]
		[Route("SaveStatusTransaction")]
		public dynamic SaveStatusTransaction(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			try

			{



				StatusTrackingModel val = JsonConvert.DeserializeObject<StatusTrackingModel>(jsondata);

				return Ok(_Loginhel.GetStatusTrackingDetails(val));
				//	return "Success";

			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}
		[HttpPost]
		[Route("GVDashboardData")]
		public dynamic GVDashboardData(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			//string jsondata = JsonConvert.SerializeObject(data);
			try
			{



				LGDMasterModel val = JsonConvert.DeserializeObject<LGDMasterModel>(jsondata);


				return Ok(_Loginhel.getGVDashboard(val));
				//	return "Success";

			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}

		[HttpPost]
		[Route("SideMenuDashboardData")]
		public dynamic SideMenuDashboardData(dynamic data)
		{

			string jsondata = token_gen.Authorize_aesdecrpty(data);
			try
			{


				TransRes val = JsonConvert.DeserializeObject<TransRes>(jsondata);


				return Ok(_Loginhel.GetRegRecDashboard(val));
				//	return "Success";

			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = _Loginhel.ErrorMessage;
				return Ok(_response);
			}
		}


		[HttpPost]
		[Route("GetEncrypt")]
		public dynamic GetEncrypt(dynamic data)
		{

			string jsondata = JsonConvert.SerializeObject(data); //token_gen.Authorize_aesdecrpty(data);
			try
			{
				EncryptDataModel val = JsonConvert.DeserializeObject<EncryptDataModel>(jsondata);

				return Ok(new EncryptDecrypt().Encypt_data(val));
				//	return "SuccessEncryptDataModel

			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}


        [HttpPost]
        [Route("GetAllURLList")]
        public dynamic GetAllURLList(dynamic data)
        {
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			//string jsondata = JsonConvert.SerializeObject(data); //token_gen.Authorize_aesdecrpty(data);
            try
            {
                LGDMasterModel val = JsonConvert.DeserializeObject<LGDMasterModel>(jsondata);

                return Ok(_Loginhel.getAllUrlData(val));
                //	return "SuccessEncryptDataModel

            }
            catch (Exception ex)
            {
                _response.Status = 102;
                _response.Reason = _Loginhel.ErrorMessage;
                return Ok(_response);
            }
        }

        [HttpPost]
        [Route("GetEncryptthirdparty")]
        public dynamic GetEncryptthirdparty(dynamic data)
        {

            string jsondata = token_gen.Authorize_aesdecrpty(data);
			try
            {
				
				//EncryptDataModel val = JsonConvert.DeserializeObject<EncryptDataModel>(jsondata);

				return Ok(new EncryptDecrypt().Encypt_datathird(jsondata));
                //	return "SuccessEncryptDataModel

            }
            catch (Exception ex)
            {
                _response.Status = 102;
                _response.Reason = ex.Message.ToString();
                return Ok(_response);
            }
        }

		[HttpPost]
		[Route("GetEncryparty")]
		public dynamic GetEncryparty(dynamic data)
		{

			string jsondata = JsonConvert.SerializeObject(data); //token_gen.Authorize_aesdecrpty(data);
			try
			{
				//EncryptDataModel val = JsonConvert.DeserializeObject<EncryptDataModel>(jsondata);

				return Ok(new EncryptDecrypt().Encypt_datathird(jsondata));
				//	return "SuccessEncryptDataModel

			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}

		[HttpPost]
		[Route("GetLGDMaster")]
		public dynamic GetLGDMaster(dynamic data)
		{

			string jsondata = token_gen.Authorize_aesdecrpty(data);
			try
			{
				LGDMasterModel val = JsonConvert.DeserializeObject<LGDMasterModel>(jsondata);

				return Ok(_Loginhel.GetLgdmaster(val));
				//	return "SuccessEncryptDataModel

			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}

		[HttpPost]
		[Route("GetGSWSSecretariatMaster")]
		public dynamic GetGSWSSecretariatMaster(dynamic data)
		{

			string jsondata = token_gen.Authorize_aesdecrpty(data);
			try
			{
				LGDMasterModel val = JsonConvert.DeserializeObject<LGDMasterModel>(jsondata);

				return Ok(_Loginhel.GetLgdmaster(val));
				//	return "SuccessEncryptDataModel

			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}

		[HttpPost]
		[Route("GetTransactionIDInitate")]
		public dynamic GetTransactionIDInitate(dynamic data)
		{

			string jsondata = JsonConvert.SerializeObject(data); //token_gen.Authorize_aesdecrpty(data);
			try
			{
				EncryptDataModel val = JsonConvert.DeserializeObject<EncryptDataModel>(jsondata);

				return Ok(new EncryptDecrypt().Encypt_data(val));
				//	return "SuccessEncryptDataModel

			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}
		[HttpPost]
		[Route("GetDecrypt")]
		public dynamic GetDecrypt(dynamic data)
		{

			string jsondata = JsonConvert.SerializeObject(data); //token_gen.Authorize_aesdecrpty(data);
			try
			{
				Decryptdatamodel val = JsonConvert.DeserializeObject<Decryptdatamodel>(jsondata);

				return Ok(new EncryptDecrypt().decypt_data(val));
				//	return "SuccessEncryptDataModel

			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}

		[HttpPost]
		[Route("GetRBEncrypt")]
		public dynamic GetRBEncrypt(dynamic data)
		{

			string jsondata = token_gen.Authorize_aesdecrpty(data);
			try
			{
				LoginModelrb val = JsonConvert.DeserializeObject<LoginModelrb>(jsondata);

				return Ok(new EncryptDecrypt().Encypt_dataRB(val));
				//	return "SuccessEncryptDataModel

			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}


		[HttpPost]
		[Route("apEpdcl_EncryptionData")]
		public dynamic apEpdcl_EncryptionData(dynamic data)
		{

			string jsondata = token_gen.Authorize_aesdecrpty(data);
			try
			{
				apEpdclModel val = JsonConvert.DeserializeObject<apEpdclModel>(jsondata);

				return Ok(new EncryptDecrypt().apEpdcl_Encryption(val));

			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}

		[HttpPost]
		[Route("GetTransactionResponse")]
		public dynamic GetTransactionResponse(dynamic data)
		{

			string jsondata = token_gen.Authorize_aesdecrpty(data);
			//string jsondata = JsonConvert.SerializeObject(data);
			try
			{
				TransRes val = JsonConvert.DeserializeObject<TransRes>(jsondata);

				return Ok(_Loginhel.GetTransResponse_helper(val));

			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}

		[HttpPost]
		[Route("SaveReceiveAction")]
		public dynamic SaveReceiveAction(dynamic data)
		{

			string jsondata = token_gen.Authorize_aesdecrpty(data);
			//string jsondata = JsonConvert.SerializeObject(data);
			try
			{
				TransRes val = JsonConvert.DeserializeObject<TransRes>(jsondata);

				return Ok(_Loginhel.SaveReceiveAction_helper(val));

			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}

		#region Profile Update

		[HttpPost]
		[Route("ProfileSendOTP")]
		public dynamic ProfileSendOTP(dynamic data)
		{

			string jsondata = token_gen.Authorize_aesdecrpty(data);
			try
			{
				Profilemodel val = JsonConvert.DeserializeObject<Profilemodel>(jsondata);
				return Ok(_Loginhel.ProfileSendOTP_helper(val));
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}

		[HttpPost]
		[Route("ProfileVerifyOTP")]
		public dynamic ProfileVerifyOTP(dynamic data)
		{

			string jsondata = token_gen.Authorize_aesdecrpty(data);
			try
			{
				Profilemodel val = JsonConvert.DeserializeObject<Profilemodel>(jsondata);
				return Ok(_Loginhel.ProfileVerifyOTP_helper(val));
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}

		//Update Profile
		[HttpPost]
		[Route("ProfileUpdate")]
		public dynamic ProfileUpdate(dynamic data)
		{

			string jsondata = token_gen.Authorize_aesdecrpty(data);
			try
			{
				Profilemodel val = JsonConvert.DeserializeObject<Profilemodel>(jsondata);
				var validresult = _valid.CheckPasswordValidation(val);
				if (validresult.Status == "Success")
				{
					return Ok(_Loginhel.ProfileUpdate_helper(val));
				}
				else
				{
					_response.Status = 102;
					_response.Reason = validresult.Reason;
					return Ok(_response);
				}
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}

		#endregion

		#region Forgot Password

		//Send OTP
		[HttpPost]
		[Route("ForgotpasswordsendOTP")]
		public dynamic ForgotpasswordsendOTP(dynamic data)
		{
			string jsondata = JsonConvert.SerializeObject(data);
			try
			{
				LoginModel val = JsonConvert.DeserializeObject<LoginModel>(jsondata);
				return Ok(_Loginhel.ForgotpasswordsendOTP_helper(val));
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}

		//Verify OTP
		[HttpPost]
		[Route("ForgotpasswordVerifyOTP")]
		public dynamic ForgotpasswordVerifyOTP(dynamic data)
		{

			string jsondata = JsonConvert.SerializeObject(data);
			try
			{
				Profilemodel val = JsonConvert.DeserializeObject<Profilemodel>(jsondata);
				return Ok(_Loginhel.ProfileVerifyOTP_helper(val));
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}

		//Update Password
		[HttpPost]
		[Route("ForgotpasswordUpdate")]
		public dynamic ForgotpasswordUpdate(dynamic data)
		{

			string jsondata = JsonConvert.SerializeObject(data);
			try
			{
				Profilemodel val = JsonConvert.DeserializeObject<Profilemodel>(jsondata);
				var validresult = _valid.CheckPasswordValidation(val);
				if (validresult.Status == "Success")
				{
					return Ok(_Loginhel.ProfileUpdate_helper(val));
				}
				else
				{
					_response.Status = 102;
					_response.Reason = validresult.Reason;
					return Ok(_response);
				}
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}
		#endregion

		#region Aadhaar OTP

		//Aadhaar Send OTP
		[HttpPost]
		[Route("AadhaarLogin")]
		public IHttpActionResult AadhaarLogin(dynamic data)
		{

			try
			{
				string jsondata = JsonConvert.SerializeObject(data);
				AadharOTP rootobj = JsonConvert.DeserializeObject<AadharOTP>(jsondata);

				//string methodname = "SendOTP/" + rootobj.AADHAR;
				//string myIP = string.Empty;

				//myIP = HttpContext.Current.Request.UserHostAddress;
				//logshel.Write_Log_Exception("SendOtpLogs", "IP Address:" + myIP + "Aadhaar:" + rootobj.AADHAR);

				return Ok(_Loginhel.AadhaarSendOTP_helper(rootobj.AADHAR));
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}

		//Verify OTP
		[HttpPost]
		[Route("AadhaarVerifyOTP")]
		public IHttpActionResult AadhaarVerifyOTP(dynamic data)
		{
			try
			{
				string jsondata = JsonConvert.SerializeObject(data);
				AadharOTP rootobj = JsonConvert.DeserializeObject<AadharOTP>(jsondata);

				//string methodname = "VerifyOTP/" + rootobj.AADHAR + "/" + rootobj.OTP;
				//string myIP = string.Empty;

				//myIP = HttpContext.Current.Request.UserHostAddress;
				//logshel.Write_Log_Exception("VerifyOtpLogs", "IP Address:" + myIP + "Aadhaar:" + rootobj.AADHAR);

				return Ok(_Loginhel.AadhaarVerifyOTP_helper(rootobj.AADHAR, rootobj.OTP));
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message;
				return Ok(_response);
			}

		}

		#region Cumilative Dashboard

		//Send OTP
		[HttpPost]
		[Route("GetCumilativeDashboardData")]
		public dynamic GetCumilativeDashboardData(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			try
			{
				Profilemodel val = JsonConvert.DeserializeObject<Profilemodel>(jsondata);
				return Ok(_Loginhel.GetCumilativeDashboardData_helper(val));
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}

		[HttpPost]
		[Route("NavaSakamaData")]
		public dynamic NavaSakamaData(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			try
			{
				Navakasammodel val = JsonConvert.DeserializeObject<Navakasammodel>(jsondata);
				return Ok(_Loginhel.GetNavasakamToken(val));
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}

		// Wallet Api Intergration

		[HttpPost]
		[Route("WalletRecharge")]
		public dynamic WalletRecharge(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			try
			{
				RaminfoWalletModel val = JsonConvert.DeserializeObject<RaminfoWalletModel>(jsondata);
				return Ok(_Loginhel.GetencryptData(val));
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}
		[HttpPost]
		[Route("WalletAmount")]
		public dynamic WalletAmount(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			try
			{
				WalletAmountmodel val = JsonConvert.DeserializeObject<WalletAmountmodel>(jsondata);
				return Ok(_Loginhel.GetWalletAmount(val));
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}
		#endregion
		#endregion

		#region Incharge Change
		[HttpPost]
		[Route("InchargeChangeAPI")]
		public dynamic InchargeChangeAPI(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			try
			{
				Digitalmodels val = JsonConvert.DeserializeObject<Digitalmodels>(jsondata);
				return Ok(_Loginhel.InchargeChange_helper(val));
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = "Error Occured";
				return Ok(_response);
			}
		}
		[HttpPost]
		[Route("AffidavitPost")]
		public dynamic AffidavitPost(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			try
			{
				Affidavitmodel val = JsonConvert.DeserializeObject<Affidavitmodel>(jsondata);
				return Ok(_Loginhel.SaveAffidavitDat(val));
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = "Error Occured";
				return Ok(_response);
			}
		}
		//22apr2020
		[HttpPost]
		[Route("SaveSecretariatForm")]
		public dynamic SaveSecretariatForm(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			try
			{
				string mappath = HttpContext.Current.Server.MapPath("SaveSecretariatFormLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, jsondata));

				//_response.Status = 100;
				//_response.Reason = "Data Submitted Successfully";
				//return Ok(_response);
				SecretariatFormObject val = JsonConvert.DeserializeObject<SecretariatFormObject>(jsondata);
				
				return Ok(_Loginhel.GetSaveSeccformDat(val));
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}

		[HttpPost]
		[Route("SaveValunteerForm")]
		public dynamic SaveValunteerForm(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			try
			{
				string mappath = HttpContext.Current.Server.MapPath("SaveValunteerFormLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, jsondata));

				VolunteerFormObject val = JsonConvert.DeserializeObject<VolunteerFormObject>(jsondata);

				return Ok(_Loginhel.GetVolunteerFormData(val));
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}

		[HttpPost]
		[Route("UpdateMailMobileForm")]
		public dynamic UpdateMailMobileForm(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			try
			{
				string mappath = HttpContext.Current.Server.MapPath("UpdateMailMobileFormLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, jsondata));

				LoginMailModel val = JsonConvert.DeserializeObject<LoginMailModel>(jsondata);

				return Ok(_Loginhel.GetGeoTrackingMailUpdate(val));
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}

		[HttpPost]
		[Route("GetSecretariatVolunteerData")]
		public dynamic GetSecretariatVolunteerData(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				LGDMasterModel val = JsonConvert.DeserializeObject<LGDMasterModel>(jsondata);

				return Ok(_Loginhel.GetSeccVolunteerData(val));
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}

		[HttpPost]
		[Route("PasswordSha")]
		public dynamic PasswordSha(dynamic data)
		{
			
				string jsondata = JsonConvert.SerializeObject(data);
				try
				{
					
						return Ok(LoginHelper.ComputeSha256Hash("GSWS@7979"));
					
				}
				catch (Exception ex)
				{
					_response.Status = 102;
					_response.Reason = ex.Message.ToString();
					return Ok(_response);
				}
			
		}
		#endregion

		[HttpPost]
		[Route("GetCountsData")]
		public dynamic GetCountsData(dynamic data)
		{

			captchahelper _chel = new captchahelper();
			string jsondata = JsonConvert.SerializeObject(data);
			try
			{
				LoginModel val = JsonConvert.DeserializeObject<LoginModel>(jsondata);
				return Ok(_Loginhel.GetCountsData_Helper(val));
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = "Something Went wrong Please Try again";
				return Ok(_response);
			}
		}

		#region Secretariate DashBoard
		[HttpPost]
		[Route("GetSecretariatReport")]
		public dynamic GetSecretariatReport(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			//string jsondata = JsonConvert.SerializeObject(data);
			try
			{
				Profilemodel val = JsonConvert.DeserializeObject<Profilemodel>(jsondata);
				return Ok(_Loginhel.SecretariatReport_helper(val));
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}

		[HttpPost]
		[Route("GetCFMSPaymentService")]
		public dynamic GetCFMSPaymentService(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			//string jsondata = JsonConvert.SerializeObject(data);
			try
			{
				CFMSPAYMENTMODEL val = JsonConvert.DeserializeObject<CFMSPAYMENTMODEL>(jsondata);
				return Ok(_Loginhel.GetCFMSPaymentService(val));
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}

		[HttpPost]
		[Route("GetCFMSPaymentGeneration")]
		public dynamic GetCFMSPaymentGeneration(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			//string jsondata = JsonConvert.SerializeObject(data);
			try
			{
				CFMSPAYMENTMODEL val = JsonConvert.DeserializeObject<CFMSPAYMENTMODEL>(jsondata);
				return Ok(_Loginhel.GetCFMSPaymentGeneration(val));
			}
			catch (Exception ex)
			{
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}
		#endregion
	}
}
