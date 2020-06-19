using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Internal.Backend
{
	public class InternalHelper : InternalSPHelper
	{


		#region Issues Tracking Report

		public dynamic commentAddition(commentAddition obj)
		{
			dynamic objupdate = new ExpandoObject();
			try
			{
				if (commentAdditionHelper(obj))
				{
					objupdate.Status = 200;
					objupdate.Reason = "Your Record Updated Successfully";
				}
				else
				{
					objupdate.Status = 400;
					objupdate.Reason = "Data Not Submitted Please Try Again";
				}
			}
			catch (Exception ex)
			{
				objupdate.Status = 500;
				objupdate.Reason = ex.Message;
			}
			return objupdate;
		}

		public dynamic IssuesTrackingReport(issueTrackingModel oj)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				DataTable dt = IssuesTrackingReportProc(oj);
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
				obj.Reason = ex.Message;
				return obj;
			}
			return obj;
		}


		#endregion



		#region Internal URL
		public dynamic LoadDepartments(InternalURL oj)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				DataTable dt = LoadDepartments_helper(oj);
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
				obj.Reason = ex.Message;
				return obj;
			}
			return obj;
		}

		// Load Geographical Data
		public dynamic LoadGeographicalData(InternalURL oj)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				DataTable dt = LoadGeographicalData_helper(oj);
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
				obj.Reason = ex.Message;
				return obj;
			}
			return obj;
		}

		// Save Services Url's Data
		public dynamic SaveServices_helper(InternalURL root)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				Int32 UniuqeCode = GetUniqueCode(root);
				UniuqeCode = UniuqeCode + 1;
				root.URL_ID = root.SERVICE + (UniuqeCode < 9 ? ("0" + UniuqeCode.ToString()) : UniuqeCode.ToString());
				DataTable data = SaveServices_data_helper(root);
				if (data != null)
				{
					obj.Status = "Success";
					obj.Reason = "Data Inserted Successfully.";
					obj.Details = data;
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
				obj.Reason = ex.Message;

			}

			return obj;

		}

		public dynamic UpdateInternal(UpdateUserManualUrlModel obj)
		{
			dynamic objupdate = new ExpandoObject();
			try
			{
				string Status = UpdateUserManualURL(obj);

				if (Status == "Success")
				{
					objupdate.Status = "100";
					objupdate.Reason = "Your Record Updated Successfully";
				}
				else
				{
					objupdate.Status = "102";
					objupdate.Reason = "Data Not Submitted Please Try Again";
				}
			}
			catch (Exception ex)
			{
				objupdate.Status = "102";
				objupdate.Reason = ex.Message;
			}
			return objupdate;
		}

		public dynamic PostFeedbackdata(FeedBackReport obj)
		{
			dynamic objupdate = new ExpandoObject();
			try
			{
				obj.REPORTID = obj.SECRETRIATID + DateTime.Now.ToString("ddMMyyHHmmssfff");
				string Status = SaveFeedbackReport(obj);

				if (Status == "Success")
				{
					objupdate.Status = "100";
					objupdate.Reason = "Your Record Updated Successfully!Your Token ID is" + "  " + obj.REPORTID;
				}
				else
				{
					objupdate.Status = "102";
					objupdate.Reason = "Data Not Submitted Please Try Again";
				}
			}
			catch (Exception ex)
			{
				objupdate.Status = "102";
				objupdate.Reason = ex.Message;
			}
			return objupdate;
		}

		public dynamic SaveReOpenTicket_Helper(FeedBackReport obj)
		{
			dynamic objupdate = new ExpandoObject();
			try
			{
				string Status = SaveReOpenTicket_Helper_SP(obj);

				if (Status == "Success")
				{
					objupdate.Status = "100";
					objupdate.Reason = "Token Re-open Successfully.";
				}
				else
				{
					objupdate.Status = "102";
					objupdate.Reason = "Data Not Submitted Please Try Again";
				}
			}
			catch (Exception ex)
			{
				objupdate.Status = "102";
				objupdate.Reason = ex.Message;
			}
			return objupdate;
		}

		public dynamic SaveSecretriatDatahel(SecretraintModel obj)
		{
			dynamic objupdate = new ExpandoObject();
			try
			{
				string Status = SaveSecretriatData(obj);

				if (Status == "Success")
				{
					objupdate.Status = "100";
					objupdate.Reason = "Your Record Updated Successfully";
				}
				else
				{
					objupdate.Status = "102";
					objupdate.Reason = "Data Not Submitted Please Try Again";
				}
			}
			catch (Exception ex)
			{
				objupdate.Status = "102";
				objupdate.Reason = ex.Message;
			}
			return objupdate;
		}

		public dynamic UpdateURL(UpdateUserManualUrlModel obj)
		{
			dynamic objupdate = new ExpandoObject();
			try
			{
				string Status = UpdateURLHelper(obj);

				if (Status == "Success")
				{
					objupdate.Status = "100";
					objupdate.Reason = "Your Record Updated Successfully";
				}
				else
				{
					objupdate.Status = "102";
					objupdate.Reason = "Data Not Submitted Please Try Again";
				}
			}
			catch (Exception ex)
			{
				objupdate.Status = "102";
				objupdate.Reason = ex.Message;
			}
			return objupdate;
		}
		#endregion

		#region Issue Closing
		public dynamic LoadGetCategories(IssueType oj)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				DataTable dt = GetCategories(oj);
				if (dt != null && dt.Rows.Count > 0)
				{
					obj.Status = "Success";
					obj.Reason = "";
					if (oj.Type == 5 || oj.Type == 6)
						obj.Details = dt.Rows[0][0].ToString();
					else
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
				obj.Reason = ex.Message;
				return obj;
			}
			return obj;
		}


		//Hardware Issue closing
		public dynamic GetHWCategoriesData_Hel(IssueType oj)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				DataTable dt = GetHWCategoriesData_SPHel(oj);
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
					obj.Details = "";
				}
			}
			catch (Exception ex)
			{
				if (ex.Message.ToString().Contains("ORA-12520:"))
				{
					obj.Status = "Failure";
					obj.Reason = "Server busy.Please try again";
				}
				else
				{
					obj.Status = "Failure";
					obj.Reason = ex.Message;
				}
				return obj;
			}
			return obj;
		}

		//SWHE Closed Issues Data
		public dynamic GetHWSWResolvedIssues_Hel(IssueType oj)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				DataTable dt = GetHWSWResolvedIssues_SPHel(oj);
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
					obj.Details = "";
				}
			}
			catch (Exception ex)
			{
				obj.Status = "Failure";
				obj.Reason = ex.Message;
				return obj;
			}
			return obj;
		}
		#endregion
	}
}