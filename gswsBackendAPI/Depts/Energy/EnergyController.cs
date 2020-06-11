using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace gswsBackendAPI.Depts.Energy
{
    [RoutePrefix("api/Energy")]
    public class EnergyController : ApiController
    {
        dynamic CatchData = new ExpandoObject();
        EnergyHelper enerhel = new EnergyHelper();

        #region APSPDCL
        [HttpPost]
        [Route("GetServiceStatus")]
        public IHttpActionResult GetServiceStatus(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                AppStatus rootobj = JsonConvert.DeserializeObject<AppStatus>(value);
                return Ok(enerhel.GetServiceStatus_helper(rootobj));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = EnergyHelper.ThirdpartyMessage;
				return Ok(CatchData);
            }
        }

        [HttpPost]
        [Route("GetTransactionHistory")]
        public IHttpActionResult GetTransactionHistory(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                AppStatus rootobj = JsonConvert.DeserializeObject<AppStatus>(value);
                return Ok(enerhel.GetTransactionHistory_helper(rootobj));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
				CatchData.Reason = EnergyHelper.ThirdpartyMessage;

				return Ok(CatchData);
            }
        }

        #endregion

        #region APEPDCL
        [HttpPost]
        [Route("GetAPEPDCLServiceStatus")]
        public IHttpActionResult GetAPEPDCLServiceStatus(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                APEPDCLStatus rootobj = JsonConvert.DeserializeObject<APEPDCLStatus>(value);
                return Ok(enerhel.GetAPEPDCLServiceStatus_helper(rootobj));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = EnergyHelper.ThirdpartyMessage;
				return Ok(CatchData);
            }
        }

        [HttpPost]
        [Route("GetAPEPDCLTransactionHistory")]
        public IHttpActionResult GetAPEPDCLTransactionHistory(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                APEPDCLHistory rootobj = JsonConvert.DeserializeObject<APEPDCLHistory>(value);
                return Ok(enerhel.GetAPEPDCLTransactionHistory_helper(rootobj));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = EnergyHelper.ThirdpartyMessage;
				return Ok(CatchData);
            }
        }

        #endregion
    }
}
