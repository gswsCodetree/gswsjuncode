using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using gswsBackendAPI.DL.CommonHel;
using System.Threading.Tasks;
using gswsBackendAPI.DL.DataConnection;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using gswsBackendAPI.transactionModule;

namespace gswsBackendAPI.Depts.REVENUE.Backend
{
	public class RevenueHelper: RevenueSPHelper
	{
		#region REVENUE
		public dynamic getRiceCardToken()
		{
			try
			{
				object obj = new
				{
					UserID = "epragati-ration",
					Mobile = "",
					Password = "rtgs@123",
				};

				var data = new EncryptDecrypt().PostData(sapandanaurl.tokenurl, obj);

				dynamic objdata = JsonConvert.DeserializeObject<spandamurlmodel>(data);

				return objdata;

			}
			catch (Exception ex)
			{
                Common_Revenue_Error(ex.Message.ToString(), "tokenurl", "2");
                string mappath = HttpContext.Current.Server.MapPath("RiceCardTokenLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From Rice Cards Token:" + ex.Message.ToString()));
				throw ex;
			}
		}

		public dynamic GetApplicantStatus(RevenueModel oj)
		{
			dynamic obj = new ExpandoObject();

			try
			{
				var tokendata = getRiceCardToken();
				if (tokendata.StatusCode == 200 && !string.IsNullOrEmpty(tokendata.Token))
				{
					var ricedata = new EncryptDecrypt().GetspandanaData("https://www.spandana.ap.gov.in/api/GsGw/GetRiceCardStatusByDocId/" + oj.UID, tokendata.Token);

					dynamic objdata = JsonConvert.DeserializeObject<dynamic>(ricedata);

					obj.Status = 100;
					obj.Reason = "";
					obj.Details = objdata;
				}
				else
				{
					obj.Status = 102;
					obj.Reason = CommonSPHel.ThirdpartyMessage;
				}
			}
			catch (Exception ex)
			{
                Common_Revenue_Error(ex.Message.ToString(), "https://www.spandana.ap.gov.in/api/GsGw/GetRiceCardStatusByDocId/", "2");
                string mappath = HttpContext.Current.Server.MapPath("RiceCardTokenLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From Rice Cards Token:" + ex.Message.ToString()));

				obj.Status = 102;
				obj.Reason = CommonSPHel.ThirdpartyMessage;
			}
			return obj;
		}

		public dynamic GetUIDSPadanaDetails(RevenueModel oj)
		{
			dynamic obj = new ExpandoObject();
			RevenueSPHelper Revenuesphel = new RevenueSPHelper();
			DataTable dt = new DataTable();

			try
			{
				
					dt = Revenuesphel.GetUidspandanaDetails(oj);


				if (dt != null && dt.Rows.Count > 0)
				{
					obj.Status = "100";
					obj.Reason = "";
					obj.Details = dt;
				}
				else
				{
					obj.Status = "102";
					obj.Reason = "No Data Found";
				}
			}
			catch (Exception ex)
			{
				obj.Status = "Failure";
				obj.Reason = CommonSPHel.ThirdpartyMessage;
			}
			return obj;
		}

		public dynamic GetSpandanaMaster(Seccmastermodel oj)
		{
			dynamic obj = new ExpandoObject();
			RevenueSPHelper Revenuesphel = new RevenueSPHelper();
			DataTable dt = new DataTable();

			try
			{

				dt = Revenuesphel.GetSpandanaMasterDetails(oj);


				if (dt != null && dt.Rows.Count > 0)
				{
					obj.Status = "100";
					obj.Reason = "";
					obj.Details = dt;
				}
				else
				{
					obj.Status = "102";
					obj.Reason = "No Data Found";
				}
			}
			catch (Exception ex)
			{
				obj.Status = "Failure";
				obj.Reason = CommonSPHel.ThirdpartyMessage;
			}
			return obj;
		}

		public dynamic GetSeccMaster(Seccmastermodel oj)
		{
			dynamic obj = new ExpandoObject();
			RevenueSPHelper Revenuesphel = new RevenueSPHelper();
			DataTable dt = new DataTable();

			try
			{

				dt = Revenuesphel.GetSeccMasterDetails(oj);


				if (dt != null && dt.Rows.Count > 0)
				{
					obj.Status = "100";
					obj.Reason = "";
					obj.Details = dt;
				}
				else
				{
					obj.Status = "102";
					obj.Reason = "No Data Found";
				}
			}
			catch (Exception ex)
			{
				obj.Status = "Failure";
				obj.Reason = CommonSPHel.ThirdpartyMessage;
			}
			return obj;
		}
		public dynamic getSpandanToken()
		{
			try
			{

               // WebLand_Revenue.WSPahani _obj = new WebLand_Revenue.WSPahani();
              //  WebLand_Revenue.pahaniAppeal _phani = new WebLand_Revenue.pahaniAppeal();

                ///List<WebLand_Revenue.pahaniAppeal> list = new List<WebLand_Revenue.pahaniAppeal>();
                //_obj.GetPahaniAppeal(list,);

                object obj = new
				{
					UserID = "codetreevs",
					Mobile = "",
					Password= "rtgsrc@123",
					RegMail="",
					AppType= "codetreeration"
				};
				
			   var data=new EncryptDecrypt().PostData(sapandanaurl.tokenurl, obj);

				dynamic objdata = JsonConvert.DeserializeObject<spandamurlmodel>(data);

				return objdata;

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("RevenExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetMandaApprovalReport_Sp:" + ex.Message.ToString()));
				throw ex;
			}
		}

		public dynamic GetSpandanaGrievanceToken()
		{
			object obj = new
			{
				UserID = "spandanaapp",
				Mobile = "",
				Password = "spandana@123",
				RegMail = "",
				AppType = "SpandanaApp"
			};

			var data = new EncryptDecrypt().PostData(sapandanaurl.tokenurl, obj);

			dynamic objdata = JsonConvert.DeserializeObject<spandamurlmodel>(data);

			return objdata;

		}

		public dynamic GetSpandanaDepartment(SpandanaInputdata objinput)
		{
			try
			{
				var druflag = "?ruFlag=" + objinput.ruFlag;

				var data = new EncryptDecrypt().GetspandanaData(sapandanaurl.depturl+druflag,objinput.token);

				dynamic objdata = JsonConvert.DeserializeObject<dynamic>(data);

				return objdata;
			}
			catch (Exception ex)
			{
                Common_Revenue_Error(ex.Message.ToString(), "depturl", "2");
                string mappath = HttpContext.Current.Server.MapPath("SpandanaExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetSpandanaDepartment:" + ex.Message.ToString()));
				throw ex;
			}
		}

		public dynamic GetSpandanaAbstractDashboardCount(SpandanaStatusModel objinput)
		{
			try
			{
				var data = new EncryptDecrypt().GetspandanaData(sapandanaurl.spandanabstracturl+objinput.SeccCode, objinput.token);

				dynamic objdata = JsonConvert.DeserializeObject<dynamic>(data);

				return objdata;
			}
			catch (Exception ex)
			{
                Common_Revenue_Error(ex.Message.ToString(), "spandanabstracturl", "2");
                string mappath = HttpContext.Current.Server.MapPath("SpandanaExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetSpandanaAbstractCount:" + ex.Message.ToString()));
				throw ex;
			}
		}
		public dynamic GetSpandanaDetailedDashboardCount(SpandanaStatusModel objinput)
		{
			try
			{
				var data = new EncryptDecrypt().GetspandanaData(sapandanaurl.spandanadetailedurl + objinput.SeccCode + "/"+objinput.Statusid, objinput.token);

				dynamic objdata = JsonConvert.DeserializeObject<dynamic>(data);

				return objdata;
			}
			catch (Exception ex)
			{
                Common_Revenue_Error(ex.Message.ToString(), "spandanadetailedurl", "2");
                string mappath = HttpContext.Current.Server.MapPath("SpandanaExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetSpandanaDetailedCount:" + ex.Message.ToString()));
				throw ex;
			}
		}
		public dynamic GetSpandanaCheck(SpandanaStatusModel objinput)
		{
			try
			{
				object obj = new {

					Statusid = "1",
					Search_Type = objinput.grievancid

				};
				  

				var data = new EncryptDecrypt().SpandanaPostData(sapandanaurl.SpandanaStatusCheck,obj,objinput.token);

				dynamic objdata = JsonConvert.DeserializeObject<dynamic>(data);

				return objdata;
			}
			catch (Exception ex)
			{
                Common_Revenue_Error(ex.Message.ToString(), "SpandanaStatusCheck", "2");
                string mappath = HttpContext.Current.Server.MapPath("SpandanaExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetSpandanaDepartment:" + ex.Message.ToString()));
				throw ex;
			}
		}
		public dynamic Getsubject(SpandanaInputdata objinput)
		{
			try
			{
				var druflag = "?ruFlag=" + objinput.ruFlag+"&hodId="+objinput.hodId;
			

				var data = new EncryptDecrypt().GetspandanaData(sapandanaurl.allsubjecturl + druflag, objinput.token);

				dynamic objdata = JsonConvert.DeserializeObject<dynamic>(data);

				return objdata;
			}
			catch (Exception ex)
			{
                Common_Revenue_Error(ex.Message.ToString(), "allsubjecturl", "2");
                string mappath = HttpContext.Current.Server.MapPath("SpandanaExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From Getsubject:" + ex.Message.ToString()));
				throw ex;
			}
		}

		public dynamic GetSubSubject(SpandanaInputdata objinput)
		{
			try
			{
				var druflag = "?ruFlag=" + objinput.ruFlag + "&hodId=" + objinput.hodId+"&subjectId="+objinput.subjectId; 


				var data = new EncryptDecrypt().GetspandanaData(sapandanaurl.allsubsubjecturl + druflag, objinput.token);

				dynamic objdata = JsonConvert.DeserializeObject<dynamic>(data);

				return objdata;
			}
			catch (Exception ex)
			{
                Common_Revenue_Error(ex.Message.ToString(), "allsubsubjecturl", "2");
                string mappath = HttpContext.Current.Server.MapPath("SpandanaExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetSubSubject:" + ex.Message.ToString()));
				throw ex;
			}
		}
		public dynamic getkeywordsubsubject(SpandanaInputdata objinput)
		{
			try
			{
				var druflag = "?ruFlag=" + objinput.ruFlag;


				var data = new EncryptDecrypt().GetspandanaData(sapandanaurl.searchkeywordurl + druflag, objinput.token);

				dynamic objdata = JsonConvert.DeserializeObject<dynamic>(data);

				return objdata;
			}
			catch (Exception ex)
			{
                Common_Revenue_Error(ex.Message.ToString(), "https://www.spandana.ap.gov.in/api/ExternalUser/GetKeywordSubSubjectsrdurl", "2");
                string mappath = HttpContext.Current.Server.MapPath("SpandanaExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From getkeywordsubsubject:" + ex.Message.ToString()));
				throw ex;
			}
		}

		public dynamic SavegetSpandanaGrievance(RootSpandaObject obj)
		{
			dynamic objdata = new ExpandoObject();
			try
			{
				string status = SaveSpandanaGrievance(obj);
				if (status == "Success")
				{
					objdata.StatusCode = "100";
					objdata.Message = "Data Submitted Successfully";
					return objdata;
				}
				else
				{
					objdata.StatusCode = "102";
					objdata.Message = "Data Not Submitted Please try Again";
					return objdata;
				}
			}
			catch (Exception ex)
			{
                objdata.StatusCode = "102";
                objdata.Message = CommonSPHel.ThirdpartyMessage;
                return objdata;
            }
		}
		#endregion


		#region Excise District API
		public dynamic GetDistricts_Excise()
		{
			dynamic objdist = new ExpandoObject();
			try
			{
				var DistrictData = GetData<dynamic>("http://cpe.ap.gov.in/EeGP/api/rest/getAllDistricts");
				var ResultData = DistrictData;//JsonConvert.DeserializeObject(DistrictData);

				objdist.Status = "Success";
				objdist.Reason = "Success";
				objdist.Data = ResultData;

			}
			catch (Exception)
			{
				objdist.Status = "Failure";
				objdist.Reason = CommonSPHel.ThirdpartyMessage;
				objdist.Data = "";
				throw;
			}
			return objdist;
		}

		public dynamic GetMandal_Excise(string distcode)
		{
			dynamic objdist = new ExpandoObject();
			try
			{
				var MandalData = GetData<dynamic>("http://cpe.ap.gov.in/EeGP/api/rest/getMandalListByRevenue?revenueCode=" + distcode);
				var ResultData = MandalData;

				objdist.Status = "Success";
				objdist.Reason = "Success";
				objdist.Data = ResultData;

			}
			catch (Exception)
			{
				objdist.Status = "Failure";
				objdist.Reason = CommonSPHel.ThirdpartyMessage;
				objdist.Data = "";
			}
			return objdist;
		}

		public dynamic GetVillages_Excise(string mandalcode)
		{
			dynamic objdist = new ExpandoObject();
			try
			{
				var MandalData = GetData<dynamic>("http://cpe.ap.gov.in/EeGP/api/rest/getVillageListByMandal?mandalCode=" + mandalcode);
				var ResultData = MandalData;

				objdist.Status = "Success";
				objdist.Reason = "Success";
				objdist.Data = ResultData;

			}
			catch (Exception)
			{
				objdist.Status = "Failure";
				objdist.Reason = CommonSPHel.ThirdpartyMessage;
				objdist.Data = "";
			}
			return objdist;
		}

		public dynamic GetComplaintStatus_Excise(string appcode)
		{
			dynamic objdist = new ExpandoObject();
			try
			{
				//appcode = "PRK0808190001";
				var result = PostData("http://cpe.ap.gov.in/EeGP/api/rest/getStatusByReqNo?reqNo=" + appcode, "");
				//var MandalData = GetData_header<dynamic>("http://cpe.ap.gov.in/EeGP/api/rest/getStatusByReqNo?reqNo=" + appcode);
				var ResultData = GetSerialzedData<dynamic>(result);
				//var ResultData = GetSerialzedData(result);

				objdist.Status = "Success";
				objdist.Reason = "Success";
				objdist.Data = ResultData;

			}
			catch (Exception ex)
			{
                Common_Revenue_Error(ex.Message.ToString(), "http://cpe.ap.gov.in/EeGP/api/rest/getStatusByReqNo?reqNo=", "2");
                objdist.Status = "Failure";
				objdist.Reason = CommonSPHel.ThirdpartyMessage;
				objdist.Data = "";
			}
			return objdist;
		}

		public dynamic Save_ExcisePCF_Info(ExciseModel objmodel)
		{
            bool Isimage = true;
			dynamic objdist = new ExpandoObject();
			try
			{
                objmodel.UploadDoc.ForEach(x => 
                {
                    var FileName = x.fileName;

                    var fileext = Path.GetExtension(FileName).ToLower();

                    if (fileext == ".jpg" || fileext == ".jpeg" || fileext == ".png")
                    {
                        var base64 = x.image.Split(',')[1];
                        byte[] imageBytes = Convert.FromBase64String(base64);
                        if (!Utils.IsValidImage(imageBytes))
                            Isimage = false;
                    }
                    else if (fileext == ".pdf")
                    {
                        var base64 = x.image.Split(',')[1];
                        byte[] imageBytes = Convert.FromBase64String(base64);
                        if (!Utils.IsValidPDF(imageBytes))
                            Isimage = false;
                    }
                    else
                        Isimage = false;

                    //Convert Base64 Encoded string to Byte Array.

                });

                if (!Isimage)
                {
                    objdist.Status = "Failure";
                    objdist.Reason = "Invalid format";
                    objdist.Data = "";
                }
                else
                {
                    var ResultData = PostData_Headers("http://cpe.ap.gov.in/EeGP/api/rest/savePCFform", objmodel);
                    ResultData = JsonConvert.DeserializeObject(ResultData);
					transactionModel objtrans = new transactionModel();
					objtrans.TYPE = "2";
					objtrans.TXN_ID = objmodel.GSWS_ID;
					objtrans.DEPT_ID = "3303";
					objtrans.DEPT_TXN_ID = ResultData.reqNo;
					objtrans.STATUS_CODE = ResultData.status=="success"?"01":"02"; //01-success
					objtrans.REMARKS= ResultData.reason;
					DataTable dt = new transactionHelper().transactionInsertion(objtrans);

					if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
					{
						objdist.Status = ResultData.status;
						objdist.Reason = ResultData.reason;
						objdist.Data = ResultData.reqNo;
					}
					else
					{
						objdist.Status = ResultData.status;
						objdist.Reason = ResultData.reason;
						objdist.Data = ResultData.reqNo;
					}
                }

			}
			catch (Exception ex)
			{
                
                string mappath = HttpContext.Current.Server.MapPath("ExciceExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetApplicantStatus:" + ex.Message.ToString()));
                Common_Revenue_Error(ex.Message.ToString(), "http://cpe.ap.gov.in/EeGP/api/rest/savePCFform", "2");
                objdist.Status = "Failure";
                objdist.Reason = CommonSPHel.ThirdpartyMessage ;
                objdist.Data = "";
            }
			return objdist;
		}
		#endregion

		#region "Service Consuming cODE"

		public T GetData<T>(string url)
		{
			try
			{
				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				var req = (HttpWebRequest)WebRequest.Create(url);
				req.ContentType = "application/json; charset=utf-8";
				req.AllowAutoRedirect = false;

				var resp = req.GetResponse();
				var sr = new StreamReader(resp.GetResponseStream());
				var response = sr.ReadToEnd().Trim();

				var data = JsonConvert.DeserializeObject<T>(response);
				// data = Json.DeserializeObject<dynamic>(response);

				return data;
			}
			catch (Exception wex)
			{
                string mappath = HttpContext.Current.Server.MapPath("RevenueExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetApplicantStatus:" + wex.Message.ToString()));

                throw wex;
			}
		}

		public T GetData_header<T>(string url)
		{
			try
			{
				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				var req = (HttpWebRequest)WebRequest.Create(url);
				req.ContentType = "application/json; charset=utf-8";
				req.AllowAutoRedirect = false;
				req.Headers.Add("CLIENT_TOKEN", "K3A9R27V84Y252");
				var resp = req.GetResponse();
				var sr = new StreamReader(resp.GetResponseStream());
				var response = sr.ReadToEnd().Trim();

				var data = JsonConvert.DeserializeObject<T>(response);
				// data = Json.DeserializeObject<dynamic>(response);

				return data;
			}
			catch (Exception wex)
			{
                string mappath = HttpContext.Current.Server.MapPath("RevenueExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetApplicantStatus:" + wex.Message.ToString()));

                throw wex;
			}
		}

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
                string mappath = HttpContext.Current.Server.MapPath("RevenueExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetApplicantStatus:" + wex.Message.ToString()));
                //	Logfile_eX("VERIFYOTP ELIGIBILITY CHECK SERVICE EXCEPTION::", wex.Message);
                throw new Exception(wex.Message);
			}

			return response;
		}

		public dynamic PostData_Headers(string url, dynamic jsonData)
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
				req.Headers.Add("CLIENT_TOKEN", "K3A9R27V84Y252");
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
                string mappath = HttpContext.Current.Server.MapPath("RevenueExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetApplicantStatus:" + wex.Message.ToString()));
                //	Logfile_eX("VERIFYOTP ELIGIBILITY CHECK SERVICE EXCEPTION::", wex.Message);
                throw wex;
			}

			return response;
		}
        #endregion
        public bool Common_Revenue_Error(string msg, string url, string etype)
        {
            ExceptionDataModel objex = new ExceptionDataModel();
            try
            {
                objex.E_DEPTID = DepartmentEnum.Department.Revenue.ToString();
                objex.E_HODID = DepartmentEnum.HOD.Revenue_CCLA.ToString();
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

        public T GetSerialzedData<T>(string Input)
		{
			return JsonConvert.DeserializeObject<T>(Input);
		}
	}

	public static class sapandanaurl
	{
		public static string tokenurl = "https://www.spandana.ap.gov.in/api/ExternalUser/GetAccess";

		public static string registrationurl = "https://www.spandana.ap.gov.in/gsws/RiceCardRegistration.aspx?";//"https://www.spandana.ap.gov.in/gsws/RationCard_Registration.aspx?";//accessToken=&Volunteerid=&AadhaarNo=&vvstype=VVS2&DistId=&MandalId=&GpId=&GpFlag=

		public static string depturl = "https://www.spandana.ap.gov.in/api/ExternalUser/GetDepartments";
		public static string allsubjecturl = "https://www.spandana.ap.gov.in/api/ExternalUser/GetSubjects"; //ruFlag=&hodId=
		public static string allsubsubjecturl = "https://www.spandana.ap.gov.in/api/ExternalUser/GetSubSubjects"; //?ruFlag=&hodId=subjectId=
		public static string searchkeywordurl = "https://www.spandana.ap.gov.in/api/ExternalUser/GetKeywordSubSubjects";//?ruFlag="
		public static string registeregrievanceurl = "https://www.spandana.ap.gov.in/api/ExternalUser/RegisterGrievance";
		public static string SpandanaStatusCheck = "https://www.spandana.ap.gov.in/API/PeopleFirst/APPLICATION_SEARCH_SPANDANA/";

		public static string spandanabstracturl = "https://www.spandana.ap.gov.in/API/GsGw/GET_GSWS_COUNT/"; //sececode
		public static string spandanadetailedurl = "https://www.spandana.ap.gov.in/API/GsGw/GET_GSWS_ABSTRACT/";  //10990074/1

	}
    public class spandamurlmodel
    {
        public int StatusCode { get; set; }
        public string Status { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public string url = sapandanaurl.registrationurl;
    }

	public class SpandanaInputdata
	{
		public string ftype { get; set; }
		public string ruFlag { get; set; }
		public string hodId { get; set; }
		public string subjectId { get; set; }
		public string token { get; set; }
	}
	public class SpandanaStatusModel
	{
		public string Statusid { get; set; }
		public string grievancid { get; set; }
		public string SeccCode { get; set; }
		public string token { get; set; }
	}
    
}