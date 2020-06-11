using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static gswsBackendAPI.Depts.Women_Children_Disabled_and_SeniorCitizen.Model;

namespace gswsBackendAPI.Depts.Women_Children_Disabled_and_SeniorCitizen
{
	[RoutePrefix("api/WomenServices")]
	public class WomensController : ApiController
	{
		dynamic objdynamic = new ExpandoObject();
		Helper hlpval = new Helper();

		#region "APDASCAC Registration form"
		[HttpPost]
		[Route("Registration_form")]
		public IHttpActionResult Registration_form(dynamic objCert)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(objCert);
			try
			{
				//string value = JsonConvert.SerializeObject(jsondata);
				GetRegData rootobj = JsonConvert.DeserializeObject<GetRegData>(jsondata);

				return Ok(hlpval.Reg(rootobj));

			}
			catch (Exception ex)
			{
				objdynamic.Status = 102;
				objdynamic.Reason = Helper.ThirdpartyMessage;
				return Ok(objdynamic);
			}

		}
		#endregion

		#region "3 Wheeler Sanctioned"
		[HttpPost]
		[Route("GetLoginToken")]
		public IHttpActionResult GetLoginToken(dynamic objCert)
		{
			try
			{
				string value = JsonConvert.SerializeObject(objCert);
				GetUserLoginToken rootobj = JsonConvert.DeserializeObject<GetUserLoginToken>(value);

				return Ok(hlpval.GetToken());

			}
			catch (Exception ex)
			{
				objdynamic.Status = "Failure";
				objdynamic.Reason = Helper.ThirdpartyMessage;
				return Ok(objdynamic);
			}

		}


		[HttpPost]
		[Route("LoadGeneralData")]
		public IHttpActionResult GetDistricts(dynamic objInput)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(objInput);
			try
			{
				//string value = JsonConvert.SerializeObject(jsondata);
				dynamic rootobj = JsonConvert.DeserializeObject<dynamic>(jsondata);
				return Ok(hlpval.GetCommonData(rootobj));

			}
			catch (Exception ex)
			{
				objdynamic.Status = 102;
				objdynamic.Reason = Helper.ThirdpartyMessage;
				return Ok(objdynamic);
			}

		}

		[HttpPost]
		[Route("GetVillages")]
		public IHttpActionResult GetVillages(dynamic objInput)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(objInput);
			try
			{
				//string value = JsonConvert.SerializeObject(jsondata);
				dynamic rootobj = JsonConvert.DeserializeObject<dynamic>(jsondata);

				return Ok(hlpval.GetVillages(rootobj));

			}
			catch (Exception ex)
			{
				objdynamic.Status = "Failure";
				objdynamic.Reason = Helper.ThirdpartyMessage;
				return Ok(objdynamic);
			}

		}

		[HttpPost]
		[Route("GetYears")]
		public IHttpActionResult GetYears(dynamic objInput)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(objInput);
			try
			{
				dynamic rootobj = JsonConvert.DeserializeObject<dynamic>(jsondata);
				return Ok(hlpval.GetFyn_Year());

			}
			catch (Exception ex)
			{
				objdynamic.Status = "Failure";
				objdynamic.Reason = Helper.ThirdpartyMessage;
				return Ok(objdynamic);
			}

		}

		[HttpPost]
		[Route("POSTWCDWApplication")]
		public IHttpActionResult POSTWCDWApplication(dynamic objInput)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(objInput);
			try
			{
				string mappath = HttpContext.Current.Server.MapPath("POSTWCDWApplicationSaveLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Input Data API:" + jsondata));
				//string value = JsonConvert.SerializeObject(jsondata);
				WCDWCLS rootobj = JsonConvert.DeserializeObject<WCDWCLS>(jsondata);
				return Ok(hlpval.SaveWCDWApplication_SP(rootobj));

			}
			catch (Exception ex)
			{
				objdynamic.Status = 102;
				objdynamic.Reason = Helper.ThirdpartyMessage;
				return Ok(objdynamic);
			}

		}

		[HttpPost]
		[Route("GetWCDWAppStatus")]
		public IHttpActionResult GetWCDWAppStatus(dynamic objInput)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(objInput);
			try
			{
				AppStatCls rootobj = JsonConvert.DeserializeObject<AppStatCls>(jsondata);
				return Ok(hlpval.GetApplicationStatus(rootobj));

			}
			catch (Exception ex)
			{
				objdynamic.Status = 102;
				objdynamic.Reason = Helper.ThirdpartyMessage;
				return Ok(objdynamic);
			}
		}

		#endregion

	}
}
