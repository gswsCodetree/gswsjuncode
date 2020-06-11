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

namespace gswsBackendAPI.Depts.knowYourVolunteer.Backend
{
    [RoutePrefix("api/KYV")]
    public class KYVController : ApiController
    {

        [Route("volunteerDetails")]
        [HttpPost]
        public IHttpActionResult volunteerDetails(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                string serialized_data = JsonConvert.SerializeObject(data); // token_gen.Authorize_aesdecrpty(data);
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
    }

    public static class KYVHelper
    {

        static CommonSPHel _Hel = new CommonSPHel();

        public static dynamic volunteerDetails(familyModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
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
            catch (Exception ex)
            {
                objdata.success = false;
                objdata.result = ex.Message.ToString();
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
    }

    public class familyModel
    {
        public string type { get; set; }
        public string districtId { get; set; }
        public string mandalId { get; set; }
        public string secId { get; set; }
        public string clusterId { get; set; }
        public string uidNum { get; set; }
        public string hhID { get; set; }
    }
}
