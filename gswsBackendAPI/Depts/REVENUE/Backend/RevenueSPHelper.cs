using gswsBackendAPI.DL.DataConnection;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace gswsBackendAPI.Depts.REVENUE.Backend
{
	public class RevenueSPHelper
	{
		OracleCommand cmd;
		CommonSPHel comhel = new CommonSPHel();

		#region REVENUE
		public DataTable get_Pension_SP(RevenueModel oj)
		{
			try
			{

				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "ap_rd_pension.gsws_check_status_pension";
				cmd.Parameters.Add("ftype", OracleDbType.Varchar2, 12).Value = "2";
				cmd.Parameters.Add("fdpart_id ", OracleDbType.Varchar2, 20).Value = oj.RID;
				cmd.Parameters.Add("fadhar_no", OracleDbType.Varchar2, 20).Value = oj.UID;
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtstatus = comhel.GetsrdhDataAdapter(cmd);
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
				string mappath = HttpContext.Current.Server.MapPath("RevenueExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetApplicantStatus:" + ex.Message.ToString()));
				throw ex;
			}

		}

		public DataTable GetUidspandanaDetails(RevenueModel oj)
		{
			try
			{

				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "pss_details_meekosam";
				cmd.Parameters.Add("ftype", OracleDbType.Varchar2, 12).Value = "1";
				cmd.Parameters.Add("fuid_num", OracleDbType.Varchar2, 20).Value = oj.UID;
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
				string mappath = HttpContext.Current.Server.MapPath("RevenueExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetApplicantStatus:" + ex.Message.ToString()));
				throw ex;
			}

		}

		public DataTable GetSpandanaMasterDetails(Seccmastermodel oj)
		{
			try
			{
				
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "gsws_spandana_sec_mapping_proc";
				cmd.Parameters.Add("ftype", OracleDbType.Varchar2, 12).Value = oj.Ftype;
				cmd.Parameters.Add("fdist", OracleDbType.Varchar2, 12).Value = oj.Fdistrict;
				cmd.Parameters.Add("fmandal", OracleDbType.Varchar2, 12).Value = oj.Fmandal;
				cmd.Parameters.Add("fvillage", OracleDbType.Varchar2, 12).Value = oj.Fvillage;
				cmd.Parameters.Add("frural", OracleDbType.Varchar2, 20).Value = oj.Fruflag;
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtstatus = comhel.GetgswsDataAdapter(cmd);
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
				string mappath = HttpContext.Current.Server.MapPath("RevenueExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetApplicantStatus:" + ex.Message.ToString()));
				throw ex;
			}

		}

		public DataTable GetSeccMasterDetails(Seccmastermodel oj)
		{
			try
			{

				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GET_SECC_CODES_MASTER_PROC";
				cmd.Parameters.Add("ftype", OracleDbType.Varchar2, 12).Value = oj.Ftype;
				cmd.Parameters.Add("fdist", OracleDbType.Varchar2, 12).Value = oj.Fdistrict;
				cmd.Parameters.Add("fmandal", OracleDbType.Varchar2, 12).Value = oj.Fmandal;
				cmd.Parameters.Add("fvillage", OracleDbType.Varchar2, 12).Value = oj.Fvillage;
				cmd.Parameters.Add("frural", OracleDbType.Varchar2, 20).Value = oj.Fruflag;
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
				string mappath = HttpContext.Current.Server.MapPath("RevenueExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetApplicantStatus:" + ex.Message.ToString()));
				throw ex;
			}

		}


		public DataTable Aarogya_Raksha_SP(RevenueModel oj)
        {
            try
            {

                cmd = new OracleCommand();
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ct_schema.gsws_check_status";
                cmd.Parameters.Add("ftype", OracleDbType.Varchar2, 12).Value = oj.INPUT;
                cmd.Parameters.Add("fdpart_id ", OracleDbType.Varchar2, 20).Value = oj.RID;
                cmd.Parameters.Add("fadhar_no", OracleDbType.Varchar2, 20).Value = oj.UID;
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
                string mappath = HttpContext.Current.Server.MapPath("RevenueExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetApplicantStatus:" + ex.Message.ToString()));
                throw ex;
            }

        }
        public DataTable Get_Ration_Sp(RevenueModel oj)
        {
            try
            {

                cmd = new OracleCommand();
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ap_fcs.gsws_check_status_ration";
                cmd.Parameters.Add("ftype", OracleDbType.Varchar2, 12).Value = oj.INPUT;
                cmd.Parameters.Add("fdpart_id ", OracleDbType.Varchar2, 20).Value = oj.RID;
                cmd.Parameters.Add("fadhar_no", OracleDbType.Varchar2, 20).Value = oj.UID;
                cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                DataTable dtstatus = comhel.GetsrdhDataAdapter(cmd);
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
                string mappath = HttpContext.Current.Server.MapPath("RevenueExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetApplicantStatus:" + ex.Message.ToString()));
                throw ex;
            }

        }
		#endregion

		#region "Spandana Grievance"

		public dynamic SaveSpandanaGrievance(RootSpandaObject objspandana)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "Insert into gsws_spandana_grienvance (AADHAARNUMBER,HOUSEHOLDID,APPLICANTNAME,CAREOF,AGE,GENDER,PINCODE,MOBILE,INCOME,OCCUPATION,APPTYPEINFO,APPCREATOR,MKMDISTCODE,MKMMANDALCODE,MKMVILLAGECODE,PSSDISTCODE,PSSMANDALCODE,PSSVILLAGECODE,HODID,FORMID,PROBLEMDETAILS,HABITATION,PRESENTADDRESS,LOGINUSER)" +
					" Values(:AADHAARNUMBER,:HOUSEHOLDID,:APPLICANTNAME,:CAREOF,:AGE,:GENDER,:PINCODE,:MOBILE,:INCOME,:OCCUPATION,:APPTYPEINFO,:APPCREATOR,:MKMDISTCODE,:MKMMANDALCODE,:MKMVILLAGECODE,:PSSDISTCODE,:PSSMANDALCODE,:PSSVILLAGECODE,:HODID,:FORMID,:PROBLEMDETAILS,:HABITATION,:PRESENTADDRESS,:LOGINUSER)";
				cmd.Parameters.Add(":AADHAARNUMBER", OracleDbType.Varchar2, 12).Value = objspandana.AadhaarNumber;
				cmd.Parameters.Add(":HOUSEHOLDID", OracleDbType.Varchar2, 50).Value = objspandana.HouseHoldId;
				cmd.Parameters.Add(":APPLICANTNAME", OracleDbType.Varchar2, 200).Value = objspandana.ApplicantName;
				cmd.Parameters.Add(":CAREOF", OracleDbType.Varchar2, 200).Value = objspandana.CareOf;
				cmd.Parameters.Add(":AGE", OracleDbType.Varchar2, 50).Value = objspandana.Age;
				cmd.Parameters.Add(":GENDER", OracleDbType.Varchar2, 20).Value = objspandana.Gender;
				cmd.Parameters.Add(":PINCODE", OracleDbType.Varchar2, 12).Value = objspandana.PinCode;
				cmd.Parameters.Add(":MOBILE", OracleDbType.Varchar2, 50).Value = objspandana.Mobile;
				cmd.Parameters.Add(":INCOME", OracleDbType.Varchar2, 50).Value = objspandana.Income;
				cmd.Parameters.Add(":OCCUPATION", OracleDbType.Varchar2, 200).Value = objspandana.Occupation;
				cmd.Parameters.Add(":APPTYPEINFO", OracleDbType.Varchar2, 12).Value = objspandana.AppTypeInfo;
				cmd.Parameters.Add(":APPCREATOR ", OracleDbType.Varchar2, 50).Value = objspandana.AppCreator;
				cmd.Parameters.Add(":MKMDISTCODE", OracleDbType.Varchar2, 200).Value = objspandana.MkmDistCode;
				cmd.Parameters.Add(":MKMMANDALCODE", OracleDbType.Varchar2, 12).Value = objspandana.MkmMandalCode;
				cmd.Parameters.Add(":MKMVILLAGECODE ", OracleDbType.Varchar2, 50).Value = objspandana.MkmVillageCode;
				cmd.Parameters.Add(":PSSDISTCODE", OracleDbType.Varchar2, 200).Value = objspandana.PssDistCode;
				cmd.Parameters.Add(":PSSMANDALCODE", OracleDbType.Varchar2, 12).Value = objspandana.PssMandalCode;
				cmd.Parameters.Add(":PSSVILLAGECODE", OracleDbType.Varchar2, 50).Value = objspandana.PssVillageCode;
				cmd.Parameters.Add(":HODID", OracleDbType.Varchar2, 200).Value = objspandana.HodId;
				cmd.Parameters.Add(":FORMID", OracleDbType.Varchar2, 12).Value = objspandana.FormID;
				cmd.Parameters.Add(":PROBLEMDETAILS", OracleDbType.Varchar2, 250).Value = objspandana.ProblemDetails;
				cmd.Parameters.Add(":HABITATION", OracleDbType.Varchar2, 200).Value = objspandana.Habitation;
				cmd.Parameters.Add(":PRESENTADDRESS", OracleDbType.Varchar2, 200).Value = objspandana.PresentAddress;
				cmd.Parameters.Add(":LOGINUSER", OracleDbType.Varchar2, 200).Value = objspandana.Loginuser;
				int k = comhel.getgswsExecuteNonQuery(cmd);
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
				string mappath = HttpContext.Current.Server.MapPath("SapandaSubmitExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, ex.Message.ToString()));
				throw ex;
			}
		}
		#endregion
	}
}