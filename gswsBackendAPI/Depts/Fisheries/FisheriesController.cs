using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static gswsBackendAPI.Depts.Fisheries.FisheriesModel;
using gswsBackendAPI.DL.CommonHel;

namespace gswsBackendAPI.Depts.Fisheries
{
    [RoutePrefix("api/Fisheries")]
    public class FisheriesController : ApiController
    {
        dynamic CatchData = new ExpandoObject();
        FisheriesHelper fihel = new FisheriesHelper();
        #region Fisheries

        //Get Applicants Status
        [HttpPost]
        [Route("GetAppStatus")]
        public IHttpActionResult GetAppStatus(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                AppSta rootobj = JsonConvert.DeserializeObject<AppSta>(value);
                return Ok(fihel.GetMethod(rootobj));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
				CatchData.Reason = FisheriesHelper.ThirdpartyMessage;

				return Ok(CatchData);
            }

        }

        #endregion


        #region Animal Husbandry

        //Get RIDS Applicants Status
        [HttpPost]
        [Route("RIDSAppStatus")]
        public IHttpActionResult RIDSAppStatus(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                AnimalCls rootobj = JsonConvert.DeserializeObject<AnimalCls>(value);
                if (Utils.IsAlphaNumeric(rootobj.UniqueNo))
                    return Ok(fihel.RIDSAppStatus(rootobj));
                else
                {
                    CatchData.Status = 102;
                    CatchData.Reason = "Special Characters are Not Allowed";
                    return Ok(CatchData);
                }
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = FisheriesHelper.ThirdpartyMessage;
				return Ok(CatchData);
            }

        }

        //Get RIDS Applicants Status
        [HttpPost]
        [Route("LLCSAppStatus")]
        public IHttpActionResult LLCSAppStatus(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                AnimalCls rootobj = JsonConvert.DeserializeObject<AnimalCls>(value);
                if (Utils.IsAlphaNumeric(rootobj.UniqueNo))
                    return Ok(fihel.LLCSAppStatus(rootobj));
                else
                {
                    CatchData.Status = 102;
                    CatchData.Reason = "Special Characters are Not Allowed";
                    return Ok(CatchData);
                }
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = FisheriesHelper.ThirdpartyMessage;
				return Ok(CatchData);
            }

        }

        #endregion
    }
}
