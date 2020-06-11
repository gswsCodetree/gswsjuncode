using System.Web;
using System.Web.Mvc;
using static gswsBackendAPI.WebApiApplication;

namespace gswsBackendAPI
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
			//filters.Add(new ContentSecurityPolicyFilterAttribute());
		}
	}

	//public class SessionTimeoutAttribute : ActionFilterAttribute
	//{
	//	public override void OnActionExecuting(ActionExecutingContext filterContext)
	//	{
	//		HttpContext ctx = HttpContext.Current;
	//		if (HttpContext.Current.Session["userId"] == null)
	//		{
	//			filterContext.Result = new RedirectResult("~/#!/Login");
	//			return;
	//		}
	//		base.OnActionExecuting(filterContext);
	//	}
	//}
}
