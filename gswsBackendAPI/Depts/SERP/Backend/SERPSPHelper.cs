using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;

namespace gswsBackendAPI.Dept.SERP.Backend
{
	public class SERPSPHelper : CommonSPHel
	{
		OracleCommand cmd;
		CommonSPHel comhel = new CommonSPHel();

		#region PENSION
		public DataTable GetApplicantStatus_helper(PENSIONModel oj)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "gsws_sp_check_status";
				cmd.Parameters.Add("ftype", OracleDbType.Varchar2, 12).Value = "1";
				cmd.Parameters.Add("fdpart_id ", OracleDbType.Varchar2, 20).Value = oj.PID;
				cmd.Parameters.Add("fadhar_no", OracleDbType.Varchar2, 20).Value = oj.UID;
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtstatus = comhel.GetgswsDataAdapter(cmd);
				if (dtstatus != null && dtstatus.Rows.Count > 0)
				{
					return dtstatus;
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("PENSIONExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetApplicantStatus:" + ex.Message.ToString()));
				throw ex;
			}

		}
		#endregion

		#region GLSVerification

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
			}
			catch (WebException wex)
			{
				string mappath = HttpContext.Current.Server.MapPath("SERPExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error on Data API:" + wex.Message.ToString()));

				throw wex;
			}

			return response;
		}

		public T GetSerialzedData<T>(string Input)
		{
			return JsonConvert.DeserializeObject<T>(Input);
		}

		#endregion
	}
}