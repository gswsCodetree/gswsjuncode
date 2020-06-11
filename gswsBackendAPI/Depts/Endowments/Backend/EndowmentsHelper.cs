using gswsBackendAPI.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using gswsBackendAPI.DL.CommonHel;
using gswsBackendAPI.DL.DataConnection;

namespace gswsBackendAPI.Dept.Endowments.Backend
{
	public class EndowmentsHelper: CommonSPHel
	{
		EndowmentsSPHelper Endowsphel = new EndowmentsSPHelper();

		#region Brahmin Corporation
		public dynamic GetApplicantStatus(BrahminModel oj)
		{
			dynamic obj = new ExpandoObject();
			try
			{
                if (Utils.IsAlphaNumeric(oj.INPUT))
                {
                    var val = Endowsphel.PostData("http://push147.sps.ap.gov.in/abwc/API/Schemes/GSWSGetStatus", oj);
                    var data = Endowsphel.GetSerialzedData<dynamic>(val);
                    return data;
                }
                else
                {
                    obj.Status = "Failure";
                    obj.Reason = "Invalid Input Format";
                    return obj;
                }
			}
			catch (Exception ex)
			{
				obj.Status = "Failure";
				obj.Reason =ThirdpartyMessage;
				return obj;
			}

		}
		#endregion
	}
}