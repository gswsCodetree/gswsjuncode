using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;

namespace gswsBackendAPI.Depts.YATC
{
    public class YATCSPHelper : CommonSPHel
    {
        #region Skill development
        public dynamic GetData(string url, List<Hearders> headers)
        {
            var response = String.Empty;
            try
            {
                string mappath = HttpContext.Current.Server.MapPath("SkillDevelopmentGetLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "URl :" + url));

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };


                var req = (HttpWebRequest)WebRequest.Create(url);
                req.ContentType = "application/json; charset=utf-8";
                headers.ForEach(x =>
                {
                    req.Headers.Add(x.key, x.value);
                });

                req.AllowAutoRedirect = false;
                var resp = req.GetResponse();
                var sr = new StreamReader(resp.GetResponseStream());
                response = sr.ReadToEnd().Trim();

                string mappath2 = HttpContext.Current.Server.MapPath("SkillDevelopmentGetResponseLogs");
                Task WriteTask2 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Url : " + url + " , Response Data : "+ response));

                var data = JsonConvert.DeserializeObject<dynamic>(response);

                return data;
            }
            catch (WebException wex)
            {
                string mappath = HttpContext.Current.Server.MapPath("SkillDevlopmentExceptionLogs");
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

                string mappath = HttpContext.Current.Server.MapPath("SkillDevelopmentSaveLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Submit Data :" + _jsonObject));

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

                string mappath2 = HttpContext.Current.Server.MapPath("SkillDevelopmentResponseLogs");
                Task WriteTask2 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Response Data :" + response));

            }
            catch (WebException wex)
            {
                string mappath = HttpContext.Current.Server.MapPath("SkillDevlopmentExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Submit Data API:" + wex.Message.ToString()));

                throw wex;
            }

            return response;
        }

        public dynamic PostDataWithHeaders(string url)
        {
            var response = String.Empty;
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.Credentials = CredentialCache.DefaultCredentials;
                WebProxy myProxy = new WebProxy();
                req.Proxy = myProxy;
                req.Method = "POST";
                //var _jsonObject = JsonConvert.SerializeObject(jsonData);

                ////If there is any json data
                //if (!String.IsNullOrEmpty(_jsonObject))
                //{
                //    using (System.IO.Stream s = req.GetRequestStream())
                //    {
                //        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(s))
                //            sw.Write(_jsonObject);
                //    }
                //}
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
                string mappath = HttpContext.Current.Server.MapPath("SkillDevlopmentExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Submit Data API:" + wex.Message.ToString()));

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