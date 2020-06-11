using gswsBackendAPI.Depts.RationVolunteermapping.Backend;
using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace gswsBackendAPI.Depts.clusterHHGreaterMapping.Backend
{
    [RoutePrefix("api/HHGreater")]
    public class HHGreaterController : ApiController
    {
        [HttpPost]
        [Route("Citizendetails")]
        public IHttpActionResult Citizendetails(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                string value = token_gen.Authorize_aesdecrpty(data);
                householdMappingModel rootobj = JsonConvert.DeserializeObject<householdMappingModel>(value);
                return Ok(householdMappingHelper.Citizendetails(rootobj));
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = "Session Expired !!! Please login again to continue...";
                return Ok(objdata);

            }
        }

        [HttpPost]
        [Route("clusterList")]
        public IHttpActionResult clusterList(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                string value = token_gen.Authorize_aesdecrpty(data);
                householdMappingModel rootobj = JsonConvert.DeserializeObject<householdMappingModel>(value);
                return Ok(householdMappingHelper.clusterList(rootobj));
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = "Session Expired !!! Please login again to continue...";
                return Ok(objdata);

            }
        }

        [HttpPost]
        [Route("addMember")]
        public IHttpActionResult addMember(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                string value = token_gen.Authorize_aesdecrpty(data);
                householdMappingModel rootobj = JsonConvert.DeserializeObject<householdMappingModel>(value);
                return Ok(householdMappingHelper.addMember(rootobj));
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = "Session Expired !!! Please login again to continue...";
                return Ok(objdata);

            }
        }

        

        [HttpPost]
        [Route("dataSubmission")]
        public IHttpActionResult dataSubmission(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                string serialized_data = token_gen.Authorize_aesdecrpty(data);
                Logdatafile.store(serialized_data, "dataSubmissionnLogs");
                personList rootobj = JsonConvert.DeserializeObject<personList>(serialized_data);
                return Ok(householdMappingHelper.dataSubmission(rootobj));
            }
            catch (Exception ex)
            {
                string serialized_log_data = JsonConvert.SerializeObject(data);
                string logData = "{'exception' : '" + ex.Message.ToString() + "','data': '" + serialized_log_data + "'}";
                Logdatafile.store(logData, "OuterdataSubmissionLogs");

                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return Ok(objdata);
        }

        [HttpPost]
        [Route("sendOTP")]
        public IHttpActionResult sendOTP(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                string value = token_gen.Authorize_aesdecrpty(data);
                householdMappingModel rootobj = JsonConvert.DeserializeObject<householdMappingModel>(value);
                return Ok(householdMappingHelper.sendOTP(rootobj));
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = "Session Expired !!! Please login again to continue...";
                return Ok(objdata);

            }
        }


    }


    public static class householdMappingHelper
    {
        public static dynamic dataSubmission(personList obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.responseData[0].TEMP_ID = "HH" + obj.insertedBy + DateTime.Now.ToString("yyyyMMddhhmmssfff");
                if (routerModule.citizenSubmissionProc(obj))
                {
                    objdata.status = 200;
                    objdata.result = "Data submitted successfully !!!";
                }
                else
                {
                    objdata.status = 400;
                    objdata.result = "Data submission failed,  Please try again !!!";
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public static dynamic Citizendetails(householdMappingModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                gswsModel objGsws = new gswsModel();
                objGsws.type = "8";
                objGsws.uid = obj.uidNum;
                DataTable dt = routerModule.householdMappingProc(objGsws);
                if (dt != null && dt.Rows.Count > 0)
                {
                    objdata.status = true;
                    objdata.result = dt;
                }
                else
                {
                    objdata.status = false;
                    objdata.result = "No data found for citizen aadhaar number";
                    ;
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
                return objdata;

            }
            return objdata;
        }

        public static dynamic clusterList(householdMappingModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                gswsModel objGsws = new gswsModel();
                objGsws.type = "9";
                objGsws.uid = obj.gsws_code;
                DataTable dt = routerModule.householdMappingProc(objGsws);
                if (dt != null && dt.Rows.Count > 0)
                {
                    objdata.status = true;
                    objdata.result = dt;
                }
                else
                {
                    objdata.status = false;
                    objdata.result = "No clusters available for this secretariat...";
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
                return objdata;

            }
            return objdata;
        }


        public static dynamic addMember(householdMappingModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                gswsModel objGsws = new gswsModel();
                objGsws.type = "7";
                objGsws.uid = obj.uidNum;
                DataTable dt = routerModule.householdMappingProc(objGsws);
                if (dt != null && dt.Rows.Count > 0)
                {
                    objdata.status = true;
                    objdata.result = dt;
                }
                else
                {
                    objdata.status = false;
                    objdata.result = "No data available for this aadhaar number...";
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
                return objdata;

            }
            return objdata;
        }


        public static dynamic sendOTP(householdMappingModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                SMSService.SMSService objSMS = new SMSService.SMSService();
                Random objRan = new Random();
                string otp = objRan.Next(111111, 999999).ToString();
                string message = "GSWS OTP : " + otp + "\n OTP will Expire in 15 minutes";
                string response = objSMS.SendTeluguSMS(obj.mobileNum, message);
                if (response.Contains("402"))
                {
                    objdata.status = true;
                    objdata.result = otp;
                }
                else
                {
                    objdata.status = false;
                    objdata.result = "Failed to send OTP please try again !!!";
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
                return objdata;

            }
            return objdata;
        }


    }

    public static class routerModule
    {

        private static string gsws_oradb_prod = ConfigurationManager.AppSettings["gswsRouterUrl"].ToString();
        private static string key = "d2e7ee118d6fb11b35dfb84b745fd3c8b643b70f33e8f0657b0b9c765b82390a";

        public static DataTable householdMappingProc(gswsModel obj)
        {
            try
            {

                List<inputModel> listInputObj = new List<inputModel>();


                inputModel inputObj = new inputModel();
                inputObj.paramName = "ptype"; inputObj.value = obj.type; inputObj.dataType = "Varchar2";
                listInputObj.Add(inputObj);
                inputModel inputObj1 = new inputModel();
                inputObj1.paramName = "puid"; inputObj1.value = obj.uid; inputObj1.dataType = "Varchar2";
                listInputObj.Add(inputObj1);
                inputModel inputObj2 = new inputModel();
                inputObj2.paramName = "pration"; inputObj2.value = obj.rationId; inputObj2.dataType = "Varchar2";
                listInputObj.Add(inputObj2);
                inputModel inputObj3 = new inputModel();
                inputObj3.paramName = "phh_id"; inputObj3.value = obj.hhId; inputObj3.dataType = "Varchar2";
                listInputObj.Add(inputObj3);

                requestModel procObj = new requestModel();
                procObj.refcursorName = "p_cur";
                procObj.procedureName = "gsws_vv_hh_mapping_proc";
                procObj.inputs = listInputObj;
                procObj.key = key;
                string json = JsonConvert.SerializeObject(procObj);
                DataTable dt = dbRouter.POST_Request(gsws_oradb_prod, json);
                if (dt != null && dt.Rows.Count > 0)
                    return dt;
                else
                    return new DataTable();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static bool citizenSubmissionProc(personList objList)
        {
            //Log when service failed
            string logger = "";

            try
            {
                logger += DateTime.Now.ToString() + " : counter initiated\n";
                int count = 0;

                string houseImage = "NA";



                foreach (ResponseData obj in objList.responseData)
                {
                    try
                    {
                        // For ekyc person household id not available so we're assigning tempId as household id in that case
                        if (string.IsNullOrEmpty(obj.HOUSEHOLD_ID))
                        {
                            obj.HOUSEHOLD_ID = objList.responseData[0].TEMP_ID;
                            logger += DateTime.Now.ToString() + "household id null so assigned temp id as household id  : " + obj.HOUSEHOLD_ID + "\n";
                        }

                        //Date conversion when date not available or not in a correct format

                        string date = string.Empty;
                        if (!string.IsNullOrEmpty(obj.DOB_DT))
                        {
                            logger += DateTime.Now.ToString() + "dob is not null for this person  : " + obj.UID_NUM + "\n";
                            if (obj.DOB_DT == "0")
                            {
                                date = "";
                                logger += DateTime.Now.ToString() + "dob is set to empty  : " + obj.UID_NUM + "\n";
                            }
                            else if (obj.DOB_DT.Length >= 10)
                            {
                                date = obj.DOB_DT.Substring(0, 10).Replace('-', '/').ToString();
                                logger += DateTime.Now.ToString() + "dob is directly assigned changed format to dd-mm-yyyy  : " + date + " : " + obj.UID_NUM + "\n";
                            }
                            else
                            {
                                //addition of zeros for 1-1-2001 to 01-01-2001
                                string[] dateParts = obj.DOB_DT.Split('-');
                                if (dateParts[0].Length == 1)
                                    dateParts[0] = "0" + dateParts[0];
                                if (dateParts[1].Length == 1)
                                    dateParts[1] = "0" + dateParts[1];
                                date = dateParts[0] + "/" + dateParts[1] + "/" + dateParts[2].Substring(0, 4).ToString();
                                logger += DateTime.Now.ToString() + "dob is changed from d-m-yyyy to dd-mm-yyyy  : " + date + " : " + obj.UID_NUM + "\n";
                            }
                        }


                        List<inputModel> listInputObj = new List<inputModel>();

                        inputModel inputObj = new inputModel();
                        inputObj.paramName = "ptype"; inputObj.value = "1"; inputObj.dataType = "Varchar2";
                        listInputObj.Add(inputObj);
                        inputModel inputObj1 = new inputModel();
                        inputObj1.paramName = "pCITIZEN_NAME"; inputObj1.value = obj.CITIZEN_NAME; inputObj1.dataType = "Varchar2";
                        listInputObj.Add(inputObj1);
                        inputModel inputObj2 = new inputModel();
                        inputObj2.paramName = "pDISTRICT_CODE"; inputObj2.value = string.IsNullOrEmpty(obj.DISTRICT_CODE) ? "0" : obj.DISTRICT_CODE; inputObj2.dataType = "Varchar2";
                        listInputObj.Add(inputObj2);
                        inputModel inputObj29 = new inputModel();
                        inputObj29.paramName = "pDOB_DT"; inputObj29.value = date; inputObj29.dataType = "Date";
                        listInputObj.Add(inputObj29);
                        inputModel inputObj3 = new inputModel();
                        inputObj3.paramName = "pDOOR_NO"; inputObj3.value = obj.DOOR_NO; inputObj3.dataType = "Varchar2";
                        listInputObj.Add(inputObj3);
                        inputModel inputObj4 = new inputModel();
                        inputObj4.paramName = "pFATHER_AADHAAR"; inputObj4.value = obj.FATHER_AADHAAR; inputObj4.dataType = "Varchar2";
                        listInputObj.Add(inputObj4);
                        inputModel inputObj5 = new inputModel();
                        inputObj5.paramName = "pGENDER"; inputObj5.value = obj.GENDER; inputObj5.dataType = "Varchar2";
                        listInputObj.Add(inputObj5);
                        inputModel inputObj6 = new inputModel();
                        inputObj6.paramName = "pGSWS_CODE"; inputObj6.value = obj.SECRETARIAT_CODE; inputObj6.dataType = "Varchar2";
                        listInputObj.Add(inputObj6);
                        inputModel inputObj7 = new inputModel();
                        inputObj7.paramName = "pHOUSEHOLD_ID"; inputObj7.value = obj.HOUSEHOLD_ID; inputObj7.dataType = "Varchar2";
                        listInputObj.Add(inputObj7);
                        inputModel inputObj8 = new inputModel();
                        inputObj8.paramName = "pINSERTED_BY"; inputObj8.value = objList.insertedBy; inputObj8.dataType = "Varchar2";
                        listInputObj.Add(inputObj8);
                        inputModel inputObj9 = new inputModel();
                        inputObj9.paramName = "pINSERTED_ON"; inputObj9.value = DateTime.Now.ToString(); inputObj9.dataType = "TimeStamp";
                        listInputObj.Add(inputObj9);
                        inputModel inputObj10 = new inputModel();
                        inputObj10.paramName = "pIS_HOFAMILY"; inputObj10.value = string.IsNullOrEmpty(obj.IS_HOFAMILY) ? "0" : obj.IS_HOFAMILY; inputObj10.dataType = "Varchar2";
                        listInputObj.Add(inputObj10);
                        inputModel inputObj11 = new inputModel();
                        inputObj11.paramName = "pIS_LIVING_WITHFAMILY"; inputObj11.value = string.IsNullOrEmpty(obj.IS_LIVING_WITHFAMILY) ? "0" : obj.IS_LIVING_WITHFAMILY; inputObj11.dataType = "Varchar2";
                        listInputObj.Add(inputObj11);
                        inputModel inputObj12 = new inputModel();
                        inputObj12.paramName = "pIS_MARRIED"; inputObj12.value = string.IsNullOrEmpty(obj.IS_MARRIED) ? "0" : obj.IS_MARRIED; inputObj12.dataType = "Varchar2";
                        listInputObj.Add(inputObj12);
                        inputModel inputObj13 = new inputModel();
                        inputObj13.paramName = "pIS_MEMBERADDED"; inputObj13.value = string.IsNullOrEmpty(obj.IS_MEMBERADDED) ? "0" : obj.IS_MEMBERADDED; inputObj13.dataType = "Varchar2";
                        listInputObj.Add(inputObj13);
                        inputModel inputObj14 = new inputModel();
                        inputObj14.paramName = "pIS_MEMBERDELETED"; inputObj14.value = string.IsNullOrEmpty(obj.IS_MEMBERDELETED) ? "0" : obj.IS_MEMBERDELETED; inputObj14.dataType = "Varchar2";
                        listInputObj.Add(inputObj14);
                        inputModel inputObj15 = new inputModel();
                        inputObj15.paramName = "pMANDAL_CODE"; inputObj15.value = string.IsNullOrEmpty(obj.MANDAL_CODE) ? "0" : obj.MANDAL_CODE; inputObj15.dataType = "Varchar2";
                        listInputObj.Add(inputObj15);
                        inputModel inputObj16 = new inputModel();
                        inputObj16.paramName = "pMOBILE_NUMBER"; inputObj16.value = obj.MOBILE_NUMBER; inputObj16.dataType = "Varchar2";
                        listInputObj.Add(inputObj16);
                        inputModel inputObj17 = new inputModel();
                        inputObj17.paramName = "pMOTHER_AADHAAR"; inputObj17.value = obj.MOTHER_AADHAAR; inputObj17.dataType = "Varchar2";
                        listInputObj.Add(inputObj17);
                        inputModel inputObj18 = new inputModel();
                        inputObj18.paramName = "pNEW_HH_ID"; inputObj18.value = ""; inputObj18.dataType = "Varchar2";
                        listInputObj.Add(inputObj18);
                        inputModel inputObj19 = new inputModel();
                        inputObj19.paramName = "pRELATION_WITHHOF"; inputObj19.value = obj.RELATION_WITHHOF; inputObj19.dataType = "Varchar2";
                        listInputObj.Add(inputObj19);
                        inputModel inputObj20 = new inputModel();
                        inputObj20.paramName = "pSPOUSE_AADHAAR"; inputObj20.value = obj.SPOUSE_AADHAAR; inputObj20.dataType = "Varchar2";
                        listInputObj.Add(inputObj20);
                        inputModel inputObj21 = new inputModel();
                        inputObj21.paramName = "pTEMP_ID"; inputObj21.value = objList.responseData[0].TEMP_ID; inputObj21.dataType = "Varchar2";
                        listInputObj.Add(inputObj21);
                        inputModel inputObj22 = new inputModel();
                        inputObj22.paramName = "pUID_NUM"; inputObj22.value = obj.UID_NUM; inputObj22.dataType = "Varchar2";
                        listInputObj.Add(inputObj22);
                        inputModel inputObj23 = new inputModel();
                        inputObj23.paramName = "pUPDATED_BY"; inputObj23.value = ""; inputObj23.dataType = "Varchar2";
                        listInputObj.Add(inputObj23);
                        inputModel inputObj24 = new inputModel();
                        inputObj24.paramName = "pUPDATED_ON"; inputObj24.value = DateTime.Now.ToString(); inputObj24.dataType = "TimeStamp";
                        listInputObj.Add(inputObj24);
                        inputModel inputObj25 = new inputModel();
                        inputObj25.paramName = "pHOUSE_IMAGE_PATH"; inputObj25.value = houseImage; inputObj25.dataType = "Varchar2";
                        listInputObj.Add(inputObj25);
                        inputModel inputObj26 = new inputModel();
                        inputObj26.paramName = "pCLUSTER_ID"; inputObj26.value = obj.CLUSTER_ID; inputObj26.dataType = "Varchar2";
                        listInputObj.Add(inputObj26);
                        inputModel inputObj27 = new inputModel();
                        inputObj27.paramName = "pCLUSTER_NAME"; inputObj27.value = obj.CLUSTER_NAME; inputObj27.dataType = "Varchar2";
                        listInputObj.Add(inputObj27);
                        inputModel inputObj28 = new inputModel();
                        inputObj28.paramName = "pMEMBER_STATUS"; inputObj28.value = obj.MAPPING_STATUS; inputObj28.dataType = "Varchar2";
                        listInputObj.Add(inputObj28);

                        requestModel procObj = new requestModel();
                        procObj.refcursorName = "p_cur";
                        procObj.procedureName = "GSWS_HH_VV_MAPP_INSERT_PROC";
                        procObj.inputs = listInputObj;
                        procObj.key = key;
                        string json = JsonConvert.SerializeObject(procObj);
                        logger += DateTime.Now.ToString() + "parameters assigned :  " + json + "\n";

                        Random rd = new Random();
                        string rdNum = rd.Next(11111, 99999).ToString();
                        DataTable dt = dbRouter.POST_Request(gsws_oradb_prod, json);
                        if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                        {
                            count++;
                            logger += DateTime.Now.ToString() + "record submitted successfully :  " + json + "\n";
                        }
                        else
                        {
                            logger += DateTime.Now.ToString() + "record submission failed :  " + json + "\n";
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.ToString().Contains("Object reference"))
                            Logdatafile.store(logger, "ObjectReferenceLogs", objList.responseData[0].TEMP_ID);

                        string serialized_data = JsonConvert.SerializeObject(objList);
                        string data = "{'exception' : '" + ex.Message.ToString() + "','data': '" + serialized_data + "'}";
                        Logdatafile.store(data, "citizenSubmissionIndividualExceptionLogs", obj.UID_NUM);
                    }
                }
                if (count == objList.responseData.Count)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().Contains("Object reference"))
                    Logdatafile.store(logger, "ObjectReferenceLogs", objList.responseData[0].TEMP_ID);

                string serialized_data = JsonConvert.SerializeObject(objList);
                string data = "{'exception' : '" + ex.Message.ToString() + "','data': '" + serialized_data + "'}";
                Logdatafile.store(data, "citizenSubmissionCompleteExceptionLogs", objList.responseData[0].TEMP_ID);
                throw ex;
            }

        }

    }

    public class gswsModel
    {
        public string type { get; set; }
        public string uid { get; set; }
        public string rationId { get; set; }
        public string hhId { get; set; }
        public string clusterId { get; set; }
    }

    public class householdMappingModel
    {
        public string uidNum { get; set; }
        public string gsws_code { get; set; }
        public string mobileNum { get; set; }
    }

    public class ResponseData
    {
        public string HOUSEHOLD_ID { get; set; }
        public string UID_NUM { get; set; }
        public string CITIZEN_NAME { get; set; }
        public string DOB_DT { get; set; }
        public string MOBILE_NUMBER { get; set; }
        public string GENDER { get; set; }
        public string IS_LIVING_WITHFAMILY { get; set; }
        public string IS_HOFAMILY { get; set; }
        public string IS_MEMBERADDED { get; set; }
        public string IS_MEMBERDELETED { get; set; }
        public string RELATION_WITHHOF { get; set; }
        public string FATHER_AADHAAR { get; set; }
        public string MOTHER_AADHAAR { get; set; }
        public string SPOUSE_AADHAAR { get; set; }
        public string IS_MARRIED { get; set; }
        public string TEMP_ID { get; set; }
        public string NEW_HH_ID { get; set; }
        public string DISTRICT_CODE { get; set; }
        public string DISTRICT_NAME { get; set; }
        public string MANDAL_CODE { get; set; }
        public string MANDAL_NAME { get; set; }
        public string SECRETARIAT_CODE { get; set; }
        public string SECRETARIAT_NAME { get; set; }
        public string DOOR_NO { get; set; }
        public string STATUS { get; set; }
        public string HOUSE_IMAGE_PATH { get; set; }
        public string CLUSTER_ID { get; set; }
        public string CLUSTER_NAME { get; set; }
        public string MAPPING_STATUS { get; set; }
        public string AGE { get; set; }
    }

    public class personList
    {
        public List<ResponseData> responseData { get; set; }
        public string insertedBy { get; set; }
    }

    public static class Logdatafile
    {

        private static string logPath = "\\\\Gws-storage01\\E\\logs\\web";
        private static string appName = "GSWSMainwebsite";

        public static void store(dynamic strMsg, string logsName, string fileName = "")
        {
            try
            {
                string strPath = logPath + "\\" + DateTime.Now.ToString("ddMMyyyy") + "\\" + appName + "\\" + logsName + "\\" + DateTime.Now.ToString("HH").ToString();
                if (!Directory.Exists(strPath))
                    Directory.CreateDirectory(strPath);
                string path2 = strPath + "\\" + "LOG_" + fileName + DateTime.Now.ToString("yyyyMMddhhmmssfff");
                using (StreamWriter sw = new StreamWriter(path2 + ".txt", true))
                {
                    sw.WriteLine(strMsg);
                    sw.Close();
                }
                //StreamWriter swLog = new StreamWriter(path2 + ".txt", true);
                //swLog.WriteLine(strMsg);
                //swLog.Close();
                //swLog.Dispose();
            }
            catch (Exception ex)
            {
                try
                {
                    string mappath = HttpContext.Current.Server.MapPath(logsName);
                    string strPath = mappath + "\\" + DateTime.Now.ToString("ddMMyyyy") + "\\" + DateTime.Now.ToString("HH").ToString();
                    if (!Directory.Exists(strPath))
                        Directory.CreateDirectory(strPath);
                    string path2 = strPath + "\\" + "LOG_" + fileName + DateTime.Now.ToString("yyyyMMddhhmmssfff");
                    using (StreamWriter sw = new StreamWriter(path2 + ".txt", true))
                    {
                        sw.WriteLine(strMsg);
                        sw.Close();
                    }
                }
                catch (Exception ex1)
                {

                }

            }
        }
    }

}
