using gswsBackendAPI.DL.DataConnection;
using gswsBackendAPI.transactionModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace gswsBackendAPI.Depts.YATC
{
    public class YATCHelper : YATCSPHelper
    {
        #region Skill Delelopment Corporation

        public dynamic GetMethod(string url, List<Hearders> headers)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                var val = GetData(url, headers);
                //var data = GetSerialzedData<dynamic>(val);

                obj.Status = 100;
                obj.Reason = "Data Loaded Successfully.";
                obj.Details = val;

                return obj;

            }
            catch (Exception ex)
            {
                obj.Status = 102;
                obj.Reason = ThirdpartyMessage;
                return obj;
            }

        }

        public dynamic VerifySkillCanLogin(usercred root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                var val = PostDataWithHeaders("http://103.231.8.28/apssdc_unified/login?username=" + root.username + "&password=" + root.password + "&type=" + root.type);
                var data = GetSerialzedData<dynamic>(val);

                obj.Status = 100;
                obj.Reason = "Data Loaded Successfully.";
                obj.Details = data;

                return obj;
            }
            catch (Exception ex)
            {
                obj.Status = 102;
                obj.Reason = ThirdpartyMessage;
                return obj;
            }

        }

        public dynamic SkillCandidateReg(CanReg root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                var val = PostData("http://103.231.8.28/apssdc_unified/api/mobile/savecandidate", root);
                var data = GetSerialzedData<dynamic>(val);
				transactionModel objtrans = new transactionModel();
				objtrans.TYPE = "2";
				objtrans.TXN_ID = root.GSWS_ID;
				objtrans.DEPT_ID = "3501";
				objtrans.DEPT_TXN_ID = root.aadharNumber;
				objtrans.BEN_ID = root.aadharNumber;
				objtrans.STATUS_CODE = "01";
				objtrans.REMARKS = data.message.ToString();
				try
				{
					DataTable dt = new transactionHelper().transactionInsertion(objtrans);
					if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
					{
						obj.Status = 100;
						obj.Reason = "Data Loaded Successfully.";
						obj.Details = data;
					}
					else
					{
						obj.Status = 100;
						obj.Reason = "Data Loaded Successfully.";
						obj.Details = data;
					}
				}
				catch (Exception ex)
				{
					string mappath = HttpContext.Current.Server.MapPath("SkillExceptionLogs");
					Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From LoadDepartments:" + ex.Message.ToString() + data));

					obj.Status = 100;
					obj.Reason = "Data Loaded Successfully.";
					obj.Details = data;
				}
                return obj;
            }
            catch (Exception ex)
            {
				string mappath = HttpContext.Current.Server.MapPath("SkillExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From LoadDepartments:" + ex.Message.ToString()));

				obj.Status = 102;
                obj.Reason = ThirdpartyMessage;
                return obj;
            }

        }

        public dynamic ApplyForJobs(JobsCls root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                var val = PostData("http://103.231.8.28/apssdc_unified/api/mobile/candidate/applyforjob?appKey=" + root.appKey + "&userMasterId=" + root.userMasterId, root.JobIds);
                var data = GetSerialzedData<dynamic>(val);

                obj.Status = 100;
                obj.Reason = "Data Loaded Successfully.";
                obj.Details = data;

                return obj;
            }
            catch (Exception ex)
            {
                obj.Status = 102;
                obj.Reason = ThirdpartyMessage;
                return obj;
            }

        }

        #endregion
    }
}