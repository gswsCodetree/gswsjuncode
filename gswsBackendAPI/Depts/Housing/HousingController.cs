using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static gswsBackendAPI.Depts.Housing.HousingModel;
using gswsBackendAPI.DL.CommonHel;
using System.Web;
using System.Threading.Tasks;

namespace gswsBackendAPI.Depts.Housing
{
    [RoutePrefix("api/Housing")]
    public class HousingController : ApiController
    {
        dynamic CatchData = new ExpandoObject();
        HousingHelper hohel = new HousingHelper();

        #region Housing

        //Get Applicants Status
        [HttpPost]
        [Route("GetAppStatus")]
        public IHttpActionResult GetAppStatus(dynamic data)
        {
			string value = token_gen.Authorize_aesdecrpty(data);
			//string value = JsonConvert.SerializeObject(data);
			try
			{
              
                AppSta rootobj = JsonConvert.DeserializeObject<AppSta>(value);
                if (Utils.IsAlphaNumeric(rootobj.ref_no))
                    return Ok(hohel.GetMethod("https://apgovhousing.apcfss.in/APSHCLWEBSERVICES/registeredData/getDetails?ref_no=" + rootobj.ref_no));
                else
                {
                    CatchData.Status = 102;
                    CatchData.Reason = "Error Occured While Getting Status";
                    return Ok(CatchData);
                }
            }
            catch (Exception ex)
            {
                Common_Housing_Error(ex.Message.ToString(), "https://apgovhousing.apcfss.in/APSHCLWEBSERVICES/registeredData/getDetails?ref_no", "2");
                CatchData.Status = 102;
				CatchData.Reason = HousingHelper.ThirdpartyMessage;

				return Ok(CatchData);
            }

        }

		#endregion

		#region House Sites

		//Get Housing Sites Applicants Status
		[HttpPost]
		[Route("GetHSitesAppStatus")]
		public IHttpActionResult GetHSitesAppStatus(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			//string value = JsonConvert.SerializeObject(data);
			try
			{

				ApplicationSta rootobj = JsonConvert.DeserializeObject<ApplicationSta>(value);
				if (Utils.IsAlphaNumeric(rootobj.AppNo))
					return Ok(hohel.GetHouseSiteStatusApp(rootobj));
				else
				{
					CatchData.Status = 102;
					CatchData.Reason = "Special charactes are not allowed";
					return Ok(CatchData);
				}
			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
			  CatchData.Reason = HousingHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		//Get Housing Sites Applicants Status
		[HttpPost]
		[Route("HSitesApplicationReg")]
		public IHttpActionResult HSitesApplicationReg(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			//string value = JsonConvert.SerializeObject(data);
			try
			{

				HousePattaCls rootobj = JsonConvert.DeserializeObject<HousePattaCls>(value);
				return Ok(hohel.GetHouseSitePattapplicationAdd(rootobj));
			}
			catch (Exception ex)
			{
				string mappath2 = HttpContext.Current.Server.MapPath("HousingSitesErrorLogs");
				Task WriteTask2 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath2, "HSitesApplicationReg:" + ex.Message.ToString()));

				CatchData.Status = 102;
				CatchData.Reason = HousingHelper.ThirdpartyMessage+ "( HSitesApplicationReg:" + ex.Message.ToString()+")";
				return Ok(CatchData);
			}

		}

        #endregion
        public static bool Common_Housing_Error(string msg, string url, string etype)
        {
            ExceptionDataModel objex = new ExceptionDataModel();
            try
            {
                objex.E_DEPTID = DepartmentEnum.Department.Housing.ToString();
                objex.E_HODID = DepartmentEnum.HOD.APHOUSING.ToString();
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
    }
}
