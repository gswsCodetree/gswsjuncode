using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using gswsBackendAPI.DL.CommonHel;
using gswsBackendAPI.DL.DataConnection;
using Oracle.ManagedDataAccess.Client;

namespace gswsBackendAPI.Depts.PRRD
{
    public class PRRDSPHelper : CommonSPHel
    {
		OracleCommand cmd;

		public DataTable GetValunteerMapping_data_helper(VolunteerCls obj)
        {
            try
            {
                var comd = new OracleCommand();

                comd.InitialLONGFetchSize = 1000;
                comd.CommandType = CommandType.StoredProcedure;
                comd.CommandText = "youth_service.vv_id_details_updations_proc";
                comd.Parameters.Add("ftype", OracleDbType.Varchar2).Value = obj.ftype;
                comd.Parameters.Add("fuid_num", OracleDbType.Varchar2).Value = obj.fuid_num;
                comd.Parameters.Add("fvv_id", OracleDbType.Varchar2).Value = obj.fvv_id;
                comd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                DataTable data = GetspsDataAdapter(comd);
                if (data != null && data.Rows.Count > 0)
                    return data;
                else
                    return null;
            }
            catch (WebException wex)
            {
                string mappath = HttpContext.Current.Server.MapPath("VolunteerMappingExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Volunteer Mapping Data API:" + wex.Message.ToString()));
                throw wex;
            }
        }

		public DataTable GetSecretraiatToVolunteer_Sp(SecretriatVVModel obj)
		{
			try
			{
				var comd = new OracleCommand();

				comd.InitialLONGFetchSize = 1000;
				comd.CommandType = CommandType.StoredProcedure;
				comd.CommandText = "youth_service.PSS_DETAILS_UPD_PROC";
				comd.Parameters.Add("ptype", OracleDbType.Varchar2).Value = obj.TYPE;
				comd.Parameters.Add("pdist", OracleDbType.Varchar2).Value = obj.DISTRICTCODE;
				comd.Parameters.Add("pmand", OracleDbType.Varchar2).Value = obj.MANDALCODE;
				comd.Parameters.Add("pgsws_code", OracleDbType.Varchar2).Value = obj.SECRETRIATCODE;
				comd.Parameters.Add("pvv_id", OracleDbType.Varchar2).Value = obj.VVID;
				comd.Parameters.Add("phh_id", OracleDbType.Varchar2).Value = obj.HHID;
				comd.Parameters.Add("ptemp_id", OracleDbType.Varchar2).Value = obj.TEMPID;
				comd.Parameters.Add("pupdated_by", OracleDbType.Varchar2).Value = obj.UpdateBy;
				comd.Parameters.Add("pupdated_date", OracleDbType.Date).Value = obj.UpdateDate;
				comd.Parameters.Add("pstatus", OracleDbType.Varchar2).Value = obj.Status;
				comd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

				DataTable data = GetspsDataAdapter(comd);
				if (data != null && data.Rows.Count > 0)
					return data;
				else
					return null;
			}
			catch (WebException wex)
			{
				string mappath = HttpContext.Current.Server.MapPath("GetSecretraiatToVolunteerExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Volunteer Mapping Data API:" + wex.Message.ToString()));
				throw wex;
			}
		}

		#region Birth or Death Registration
		public DataTable LoadDistricts_helper(DistrictsCls oj)
		{
			try
			{

				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_GET_SERVICE_DETAILS";
				cmd.Parameters.Add("FTYPE", OracleDbType.Varchar2, 12).Value = oj.TYPE;
				cmd.Parameters.Add("FDEPT_ID", OracleDbType.Varchar2, 12).Value = oj.DEPARTMENT;
				cmd.Parameters.Add("FHOD_ID", OracleDbType.Varchar2, 12).Value = oj.HOD;
				cmd.Parameters.Add("FDIST_CODE", OracleDbType.Varchar2, 12).Value = oj.DISTRICT;
				cmd.Parameters.Add("FMAND_CODE", OracleDbType.Varchar2, 12).Value = oj.MANDAL;
				cmd.Parameters.Add("FVILL_CODE", OracleDbType.Varchar2, 12).Value = oj.PANCHAYAT;
				cmd.Parameters.Add("P_URFLG", OracleDbType.Varchar2, 12).Value = oj.RUFLAG;
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtstatus = GetgswsDataAdapter(cmd);
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
				string mappath = HttpContext.Current.Server.MapPath("BirthExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From Load Districts Data :" + ex.Message.ToString()));
				throw ex;
			}

		}

		public DataTable GetGSWS_SecretariatMaster_LGD_SP(LGDMasterModel ObjLGD) //type=6  and secc code to lgd code

		{
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_GET_SECRETARIAT_MASTER";
				cmd.Parameters.Add("ftype", OracleDbType.Varchar2, 10).Value = ObjLGD.FTYPE;
				cmd.Parameters.Add("FDIST_ID", OracleDbType.Varchar2, 50).Value = ObjLGD.DISTCODE;
				cmd.Parameters.Add("FMANDAL_ID", OracleDbType.Varchar2, 50).Value = ObjLGD.MCODE;
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtLogin = GetProdgswsDataAdapter(cmd);
				if (dtLogin != null && dtLogin.Rows.Count>0)
				{
					return dtLogin;//dtreturn dtLogin.Rows[0]["GP_CODE"].ToString();
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("LoginSPExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetLogin_SP:" + ex.Message.ToString()));
				throw ex;
			}
		}
		#endregion

		#region   JobcardRegistration
		public DataTable GetDistandMandalCode_SP(LGDMasterModel ObjLGD)
		{
			try
			{

				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "PR_GET_GSWS_PRRD_HAB_CODES";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Int32, 10).Value = ObjLGD.FTYPE;
				cmd.Parameters.Add("P_LGD_DISTRICT_CODE", OracleDbType.Int32, 50).Value = ObjLGD.DISTCODE;
				cmd.Parameters.Add("P_LGD_MANDAL_CODE", OracleDbType.Int32, 50).Value = ObjLGD.MandalCode;
				cmd.Parameters.Add("P_PANCHAYAT_ID", OracleDbType.Varchar2, 50).Value = ObjLGD.GPCODE;
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtLogin = GetgswsDataAdapter(cmd);
				if (dtLogin != null)
				{
					return dtLogin;
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{

				string mappath = HttpContext.Current.Server.MapPath("JobCardExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error GetDistandMandalCode_SP" + ex.Message.ToString()));
				throw ex;
			}
		}
		public DataTable GetBankDetails_SP(JobCardBankModel ObjLGD)
		{
			try
			{

				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "PR_GET_BANK_IFSC_CODES";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2, 10).Value = ObjLGD.TYPE;
				cmd.Parameters.Add("P_BANK_NAME", OracleDbType.Varchar2, 50).Value = ObjLGD.BankName;
				cmd.Parameters.Add("P_BRANCH_NAME", OracleDbType.Varchar2, 50).Value = ObjLGD.BranchName;
				cmd.Parameters.Add("P_IFSC_CODE", OracleDbType.Varchar2, 50).Value = ObjLGD.IFSCCode;
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtLogin = GetgswsDataAdapter(cmd);
				if (dtLogin != null)
				{
					return dtLogin;
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobCardExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error GetBankDetails_SP" + ex.Message.ToString()));
				throw ex;
			}
		}

		public DataTable GetHabitationCode_SP(LGDMasterModel ObjLGD)
		{
			try
			{

				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "PR_GET_GSWS_PRRD_HAB_CODES";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Int32, 10).Value = ObjLGD.FTYPE;
				cmd.Parameters.Add("P_LGD_DISTRICT_CODE", OracleDbType.Int32, 50).Value = ObjLGD.DISTCODE;
				cmd.Parameters.Add("P_LGD_MANDAL_CODE", OracleDbType.Int32, 50).Value = ObjLGD.MandalCode;
				cmd.Parameters.Add("P_PANCHAYAT_ID", OracleDbType.Varchar2, 50).Value = ObjLGD.GPCODE;
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtLogin = GetgswsDataAdapter(cmd);
				if (dtLogin != null)
				{
					return dtLogin;
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobCardExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error GetHabitationCode_SP" + ex.Message.ToString()));
				throw ex;
			}
		}
		public DataTable CreateJobCard_SP(List<JobCardModel> Objitem)
		{
			// Start a local transaction
			OracleTransaction transaction;
			ConnectionHelper _conHe = new ConnectionHelper();
			OracleConnection con = new OracleConnection(_conHe.Congsws);
			if (con.State == ConnectionState.Closed)
				con.Open();

			transaction = con.BeginTransaction(IsolationLevel.ReadCommitted);
			DataTable dtLogin = null;
			try
			{
				foreach (var Obj in Objitem)
				{

					CommonSPHel comhel = new CommonSPHel();
					cmd = new OracleCommand();
					cmd.Transaction = transaction;
					cmd.InitialLONGFetchSize = 1000;
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.CommandText = "PR_INS_GSWS_PRRD_JOB_CARD_DTLS";
					cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2, 10).Value = Obj.P_TYPE;
					cmd.Parameters.Add("P_JC_ID", OracleDbType.Varchar2, 500).Value = Obj.P_JC_ID;
					cmd.Parameters.Add("P_LGD_DISTRICT_CODE", OracleDbType.Varchar2, 10).Value = Obj.P_LGD_DISTRICT_CODE;
					cmd.Parameters.Add("P_LGD_MANDAL_CODE", OracleDbType.Varchar2, 50).Value = Obj.P_LGD_MANDAL_CODE;
					cmd.Parameters.Add("P_GP_ID", OracleDbType.Varchar2, 50).Value = Obj.P_GP_ID;
					cmd.Parameters.Add("P_GP_NAME", OracleDbType.Varchar2, 50).Value = Obj.P_GP_NAME;
					cmd.Parameters.Add("P_HB_CODE", OracleDbType.Varchar2, 10).Value = Obj.P_HB_CODE;
					cmd.Parameters.Add("P_HB_NAME", OracleDbType.Varchar2, 10).Value = Obj.P_HB_NAME;
					cmd.Parameters.Add("P_REGD_DATE", OracleDbType.Date, 50).Value = Obj.P_REGD_DATE;
					cmd.Parameters.Add("P_APPLICATION_NUMBER", OracleDbType.Varchar2, 50).Value = Obj.P_APPLICATION_NUMBER;
					cmd.Parameters.Add("P_GP_SECRETARY_NAME", OracleDbType.Varchar2, 10).Value = Obj.P_GP_SECRETARY_NAME;
					cmd.Parameters.Add("P_CASTE", OracleDbType.Varchar2, 50).Value = Obj.P_CASTE;
					cmd.Parameters.Add("P_ADDRESS", OracleDbType.Varchar2, 50).Value = Obj.P_ADDRESS;
					cmd.Parameters.Add("P_ADDRESS_TEL", OracleDbType.Varchar2, 10).Value = Obj.P_ADDRESS_TEL;
					cmd.Parameters.Add("P_RATIONCARD_NO", OracleDbType.Varchar2, 50).Value = Obj.P_RATIONCARD_NO;
					cmd.Parameters.Add("P_BPL_NO", OracleDbType.Varchar2, 50).Value = Obj.P_BPL_NO;
					cmd.Parameters.Add("P_KHATA_NO", OracleDbType.Varchar2, 10).Value = Obj.P_KHATA_NO;
					cmd.Parameters.Add("P_LAND_OWNER", OracleDbType.Varchar2, 50).Value = Obj.P_LAND_OWNER;
					cmd.Parameters.Add("P_LAND_OWNER_ACRES", OracleDbType.Varchar2, 50).Value = Obj.P_LAND_OWNER_ACRES;
					cmd.Parameters.Add("P_ASSIGNED_LAND_BENEFICIARY", OracleDbType.Varchar2, 10).Value = Obj.P_ASSIGNED_LAND_BENEFICIARY;
					cmd.Parameters.Add("P_IAY_BENEFICIARY", OracleDbType.Varchar2, 50).Value = Obj.P_IAY_BENEFICIARY;
					cmd.Parameters.Add("P_MEMBER_NAME", OracleDbType.Varchar2, 50).Value = Obj.P_MEMBER_NAME;
					cmd.Parameters.Add("P_MEMBER_NAME_TEL", OracleDbType.Varchar2, 50).Value = Obj.P_MEMBER_NAME_TEL;
					cmd.Parameters.Add("P_SUR_NAME", OracleDbType.Varchar2, 50).Value = Obj.P_SUR_NAME;
					cmd.Parameters.Add("P_SUR_NAME_TEL", OracleDbType.Varchar2, 50).Value = Obj.P_SUR_NAME_TEL;
					cmd.Parameters.Add("P_FAMILY_HEAD", OracleDbType.Varchar2, 10).Value = Obj.P_FAMILY_HEAD;
					cmd.Parameters.Add("P_RELATION_HOF", OracleDbType.Varchar2, 50).Value = Obj.P_RELATION_HOF;
					cmd.Parameters.Add("P_GENDER", OracleDbType.Varchar2, 50).Value = Obj.P_GENDER;
					cmd.Parameters.Add("P_AGE", OracleDbType.Varchar2, 10).Value = Obj.P_AGE;
					cmd.Parameters.Add("P_HIV", OracleDbType.Varchar2, 50).Value = Obj.P_HIV;
					cmd.Parameters.Add("P_DISABLED", OracleDbType.Varchar2, 50).Value = Obj.P_DISABLED;
					cmd.Parameters.Add("P_SHG_MEMBER", OracleDbType.Varchar2, 10).Value = Obj.P_SHG_MEMBER;
					cmd.Parameters.Add("P_SHG_ID", OracleDbType.Varchar2, 50).Value = Obj.P_SHG_ID;
					cmd.Parameters.Add("P_SHG_NAME", OracleDbType.Varchar2, 50).Value = Obj.P_SHG_NAME;
					cmd.Parameters.Add("P_PERMANENT_JOB_CARD", OracleDbType.Varchar2, 10).Value = Obj.P_PERMANENT_JOB_CARD;
					cmd.Parameters.Add("P_MPHSS_ID", OracleDbType.Varchar2, 50).Value = Obj.P_MPHSS_ID;
					cmd.Parameters.Add("P_PAYING_AGENCY_TYPE", OracleDbType.Varchar2, 50).Value = Obj.P_PAYING_AGENCY_TYPE;
					cmd.Parameters.Add("P_PAYING_AGENCY_NAME", OracleDbType.Varchar2, 100).Value = Obj.P_PAYING_AGENCY_NAME;
					cmd.Parameters.Add("P_BRANCH_NAME", OracleDbType.Varchar2, 50).Value = Obj.P_BRANCH_NAME;
					cmd.Parameters.Add("P_IFSC_CODE", OracleDbType.Varchar2, 50).Value = Obj.P_IFSC_CODE;
					cmd.Parameters.Add("P_BANK_ACC_NO", OracleDbType.Varchar2, 50).Value = Obj.P_BANK_ACC_NO;
					cmd.Parameters.Add("P_BANK_ACC_NAME ", OracleDbType.Varchar2, 50).Value = Obj.P_BANK_ACC_NAME;
					cmd.Parameters.Add("P_VOTER_ID", OracleDbType.Varchar2, 50).Value = Obj.P_VOTER_ID;
					cmd.Parameters.Add("P_UID_NO", OracleDbType.Varchar2, 50).Value = Obj.P_UID_NO;
					cmd.Parameters.Add("P_MOBILE_NO", OracleDbType.Varchar2, 50).Value = Obj.P_MOBILE_NO;
					cmd.Parameters.Add("P_USER_ID", OracleDbType.Varchar2, 20).Value = Obj.P_USER_ID;
					cmd.Parameters.Add("P_SACHIVALAYAM_ID", OracleDbType.Varchar2, 10).Value = Obj.P_SACHIVALAYAM_ID;
					cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2, 10).Value = null;
					cmd.Parameters.Add("P_PENSION_NUMBER", OracleDbType.Varchar2, 30).Value = Obj.P_PENSION_NUMBER;
					cmd.Parameters.Add("P_SMARTCARD_NUMBER", OracleDbType.Varchar2, 30).Value = null;
					cmd.Parameters.Add("P_UID_PATH", OracleDbType.Varchar2, 300).Value = Obj.P_UID_PATH;
					cmd.Parameters.Add("P_PASSBOOK_PATH", OracleDbType.Varchar2, 300).Value = Obj.P_PASSBOOK_PATH;
					cmd.Parameters.Add("P_F1_PATH", OracleDbType.Varchar2, 300).Value = Obj.P_F1_PATH;
					cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
					dtLogin = comhel.GetgswsDataAdapter(cmd);
				}
				transaction.Commit();

				if (dtLogin != null)
				{
					return dtLogin;
				}
				else
				{
					return null;
				}

			}
			catch (Exception ex)
			{
				transaction.Rollback();
				string mappath = HttpContext.Current.Server.MapPath("JobCardExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error CreateJobCard_SP Saving jobcard into db" + ex.Message.ToString()));
				throw ex;

				throw ex;
			}
		}
		#endregion
		#region BindJobcarddetails
		public DataTable GetJobCardDetails(JobCardModel ObjLGD)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "PR_GET_GSWS_PRRD_JOB_CARD_DTLS";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2, 10).Value = ObjLGD.P_TYPE;
				cmd.Parameters.Add("P_JC_ID", OracleDbType.Varchar2, 100).Value = ObjLGD.P_JC_ID;
				cmd.Parameters.Add("P_MANDAL_ID", OracleDbType.Varchar2, 10).Value = ObjLGD.P_LGD_MANDAL_CODE;
				cmd.Parameters.Add("P_GP_ID", OracleDbType.Varchar2, 10).Value = ObjLGD.P_GP_ID;
				cmd.Parameters.Add("P_SACHIVALAYAM_ID", OracleDbType.Varchar2, 50).Value = ObjLGD.P_SACHIVALAYAM_ID;
				cmd.Parameters.Add("P_UID_NO", OracleDbType.Varchar2, 50).Value = ObjLGD.P_UID_NO;
				cmd.Parameters.Add("P_USER_ID", OracleDbType.Varchar2, 10).Value = ObjLGD.P_USER_ID;
				cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2, 10).Value = ObjLGD.P_STATUS;
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtLogin = GetgswsDataAdapter(cmd);

				if (dtLogin != null)
				{
					return dtLogin;
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobCardExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error GetJobCardDetails -getting all jobcard data based on savchivalyamid:" + ex.Message.ToString()));
				throw ex;
			}
		}
		public DataTable GetJobCardSP(JobCardModel ObjLGD)
		{
			try
			{
				CommonSPHel comhel = new CommonSPHel();
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "PR_GET_GSWS_PRRD_JOB_CARD_DTLS";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2, 10).Value = ObjLGD.P_TYPE;
				cmd.Parameters.Add("P_JC_ID", OracleDbType.Varchar2, 100).Value = ObjLGD.P_JC_ID;
				cmd.Parameters.Add("P_MANDAL_ID", OracleDbType.Varchar2, 10).Value = ObjLGD.P_LGD_MANDAL_CODE;
				cmd.Parameters.Add("P_GP_ID", OracleDbType.Varchar2, 10).Value = ObjLGD.P_GP_ID;
				cmd.Parameters.Add("P_SACHIVALAYAM_ID", OracleDbType.Varchar2, 50).Value = ObjLGD.P_SACHIVALAYAM_ID;
				cmd.Parameters.Add("P_UID_NO", OracleDbType.Varchar2, 50).Value = ObjLGD.P_UID_NO;
				cmd.Parameters.Add("P_USER_ID", OracleDbType.Varchar2, 10).Value = ObjLGD.P_USER_ID;
				cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2, 10).Value = ObjLGD.P_STATUS;

				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtLogin = comhel.GetgswsDataAdapter(cmd);
				if (dtLogin != null)
				{
					return dtLogin;
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobCardExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting GetJobCardSP:" + ex.Message.ToString()));
				throw ex;
			}
		}

		public DataTable GetJobCardDetailsbyTransIdandUID(JobCardModel Lobj)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "PR_GET_GSWS_PRRD_JOB_CARD_DTLS";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Int32, 10).Value = 7;
				cmd.Parameters.Add("P_JC_ID", OracleDbType.Varchar2, 500).Value = Lobj.P_JC_ID;
				cmd.Parameters.Add("P_MANDAL_ID", OracleDbType.Varchar2, 50).Value = Lobj.P_LGD_MANDAL_CODE;
				cmd.Parameters.Add("P_GP_ID", OracleDbType.Varchar2, 50).Value = Lobj.P_GP_ID;
				cmd.Parameters.Add("P_SACHIVALAYAM_ID", OracleDbType.Varchar2, 50).Value = Lobj.P_SACHIVALAYAM_ID;
				cmd.Parameters.Add("P_UID_NO", OracleDbType.Varchar2, 50).Value = Lobj.P_UID_NO;
				cmd.Parameters.Add("P_USER_ID", OracleDbType.Varchar2, 50).Value = Lobj.P_USER_ID;
				cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2, 50).Value = Lobj.P_STATUS;
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtLogin = GetgswsDataAdapter(cmd);

				if (dtLogin != null)
				{
					return dtLogin;
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

		public DataTable UpdateStatus(JobCardModel ObjLGD)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "PR_GET_GSWS_PRRD_JOB_CARD_DTLS";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2, 10).Value = ObjLGD.P_TYPE;
				cmd.Parameters.Add("P_JC_ID", OracleDbType.Varchar2, 100).Value = ObjLGD.P_JC_ID;
				cmd.Parameters.Add("P_MANDAL_ID", OracleDbType.Varchar2, 10).Value = ObjLGD.P_LGD_MANDAL_CODE;
				cmd.Parameters.Add("P_GP_ID", OracleDbType.Varchar2, 10).Value = ObjLGD.P_GP_ID;
				cmd.Parameters.Add("P_SACHIVALAYAM_ID", OracleDbType.Varchar2, 50).Value = ObjLGD.P_SACHIVALAYAM_ID;
				cmd.Parameters.Add("P_UID_NO", OracleDbType.Varchar2, 50).Value = ObjLGD.P_UID_NO;
				cmd.Parameters.Add("P_USER_ID", OracleDbType.Varchar2, 10).Value = ObjLGD.P_USER_ID;
				cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2, 10).Value = ObjLGD.P_STATUS;
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtLogin = GetgswsDataAdapter(cmd);

				if (dtLogin != null)
				{
					return dtLogin;
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobCardExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Update Status:" + ex.Message.ToString()));
				throw ex;
			}
		}
		#endregion
	}
}