using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;

namespace gswsBackendAPI.Depts.Fisheries
{
    public class FisheriesSPHelper:CommonSPHel
    {
        #region Fisheries
        public dynamic GetData(string url, string Token)
        {
            var response = String.Empty;
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.ContentType = "application/json; charset=utf-8";
                req.Headers.Add("Authorization", "Bearer " + Token);
                req.AllowAutoRedirect = false;
                var resp = req.GetResponse();
                var sr = new StreamReader(resp.GetResponseStream());
                response = sr.ReadToEnd().Trim();

                var data = JsonConvert.DeserializeObject<dynamic>(response);

                return data;
            }
            catch (WebException wex)
            {
                string mappath = HttpContext.Current.Server.MapPath("FisheriesExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + wex.Message.ToString()));

                throw new Exception(wex.Message);
            }
        }

        public dynamic GetToken(string url, dynamic jsonData)
        {
            var response = String.Empty;
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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

                return response;
            }
            catch (WebException wex)
            {
                string mappath = HttpContext.Current.Server.MapPath("FisheriesExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Token API:" + wex.Message.ToString()));

                throw new Exception(wex.Message);
            }


        }

		public dynamic PostData(string url, string Token, dynamic jsonData)
		{
			var response = String.Empty;
			try
			{
				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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
				req.Headers.Add("Authorization", "Bearer " + Token);
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

				return response;
			}
			catch (WebException wex)
			{
				string mappath = HttpContext.Current.Server.MapPath("FisheriesExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data By Application API:" + wex.Message.ToString()));

				throw new Exception(wex.Message);
			}
		}

		public T GetSerialzedData<T>(string Input)
		{
			return JsonConvert.DeserializeObject<T>(Input);
		}
		#endregion
	}
}