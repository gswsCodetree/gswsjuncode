using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using gswsBackendAPI.Models;
using gswsBackendAPI.DL.DataConnection;
using gswsBackendAPI.Dept.Endowments.Backend;
using Newtonsoft.Json;
using System.Dynamic;

namespace gswsBackendAPI.Controllers
{
	[RoutePrefix("api/Endowments")]
	public class EndowmentsController : ApiController
    {
		dynamic CatchData = new ExpandoObject();
		EndowmentsHelper Endowhel = new EndowmentsHelper();

		#region Brahmin Corporation
		//Get Applicants Status
		[HttpPost]
		[Route("GetApplicantStatus")]
		public IHttpActionResult GetApplicantStatus(dynamic data)
		{
            string value = token_gen.Authorize_aesdecrpty(data);
            try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				BrahminModel rootobj = JsonConvert.DeserializeObject<BrahminModel>(value);
				return Ok(Endowhel.GetApplicantStatus(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = ex.Message;
				return Ok(CatchData);
			}

		}
		#endregion
	}
}
