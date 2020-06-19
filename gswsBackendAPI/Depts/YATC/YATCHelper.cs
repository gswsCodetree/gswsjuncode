using gswsBackendAPI.DL.DataConnection;
using gswsBackendAPI.transactionModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using gswsBackendAPI.DL.CommonHel;

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
                var val = PostDataWithHeaders("https://www.apssdc.in/home/login?username=" + root.username + "&password=" + root.password + "&type=" + root.type);
                var data = GetSerialzedData<dynamic>(val);

                obj.Status = 100;
                obj.Reason = "Data Loaded Successfully.";
                obj.Details = data;

                return obj;
            }
            catch (Exception ex)
            {
                Common_YATC_Error(ex.Message.ToString(), "https://www.apssdc.in/home/login?username=", "2");
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
                var val = PostData("https://www.apssdc.in/home/api/mobile/savecandidate", root);
                var data = GetSerialzedData<dynamic>(val);
				transactionModel objtrans = new transactionModel();
				objtrans.TYPE = "2";
				objtrans.TXN_ID = root.gsws_id;
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
                    Common_YATC_Error(ex.Message.ToString(), "https://www.apssdc.in/home/api/mobile/savecandidate", "2");
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
                var val = PostData("https://www.apssdc.in/home/api/mobile/candidate/applyforjob?appKey=" + root.appKey + "&userMasterId=" + root.userMasterId, root.JobIds);
                var data = GetSerialzedData<dynamic>(val);

                obj.Status = 100;
                obj.Reason = "Data Loaded Successfully.";
                obj.Details = data;

                return obj;
            }
            catch (Exception ex)
            {
                Common_YATC_Error(ex.Message.ToString(), "https://www.apssdc.in/home/api/mobile/candidate/applyforjob?appKey=", "2");
                obj.Status = 102;
                obj.Reason = ThirdpartyMessage;
                return obj;
            }

        }
        public bool Common_YATC_Error(string msg, string url, string etype)
        {
            ExceptionDataModel objex = new ExceptionDataModel();
            try
            {
                objex.E_DEPTID = DepartmentEnum.Department.Youth_Advancement_Tourism_and_Culture.ToString();
                objex.E_HODID = DepartmentEnum.HOD.AP_Tourism_Development_Corporation.ToString();
                objex.E_ERRORMESSAGE = msg;
                objex.E_SERVICEAPIURL = url;
                objex.E_ERRORTYPE = etype;
                new LoginSPHelper().Save_Exception_Data(objex);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion 
    }
}