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
using System.Threading.Tasks;
using System.Web;
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

		[Route("TransactioHistory")]
		[HttpPost]
		public IHttpActionResult TransactioHistory(dynamic data)
		{
			dynamic objdata = new ExpandoObject();
			string serialized_data = token_gen.Authorize_aesdecrpty(data);
			try
			{
				serviceRequestModel rootobj = JsonConvert.DeserializeObject<serviceRequestModel>(serialized_data);
				return Ok(cfmsHelper.TransactionHistory(rootobj));
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
        static string cfmsPaymentUrl = "https://devcfms.apcfss.in:44301/RESTAdapter/Volunteer/ChallanPayments";

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
					var data = Send_CFMS_Payment_Response(dt);

					RootCfMSResponse objresponse = JsonConvert.DeserializeObject<RootCfMSResponse>(data);

					if (string.IsNullOrEmpty(objresponse.Response.Message))
					{

						obj.type = "5";
						obj.transaction_status = objresponse.Response.Transaction_Status;
						obj.challanId = objresponse.Response.CFMS_ID.ToString();
						obj.ifsc_code = objresponse.Response.IFSC_Code;
						obj.valid_upto = objresponse.Response.Valid_Upto.ToString();
						obj.deptTxnId = objresponse.Response.DeptTransID.ToString();
						DataTable dtcfms = cfmsChallanProc(obj);
						if (dtcfms != null && dtcfms.Rows.Count > 0 && dtcfms.Rows[0][0].ToString() == "1")
						{
							objdata.status = true;
							objdata.result = objresponse.Response.Message;
							objdata.Returnurl = "https://devcfms.apcfss.in:44300/sap/bc/ui5_ui5/sap/zfi_rcp_cstatus/index.html?sap-client=150&DeptID=" + objresponse.Response.DeptTransID.ToString();
						}
						else
						{
							DataTable dtcfms1 = cfmsChallanProc(obj);
							if (dtcfms1 != null && dtcfms1.Rows.Count > 0 && dtcfms1.Rows[0][0].ToString() == "1")
							{
								objdata.status = true;
								objdata.result = objresponse.Response.Message;
								objdata.Returnurl = "https://devcfms.apcfss.in:44300/sap/bc/ui5_ui5/sap/zfi_rcp_cstatus/index.html?sap-client=150&DeptID=" + objresponse.Response.DeptTransID.ToString();
							}
						}

					}
					else
					{

						objdata.status = false;
						objdata.result = objresponse.Response.Message;
					}
					//objdata.status = true;
                   // objdata.result = dt;
                }
                else
                {
                    objdata.status = false;
                    objdata.result = "Service Requests Not Available to load !!!";
                }
            }
            catch (Exception ex)
            {

				string mappath2 = HttpContext.Current.Server.MapPath("GenerateChallanExceptionLog");
				Task WriteTask2 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath2, "error from generate challan:"+JsonConvert.SerializeObject(obj)+":"+ex.Message.ToString()));
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

		public static dynamic TransactionHistory(serviceRequestModel obj)
		{
			dynamic objdata = new ExpandoObject();
			try
			{
				obj.type = "4";
				DataTable dt = cfmsChallanProc(obj);
				if (dt != null && dt.Rows.Count > 0)
				{
					objdata.status = true;
					objdata.result = dt;
				}
				else
				{
					objdata.status = false;
					objdata.result = "Transaction History Details is Not Available !!!";
				}
			}
			catch (Exception ex)
			{
				objdata.status = false;
				objdata.result = ex.Message.ToString();
			}
			return objdata;
		}

		public static dynamic Send_CFMS_Payment_Response(DataTable dtcf)
		{
			cfmsReqModel OBJA = new cfmsReqModel();
			try
			{
				Header head1 = new Header();
				List<ChallanValue> objlist=new List<ChallanValue>();
				foreach (DataRow dr in dtcf.Rows)
				{
					ChallanValue objch = new ChallanValue();
					objch.DeptCode = dr["DEPTCODE"].ToString();
					objch.DDOCode = dr["DDOCODE"].ToString();
					objch.HOA = dr["HOA"].ToString();
					objch.ServiceCode = dr["SERVICECODE"].ToString();
					Double val =Convert.ToDouble(dr["CHALLANAMOUNT"].ToString());
					objch.ChallanAmount = Math.Round(val).ToString();
					objlist.Add(objch);
				}
				head1.DeptTransID = dtcf.Rows[0]["DEPTTRANSID"].ToString();
				head1.EmailID = dtcf.Rows[0]["EMAILID"].ToString(); 
				head1.MobileNumber = dtcf.Rows[0]["MOBILENUMBER"].ToString(); 
				head1.RemittersID = dtcf.Rows[0]["REMITTERSID"].ToString(); 
				head1.RemitterName = dtcf.Rows[0]["REMITTERNAME"].ToString(); 
				head1.TotalAmount = Math.Round(Convert.ToDouble(dtcf.Rows[0]["TOTALAMOUNT"].ToString())).ToString();
				head1.ChallanValues = objlist;
				//new List<ChallanValue>();
				OBJA.Header = head1;
				var data= POST_RequestAsync(cfmsPaymentUrl, JsonConvert.SerializeObject(OBJA), 0);

				string mappath2 = HttpContext.Current.Server.MapPath("CFMSResponseLog");
				Task WriteTask2 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath2, data));
				return data;

			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

        public static string POST_RequestAsync(string uri, string json, int count = 0)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.ContentType = "application/json";
                request.Method = "POST";
				request.Headers.Add("Authorization", "Basic YXBsZHB0OiRhbGRwdCQxNw==");

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
				string mappath4 = HttpContext.Current.Server.MapPath("POST_RequestAsyncExceptionLogs");
				Task WriteTask4 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath4, "input Data:"+count + json + "error:" + ex.Message.ToString()));

				if (count < 3)
                {
                    Thread.Sleep(1000);
                    count++;
                    return POST_RequestAsync(uri, json, count);
                }
                else
				{
					string mappath2 = HttpContext.Current.Server.MapPath("POST_RequestAsyncExceptionLogs");
					Task WriteTask2 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath2, "input Data:" + json+ "error:" + ex.Message.ToString()));

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
				cmd.Parameters.Add("P_IFSC_CODE", OracleDbType.Varchar2).Value = obj.ifsc_code;
				cmd.Parameters.Add("P_TRANSACTION_STATUS", OracleDbType.Varchar2).Value = obj.transaction_status;
				cmd.Parameters.Add("P_VALID_UPTO", OracleDbType.Varchar2).Value = obj.valid_upto;
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
