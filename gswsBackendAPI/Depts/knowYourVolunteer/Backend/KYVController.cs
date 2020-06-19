using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using gswsBackendAPI.DL.CommonHel;
using gswsBackendAPI.Depts.PensionVolunteerMapping.Backend;

namespace gswsBackendAPI.Depts.knowYourVolunteer.Backend
{
    [RoutePrefix("api/KYV")]
    public class KYVController : ApiController
    {
        captchahelper _chel = new captchahelper();

        [HttpPost]
        [Route("GetCaptcha")]
        public dynamic GetCaptcha(dynamic data)
        {
            dynamic _response = new ExpandoObject();
            captchahelper _chel = new captchahelper();
            string jsondata = JsonConvert.SerializeObject(data);
            try
            {
                captch val = JsonConvert.DeserializeObject<captch>(jsondata);
                return Ok(_chel.check_s_captch(val));
            }
            catch (Exception ex)
            {
                _response.Status = 102;
                _response.Reason = "Something Went wrong Please Try again";
                return Ok(_response);
            }
        }

        [Route("volunteerDetails")]
        [HttpPost]
        public IHttpActionResult volunteerDetails(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                string serialized_data = JsonConvert.SerializeObject(data);
                familyModel rootobj = JsonConvert.DeserializeObject<familyModel>(serialized_data);
                return Ok(KYVHelper.volunteerDetails(rootobj));
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return Ok(objdata);
        }

        [HttpPost]
        [Route("loadClusters")]
        public IHttpActionResult loadClusters(dynamic data)
        {
            pensionHelper _Hel = new pensionHelper();
            dynamic objdata = new ExpandoObject();

            try
            {
                string value = token_gen.Authorize_aesdecrpty(data);
                pensionModel rootobj = JsonConvert.DeserializeObject<pensionModel>(value);
                return Ok(_Hel.loadClusters(rootobj));
            }

            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = "Session Expired !!! Please login again to continue...";
                return Ok(objdata);

            }
        }

        [Route("districtList")]
        [HttpPost]
        public IHttpActionResult districtList(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                string serialized_data = JsonConvert.SerializeObject(data);
                NewaadherModel rootobj = JsonConvert.DeserializeObject<NewaadherModel>(serialized_data);
                return Ok(KYVHelper.districtList(rootobj));
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return Ok(objdata);
        }

        [Route("mandalList")]
        [HttpPost]
        public IHttpActionResult mandalList(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                string serialized_data = JsonConvert.SerializeObject(data);
                NewaadherModel rootobj = JsonConvert.DeserializeObject<NewaadherModel>(serialized_data);
                return Ok(KYVHelper.mandalList(rootobj));
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return Ok(objdata);
        }

        [Route("secList")]
        [HttpPost]
        public IHttpActionResult secList(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                string serialized_data = JsonConvert.SerializeObject(data);
                NewaadherModel rootobj = JsonConvert.DeserializeObject<NewaadherModel>(serialized_data);
                return Ok(KYVHelper.secList(rootobj));
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return Ok(objdata);
        }

        [Route("citizenDetailsSub")]
        [HttpPost]
        public IHttpActionResult citizenDetailsSub(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                string serialized_data = JsonConvert.SerializeObject(data);
                string mappath2 = HttpContext.Current.Server.MapPath("citizenDetailsSubLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath2, serialized_data));
                NewaadherModel rootobj = JsonConvert.DeserializeObject<NewaadherModel>(serialized_data);
                return Ok(KYVHelper.citizenDetailsSub(rootobj));
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return Ok(objdata);
        }

        [Route("citizenDetails")]
        [HttpPost]
        public IHttpActionResult citizenDetails(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                string serialized_data = token_gen.Authorize_aesdecrpty(data);
                NewaadherModel rootobj = JsonConvert.DeserializeObject<NewaadherModel>(serialized_data);
                return Ok(KYVHelper.citizenDetails(rootobj));
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return Ok(objdata);
        }

        [Route("citizenUpdate")]
        [HttpPost]
        public IHttpActionResult citizenUpdate(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                string serialized_data = token_gen.Authorize_aesdecrpty(data);
                string mappath2 = HttpContext.Current.Server.MapPath("citizenUpdateLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath2, serialized_data));
                NewaadherModel rootobj = JsonConvert.DeserializeObject<NewaadherModel>(serialized_data);
                return Ok(KYVHelper.citizenUpdate(rootobj));
            }
            catch (Exception ex)
            {
                objdata.status = false;
                objdata.result = ex.Message.ToString();
            }
            return Ok(objdata);
        }


    }

    public static class KYVHelper
    {

        static CommonSPHel _Hel = new CommonSPHel();

        public static dynamic volunteerDetails(familyModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                string captchaverify = new captchahelper().GetCaptchVerify(obj);
                if (captchaverify == "Success")
                {

                    obj.type = "2";
                    DataTable dt = familyProc(obj);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        objdata.success = true;
                        objdata.result = dt;
                    }
                    else
                    {
                        objdata.success = false;
                        objdata.result = "Your Aadhaar Number is not surveyed !!!";
                    }
                }
                else
                {
                    objdata.Status = "102";
                    objdata.result = "Invalid Captcha";
                }

            }
            catch (Exception ex)
            {
                objdata.success = false;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public static dynamic citizenDetails(NewaadherModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.type = "5";
                DataTable dt = citizenGrevProc(obj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    objdata.success = true;
                    objdata.result = dt;
                }
                else
                {
                    objdata.success = false;
                    objdata.result = "No Citizens are available assign !!!, Please try again !!!";
                }
            }
            catch (Exception ex)
            {
                objdata.success = false;
                objdata.result = ex.Message.ToString();
            }

            return objdata;
        }

        public static dynamic districtList(NewaadherModel obj)
        {

            dynamic objdata = new ExpandoObject();
            try
            {
                obj.type = "1";
                DataTable dt = citizenGrevProc(obj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    objdata.success = true;
                    objdata.result = dt;
                }
                else
                {
                    objdata.success = false;
                    objdata.result = "No Districts available to load, Please try again !!!";

                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public static dynamic mandalList(NewaadherModel obj)
        {

            dynamic objdata = new ExpandoObject();
            try
            {
                obj.type = "2";
                DataTable dt = citizenGrevProc(obj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    objdata.success = true;
                    objdata.result = dt;
                }
                else
                {
                    objdata.success = false;
                    objdata.result = "No Mandals available to load, Please try again !!!";
                }

            }
            catch (Exception ex)
            {
                objdata.success = false;
                objdata.result = ex.Message.ToString();
            }

            return objdata;
        }

        public static dynamic secList(NewaadherModel obj)
        {

            dynamic objdata = new ExpandoObject();
            try
            {
                obj.type = "3";
                DataTable dt = citizenGrevProc(obj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    objdata.success = true;
                    objdata.result = dt;
                }
                else
                {
                    objdata.success = false;
                    objdata.result = "No Secretariats available to load, Please try again !!!";
                }

            }
            catch (Exception ex)
            {
                objdata.success = false;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public static dynamic citizenDetailsSub(NewaadherModel obj)
        {

            dynamic objdata = new ExpandoObject();

            try
            {
                string captchaverify = new captchahelper().GetCaptchVerify(obj);
                if (captchaverify == "Success")
                {
                    obj.type = "4";
                    DataTable dt = citizenGrevProc(obj);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        objdata.success = true;
                        objdata.result = dt;

                        string message = "క్లస్టర్ నందు హౌస్ హోల్డ్ మాపింగ్ చేసుకొనుటకు మీ యొక్క రిక్వెస్ట్ ను మీ సచివాలయం యొక్క పంచాయితీ సెక్రటరీ కు పంపబడినది.\n";
                        message += "మీ యొక్క రిక్వెస్ట్ ఐ డి నెంబర్ : " + dt.Rows[0]["GREVIANCE_ID"].ToString() + "\n";
                        message += "పంచాయితీ సెక్రటరీ మొబైల్ నెంబర్ : " + dt.Rows[0]["MOBILE_NUMBER"].ToString() + "\n";
                        message += "సచివాలయం పేరు : " + dt.Rows[0]["SEC_NAME"].ToString() + "\n";
                        SMSService.SMSService objSMS = new SMSService.SMSService();
                        objSMS.SendTeluguSMS(obj.mobileNumber, message);
                    }
                    else
                    {
                        objdata.success = false;
                        objdata.result = "Failed to submit details, Please try again !!!";
                    }
                }
                else
                {
                    objdata.Status = false;
                    objdata.result = "Invalid Captcha";
                }

            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                if (message.Contains("ORA-00001"))
                {
                    objdata.success = false;
                    objdata.result = "Grevenance Aleary raised, Please contact your secretariat Panchayat Secretary !!!";
                }
                else
                {
                    objdata.success = false;
                    objdata.result = message;
                }
            }


            return objdata;
        }

        public static dynamic citizenUpdate(NewaadherModel obj)
        {

            dynamic objdata = new ExpandoObject();

            try
            {
                obj.type = "6";
                DataTable dt = citizenGrevProc(obj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    objdata.success = true;
                    objdata.result = "Citizen Details Mapped Successfully !!!";
                }
                else
                {
                    objdata.success = false;
                    objdata.result = "Failed to submit details, Please try again !!!";
                }

            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                objdata.success = false;
                objdata.result = message;
            }


            return objdata;
        }

        public static DataTable familyProc(familyModel obj)
        {
            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "gsws_get_family_details";
                cmd.Parameters.Add("ptype", OracleDbType.Varchar2).Value = obj.type;
                cmd.Parameters.Add("pdist", OracleDbType.Varchar2).Value = obj.districtId;
                cmd.Parameters.Add("pmandal", OracleDbType.Varchar2).Value = obj.mandalId;
                cmd.Parameters.Add("psecretariat", OracleDbType.Varchar2).Value = obj.secId;
                cmd.Parameters.Add("pcluster", OracleDbType.Varchar2).Value = obj.clusterId;
                cmd.Parameters.Add("puid_num", OracleDbType.Varchar2).Value = obj.uidNum;
                cmd.Parameters.Add("phh_id", OracleDbType.Varchar2).Value = obj.hhID;
                cmd.Parameters.Add("p_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                DataTable dtstatus = _Hel.GetProdgswsDataAdapter(cmd);
                return dtstatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable PensionVolunteerMappingProc(pensionModel obj)
        {

            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GSWS_PENSION_VV_MAPPING_PROC";
                cmd.Parameters.Add("ptype", OracleDbType.Varchar2).Value = obj.type;
                cmd.Parameters.Add("psec_id", OracleDbType.Varchar2).Value = obj.gsws_code;
                cmd.Parameters.Add("pPENSIONID", OracleDbType.Varchar2).Value = obj.pension_id;
                cmd.Parameters.Add("pvv_id", OracleDbType.Varchar2).Value = obj.vv_id;
                cmd.Parameters.Add("pUPDATED_ON", OracleDbType.TimeStamp).Value = DateTime.Now;
                cmd.Parameters.Add("pUPDATED_BY", OracleDbType.Varchar2).Value = obj.updated_by;
                cmd.Parameters.Add("pvv_name", OracleDbType.Varchar2).Value = obj.vv_name;
                cmd.Parameters.Add("pCLUSTER_ID", OracleDbType.Varchar2).Value = obj.cluster_id;
                cmd.Parameters.Add("pCLUSTER_NAME", OracleDbType.Varchar2).Value = obj.cluster_name;
                cmd.Parameters.Add("pcur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                DataTable dt = _Hel.GetyouthserviceDataAdapter(cmd);//db.executeProcedure(cmd, oradb_youth_service);
                return dt;
            }
            catch (Exception ex)
            {
                string mappath = HttpContext.Current.Server.MapPath("pensionVolunteerMappingProcedure");
                string serialized_data = JsonConvert.SerializeObject(obj);
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, serialized_data));
                throw ex;
            }
        }

        public static DataTable citizenGrevProc(NewaadherModel obj)
        {
            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GSWS_GRIEVIANCE_PROC";
                cmd.Parameters.Add("REPORT_TYPE", OracleDbType.Varchar2).Value = obj.type;
                cmd.Parameters.Add("P_UID_NUM", OracleDbType.Varchar2).Value = obj.uidNum;
                cmd.Parameters.Add("P_NAME", OracleDbType.Varchar2).Value = obj.citizenName;
                cmd.Parameters.Add("P_GENDER", OracleDbType.Varchar2).Value = obj.gender;
                cmd.Parameters.Add("P_DISTRICT_NAME", OracleDbType.Varchar2).Value = obj.districtName;
                cmd.Parameters.Add("P_DISTRICT_ID", OracleDbType.Varchar2).Value = obj.districtId;
                cmd.Parameters.Add("P_MANDAL_NAME", OracleDbType.Varchar2).Value = obj.mandalName;
                cmd.Parameters.Add("P_MANDAL_ID", OracleDbType.Varchar2).Value = obj.mandalId;
                cmd.Parameters.Add("P_SECRETARIAT_ID", OracleDbType.Varchar2).Value = obj.secId;
                cmd.Parameters.Add("P_SECRETARIAT_NAME", OracleDbType.Varchar2).Value = obj.secId;
                cmd.Parameters.Add("P_MOBILE_NUMBER", OracleDbType.Varchar2).Value = obj.mobileNumber;
                cmd.Parameters.Add("P_CLUSTER_ID", OracleDbType.Varchar2).Value = obj.clusterId;
                cmd.Parameters.Add("P_CLUSTER_NAME", OracleDbType.Varchar2).Value = obj.clusterName;
                cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2).Value = obj.status;
                cmd.Parameters.Add("P_UPDATED_BY", OracleDbType.Varchar2).Value = obj.updatedBy;
                cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                DataTable dtstatus = _Hel.GetProdgswsDataAdapter(cmd);
                return dtstatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    public class familyModel : LoginModel
    {
        public string type { get; set; }
        public string districtId { get; set; }
        public string mandalId { get; set; }
        public string secId { get; set; }
        public string clusterId { get; set; }
        public string uidNum { get; set; }
        public string hhID { get; set; }
    }

    public class UserData
    {
        public string PENSIONID { get; set; }
        public string PENSIONER_NAME { get; set; }
        public string RELATION_NAME { get; set; }
        public int SEC_ID { get; set; }
    }

    public class NewaadherModel : LoginModel
    {
        public string type { get; set; }
        public string uidNum { get; set; }
        public string citizenName { get; set; }
        public string gender { get; set; }
        public string mobileNumber { get; set; }
        public string districtName { get; set; }
        public string districtId { get; set; }
        public string mandalName { get; set; }
        public string mandalId { get; set; }
        public string secName { get; set; }
        public string secId { get; set; }
        public string clusterName { get; set; }
        public string clusterId { get; set; }
        public string status { get; set; }
        public string updatedBy { get; set; }

    }
}
