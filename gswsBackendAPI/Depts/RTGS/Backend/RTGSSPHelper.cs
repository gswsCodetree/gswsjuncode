using gswsBackendAPI.DL.DataConnection;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace gswsBackendAPI.Dept.RTGS.Backend
{
	public class RTGSSPHelper
	{
		OracleCommand cmd;
		CommonSPHel comhel = new CommonSPHel();

		#region PSS
		public DataTable GetApplicantStatus(PSSModel oj)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "check_survey_status";
				cmd.Parameters.Add("ftype", OracleDbType.Varchar2, 12).Value = "2";
				cmd.Parameters.Add("fuid", OracleDbType.Varchar2, 20).Value = oj.INPUT;
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtstatus = comhel.GetspsDataAdapter(cmd);
				if (dtstatus != null && dtstatus.Rows.Count > 0)
				{
					return dtstatus;
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("PSSExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetApplicantStatus:" + ex.Message.ToString()));
				throw ex;
			}

		}


		public string SaveUnsurveyRequest(UnSurveyRequestmodel objreq)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.CommandText = @"Insert into UNSURVEYED(UID_NUM,MOBILE_NUMBER,DISTRICT_NAME,DISTRICT_ID,MANDAL_NAME,MANDAL_ID,VT_NAME,VT_ID,RURAL_URBAN_FLAG,REQUEST_TYPE,EMAIL,UNSURVEYED_MEMBER_COUNT,NETWORK_STATUS)"+
				" values(:UID_NUM,:MOBILE_NUMBER,:DISTRICT_NAME,:DISTRICT_ID,:MANDAL_NAME,:MANDAL_ID,:VT_NAME,:VT_ID,:RURAL_URBAN_FLAG,:REQUEST_TYPE,:EMAIL,:UNSURVEYED_MEMBER_COUNT,:NETWORK_STATUS)";
				cmd.Parameters.Add(":UID_NUM", OracleDbType.Varchar2, 12).Value = objreq.UID;
				cmd.Parameters.Add(":MOBILE_NUMBER", OracleDbType.Varchar2, 12).Value = objreq.MOBILE_NUMBER;
				cmd.Parameters.Add(":DISTRICT_NAME", OracleDbType.Varchar2, 120).Value = objreq.DISTRICT_NAME;
				cmd.Parameters.Add(":DISTRICT_ID", OracleDbType.Varchar2, 12).Value = objreq.DISTRICT_ID;
				cmd.Parameters.Add(":MANDAL_NAME", OracleDbType.Varchar2, 120).Value = objreq.MANDAL_NAME;
				cmd.Parameters.Add(":MANDAL_ID", OracleDbType.Varchar2, 12).Value = objreq.MANDAL_ID;
				cmd.Parameters.Add(":VT_NAME", OracleDbType.Varchar2, 120).Value = objreq.VT_NAME;
				cmd.Parameters.Add(":VT_ID", OracleDbType.Varchar2, 12).Value = objreq.VT_ID;
				cmd.Parameters.Add(":RURAL_URBAN_FLAG", OracleDbType.Varchar2, 12).Value = objreq.RURAL_URBAN_FLAG;
				cmd.Parameters.Add(":REQUEST_TYPE", OracleDbType.Varchar2, 12).Value = objreq.REQUEST_TYPE;
				cmd.Parameters.Add(":EMAIL", OracleDbType.Varchar2, 120).Value = objreq.EMAIL;
				cmd.Parameters.Add(":UNSURVEYED_MEMBER_COUNT", OracleDbType.Varchar2, 12).Value = objreq.UNSURVEYED_MEMBER_COUNT;
				cmd.Parameters.Add(":NETWORK_STATUS", OracleDbType.Varchar2, 12).Value ="GSWS";
				int k = comhel.getspsExecuteNonQuery(cmd);
				if (k > 0)
				{
					return "Success";
				}
				else
				{
					return "Failure";
				}

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("SaveUnsurveyRequestLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From SaveUnsurveyRequest:" + ex.Message.ToString()));
				throw ex;
			}
		}

		#endregion

	}
}