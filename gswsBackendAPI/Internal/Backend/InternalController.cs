using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using gswsBackendAPI.Support;
using System.Web;
using System.Threading.Tasks;
using gswsBackendAPI.DL.DataConnection;

namespace gswsBackendAPI.Internal.Backend
{
	[RoutePrefix("api/Internal")]
	public class InternalController : ApiController
	{
		dynamic CatchData = new ExpandoObject();
		InternalHelper Internalhel = new InternalHelper();
		ServerSideValidations _valid = new ServerSideValidations();

		#region Internal URL
		//Load Departments
		[HttpPost]
		[Route("LoadDepartments")]
		public IHttpActionResult LoadDepartments(dynamic data)
		{
			try
			{
				//string value = token_gen.Authorize_aesdecrpty(data);
				string value = JsonConvert.SerializeObject(data);
				InternalURL rootobj = JsonConvert.DeserializeObject<InternalURL>(value);
				return Ok(Internalhel.LoadDepartments(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = ex.Message;
				return Ok(CatchData);
			}

		}

		// Save Services Master
		[HttpPost]
		[Route("SaveServicesURLData")]
		public IHttpActionResult SaveServicesURLData(dynamic data)
		{
			try
			{
				string value = JsonConvert.SerializeObject(data);
				InternalURL rootobj = JsonConvert.DeserializeObject<InternalURL>(value);

				var validresult = _valid.CheckServicesURL(rootobj);
				if (validresult.Status == "Success")
				{
					return Ok(Internalhel.SaveServices_helper(rootobj));
				}
				else
				{
					CatchData.Status = "Failure";
					CatchData.Reason = validresult.Reason;
					return Ok(CatchData);
				}
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = ex.Message;
				return Ok(CatchData);
			}
		}



		// Save Services Master
		[HttpPost]
		[Route("UpdateUserManual")]
		public IHttpActionResult UpdateUserManual(dynamic data)
		{
			try
			{
				//string value = token_gen.Authorize_aesdecrpty(data);
				string value = JsonConvert.SerializeObject(data);
				string mappath = HttpContext.Current.Server.MapPath("UpdateUserManualLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, value));
				UpdateUserManualUrlModel rootobj = JsonConvert.DeserializeObject<UpdateUserManualUrlModel>(value);
				return Ok(Internalhel.UpdateInternal(rootobj));

			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = ex.Message;
				return Ok(CatchData);
			}
		}


		// Save Services Master
		[HttpPost]
		[Route("SaveFeedBackReport")]
		public IHttpActionResult SaveFeedBackReport(dynamic data)
		{
			 string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				//string value = token_gen.Authorize_aesdecrpty(data);
				//string value = JsonConvert.SerializeObject(data);
				string mappath = HttpContext.Current.Server.MapPath("SaveFeedBackReportLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, value));
				FeedBackReport rootobj = JsonConvert.DeserializeObject<FeedBackReport>(value);
				return Ok(Internalhel.PostFeedbackdata(rootobj));

			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = ex.Message;
				return Ok(CatchData);
			}
		}

		[HttpPost]
		[Route("PostSecretriatData")]
		public IHttpActionResult PostSecretriatData(dynamic data)
		{
			try
			{
				//string value = token_gen.Authorize_aesdecrpty(data);
				string value = JsonConvert.SerializeObject(data);
				string mappath = HttpContext.Current.Server.MapPath("PostSecretriatDataLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, value));
				SecretraintModel rootobj = JsonConvert.DeserializeObject<SecretraintModel>(value);
				return Ok(Internalhel.SaveSecretriatDatahel(rootobj));

			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = ex.Message;
				return Ok(CatchData);
			}
		}

		// Update URL
		[HttpPost]
		[Route("UpdateURL")]
		public IHttpActionResult UpdateURL(dynamic data)
		{
			try
			{
				//string value = token_gen.Authorize_aesdecrpty(data);
				string value = JsonConvert.SerializeObject(data);
				string mappath = HttpContext.Current.Server.MapPath("UpdateURLLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, value));
				UpdateUserManualUrlModel rootobj = JsonConvert.DeserializeObject<UpdateUserManualUrlModel>(value);
				return Ok(Internalhel.UpdateURL(rootobj));

			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = ex.Message;
				return Ok(CatchData);
			}
		}
		#endregion


		#region Issues Tracking Report

		//Load Report
		[HttpPost]
		[Route("IssuesTrackingReport")]
		public IHttpActionResult IssuesTrackingReport(dynamic data)
		{
			try
			{
				//string value = token_gen.Authorize_aesdecrpty(data);
				string value = JsonConvert.SerializeObject(data);
				issueTrackingModel rootobj = JsonConvert.DeserializeObject<issueTrackingModel>(value);
				rootobj.TYPE = "1";
				return Ok(Internalhel.IssuesTrackingReport(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = ex.Message;
				return Ok(CatchData);
			}

		}

		//Load comments
		[HttpPost]
		[Route("IssuesTrackingComments")]
		public IHttpActionResult IssuesTrackingComments(dynamic data)
		{
			try
			{
				//string value = token_gen.Authorize_aesdecrpty(data);
				string value = JsonConvert.SerializeObject(data);
				issueTrackingModel rootobj = JsonConvert.DeserializeObject<issueTrackingModel>(value);
				rootobj.TYPE = "2";
				rootobj.SECRETARIAT_ID = rootobj.REPORT_ID;
				return Ok(Internalhel.IssuesTrackingReport(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = ex.Message;
				return Ok(CatchData);
			}

		}

		//comments Addition
		[HttpPost]
		[Route("commentAddition")]
		public IHttpActionResult commentAddition(dynamic data)
		{
			try
			{
				//string value = token_gen.Authorize_aesdecrpty(data);
				string value = JsonConvert.SerializeObject(data);
				commentAddition rootobj = JsonConvert.DeserializeObject<commentAddition>(value);
				return Ok(Internalhel.commentAddition(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = ex.Message;
				return Ok(CatchData);
			}

		}


		#endregion

		#region Issue Closing
		[HttpPost]
		[Route("GetCategoriesData")]
		public IHttpActionResult GetCategoriesData(dynamic data)
		{
			try
			{
				string value = token_gen.Authorize_aesdecrpty(data);
				IssueType rootobj = JsonConvert.DeserializeObject<IssueType>(value);

				return Ok(Internalhel.LoadGetCategories(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = ex.Message;
				return Ok(CatchData);
			}

		}

		//Hardware Issues Closing
		[HttpPost]
		[Route("GetHWCategoriesData")]
		public IHttpActionResult GetHWCategoriesData(dynamic data)
		{
			try
			{
				string value = token_gen.Authorize_aesdecrpty(data);
				IssueType rootobj = JsonConvert.DeserializeObject<IssueType>(value);

				return Ok(Internalhel.GetHWCategoriesData_Hel(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = ex.Message;
				return Ok(CatchData);
			}

		}

		//Get Software and Hardware Closed Issues Data
		[HttpPost]
		[Route("GetHWSWResolvedIssues")]
		public IHttpActionResult GetHWSWResolvedIssues(dynamic data)
		{
			try
			{
				string value = JsonConvert.SerializeObject(data);
				IssueType rootobj = JsonConvert.DeserializeObject<IssueType>(value);

				return Ok(Internalhel.GetHWSWResolvedIssues_Hel(rootobj));
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
