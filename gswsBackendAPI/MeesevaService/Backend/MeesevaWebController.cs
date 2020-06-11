using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Dynamic;
using gswsBackendAPI.DL.DataConnection;

namespace gswsBackendAPI.MeesevaService.Backend
{
    [RoutePrefix("api/MeesevaWeb")]
    public class MeesevaWebController : ApiController
    {
        EncryptMeeseva _objemeesva = new EncryptMeeseva();
        [HttpPost]
        [Route("MeesevaEncrypt")]
        public IHttpActionResult MeesevaEncrypt(dynamic data)
        {
            try
            {
				string json = JsonConvert.SerializeObject(data);
				MeesevaModel _rotobj = JsonConvert.DeserializeObject<MeesevaModel>(json);
                return Ok(_objemeesva.MeesevaEncryptData(_rotobj));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		[HttpPost]
		[Route("MeesevaVROEncrypt")]
		public IHttpActionResult MeesevaVROEncrypt(dynamic data)
		{
			try
			{
				string json = JsonConvert.SerializeObject(data);
				MeesevaModel _rotobj = JsonConvert.DeserializeObject<MeesevaModel>(json);
			    return Ok(_objemeesva.MeesevaVROEncryptData(_rotobj));
				//return Ok(_objemeesva.MeesevaHousesiteVROEncryptData(_rotobj));

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		
		[HttpPost]
		[Route("GetMeesevaAppStatus")]
		public IHttpActionResult GetMeesevaAppStatus(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				string json = JsonConvert.SerializeObject(value);
				MEESEVASTATUSMODEL _rotobj = JsonConvert.DeserializeObject<MEESEVASTATUSMODEL>(json);
				return Ok(_objemeesva.GetMeesevaAppStatucData(_rotobj));
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[HttpPost]
		[Route("VROApproval")]
		public IHttpActionResult VROApproval(dynamic data)
		{
			try
			{
				string json = JsonConvert.SerializeObject(data);
				MeesevaModel _rotobj = JsonConvert.DeserializeObject<MeesevaModel>(json);
				return Ok(_objemeesva.MeesevaHousesiteVROEncryptData(_rotobj));
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


	}
}
