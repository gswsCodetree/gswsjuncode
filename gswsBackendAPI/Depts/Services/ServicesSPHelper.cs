using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;

namespace gswsBackendAPI.Depts.Services
{
    public class ServicesSPHelper : CommonSPHel
    {
		OracleCommand cmd;

		public DataTable GetNPCIStatus_data_helper(AppStatus obj)
        {
            try
            {
                var comd = new OracleCommand();

                comd.InitialLONGFetchSize = 1000;
                comd.CommandType = CommandType.StoredProcedure;
                comd.CommandText = "unemployment.gsws_check_status_npci_data";
                comd.Parameters.Add("ftype", OracleDbType.Varchar2).Value = obj.ftype;
                comd.Parameters.Add("fdpart_id", OracleDbType.Varchar2).Value = obj.fdpart_id;
                comd.Parameters.Add("fadhar_no", OracleDbType.Varchar2).Value = obj.fadhar_no;
                comd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                DataTable data = GetspsDataAdapter(comd);
                if (data != null && data.Rows.Count > 0)
                    return data;
                else
                    return null;
            }
            catch (WebException wex)
            {
                string mappath = HttpContext.Current.Server.MapPath("NPCIExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error NPCI App Status API:" + wex.Message.ToString()));
                throw wex;
            }
        }

		#region Textile
		public DataTable GetTextileData_helper(AppStatus obj)
		{
			try
			{
				var comd = new OracleCommand();

				comd.InitialLONGFetchSize = 1000;
				comd.CommandType = CommandType.StoredProcedure;
				comd.CommandText = "COTTEN_PEOPLES_PSS_STATUS";
				comd.Parameters.Add("ftype", OracleDbType.Varchar2).Value = obj.ftype;
				comd.Parameters.Add("fuid_num", OracleDbType.Varchar2).Value = obj.fadhar_no;
				comd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

				DataTable data = GetspsDataAdapter(comd);
				if (data != null && data.Rows.Count > 0)
					return data;
				else
					return null;
			}
			catch (WebException wex)
			{
				string mappath = HttpContext.Current.Server.MapPath("TextileGetDataExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error Textile Get Data API:" + wex.Message.ToString()));
				throw wex;
			}
		}

		//Save Textile Data
		public string SaveTextileData_helper(TextileModel obj)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.CommandText = @"insert into chenetha_persons_data(CITIZEN_NAME,FATHER_HUSBAND_NAME,AGE,DOOR_NO,VILLAGE_WARD_ID,MANDAL_ID,DISTRICT_ID,PINCODE,MOBILE_NUMBER,CH_UNIQUE_ID,UID_NUM,RATION_CARD_NO,PSS_STATUS,CASTE,TOTAL_FAMILY_MEMBERS,LOOM_STATUS,PIT_LOOM_OR_FRAME_LOOM,OWNED_OR_TEMPORARY_LOOM,TOTAL_LOOMS,HOUSE_TYPE,ANNUAL_INCOME,BANK_NAME,BANK_BRANCH,ACCOUNT_NO,IFSC_CODE,INSERTED_BY) values (:CITIZEN_NAME,:FATHER_HUSBAND_NAME,:AGE,:DOOR_NO,:VILLAGE_WARD_ID,:MANDAL_ID,:DISTRICT_ID,:PINCODE,:MOBILE_NUMBER,:CH_UNIQUE_ID,:UID_NUM,:RATION_CARD_NO,:PSS_STATUS,:CASTE,:TOTAL_FAMILY_MEMBERS,:LOOM_STATUS,:PIT_LOOM_OR_FRAME_LOOM,:OWNED_OR_TEMPORARY_LOOM,:TOTAL_LOOMS,:HOUSE_TYPE,:ANNUAL_INCOME,:BANK_NAME,:BANK_BRANCH,:ACCOUNT_NO,:IFSC_CODE,:INSERTED_BY)";
				cmd.Parameters.Add(":CITIZEN_NAME", OracleDbType.Varchar2, 100).Value = obj.NAME;
				cmd.Parameters.Add(":FATHER_HUSBAND_NAME", OracleDbType.Varchar2, 100).Value = obj.FATHERNAME;
				cmd.Parameters.Add(":AGE", OracleDbType.Varchar2, 5).Value = obj.AGE;
				cmd.Parameters.Add(":DOOR_NO", OracleDbType.Varchar2, 50).Value = obj.DOORNO;
				cmd.Parameters.Add(":VILLAGE_WARD_ID", OracleDbType.Varchar2, 20).Value = obj.VILLAGEPANCHAYATH;
				cmd.Parameters.Add(":MANDAL_ID", OracleDbType.Varchar2, 20).Value = obj.MANDAL;
				cmd.Parameters.Add(":DISTRICT_ID", OracleDbType.Varchar2, 20).Value = obj.DISTRICT;
				cmd.Parameters.Add(":PINCODE", OracleDbType.Varchar2, 10).Value = obj.PINCODE;
				cmd.Parameters.Add(":MOBILE_NUMBER", OracleDbType.Varchar2, 10).Value = obj.MOBILE;
				cmd.Parameters.Add(":CH_UNIQUE_ID", OracleDbType.Varchar2, 50).Value = obj.IDCARD;
				cmd.Parameters.Add(":UID_NUM", OracleDbType.Varchar2, 12).Value = obj.AADHAAR;
				cmd.Parameters.Add(":RATION_CARD_NO", OracleDbType.Varchar2, 50).Value = obj.RATION;
				cmd.Parameters.Add(":PSS_STATUS", OracleDbType.Varchar2, 10).Value = obj.PSSREG;
				cmd.Parameters.Add(":CASTE", OracleDbType.Varchar2, 50).Value = obj.CASTE;
				cmd.Parameters.Add(":TOTAL_FAMILY_MEMBERS", OracleDbType.Varchar2, 10).Value = obj.FAMILYCOUNT;
				cmd.Parameters.Add(":LOOM_STATUS", OracleDbType.Varchar2, 10).Value = obj.DOINGLOOM;
				cmd.Parameters.Add(":PIT_LOOM_OR_FRAME_LOOM", OracleDbType.Varchar2, 100).Value = obj.LOOMTYPE;
				cmd.Parameters.Add(":OWNED_OR_TEMPORARY_LOOM", OracleDbType.Varchar2, 100).Value = obj.LOOMOWNERSHIP;
				cmd.Parameters.Add(":TOTAL_LOOMS", OracleDbType.Varchar2, 10).Value = obj.LOOMCOUNT;
				cmd.Parameters.Add(":HOUSE_TYPE", OracleDbType.Varchar2, 100).Value = obj.HOUSETYPE;
				cmd.Parameters.Add(":ANNUAL_INCOME", OracleDbType.Varchar2, 100).Value = obj.ANNINCOME;
				cmd.Parameters.Add(":BANK_NAME", OracleDbType.Varchar2, 100).Value = obj.BANK;
				cmd.Parameters.Add(":BANK_BRANCH", OracleDbType.Varchar2, 100).Value = obj.BRANCH;
				cmd.Parameters.Add(":ACCOUNT_NO", OracleDbType.Varchar2, 100).Value = obj.ACCOUNT;
				cmd.Parameters.Add(":IFSC_CODE", OracleDbType.Varchar2, 100).Value = obj.IFSC;
				cmd.Parameters.Add(":INSERTED_BY", OracleDbType.Varchar2, 100).Value = obj.SUBMITTEDBY;

				int k = getspsExecuteNonQuery(cmd);
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
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error Save Textile Data :" + ex.Message.ToString()));
				throw ex;
			}
		}
		#endregion

		#region Asset Tracking


		public DataTable seccdata_helper(Assetmodel oj)
		{
			try
			{

				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_ASSETS_DETAILS_PROC";
				cmd.Parameters.Add("ftype", OracleDbType.Varchar2, 12).Value = oj.TYPE;
				cmd.Parameters.Add("fdistrict_id", OracleDbType.Varchar2, 12).Value = oj.DISTRICT;
				cmd.Parameters.Add("fmandal_id", OracleDbType.Varchar2, 12).Value = oj.MANDAL;
				cmd.Parameters.Add("fsecretariat_id", OracleDbType.Varchar2, 12).Value = oj.SECRATARIAT;
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
				string mappath = HttpContext.Current.Server.MapPath("SeccDataLoadingExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From LoadDistricts:" + ex.Message.ToString()));
				throw ex;
			}

		}


		public DataTable LoadDepartments_helper(Assetmodel oj)
		{
			try
			{

				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "gsws_get_secretariat_master";
				cmd.Parameters.Add("ftype", OracleDbType.Varchar2, 12).Value = oj.TYPE;
				cmd.Parameters.Add("fdistrict_id", OracleDbType.Varchar2, 12).Value = oj.DISTRICT;
				cmd.Parameters.Add("fmandal_id", OracleDbType.Varchar2, 12).Value = oj.MANDAL;
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
				string mappath = HttpContext.Current.Server.MapPath("DataLoadingExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From LoadDistricts:" + ex.Message.ToString()));
				throw ex;
			}

		}

		//Save Data
		public dynamic SaveSystemDataSpHelper(AssetTracking obj)
		{
			try
			{
				int k = 0;
				foreach (insarray array in obj.DATAARRAY)
				{
					var comd = new OracleCommand();
					comd.CommandText = @"insert into gsws_assets_masters (DISTRICT,MANDAL,SECRATARIAT,CPUSERIALNO,CPUCONN,CPUWORKING,CPUCONREMARKS,CPUWORREMARKS,MONITORSERIALNO,MONITORCONN,MONITORWORKING,MONITORCONREMARKS,MONITORWORREMARKS,KEYBOARDSERIALNO,KEYBOARDCONN,KEYBOARDWORKING,KEYBOARDCONREMARKS,KEYBOARDWORREMARKS,MOUSESERIALNO,MOUSECONN,MOUSEWORKING,MOUSECONREMARKS,MOUSEWORREMARKS,INVERTORSERIALNO,INVERTORCONN,INVERTORWORKING,INVERTORCONREMARKS,INVERTORWORREMARKS,BATTERIESSERIALNO,BATTERIESCONN,BATTERIESWORKING,BATTERIESCONREMARKS,BATTERIESWORREMARKS,MACADDRESS,MODELNO,BATCHNO,SYS_NO,UPDATED_BY,PRINTERSERIALNO,PRINTERCONN,PRINTERWORKING,PRINTERCONREMARKS,PRINTERWORREMARKS,LAMINATORSERIALNO,LAMINATORCONN,LAMINATORWORKING,LAMINATORCONREMARKS,LAMINATORWORREMARKS,BIOMETRICSERIALNO,BIOMETRICCONN,BIOMETRICWORKING,BIOMETRICCONREMARKS,BIOMETRICWORREMARKS) 
																values(:DISTRICT,:MANDAL,:SECRATARIAT,:CPUSERIALNO,:CPUCONN,:CPUWORKING,:CPUCONREMARKS,:CPUWORREMARKS,:MONITORSERIALNO,:MONITORCONN,:MONITORWORKING,:MONITORCONREMARKS,:MONITORWORREMARKS,:KEYBOARDSERIALNO,:KEYBOARDCONN,:KEYBOARDWORKING,:KEYBOARDCONREMARKS,:KEYBOARDWORREMARKS,:MOUSESERIALNO,:MOUSECONN,:MOUSEWORKING,:MOUSECONREMARKS,:MOUSEWORREMARKS,:INVERTORSERIALNO,:INVERTORCONN,:INVERTORWORKING,:INVERTORCONREMARKS,:INVERTORWORREMARKS,:BATTERIESSERIALNO,:BATTERIESCONN,:BATTERIESWORKING,:BATTERIESCONREMARKS,:BATTERIESWORREMARKS,:MACADDRESS,:MODELNO,:BATCHNO,:SYS_NO,:UPDATED_BY,:PRINTERSERIALNO,:PRINTERCONN,:PRINTERWORKING,:PRINTERCONREMARKS,:PRINTERWORREMARKS,:LAMINATORSERIALNO,:LAMINATORCONN,:LAMINATORWORKING,:LAMINATORCONREMARKS,:LAMINATORWORREMARKS,:BIOMETRICSERIALNO,:BIOMETRICCONN,:BIOMETRICWORKING,:BIOMETRICCONREMARKS,:BIOMETRICWORREMARKS)";


					comd.Parameters.Add(":DISTRICT", OracleDbType.Varchar2, 20).Value = array.DISTRICT;
					comd.Parameters.Add(":MANDAL", OracleDbType.Varchar2, 20).Value = array.MANDAL;
					comd.Parameters.Add(":SECRATARIAT", OracleDbType.Varchar2, 20).Value = array.SECRATARIAT;

					comd.Parameters.Add(":CPUSERIALNO", OracleDbType.Varchar2, 50).Value = array.CPUSERIALNO;
					comd.Parameters.Add(":CPUCONN", OracleDbType.Varchar2, 20).Value = array.CPUCONN;
					comd.Parameters.Add(":CPUWORKING", OracleDbType.Varchar2, 20).Value = array.CPUWORKING;
					comd.Parameters.Add(":CPUCONREMARKS", OracleDbType.Varchar2, 200).Value = array.CPUCONREMARKS;
					comd.Parameters.Add(":CPUWORREMARKS", OracleDbType.Varchar2, 200).Value = array.CPUWORREMARKS;

					comd.Parameters.Add(":MONITORSERIALNO", OracleDbType.Varchar2, 20).Value = array.MONITORSERIALNO;
					comd.Parameters.Add(":MONITORCONN", OracleDbType.Varchar2, 20).Value = array.MONITORCONN;
					comd.Parameters.Add(":MONITORWORKING", OracleDbType.Varchar2, 20).Value = array.MONITORWORKING;
					comd.Parameters.Add(":MONITORCONREMARKS", OracleDbType.Varchar2, 200).Value = array.MONITORCONREMARKS;
					comd.Parameters.Add(":MONITORWORREMARKS", OracleDbType.Varchar2, 200).Value = array.MONITORWORREMARKS;

					comd.Parameters.Add(":KEYBOARDSERIALNO", OracleDbType.Varchar2, 20).Value = array.KEYBOARDSERIALNO;
					comd.Parameters.Add(":KEYBOARDCONN", OracleDbType.Varchar2, 20).Value = array.KEYBOARDCONN;
					comd.Parameters.Add(":KEYBOARDWORKING", OracleDbType.Varchar2, 20).Value = array.KEYBOARDWORKING;
					comd.Parameters.Add(":KEYBOARDCONREMARKS", OracleDbType.Varchar2, 200).Value = array.KEYBOARDCONREMARKS;
					comd.Parameters.Add(":KEYBOARDWORREMARKS", OracleDbType.Varchar2, 200).Value = array.KEYBOARDWORREMARKS;

					comd.Parameters.Add(":MOUSESERIALNO", OracleDbType.Varchar2, 20).Value = array.MOUSESERIALNO;
					comd.Parameters.Add(":MOUSECONN", OracleDbType.Varchar2, 20).Value = array.MOUSECONN;
					comd.Parameters.Add(":MOUSEWORKING", OracleDbType.Varchar2, 20).Value = array.MOUSEWORKING;
					comd.Parameters.Add(":MOUSECONREMARKS", OracleDbType.Varchar2, 200).Value = array.MOUSECONREMARKS;
					comd.Parameters.Add(":MOUSEWORREMARKS", OracleDbType.Varchar2, 200).Value = array.MOUSEWORREMARKS;

					comd.Parameters.Add(":INVERTORSERIALNO", OracleDbType.Varchar2, 20).Value = array.INVERTORSERIALNO;
					comd.Parameters.Add(":INVERTORCONN", OracleDbType.Varchar2, 20).Value = array.INVERTORCONN;
					comd.Parameters.Add(":INVERTORWORKING", OracleDbType.Varchar2, 20).Value = array.INVERTORWORKING;
					comd.Parameters.Add(":INVERTORCONREMARKS", OracleDbType.Varchar2, 200).Value = array.INVERTORCONREMARKS;
					comd.Parameters.Add(":INVERTORWORREMARKS", OracleDbType.Varchar2, 200).Value = array.INVERTORWORREMARKS;

					comd.Parameters.Add(":BATTERIESSERIALNO", OracleDbType.Varchar2, 20).Value = array.BATTERIESSERIALNO;
					comd.Parameters.Add(":BATTERIESCONN", OracleDbType.Varchar2, 20).Value = array.BATTERIESCONN;
					comd.Parameters.Add(":BATTERIESWORKING", OracleDbType.Varchar2, 20).Value = array.BATTERIESWORKING;
					comd.Parameters.Add(":BATTERIESCONREMARKS", OracleDbType.Varchar2, 200).Value = array.BATTERIESCONREMARKS;
					comd.Parameters.Add(":BATTERIESWORREMARKS", OracleDbType.Varchar2, 200).Value = array.BATTERIESWORREMARKS;


					comd.Parameters.Add(":MACADDRESS", OracleDbType.Varchar2, 100).Value = array.MACADDRESS;
					comd.Parameters.Add(":MODELNO", OracleDbType.Varchar2, 50).Value = array.MODELNO;
					comd.Parameters.Add(":BATCHNO", OracleDbType.Varchar2, 20).Value = array.BATCHNO;
					comd.Parameters.Add(":SYS_NO", OracleDbType.Varchar2, 50).Value = array.SYSNO;
					comd.Parameters.Add(":UPDATED_BY", OracleDbType.Varchar2, 50).Value = array.USERNAME;

					comd.Parameters.Add(":PRINTERSERIALNO", OracleDbType.Varchar2, 20).Value = array.PRINTERSERIALNO;
					comd.Parameters.Add(":PRINTERCONN", OracleDbType.Varchar2, 20).Value = array.PRINTERCONN;
					comd.Parameters.Add(":PRINTERWORKING", OracleDbType.Varchar2, 20).Value = array.PRINTERWORKING;
					comd.Parameters.Add(":PRINTERCONREMARKS", OracleDbType.Varchar2, 200).Value = array.PRINTERCONREMARKS;
					comd.Parameters.Add(":PRINTERWORREMARKS", OracleDbType.Varchar2, 200).Value = array.PRINTERWORREMARKS;

					comd.Parameters.Add(":LAMINATORSERIALNO", OracleDbType.Varchar2, 20).Value = array.LAMINATORSERIALNO;
					comd.Parameters.Add(":LAMINATORCONN", OracleDbType.Varchar2, 20).Value = array.LAMINATORCONN;
					comd.Parameters.Add(":LAMINATORWORKING", OracleDbType.Varchar2, 20).Value = array.LAMINATORWORKING;
					comd.Parameters.Add(":LAMINATORCONREMARKS", OracleDbType.Varchar2, 200).Value = array.LAMINATORCONREMARKS;
					comd.Parameters.Add(":LAMINATORWORREMARKS", OracleDbType.Varchar2, 200).Value = array.LAMINATORWORREMARKS;


					comd.Parameters.Add(":BIOMETRICSERIALNO", OracleDbType.Varchar2, 20).Value = array.BIOMETRICSERIALNO;
					comd.Parameters.Add(":BIOMETRICCONN", OracleDbType.Varchar2, 20).Value = array.BIOMETRICCONN;
					comd.Parameters.Add(":BIOMETRICWORKING", OracleDbType.Varchar2, 20).Value = array.BIOMETRICWORKING;
					comd.Parameters.Add(":BIOMETRICCONREMARKS", OracleDbType.Varchar2, 200).Value = array.BIOMETRICCONREMARKS;
					comd.Parameters.Add(":BIOMETRICWORREMARKS", OracleDbType.Varchar2, 200).Value = array.BIOMETRICWORREMARKS;


					k = getgswsExecuteNonQuery(comd);
				}

				if (k > 0)
				{
					return "Success";
				}
				else
				{
					return "Failure";
				}
			}
			catch (WebException wex)
			{
				string mappath = HttpContext.Current.Server.MapPath("ExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error Save Index Page data :" + wex.Message.ToString()));
				throw new Exception(wex.Message);
			}
		}

		//Reports
		public DataTable LoadRptDistrictSPData(Assetmodel oj)
		{
			try
			{

				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_ASSETS_CNT_REPORT";
				cmd.Parameters.Add("ftype", OracleDbType.Varchar2, 12).Value = oj.TYPE;
				cmd.Parameters.Add("fdist_id", OracleDbType.Varchar2, 12).Value = oj.DISTRICT;
				cmd.Parameters.Add("fmandal_id", OracleDbType.Varchar2, 12).Value = oj.MANDAL;
				cmd.Parameters.Add("fsec_id", OracleDbType.Varchar2, 12).Value = oj.SECRATARIAT;
				cmd.Parameters.Add("p_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
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
				string mappath = HttpContext.Current.Server.MapPath("SeccDataLoadingExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From LoadDistricts:" + ex.Message.ToString()));
				throw ex;
			}

		}

		//Hardware Issue Compnent Loading
		public DataTable Loadhwcomponent_helper(Assetmodel oj)
		{
			try
			{

				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_HARDWARE_ISSUES";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2, 12).Value = oj.TYPE;
				cmd.Parameters.Add("P_COMPONENT_ID", OracleDbType.Varchar2, 12).Value = oj.COMPONENTID;
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
				string mappath = HttpContext.Current.Server.MapPath("DataLoadingExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From LoadDistricts:" + ex.Message.ToString()));
				throw ex;
			}

		}

		//Save Hardware Issue
		public dynamic SaveHardwareIssueSpHelper(Assetmodel oj)
		{
			try
			{
				//int k = 0;
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_IN_HARDWARE_ISSUES";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2, 12).Value = "1";
				cmd.Parameters.Add("P_DISTRICT", OracleDbType.Varchar2, 12).Value = oj.DISTRICT;
				cmd.Parameters.Add("P_MANDAL", OracleDbType.Varchar2, 12).Value = oj.MANDAL;
				cmd.Parameters.Add("P_SECRETARIAT", OracleDbType.Varchar2, 12).Value = oj.SECRATARIAT;
				cmd.Parameters.Add("P_COMPONENT_NAME", OracleDbType.Varchar2, 200).Value = oj.HWCOMPONENT;
				cmd.Parameters.Add("P_ISSUE_NAME", OracleDbType.Varchar2, 200).Value = oj.HWISSUE;
				cmd.Parameters.Add("P_REMARKS", OracleDbType.Varchar2, 200).Value = oj.REMARKS;
				cmd.Parameters.Add("P_IMAGE_URL", OracleDbType.Varchar2, 500).Value = oj.IMAGEURL;
				cmd.Parameters.Add("P_IP_ADDRESS", OracleDbType.Varchar2, 150).Value = GetIPAddress();
				cmd.Parameters.Add("P_ENTRY_BY", OracleDbType.Varchar2, 120).Value = oj.USERNAME;
				cmd.Parameters.Add("P_SOURCE", OracleDbType.Varchar2, 12).Value = oj.SOURCE;
				cmd.Parameters.Add("p_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

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
				string mappath = HttpContext.Current.Server.MapPath("HardwareIssuesExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From Submit Data:" + ex.Message.ToString()));
				throw ex;
			}
		}

		#endregion

		#region Rajakas
		public dynamic GetData(string url)
		{
			var response = String.Empty;
			try
			{
				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				var req = (HttpWebRequest)WebRequest.Create(url);
				req.ContentType = "application/json; charset=utf-8";
				req.AllowAutoRedirect = false;
				var resp = req.GetResponse();
				var sr = new StreamReader(resp.GetResponseStream());
				response = sr.ReadToEnd().Trim();

				var data = JsonConvert.DeserializeObject<dynamic>(response);

				return data;
			}
			catch (WebException wex)
			{
				string mappath = HttpContext.Current.Server.MapPath("ServicesExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + wex.Message.ToString()));

				throw wex;
			}
		}
		#endregion

		#region MUID
		public dynamic PostData(string url, dynamic jsonData)
		{
			var response = String.Empty;
			try
			{
				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				var req = (HttpWebRequest)WebRequest.Create(url);
				req.Credentials = CredentialCache.DefaultCredentials;
				WebProxy myProxy = new WebProxy();
				req.Proxy = myProxy;
				req.Method = "POST";
				var _jsonObject = JsonConvert.SerializeObject(jsonData);

				//If there is any json data
				if (!String.IsNullOrEmpty(_jsonObject))
				{
					using (System.IO.Stream s = req.GetRequestStream())
					{
						using (System.IO.StreamWriter sw = new System.IO.StreamWriter(s))
							sw.Write(_jsonObject);
					}
				}
				req.ContentType = "application/json; charset=utf-8";
				req.AllowAutoRedirect = false;
				var resp = (HttpWebResponse)req.GetResponse();
				var sr = new StreamReader(resp.GetResponseStream());

				if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
					(resp.StatusCode == HttpStatusCode.RedirectMethod))
				{
				}
				else
				{
					response = sr.ReadToEnd().Trim();
				}
			}
			catch (WebException wex)
			{
				string mappath = HttpContext.Current.Server.MapPath("SERPExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error on Data API:" + wex.Message.ToString()));

				throw wex;
			}

			return response;
		}

		public T GetSerialzedData<T>(string Input)
		{
			return JsonConvert.DeserializeObject<T>(Input);
		}

		public string GetIPAddress()
		{
			System.Web.HttpContext context = System.Web.HttpContext.Current;
			string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

			if (!string.IsNullOrEmpty(ipAddress))
			{
				string[] addresses = ipAddress.Split(',');
				if (addresses.Length != 0)
				{
					return addresses[0];
				}
			}

			return context.Request.ServerVariables["REMOTE_ADDR"];
		}
		#endregion

		public DataTable GetMeesevaAppStatus_data_helper(TransCls obj)
		{
			try
			{
				var comd = new OracleCommand();

				comd.InitialLONGFetchSize = 1000;
				comd.CommandType = CommandType.StoredProcedure;
				comd.CommandText = "gsws_Meeseva_id_details";
				comd.Parameters.Add("p_meeseva_id", OracleDbType.Varchar2).Value = obj.application_number;
				comd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

				DataTable data = GetgswsDataAdapter(comd);
				if (data != null && data.Rows.Count > 0)
					return data;
				else
					return null;
			}
			catch (WebException wex)
			{
				string mappath = HttpContext.Current.Server.MapPath("MeesevaExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Meeseva App Status API:" + wex.Message.ToString()));
				throw wex;
			}
		}
	}
}