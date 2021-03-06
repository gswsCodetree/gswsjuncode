﻿using gswsBackendAPI.DL.CommonHel;
using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace gswsBackendAPI.Depts.AgriCulture
{
	public class Helper : CommonSPHel
	{
		OracleCommand cmd;

        //public object DepartmentEnum { get; private set; }

        //CommonSPHel _comhel = new CommonSPHel();
        public DataTable DemoAPI(DemoModel objrb)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "RB_SP_OWNERS_DETAILS";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2, 12).Value = objrb.Ftype;
				cmd.Parameters.Add("P_DISTRICT", OracleDbType.Varchar2, 20).Value = objrb.FDistrict;
				cmd.Parameters.Add("P_MANDAL", OracleDbType.Varchar2, 20).Value = objrb.FMandal;
				cmd.Parameters.Add("P_VILLAGE", OracleDbType.Varchar2, 20).Value = objrb.FVillage;
				cmd.Parameters.Add("P_KATHA_NO", OracleDbType.Varchar2, 20).Value = objrb.FKathano;
				cmd.Parameters.Add("P_UNIQUE_ID", OracleDbType.Varchar2, 50).Value = objrb.FUID;
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtApproval = GetspsDataAdapter(cmd);
				if (dtApproval != null && dtApproval.Rows.Count > 0)
				{
					return dtApproval;
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("MandalExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetMandaApprovalReport_Sp:" + ex.Message.ToString()));
				throw ex;//throw ex;
			}

		}

		public dynamic GetSeedGroupsData(dynamic Objddata)
		{
			dynamic objectData = new ExpandoObject();
			try
			{
				var data = PostData("https://eseed.ap.gov.in/eseed/RestFul/VSServices/getSeedGroup", Objddata);

				objectData.Status = "Success";
				objectData.Reason = "";
				var objdata = JsonConvert.DeserializeObject<dynamic>(data);
				objectData.Data = objdata.response.rtgsSeedGroupList;
			}
			catch (Exception ex)
			{

				ExceptionDataModel objex = new ExceptionDataModel();
				string mappath = HttpContext.Current.Server.MapPath("AgricultureExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "GetSeedGroupsData Service:" + ex.Message.ToString()));
				Common_Agriculture_Error(ex.Message.ToString(), "https://eseed.ap.gov.in/eseed/RestFul/VSServices/getSeedGroup", "2");

				objectData.Status = "Failure";
			
				objectData.Reason = ThirdpartyMessage; //ex.Message.ToString();
				objectData.Data = "";
			}
			return objectData;
		}

		public dynamic GetSeedGroupsVarietyData(dynamic Objddata)
		{
			dynamic objectData = new ExpandoObject();
			try
			{
				var data = PostData("https://eseed.ap.gov.in/eseed/RestFul/VSServices/getSeedVarietiesByGroup", Objddata);

				objectData.Status = "Success";
				objectData.Reason = "";
				var objdata = JsonConvert.DeserializeObject<dynamic>(data);
				objectData.Data = objdata.response.rtgsSeedGroupList;
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("AgricultureExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "GetSeedGroupsVarietyData Service:" + ex.Message.ToString()));
				Common_Agriculture_Error(ex.Message.ToString(), "https://eseed.ap.gov.in/eseed/RestFul/VSServices/getSeedVarietiesByGroup", "2");
				objectData.Status = "Failure";
				objectData.Reason = ThirdpartyMessage;
				objectData.Data = "";
			}
			return objectData;
		}

		public dynamic GetSeedVarietesData(dynamic Objddata)
		{
			dynamic objectData = new ExpandoObject();
			try
			{
				var data = PostData("https://eseed.ap.gov.in/eseed/RestFul/VSServices/getSeedVarietiesByGroup", Objddata);

				objectData.Status = "Success";
				objectData.Reason = "";
				var objdata = JsonConvert.DeserializeObject<dynamic>(data);
				objectData.Data = objdata.response.rtgsSeedVarietylist;

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("AgricultureExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "GetSeedVarietesData Service:" + ex.Message.ToString()));
				Common_Agriculture_Error(ex.Message.ToString(), "https://eseed.ap.gov.in/eseed/RestFul/VSServices/getSeedVarietiesByGroup", "2");
				objectData.Status = "Failure";
				objectData.Reason = ThirdpartyMessage;
				objectData.Data = "";
			}
			return objectData;
		}

		public dynamic GetSeedVarietesResponse(dynamic Objddata)
		{
			dynamic objectData = new ExpandoObject();
			try
			{
				var data = PostData("https://eseed.ap.gov.in/eseed/RestFul/VSServices/getSeedEligibility", Objddata);

				objectData.Status = "Success";
				objectData.Reason = "";
				var objdata = JsonConvert.DeserializeObject<dynamic>(data);
				objectData.Data = objdata.response;

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("AgricultureExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "GetSeedVarietesResponse Service:" + ex.Message.ToString()));
				Common_Agriculture_Error(ex.Message.ToString(), "https://eseed.ap.gov.in/eseed/RestFul/VSServices/getSeedEligibility", "2");
				objectData.Status = "Failure";
				objectData.Reason = ThirdpartyMessage;
				objectData.Data = "";
			}
			return objectData;
		}
		public dynamic GetEligileBeneficiariesResponse(dynamic Objddata)
		{
			dynamic objectData = new ExpandoObject();
			try
			{
				var data = PostData("https://eseed.ap.gov.in/eseed/RestFul/VSServices/getbenfDetailsVillagewise", Objddata);

				objectData.Status = "Success";
				objectData.Reason = "";
				var objdata = JsonConvert.DeserializeObject<dynamic>(data);
				objectData.Data = objdata.response;

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("AgricultureExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "GetEligileBeneficiariesResponse Service:" + ex.Message.ToString()));
				Common_Agriculture_Error(ex.Message.ToString(), "https://eseed.ap.gov.in/eseed/RestFul/VSServices/getbenfDetailsVillagewise", "2");
				objectData.Status = "Failure";
				objectData.Reason =ThirdpartyMessage;
				objectData.Data = "";
			}
			return objectData;
		}

		public dynamic GetVAADetailsResponse(dynamic Objddata)
		{
			dynamic objectData = new ExpandoObject();
			try
			{
				var data = PostData("https://eseed.ap.gov.in/eseed/RestFul/VSServices/getVAADetailsMandalWise", Objddata);

				objectData.Status = "Success";
				objectData.Reason = "";
				var objdata = JsonConvert.DeserializeObject<dynamic>(data);
				objectData.Data = objdata.response;

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("AgricultureExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "GetVAADetailsResponse Service:" + ex.Message.ToString()));
				Common_Agriculture_Error(ex.Message.ToString(), "https://eseed.ap.gov.in/eseed/RestFul/VSServices/getVAADetailsMandalWise", "2");
				objectData.Status = "Failure";
				objectData.Reason = ThirdpartyMessage;
				objectData.Data = "";
			}
			return objectData;
		}

		public dynamic GetSeasonFinancialYear(dynamic Objddata)
		{
			dynamic objectData = new ExpandoObject();
			try
			{
				var data = PostData("https://eseed.ap.gov.in/eseed/RestFul/VSServices/getSeasonCrp", Objddata);
				objectData.Status = "Success";
				objectData.Reason = "";
				var objdata = JsonConvert.DeserializeObject<dynamic>(data);
				objectData.Data = objdata.response.rtgsActiveSeasonList;
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("AgricultureExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "GetSeasonFinancialYear Service:" + ex.Message.ToString()));

				objectData.Status = "Failure";
				objectData.Reason = ThirdpartyMessage;
				objectData.Data = "";
			}
			return objectData;
		}

		#region EPanta

		public dynamic GetVillgeData(EPantaCls oj)
		{
			dynamic obj = new ExpandoObject();

			try
			{
				EPantaService.getVillageData vildata = new EPantaService.getVillageData();
				var data = vildata.CallgetVillageData("user_adrt", "user@rtgs7", oj.District, oj.Mandal, oj.Village);
				dynamic objroot = JsonConvert.DeserializeObject<dynamic>(data);

				if (objroot != null)
				{
					obj.Status = 100;
					obj.Reason = "Data Getting Successfully.";
					obj.Details = objroot;
				}
				else
				{
					obj.Status = 101;
					obj.Reason = "No Data Found";
				}
			}
			catch (WebException wex)
			{
				string mappath = HttpContext.Current.Server.MapPath("EPantaExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error Gettting EPanta Data of Village API:" + wex.Message.ToString()));

				obj.Status = 102;
				obj.Reason =ThirdpartyMessage;
			}

			return obj;
		}

		public dynamic GetVillageDetailsByServey(EPantaCls oj)
		{
			dynamic obj = new ExpandoObject();

			try
			{
                if (Utils.IsAlphaNumeric(oj.SurveyNo))
                {
					EPantaService.getVillageData vildata = new EPantaService.getVillageData();
                    var data = vildata.getVillageSurveyNoData("user_adrt", "user@rtgs7", oj.District, oj.Mandal, oj.Village, oj.SurveyNo);
                    dynamic objroot = JsonConvert.DeserializeObject<dynamic>(data);

                    if (objroot != null)
                    {
                        obj.Status = 100;
                        obj.Reason = "Data Getting Successfully.";
                        obj.Details = objroot;
                    }
                    else
                    {
                        obj.Status = 101;
                        obj.Reason = "No Data Found";
                    }
                }
                else
                {
                    obj.Status = 102;
                    obj.Reason = "Error Occured While Getting Village Data.";
                }
			}
			catch (WebException wex)
			{
				string mappath = HttpContext.Current.Server.MapPath("EPantaExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error Gettting EPanta Data of Village API:" + wex.Message.ToString()));

				obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
			}

			return obj;
		}

		public dynamic GetVillageDetailsByAadhaar(EPantaCls oj)
		{
			dynamic obj = new ExpandoObject();

			try
			{
                if (Utils.IsAlphaNumeric(oj.SurveyNo))
                {
                    EPantaService.getVillageData vildata = new EPantaService.getVillageData();
                    var data = vildata.getVillageAadharData("user_adrt", "user@rtgs7", oj.District, oj.Mandal, oj.Village, oj.Aadhar);
                    dynamic objroot = JsonConvert.DeserializeObject<dynamic>(data);

                    if (objroot != null)
                    {
                        obj.Status = 100;
                        obj.Reason = "Data Getting Successfully.";
                        obj.Details = objroot;
                    }
                    else
                    {
                        obj.Status = 101;
                        obj.Reason = "No Data Found";
                    }
                }
                else
                {
                    obj.Status = 102;
                    obj.Reason = "Error Occured While Getting Village Data.";
                }
            }
			catch (WebException wex)
			{
				string mappath = HttpContext.Current.Server.MapPath("EPantaLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error Gettting EPanta Data of Village API:" + wex.Message.ToString()));

				obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
			}

			return obj;
		}

		#endregion



		#region Consuming

		public dynamic PostData(string url, dynamic jsonData)
		{
			var response = String.Empty;
			try
			{
				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				var req = (HttpWebRequest)WebRequest.Create(url);
				req.Credentials = CredentialCache.DefaultCredentials;
				WebProxy myProxy = new WebProxy();
				//myProxy.Address = new Uri("http://proxy.uk.research-int.com:8080"); ;
				//myProxy.UseDefaultCredentials = true;
				req.Proxy = myProxy;

				req.Method = "POST";

				var _jsonObject = JsonConvert.SerializeObject(jsonData);

				//If there is any json data
				if (!String.IsNullOrEmpty(_jsonObject))
				{
					using (System.IO.Stream s = req.GetRequestStream())
					{
						using (System.IO.StreamWriter sw = new System.IO.StreamWriter(s))
							sw.Write(_jsonObject);
					}
				}


				req.ContentType = "application/json; charset=utf-8";
				req.AllowAutoRedirect = false;

				var resp = (HttpWebResponse)req.GetResponse();
				var sr = new StreamReader(resp.GetResponseStream());

				if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
					(resp.StatusCode == HttpStatusCode.RedirectMethod))
				{
					// response = GetData(resp.Headers["Location"]);
				}
				else
				{
					response = sr.ReadToEnd().Trim();
				}


			}
			catch (WebException wex)
			{
				//	Logfile_eX("VERIFYOTP ELIGIBILITY CHECK SERVICE EXCEPTION::", wex.Message);
				throw new Exception(wex.Message);
			}

			return response;
		}

		#endregion

		#region Farmer Mechanization
		public dynamic GetFMAppStatus(FMAppSts oj)
		{
			dynamic obj = new ExpandoObject();
			APITRACKMODEL objapi = new APITRACKMODEL();
			try
			{

				objapi.DeptId = oj.UrlId.Substring(0, 2);
				objapi.HODId = oj.UrlId.Substring(2,4);
				objapi.UrlId = oj.UrlId;
				objapi.DistrictCode = oj.DistrictCode;
				objapi.MandalCode = oj.MandalCode;
				objapi.SceretriatCode = oj.SceretriatCode;
				objapi.Ptype = "1";
				objapi.Loginid = oj.Loginid;
				objapi.InputData = oj.Application;
				objapi.TrackingId = oj.SceretriatCode + DateTime.Now.ToString("yymmddHHmm") + new Random().Next(1000, 9999);
				

				    
				if (Utils.IsAlphaNumeric(oj.Application))
				{
					var val = PostFMData("https://agrimachinery.nic.in/api/services/GetApplications?Application=" + oj.Application + "&Key='APagri1234'", "");
					var data = GetSerialzedData<dynamic>(val);
					objapi.Status = "1";
					objapi.Remarks = data.AplicationStatus + "," + data.ReasonforRejection;
					new LoginSPHelper().APITRacking_SP(objapi);
					obj.Status = 100;
					obj.Reason = "Data Getting Successfully.";
					obj.Details = data;
				}
				else
				{
					obj.Status = 102;
					obj.Reason = "Special Characters Are Not Allowed.";
				}


			}
			catch (WebException wex)
			{
				string mappath = HttpContext.Current.Server.MapPath("FormerMechanizationExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error Gettting FormerMechanization Status Data API:" + wex.Message.ToString()));
				objapi.Status = "2";
				objapi.Remarks = wex.Message.ToString();
				new LoginSPHelper().APITRacking_SP(objapi);
				obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
			}

			return obj;
		}

		public dynamic FMAppRegister(FMReg oj)
		{
			dynamic obj = new ExpandoObject();

			try
			{
				var val = PostFMData("https://agrimachinery.nic.in/api/FarmerRegistration/Regis?value={StateCode:28,DistrictCode:'" + oj.DistrictCode + "',BlockCode:'" + oj.BlockCode + "',SubDistrictCode:'" + oj.SubDistrictCode + "',PanchayatCode:'" + oj.PanchayatCode + "',VillageCode:'" + oj.VillageCode + "',AadharNo:'" + oj.AadharNo + "',MobileNo:'" + oj.MobileNo + "',FarmerName:'" + oj.FarmerName + "',FatherHusbandName:'" + oj.FatherHusbandName + "',DOB:'" + oj.DOB + "',Gender:'" + oj.Gender + "',CasteCategory:'" + oj.CasteCategory + "',FarmerType:'" + oj.FarmerType + "',Phone:'" + oj.Phone + "',EmailId:'" + oj.EmailId + "',PinCode:'" + oj.PinCode + "',Address:'" + oj.Address + "',UserID:'" + oj.UserID + "',Password:'" + oj.Password + "',AadharConcent:true,PAN:'" + oj.PAN + "',CentralUnique_BenID:'" + oj.CentralUnique_BenID + "',Key:'APagri1234'}", "");
				var data = GetSerialzedData<dynamic>(val);

				obj.Status = 100;
				obj.Reason = "Data Submitted Successfully.";
				obj.Details = data;
			}
			catch (WebException wex)
			{
				string mappath = HttpContext.Current.Server.MapPath("FormerMechanizationExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error Submitting FormerMechanization Data API:" + wex.Message.ToString()));

				obj.Status = 102;
				obj.Reason = "Error Occured While Submitting Data.";
			}

			return obj;
		}

		public dynamic PostFMData(string url, dynamic jsonData)
		{
			var response = String.Empty;
			try
			{
				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				var req = (HttpWebRequest)WebRequest.Create(url);
				req.Credentials = CredentialCache.DefaultCredentials;
				WebProxy myProxy = new WebProxy();
				//myProxy.Address = new Uri("http://proxy.uk.research-int.com:8080"); ;
				//myProxy.UseDefaultCredentials = true;
				req.Proxy = myProxy;

				req.Method = "POST";

				var _jsonObject = JsonConvert.SerializeObject(jsonData);

				//If there is any json data
				if (!String.IsNullOrEmpty(_jsonObject))
				{
					using (System.IO.Stream s = req.GetRequestStream())
					{
						using (System.IO.StreamWriter sw = new System.IO.StreamWriter(s))
							sw.Write(_jsonObject);
					}
				}


				req.ContentType = "application/json; charset=utf-8";
				req.AllowAutoRedirect = false;

				var resp = (HttpWebResponse)req.GetResponse();
				var sr = new StreamReader(resp.GetResponseStream());

				if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
					(resp.StatusCode == HttpStatusCode.RedirectMethod))
				{
					// response = GetData(resp.Headers["Location"]);
				}
				else
				{
					response = sr.ReadToEnd().Trim();
				}

				string mappath = HttpContext.Current.Server.MapPath("FormerMechanizationSaveLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "FormerMechanization Save Data :" + _jsonObject));

				string mappath1 = HttpContext.Current.Server.MapPath("FormerMechanizationResponseLogs");
				Task WriteTask1 = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "FormerMechanization Response Data :" + response));

			}
			catch (WebException wex)
			{
				string mappath = HttpContext.Current.Server.MapPath("FormerMechanizationExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error FormerMechanization Data API:" + wex.Message.ToString()));
				//	Logfile_eX("VERIFYOTP ELIGIBILITY CHECK SERVICE EXCEPTION::", wex.Message);
				throw wex;
			}

			return response;
		}

		public T GetSerialzedData<T>(string Input)
		{
			return JsonConvert.DeserializeObject<T>(Input);
		}

		public bool Common_Agriculture_Error(string msg,string url,string etype)
		{
			ExceptionDataModel objex = new ExceptionDataModel();	
			try
			{
                objex.E_DEPTID = DepartmentEnum.Department.Agriculture_and_Marketing.ToString();
				objex.E_HODID = DepartmentEnum.HOD.Agriculture.ToString();
				objex.E_ERRORMESSAGE = msg;
				objex.E_SERVICEAPIURL = url;
				objex.E_ERRORTYPE = etype;				
				new LoginSPHelper().Save_Exception_Data(objex);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		#endregion

		#region RBStatus


		public dynamic RBStatus(dynamic objdata)
		{
			dynamic objResponse = new ExpandoObject();
			try
			{
				//string Apidata = objdata.data;
				//string Apid = objdata.last;
				string captcha = objdata.Captcha;
				string Confirm = objdata.ConfirmCaptch;
				string captchaverify = new captchahelper().CaptchVerify(captcha, Confirm);
				if (captchaverify == "Success")
				{
					using (var client = new HttpClient())
					{
						ServicePointManager.Expect100Continue = true;
						ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

						System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

						var req = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["AgricultureApiBaseAddress"].ToString() + "Agriculture/v1/api/v1/agriculture");
						req.ContentType = "application/json";
						req.Method = "POST";


						req.Headers.Add("Authorization", "Bearer 7d1a2c9a-72d2-3b6e-8663-e91f4e58d578");

						req.AllowAutoRedirect = false;
						string str = JsonConvert.SerializeObject(objdata);

						str = str.Replace("null", "\"\"");
						var _jsonObject = str;
						if (!String.IsNullOrEmpty(_jsonObject))
						{
							using (System.IO.Stream s = req.GetRequestStream())
							{
								using (System.IO.StreamWriter sw = new System.IO.StreamWriter(s))
									sw.Write(_jsonObject);
							}
						}

						var resp = (HttpWebResponse)req.GetResponse();
						var sr = new StreamReader(resp.GetResponseStream());

						if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
							(resp.StatusCode == HttpStatusCode.RedirectMethod))
						{
							objResponse.Status = "Failed";
							objResponse.data = "";
						}
						else
						{
							string data = sr.ReadToEnd().Trim();
							dynamic RBData = JsonConvert.DeserializeObject<dynamic>(data);
							JArray userInfo = RBData.Details;
							if (userInfo != null)
							{
								string status = userInfo[0]["STATUS"].ToString();
								if (status == "Belong To Beneficiary Family")
								{
									objResponse.REASON1 = BeneficiaryStatus(objdata);
								}
							}

							objResponse.Status = "100";
							objResponse.REASON = data;
						}


					}
				}
				else
				{
					objResponse.Status = 102;
					objResponse.REASON = "Invalid Captcha";
				}
			}
			catch (WebException ex)
			{
				objResponse.Status = "Failed";
				objResponse.REASON =ex.Message.ToString();
			}
			return objResponse;
		}

		public dynamic BeneficiaryStatus(dynamic objdata)
		{
			dynamic objResponse = new ExpandoObject();

			string jsondata = JsonConvert.SerializeObject(objdata);

			dynamic val = JsonConvert.DeserializeObject<dynamic>(jsondata);
			string dt1 = val.data;
			// JObject Apidata = val.data;
			Data OBJ1 = JsonConvert.DeserializeObject<Data>(dt1);
			//JObject Apidata = val.data;
			string Ptype = "2";
			string UId = OBJ1.PUIDNUM; //Apidata["PUIDNUM"].ToString();
			Data obj = new Data();
			obj.PTYPE = Ptype;
			obj.PUIDNUM = UId;
			string objjson = JsonConvert.SerializeObject(obj);
			val.data = objjson;


			try
			{

				using (var client = new HttpClient())
				{
					ServicePointManager.Expect100Continue = true;
					ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

					System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

					var req = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["AgricultureApiBaseAddress"].ToString() + "Agriculture/v1/api/v1/agriculture");
					req.ContentType = "application/json";
					req.Method = "POST";


					req.Headers.Add("Authorization", "Bearer 7d1a2c9a-72d2-3b6e-8663-e91f4e58d578");

					req.AllowAutoRedirect = false;
					string str = JsonConvert.SerializeObject(val);

					str = str.Replace("null", "\"\"");
					var _jsonObject = str;
					if (!String.IsNullOrEmpty(_jsonObject))
					{
						using (System.IO.Stream s = req.GetRequestStream())
						{
							using (System.IO.StreamWriter sw = new System.IO.StreamWriter(s))
								sw.Write(_jsonObject);
						}
					}

					var resp = (HttpWebResponse)req.GetResponse();
					var sr = new StreamReader(resp.GetResponseStream());

					if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
						(resp.StatusCode == HttpStatusCode.RedirectMethod))
					{
						
						objResponse.Status = "Failed";
						objResponse.data = "";
					}
					else
					{
						
						string data = sr.ReadToEnd().Trim();

						objResponse.Status = "100";
						objResponse.REASON = data;
					}


				}

			}
			catch (WebException ex)
			{
				objResponse.Status = "Failed";
				objResponse.REASON = CommonSPHel.ThirdpartyMessage;
			}
			return objResponse;
		}

		#endregion
		#region CropDetails

		public static string ComputeSha256Hash(string rawData)
		{
			//_log.Info("In the LoginHelper=>ComputeSha256Hash=>" + rawData);
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


		public dynamic CropDetails(dynamic objdata)
		{
			dynamic objResponse = new ExpandoObject();
			try
			{

				using (var client = new HttpClient())
				{
					ServicePointManager.Expect100Continue = true;
					ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

					System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

					var req = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["AgricultureApiBaseAddress"].ToString() + "Agriculture/v1/api/v1/agriculture");
					req.ContentType = "application/json";
					req.Method = "POST";


					req.Headers.Add("Authorization", "Bearer 7d1a2c9a-72d2-3b6e-8663-e91f4e58d578");

					req.AllowAutoRedirect = false;
					string str = JsonConvert.SerializeObject(objdata);

					str = str.Replace("null", "\"\"");
					var _jsonObject = str;
					if (!String.IsNullOrEmpty(_jsonObject))
					{
						using (System.IO.Stream s = req.GetRequestStream())
						{
							using (System.IO.StreamWriter sw = new System.IO.StreamWriter(s))
								sw.Write(_jsonObject);
						}
					}

					var resp = (HttpWebResponse)req.GetResponse();
					var sr = new StreamReader(resp.GetResponseStream());

					if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
						(resp.StatusCode == HttpStatusCode.RedirectMethod))
					{
						// _log.Info("In the PRRDHelper=>ConfirmDemand: Failed");
						objResponse.Status = "Failed";
						objResponse.data = "";
					}
					else
					{
						//_log.Info("In the Helper=>ConfirmDemand: Success");
						string data = sr.ReadToEnd().Trim();

						objResponse.Status = "100";
						objResponse.REASON = data;
					}


				}

			}
			catch (WebException ex)
			{
				objResponse.Status = "Failed";
				objResponse.REASON = CommonSPHel.ThirdpartyMessage;
			}
			return objResponse;
		}

		public dynamic GetAgrimaster(dynamic objdata)
		{
			dynamic objResponse = new ExpandoObject();
			try
			{
				using (var client = new HttpClient())
				{
					ServicePointManager.Expect100Continue = true;
					ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

					System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

					var req = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["AgricultureApiBaseAddress"].ToString() + "Agriculture/v1/api/v1/agriculture");
					req.ContentType = "application/json";
					req.Method = "POST";


					req.Headers.Add("Authorization", "Bearer 7d1a2c9a-72d2-3b6e-8663-e91f4e58d578");

					req.AllowAutoRedirect = false;
					string str = JsonConvert.SerializeObject(objdata);

					str = str.Replace("null", "\"\"");
					var _jsonObject = str;
					if (!String.IsNullOrEmpty(_jsonObject))
					{
						using (System.IO.Stream s = req.GetRequestStream())
						{
							using (System.IO.StreamWriter sw = new System.IO.StreamWriter(s))
								sw.Write(_jsonObject);
						}
					}

					var resp = (HttpWebResponse)req.GetResponse();
					var sr = new StreamReader(resp.GetResponseStream());

					if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
						(resp.StatusCode == HttpStatusCode.RedirectMethod))
					{
						//_log.Info("In the PRRDHelper=>ConfirmDemand: Failed");
						objResponse.Status = "Failed";
						objResponse.data = "";
					}
					else
					{
						//_log.Info("In the Helper=>ConfirmDemand: Success");
						string data = sr.ReadToEnd().Trim();

						objResponse.Status = "100";
						objResponse.REASON = data;
					}


				}
			}
			catch (WebException ex)
			{
				objResponse.Status = "Failed";
				objResponse.REASON = CommonSPHel.ThirdpartyMessage;
			}
			return objResponse;
		}

		public dynamic GetFarmerData(dynamic objdata)
		{
			dynamic objResponse = new ExpandoObject();
			try
			{
				using (var client = new HttpClient())
				{
					ServicePointManager.Expect100Continue = true;
					ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

					System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

					var req = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["AgricultureApiBaseAddress"].ToString() + "Agriculture/v1/api/v1/agriculture");
					req.ContentType = "application/json";
					req.Method = "POST";


					req.Headers.Add("Authorization", "Bearer 7d1a2c9a-72d2-3b6e-8663-e91f4e58d578");

					req.AllowAutoRedirect = false;
					string str = JsonConvert.SerializeObject(objdata);

					str = str.Replace("null", "\"\"");
					var _jsonObject = str;
					if (!String.IsNullOrEmpty(_jsonObject))
					{
						using (System.IO.Stream s = req.GetRequestStream())
						{
							using (System.IO.StreamWriter sw = new System.IO.StreamWriter(s))
								sw.Write(_jsonObject);
						}
					}

					var resp = (HttpWebResponse)req.GetResponse();
					var sr = new StreamReader(resp.GetResponseStream());

					if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
						(resp.StatusCode == HttpStatusCode.RedirectMethod))
					{
						// _log.Info("In the PRRDHelper=>ConfirmDemand: Failed");
						objResponse.Status = "Failed";
						objResponse.data = "";
					}
					else
					{
						//_log.Info("In the Helper=>ConfirmDemand: Success");
						string data = sr.ReadToEnd().Trim();

						objResponse.Status = "100";
						objResponse.REASON = data;
					}


				}
			}
			catch (WebException ex)
			{
				objResponse.Status = "Failed";
				objResponse.REASON = CommonSPHel.ThirdpartyMessage;
			}
			return objResponse;
		}

		#endregion
	}
}
