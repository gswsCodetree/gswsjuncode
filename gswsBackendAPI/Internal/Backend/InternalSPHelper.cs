using gswsBackendAPI.DL.CommonHel;
using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace gswsBackendAPI.Internal.Backend
{
	public class InternalSPHelper : CommonSPHel
	{
		OracleCommand cmd;
		CommonSPHel comhel = new CommonSPHel();


		#region Issues Tracking Report


		public bool commentAdditionHelper(commentAddition obj)
		{

			OracleTransaction transaction;
			OracleCommand cmd = new OracleCommand();

			try
			{

				string error_log = "{ 'IP_ADDRESS' : '" + HttpContext.Current.Request.UserHostAddress + "','DELETTION_TIME' : '" + DateTime.Now.ToString() + "' , 'DATA' : '" + JsonConvert.SerializeObject(obj) + "' }";
				string log_path = HttpContext.Current.Server.MapPath("comment addition");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(log_path, error_log));

				ConnectionHelper _conHe = new ConnectionHelper();
				OracleConnection con = new OracleConnection(_conHe.Congsws);

				if (con.State == ConnectionState.Closed)
					con.Open();

				// Start a local transaction
				transaction = con.BeginTransaction(IsolationLevel.ReadCommitted);

				try
				{
					cmd = con.CreateCommand();

					// Assign transaction object for a pending local transaction
					cmd.Transaction = transaction;

					cmd.CommandText = @"update gsws_report_issues set comp_status=:issue_status where report_id=:report_id";
					cmd.Parameters.Add(":issue_status", OracleDbType.Varchar2).Value = obj.issue_status;
					cmd.Parameters.Add(":report_id", OracleDbType.Varchar2).Value = obj.report_id;
					cmd.CommandType = CommandType.Text;

					int i = cmd.ExecuteNonQuery();

					cmd = con.CreateCommand();
					obj.comment_id = obj.report_id + DateTime.Now.ToString("yyyyMMddhhmmss");
					obj.image_path = GetImagePath(obj.image, "IssuesComment", obj.report_id);

					cmd.CommandText = @"insert into gsws_report_issues_cmts(REPORT_ID,COMMENTS,COMMENT_ID,LOGIN_USER,MSG_FLAG,IMAGE_PATH)values(:REPORT_ID,:COMMENTS,:COMMENT_ID,:LOGIN_USER,:MSG_FLAG,:IMAGE_PATH)";
					cmd.Parameters.Add(":REPORT_ID", OracleDbType.Varchar2).Value = obj.report_id;
					cmd.Parameters.Add(":COMMENTS", OracleDbType.Varchar2, 2000).Value = obj.comment;
					cmd.Parameters.Add(":COMMENT_ID", OracleDbType.Varchar2).Value = obj.comment_id;
					cmd.Parameters.Add(":LOGIN_USER", OracleDbType.Varchar2).Value = obj.username;
					cmd.Parameters.Add(":MSG_FLAG", OracleDbType.Varchar2).Value = obj.message_flag ? 0 : 1;
					cmd.Parameters.Add(":IMAGE_PATH", OracleDbType.Varchar2).Value = obj.image_path;
					cmd.CommandType = CommandType.Text;
					i += cmd.ExecuteNonQuery();

					transaction.Commit();
					cmd.Dispose();

					if (con.State == ConnectionState.Open)
					{
						con.Close();
						con.Dispose();
					}

					if (i > 1)
						return true;
					else
						return false;

				}
				catch (Exception ex)
				{
					transaction.Rollback();
					if (con != null)
					{
						if (con.State == ConnectionState.Open)
						{
							con.Close();
							con.Dispose();
						}
					}
					if (cmd != null)
					{
						cmd.Dispose();
					}
					throw ex;
				}

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}


		public DataTable IssuesTrackingReportProc(issueTrackingModel obj)
		{

			try
			{
				ConnectionHelper _conHe = new ConnectionHelper();
				using (OracleConnection con = new OracleConnection(_conHe.Congsws))
				{
					using (OracleCommand cmd = new OracleCommand())
					{
						cmd.Connection = con;
						cmd.InitialLONGFetchSize = 1000;
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.CommandText = "GSWS_GET_RPT_ISSUES_CMTS_PROC";
						cmd.Parameters.Add("PTYPE", OracleDbType.Varchar2).Value = obj.TYPE;
						cmd.Parameters.Add("PDIST", OracleDbType.Varchar2).Value = obj.DISTRICT_ID;
						cmd.Parameters.Add("PMANDAL", OracleDbType.Varchar2).Value = obj.MANDAL_ID;
						cmd.Parameters.Add("PSEC", OracleDbType.Varchar2).Value = obj.SECRETARIAT_ID;
						cmd.Parameters.Add("PUSER", OracleDbType.Varchar2).Value = obj.USER;
						cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
						OracleDataAdapter adp = new OracleDataAdapter(cmd);
						cmd.CommandTimeout = 0;
						DataTable dt = new DataTable();
						adp.Fill(dt);
						if (dt != null && dt.Rows.Count > 0)
						{

							return dt;
						}
						else
						{
							return null;
						}
					}
				}

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("InternalURLExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From LoadDepartments:" + ex.Message.ToString()));
				throw ex;
			}

		}


		public string GetImagePath(string base64, string filePathName, string fileName)
		{
			try
			{

				string directoryPath = "";
				string imagepath = "";
				string path = "http://push161.sps.ap.gov.in/RationCardImages/IssuesImages/" + filePathName;

				try
				{
					directoryPath = System.Web.HttpContext.Current.Server.MapPath(string.Format("~/IssuesImages/") + filePathName);

				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message.ToString());
				}

				if (base64 != "" && base64 != null)
				{
					if (!Directory.Exists(directoryPath))
					{
						Directory.CreateDirectory(directoryPath);
					}
					if (generateImage(directoryPath + "\\" + fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".jpg", base64))
					{
						imagepath = path + "/" + fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".jpg";
					}
					return imagepath;
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

		private bool generateImage(string strImagePath, string strImage)
		{
			try
			{
				byte[] bytes = Convert.FromBase64String(strImage);
				using (MemoryStream ms = new MemoryStream(bytes))
				{
					FileStream fs = new FileStream(strImagePath, FileMode.Create);
					ms.WriteTo(fs);
					ms.Close();
					fs.Close();
					fs.Dispose();
				}

				return true;
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}


		#endregion



		#region Internal URL
		public DataTable LoadDepartments_helper(InternalURL oj)
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
				string mappath = HttpContext.Current.Server.MapPath("InternalURLExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From LoadDepartments:" + ex.Message.ToString()));
				throw ex;
			}

		}

		public DataTable LoadGeographicalData_helper(InternalURL oj)
		{
			try
			{

				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_GET_SERVICE_DETAILS";
				cmd.Parameters.Add("FTYPE", OracleDbType.Varchar2, 12).Value = oj.TYPE;
				cmd.Parameters.Add("FPS_CODE", OracleDbType.Varchar2, 12).Value = oj.DEPARTMENT;
				cmd.Parameters.Add("FGSWS_ID", OracleDbType.Varchar2, 12).Value = "";
				cmd.Parameters.Add("FDIST_CODE", OracleDbType.Varchar2, 12).Value = oj.DISTRICT;
				cmd.Parameters.Add("FMAND_CODE", OracleDbType.Varchar2, 12).Value = oj.MANDAL;
				cmd.Parameters.Add("FVILL_CODE", OracleDbType.Varchar2, 12).Value = oj.PANCHAYAT;
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
				string mappath = HttpContext.Current.Server.MapPath("InternalURLExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From LoadDepartments:" + ex.Message.ToString()));
				throw ex;
			}

		}

		// save Services Url's
		public DataTable SaveServices_data_helper(InternalURL obj)
		{
			try
			{


				var comd = new OracleCommand();

				comd.InitialLONGFetchSize = 1000;
				comd.CommandType = CommandType.StoredProcedure;
				comd.CommandText = "GSWS_IN_URL_MASTER";
				comd.Parameters.Add("P_TYPE", OracleDbType.Varchar2).Value = obj.TYPE;
				comd.Parameters.Add("P_SD_ID", OracleDbType.Varchar2).Value = obj.DEPARTMENT;
				comd.Parameters.Add("P_HOD_ID", OracleDbType.Varchar2).Value = obj.HOD;
				comd.Parameters.Add("P_SCHEME_ID", OracleDbType.Varchar2).Value = obj.SERVICE;
				comd.Parameters.Add("P_TYPE_OF_REQUEST", OracleDbType.Varchar2).Value = obj.REQUESTTYPE;
				comd.Parameters.Add("P_URL_ID", OracleDbType.Varchar2).Value = obj.URL_ID;
				comd.Parameters.Add("P_URL", OracleDbType.Varchar2).Value = obj.URL;
				comd.Parameters.Add("P_URL_DESCRIPTION", OracleDbType.Varchar2).Value = obj.URLDESCRIPTION;
				comd.Parameters.Add("P_ACCESS_LEVEL", OracleDbType.Varchar2).Value = obj.ACCESSLEVEL;
				comd.Parameters.Add("P_DISTRICT_ID", OracleDbType.Varchar2).Value = obj.DISTRICT;
				comd.Parameters.Add("P_MANDAL_ID", OracleDbType.Varchar2).Value = obj.MANDAL;
				comd.Parameters.Add("P_GP_WARD_ID", OracleDbType.Varchar2).Value = obj.PANCHAYAT;
				comd.Parameters.Add("P_USER_NAME", OracleDbType.Varchar2).Value = obj.USERNAME;
				comd.Parameters.Add("P_PASSWORD", OracleDbType.Varchar2).Value = obj.PASSWORD;
				comd.Parameters.Add("P_ENCRYPT_PASSWORD", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(obj.ENCRYPT_PASSWORD) ? null : CryptLib.getHashSha256(obj.ENCRYPT_PASSWORD, 31); ;
				comd.Parameters.Add("P_TYPE_OF_SERVICE", OracleDbType.Varchar2).Value = obj.SERVICETYPE;
				comd.Parameters.Add("P_UR_FLAG", OracleDbType.Varchar2).Value = obj.RUFLAG;//
				comd.Parameters.Add("P_URL_DESC_TEL", OracleDbType.Varchar2).Value = obj.P_URL_DESC_TEL;

				comd.Parameters.Add("DESIGN_R", OracleDbType.Varchar2).Value = obj.RURALDESIGNATION;
				comd.Parameters.Add("DESIGN_U", OracleDbType.Varchar2).Value = obj.URBANDESIGNATION;

				comd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

				DataTable data = GetgswsDataAdapter(comd);
				if (data != null && data.Rows.Count > 0)
					return data;
				else
					return null;
			}
			catch (WebException wex)
			{
				string mappath = HttpContext.Current.Server.MapPath("ExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error Save Services URL data :" + wex.Message.ToString()));
				throw new Exception(wex.Message);
			}
		}

		public String UpdateUserManualURL(UpdateUserManualUrlModel obj)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.CommandText = @"UPDATE gsws_url_master SET USER_MANUAL_URL=:USER_MANUAL_URL WHERE URL_ID=:URL_ID";
				cmd.Parameters.Add(":USER_MANUAL_URL", OracleDbType.Varchar2, 500).Value = obj.USERMAUALID;
				cmd.Parameters.Add(":URL_ID", OracleDbType.Varchar2, 50).Value = obj.URL_ID;
				int k = getgswsExecuteNonQuery(cmd);
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
				string mappath = HttpContext.Current.Server.MapPath("ExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error UpdateUserManualURL :" + ex.Message.ToString()));
				throw ex;
			}
		}

		public String SaveFeedbackReport(FeedBackReport obj)
		{
			try
			{
				//obj.REPORTID = obj.SECRETRIATID + DateTime.Now.ToString("ddMMyyHHmmssfff");
				cmd = new OracleCommand();
				cmd.CommandText = @"INSERT INTO GSWS_REPORT_ISSUES(SD_ID,HOD_ID,URL_ID,USER_NAME,REMARKS,IMAGE1_URL,IMAGE2_URL,IMAGE3_URL,DISTRICT_CODE,MANDAL_CODE,SEC_CODE,REPORT_ID,SUBJECT,SUBSUBJECT,SOURCE) values(:SD_ID,:HOD_ID,:URL_ID,:USER_NAME,:REMARKS,:IMAGE1_URL,:IMAGE2_URL,:IMAGE3_URL,:DISTRICT_CODE,:MANDAL_CODE,:SEC_CODE,:REPORT_ID,:SUBJECT,:SUBSUBJECT,:SOURCE)";
				cmd.Parameters.Add(":SD_ID", OracleDbType.Varchar2, 50).Value = obj.DEPTID;
				cmd.Parameters.Add(":HOD_ID", OracleDbType.Varchar2, 50).Value = obj.HODID;
				cmd.Parameters.Add(":URL_ID", OracleDbType.Varchar2, 500).Value = obj.URL_ID;
				cmd.Parameters.Add(":USER_NAME", OracleDbType.Varchar2, 50).Value = obj.USER;
				cmd.Parameters.Add(":REMARKS", OracleDbType.Varchar2, 500).Value = obj.REMARKS;
				cmd.Parameters.Add(":IMAGE1_URL", OracleDbType.Varchar2, 500).Value = obj.IMAGE1URL;
				cmd.Parameters.Add(":IMAGE2_URL", OracleDbType.Varchar2, 500).Value = obj.IMAGE2URL;
				cmd.Parameters.Add(":IMAGE3_URL", OracleDbType.Varchar2, 500).Value = obj.IMAGE3URL;
				cmd.Parameters.Add(":DISTRICT_CODE", OracleDbType.Varchar2, 50).Value = obj.DISTRICTID;
				cmd.Parameters.Add(":MANDAL_CODE", OracleDbType.Varchar2, 50).Value = obj.MANDALID;
				cmd.Parameters.Add(":SEC_CODE", OracleDbType.Varchar2, 50).Value = obj.SECRETRIATID;
				cmd.Parameters.Add(":REPORT_ID", OracleDbType.Varchar2, 50).Value = obj.REPORTID;
				cmd.Parameters.Add(":SUBJECT", OracleDbType.Varchar2, 50).Value = obj.SUBJECT;
				cmd.Parameters.Add(":SUBSUBJECT", OracleDbType.Varchar2, 50).Value = obj.SUBSUBJECT;
				cmd.Parameters.Add(":SOURCE", OracleDbType.Varchar2, 50).Value = obj.SOURCE;
				int k = getgswsExecuteNonQuery(cmd);
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
				string mappath = HttpContext.Current.Server.MapPath("ExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error UpdateUserManualURL :" + ex.Message.ToString()));
				throw ex;
			}
		}

		public String SaveReOpenTicket_Helper_SP(FeedBackReport obj)
		{
			try
			{
				//obj.REPORTID = obj.SECRETRIATID + DateTime.Now.ToString("ddMMyyHHmmssfff");
				cmd = new OracleCommand();
				cmd.CommandText = @"update GSWS_REPORT_ISSUES set REOPEN_DATE=sysdate, REOPEN_BY = :USER_NAME,REOPEN_REASONS=:REMARKS,comp_status=:comp_status where REPORT_ID= :REPORT_ID";
				cmd.Parameters.Add(":USER_NAME", OracleDbType.Varchar2, 50).Value = obj.USER;
				cmd.Parameters.Add(":REMARKS", OracleDbType.Varchar2, 500).Value = obj.REMARKS;
				cmd.Parameters.Add(":comp_status", OracleDbType.Varchar2, 50).Value = obj.SOURCE;
				cmd.Parameters.Add(":REPORT_ID", OracleDbType.Varchar2, 50).Value = obj.REPORTID;
				int k = getProdgswsExecuteNonQuery(cmd);
				if (k > 0)
					return "Success";
				else
					return "Failure";
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("SaveReOpenTicketExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error GSWS_REPORT_ISSUES :" + ex.Message.ToString()));
				throw ex;
			}
		}

		public String SaveSecretriatData(SecretraintModel obj)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.CommandText = @"insert into gsws_secretariat_master (DISTRICT_CODE, DISTRICT_NAME, MANDAL_CODE, MANDAL_NAME, GP_CODE, GP_NAME, SECRETARIAT_CODE, SECRETARIAT_NAME, RURAL_URBAN) Values(:DISTRICT_CODE, :DISTRICT_NAME, :MANDAL_CODE, :MANDAL_NAME, :GP_CODE, :GP_NAME, :SECRETARIAT_CODE, :SECRETARIAT_NAME, :RURAL_URBAN)";
				cmd.Parameters.Add(":DISTRICT_CODE", OracleDbType.Varchar2, 5).Value = obj.DISTRICTID;
				cmd.Parameters.Add(":DISTRICT_NAME", OracleDbType.Varchar2, 50).Value = obj.DISTRICTNAME;
				cmd.Parameters.Add(":MANDAL_CODE", OracleDbType.Varchar2, 5).Value = obj.MANDALID;
				cmd.Parameters.Add(":MANDAL_NAME", OracleDbType.Varchar2, 100).Value = obj.MANDALNAME;
				cmd.Parameters.Add(":GP_CODE", OracleDbType.Varchar2, 10).Value = obj.PANCHAYATID;
				cmd.Parameters.Add(":GP_NAME", OracleDbType.Varchar2, 200).Value = obj.PANCHAYATNAME;
				cmd.Parameters.Add(":SECRETARIAT_CODE", OracleDbType.Varchar2, 10).Value = obj.SECRETRIATID.Trim();
				cmd.Parameters.Add(":SECRETARIAT_NAME", OracleDbType.Varchar2, 200).Value = obj.SECRETRIATNAME.Trim();
				cmd.Parameters.Add(":RURAL_URBAN", OracleDbType.Varchar2, 500).Value = obj.RUFLAG;
				
				int k = getgswsExecuteNonQuery(cmd);
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
				string mappath = HttpContext.Current.Server.MapPath("ExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error UpdateUserManualURL :" + ex.Message.ToString()));
				throw ex;
			}
		}

		// Get UniqueCode
		public Int32 GetUniqueCode(InternalURL obj)

		{
			Int32 Cnt = 0;
			try
			{
				var comd = new OracleCommand();
				comd.InitialLONGFetchSize = 1000;
				comd.CommandType = CommandType.Text;
				comd.CommandText = "SELECT COUNT(*) AS COUNT FROM GSWS_URL_MASTER WHERE SD_ID=:SD_ID AND HOD_ID=:HOD_ID AND SCHEME_ID=:SCHEME_ID";
				comd.Parameters.Add(":SD_ID", OracleDbType.Varchar2).Value = obj.DEPARTMENT;
				comd.Parameters.Add(":HOD_ID", OracleDbType.Varchar2).Value = obj.HOD;
				comd.Parameters.Add(":SCHEME_ID", OracleDbType.Varchar2).Value = obj.SERVICE;
				DataTable data = GetgswsDataAdapter(comd);
				if (data != null && data.Rows.Count > 0)
					Cnt = Convert.ToInt32(data.Rows[0]["COUNT"]);
			}
			catch (WebException wex)
			{
				string mappath = HttpContext.Current.Server.MapPath("ExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error Get Unique code based on url :" + wex.Message.ToString()));

			}

			return Cnt;
		}

		public String UpdateURLHelper(UpdateUserManualUrlModel obj)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.CommandText = @"UPDATE gsws_url_master SET URL=:NEW_URL,URL_DESCRIPTION=:ENGLISH_DESCRIPTION,URL_DESC_TEL=:TELUGU_DESCRIPTION WHERE URL_ID=:URL_ID";
				cmd.Parameters.Add(":NEW_URL", OracleDbType.Varchar2, 500).Value = obj.NEWURL;
				cmd.Parameters.Add(":ENGLISH_DESCRIPTION", OracleDbType.Varchar2, 500).Value = obj.ENGLISHDESCRIPTION;
				cmd.Parameters.Add(":TELUGU_DESCRIPTION", OracleDbType.Varchar2, 500).Value = obj.TELUGUDESCRIPTION;
				cmd.Parameters.Add(":URL_ID", OracleDbType.Varchar2, 50).Value = obj.URL_ID;
				int k = getgswsExecuteNonQuery(cmd);
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
				string mappath = HttpContext.Current.Server.MapPath("ExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error UpdateURL :" + ex.Message.ToString()));
				throw ex;
			}
		}
		#endregion


		#region Issue Closing
		public DataTable GetCategories(IssueType obj)
		{
			try
			{
				ConnectionHelper _conHe = new ConnectionHelper();
				using (OracleConnection con = new OracleConnection(_conHe.Congsws))
				{
					using (OracleCommand cmd = new OracleCommand())
					{
						cmd.Connection = con;
						cmd.InitialLONGFetchSize = 1000;
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.CommandText = "GSWS_SP_S_H_ISSUES";
						cmd.Parameters.Add("PTYPE", OracleDbType.Varchar2).Value = obj.Type; //2-Sowftware,1-Hardware,3-Software Details,4-Hardware details
						cmd.Parameters.Add("PDID", OracleDbType.Varchar2).Value = obj.DID;
						cmd.Parameters.Add("PDEPARTMENT_ID", OracleDbType.Varchar2).Value = obj.CategoryID;

						cmd.Parameters.Add("PUPDATED_BY", OracleDbType.Varchar2).Value = obj.UpdatedBy;
						cmd.Parameters.Add("PREASON", OracleDbType.Varchar2).Value = obj.Reason;
						cmd.Parameters.Add("PUNIQUE_ID", OracleDbType.Varchar2).Value = obj.UniqueID;

						cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
						OracleDataAdapter adp = new OracleDataAdapter(cmd);
						cmd.CommandTimeout = 0;
						DataTable dt = new DataTable();
						adp.Fill(dt);
						if (dt != null && dt.Rows.Count > 0)
						{
							return dt;
						}
						else
						{
							return null;
						}
					}
				}

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("InternalURLExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From LoadDepartments:" + ex.Message.ToString()));
				throw ex;
			}

		}


		//Hardware Issue Closing
		public DataTable GetHWCategoriesData_SPHel(IssueType obj)
		{
			try
			{
				ConnectionHelper _conHe = new ConnectionHelper();
				using (OracleConnection con = new OracleConnection(_conHe.Congsws))
				{
					using (OracleCommand cmd = new OracleCommand())
					{
						cmd.Connection = con;
						cmd.InitialLONGFetchSize = 1000;
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.CommandText = "GSWS_HW_ISSUES_REPORT";

						cmd.Parameters.Add("prole", OracleDbType.Varchar2).Value = obj.ROLE;
						cmd.Parameters.Add("passet_id", OracleDbType.Varchar2).Value = (obj.ASSET == "null" ? "" : obj.ASSET);
						cmd.Parameters.Add("pdid", OracleDbType.Varchar2).Value = obj.DID;


						cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
						OracleDataAdapter adp = new OracleDataAdapter(cmd);
						cmd.CommandTimeout = 0;
						DataTable dt = new DataTable();
						adp.Fill(dt);
						if (dt != null && dt.Rows.Count > 0)
						{
							return dt;
						}
						else
						{
							return null;
						}
					}
				}

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("HWIssueclosingExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From LoadDepartments:" + ex.Message.ToString()));
				throw ex;
			}

		}

		//SWHW closed Issues Data
		public DataTable GetHWSWResolvedIssues_SPHel(IssueType obj)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_HWSW_CLOSED_ISSUES";
				cmd.Parameters.Add("p_type", OracleDbType.Varchar2).Value = obj.Type;
				cmd.Parameters.Add("p_user_id", OracleDbType.Varchar2).Value = obj.UpdatedBy;
				cmd.Parameters.Add("p_sec_code", OracleDbType.Varchar2).Value = obj.SECRETARIAT;
				cmd.Parameters.Add("p_status_type", OracleDbType.Varchar2).Value = obj.ACTIVE_STATUS;


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
				string mappath = HttpContext.Current.Server.MapPath("HWIssueclosingExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From LoadDepartments:" + ex.Message.ToString()));
				throw ex;
			}

		}
		#endregion
	}
}