using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;
using gswsBackendAPI.DL.CommonHel;
namespace gswsBackendAPI.Depts.Industries
{
    public class IndustriesHelper : IndustriesSPHelper
    {
        #region YSR Navodayam
        public dynamic GetMethod(string url)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                var val = GetData(url);
                obj.Status = 100;
                obj.Reason = "Data Loaded Successfully";
                obj.Data = val;

                return obj;
            }
            catch (Exception ex)
            {
                obj.Status = 102;
                obj.Reason = ThirdpartyMessage;
                return obj;
            }

        }

        public dynamic SaveOTRReg_helper(NavodayamOTR root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                var val = PostData("http://125.17.121.166:8080/SDPIntegrationServices/YSRNavodayamService/SaveOTRDetails", root);
                var data = GetSerialzedData<dynamic>(val);

                obj.Status = 100;
                obj.Reason = "Data Loaded Successfully";
                obj.Data = data;

                return obj;
            }
            catch (Exception ex)
            {
                ExceptionDataModel objex = new ExceptionDataModel();
                string mappath = HttpContext.Current.Server.MapPath("IndustriesExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "GetSeedGroupsData Service:" + ex.Message.ToString()));
                Common_Industries_Error(ex.Message.ToString(), "http://125.17.121.166:8080/SDPIntegrationServices/YSRNavodayamService/SaveOTRDetails", "2");

                //objectData.Status = "Failure";

                //objectData.Reason = ThirdpartyMessage; //ex.Message.ToString();
                //objectData.Data = "";
                obj.Status = 102;
                obj.Reason = ThirdpartyMessage;
                return obj;
            }

        }
            static bool Common_Industries_Error(string msg, string url, string etype)
               {
                ExceptionDataModel objex = new ExceptionDataModel();
                try
                {
                    objex.E_DEPTID = DepartmentEnum.Department.Industries_Infrastructure_Investment_and_Commerce.ToString();
                    objex.E_HODID = DepartmentEnum.HOD.Industries_Commerce_and_Export_Promotion.ToString();
                    objex.E_ERRORMESSAGE = msg;
                    objex.E_SERVICEAPIURL = url;
                    objex.E_ERRORTYPE = etype;
                    new LoginSPHelper().Save_Exception_Data(objex);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
           #endregion
        
    }
}