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
			}

            return obj;

        }

       
        #endregion
    }
}