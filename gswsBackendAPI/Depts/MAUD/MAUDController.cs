using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using gswsBackendAPI.DL.CommonHel;
using System.Dynamic;

namespace gswsBackendAPI.Depts.MAUD
{
    [RoutePrefix("api/MAUDServies")]
    public class MAUDController : ApiController
    {
        MAUDHelper hlpval = new MAUDHelper();
        dynamic CatchData = new ExpandoObject();

        #region "Check Applicant Status - Building Permission,Layout Permission,Occupancy certificate"
        [HttpPost]
        [Route("CheckMaudAppStatus")]
        public IHttpActionResult CheckMaudAppStatus(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                dynamic objCert = JsonConvert.DeserializeObject<dynamic>(value);
                if (Utils.IsAlphaNumeric(objCert.AppID))
                    return Ok(hlpval.CheckAppStatus(objCert.AppID.ToString()));
                else
                {
                    dynamic RData = new ExpandoObject();
                    RData.status = 102;
                    RData.Reason = "Special Characters are Not Allowed.";
                    return Ok(RData);
                }

            }
            catch (Exception ex)
            {
                CatchData.Status = "Failed";
                CatchData.Reason = CommonSPHel.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }
        #endregion


    }
}
