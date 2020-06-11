using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace gswsBackendAPI.Depts.Industries
{

    public class IndustriesSPHelper : CommonSPHel
    {
        #region YSR Navodayam
        public dynamic GetData(string url)
        {
            var response = String.Empty;
            try
            {
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
                string mappath = HttpContext.Current.Server.MapPath("IndustriesGetLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Get Deatials From Industries API:" + wex.Message.ToString()));

                throw wex;
            }


        }

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

				string mappath = HttpContext.Current.Server.MapPath("IndustriesSaveLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Submit Data :" + _jsonObject));
			}
            catch (WebException wex)
            {
                string mappath = HttpContext.Current.Server.MapPath("IndustriesErrorLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error OTPR Registration  API:" + wex.Message.ToString()));

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