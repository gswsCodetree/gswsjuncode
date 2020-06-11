using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Dept.RTGS.Backend
{
	public class RTGSHelper
	{
		#region PSS
		public dynamic GetApplicantStatus(PSSModel oj)
		{
			dynamic obj = new ExpandoObject();
			RTGSSPHelper RTGSsphel = new RTGSSPHelper();

			try
			{
				DataTable dt = RTGSsphel.GetApplicantStatus(oj);
				if (dt != null && dt.Rows.Count > 0)
				{
					obj.Status = "Success";
					obj.Reason = "";
					obj.Details = dt;
				}
				else
				{
					obj.Status = "Failure";
					obj.Reason = "No Data Found";
				}
			}
			catch (Exception ex)
			{
				obj.Status = "Failure";
				obj.Reason = "Error Occured While Getting Data";
			}
			return obj;
		}
		#endregion
	}
}