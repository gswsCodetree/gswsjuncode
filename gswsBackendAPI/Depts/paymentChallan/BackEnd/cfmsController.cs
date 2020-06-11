using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace gswsBackendAPI.Depts.paymentChallan.BackEnd
{
    [RoutePrefix("api/cfms")]
    public class cfmsController : ApiController
    {
        [Route("serviceRequests")]
        [HttpPost]
        public IHttpActionResult serviceRequests(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            string serialized_data = token_gen.Authorize_aesdecrpty(data);
            try
            {
                serviceRequestModel rootobj = JsonConvert.DeserializeObject<serviceRequestModel>(serialized_data);
                return Ok(cfmsHelper.serviceRequests(rootobj));
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return Ok(objdata);
        }

        [Route("lastChallanDetails")]
        [HttpPost]
        public IHttpActionResult lastChallanDetails(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            string serialized_data = token_gen.Authorize_aesdecrpty(data);
            try
            {
                serviceRequestModel rootobj = JsonConvert.DeserializeObject<serviceRequestModel>(serialized_data);
                return Ok(cfmsHelper.lastChallanDetails(rootobj));
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return Ok(objdata);
        }

        [Route("generateChallan")]
        [HttpPost]
        public IHttpActionResult generateChallan(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            string serialized_data = token_gen.Authorize_aesdecrpty(data);
            try
            {
                serviceRequestModel rootobj = JsonConvert.DeserializeObject<serviceRequestModel>(serialized_data);
                return Ok(cfmsHelper.generateChallan(rootobj));
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return Ok(objdata);
        }

    }

    public static class cfmsHelper
    {
        static CommonSPHel _Hel = new CommonSPHel();
        static string cfmsPaymentUrl = "";

        public static dynamic serviceRequests(serviceRequestModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.type = "1";
                DataTable dt = cfmsChallanProc(obj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    objdata.status = true;
                    objdata.result = dt;
                }
                else
                {
                    objdata.status = false;
                    objdata.result = "Service Requests Not Available to load !!!";
                }
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public static dynamic generateChallan(serviceRequestModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.type = "2";
                DataTable dt = cfmsChallanProc(obj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    objdata.status = true;
                    objdata.result = dt;
                }
                else
                {
                    objdata.status = false;
                    objdata.result = "Service Requests Not Available to load !!!";
                }
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public static dynamic lastChallanDetails(serviceRequestModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.type = "3";
                DataTable dt = cfmsChallanProc(obj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    objdata.status = true;
                    objdata.result = dt;
                }
                else
                {
                    objdata.status = false;
                    objdata.result = "last Challan Details Not Available to load !!!";
                }
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }



        public static string POST_RequestAsync(string uri, string json, int count = 0)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

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
                return result;

            }
            catch (Exception ex)
            {
                if (count < 3)
                {
                    Thread.Sleep(1000);
                    count++;
                    return POST_RequestAsync(uri, json, count);
                }
                else
                {
                    throw ex;
                }
            }


        }


        public static DataTable cfmsChallanProc(serviceRequestModel obj)
        {
            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GSWS_CFMS_CHALLAN_PROC";
                cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2).Value = obj.type;
                cmd.Parameters.Add("P_DISTRICT", OracleDbType.Varchar2).Value = obj.districtId;
                cmd.Parameters.Add("P_MANDAL", OracleDbType.Varchar2).Value = obj.mandalId;
                cmd.Parameters.Add("P_SEC", OracleDbType.Varchar2).Value = obj.secId;
                cmd.Parameters.Add("P_DEPT_TRANS_ID", OracleDbType.Varchar2).Value = obj.deptTxnId;
                cmd.Parameters.Add("P_CHALLAN_ID", OracleDbType.Varchar2).Value = obj.challanId;
                cmd.Parameters.Add("P_CHALLAN_DATE", OracleDbType.Varchar2).Value = obj.challanDate;
                cmd.Parameters.Add("p_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                DataTable dtstatus = _Hel.GetProdgswsDataAdapter(cmd);
                return dtstatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
