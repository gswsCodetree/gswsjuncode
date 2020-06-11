
using gswsBackendAPI.DL.CommonHel;
using gswsBackendAPI.DL.DataConnection;
using gswsBackendAPI.transactionModule;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;

namespace gswsBackendAPI.Depts.PRRD
{
	public class JobCardHelper
	{
		PRRDResponse objResponse = new PRRDResponse();

		#region   JobcardRegistration
		public dynamic LoadPanchayatlist(LGDMasterModel oj)
		{
			dynamic obj = new ExpandoObject();
			PRRDSPHelper spobj = new PRRDSPHelper();
			try
			{

				DataTable dt = spobj.GetHabitationCode_SP(oj);
				if (dt != null && dt.Rows.Count > 0)
				{

					obj.Status = 100;
					obj.Reason = "";
					obj.Details = dt;
				}
				else
				{

					obj.Status = 101;
					obj.Reason = "No Data Found";
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobcardHelperExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting LoadPanchayatlist " + ex.ToString()));
				obj.Status = 102;
				obj.Reason = "Error Occued While Loading Data";
				return obj;
			}
			return obj;
		}

		public dynamic LoadBankDetails(JobCardBankModel ObjLGD)
		{
			dynamic obj = new ExpandoObject();
			PRRDSPHelper spobj = new PRRDSPHelper();
			try
			{

				DataTable dt = spobj.GetBankDetails_SP(ObjLGD);
				if (dt != null && dt.Rows.Count > 0)
				{

					obj.Status = 100;
					obj.Reason = "";
					obj.BankDetails = dt;
				}
				else
				{

					obj.Status = 101;
					obj.Reason = "No Data Found";
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobcardHelperExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting LoadBankDetails:" + ex.ToString()));
				obj.Status = 102;
				obj.Reason = ex.Message.ToString() + "Error Occued While Loading Data";
				return obj;
			}
			return obj;
		}

		public dynamic LoadHabitationCode(LGDMasterModel oj)
		{
			dynamic obj = new ExpandoObject();
			PRRDSPHelper spobj = new PRRDSPHelper();
			try
			{

				DataTable dt = spobj.GetHabitationCode_SP(oj);
				if (dt != null && dt.Rows.Count > 0)
				{

					obj.Status = 100;
					obj.Reason = "";
					obj.Details = dt;
				}
				else
				{

					obj.Status = 101;
					obj.Reason = "No Data Found";
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobcardHelperExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting LoadHabitationCode:" + ex.ToString()));
				obj.Status = 102;
				obj.Reason = "Error Occued While Loading Data";
				return obj;
			}
			return obj;
		}
		public dynamic SendJobCardAPI(dynamic objdata)
		{
			dynamic objResponse = new ExpandoObject();
			try
			{
				string ourtransid = objdata["data"][0]["InterfaceUniqueId"];

				using (var client = new HttpClient())
				{
					ServicePointManager.Expect100Continue = true;
					ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

					System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

					var req = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["ApiBaseAddress"].ToString() + "Rural-Development/v1/api/v1/ruralDevelopment");
					req.ContentType = "application/json";
					req.Method = "POST";


					req.Headers.Add("Authorization", "Bearer 16cce3cd-15d5-3c19-8691-14203061b278");

					req.AllowAutoRedirect = false;
					string str = JsonConvert.SerializeObject(objdata);

					str = str.Replace("null", "\"\"");
					var _jsonObject = str;

					//var _jsonObject = JsonConvert.SerializeObject(str);
					string mappath = HttpContext.Current.Server.MapPath("SendJobcardAPIJSON");
					Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "JSON :" + str));
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
						if (!data.Contains("An error has occurred"))
						{
							List<JobCardAPIModel> objjobcard = new List<JobCardAPIModel>();

							//objResponse.Status = "Success";
							objjobcard = JsonConvert.DeserializeObject<List<JobCardAPIModel>>(data);

							string outputdata = string.Empty;
							objResponse.data = objjobcard;
							foreach (var item in objjobcard)
							{
								outputdata += item.Remarks + "/";
							}
							objResponse.REASON = outputdata.TrimEnd('/');
							objResponse.Status = objjobcard[0].Transactionid;
							if (objResponse.Status != "")
							{
								try
								{
									transactionModel objtrans = new transactionModel();
									objtrans.TYPE = "2";
									objtrans.TXN_ID = ourtransid;
									objtrans.DEPT_ID = "3301";
									objtrans.DEPT_TXN_ID = objjobcard[0].Transactionid;
									objtrans.STATUS_CODE = "01";
									objtrans.REMARKS = objjobcard[0].Remarks;

									DataTable dt = new transactionHelper().transactionInsertion(objtrans);
									JobCardModel updatestatus = new JobCardModel();
									updatestatus.P_TYPE = "11";
									updatestatus.P_JC_ID = ourtransid;
									updatestatus.P_STATUS = "1";
									UpdateJobcardStatus(updatestatus);
								}
								catch (Exception ex)
								{

									string mappath3 = HttpContext.Current.Server.MapPath("JobcardHelperExceptions");
									Task WriteTask3 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath3, "Send Jobcard API Error:" + ex.Message.ToString()));
								}
							}
						}
						else
						{


							objResponse.Status = "428";
							objResponse.REASON = CommonSPHel.ThirdpartyMessage;
						}
					}


				}
			}
			catch (WebException ex)
			{

				string mappath = HttpContext.Current.Server.MapPath("JobcardHelperExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Send Jobcard API :" + ex.Message.ToString()));

				objResponse.Status = "Failed";
				objResponse.REASON = CommonSPHel.ThirdpartyMessage;
			}
			return objResponse;

		}
		public dynamic AadharVaiidateAPI(dynamic objdata)
		{
			try
			{


				using (var client = new HttpClient())
				{
					ServicePointManager.Expect100Continue = true;
					ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

					System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

					var req = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["ApiBaseAddress"].ToString() + "Rural-Development/v1/api/v1/ruralDevelopment");
					req.ContentType = "application/json";
					req.Method = "POST";


					req.Headers.Add("Authorization", "Bearer 16cce3cd-15d5-3c19-8691-14203061b278");

					req.AllowAutoRedirect = false;

					var _jsonObject = JsonConvert.SerializeObject(objdata);

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
						if (!data.Contains("An error has occurred"))
						{
							objResponse.Status = "100";
							objResponse.REASON = data;


						}
						else
						{
							objResponse.Status = "428";
							objResponse.REASON = objResponse.data[0].Remarks;
						}
					}


				}
			}
			catch (WebException ex)
			{

				string mappath = HttpContext.Current.Server.MapPath("JobcardHelperExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Adhar Validate API :" + ex.Message.ToString()));
				objResponse.Status = "Failed";
				objResponse.data = CommonSPHel.ThirdpartyMessage;
			}
			return objResponse;
		}

		public dynamic CreateJobCard_hel(List<JobCardModel> obj)
		{
			dynamic objdata = new ExpandoObject();
			PRRDSPHelper spobj = new PRRDSPHelper();
			DataTable dt = null;
			try
			{

				dt = spobj.CreateJobCard_SP(obj);


				if (dt != null && dt.Rows.Count > 0)
				{
					objdata.Status = 100;
					objdata.Reason = "JobCard Data Inserted successfully";
					objdata.DataList = dt;

				}
				else
				{
					objdata.Status = 102;
					objdata.Reason = "Not Created";
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobcardHelperExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error CreateJobCard_hel :" + ex.Message.ToString()));
				objdata.Status = 102;
				objdata.Reason = "Not Created";

			}
			return objdata;
		}

		public dynamic GetTranslationText(TranslationModel objrans)
		{
			string res = "";
			try
			{
				string mainUrl = ConfigurationManager.AppSettings["ApiBaseAddress"].ToString();
				//string oldUrl = "Transliteration?itext=" + objrans.itext + "&transliteration=" + objrans.translitaration + "&locale=" + objrans.locale + "&transRev=" + objrans.transRev;
				string oldUrl = "Transliteration/v1/api/v1/transliteration?itext=" + objrans.itext;

				using (var client = new System.Net.Http.HttpClient())
				{
					// HTTP POST
					client.BaseAddress = new Uri(mainUrl);
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					client.DefaultRequestHeaders.Add("Authorization", "Bearer a6910586-f244-3f2e-8300-cbf87210d787");
					var response = client.GetAsync("/" + oldUrl).Result;

					using (HttpContent content = response.Content)
					{
						// ... Read the string.
						Task<string> result = content.ReadAsStringAsync();
						res = result.Result;
						res = res.Replace("\"", "");
					}
				}

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobcardHelperExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "GetTranslationText :" + ex.Message.ToString()));

				throw;
			}
			return res;

		}
		#endregion
		#region GetJobCardDetails
		public dynamic GetJobcardetails(JobCardModel ObjLGD)
		{
			dynamic obj = new ExpandoObject();
			PRRDSPHelper spobj = new PRRDSPHelper();
			try
			{

				DataTable dt = spobj.GetJobCardDetails(ObjLGD);
				if (dt != null && dt.Rows.Count > 0)
				{

					obj.Status = 100;
					obj.Reason = "";
					obj.Jobcarddetails = dt;
				}
				else
				{

					obj.Status = 101;
					obj.Reason = "No Data Found";
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobcardHelperExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "getting error GetJobcardetails :" + ex.Message.ToString()));
				obj.Status = 102;
				obj.Reason = "Error Occued While Loading Data";
				return obj;
			}
			return obj;
		}


		public dynamic UpdateJobcardStatus(JobCardModel ObjLGD)
		{
			dynamic obj = new ExpandoObject();
			PRRDSPHelper spobj = new PRRDSPHelper();
			try
			{

				DataTable dt = spobj.UpdateStatus(ObjLGD);
				if (dt != null && dt.Rows.Count > 0)
				{

					obj.Status = 100;
					obj.Reason = "";
					obj.Jobcarddetails = dt;
				}
				else
				{

					obj.Status = 101;
					obj.Reason = "No Data Found";
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobcardHelperExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "getting error UpdateJobcardStatus :" + ex.Message.ToString()));
				obj.Status = 102;
				obj.Reason = "Error Occued While Loading Data";
				return obj;
			}
			return obj;
		}


		#endregion

		public dynamic GetJobCardDataHel(JobCardModel Lobj)
		{
			PRRDSPHelper spobj = new PRRDSPHelper();
			dynamic objdata = new ExpandoObject();
			try
			{
				//_log.Info("In the PRRDHelper=>GetJobCardData : " + JsonConvert.SerializeObject(Lobj));
				DataTable dtLgd = spobj.GetJobCardSP(Lobj);
				if (dtLgd != null && dtLgd.Rows.Count > 0)
				{
					objdata.Status = 100;
					objdata.Reason = "Data Loaded successfully";
					objdata.DataList = dtLgd;
				}
				else
				{
					objdata.Status = 102;
					objdata.Reason = "No Data Found";
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobcardHelperExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "getting error GetJobCardDataHel :" + ex.Message.ToString()));
				objdata.Status = 102;
				objdata.Reason = "Data not found";

			}
			return objdata;
		}


		public dynamic GetJobCardDetailsbyTransIdandUIDhelper(dynamic val)
		{
			PRRDSPHelper spobj = new PRRDSPHelper();

			try
			{

				objResponse = SendJobCardAPI(val);

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobcardHelperExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "getting error GetJobCardDetailsbyTransIdandUIDhelper :" + ex.Message.ToString()));
			}
			return objResponse;
		}


		public dynamic GetDistandMandalCode(LGDMasterModel oj)
		{
			dynamic obj = new ExpandoObject();
			PRRDSPHelper spobj = new PRRDSPHelper();
			try
			{

				DataTable dt = spobj.GetDistandMandalCode_SP(oj);
				if (dt != null && dt.Rows.Count > 0)
				{

					obj.Status = 100;
					obj.Reason = "";
					obj.Details = dt;
				}
				else
				{

					obj.Status = 101;
					obj.Reason = "No Data Found";
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobcardHelperExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "getting error GetDistandMandalCodes :" + ex.Message.ToString()));
				obj.Status = 102;
				obj.Reason = "Error Occued While Loading Data";
				return obj;
			}
			return obj;

		}
	}
}