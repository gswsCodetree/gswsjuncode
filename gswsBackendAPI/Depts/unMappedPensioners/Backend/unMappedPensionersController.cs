using gswsBackendAPI.Depts.RationVolunteermapping.Backend;
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

namespace gswsBackendAPI.Depts.unMappedPensioners.Backend
{
    [RoutePrefix("api/unMappedPensioners")]
    public class unMappedPensionersController : ApiController
    {

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
            return Ok(pensionHelper.loadClusters(rootobj));
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
            return Ok(pensionHelper.loadPensioners(rootobj));
        }

        [HttpPost]
        [Route("assignClusterToPensioner")]
        public IHttpActionResult assignClusterToPensioner(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            pensionModel rootobj;
            try
            {
                string value = token_gen.Authorize_aesdecrpty(data);
                string mappath = HttpContext.Current.Server.MapPath("assignClusterToPensionerLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, value));
                rootobj = JsonConvert.DeserializeObject<pensionModel>(value);
            }

            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = "Session Expired !!! Please login again to continue...";
                return Ok(objdata);

            }
            return Ok(pensionHelper.assignClusterToPensioner(rootobj));
        }

    }

    public static class pensionHelper
    {
        private static string gswsRouterUrl = ConfigurationManager.AppSettings["gswsRouterUrl"].ToString();
        private static string key = "d2e7ee118d6fb11b35dfb84b745fd3c8b643b70f33e8f0657b0b9c765b82390a";

        static LoginHelper _LoginHel = new LoginHelper();

        public static dynamic loadPensioners(pensionModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.type = "3";
                DataTable dt = unMappedPensionerMappingProc(obj);
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

        public static dynamic loadClusters(pensionModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.type = "2";
                DataTable dt = unMappedPensionerMappingProc(obj);
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

        public static dynamic assignClusterToPensioner(pensionModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.type = "4";
                DataTable dt = unMappedPensionerMappingProc(obj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    objdata.status = 200;
                    objdata.result = "cluster assgined successfully !!!";
                }
                else
                {
                    objdata.status = 400;
                    objdata.result = "failed to assign cluster, please try again !!!";
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public static DataTable unMappedPensionerMappingProc(pensionModel obj)
        {

            try
            {
                List<inputModel> listInputObj = new List<inputModel>();


                inputModel inputObj = new inputModel();
                inputObj.paramName = "p_type"; inputObj.value = obj.type; inputObj.dataType = "Varchar2";
                listInputObj.Add(inputObj);
                inputModel inputObj1 = new inputModel();
                inputObj1.paramName = "p_cluster_id"; inputObj1.value = obj.cluster_id; inputObj1.dataType = "Varchar2";
                listInputObj.Add(inputObj1);
                inputModel inputObj2 = new inputModel();
                inputObj2.paramName = "p_uid"; inputObj2.value = obj.uid; inputObj2.dataType = "Varchar2";
                listInputObj.Add(inputObj2);
                inputModel inputObj3 = new inputModel();
                inputObj3.paramName = "p_secretariat"; inputObj3.value = obj.gsws_code; inputObj3.dataType = "Varchar2";
                listInputObj.Add(inputObj3);

                requestModel procObj = new requestModel();
                procObj.refcursorName = "pcur";
                procObj.procedureName = "GSWS_HH_NON_MAPPED1";
                procObj.inputs = listInputObj;
                procObj.key = key;

                string json = JsonConvert.SerializeObject(procObj);

                return dbRouter.POST_Request(gswsRouterUrl, json);
            }
            catch (Exception ex)
            {
                string mappath = HttpContext.Current.Server.MapPath("unMappedPensionerMappingProcedure");
                string serialized_data = JsonConvert.SerializeObject(obj);
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, serialized_data));
                throw ex;
            }
        }

    }




    public class pensionModel
    {
        public string uid { get; set; }
        public string cluster_id { get; set; }
        public string type { get; set; }
        public string gsws_code { get; set; }
    }

}
