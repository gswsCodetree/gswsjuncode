using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Web.Http;
using System.Net.Http.Headers;
using System.Web.Mvc.Filters;
using gswsBackendAPI.DL.CommonHel;
using System.Data;
using System.Dynamic;
using Newtonsoft.Json;

namespace gswsBackendAPI
{
	public sealed class requestFilterAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			if (actionContext == null)
			{
				throw new ArgumentNullException("actionContext");
			}

			var headers = actionContext.Request.Headers;
			//string sec_key = string.Empty;
			//if (headers.Contains("SEC_KEY"))
			//{
				//sec_key = headers.GetValues("SEC_KEY").FirstOrDefault();
				//if (actionContext.Request.Method.Method != "GET")
				//{

				var tokenCookie = headers
					.GetCookies()
					.Select(c => c[AntiForgeryConfig.CookieName])
					.FirstOrDefault();


				//string val = actionContext.Request.Headers.GetValues("hkey").First();
				var tokenHeader = string.Empty;
				if (headers.Contains("X-XSRF-Token"))
				{
					tokenHeader = headers.GetValues("X-XSRF-Token").FirstOrDefault();
				}
			AntiForgery.Validate(tokenCookie != null ? tokenCookie.Value : null, tokenHeader);
			//	if (UnixTimeStampToDateTime(long.Parse(sec_key)).AddMinutes(1) > DateTime.Now)
			//	{
			//		AntiForgery.Validate(tokenCookie != null ? tokenCookie.Value : null, tokenHeader);
			//	}
			//	else
			//	{
			//		actionContext.Response = get_status("Expired");
			//	}
			//}
			//else
			//{
			//	actionContext.Response = get_status("Key Missed");
			//}

			//}

			base.OnActionExecuting(actionContext);
		}

		public static DateTime UnixTimeStampToDateTime(long unixTime)
		{
			try
			{
				var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				return epoch.AddMilliseconds(unixTime);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		
	}

	public sealed class AuthorizationFilterAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			if (actionContext == null)
			{
				throw new ArgumentNullException("actionContext");
			}

			//if (actionContext.Request.Method.Method != "GET")
			//{
			var headers = actionContext.Request.Headers;

			//string val = actionContext.Request.Headers.GetValues("hkey").First();
			var tokenHeader = string.Empty;
			if (headers.Contains("hkey"))
			{
				tokenHeader = headers.GetValues("hkey").First();
				if (tokenHeader != "admin")
				{
					HeadertokenModel objl = new HeadertokenModel();
					objl.Ftype = "4";
					objl.Token = tokenHeader;
					try
					{
						string status =new LoginHelper().SaveToken_Login(objl);
						if (status.Equals("Success"))
						{
						}
						else
						{
							actionContext.Response = get_status("Another User is Login With Same Credentials");
							//throw new UnauthorizedAccessException();
						}
					}
					catch (Exception ex)
					{
						if(ex.Message.ToString().Contains("ORA-12520:"))
						actionContext.Response = get_status("Server is busy Please try again some other time");
						else
							actionContext.Response = get_status(ex.Message.ToString());
					}
				}
			}

			// AntiForgery.Validate(tokenCookie != null ? tokenCookie.Value : null, tokenHeader);
			//}

			base.OnActionExecuting(actionContext);
		}

	
		private HttpResponseMessage get_status(string data)
		{
			dynamic objdata = new ExpandoObject();
			objdata.Status = "428";
			objdata.Reason = data;
			string rootdata = JsonConvert.SerializeObject(objdata);
			HttpResponseMessage responseMessage = new HttpResponseMessage()
			{
				Content = new StringContent(rootdata)
			};
			responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			return responseMessage;
		}
	}



}