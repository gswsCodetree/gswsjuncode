using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using gswsBackendAPI.DL.CommonHel;

namespace gswsBackendAPI.Dept.RTGS.Backend
{
    [RoutePrefix("api/RTGS")]
    public class RTGSController : ApiController
    {
        dynamic CatchData = new ExpandoObject();
        RTGSHelper RTGShel = new RTGSHelper();

        #region PSS
        //Get Applicants Status
        [HttpPost]
        [Route("GetApplicantStatus")]
        public IHttpActionResult GetApplicantStatus(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {

                //string value = JsonConvert.SerializeObject(data);
                PSSModel rootobj = JsonConvert.DeserializeObject<PSSModel>(value);
                if (Utils.IsAlphaNumeric(rootobj.INPUT))
                    return Ok(RTGShel.GetApplicantStatus(rootobj));
                else
                {
                    dynamic RData = new ExpandoObject();
                    RData.Status = "Failure";
                    RData.Reason = "Special Characters Not Allowed.";
                    return Ok(RData);
                }
            }
            catch (Exception ex)
            {
                CatchData.Status = "Failure";
                CatchData.Reason = "Error Occured While Getting Data";
                return Ok(CatchData);
            }

        }
        #endregion
    }
}
