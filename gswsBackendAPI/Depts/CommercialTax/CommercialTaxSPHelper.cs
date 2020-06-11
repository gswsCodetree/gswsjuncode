using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace gswsBackendAPI.Depts.CommercialTax
{
    public class CommercialTaxSPHelper: CommonSPHel
	{
        public dynamic PostData(string url, dynamic jsonData)
        {
            var response = String.Empty;
            try
            {

                var req = (HttpWebRequest)WebRequest.Create(url);

                req.Credentials = CredentialCache.DefaultCredentials;
                WebProxy myProxy = new WebProxy();
                req.Proxy = myProxy;
                req.Headers.Add("Authorization", "Basic YXBjdGQ6c3RhbnNAMjQ4Ng==");
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
                string mappath = HttpContext.Current.Server.MapPath("ProfessionTaxErrorLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error  Message :" + wex.Message.ToString()));
				throw wex;
            }

            return response;
        }

        public T GetSerialzedData<T>(string Input)
        {
            return JsonConvert.DeserializeObject<T>(Input);
        }
    }
}