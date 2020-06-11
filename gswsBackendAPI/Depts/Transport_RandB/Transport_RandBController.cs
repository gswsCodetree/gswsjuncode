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
using static gswsBackendAPI.Depts.Transport_RandB.Model;

namespace gswsBackendAPI.Depts.Transport_RandB
{
	[RoutePrefix("api/Transport")]
	public class Transport_RandBController : ApiController
    {
		dynamic CatchData = new ExpandoObject();
		Helper hlpval = new Helper();
		[HttpPost]
		[Route("Complaint_StatusCheck")]
		public IHttpActionResult Complaint_StatusCheck(dynamic Jsondata)
		{
			string value = token_gen.Authorize_aesdecrpty(Jsondata);
			try
			{
				dynamic data = JsonConvert.DeserializeObject<dynamic>(value);
				return Ok(hlpval.GetComplaintStatus_RandB(data.appcode.ToString()));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		[HttpPost]
		[Route("GetRTARegAppStatus")]
		public IHttpActionResult GetRTARegAppStatus(dynamic Jsondata)
		{
			string value = token_gen.Authorize_aesdecrpty(Jsondata);
			try
			{
				RTAStatusCls data = JsonConvert.DeserializeObject<RTAStatusCls>(value);
				if (Utils.IsAlphaNumeric(data.applicationNo) && Utils.IsAlphaNumeric(data.chassisNo))
					return Ok(hlpval.GetRTARegAppStatus(data));
				else
				{
					CatchData.Status = 102;
					CatchData.Reason = "Special Characters Are Not Allowed.";
					return Ok(CatchData);
				}
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		[HttpPost]
		[Route("LLRAplicationsStatus")]
		public IHttpActionResult LLRAplicationsStatus(dynamic Jsondata)
		{
			string value = token_gen.Authorize_aesdecrpty(Jsondata);
			try
			{
				dynamic data = JsonConvert.DeserializeObject<ApplicationStatus>(value);
				if (Utils.IsAlphaNumeric(data.applicationFormNo))
					return Ok(hlpval.GetLLRAppStatus_helper(data));
				else
				{
					CatchData.Status = 102;
					CatchData.Reason = "Special Characters Are Not Allowed";
					return Ok(CatchData);
				}
			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = "Error Occured While Getting Status";
				return Ok(CatchData);
			}

		}

		[HttpPost]
		[Route("GetVahanaMitraRegStatus")]
		public IHttpActionResult GetVahanaMitraRegStatus(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				dynamic rootobj = JsonConvert.DeserializeObject<dynamic>(value);
				if (Utils.IsAlphaNumeric(rootobj.BenID.ToString()))
					return Ok(hlpval.GetMethod("https://jnanabhumi.ap.gov.in/jnbWebservices/services/prajaSachivalayam/getBillStatus?userId=admin&password=jnb@dmin20!9&aadhar=" + rootobj.BenID.ToString() + "&schemeId=VAHANAMITRA"));
				else
				{
					CatchData.Status = 102;
					CatchData.Reason = "Special Characters Are Not Allowed.";
					return Ok(CatchData);
				}
			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}
	}
}
