using gswsBackendAPI.Depts.RBKPayments.Backend;
using gswsBackendAPI.DL.CommonHel;
using gswsBackendAPI.DL.DataConnection;
using gswsBackendAPI.transactionModule;
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

namespace gswsBackendAPI.Depts.Pension.Backend
{
    [RoutePrefix("api/pensionDept")]
    public class pensionDeptController : ApiController
    {
        #region DA Login

        [Route("personDetails")]
        [HttpPost]
        public IHttpActionResult personDetails(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                string serialized_data = token_gen.Authorize_aesdecrpty(data);
                pensionDeptModel rootobj = JsonConvert.DeserializeObject<pensionDeptModel>(serialized_data);
                return Ok(pensionDeptHelper.personDetails(rootobj));
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return Ok(objdata);
        }

        [Route("habitationList")]
        [HttpPost]
        public IHttpActionResult habitationList(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                string serialized_data = token_gen.Authorize_aesdecrpty(data);
                habitationMasterModel rootobj = JsonConvert.DeserializeObject<habitationMasterModel>(serialized_data);
                return Ok(pensionDeptHelper.habitationList(rootobj));
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return Ok(objdata);
        }

        [Route("subCasteList")]
        [HttpPost]
        public IHttpActionResult subCasteList(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                string serialized_data = token_gen.Authorize_aesdecrpty(data);
                casteModel rootobj = JsonConvert.DeserializeObject<casteModel>(serialized_data);
                return Ok(pensionDeptHelper.subCasteList(rootobj));
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return Ok(objdata);
        }

        [Route("pensionAppSub")]
        [HttpPost]
        public IHttpActionResult pensionAppSub(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                string serialized_data = token_gen.Authorize_aesdecrpty(data);

                string mappath2 = HttpContext.Current.Server.MapPath("pensionAppSubLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath2, serialized_data));

                pensionAppSubModel rootobj = JsonConvert.DeserializeObject<pensionAppSubModel>(serialized_data);
                return Ok(pensionDeptHelper.pensionAppSub(rootobj));
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return Ok(objdata);
        }

        #endregion

        #region WEA Login

        [Route("grevianceList")]
        [HttpPost]
        public IHttpActionResult grevianceList(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                string serialized_data = token_gen.Authorize_aesdecrpty(data);
                grevianceListModeL rootobj = JsonConvert.DeserializeObject<grevianceListModeL>(serialized_data);
                return Ok(pensionDeptHelper.grevianceList(rootobj));
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return Ok(objdata);
        }

        [Route("personGrevDetails")]
        [HttpPost]
        public IHttpActionResult personGrevDetails(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                string serialized_data = token_gen.Authorize_aesdecrpty(data);
                personGrevDetailsModel rootobj = JsonConvert.DeserializeObject<personGrevDetailsModel>(serialized_data);
                return Ok(pensionDeptHelper.personGrevDetails(rootobj));
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return Ok(objdata);
        }

        [Route("pensionAppWEASub")]
        [HttpPost]
        public IHttpActionResult pensionAppWEASub(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                string serialized_data = token_gen.Authorize_aesdecrpty(data);

                string mappath2 = HttpContext.Current.Server.MapPath("pensionAppWEASubLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath2, serialized_data));

                subWEAModel rootobj = JsonConvert.DeserializeObject<subWEAModel>(serialized_data);
                return Ok(pensionDeptHelper.pensionAppWEASub(rootobj));
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return Ok(objdata);
        }

        #endregion
    }

    public static class pensionDeptHelper
    {

        static CommonSPHel _Hel = new CommonSPHel();


        //Prod Urls
        static string GrievenceListUrl = "https://sspensions.ap.gov.in/gswsweadata/rest/generic1/getGrievenceDetails";
        static string personGrevenacneDetailsUrl = "https://sspensions.ap.gov.in/gswsweadata/rest/getpensionerdetails/getgrievanceIdDetails";
        static string WEAsubrUrl = "https://sspensions.ap.gov.in/gswsweadata/rest/genericGsws/newApplicationByWEA";
        static string personDetailsUrl = "https://sspensions.ap.gov.in/TESTING/rest/NewApplicationAadharValidation/validateAadhar";
        static string panchayatMasterUrl = "https://sspensions.ap.gov.in/gswsweadata/rest/generic/MastersData";
        static string tokenrUrl = "https://sspensions.ap.gov.in/GSWSPenToken/rest/gsws/tokenGeneration";
		static string subrUrl = "https://uat-nbpsdm.aponline.gov.in/NTRBPensions/rest/gsws/newApplication";//"https://sspensions.ap.gov.in/gswsweadata/rest/gsws/newApplication";

		#region DA Login

		public static dynamic personDetails(pensionDeptModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.password = "B205A8A530701C7824C52D6AE65B2BAD";
                obj.userName = "SSPPENSIONS";
                string pensionResponse = string.Empty;
                try
                {
                    pensionResponse = POST_RequestAsync(personDetailsUrl, JsonConvert.SerializeObject(obj));
                }
                catch (Exception ex)
                {
                    objdata.status = false;
                    objdata.result = "RESPONSE FROM PENSION DEPT : " + ex.Message.ToString();
                    return objdata;
                }
                if (string.IsNullOrEmpty(pensionResponse))
                {
                    objdata.status = false;
                    objdata.result = "No Response from pension department service !!!, Please try after some time";
                }

                PanchayatMastersModel panchayatReq = new PanchayatMastersModel();
                panchayatReq.flag = "S";
                panchayatReq.password = "YsrPension$2020";
                panchayatReq.secretariatId = obj.secId;
                panchayatReq.userName = "YsrPension";
                string panchayatResponse = string.Empty;
                try
                {
                    panchayatResponse = POST_RequestAsync(panchayatMasterUrl, JsonConvert.SerializeObject(panchayatReq));
                }
                catch (Exception ex)
                {
                    objdata.status = false;
                    objdata.result = "RESPONSE FROM PENSION DEPT : " + ex.Message.ToString();
                    return objdata;
                }
                panchayatResponseModel panchayatRespObj = JsonConvert.DeserializeObject<panchayatResponseModel>(panchayatResponse);
                if (string.IsNullOrEmpty(panchayatResponse) || panchayatRespObj.PanchayatList == null)
                {
                    objdata.status = false;
                    objdata.result = "No Response from pension department panchayat list service !!!, Please try after some time";
                }


                pensionResponseModel pensionRespObj = JsonConvert.DeserializeObject<pensionResponseModel>(pensionResponse);
                if (pensionRespObj.responseCode == "102")
                {
                    casteModel objCaste = new casteModel();
                    objCaste.type = "1";
                    DataTable dt = casteProc(objCaste);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        objdata.status = true;
                        objdata.result = pensionRespObj;
                        objdata.PanchayatList = panchayatRespObj.PanchayatList;
                        objdata.casteList = dt;
                    }
                    else
                    {
                        objdata.status = false;
                        objdata.result = "Castes Not Available to load !!!";
                    }
                }
                else
                {
                    objdata.status = false;
                    objdata.result = "RESPONSE FROM PENSION DEPT : " + pensionRespObj.reason;
                }
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public static dynamic habitationList(habitationMasterModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.flag = "P";
                obj.password = "YsrPension$2020";
                obj.userName = "YsrPension";

                string habResponse = POST_RequestAsync(panchayatMasterUrl, JsonConvert.SerializeObject(obj));
                habResponseModel habRespObj = JsonConvert.DeserializeObject<habResponseModel>(habResponse);
                if (string.IsNullOrEmpty(habResponse))
                {
                    objdata.status = false;
                    objdata.result = "No Response from pension department habitation list service !!!, Please try after some time";
                }

                if (habRespObj.HabitationList != null)
                {
                    objdata.status = true;
                    objdata.result = habRespObj.HabitationList;
                }
                else
                {
                    objdata.status = false;
                    objdata.result = "NO RESPONSE FROM PENSION DEPT : ";
                }
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = "RESPONSE FROM PENSION DEPT : " + ex.Message.ToString();
            }
            return objdata;
        }

        public static dynamic subCasteList(casteModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.type = "2";
                DataTable dt = casteProc(obj);

                if (dt != null && dt.Rows.Count > 0)
                {
                    objdata.status = true;
                    objdata.result = dt;
                }
                else
                {
                    objdata.status = false;
                    objdata.result = "Sub Castes Not Available to load !!!";
                }
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public static dynamic pensionAppSub(pensionAppSubModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                if (obj.pensionType == "3" && string.IsNullOrEmpty(obj.disabilityId))
                {
                    objdata.status = false;
                    objdata.result = "Please Enter SADAREM ID";
                    return objdata;
                }

                obj.encrypted_data = obj.encrypted_data.Replace(" ", "+");
                string decrypted_text = EncryptDecryptAlgoritham.DecryptStringAES(obj.encrypted_data, "3fee5395f01bee349feed65629bd442a", obj.iv);
                decModel decData = JsonConvert.DeserializeObject<decModel>(decrypted_text);

                tokenModel objToken = new tokenModel();
                objToken.username = "Pensionapp";
                objToken.password = "zg3zw/JwQ8r1u5d59kmKGl5s3jas4+yyZFmPaJbha0Nr2e1CsfqzrCLMxt+6ANo1";
                objToken.wdsLoginId = decData.USERNAME;

                string tokenResponse = string.Empty;
                try
                {
                    tokenResponse = POST_RequestAsync(tokenrUrl, JsonConvert.SerializeObject(objToken));
                }
                catch (Exception ex)
                {
                    objdata.status = false;
                    objdata.result = "RESPONSE FROM PENSION DEPT : " + ex.Message.ToString();
                    return objdata;
                }

                tokenResponseModel tokenRespObj = JsonConvert.DeserializeObject<tokenResponseModel>(tokenResponse);
                if (string.IsNullOrEmpty(tokenRespObj.Token))
                {
                    objdata.status = false;
                    objdata.result = "No Response from pension department token service !!!, Please try after some time";
                    return objdata;
                }

                obj.loginId = decData.USERNAME;
                obj.userName = "PensionNewApp";
                obj.password = "GswsPension@2020";
                obj.token = tokenRespObj.Token;
                obj.transactionId = decData.PS_TXN_ID;
                obj.systemIp = HttpContext.Current.Request.UserHostAddress;

                string subResponse = string.Empty;
                try
                {
                    subResponse = POST_RequestAsync(subrUrl, JsonConvert.SerializeObject(obj));
                }
                catch (Exception ex)
                {
                    objdata.status = false;
                    objdata.result = "RESPONSE FROM PENSION DEPT : " + ex.Message.ToString();
                    return objdata;
                }
                subRespModel subResp = JsonConvert.DeserializeObject<subRespModel>(subResponse);

                if (subResp.Response != null && subResp.Response.status_Code == "101")
                {
                    transactionModel objTrans = new transactionModel();
                    objTrans.TYPE = "2";
                    objTrans.TXN_ID = decData.PS_TXN_ID;
                    objTrans.DEPT_ID = "31";
                    objTrans.DEPT_TXN_ID = subResp.Response.BenTransId;
                    objTrans.BEN_ID = obj.aadharNumber;
                    objTrans.STATUS_CODE = subResp.Response.status_Code;
                    objTrans.REMARKS = subResp.Response.remarks;

                    transactionHelper transHel = new transactionHelper();
                    DataTable dt = transHel.transactionInsertion(objTrans);
                    if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                    {
                        objdata.status = true;
                        objdata.result = subResp.Response.remarks + ", YOUR Grievance ID IS : " + subResp.Response.BenTransId;
                    }
                    else
                    {
                        objdata.status = false;
                        objdata.result = "Failed to submit Details, Please try again !!!";
                    }
                }
                else
                {
                    objdata.status = false;
                    objdata.result = "RESPONSE FROM PENSION DEPT : " + subResp.Response.remarks;
                }
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public static DataTable casteProc(casteModel obj)
        {
            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "CERT_CASTE_SUB_CASTE";
                cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2).Value = obj.type;
                cmd.Parameters.Add("P_CASTE_CAT_ID", OracleDbType.Varchar2).Value = obj.casteId;
                cmd.Parameters.Add("P_SUB_CASTE_ID", OracleDbType.Varchar2).Value = obj.subCasteId;
                cmd.Parameters.Add("p_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                DataTable dtstatus = _Hel.GetProdgswsDataAdapter(cmd);
                return dtstatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region WEA Login
        public static dynamic grevianceList(grevianceListModeL obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {

                obj.password = "YsrPension$2020";
                obj.userName = "YsrPension";
                string pensionResponse = POST_RequestAsync(GrievenceListUrl, JsonConvert.SerializeObject(obj));
                if (string.IsNullOrEmpty(pensionResponse))
                {
                    objdata.status = false;
                    objdata.result = "No Response from pension department service !!!, Please try after some time";
                }

                greRespModel pensionRespObj = JsonConvert.DeserializeObject<greRespModel>(pensionResponse);
                if (pensionRespObj.response == "success")
                {
                    objdata.status = true;
                    objdata.result = pensionRespObj.sectraite;
                }
                else
                {
                    objdata.status = false;
                    objdata.result = "RESPONSE FROM PENSION DEPT : " + pensionRespObj.sectraiteList;
                }
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = "RESPONSE FROM PENSION DEPT : " + ex.Message.ToString();
            }
            return objdata;
        }

        public static dynamic personGrevDetails(personGrevDetailsModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {

                obj.password = "B205A8A530701C7824C52D6AE65B2BAD";
                obj.userName = "SSPPENSIONS";
                string pensionResponse = POST_RequestAsync(personGrevenacneDetailsUrl, JsonConvert.SerializeObject(obj));
                if (string.IsNullOrEmpty(pensionResponse))
                {
                    objdata.status = false;
                    objdata.result = "No Response from pension department service !!!, Please try after some time";
                }

                grevDetailsRespModel pensionRespObj = JsonConvert.DeserializeObject<grevDetailsRespModel>(pensionResponse);
                if (pensionRespObj.responseCode == "101")
                {
                    objdata.status = true;
                    objdata.result = pensionRespObj;
                }
                else
                {
                    objdata.status = false;
                    objdata.result = "RESPONSE FROM PENSION DEPT : " + pensionRespObj.reason;
                }
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = "RESPONSE FROM PENSION DEPT : " + ex.Message.ToString();
            }
            return objdata;
        }

        public static dynamic pensionAppWEASub(subWEAModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.data.userName = "PensionNewApp";
                obj.data.password = "GswsPension@2020";
                obj.data.systemIp = HttpContext.Current.Request.UserHostAddress;

                string subResponse = string.Empty;
                try
                {
                    subResponse = POST_RequestAsync(WEAsubrUrl, JsonConvert.SerializeObject(obj.data));
                }
                catch (Exception ex)
                {
                    objdata.status = false;
                    objdata.result = "RESPONSE FROM PENSION DEPT : " + ex.Message.ToString();
                    return objdata;
                }
                subWEARespModel subResp = JsonConvert.DeserializeObject<subWEARespModel>(subResponse);

                if (subResp.Response != null && subResp.Response.status_Code == "104")
                {

                    ScheduleTransactionModel obja = new ScheduleTransactionModel();

                    obja.TYPE = "1";
                    obja.DEPARTMENT_APPLICATION_ID = obj.data.aadharNumber;
                    obja.DEPARTMENT_Transaction_ID = obj.data.grievanceId;
                    obja.GSWS_TRANS_ID = obj.txnId;
                    obja.STATUS_CODE = subResp.Response.status_Code;
                    obja.STATUS_MESSAGE = subResp.Response.Remarks;
                    obja.SERVICE_NAME = "New Pension Verification";



                    transactionHelper transHel = new transactionHelper();
                    DataTable dt = transHel.TransactionSchedule_TaskSP(obja);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        objdata.status = true;
                        objdata.result = subResp.Response.Remarks;
                    }
                    else
                    {
                        objdata.status = false;
                        objdata.result = "Failed to submit Details, Please try again !!!";
                    }
                }
                else
                {
                    objdata.status = false;
                    objdata.result = subResp.Response.Remarks;
                }
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        #endregion

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

    }


}
