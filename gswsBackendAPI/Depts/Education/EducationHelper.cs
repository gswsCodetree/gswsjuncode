using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Data;
using System.Threading.Tasks;
using gswsBackendAPI.DL.CommonHel;
using gswsBackendAPI.DL.DataConnection;

namespace gswsBackendAPI.Depts.Education
{
    public class EducationHelper : EducationSPHelper
    {
        #region Ammavodi

        
        public dynamic GetApplicantStatus(Ammavodi root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                var val = PostData("http://push147.sps.ap.gov.in/abwc/API/Schemes/GSWSGetStatus", root);
                var data = GetSerialzedData<dynamic>(val);
                return data;
            }
            catch (Exception ex)
            {
                obj.Status = 102;
                obj.Reason = ThirdpartyMessage;
                Common_Education_Error(ex.Message.ToString(), "http://push147.sps.ap.gov.in/abwc/API/Schemes/GSWSGetStatus", "2");
                return obj;
            }

        }
       
        public dynamic GetAmmavodiAppStatus(Ammavodi root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                if (Utils.IsAlphaNumeric(root.fadhar_no))
                {
					//SAMPLE NUMBER 847968535314&schemeId=AMMAVODI  
					var val = GetData("https://jnanabhumi.ap.gov.in/jnbWebservices/services/prajaSachivalayam/getBillStatus?userId=admin&password=jnb@dmin20!9&aadhar=" + root.fadhar_no + "&schemeId=AMMAVODI");

					var data = GetSerialzedData<dynamic>(val);
					return data;
				}
                else
                {
                    obj.Status = 102;
                    obj.Reason = "Invalid Input Request";
                }
                
            }
            catch (Exception ex)
            {
                obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
				string mappath = HttpContext.Current.Server.MapPath("AmmavodiExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Ammavodi App Status API:" + ex.Message.ToString()));
                //throw new Exception(ex.Message);
                Common_Education_Error(ex.Message.ToString(), "https://jnanabhumi.ap.gov.in/jnbWebservices/services/prajaSachivalayam/getBillStatus?userId=admin&password=jnb@dmin20!9&aadhar=", "2");
            }

            return obj;

        }

        public static bool Common_Education_Error(string msg, string url, string etype)
        {
            ExceptionDataModel objex = new ExceptionDataModel();
            try
            {
                objex.E_DEPTID = DepartmentEnum.Department.Human_Resources_Higher_Education.ToString();
                objex.E_HODID = DepartmentEnum.HOD.School_Education.ToString();
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