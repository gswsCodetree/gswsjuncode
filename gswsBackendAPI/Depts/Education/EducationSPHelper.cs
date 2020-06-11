using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using gswsBackendAPI.DL.DataConnection;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace gswsBackendAPI.Depts.Education
{
    public class EducationSPHelper : CommonSPHel
    {

        #region Ammavadi
        OracleCommand cmd;
        #region Brahmin Corporation
        public dynamic PostData(string url, dynamic jsonData)
        {
            var response = String.Empty;
            try
            {
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
                string mappath = HttpContext.Current.Server.MapPath("EducationPostDataLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error From EducationPostData Method:" + wex.Message.ToString()));

                throw new Exception(wex.Message);
            }

            return response;
        }
		public dynamic GetData(string url)
		{
			var response1 = string.Empty;
			try
			{
				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Method = "GET";
				request.ContentType = "application/json";

				WebResponse response = request.GetResponse();
				using (var reader = new StreamReader(response.GetResponseStream()))
				{
					response1 = reader.ReadToEnd();

				}


			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return response1;

		}
		public T GetSerialzedData<T>(string Input)
        {
            return JsonConvert.DeserializeObject<T>(Input);
        }
        #endregion

        public DataTable GetAmmavodiAppStatus_data_helper(Ammavodi obj)
        {
            try
            {
                var comd = new OracleCommand();
                
                comd.InitialLONGFetchSize = 1000;
                comd.CommandType = CommandType.StoredProcedure;
                comd.CommandText = "unemployment.gsws_check_status_npci_data";
                comd.Parameters.Add("ftype", OracleDbType.Varchar2).Value = obj.ftype;
                comd.Parameters.Add("fdpart_id", OracleDbType.Varchar2).Value = obj.fdpart_id;
                comd.Parameters.Add("fadhar_no", OracleDbType.Varchar2).Value = obj.fadhar_no;
                comd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                DataTable data = GetspsDataAdapter(comd);
                if (data != null && data.Rows.Count > 0)
                    return data;
                else
                    return null;
            }
            catch (WebException wex)
            {
                string mappath = HttpContext.Current.Server.MapPath("AmmavodiExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Ammavodi App Status API:" + wex.Message.ToString()));
                throw new Exception(wex.Message);
            }
        }

        public DataTable DemoAPI(string id)
        {
            try
            {
                cmd = new OracleCommand();
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "RB_SP_OWNERS_DETAILS";
                //cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2, 12).Value = objrb.Ftype;
                //cmd.Parameters.Add("P_DISTRICT", OracleDbType.Varchar2, 20).Value = objrb.FDistrict;
                //cmd.Parameters.Add("P_MANDAL", OracleDbType.Varchar2, 20).Value = objrb.FMandal;
                //cmd.Parameters.Add("P_VILLAGE", OracleDbType.Varchar2, 20).Value = objrb.FVillage;
                //cmd.Parameters.Add("P_KATHA_NO", OracleDbType.Varchar2, 20).Value = objrb.FKathano;
                //cmd.Parameters.Add("P_UNIQUE_ID", OracleDbType.Varchar2, 50).Value = objrb.FUID;
                cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                DataTable dtApproval = GetgswsDataAdapter(cmd);
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
                string mappath = HttpContext.Current.Server.MapPath("AmmavodiExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error From Demo API:" + ex.Message.ToString()));
                throw ex;//throw ex;
            }

        }

        #endregion
    }
}