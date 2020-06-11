using gswsBackendAPI.Depts.PensionVolunteerMapping.Backend;
using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using static gswsBackendAPI.Depts.RationVolunteermapping.Backend.ResponseModel;

namespace gswsBackendAPI.Depts.RationVolunteermapping.Backend
{
    public class RationVolunteerSPHelper : CommonSPHel
    {


        private static string gswsTestUrl = ConfigurationManager.AppSettings["gswsRouterUrl"].ToString();
        private static string key = "d2e7ee118d6fb11b35dfb84b745fd3c8b643b70f33e8f0657b0b9c765b82390a";
        public DataTable RationVolunteerMappingProc(RationInputs obj)
        {

            try
            {
                List<inputModel> listInputObj = new List<inputModel>();


                inputModel inputObj = new inputModel();
                inputObj.paramName = "ptype"; inputObj.value = obj.ptype; inputObj.dataType = "Varchar2";
                listInputObj.Add(inputObj);
                inputModel inputObj1 = new inputModel();
                inputObj1.paramName = "psec_id"; inputObj1.value = obj.psec_id.ToString(); inputObj1.dataType = "Varchar2";
                listInputObj.Add(inputObj1);
                inputModel inputObj2 = new inputModel();
                inputObj2.paramName = "pration_id"; inputObj2.value = obj.pration_id; inputObj2.dataType = "Varchar2";
                listInputObj.Add(inputObj2);
                inputModel inputObj3 = new inputModel();
                inputObj3.paramName = "pvv_id"; inputObj3.value = obj.pvv_id; inputObj3.dataType = "Varchar2";
                listInputObj.Add(inputObj3);
                inputModel inputObj5 = new inputModel();
                inputObj5.paramName = "pUPDATED_BY"; inputObj5.value = obj.pUPDATED_BY; inputObj5.dataType = "Varchar2";
                listInputObj.Add(inputObj5);
                inputModel inputObj6 = new inputModel();
                inputObj6.paramName = "pvv_name"; inputObj6.value = obj.pvv_name; inputObj6.dataType = "Varchar2";
                listInputObj.Add(inputObj6);
                inputModel inputObj7 = new inputModel();
                inputObj7.paramName = "pCLUSTER_ID"; inputObj7.value = obj.pCLUSTER_ID; inputObj7.dataType = "Varchar2";
                listInputObj.Add(inputObj7);
                inputModel inputObj8 = new inputModel();
                inputObj8.paramName = "pCLUSTER_NAME"; inputObj8.value = obj.pCLUSTER_NAME; inputObj8.dataType = "Varchar2";
                listInputObj.Add(inputObj8);

                requestModel procObj = new requestModel();
                procObj.refcursorName = "pcur";
                procObj.procedureName = "GSWS_RATION_VV_MAPPING_PROC";
                procObj.inputs = listInputObj;
                procObj.key = key;

                string json = JsonConvert.SerializeObject(procObj);

                return dbRouter.POST_Request(gswsTestUrl, json);
            }
            catch (Exception ex)
            {
                string mappath = HttpContext.Current.Server.MapPath("rationVolunteerMappingProcedure");
                string serialized_data = JsonConvert.SerializeObject(obj);
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, serialized_data));
                throw ex;
            }
        }



        public int RationVolunteerMappingProc1(RationInputs obj)
        {
            int count = 0;
            for (int i = 0; i < obj.user_data.Count; i++)
            {
                try
                {
                    List<inputModel> listInputObj = new List<inputModel>();


                    inputModel inputObj = new inputModel();
                    inputObj.paramName = "ptype"; inputObj.value = obj.ptype; inputObj.dataType = "Varchar2";
                    listInputObj.Add(inputObj);
                    inputModel inputObj1 = new inputModel();
                    inputObj1.paramName = "psec_id"; inputObj1.value = obj.psec_id.ToString(); inputObj1.dataType = "Varchar2";
                    listInputObj.Add(inputObj1);
                    inputModel inputObj2 = new inputModel();
                    inputObj2.paramName = "pration_id"; inputObj2.value = obj.user_data[i].EXISTING_RC_NUMBER; inputObj2.dataType = "Varchar2";
                    listInputObj.Add(inputObj2);
                    inputModel inputObj3 = new inputModel();
                    inputObj3.paramName = "pvv_id"; inputObj3.value = obj.pvv_id; inputObj3.dataType = "Varchar2";
                    listInputObj.Add(inputObj3);
                    inputModel inputObj5 = new inputModel();
                    inputObj5.paramName = "pUPDATED_BY"; inputObj5.value = obj.pUPDATED_BY; inputObj5.dataType = "Varchar2";
                    listInputObj.Add(inputObj5);
                    inputModel inputObj6 = new inputModel();
                    inputObj6.paramName = "pvv_name"; inputObj6.value = obj.pvv_name; inputObj6.dataType = "Varchar2";
                    listInputObj.Add(inputObj6);
                    inputModel inputObj7 = new inputModel();
                    inputObj7.paramName = "pCLUSTER_ID"; inputObj7.value = obj.pCLUSTER_ID; inputObj7.dataType = "Varchar2";
                    listInputObj.Add(inputObj7);
                    inputModel inputObj8 = new inputModel();
                    inputObj8.paramName = "pCLUSTER_NAME"; inputObj8.value = obj.pCLUSTER_NAME; inputObj8.dataType = "Varchar2";
                    listInputObj.Add(inputObj8);

                    requestModel procObj = new requestModel();
                    procObj.refcursorName = "pcur";
                    procObj.procedureName = "GSWS_RATION_VV_MAPPING_PROC";
                    procObj.inputs = listInputObj;
                    procObj.key = key;
                    string json = JsonConvert.SerializeObject(procObj);

                    DataTable dt = dbRouter.POST_Request(gswsTestUrl, json);
                    if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                        count = count + 1;
                }
                catch (Exception ex)
                {
                    string mappath = HttpContext.Current.Server.MapPath("rationVolunteerMappingProcedure");
                    string serialized_data = JsonConvert.SerializeObject(obj);
                    Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, serialized_data));
                }
            }
            return count;


        }





    }

    public class inputModel
    {
        public string paramName { get; set; }
        public string dataType { get; set; }
        public string value { get; set; }
    }
    public class requestModel
    {
        public string procedureName { get; set; }
        public string refcursorName { get; set; }
        public string key { get; set; }
        public List<inputModel> inputs { get; set; }

    }

}