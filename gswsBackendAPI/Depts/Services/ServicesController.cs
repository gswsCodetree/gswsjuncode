using gswsBackendAPI.Depts.SocialWelfare_Tribal;
using gswsBackendAPI.DL.CommonHel;
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

namespace gswsBackendAPI.Depts.Services
{
    [RoutePrefix("api/Services")]
    public class ServicesController : ApiController
    {
        dynamic CatchData = new ExpandoObject();
        ServicesHelper heahel = new ServicesHelper();

		#region NPCI
		[HttpPost]
		[Route("GetNPCIStatus")]
		public IHttpActionResult GetNPCIStatus(dynamic data)
		{
            string value = token_gen.Authorize_aesdecrpty(data);
            try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				AppStatus rootobj = JsonConvert.DeserializeObject<AppStatus>(value);
				return Ok(heahel.GetNPCIStatus_helper(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = "Error Occured While Getting Data ";
				return Ok(CatchData);
			}
		}

		#endregion

		#region "Integrated & Income Certificates Download"
		[HttpPost]
		[Route("CertificatesDownload")]
		public IHttpActionResult CertificatesDownload(MeesevaCertificates objCert)
		{
			try
			{
				//string logdata = JsonConvert.SerializeObject(objCert);
				//var location = HttpContext.Current.Server.MapPath("Log");
				////logging here
				//Task.Run(() => Helper.SaveToLog(logdata, location));

				return Ok(heahel.Certificate_Download(objCert.strIntegratedID.ToString(), objCert.CertType.ToString()));

			}
			catch (Exception ex)
			{
                CatchData.Status = "Failure";
                CatchData.Reason = ServicesHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

		}

		#endregion

		#region Textile

		//Get Textile Data
		[HttpPost]
		[Route("GetTextileData")]
		public IHttpActionResult GetTextileData(dynamic data)
		{
            string value = token_gen.Authorize_aesdecrpty(data);
            try
			{
				//string value = token_gen.Authorize_aesdecrpty(data);
				//string value = JsonConvert.SerializeObject(data);
				AppStatus rootobj = JsonConvert.DeserializeObject<AppStatus>(value);
				return Ok(heahel.GetTextileData(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = ServicesHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}

		//Save Textile Data
		[HttpPost]
		[Route("SaveTextileData")]
		public IHttpActionResult SaveTextileData(dynamic data)
		{
            string value = token_gen.Authorize_aesdecrpty(data);
            try
			{
				//string value = JsonConvert.SerializeObject(data);
				TextileModel rootobj = JsonConvert.DeserializeObject<TextileModel>(value);
				return Ok(heahel.SaveTextileData(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = ServicesHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}

		#endregion

		#region Asset Tracking

		//Load Districts
		[HttpPost]
		[Route("LoadDistricts")]
		public IHttpActionResult LoadDistricts(dynamic data)
		{
			try
			{
				string value = token_gen.Authorize_aesdecrpty(data);
				//string value = JsonConvert.SerializeObject(data);
				Assetmodel rootobj = JsonConvert.DeserializeObject<Assetmodel>(value);
				return Ok(heahel.LoadDistricts(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = ServicesHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}


		[HttpPost]
		[Route("LoadSeccDetails")]
		public IHttpActionResult LoadSeccDetails(dynamic data)
		{
			try
			{
				string value = token_gen.Authorize_aesdecrpty(data);
				//string value = JsonConvert.SerializeObject(data);
				Assetmodel rootobj = JsonConvert.DeserializeObject<Assetmodel>(value);
				return Ok(heahel.LoadSeccDetails(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = ServicesHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		//Save Data
		[HttpPost]
		[Route("SaveSystemData")]
		public IHttpActionResult SaveSystemData(dynamic data)
		{
			try
			{
				string value = token_gen.Authorize_aesdecrpty(data);
				AssetTracking rootobj = JsonConvert.DeserializeObject<AssetTracking>(value);
				//var validresult.Status = "Success";//_valid.CheckSaveAssetTracking(rootobj);
				//if (validresult.Status == "Success")
				//{
				return Ok(heahel.SaveSystemData(rootobj));
				//}
				//else
				//{
				//	CatchData.Status = "Failure";
				//	CatchData.Reason = validresult.Reason;
				//	return Ok(CatchData);
				//}
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = ServicesHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}


		//Reports
		[HttpPost]
		[Route("LoadRptDistrictData")]
		public IHttpActionResult LoadRptDistrictData(dynamic data)
		{
			try
			{
				string value = token_gen.Authorize_aesdecrpty(data);
				//string value = JsonConvert.SerializeObject(data);
				Assetmodel rootobj = JsonConvert.DeserializeObject<Assetmodel>(value);
				return Ok(heahel.LoadRptDistrictData(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = ServicesHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		//Hardware Issues Compnent Loading
		[HttpPost]
		[Route("Loadhwcomponent")]
		public IHttpActionResult Loadhwcomponent(dynamic data)
		{
			try
			{
				string value = token_gen.Authorize_aesdecrpty(data);
				//string value = JsonConvert.SerializeObject(data);
				Assetmodel rootobj = JsonConvert.DeserializeObject<Assetmodel>(value);
				return Ok(heahel.Loadhwcomponent(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = ServicesHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		//Save Hardware Issues
		[HttpPost]
		[Route("SaveHardwareIssue")]
		public IHttpActionResult SaveHardwareIssue(dynamic data)
		{
			try
			{
				string value = token_gen.Authorize_aesdecrpty(data);

				string mappath = HttpContext.Current.Server.MapPath("HardwareIssueSubmitLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, value));
				Assetmodel rootobj = JsonConvert.DeserializeObject<Assetmodel>(value);



				//var validresult.Status = "Success";//_valid.CheckSaveAssetTracking(rootobj);
				//if (validresult.Status == "Success")
				//{
				return Ok(heahel.SaveHardwareIssue(rootobj));
				//}
				//else
				//{
				//	CatchData.Status = "Failure";
				//	CatchData.Reason = validresult.Reason;
				//	return Ok(CatchData);
				//}

			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = ServicesHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		#endregion

		#region Navasakam
		[HttpPost]
		[Route("RajakasApplicationStatus")]
		public IHttpActionResult RajakasApplicationStatus(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				AppStatusCls rootobj = JsonConvert.DeserializeObject<AppStatusCls>(value);
				if (Utils.IsAlphaNumeric(rootobj.BenID))
					return Ok(heahel.GetMethod("https://jnanabhumi.ap.gov.in/jnbWebservices/services/prajaSachivalayam/getBillStatus?userId=admin&password=jnb@dmin20!9&aadhar=" + rootobj.BenID + "&schemeId=RAJAKAS"));
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
				CatchData.Reason = ServicesHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}
		
		[HttpPost]
		[Route("TailorsApplicationStatus")]
		public IHttpActionResult TailorsApplicationStatus(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				AppStatusCls rootobj = JsonConvert.DeserializeObject<AppStatusCls>(value);
				if (Utils.IsAlphaNumeric(rootobj.BenID))
					return Ok(heahel.GetMethod("https://jnanabhumi.ap.gov.in/jnbWebservices/services/prajaSachivalayam/getBillStatus?userId=admin&password=jnb@dmin20!9&aadhar=" + rootobj.BenID + "&schemeId=TAILORS"));
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
				CatchData.Reason = ServicesHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}
	
		[HttpPost]
		[Route("NayeeApplicationStatus")]
		public IHttpActionResult NayeeApplicationStatus(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				AppStatusCls rootobj = JsonConvert.DeserializeObject<AppStatusCls>(value);
				if (Utils.IsAlphaNumeric(rootobj.BenID))
					return Ok(heahel.GetMethod("https://jnanabhumi.ap.gov.in/jnbWebservices/services/prajaSachivalayam/getBillStatus?userId=admin&password=jnb@dmin20!9&aadhar=" + rootobj.BenID + "&schemeId=NAYEEBRAHMINS"));
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
				CatchData.Reason = ServicesHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}
		
		[HttpPost]
		[Route("PastorsApplicationStatus")]
		public IHttpActionResult PastorsApplicationStatus(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				AppStatusCls rootobj = JsonConvert.DeserializeObject<AppStatusCls>(value);
				if (Utils.IsAlphaNumeric(rootobj.BenID))
					return Ok(heahel.GetMethod("https://jnanabhumi.ap.gov.in/jnbWebservices/services/prajaSachivalayam/getBillStatus?userId=admin&password=jnb@dmin20!9&aadhar=" + rootobj.BenID + "&schemeId=PASTORS"));
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
				CatchData.Reason = ServicesHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}
		
		[HttpPost]
		[Route("KapuNestamApplicationStatus")]
		public IHttpActionResult KapuNestamApplicationStatus(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				AppStatusCls rootobj = JsonConvert.DeserializeObject<AppStatusCls>(value);
				if (Utils.IsAlphaNumeric(rootobj.BenID))
					return Ok(heahel.GetMethod("https://jnanabhumi.ap.gov.in/jnbWebservices/services/prajaSachivalayam/getBillStatus?userId=admin&password=jnb@dmin20!9&aadhar=" + rootobj.BenID + "&schemeId=KAPUNESTHAM"));
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
				CatchData.Reason = ServicesHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}

		[HttpPost]
		[Route("ArogyasreeApplicationStatus")]
		public IHttpActionResult ArogyasreeApplicationStatus(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				AppStatusCls rootobj = JsonConvert.DeserializeObject<AppStatusCls>(value);
				if (Utils.IsAlphaNumeric(rootobj.BenID))
					return Ok(heahel.GetMethod("https://jnanabhumi.ap.gov.in/jnbWebservices/services/prajaSachivalayam/getBillStatus?userId=admin&password=jnb@dmin20!9&aadhar=" + rootobj.BenID + "&schemeId=AAROGYASRI"));
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
				CatchData.Reason = ServicesHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}

		[HttpPost]
		[Route("GetJVDAppStatus")]
		public IHttpActionResult GetJVDAppStatus(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				AppStatusCls rootobj = JsonConvert.DeserializeObject<AppStatusCls>(value);
				if (Utils.IsAlphaNumeric(rootobj.BenID))
					return Ok(heahel.GetMethod("https://jnanabhumi.ap.gov.in/jnbWebservices/services/prajaSachivalayam/getBillStatus?userId=admin&password=jnb@dmin20!9&aadhar=" + rootobj.BenID + "&schemeId=JVD"));
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
				CatchData.Reason = ServicesHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}
		#endregion

		#region MUID
		[HttpPost]
		[Route("GetMUIDAppStatus")]
		public IHttpActionResult GetMUIDAppStatus(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				MUIDCls rootobj = JsonConvert.DeserializeObject<MUIDCls>(value);
				return Ok(heahel.GetMUIDAppStatus(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = ServicesHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}

		#endregion

		[HttpPost]
		[Route("GetLaborAppStatus")]
		public IHttpActionResult GetLaborAppStatus(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				AppStatusCls rootobj = JsonConvert.DeserializeObject<AppStatusCls>(value);
				return Ok(heahel.GetMethod("https://apbocwwb.ap.nic.in/RegService/nic/webservice/BOCRegStatus/" + rootobj.BenID + "/Umx3QDJ3Z3c="));

			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = ServicesHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}

		[HttpPost]
		[Route("GetMeesevaAppStatus")]
		public IHttpActionResult GetMeesevaAppStatus(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				TransCls rootobj = JsonConvert.DeserializeObject<TransCls>(value);
				return Ok(heahel.GetMeesevaAppStatus_helper(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = ServicesHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}

	}
}
