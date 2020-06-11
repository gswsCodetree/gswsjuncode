using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace gswsBackendAPI.Depts.SocialWelfare_Tribal
{
	[RoutePrefix("api/SocialWelfare")]
	public class SocialWelfare_TribalController : ApiController
	{
		Helper hlpval = new Helper();
        dynamic CatchData = new ExpandoObject();

        #region "Marriage Certificate Download for Pelli Kannuka"
        [HttpPost]
		[Route("MrgCert_Download")]
		public IHttpActionResult MrgCert_Download(dynamic objCert)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(objCert);
			try
			{
				MrgCert obj = JsonConvert.DeserializeObject<MrgCert>(jsondata);
				return Ok(hlpval.Mrgcertificate_Download(obj));

			}
			catch (Exception ex)
			{
                CatchData.Status = "Failure";
                CatchData.Reason = CommonSPHel.ThirdpartyMessage;
                return Ok(CatchData);
            }

		}
		#endregion

		#region "Pelli Kaanuka Status Check"
		[HttpPost]
		[Route("StatusCheck")]
		public IHttpActionResult StatusCheck(dynamic objCert)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(objCert);
			try
			{
				MrgCert obj = JsonConvert.DeserializeObject<MrgCert>(jsondata);
				return Ok(hlpval.PK_StatusCheck(obj));

			}
			catch (Exception ex)
			{
                CatchData.Status = "Failure";
                CatchData.Reason = CommonSPHel.ThirdpartyMessage;
                return Ok(CatchData);
            }

		}
		#endregion

		#region "Education Status Check- Overseas Application check"
		[HttpPost]
		[Route("Education_ApplicationCheck")]
		public IHttpActionResult Education_ApplicationCheck(dynamic objCert)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(objCert);
			try
			{
				S_W_Education obj = JsonConvert.DeserializeObject<S_W_Education>(jsondata);
				return Ok(hlpval.Education_app_Check(obj));

			}
			catch (Exception ex)
			{
                CatchData.Status = "Failure";
                CatchData.Reason = CommonSPHel.ThirdpartyMessage;
                return Ok(CatchData);
            }

		}
		[HttpPost]
		[Route("GetAnnaualyears")]
		public IHttpActionResult GetAnnaualyears(dynamic objCert)
		{
			try
			{
				//string logdata = JsonConvert.SerializeObject(objCert);
				//var location = HttpContext.Current.Server.MapPath("Log");
				////logging here
				//Task.Run(() => Helper.SaveToLog(logdata, location));
				return Ok(hlpval.GetFyn_Year());

			}
			catch (Exception ex)
			{
                CatchData.Status = "Failure";
                CatchData.Reason = CommonSPHel.ThirdpartyMessage;
                return Ok(CatchData);
            }

		}
		#endregion
	}
}
