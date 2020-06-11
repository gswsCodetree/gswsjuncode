using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace gswsBackendAPI.Depts.RationVolunteermapping.Backend
{
    public static class dbRouter
    {
        public static DataTable POST_Request(string uri, string json)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.ContentType = "application/json";
                request.Method = "POST";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }

                var response = (HttpWebResponse)request.GetResponse();
                string result = "";
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                if (!string.IsNullOrEmpty(result))
                {
                    if (result.Contains("\"status\":"))
                    {
                        responseMethod obj = JsonConvert.DeserializeObject<responseMethod>(result);

                        throw new Exception(obj.result);
                    }
                    else
                    {
                        DataTable dt = (DataTable)JsonConvert.DeserializeObject(result, (typeof(DataTable)));
                        return dt;
                    }
                }
                throw new NullReferenceException("Output is null from URL");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class responseMethod
    {
        public string status { get; set; }
        public string result { get; set; }
    }
}