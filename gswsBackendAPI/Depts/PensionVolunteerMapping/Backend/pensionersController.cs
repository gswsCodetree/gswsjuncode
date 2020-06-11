using gswsBackendAPI.DL.CommonHel;
using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using OracleConnect;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace gswsBackendAPI.Depts.PensionVolunteerMapping.Backend
{
    [RoutePrefix("api/pensioners")]
    public class pensionersController : ApiController
    {

        pensionHelper _Hel = new pensionHelper();

        [HttpPost]
        [Route("loadClusters")]
        public IHttpActionResult loadClusters(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            pensionModel rootobj;
            try
            {
                string value = token_gen.Authorize_aesdecrpty(data);
                rootobj = JsonConvert.DeserializeObject<pensionModel>(value);

            }

            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = "Session Expired !!! Please login again to continue...";
                return Ok(objdata);

            }
            return Ok(_Hel.loadClusters(rootobj));
        }

        [HttpPost]
        [Route("loadPensioners")]
        public IHttpActionResult loadPensioners(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            pensionModel rootobj;
            try
            {
                string value = token_gen.Authorize_aesdecrpty(data);
                rootobj = JsonConvert.DeserializeObject<pensionModel>(value);
            }

            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = "Session Expired !!! Please login again to continue...";
                return Ok(objdata);
            }
            return Ok(_Hel.loadPensioners(rootobj));
        }

        [HttpPost]
        [Route("assignVounteer")]
        public IHttpActionResult assignVounteer(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            pensionModel rootobj;
            try
            {
                string value = token_gen.Authorize_aesdecrpty(data);
                string mappath = HttpContext.Current.Server.MapPath("assignVounteerToPensioners");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, value));
                rootobj = JsonConvert.DeserializeObject<pensionModel>(value);
            }

            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = "Session Expired !!! Please login again to continue...";
                return Ok(objdata);

            }
            return Ok(_Hel.assignVounteerToPensioners(rootobj));
        }

        [HttpPost]
        [Route("unassignVounteer")]
        public IHttpActionResult unassignVounteer(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            pensionModel rootobj;
            try
            {
                string value = token_gen.Authorize_aesdecrpty(data);
                string mappath = HttpContext.Current.Server.MapPath("unassignVounteerToPensioners");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, value));
                rootobj = JsonConvert.DeserializeObject<pensionModel>(value);
            }

            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = "Session Expired !!! Please login again to continue...";
                return Ok(objdata);
            }
            return Ok(_Hel.unassignVounteerToPensioners(rootobj));
        }

        [HttpPost]
        [Route("individualPensionData")]
        public IHttpActionResult individualPensionData(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            pensionModel rootobj;
            try
            {
                string value = token_gen.Authorize_aesdecrpty(data);
                rootobj = JsonConvert.DeserializeObject<pensionModel>(value);
            }

            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = "Session Expired !!! Please login again to continue...";
                return Ok(objdata);
            }
            return Ok(_Hel.individualPensionData(rootobj));
        }

        [HttpPost]
        [Route("assignSecretariat")]
        public IHttpActionResult assignSecretariat(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            pensionModel rootobj;
            try
            {
                string value = token_gen.Authorize_aesdecrpty(data);
                string mappath = HttpContext.Current.Server.MapPath("assignSecretariatToPensioners");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, value));
                rootobj = JsonConvert.DeserializeObject<pensionModel>(value);
            }

            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = "Session Expired !!! Please login again to continue...";
                return Ok(objdata);
            }
            return Ok(_Hel.assignSecretariatToPensioners(rootobj));
        }

        [HttpPost]
        [Route("assignedPensionerData")]
        public IHttpActionResult assignedPensionerData(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            pensionModel rootobj;
            try
            {
                string value = token_gen.Authorize_aesdecrpty(data);
                rootobj = JsonConvert.DeserializeObject<pensionModel>(value);
            }

            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = "Session Expired !!! Please login again to continue...";
                return Ok(objdata);
            }
            return Ok(_Hel.assignedPensionerData(rootobj));
        }

        [HttpPost]
        [Route("unassignPensioner")]
        public IHttpActionResult unassignPensioner(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            pensionModel rootobj;
            try
            {
                string value = token_gen.Authorize_aesdecrpty(data);
                string mappath = HttpContext.Current.Server.MapPath("unassignPensionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, value));
                rootobj = JsonConvert.DeserializeObject<pensionModel>(value);
            }

            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = "Session Expired !!! Please login again to continue...";
                return Ok(objdata);
            }
            return Ok(_Hel.unassignPensioner(rootobj));
        }

    }

    public  class pensionHelper:CommonSPHel
    {
       // public static string oradb_youth_service = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=apexadata-scan1.apsdc.ap.gov.in)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=APSPS16)));User Id=" + ConfigurationManager.AppSettings["youth_service_username"].ToString() + ";Password=" + ConfigurationManager.AppSettings["youth_service_password"].ToString() + ";";

        static LoginHelper _LoginHel = new LoginHelper();

        public  dynamic loadPensioners(pensionModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.type = "1";
                DataTable dt = pensionVolunteerMappingProc(obj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    objdata.status = 200;
                    objdata.result = dt;
                }
                else
                {
                    objdata.status = 400;
                    objdata.result = "No Pensioner available for this secretariat !!!";
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public  dynamic loadClusters(pensionModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.type = "2";
                DataTable dt = pensionVolunteerMappingProc(obj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    objdata.status = 200;
                    objdata.result = dt;
                }
                else
                {
                    objdata.status = 400;
                    objdata.result = "No Clusters available for this secretariat !!!";
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }
        public  dynamic individualPensionData(pensionModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.type = "5";
                DataTable dt = pensionVolunteerMappingProc(obj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["STATUS"].ToString() != "1")
                    {
                        objdata.status = 200;
                        objdata.result = dt;
                    }
                    else
                    {
                        objdata.status = 400;
                        objdata.result = "Pensioner already assigned to " + dt.Rows[0]["DISTRICT_NAME"].ToString() + " District, " + dt.Rows[0]["MANDAL_NAME"].ToString() + " Mandal, " + dt.Rows[0]["SACHIVALAYAM_NAME"].ToString() + " Sachivalayam, " + dt.Rows[0]["CLUSTER_NAME"].ToString() + " cluster";
                    }
                }
                else
                {
                    objdata.status = 400;
                    objdata.result = "No data available with the entered Pensioner ID";
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public dynamic assignVounteerToPensioners(pensionModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.type = "3";
                int count = pensionVolunteerMappingProc1(obj);
                if (count > 0)
                {
                    objdata.status = 200;
                    objdata.result = "Record Updated Successfully !!!";
                }
                else
                {
                    objdata.status = 400;
                    objdata.result = "Failed to update record, Please try again!!!";
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public  dynamic assignSecretariatToPensioners(pensionModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.type = "6";
                DataTable dt = pensionVolunteerMappingProc(obj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    objdata.status = 200;
                    objdata.result = "Record Updated Successfully !!!";
                }
                else
                {
                    objdata.status = 400;
                    objdata.result = "Failed to update record, Please try again!!!";
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public  dynamic unassignVounteerToPensioners(pensionModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.type = "4";
                int count = pensionVolunteerMappingProc1(obj);
                if (count > 0)
                {
                    objdata.status = 200;
                    objdata.result = "Record Updated Successfully !!!";
                }
                else
                {
                    objdata.status = 400;
                    objdata.result = "Failed to update record, Please try again!!!";
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public  dynamic assignedPensionerData(pensionModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.type = "7";
                DataTable dt = pensionVolunteerMappingProc(obj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    objdata.status = 200;
                    objdata.result = dt;
                }
                else
                {
                    objdata.status = 400;
                    objdata.result = "No data available with the entered Pensioner ID";
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public  dynamic unassignPensioner(pensionModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.type = "8";
                int count = pensionVolunteerMappingProc1(obj);
                if (count > 0)
                {
                    objdata.status = 200;
                    objdata.result = "Pensioner Unassigned Successfully !!!";
                }
                else
                {
                    objdata.status = 400;
                    objdata.result = "Failed to update record, Please try again!!!";
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public DataTable pensionVolunteerMappingProc(pensionModel obj)
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
				DataTable dt = GetyouthserviceDataAdapter(cmd);//db.executeProcedure(cmd, oradb_youth_service);
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

        public  int pensionVolunteerMappingProc1(pensionModel obj)
        {
            int count = 0;
            for (int i = 0; i < obj.user_data.Count; i++)
            {
                try
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.InitialLONGFetchSize = 1000;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GSWS_PENSION_VV_MAPPING_PROC";
                    cmd.Parameters.Add("ptype", OracleDbType.Varchar2).Value = obj.type;
                    cmd.Parameters.Add("psec_id", OracleDbType.Varchar2).Value = obj.gsws_code;
                    cmd.Parameters.Add("pPENSIONID", OracleDbType.Varchar2).Value = obj.user_data[i].PENSIONID;
                    cmd.Parameters.Add("pvv_id", OracleDbType.Varchar2).Value = obj.vv_id;
                    cmd.Parameters.Add("pUPDATED_ON", OracleDbType.TimeStamp).Value = DateTime.Now;
                    cmd.Parameters.Add("pUPDATED_BY", OracleDbType.Varchar2).Value = obj.updated_by;
                    cmd.Parameters.Add("pvv_name", OracleDbType.Varchar2).Value = obj.vv_name;
                    cmd.Parameters.Add("pCLUSTER_ID", OracleDbType.Varchar2).Value = obj.cluster_id;
                    cmd.Parameters.Add("pCLUSTER_NAME", OracleDbType.Varchar2).Value = obj.cluster_name;
                    cmd.Parameters.Add("pcur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
					DataTable dt = GetyouthserviceDataAdapter(cmd); //db.executeProcedure(cmd, oradb_youth_service);
                    if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                        count = count + 1;
                }
                catch (Exception ex)
                {
                    string mappath = HttpContext.Current.Server.MapPath("pensionVolunteerMappingProcedure");
                    string serialized_data = JsonConvert.SerializeObject(obj);
                    Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, serialized_data));
                }
            }
            return count;
        }
    }



    public class UserData
    {
        public string PENSIONID { get; set; }
        public string PENSIONER_NAME { get; set; }
        public string RELATION_NAME { get; set; }
        public int SEC_ID { get; set; }
    }

    public class pensionModel
    {
        public string vv_name { get; set; }
        public string cluster_id { get; set; }
        public string cluster_name { get; set; }
        public string type { get; set; }
        public string gsws_code { get; set; }
        public List<UserData> user_data { get; set; }
        public string vv_id { get; set; }
        public string updated_by { get; set; }
        public string pension_id { get; set; }
    }

}
