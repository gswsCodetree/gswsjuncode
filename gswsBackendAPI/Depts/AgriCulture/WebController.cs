using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace gswsBackendAPI.Depts.AgriCulture
{

	[RoutePrefix("api/Agriculture")]
	public class WebController : ApiController
	{
		Helper _Hel = new Helper();

		[Route("DemoAPI")]
		public IHttpActionResult DemoAPI(dynamic data)
		{
			string serialized_data = JsonConvert.SerializeObject(data);
			DemoModel obj = JsonConvert.DeserializeObject<DemoModel>(serialized_data);
			return Ok(_Hel.DemoAPI(obj));
		}

		[HttpPost]
		[Route("AgricultureserviceMaster")]
		public IHttpActionResult AgricultureserviceMaster(dynamic data)
		{
			dynamic objdata = new ExpandoObject();
			try
			{
			
				
				string agricultureservicepath = ConfigurationManager.AppSettings["fMastername"].ToString()+"MasterTextFiles\\agricultureservice.txt";

				string json = File.ReadAllText(agricultureservicepath);

				objdata.Status = 100;
				objdata.RevenuMaster = json;

			}
			catch (Exception ex)
			{
				objdata.Status = 500;
				objdata.result = ex.Message.ToString();
			}
			return Ok(objdata);
		}
		#region Agriculture Decrist - NIC
		[HttpPost]
		[Route("GetSeedGroup")]
		public IHttpActionResult GetSeedGroup(dynamic Jsondata)
		{
			string value = token_gen.Authorize_aesdecrpty(Jsondata);
			try
			{
				dynamic data = JsonConvert.DeserializeObject<dynamic>(value);
				data.deptcode = "3"; data.username = "villsec"; data.password = "f794b75239e84b1675a100fc85efb213";
				return Ok(_Hel.GetSeedGroupsData(data));
			}
			catch (Exception ex)
			{
				throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
			}
		}

		[HttpPost]
		[Route("GetSeedVariety")]
		public IHttpActionResult GetSeedVariety(dynamic Jsondata)
		{

			string value = token_gen.Authorize_aesdecrpty(Jsondata);
			try
			{
				dynamic data = JsonConvert.DeserializeObject<dynamic>(value);
				data.deptcode = "3"; data.username = "villsec"; data.password = "f794b75239e84b1675a100fc85efb213";
				return Ok(_Hel.GetSeedVarietesData(data));
			}
			catch (Exception ex)
			{
				throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
			}
		}

		[HttpPost]
		[Route("GetSeedVarietyData")]
		public IHttpActionResult GetSeedVarietyData(dynamic Jsondata)
		{
			string value = token_gen.Authorize_aesdecrpty(Jsondata);
			try
			{
				dynamic data = JsonConvert.DeserializeObject<dynamic>(value);
				data.deptcode = "3"; data.username = "villsec"; data.password = "f794b75239e84b1675a100fc85efb213"; data.statecode = "28";
				return Ok(_Hel.GetSeedVarietesResponse(data));
			}
			catch (Exception ex)
			{
				throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
			}
		}

		//Get Eligible Benificiaries Data
		[HttpPost]
		[Route("GetEligibleBeneficiaries")]
		public IHttpActionResult GetEligibleBeneficiaries(dynamic Jsondata)
		{
			string value = token_gen.Authorize_aesdecrpty(Jsondata);
			try
			{
				dynamic data = JsonConvert.DeserializeObject<dynamic>(value);
				data.deptcode = "3"; data.username = "villsec"; data.password = "f794b75239e84b1675a100fc85efb213"; data.statecode = "28";
				return Ok(_Hel.GetEligileBeneficiariesResponse(data));
			}
			catch (Exception ex)
			{
				throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
			}
		}
		// Get VAA Details
		[HttpPost]
		[Route("GetVAADetails")]
		public IHttpActionResult GetVAADetails(dynamic Jsondata)
		{
			string value = token_gen.Authorize_aesdecrpty(Jsondata);
			try
			{
				dynamic data = JsonConvert.DeserializeObject<dynamic>(value);
				data.deptcode = "3"; data.username = "villsec"; data.password = "f794b75239e84b1675a100fc85efb213"; data.statecode = "28";
				return Ok(_Hel.GetVAADetailsResponse(data));
			}
			catch (Exception ex)
			{
				throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
			}
		}

		[HttpPost]
		[Route("GetSeasonFNY")]
		public IHttpActionResult GetSeasonFNY(dynamic Jsondata)
		{
			string value = token_gen.Authorize_aesdecrpty(Jsondata);
			try
			{
				dynamic data = JsonConvert.DeserializeObject<dynamic>(value);
				data.deptcode = "3"; data.username = "villsec"; data.password = "f794b75239e84b1675a100fc85efb213";
				return Ok(_Hel.GetSeasonFinancialYear(data));
			}
			catch (Exception ex)
			{
				throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
			}
		}

		#endregion

		#region EPanta

		[HttpPost]
		[Route("GetVillageDetails")]
		public IHttpActionResult GetVillageDetails(dynamic Jsondata)
		{
			string value = token_gen.Authorize_aesdecrpty(Jsondata);
			try
			{
				///dynamic data = JsonConvert.DeserializeObject<dynamic>(value);
				//string value = token_gen.Authorize_aesdecrpty(data);
				//string value = JsonConvert.SerializeObject(data);
				EPantaCls rootobj = JsonConvert.DeserializeObject<EPantaCls>(value);
				return Ok(_Hel.GetVillgeData(rootobj));
			}
			catch (Exception ex)
			{
				dynamic CatchData = new ExpandoObject();
				CatchData.Status = 102;
				CatchData.Reason = "Error While Getting Village Data";
				return Ok(CatchData);
			}
		}

		[HttpPost]
		[Route("GetVillageDetailsByServey")]
		public IHttpActionResult GetVillageDetailsByServey(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				//
				//string value = JsonConvert.SerializeObject(data);
				EPantaCls rootobj = JsonConvert.DeserializeObject<EPantaCls>(value);
				return Ok(_Hel.GetVillageDetailsByServey(rootobj));
			}
			catch (Exception ex)
			{
				dynamic CatchData = new ExpandoObject();
				CatchData.Status = 102;
				CatchData.Reason = "Error While Getting Village Data By Servey NO";
				return Ok(CatchData);
			}
		}

		[HttpPost]
		[Route("GetVillageDetailsByAadhaar")]
		public IHttpActionResult GetVillageDetailsByAadhaar(dynamic data)
		{
			 string value = token_gen.Authorize_aesdecrpty(data);
			//string value = JsonConvert.SerializeObject(data);
			try
			{
				//string value = token_gen.Authorize_aesdecrpty(data);
				
				EPantaCls rootobj = JsonConvert.DeserializeObject<EPantaCls>(value);
				return Ok(_Hel.GetVillageDetailsByAadhaar(rootobj));
			}
			catch (Exception ex)
			{
				dynamic CatchData = new ExpandoObject();
				CatchData.Status = 102;
				CatchData.Reason = "Error While Getting Village Data By Aadhaar NO";
				return Ok(CatchData);
			}
		}

		#endregion

		#region Farmer Mechanization
		[HttpPost]
		[Route("GetFMAppStatus")]
		public IHttpActionResult GetFMAppStatus(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{

				//string value = JsonConvert.SerializeObject(data);
				FMAppSts rootobj = JsonConvert.DeserializeObject<FMAppSts>(value);
				return Ok(_Hel.GetFMAppStatus(rootobj));
			}
			catch (Exception ex)
			{
				dynamic CatchData = new ExpandoObject();
				CatchData.Status = 102;
				CatchData.Reason = "Error While Getting Application Status Data";
				return Ok(CatchData);
			}
		}

		[HttpPost]
		[Route("FMAppRegister")]
		public IHttpActionResult FMAppRegister(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				string mappath = HttpContext.Current.Server.MapPath("FormerMechanizationSaveLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath,  value));

				//string value = JsonConvert.SerializeObject(data);
				FMReg rootobj = JsonConvert.DeserializeObject<FMReg>(value);
				return Ok(_Hel.FMAppRegister(rootobj));
			}
			catch (Exception ex)
			{
				dynamic CatchData = new ExpandoObject();
				CatchData.Status = 102;
				CatchData.Reason = "Error While Submitting Application Data";
				return Ok(CatchData);
			}
		}

		#endregion

	}
}
