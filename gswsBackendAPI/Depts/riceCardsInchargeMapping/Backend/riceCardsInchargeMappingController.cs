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
using System.Web.Http;

namespace gswsBackendAPI.Depts.riceCardsInchargeMapping.Backend
{
    [RoutePrefix("api/riceCardsInchargeMapping")]
    public class riceCardsInchargeMappingController : ApiController
    {


        [Route("inChargesList")]
        [HttpPost]
        public IHttpActionResult inChargesList(dynamic data)
        {
            string serialized_data = token_gen.Authorize_aesdecrpty(data);
            clusterMappingModel rootobj = JsonConvert.DeserializeObject<clusterMappingModel>(serialized_data);
            return Ok(clusterMappingHelper.inChargesList(rootobj));
        }

        [Route("volunteersList")]
        [HttpPost]
        public IHttpActionResult volunteersList(dynamic data)
        {
            string serialized_data = token_gen.Authorize_aesdecrpty(data);
            clusterMappingModel rootobj = JsonConvert.DeserializeObject<clusterMappingModel>(serialized_data);
            return Ok(clusterMappingHelper.volunteersList(rootobj));
        }

        [Route("inChargeUpdate")]
        [HttpPost]
        public IHttpActionResult inChargeUpdate(dynamic data)
        {
            string serialized_data = token_gen.Authorize_aesdecrpty(data);
            clusterMappingModel rootobj = JsonConvert.DeserializeObject<clusterMappingModel>(serialized_data);
            return Ok(clusterMappingHelper.inChargeUpdate(rootobj));
        }

    }

    public static class clusterMappingHelper
    {
        static CommonSPHel _Hel = new CommonSPHel();



        public static dynamic volunteersList(clusterMappingModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.type = "24";
                DataTable dt = clusterMappingProc(obj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    objdata.success = true;
                    objdata.result = dt;
                }
                else
                {
                    objdata.success = false;
                    objdata.result = "Clusters Not available for this secretariat";
                }

            }
            catch (Exception ex)
            {
                objdata.success = false;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public static dynamic inChargesList(clusterMappingModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.type = "22";
                DataTable dt = clusterMappingProc(obj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    objdata.success = true;
                    objdata.result = dt;
                }
                else
                {
                    objdata.success = false;
                    objdata.result = "Clusters Not available for this secretariat";
                }

            }
            catch (Exception ex)
            {
                objdata.success = false;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public static dynamic inChargeUpdate(clusterMappingModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.type = "23";
                DataTable dt = clusterMappingProc(obj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    objdata.success = true;
                    objdata.result = dt;
                }
                else
                {
                    objdata.success = false;
                    objdata.result = "Failed to update Incharge, Please try again !!!";
                }

            }
            catch (Exception ex)
            {
                objdata.success = false;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public static DataTable clusterMappingProc(clusterMappingModel obj)
        {
            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GSWS_VOLUNTEER_DATA_UPDATE";
                cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2).Value = obj.type;
                cmd.Parameters.Add("P_DISTRICT_ID", OracleDbType.Varchar2).Value = obj.districtId;
                cmd.Parameters.Add("P_MANDAL_ID", OracleDbType.Varchar2).Value = obj.mandalId;
                cmd.Parameters.Add("P_SEC_ID", OracleDbType.Varchar2).Value = obj.secId;
                cmd.Parameters.Add("P_CLUSTER_ID", OracleDbType.Varchar2).Value = obj.clusterId;
                cmd.Parameters.Add("P_FLAG_U_R", OracleDbType.Varchar2).Value = obj.ruralUrbanFlag;
                cmd.Parameters.Add("P_CLUSTER_NAME", OracleDbType.Varchar2).Value = obj.clusterName;
                cmd.Parameters.Add("P_VOL_NAME", OracleDbType.Varchar2).Value = obj.volName;
                cmd.Parameters.Add("P_VOL_UID", OracleDbType.Varchar2).Value = obj.volUid;
                cmd.Parameters.Add("P_VOL_MOBILE", OracleDbType.Varchar2).Value = obj.volMobile;
                cmd.Parameters.Add("P_VOL_CFMS_ID", OracleDbType.Varchar2).Value = obj.volCfmsId;
                cmd.Parameters.Add("P_VOL_TYPE", OracleDbType.Varchar2).Value = obj.volType;
                cmd.Parameters.Add("P_USER_ID", OracleDbType.Varchar2).Value = obj.userId;
                cmd.Parameters.Add("P_PWD", OracleDbType.Varchar2).Value = obj.password;
                cmd.Parameters.Add("P_REASON", OracleDbType.Varchar2).Value = obj.reason;
                cmd.Parameters.Add("P_REMARKS", OracleDbType.Varchar2).Value = obj.remarks;
                cmd.Parameters.Add("p_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                DataTable dtstatus = _Hel.GetgswsDataAdapter(cmd);
                return dtstatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class clusterMappingModel
    {
        public string mobileNum { get; set; }
        public string sessionId { get; set; }
        public string otp { get; set; }
        public string remarks { get; set; }
        public string reason { get; set; }
        public string password { get; set; }
        public string captchaData { get; set; }
        public string captchaChiper { get; set; }
        public string type { get; set; }
        public string districtId { get; set; }
        public string mandalId { get; set; }
        public string secId { get; set; }
        public string clusterId { get; set; }
        public string ruralUrbanFlag { get; set; }
        public string clusterName { get; set; }
        public string volName { get; set; }
        public string volUid { get; set; }
        public string volMobile { get; set; }
        public string volCfmsId { get; set; }
        public string volType { get; set; }
        public string userId { get; set; }
    }
}
