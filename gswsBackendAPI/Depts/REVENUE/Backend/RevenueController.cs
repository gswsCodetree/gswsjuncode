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
using gswsBackendAPI.DL.CommonHel;

namespace gswsBackendAPI.Depts.REVENUE.Backend
{
	[RoutePrefix("api/REVENUE")]
	public class RevenueController : ApiController
    {
		dynamic CatchData = new ExpandoObject();
		RevenueHelper Revenuehel = new RevenueHelper();

		#region REVENUE
		//Get Applicants Status
		[HttpPost]
		[Route("GetApplicantStatus")]
		public IHttpActionResult GetApplicantStatus(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				//
				//string value = JsonConvert.SerializeObject(data);
				RevenueModel rootobj = JsonConvert.DeserializeObject<RevenueModel>(value);
                if (Utils.IsAlphaNumeric(rootobj.RID) && Utils.IsAlphaNumeric(rootobj.UID))
                    return Ok(Revenuehel.GetApplicantStatus(rootobj));
                else
                {
                    dynamic RData = new ExpandoObject();
                    RData.status = 102;
                    RData.Reason = "Special Characters Not Allowed.";
                    return Ok(RData);
                }
            }
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		[HttpPost]
		[Route("GetSpandanaUIDDetails")]
		public IHttpActionResult GetSpandanaUIDDetails(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				RevenueModel rootobj = JsonConvert.DeserializeObject<RevenueModel>(value);

				return Ok(Revenuehel.GetUIDSPadanaDetails(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		[HttpPost]
		[Route("GetSpandanaMaster")]
		public IHttpActionResult GetSpandanaMaster(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				Seccmastermodel rootobj = JsonConvert.DeserializeObject<Seccmastermodel>(value);

				return Ok(Revenuehel.GetSpandanaMaster(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		[HttpPost]
		[Route("GetSeccMaster")]
		public IHttpActionResult GetSeccMaster(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				Seccmastermodel rootobj = JsonConvert.DeserializeObject<Seccmastermodel>(value);

				return Ok(Revenuehel.GetSeccMaster(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}
		[HttpPost]
		[Route("GetSpadanaToken")]
		public IHttpActionResult GetSpadanaToken(dynamic data)
		{
			try
			{
				//string value = token_gen.Authorize_aesdecrpty(data);
				//string value = JsonConvert.SerializeObject(data);
				//RevenueModel rootobj = JsonConvert.DeserializeObject<RevenueModel>(value);
				return Ok(Revenuehel.getSpandanToken());
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		[HttpPost]
		[Route("GetSpandaGrievanceToken")]
		public IHttpActionResult GetSpandaGrievanceToken(dynamic data)
		{
			try
			{
				//string value = token_gen.Authorize_aesdecrpty(data);
				//string value = JsonConvert.SerializeObject(data);
				//RevenueModel rootobj = JsonConvert.DeserializeObject<RevenueModel>(value);
				return Ok(Revenuehel.GetSpandanaGrievanceToken());
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Message = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		[HttpPost]
		[Route("GetSpandanaDepartments")]
		public IHttpActionResult GetSpandanaDepartments(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				SpandanaInputdata rootobj = JsonConvert.DeserializeObject<SpandanaInputdata>(value);

			      if(rootobj.ftype=="1") //departments
				return Ok(Revenuehel.GetSpandanaDepartment(rootobj));
				else if (rootobj.ftype == "2") //subject
					return Ok(Revenuehel.Getsubject(rootobj));
				else if (rootobj.ftype == "3") // sub subject
					return Ok(Revenuehel.GetSubSubject(rootobj));
				else if (rootobj.ftype == "4")//search keyword
					return Ok(Revenuehel.getkeywordsubsubject(rootobj));				
				  else
					return Ok(Revenuehel.GetSpandanaDepartment(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}
		[HttpPost]
		[Route("GetSpandanaStatusCheck")]
		public IHttpActionResult GetSpandanaStatusCheck(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				SpandanaStatusModel rootobj = JsonConvert.DeserializeObject<SpandanaStatusModel>(value);

			
					return Ok(Revenuehel.GetSpandanaCheck(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		[HttpPost]
		[Route("SapandaSubmit")]
		public IHttpActionResult SapandaSubmit(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);

			try
			{
				
				string mappath = HttpContext.Current.Server.MapPath("SapandaSubmitLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, value));
				RootSpandaObject root = JsonConvert.DeserializeObject<RootSpandaObject>(value);
				//CatchData.StatusCode = "200";
				//CatchData.Message = "Data Submitted Successfully";
				return Ok(Revenuehel.SavegetSpandanaGrievance(root));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		[HttpPost]
		[Route("GetSpandanaSideDashboard")]
		public IHttpActionResult GetSpandanaSideDashboard(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				SpandanaStatusModel rootobj = JsonConvert.DeserializeObject<SpandanaStatusModel>(value);
				return Ok(Revenuehel.GetSpandanaAbstractDashboardCount(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Message = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}
		[HttpPost]
		[Route("GetSpandanaSideDashboardDetailed")]
		public IHttpActionResult GetSpandanaSideDashboardDetailed(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{

				//string value = JsonConvert.SerializeObject(data);
				SpandanaStatusModel rootobj = JsonConvert.DeserializeObject<SpandanaStatusModel>(value);
				return Ok(Revenuehel.GetSpandanaDetailedDashboardCount(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Message = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}
		#endregion

		#region Prohibition and Excise
		//Get Applicants Status
		[HttpPost]
		[Route("Excise_DistrictLoad")]
		public IHttpActionResult Excise_DistrictLoad(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);

			try
			{
				return Ok(Revenuehel.GetDistricts_Excise());
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}

		[HttpPost]
		[Route("Excise_MandalsLoad")]
		public IHttpActionResult Excise_MandalsLoad(dynamic JSONdata)
		{
			string value = token_gen.Authorize_aesdecrpty(JSONdata);

			try
			{
				dynamic data = JsonConvert.DeserializeObject<dynamic>(value);
				return Ok(Revenuehel.GetMandal_Excise(data.distcode.ToString()));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}

		[HttpPost]
		[Route("Excise_VillagesLoad")]
		public IHttpActionResult Excise_VillagesLoad(dynamic JSONdata)
		{
			string value = token_gen.Authorize_aesdecrpty(JSONdata);
			try
			{
				dynamic data = JsonConvert.DeserializeObject<dynamic>(value);
				return Ok(Revenuehel.GetVillages_Excise(data.mandalcode.ToString()));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}

		[HttpPost]
		[Route("CheckComplaintStatus")]
		public IHttpActionResult CheckComplaintStatus(dynamic Jdata)
		{
			string value = token_gen.Authorize_aesdecrpty(Jdata);

			try
			{
				dynamic data = JsonConvert.DeserializeObject<dynamic>(value);
				if (Utils.IsAlphaNumeric((string)data.appcode))
					return Ok(Revenuehel.GetComplaintStatus_Excise(data.appcode.ToString()));
				else
				{
					dynamic RData = new ExpandoObject();
					RData.Status = "Failure";
					RData.Reason = "Special Characters Not Allowed.";
					return Ok(RData);
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
		[Route("SaveExcisePCF")]
		public IHttpActionResult SaveExcisePCF(ExciseModel data)
		{
			try
			{
				string mappath = HttpContext.Current.Server.MapPath("SaveExcisePCFLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath,JsonConvert.SerializeObject(data)));
				return Ok(Revenuehel.Save_ExcisePCF_Info(data));
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
