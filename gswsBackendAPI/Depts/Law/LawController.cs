using gswsBackendAPI.DL.CommonHel;
using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static gswsBackendAPI.Depts.Law.LawModel;

namespace gswsBackendAPI.Depts.Law
{
    [RoutePrefix("api/Law")]
    public class LawController : ApiController
    {
        dynamic CatchData = new ExpandoObject();
        LawHelper hohel = new LawHelper();

        #region Law

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
                    return Ok(hohel.GetMethod("https://myap.e-pragati.in:443/prweb/PRRestService/LawApplicantsCountAPI/V1/Status/" + rootobj.ref_no));
                else
                {
                    CatchData.Status = 102;
                    CatchData.Reason = "Special Characters Not Allowed";
                    return Ok(CatchData);
                }
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = LawHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        #endregion
    }
}
