using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using gswsBackendAPI.DL.CommonHel;

namespace gswsBackendAPI.Depts.Health
{
    public class HealthHelper : HealthSPModel
    {
		public dynamic GetArogyaRakshaStatus_helper(AppStatus root)
		{
			dynamic obj = new ExpandoObject();
			try
			{
                if (Utils.IsAlphaNumeric(root.fadhar_no))
                {
                    DataTable data = GetArogyaRakshaStatus_data_helper(root);


                    if (data != null)
                    {
                        obj.Status = 100;
                        obj.Reason = "Data Loaded Successfully.";
                        obj.Details = data;
                    }
                    else
                    {
                        obj.Status = 101;
                        obj.Reason = "No Data Found";
                    }
                }
                else
                {
                    obj.Status = 102;
                    obj.Reason = "Does not contain special characters";
                }

			}
			catch (Exception ex)
			{
				obj.Status = 102;
				obj.Reason = "Error Occured";

			}

			return obj;

		}

		public dynamic GetMethod(string url)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				var val = GetData(url);
				obj.Status = 100;
				obj.Details = val;
				return obj;
			}
			catch (Exception ex)
			{
				obj.Status = 102;
				obj.Reason = "Error Occured";
				return obj;
			}

		}
	}
}