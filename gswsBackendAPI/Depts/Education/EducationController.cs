using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Dynamic;
using gswsBackendAPI.DL.DataConnection;

namespace gswsBackendAPI.Depts.Education
{
    [RoutePrefix("api/Education")]
    public class EducationController : ApiController
    {
        dynamic CatchData = new ExpandoObject();
        EducationHelper eduhel = new EducationHelper();

        #region Amma Vodi
        //Get Applicants Status
        [HttpPost]
        [Route("GetAmmavodiAppStatus")]
        public IHttpActionResult GetAmmavodiAppStatus(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                Ammavodi rootobj = JsonConvert.DeserializeObject<Ammavodi>(value);
                return Ok(eduhel.GetAmmavodiAppStatus(rootobj));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = "Error Occured";
                return Ok(CatchData);
            }
        }

        [HttpPost]
        [Route("GetApplicantStatus")]
        public IHttpActionResult GetApplicantStatus(dynamic data)
        {
            try
            {
                //string value = token_gen.Authorize_aesdecrpty(data);
                string value = JsonConvert.SerializeObject(data);
                Ammavodi rootobj = JsonConvert.DeserializeObject<Ammavodi>(value);
                return Ok(eduhel.GetApplicantStatus(rootobj));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = "Error Occured";
                return Ok(CatchData);
            }
        }

        #endregion


    }
}