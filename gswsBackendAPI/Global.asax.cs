using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace gswsBackendAPI
{
	public class WebApiApplication : System.Web.HttpApplication
	{

		protected void Application_BeginRequest()
		{
			//Response.AddHeader("X-Frame-Options", "DENY");
			Response.AddHeader("X-XSS-Protection", "1; mode=block");
			Response.AddHeader("X-Content-Type-Options", "nosniff");
			Response.AddHeader("Content-Security-Policy", "default-src 'self' 'unsafe-inline' http: https:; script-src 'self' 'unsafe-inline' http: https:; img-src 'self' http: https: data:; style-src 'self' 'unsafe-inline' http: https:; style-src-elem 'self' 'unsafe-inline' http: https:");
		}
		protected void Application_Start()
		{

			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);			
			MvcHandler.DisableMvcResponseHeader = true;
		}
		protected void Application_PostAuthorizeRequest()
		{
			System.Web.HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
		}

		//public class ContentSecurityPolicyFilterAttribute : ActionFilterAttribute
		//{
		//	public override void OnActionExecuting(ActionExecutingContext filterContext)
		//	{
		//		HttpResponseBase response = filterContext.HttpContext.Response;

		//		response.AddHeader("Content-Security-Policy", "default-src *;");

		//		base.OnActionExecuting(filterContext);
		//	}
		//}

	}
}
