using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Dynamic;
using System.Data;

namespace gswsBackendAPI.transactionModule
{
	[requestFilter]
	[AuthorizationFilter]
	[RoutePrefix("api/transaction")]
	public class transactionController : ApiController
	{
		readonly transactionHelper _Hel = new transactionHelper();
	
		[Route("initiateTransaction")]
		[HttpPost]
		public IHttpActionResult initiateTransaction(dynamic data)
		{
			string serialized_data = token_gen.Authorize_aesdecrpty(data);
			try
			{

				transactionModel rootobj = JsonConvert.DeserializeObject<transactionModel>(serialized_data);
				return Ok(_Hel.initiateTransaction(rootobj));
			}
			catch (Exception ex)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
			}
		}

		[Route("SpandanainitiateTransaction")]
		[HttpPost]
		public IHttpActionResult SpandanainitiateTransaction(dynamic data)
		{
			string serialized_data = token_gen.Authorize_aesdecrpty(data);
			try
			{

				transactionModel rootobj = JsonConvert.DeserializeObject<transactionModel>(serialized_data);
				return Ok(_Hel.initiateSpandanaTransaction(rootobj));
			}
			catch (Exception ex)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
			}
		}

		[Route("closingTransaction")]
		[HttpPost]
		public IHttpActionResult closingTransaction(dynamic data)
		{
			string serialized_data = JsonConvert.SerializeObject(data);
			string mappath = HttpContext.Current.Server.MapPath("closingTransactionLogs");
			Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, serialized_data));
			transactionModel rootobj = JsonConvert.DeserializeObject<transactionModel>(serialized_data);
			return Ok(_Hel.closingTransaction(rootobj));
		}

		[Route("authenticateUserSendOTP")]
		[HttpPost]
		public IHttpActionResult authenticateUserSendOTP(dynamic data)
		{
			string serialized_data = JsonConvert.SerializeObject(data);
			userAuthenticationModel rootobj = JsonConvert.DeserializeObject<userAuthenticationModel>(serialized_data);
			return Ok(_Hel.authenticateUserSendOTP(rootobj));
		}

		[Route("authenticateUserVerifyOTP")]
		[HttpPost]
		public IHttpActionResult authenticateUserVerifyOTP(dynamic data)
		{
			string serialized_data = JsonConvert.SerializeObject(data);
			userAuthenticationModel rootobj = JsonConvert.DeserializeObject<userAuthenticationModel>(serialized_data);
			return Ok(_Hel.authenticateUserVerifyOTP(rootobj));
		}

		[Route("authenticatedUserDetails")]
		[HttpPost]
		public IHttpActionResult authenticatedUserDetails(dynamic data)
		{
			string serialized_data = JsonConvert.SerializeObject(data);
			userAuthenticationModel rootobj = JsonConvert.DeserializeObject<userAuthenticationModel>(serialized_data);
			return Ok(_Hel.authenticatedUserDetails(rootobj));
		}

		[Route("PaymentConfirmationByMeeseva")]
		[HttpPost]
		public string PaymentConfirmationByMeeseva(dynamic data)
		{
			try
			{
				string serialized_data = JsonConvert.SerializeObject(data);
				string mappath = HttpContext.Current.Server.MapPath("PaymentConfirmationByMeesevaLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, serialized_data));
				RootMeesevaObject _objroot = JsonConvert.DeserializeObject<RootMeesevaObject>(serialized_data);
				return _Hel.SavePaymentConfirmation(_objroot);
				//return "0#Success#GSWS" + DateTime.Now.ToString("ddMMyy") + new Random().Next(1000, 9999);
				//return Ok(_Hel.authenticatedUserDetails(rootobj));
			}
			catch (Exception ex)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
			}
		}

		[Route("PaymentResponseByMeeseva")]
		[HttpPost]
		public IHttpActionResult PaymentResponseByMeeseva(dynamic data)
		{
			try
			{
				dynamic obj = new ExpandoObject();
				string serialized_data = JsonConvert.SerializeObject(data);
				string mappath = HttpContext.Current.Server.MapPath("PaymentResponseByMeesevaLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, serialized_data));
				RootMeesevaResponseObject _objmeeseva = JsonConvert.DeserializeObject<RootMeesevaResponseObject>(serialized_data);
				//obj.Status = "100";
				//  obj.Reason = "Data Submitted Successfully";
				// return Ok(obj);
				return Ok(_Hel.SavePaymentResponse(_objmeeseva));
			}
			catch (Exception ex)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
			}
		}

		
	}
}
