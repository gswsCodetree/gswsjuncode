
using gswsBackendAPI.Depts.SocialWelfare_Tribal;
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

namespace gswsBackendAPI.Depts.Minority
{
    [RoutePrefix("api/Minority")]
    public class MinorityController : ApiController
    {
        MinorityHelper hlpval = new MinorityHelper();
        dynamic CatchData = new ExpandoObject();


        #region "GetWomenDivorcedDetails"
        [HttpPost]
        [Route("GetWomenDivorcedDetails")]
        public IHttpActionResult GetWomenDivorcedDetails(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                dynamic objCert = JsonConvert.DeserializeObject<dynamic>(value);
                if (Utils.IsAlphaNumeric(objCert.MCNO))
                    return Ok(hlpval.GetWomenDivorcedDetails(objCert));
                else
                {
                    CatchData.Status = "Failed";
                    CatchData.Reason = "Special Characters are Not Allowed.";
                    return Ok(CatchData);
                }

            }
            catch (Exception ex)
            {
                CatchData.Status = "Failed";
                CatchData.data = CommonSPHel.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }
        #endregion

        #region "GetHonorariumToImamAndMouzansDetails"
        [HttpPost]
        [Route("GetHonorariumToImamAndMouzansDetails")]
        public IHttpActionResult GetHonorariumToImamAndMouzansDetails(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                dynamic objCert = JsonConvert.DeserializeObject<dynamic>(value);

                if (Utils.IsAlphaNumeric(objCert.BeneficiaryCode))
                    return Ok(hlpval.GetHonorariumToImamAndMouzansDetails(value));
                else
                {
                    dynamic RData = new ExpandoObject();
                    RData.Status = "Failed";
                    RData.Reason = "Special Characters are Not Allowed.";
                    return Ok(RData);
                }

            }
            catch (Exception ex)
            {
                CatchData.Status = "Failed";
                CatchData.data = CommonSPHel.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }
        #endregion
    }
}
