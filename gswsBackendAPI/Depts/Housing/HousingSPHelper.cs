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

namespace gswsBackendAPI.Depts.Housing
{
    public class HousingSPHelper:CommonSPHel
    {
        #region Housing
        public dynamic GetData(string url)
        {
            var response = String.Empty;
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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
                string mappath = HttpContext.Current.Server.MapPath("HousingExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + wex.Message.ToString()));

                throw new Exception(wex.Message);
            }


        }
        #endregion
    }
}