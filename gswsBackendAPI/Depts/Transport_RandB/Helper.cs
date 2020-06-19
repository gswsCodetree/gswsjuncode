using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using gswsBackendAPI.DL.DataConnection;
using gswsBackendAPI.Transport_ComplaintsStatusCheck;
using Newtonsoft.Json;
using gswsBackendAPI.DL.CommonHel;

namespace gswsBackendAPI.Depts.Transport_RandB
{

	public class Helper : Model
    {

		public dynamic GetComplaintStatus_RandB(string cmpid)
		{
			dynamic objresult = new ExpandoObject();
			try
			{
				string strAttribute = string.Empty;
				string strResult = string.Empty;
				ComplaintsSearchService a = new ComplaintsSearchService();
				//RoadsComplaint_StatusCheck.ComplaintsSearchService rb = new RoadsComplaint_StatusCheck.ComplaintsSearchService();
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				a.Timeout = System.Threading.Timeout.Infinite;
				string elemList1 = a.Get_Details(cmpid);
		
				XmlElement elemListData = GetElement(elemList1);
				XmlNodeList elemList = elemListData.GetElementsByTagName("Table");

				if (elemList.Count > 0)
				{
					Complaintdetails objComplaintData = new Complaintdetails();
					for (int i = 0; i < elemList[0].ChildNodes.Count; i++)
					{

						strAttribute = elemList[0].ChildNodes[i].Name;
						strResult = elemList[0].ChildNodes[i].InnerText;

						if (strAttribute == "UserName")
							objComplaintData.Username = strResult;
						if (strAttribute == "Email")
							objComplaintData.Email = strResult;
						if (strAttribute == "MobileNumber")
							objComplaintData.MobileNumber = strResult;
						if (strAttribute == "PersonalAddress")
							objComplaintData.PersonalAddress = strResult;
						if (strAttribute == "ComplaintAddress")
							objComplaintData.ComplaintAddress = strResult;

						if (strAttribute == "ComplaintTitle")
							objComplaintData.ComplaintTitle = strResult;
						if (strAttribute == "ComplaintDescription")
							objComplaintData.ComplaintDescription = strResult;
						if (strAttribute == "ReferenceId")
							objComplaintData.ReferenceId = strResult.ToString();
						if (strAttribute == "CreatedDate")
							objComplaintData.CreatedDate = strResult;
						if (strAttribute == "OtherCompRefId")
							objComplaintData.OtherCompRefId = strResult;

						if (strAttribute == "distname")
							objComplaintData.distname = strResult;
						if (strAttribute == "Status")
							objComplaintData.Status = strResult;
						if (strAttribute == "Category")
							objComplaintData.Category = strResult;

					}
					objresult.Status = "Success";
					objresult.Reason = "";
					objresult.Data = objComplaintData;

				}
				else
				{
					objresult.Status = "Failure";
					objresult.Reason = "No Data Found";
					objresult.Data = "";
				}



			}
			catch (Exception ex)
			{
                string mappath = HttpContext.Current.Server.MapPath("TransportExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + ex.Message.ToString()));

                objresult.Status = "Failure";
				objresult.Reason = CommonSPHel.ThirdpartyMessage ;
				objresult.Data = "";
			}
			return objresult;
		}

		private static XmlElement GetElement(string xml)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xml);
			return doc.DocumentElement;
		}

		#region RTA

		public dynamic GetRTARegAppStatus(RTAStatusCls root)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				var val = PostData("https://rtaappsc.epragathi.org:1201/reg/citizenServices/applicationSearchForServicesRegisration", root);
				var data = GetSerialzedData<dynamic>(val);
				obj.Status = 100;
				obj.Reason = "Data Loaded Successfully.";
				obj.Details = data;
				return obj;
			}
			catch (Exception ex)
			{
                Common_Transport_RandB_Error(ex.Message.ToString(), "https://rtaappsc.epragathi.org:1201/reg/citizenServices/applicationSearchForServicesRegisration", "2");
                obj.Status = 102;
				obj.Reason = CommonSPHel.ThirdpartyMessage;
				return obj;
			}

		}

		public dynamic GetLLRAppStatus_helper(ApplicationStatus root)
		{
			dynamic obj = new ExpandoObject();
			try
			{

				var val = PostData(" https://otsiqa.epragathi.org:9393/dl/searchByApplicantId", root);
				var data = GetSerialzedData<dynamic>(val);
				obj.Status = 100;
				obj.Reason = "Data Loaded Successfully.";
				obj.Details = data;

				return obj;
			}
			catch (Exception ex)
			{
                Common_Transport_RandB_Error(ex.Message.ToString(), "https://otsiqa.epragathi.org:9393/dl/searchByApplicantId", "2");
                obj.Status = 102;
				obj.Reason = "Error Occured While Getting Status";
				return obj;
			}

		}

		public dynamic GetMethod(string url)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				dynamic val = GetData(url);
				obj.Status = 100;
				obj.Reason = "Data Loaded Successfully.";
				obj.Details = val;
				return obj;
			}
			catch (Exception ex)
			{
				obj.Status = 102;
				obj.Reason = CommonSPHel.ThirdpartyMessage;
				return obj;
			}

		}

		public dynamic GetData(string url)
		{
			var response = String.Empty;
			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

				var req = (HttpWebRequest)WebRequest.Create(url);
				req.ContentType = "application/json; charset=utf-8";
				req.AllowAutoRedirect = false;
				var resp = req.GetResponse();
				var sr = new StreamReader(resp.GetResponseStream());
				response = sr.ReadToEnd().Trim();

				var data = JsonConvert.DeserializeObject<dynamic>(response);

				return data;
			}
			catch (WebException wex)
			{
				string mappath = HttpContext.Current.Server.MapPath("RTAExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + wex.Message.ToString()));

				throw wex;
			}
		}

		public dynamic PostData(string url, dynamic jsonData)
		{
			var response = String.Empty;
			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

				var req = (HttpWebRequest)WebRequest.Create(url);
				req.Credentials = CredentialCache.DefaultCredentials;
				WebProxy myProxy = new WebProxy();
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
				}
				else
				{
					response = sr.ReadToEnd().Trim();
				}

				string mappath = HttpContext.Current.Server.MapPath("RTAResponseLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Respose on Data API:" + response));
			}
			catch (WebException wex)
			{
				string mappath = HttpContext.Current.Server.MapPath("RTAExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error on Data API:" + wex.Message.ToString()));

				throw wex;
			}

			return response;
		}

		public T GetSerialzedData<T>(string Input)
		{
			return JsonConvert.DeserializeObject<T>(Input);
		}
        public bool Common_Transport_RandB_Error(string msg, string url, string etype)
        {
            ExceptionDataModel objex = new ExceptionDataModel();
            try
            {
                objex.E_DEPTID = DepartmentEnum.Department.Transport_Roads_and_Buildings.ToString();
                objex.E_HODID = DepartmentEnum.HOD.APSRTC.ToString();
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
    }
}