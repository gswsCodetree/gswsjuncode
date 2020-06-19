using gswsBackendAPI.DL.DataConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Net;
using Newtonsoft.Json;

namespace gswsBackendAPI.DL.CommonHel
{
	public class LoginSPHelper: CommonSPHel
	{
		OracleCommand cmd;
		public DataTable GetLogin_SP(LoginModel ObjLogin)
		{
			try
			{
				cmd = new OracleCommand();			
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "gsws_sp_logins";
				cmd.Parameters.Add("ftype", OracleDbType.Varchar2, 10).Value = ObjLogin.Ftype;
				cmd.Parameters.Add("fuser_name", OracleDbType.Varchar2, 50).Value = ObjLogin.FUsername;
				cmd.Parameters.Add("fpassword", OracleDbType.Varchar2, 100).Value = ObjLogin.Newpassword;
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
			catch(Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("LoginSPExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetLogin_SP:" + ex.Message.ToString()));
				throw ex;
			}
		}


		public DataTable GetLogin_TokenSave(HeadertokenModel ObjLogin)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_CONCURRENT_USERS_PROC";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2, 10).Value = ObjLogin.Ftype;
				cmd.Parameters.Add("P_USER_ID", OracleDbType.Varchar2, 50).Value = ObjLogin.UserId;
				cmd.Parameters.Add("P_IP_ADDRESS", OracleDbType.Varchar2, 150).Value = GetIPAddress(); 
				cmd.Parameters.Add("P_TOKEN", OracleDbType.Varchar2, 350).Value = ObjLogin.Token;
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
				string mappath = HttpContext.Current.Server.MapPath("LoginSPExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetLogin_SP:" + ex.Message.ToString()));
				throw ex;
			}
		}
		public DataTable GetLgdMaster_SP(LGDMasterModel ObjLGD) //ftype=4,district,5-mandal 6-gp
		{
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_GET_SERVICE_DETAILS";
				cmd.Parameters.Add("ftype", OracleDbType.Varchar2, 10).Value = ObjLGD.FTYPE;
				cmd.Parameters.Add("FDEPT_ID", OracleDbType.Varchar2, 50).Value = ObjLGD.DEPTCODE;
				cmd.Parameters.Add("FHOD_ID", OracleDbType.Varchar2, 50).Value = ObjLGD.HODCODE;
				cmd.Parameters.Add("FDIST_CODE", OracleDbType.Varchar2, 50).Value = ObjLGD.DISTCODE;
				cmd.Parameters.Add("FMAND_CODE", OracleDbType.Varchar2, 50).Value = ObjLGD.MCODE;
				cmd.Parameters.Add("FVILL_CODE", OracleDbType.Varchar2, 50).Value = ObjLGD.GPCODE;
				cmd.Parameters.Add("P_URFLG", OracleDbType.Varchar2, 50).Value = ObjLGD.RUFLAG;
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
				string mappath = HttpContext.Current.Server.MapPath("LoginSPExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetLogin_SP:" + ex.Message.ToString()));
				throw ex;
			}
		}

		public DataTable GetGSWS_SecretariatMaster_SP(LGDMasterModel ObjLGD) //ftype=4,district,5-mandal 6-gp
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
				string mappath = HttpContext.Current.Server.MapPath("LoginSPExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetLogin_SP:" + ex.Message.ToString()));
				throw ex;
			}
		}

		public DataTable GetAllUrls_SP(LGDMasterModel ObjLGD) //ftype=4,district,5-mandal 6-gp
        {
            try
            {
                cmd = new OracleCommand();
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GSWS_GET_URL_DETAILS";
                cmd.Parameters.Add("ftype", OracleDbType.Varchar2, 10).Value = ObjLGD.FTYPE;
				cmd.Parameters.Add("fdist", OracleDbType.Varchar2, 10).Value = ObjLGD.DISTCODE;
				cmd.Parameters.Add("fmandal", OracleDbType.Varchar2, 10).Value = ObjLGD.MCODE;
				cmd.Parameters.Add("frole", OracleDbType.Varchar2, 10).Value = ObjLGD.ROLE;
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
                string mappath = HttpContext.Current.Server.MapPath("LoginSPExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetAllUrls_SP:" + ex.Message.ToString()));
                throw ex;
            }
        }

		public DataTable Get_GVDashboard_SP(LGDMasterModel ObjLGD) //ftype=4,district,5-mandal 6-gp
		{
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "youth_service.vv_dashboard_cluster_proc";
				cmd.Parameters.Add("ftype", OracleDbType.Varchar2, 10).Value = ObjLGD.FTYPE;
				cmd.Parameters.Add("fdist", OracleDbType.Varchar2, 10).Value = ObjLGD.DISTCODE;				
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtLogin = GetspsDataAdapter(cmd);
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
				string mappath = HttpContext.Current.Server.MapPath("LoginSPExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From Get_GVDashboard_SP:" + ex.Message.ToString()));
				throw ex;
			}
		}

		public DataTable GetTransactionResponse_SP(TransRes ObjLGD)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_SP_RESPONSE_TRACKING";
				cmd.Parameters.Add("ptype", OracleDbType.Varchar2, 10).Value = ObjLGD.type;
				cmd.Parameters.Add("puser", OracleDbType.Varchar2, 50).Value = ObjLGD.user;
				cmd.Parameters.Add("psecid", OracleDbType.Varchar2, 50).Value = ObjLGD.secid;
				cmd.Parameters.Add("pcur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtTrans = GetgswsDataAdapter(cmd);
				if (dtTrans != null)
				{
					return dtTrans;
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

		public DataTable GetRegReceivedDashboardCount(TransRes ObjLGD)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_SP_REGI_RECI_PROC";
				cmd.Parameters.Add("ptype", OracleDbType.Varchar2, 10).Value = ObjLGD.type;
				cmd.Parameters.Add("pdesig_id", OracleDbType.Varchar2, 50).Value = ObjLGD.designationId;
				cmd.Parameters.Add("pcur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtTrans = GetgswsDataAdapter(cmd);
				if (dtTrans != null)
				{
					return dtTrans;
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("LoginSPExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetRegReceivedDashboardCount:" + ex.Message.ToString()));
				throw ex;
			}
		}


		public string SaveStatusTracking(StatusTrackingModel objmodel)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.CommandText = @"Insert into gsws_status_tracking(DISTRICT_CODE,MANDAL_CODE,PANCHAYAT_CODE,SECC_CODE,PS_SD_ID,HOD_ID,URL_ID,CHECK_TRANS_ID,LOGIN_USER,LOGIN_USER,IP_ADDRESSES,SYSTEM_NAME,STATUS) values(:DISTRICT_CODE,:MANDAL_CODE,:PANCHAYAT_CODE,:SECC_CODE,:PS_SD_ID,:HOD_ID,:URL_ID,:CHECK_TRANS_ID,:LOGIN_USER,:IP_ADDRESSES,:SYSTEM_NAME,:STATUS)";
				cmd.Parameters.Add(":DISTRICT_CODE", OracleDbType.Varchar2, 10).Value = objmodel.DISTID;
				cmd.Parameters.Add(":MANDAL_CODE", OracleDbType.Varchar2, 10).Value = objmodel.MANDALID;
				cmd.Parameters.Add(":PANCHAYAT_CODE", OracleDbType.Varchar2, 10).Value = objmodel.GPID;
				cmd.Parameters.Add(":SECC_CODE", OracleDbType.Varchar2, 10).Value = objmodel.SECCID;
				cmd.Parameters.Add(":PS_SD_ID", OracleDbType.Varchar2, 10).Value = objmodel.DEPTID;
				cmd.Parameters.Add(":HOD_ID", OracleDbType.Varchar2, 10).Value = objmodel.HODID;
				cmd.Parameters.Add(":URL_ID", OracleDbType.Varchar2, 20).Value = objmodel.URLID;
				cmd.Parameters.Add(":CHECK_TRANS_ID", OracleDbType.Varchar2, 100).Value = objmodel.CHECKTRANSID;
				cmd.Parameters.Add(":LOGIN_USER", OracleDbType.Varchar2, 100).Value = objmodel.USERID;
				cmd.Parameters.Add(":IP_ADDRESSES", OracleDbType.Varchar2, 100).Value = GetIPAddress();
				cmd.Parameters.Add(":SYSTEM_NAME", OracleDbType.Varchar2, 100).Value = Dns.GetHostName(); 
				cmd.Parameters.Add(":STATUS", OracleDbType.Varchar2, 10).Value = "Initiate";
				
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
				string mappath = HttpContext.Current.Server.MapPath("LoginSPExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From SaveStatusTracking:" + ex.Message.ToString()));
				throw ex;
			}

		}

		//date :22/APR/2020
		public string SaveSecretariatForm_SP(SecretariatFormObject objseccform)
		{
			objseccform.UniqueId = objseccform.DistrictCode + objseccform.MandalCode + objseccform.SecretariatCode;
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_SEC_HW_INFO_PROC";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2, 10).Value = objseccform.Ftype;
				cmd.Parameters.Add("P_UNIQUE_ID", OracleDbType.Varchar2, 50).Value = objseccform.UniqueId;
				cmd.Parameters.Add("P_DISTRICT_CODE", OracleDbType.Varchar2, 10).Value = objseccform.DistrictCode;
				cmd.Parameters.Add("P_RURAL_URBAN", OracleDbType.Varchar2, 10).Value = objseccform.RUFlag;
				cmd.Parameters.Add("P_MANDAL_MUNICIPALITY_CODE", OracleDbType.Varchar2, 10).Value = objseccform.MandalCode;
				cmd.Parameters.Add("P_SECRETARIAT_CODE", OracleDbType.Varchar2, 10).Value = objseccform.SecretariatCode;
				cmd.Parameters.Add("P_IS_SEC_REC_SMART_PHONE", OracleDbType.Varchar2, 10).Value = objseccform.SecterataitSmartPhone;
				cmd.Parameters.Add("P_SMART_PHONE_MAKER", OracleDbType.Varchar2, 100).Value = objseccform.SecSmartPhoneMake;
				cmd.Parameters.Add("P_IMEI_SIM_1", OracleDbType.Varchar2, 100).Value = objseccform.SecSIMOneIMEI;
				cmd.Parameters.Add("P_IMEI_SIM_2", OracleDbType.Varchar2, 100).Value = objseccform.SecIMEITwoSIM;
				cmd.Parameters.Add("P_IS_SEC_REC_OFFICIAL_MOB_NO", OracleDbType.Varchar2, 100).Value = objseccform.SecRecieveMobileNum;
				cmd.Parameters.Add("P_SEC_MOBILE_OPERATOR", OracleDbType.Varchar2, 100).Value = objseccform.SecMobileOperator;
				cmd.Parameters.Add("P_SEC_MOBILE_NO", OracleDbType.Varchar2, 100).Value = objseccform.SecMobileNum;
				cmd.Parameters.Add("P_SEC_SIM_SERIAL_NO", OracleDbType.Varchar2, 100).Value = objseccform.SecSIMSerialNum;
				cmd.Parameters.Add("P_IS_SEC_REC_FPS", OracleDbType.Varchar2, 100).Value = objseccform.FPSScannerAviaiable;
				cmd.Parameters.Add("P_FPS_MAKER", OracleDbType.Varchar2, 100).Value = objseccform.FPSMake;
				cmd.Parameters.Add("P_FPS_MODEL_NO", OracleDbType.Varchar2, 100).Value = objseccform.FPSModelNum;
				cmd.Parameters.Add("P_FPS_SERIAL_NO", OracleDbType.Varchar2, 100).Value = objseccform.FPSSerialNum;
				cmd.Parameters.Add("P_IS_SEC_REC_PRINTER_SCANNER", OracleDbType.Varchar2, 100).Value = objseccform.PrinterAvailable;
				cmd.Parameters.Add("P_PRINTER_SCANNER_MAKER", OracleDbType.Varchar2, 100).Value = objseccform.PrinterMake;
				cmd.Parameters.Add("P_PRINTER_SCANNER_MODEL_NO", OracleDbType.Varchar2, 100).Value = objseccform.PrinterModelNum;
				cmd.Parameters.Add("P_PRINTER_SCANNER_SERIAL_NO", OracleDbType.Varchar2, 100).Value = objseccform.PrinterSerialNum;
				cmd.Parameters.Add("P_DESKTOP_COMPUTER_AVAILABLE", OracleDbType.Varchar2, 100).Value = objseccform.NumofDesktop;
				cmd.Parameters.Add("P_MONITOR_1_MAKER", OracleDbType.Varchar2, 100).Value = objseccform.DesktoponeMake;
				cmd.Parameters.Add("P_MONITOR_1_SERIAL_NO", OracleDbType.Varchar2, 100).Value = objseccform.DesktoponeSerial;
				cmd.Parameters.Add("P_CPU_1_MAKER", OracleDbType.Varchar2, 100).Value = objseccform.CPUoneMake;
				cmd.Parameters.Add("P_CPU_1_SERIAL_NO", OracleDbType.Varchar2, 100).Value = objseccform.CPUoneSerialNum;
				cmd.Parameters.Add("P_MOUSE_1_MAKER", OracleDbType.Varchar2, 100).Value = objseccform.MouseoneMake;
				cmd.Parameters.Add("P_MOUSE_1_SERIAL_NO", OracleDbType.Varchar2, 100).Value = objseccform.MouseoneSerialNum;
				cmd.Parameters.Add("P_KEYBOARD_1_MAKER", OracleDbType.Varchar2, 100).Value = objseccform.keyboardoneMake;
				cmd.Parameters.Add("P_KEYBOARD_1_SERIAL_NO", OracleDbType.Varchar2, 100).Value = objseccform.keyboardoneSerialNum;
				cmd.Parameters.Add("P_MONITOR_2_MAKER", OracleDbType.Varchar2, 100).Value = objseccform.DesktoptwoMake;
				cmd.Parameters.Add("P_MONITOR_2_SERIAL_NO", OracleDbType.Varchar2, 100).Value = objseccform.DesktoptwoSerial;
				cmd.Parameters.Add("P_CPU_2_MAKER", OracleDbType.Varchar2, 100).Value = objseccform.CPUtwoMake;
				cmd.Parameters.Add("P_CPU_2_SERIAL_NO", OracleDbType.Varchar2, 100).Value = objseccform.CPUtwoSerialNum;
				cmd.Parameters.Add("P_MOUSE_2_MAKER", OracleDbType.Varchar2, 100).Value = objseccform.MousetwoMake;
				cmd.Parameters.Add("P_MOUSE_2_SERIAL_NO", OracleDbType.Varchar2, 100).Value = objseccform.MousetwoSerialNum;
				cmd.Parameters.Add("P_KEYBOARD_2_MAKER", OracleDbType.Varchar2, 100).Value = objseccform.keyboardtwoMake;
				cmd.Parameters.Add("P_KEYBOARD_2_SERIAL_NO", OracleDbType.Varchar2, 100).Value = objseccform.keyboardtwoSerialNum;
				cmd.Parameters.Add("P_NO_OF_UPS_AVAILABLE", OracleDbType.Varchar2, 100).Value = objseccform.UPSAvailable;
				cmd.Parameters.Add("P_UPS_1_MAKER", OracleDbType.Varchar2, 100).Value = objseccform.UPSMakeone;
				cmd.Parameters.Add("P_UPS_1_SERIAL_NO", OracleDbType.Varchar2, 100).Value = objseccform.UPSoneSerialnum;
				cmd.Parameters.Add("P_UPS_2_MAKER", OracleDbType.Varchar2, 100).Value = objseccform.UPSMaketwo;
				cmd.Parameters.Add("P_UPS_2_SERIAL_NO", OracleDbType.Varchar2, 100).Value = objseccform.UPStwoSerialnum;
				cmd.Parameters.Add("P_IS_MP_REC_OFF_SMART_PHONE", OracleDbType.Varchar2, 100).Value = objseccform.RecieveMahilePoliceSmartPhone;
				cmd.Parameters.Add("P_MP_SMART_PHONE_MAKER", OracleDbType.Varchar2, 100).Value = objseccform.MahilaSmartPhMake;
				cmd.Parameters.Add("P_MP_IMEI_SIM_1", OracleDbType.Varchar2, 100).Value = objseccform.MahilaIMEINumone;
				cmd.Parameters.Add("P_MP_IMEI_SIM_2", OracleDbType.Varchar2, 100).Value = objseccform.MahilaIMEINumtwo;
				cmd.Parameters.Add("P_IS_MP_REC_OFFICIAL_MOB_NO", OracleDbType.Varchar2, 100).Value = objseccform.RecieveMobileNumber;
				cmd.Parameters.Add("P_MP_MOBILE_OPERATOR", OracleDbType.Varchar2, 100).Value = objseccform.MahilaMobileOperator;
				cmd.Parameters.Add("P_MP_MOBILE_NO", OracleDbType.Varchar2, 100).Value = objseccform.MahileMobileNumber;
				cmd.Parameters.Add("P_MP_SIM_SERIAL_NO", OracleDbType.Varchar2, 100).Value = objseccform.MahilaSIMSerialNum;
				cmd.Parameters.Add("P_NO_OF_SMART_PHONES_REC", OracleDbType.Varchar2, 100).Value = objseccform.VolunteerNoofSmartPh;
				cmd.Parameters.Add("P_NO_OF_SIM_CARDS_REC", OracleDbType.Varchar2, 100).Value = objseccform.VolunteerNoofSIMCard;
				cmd.Parameters.Add("P_INSERTED_BY", OracleDbType.Varchar2, 100).Value = objseccform.Insertby;
				cmd.Parameters.Add("P_IS_INTERNET_FAC_AVAILABLE", OracleDbType.Varchar2, 100).Value = objseccform.InternetAvailble;
				cmd.Parameters.Add("P_NO_OF_MOBILE_INSTALL_MDM", OracleDbType.Varchar2, 100).Value = objseccform.MobileMdmInstall;
				cmd.Parameters.Add("P_NO_OF_VOL_INST_AROGYASETHU", OracleDbType.Varchar2, 100).Value = objseccform.NoofArogyasetuapp;
				cmd.Parameters.Add("P_HIGH_SECUR_STATIONERY_REC", OracleDbType.Varchar2, 100).Value = objseccform.HighSecurityStationeryRec;
				cmd.Parameters.Add("P_HIGH_SECUR_STAT_STOCK_AVAI", OracleDbType.Varchar2, 100).Value = objseccform.HighSecurityStationeryStock_Availoable;
				cmd.Parameters.Add("P_IS_ELIG_CRITERIA_POSTER_DIS", OracleDbType.Varchar2, 100).Value = objseccform.EligibilityCriteria;
				cmd.Parameters.Add("P_ELIGI_CRITERIA_LAT_LONGS", OracleDbType.Varchar2, 600).Value = objseccform.EligibilityImage;
				cmd.Parameters.Add("P_IS_BENEFI_LIST_DISPLAYED", OracleDbType.Varchar2, 100).Value = objseccform.Beneficiarylist;
				cmd.Parameters.Add("P_BEN_LIST_LAT_LONGS", OracleDbType.Varchar2, 600).Value = objseccform.BeneficiarylistImage;
				cmd.Parameters.Add("P_NO_OF_TABLES", OracleDbType.Varchar2, 100).Value = objseccform.SecTables;
				cmd.Parameters.Add("P_NO_OF_PLASTIC_CHAIRS", OracleDbType.Varchar2, 100).Value = objseccform.SecPlasticchairs;
				cmd.Parameters.Add("P_NO_OF_S_TYPE_CHAIRS", OracleDbType.Varchar2, 100).Value = objseccform.SecStypechairs;
				cmd.Parameters.Add("P_NO_OF_NOTICE_BOARDS", OracleDbType.Varchar2, 100).Value = objseccform.Secnoticeboard;
				cmd.Parameters.Add("P_NO_OF_IRON_SAFES", OracleDbType.Varchar2, 100).Value = objseccform.SecIronsafes;
				cmd.Parameters.Add("P_NO_OF_IRON_RACKS", OracleDbType.Varchar2, 100).Value = objseccform.SecIronracks;
				cmd.Parameters.Add("P_NO_OF_VOL_TRAINED_1_PHASE", OracleDbType.Varchar2, 100).Value = objseccform.VolunteerPhaseone;
				cmd.Parameters.Add("P_NO_OF_VOL_TRAINED_2_PHASE", OracleDbType.Varchar2, 100).Value = objseccform.VolunteerPhasetwo;
				cmd.Parameters.Add("P_FUNCTIONARIES_TRAINED", OracleDbType.Varchar2, 100).Value = objseccform.FunctionariesTrained;
				cmd.Parameters.Add("P_VOLUNTEER_POST_VACANCIES", OracleDbType.Varchar2, 100).Value = objseccform.VolunteerVacant;

				cmd.Parameters.Add("P_IS_SEC_BUILDING_PAINTED", OracleDbType.Varchar2, 100).Value = objseccform.SecBuildingPaint;
				cmd.Parameters.Add("P_IS_NAME_BOARD_AVAILABLE", OracleDbType.Varchar2, 100).Value = objseccform.Secnameboard;
				cmd.Parameters.Add("P_IS_SEC_ELECTRIFICATION", OracleDbType.Varchar2, 100).Value = objseccform.SecElectrification;
				cmd.Parameters.Add("P_IS_DRINKING_WATER_AVAILABLE", OracleDbType.Varchar2, 100).Value = objseccform.SecDrinkwater;
				cmd.Parameters.Add("P_IS_TOILETS_AVAILABLE", OracleDbType.Varchar2, 100).Value = objseccform.SecToilets;
				cmd.Parameters.Add("P_IS_STATIONARY_AVAILABLE", OracleDbType.Varchar2, 100).Value = objseccform.SecStationary;
				cmd.Parameters.Add("P_IS_SPANDHANA_CENT_AVAILABLE", OracleDbType.Varchar2, 100).Value = objseccform.SecSpandana;
				cmd.Parameters.Add("P_IS_BILLING_CENTER_AVAILABLE", OracleDbType.Varchar2, 100).Value = objseccform.SecBillingCounter;
				cmd.Parameters.Add("P_IS_MCASS_ACTIVATED", OracleDbType.Varchar2, 100).Value = objseccform.MCassActive;
				cmd.Parameters.Add("P_MCASS_USER_ID", OracleDbType.Varchar2, 100).Value = objseccform.MCassUserId;

				cmd.Parameters.Add("P_CUR",OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				
				DataTable dtTrans = GetgswsDataAdapter(cmd);
				if (dtTrans != null && dtTrans.Rows.Count > 0 && dtTrans.Rows[0][0].ToString() == "1")
				{
					return "Success";
				}
				else if (dtTrans != null && dtTrans.Rows.Count > 0 && dtTrans.Rows[0][0].ToString() == "0")
				{
					return "Data Not Submitted.Please try Again";
				}
				else
				{
					return dtTrans.Rows[0][0].ToString();
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("SaveSeccformExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From SaveStatusTracking:" + ex.Message.ToString()));
				throw ex;
			}

		}


		//date :22/APR/2020
		public string SaveVolunteerForm_SP(VolunteerFormObject objvol)
		{
			
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;

				cmd.CommandText = "GSWS_VOLUNTEER_HW_INFO_PROC";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2, 10).Value = objvol.Ftype;
				cmd.Parameters.Add("P_DISTRICT_CODE", OracleDbType.Varchar2, 10).Value = objvol.DistrictCode;
				cmd.Parameters.Add("P_RURAL_URBAN_FLAG", OracleDbType.Varchar2, 10).Value = objvol.RUFlag;
				cmd.Parameters.Add("P_MANDAL_CODE", OracleDbType.Varchar2, 10).Value = objvol.MandalCode;
				cmd.Parameters.Add("P_SEC_CODE", OracleDbType.Varchar2, 10).Value = objvol.SecretariatCode;
				cmd.Parameters.Add("P_VOLUNTEER_NAME", OracleDbType.Varchar2, 150).Value = objvol.VolunteerName;
				cmd.Parameters.Add("P_VOLUNTEER_AADHAR", OracleDbType.Varchar2, 12).Value = objvol.VolunteerUID;
				cmd.Parameters.Add("P_VOL_CFMS_ID", OracleDbType.Varchar2, 50).Value = objvol.VolunteerCFMSID;
				cmd.Parameters.Add("P_CLUSTER_NAME", OracleDbType.Varchar2, 100).Value = objvol.VolunteerClusterName;
				cmd.Parameters.Add("P_VOLUNTEER_TYPE", OracleDbType.Varchar2, 100).Value = objvol.VolunteertType;
				cmd.Parameters.Add("P_VOL_SMART_PHONE_MAKER", OracleDbType.Varchar2, 100).Value = objvol.VolunteerSmartPhone;
				cmd.Parameters.Add("P_VOL_IMEI_SIM_1", OracleDbType.Varchar2, 50).Value = objvol.VolunteerSIMIMEIOne;
				cmd.Parameters.Add("P_VOL_IMEI_SIM_2", OracleDbType.Varchar2, 100).Value = objvol.VolunteerSIMIMEITwo;
				cmd.Parameters.Add("P_VOL_OFF_MOBILE_OPERATOR", OracleDbType.Varchar2, 100).Value = objvol.VolunteerMobileOperator;
				cmd.Parameters.Add("P_VOL_OFFICIAL_MOB_NO", OracleDbType.Varchar2, 10).Value = objvol.VolunteerOfficialMobile;
				cmd.Parameters.Add("P_VOL_PERSONAL_MOB_NO", OracleDbType.Varchar2, 10).Value = objvol.VolunteerPersonalMobile;
				cmd.Parameters.Add("P_VOL_OFFICAIL_SNO", OracleDbType.Varchar2, 100).Value = objvol.VolunteerSerialNumber;
				cmd.Parameters.Add("P_VOL_FPS_MAKER", OracleDbType.Varchar2, 100).Value = objvol.VolunteerFPS;
				cmd.Parameters.Add("P_VOL_FPS_MODEL_NO", OracleDbType.Varchar2, 100).Value = objvol.VolunteerFPSModelNumber;
				cmd.Parameters.Add("P_VOL_FPS_SERIAL_NO", OracleDbType.Varchar2, 100).Value = objvol.VolunteerFPSSerialNumber;
				cmd.Parameters.Add("P_INSERTED_BY", OracleDbType.Varchar2, 100).Value = objvol.Insertby;
				cmd.Parameters.Add("P_IS_VOL_REC_SMART_PHONE", OracleDbType.Varchar2, 100).Value = objvol.VolunteerSmartPhoneRecieve;
				cmd.Parameters.Add("P_IS_VOL_REC_MOB_NO", OracleDbType.Varchar2, 100).Value = objvol.VolunteerSIMMobileNumbers;
				cmd.Parameters.Add("P_IS_VOL_REC_FPS", OracleDbType.Varchar2, 100).Value = objvol.VolunteerFingerprintcumscanner;
				cmd.Parameters.Add("P_IS_VOL_INSTALL_TELEGRAM", OracleDbType.Varchar2, 100).Value = objvol.VolunteerinstallTelgram;
				cmd.Parameters.Add("P_VOL_WTSAPP_GROUP_CLUSTER", OracleDbType.Varchar2, 100).Value = objvol.VolunteerWtsclustergroup;
				cmd.Parameters.Add("P_IS_MCASS_ACTIVATED", OracleDbType.Varchar2, 100).Value = objvol.VolMCassActive;
				cmd.Parameters.Add("P_MCASS_USER_ID", OracleDbType.Varchar2, 100).Value = objvol.VolMCassUserId;

				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

				DataTable dtTrans = GetgswsDataAdapter(cmd);
				if (dtTrans != null && dtTrans.Rows.Count > 0 && dtTrans.Rows[0][0].ToString() == "1")
				{
					return "Success";
				}
				else if (dtTrans != null && dtTrans.Rows.Count > 0 && dtTrans.Rows[0][0].ToString() == "0")
				{
					return "Data Not Submitted.Please try Again";
				}
				else
				{
					return dtTrans.Rows[0][0].ToString();
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("SaveSeccformExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From SaveStatusTracking:" + ex.Message.ToString()));
				throw ex;
			}

		}

		//date :07/May/2020
		public string UpdateMailMobileForm_SP(LoginMailModel objLogin)
		{

			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;

				cmd.CommandText = "GSWS_GEO_STATUS_LOGINS_PROC";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2, 10).Value = objLogin.Ftype;
				cmd.Parameters.Add("P_DISTRICT_CODE", OracleDbType.Varchar2, 10).Value = objLogin.FDistrictCode;
				cmd.Parameters.Add("P_MANDAL_CODE", OracleDbType.Varchar2, 10).Value = objLogin.FMandalCode;
				cmd.Parameters.Add("P_SEC_CODE", OracleDbType.Varchar2, 10).Value = objLogin.FSeccCode;
				cmd.Parameters.Add("P_USER_ID", OracleDbType.Varchar2, 150).Value = objLogin.FUserId;
				cmd.Parameters.Add("P_MOBILE_NO", OracleDbType.Varchar2, 12).Value = objLogin.FMobileNumber;
				cmd.Parameters.Add("P_MAIL_ID", OracleDbType.Varchar2, 50).Value = objLogin.FMailID;
				cmd.Parameters.Add("P_PASSWORD", OracleDbType.Varchar2, 100).Value = objLogin.FPassword;
				cmd.Parameters.Add("P_DESIG_ROLE_ID", OracleDbType.Varchar2, 100).Value = objLogin.FRoleId;
				cmd.Parameters.Add("P_INSERTED_BY", OracleDbType.Varchar2, 100).Value = objLogin.Insertby;
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

				DataTable dtTrans = GetgswsDataAdapter(cmd);
				if (dtTrans != null && dtTrans.Rows.Count > 0 && dtTrans.Rows[0][0].ToString() == "1")
				{
					return "Success";
				}
				else if (dtTrans != null && dtTrans.Rows.Count > 0 && dtTrans.Rows[0][0].ToString() == "0")
				{
					return "Data Not Submitted.Please try Again";
				}
				else
				{
					return dtTrans.Rows[0][0].ToString();
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("UpdateMailExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From SaveStatusTracking:" + ex.Message.ToString()));
				throw ex;
			}

		}

		public DataTable GetHardwareValidate_SP(HardwareValidateModel ObjLGD)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_HW_VALIDATION_PROC";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2, 10).Value = ObjLGD.FTYPE;
				cmd.Parameters.Add("P_IMEI", OracleDbType.Varchar2, 50).Value = ObjLGD.IMEINUM;
				cmd.Parameters.Add("P_SERIAL_NO", OracleDbType.Varchar2, 50).Value = ObjLGD.SERIALNUM;
				cmd.Parameters.Add("P_MODEL_NO", OracleDbType.Varchar2, 50).Value = ObjLGD.MODELNUM;
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtTrans = GetgswsDataAdapter(cmd);
				if (dtTrans != null)
				{
					return dtTrans;
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("LoginSPExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetRegReceivedDashboardCount:" + ex.Message.ToString()));
				throw ex;
			}
		}

		public DataTable GetSeccandVolunteerdetail_SP(LGDMasterModel ObjLGD)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_SEC_VOL_HW_DETAILS";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2, 10).Value = ObjLGD.FTYPE;
				cmd.Parameters.Add("P_DISTRICT", OracleDbType.Varchar2, 50).Value = ObjLGD.DISTCODE;
				cmd.Parameters.Add("P_MANDAL", OracleDbType.Varchar2, 50).Value = ObjLGD.MCODE;
				cmd.Parameters.Add("P_RURAL_FLAG", OracleDbType.Varchar2, 50).Value = ObjLGD.RUFLAG;
				cmd.Parameters.Add("P_SEC_CODE", OracleDbType.Varchar2, 50).Value = ObjLGD.GPCODE;
				cmd.Parameters.Add("P_VOL_ADHAAR", OracleDbType.Varchar2, 50).Value = ObjLGD.ROLE;
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtTrans = GetgswsDataAdapter(cmd);
				if (dtTrans != null)
				{
					return dtTrans;
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("LoginSPExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetRegReceivedDashboardCount:" + ex.Message.ToString()));
				throw ex;
			}
		}

		public DataTable SaveReceiveAction_SP(TransRes ObjLGD)
		{
			try
			{
				//cmd = new OracleCommand();
				//cmd.InitialLONGFetchSize = 1000;
				//cmd.CommandType = CommandType.StoredProcedure;
				//cmd.CommandText = "GSWS_SP_RESPONSE_TRACKING";
				//cmd.Parameters.Add("ptype", OracleDbType.Varchar2, 10).Value = ObjLGD.type;
				//cmd.Parameters.Add("puser", OracleDbType.Varchar2, 50).Value = ObjLGD.user;
				//cmd.Parameters.Add("psecid", OracleDbType.Varchar2, 50).Value = ObjLGD.secid;
				//cmd.Parameters.Add("pcur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				//DataTable dtTrans = GetgswsDataAdapter(cmd);
				//if (dtTrans != null)
				//{
				//    return dtTrans;
				//}
				//else
				//{
				//    return null;
				//}



				string mappath = HttpContext.Current.Server.MapPath("SaveReceivedLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Recevied Trasaction Data:" + JsonConvert.SerializeObject(ObjLGD)));

				DataTable dtTrans = new DataTable();
				return dtTrans;
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("LoginSPExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetLogin_SP:" + ex.Message.ToString()));
				throw ex;
			}
		}
		//18-jun-2020 chandu	
		public  DataTable CFMSGATEWAY_Save_Data_SP(CFMSPAYMENTMODEL obj)
		{

			try
			{
				OracleCommand cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "gsws_in_payment_trans_data";
				cmd.Parameters.Add("pTYPE", OracleDbType.Varchar2).Value = obj.Ftype;
				cmd.Parameters.Add("pGSWS_TRANS_ID", OracleDbType.Varchar2).Value = obj.GSWSTransactionID;
				cmd.Parameters.Add("pPAYMENT_TRANS_ID", OracleDbType.Varchar2).Value = obj.PAYMENT_TRANS_ID;
				cmd.Parameters.Add("pGSWS_PAYMENT_TRANS_ID", OracleDbType.Varchar2).Value = obj.GSWSPaymentTransactionID;
				cmd.Parameters.Add("pSECRETARIAT_CODE", OracleDbType.Varchar2).Value = obj.SecretartaitCode;
				cmd.Parameters.Add("pLOGIN_USER", OracleDbType.Varchar2).Value = obj.LoginUser;
				cmd.Parameters.Add("pURL_ID", OracleDbType.Varchar2).Value = obj.URLID;
				cmd.Parameters.Add("pHOA_CODE", OracleDbType.Varchar2).Value = obj.HOA_CODE;
				cmd.Parameters.Add("pDDO_CODE", OracleDbType.Varchar2).Value = obj.DDO_CODE;
				cmd.Parameters.Add("pAMOUNT", OracleDbType.Varchar2).Value = obj.AMOUNT;
				cmd.Parameters.Add("pDEPT_ID", OracleDbType.Varchar2).Value = obj.DEPT_ID;
				cmd.Parameters.Add("pTOTAL_AMOUNT", OracleDbType.Varchar2).Value = obj.TOTAL_AMOUNT;
				cmd.Parameters.Add("pPAYMENT_STATUS", OracleDbType.Varchar2).Value = obj.PAYMENT_STATUS;
				cmd.Parameters.Add("pCHALLAN_ID", OracleDbType.Varchar2).Value = obj.CHALLAN_ID;
				cmd.Parameters.Add("pCFMS_STATUS", OracleDbType.Varchar2).Value = obj.CFMS_STATUS;
				cmd.Parameters.Add("pcur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtTrans = GetProdgswsDataAdapter(cmd);//db.executeProcedure(cmd, oradbGswsProd);
				if (dtTrans != null)
				{
					return dtTrans;
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("CFMSGATEWAY_Save_Data_SPExceptionLogs");
				
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetLogin_SP:" + ex.Message.ToString()));

				throw ex;
			}
		}


		public bool GSWS_SP_IN_CAPTCHA(captch root)
		{
			try
			{

				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_SP_CAPTURE_DATA";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2, 10).Value = root.type;
				cmd.Parameters.Add("P_CAPTURE_ID", OracleDbType.Varchar2, 50).Value = root.id;
				cmd.Parameters.Add("P_CAPTURE", OracleDbType.Varchar2, 50).Value = root.Capchid;
				cmd.Parameters.Add("P_REQUESTED_IP", OracleDbType.Varchar2, 50).Value = GetIPAddress();
				cmd.Parameters.Add("P_PROJECT_NAME", OracleDbType.Varchar2, 50).Value = "GSWS";				
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtLogin = GetgswsDataAdapter(cmd);				
				if (dtLogin != null && dtLogin.Rows.Count>0 && dtLogin.Rows[0][0].ToString()=="1")
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("LoginSPExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GSWS_SP_IN_CAPTCHA:" + ex.Message.ToString()));
				throw ex;
			}


		}

		public int GSWS_SP_IN_RoleAccess(captch root)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_GET_URL_DETAILS";
				cmd.Parameters.Add("ftype", OracleDbType.Varchar2, 10).Value = "6";
				cmd.Parameters.Add("fdist", OracleDbType.Varchar2, 10).Value =root.id;
				cmd.Parameters.Add("fmandal", OracleDbType.Varchar2, 10).Value = "";
				cmd.Parameters.Add("frole", OracleDbType.Varchar2, 150).Value = root.SOURCE;
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtLogin = GetgswsDataAdapter(cmd);
				if (dtLogin != null && dtLogin.Rows.Count > 0 && dtLogin.Rows[0][0].ToString() == "1")
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("LoginSPExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GSWS_SP_IN_RoleAccess:" + ex.Message.ToString()));
				throw ex;
			}


		}

		public string GenerateuniTransactionID()
		{
			//urlid +secretarint id+

			return "success";
		}
		public string ErrorMessage = "Something Went Wrong";
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
		#region Cumilative Dashboard
		public DataTable GetCumilativeDashboardData_SP(Profilemodel Obj)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_DASHBOARD";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2, 10).Value = Obj.TYPE;
				cmd.Parameters.Add("P_DISTRICT", OracleDbType.Varchar2, 50).Value = Obj.DISTRICT;
				cmd.Parameters.Add("P_DEPARTMENT", OracleDbType.Varchar2, 500).Value = Obj.DEPARTMENT;
				cmd.Parameters.Add("P_FROM_DATE", OracleDbType.Varchar2, 500).Value = Obj.FROMDATE;
				cmd.Parameters.Add("P_TO_DATE", OracleDbType.Varchar2, 500).Value = Obj.TODATE;
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
				string mappath = HttpContext.Current.Server.MapPath("LoginSPExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetLogin_SP:" + ex.Message.ToString()));
				throw ex;
			}
		}
		#endregion
		#region Profile Update
		//Send OTP
		public dynamic ProfileSendOTP_SP(Profilemodel obj)
		{
			try
			{
				int k = 0;

				var comd = new OracleCommand();
				comd.CommandText = @"insert into gsws_mobile_auth_otp_log (OTP,UNIQUE_ID,OTP_TYPE) 
																values(:OTP,:UNIQUE_ID,:OTP_TYPE)";


				comd.Parameters.Add(":OTP", OracleDbType.Varchar2, 20).Value = obj.OTP;
				comd.Parameters.Add(":UNIQUE_ID", OracleDbType.Varchar2, 20).Value = obj.UNIQUEID;
				comd.Parameters.Add(":OTP_TYPE", OracleDbType.Varchar2, 20).Value = obj.TYPE;

				k = getgswsExecuteNonQuery(comd);

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
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error Send Profile Page OTP data :" + wex.Message.ToString()));
				throw new Exception(wex.Message);
			}
		}

		//Verify OTP
		public bool ProfileVerifyOTP_SP(Profilemodel root)
		{
			try
			{

				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_SP_MOBILE_AUTH_OTP_LOG";
				cmd.Parameters.Add("FTYPE", OracleDbType.Varchar2, 50).Value = "2";
				cmd.Parameters.Add("FMOBILE_NUMBER", OracleDbType.Varchar2, 50).Value = root.UNIQUEID;
				cmd.Parameters.Add("FOTP", OracleDbType.Varchar2, 10).Value = root.OTP;
				cmd.Parameters.Add("F_SRC_ID", OracleDbType.Varchar2, 50).Value = root.TYPE;
				cmd.Parameters.Add("FSTATUS", OracleDbType.Varchar2, 50).Value = root.STATUS;
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtLogin = GetgswsDataAdapter(cmd);
				if (dtLogin != null && dtLogin.Rows[0][0].ToString() == "OTP Verified Successfully")
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("LoginSPExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GSWS_SP_IN_CAPTCHA:" + ex.Message.ToString()));
				throw ex;
			}


		}

		//Updagte Profile Data
		public bool ProfileUpdate_SP(Profilemodel root)
		{
			try
			{

				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_USER_PROFILE_UPDATE";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2, 50).Value = root.TYPE;
				cmd.Parameters.Add("P_PWD", OracleDbType.Varchar2, 500).Value = root.PASSWORD;
				cmd.Parameters.Add("P_MOBILE", OracleDbType.Varchar2, 10).Value = root.MOBILE;
				cmd.Parameters.Add("P_MAILID", OracleDbType.Varchar2, 50).Value = root.EMAILID;
				cmd.Parameters.Add("P_UID", OracleDbType.Varchar2, 50).Value = root.UNIQUEID;
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtLogin = GetgswsDataAdapter(cmd);
				if (dtLogin != null && dtLogin.Rows.Count > 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("LoginSPExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GSWS_SP_IN_CAPTCHA:" + ex.Message.ToString()));
				throw ex;
			}


		}

		public bool APITRacking_SP(APITRACKMODEL root)
		{
			try
			{

				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "gsws_in_api_tracking_proc";
				cmd.Parameters.Add("p_type", OracleDbType.Varchar2, 50).Value = root.Ptype;
				cmd.Parameters.Add("ptransaction_id", OracleDbType.Varchar2, 50).Value = root.TrackingId;
				cmd.Parameters.Add("pdistrict_code", OracleDbType.Varchar2, 10).Value = root.DistrictCode;
				cmd.Parameters.Add("pmandal_code", OracleDbType.Varchar2, 50).Value = root.MandalCode;
				cmd.Parameters.Add("psec_code", OracleDbType.Varchar2, 50).Value = root.SceretriatCode;
				cmd.Parameters.Add("pdept_id", OracleDbType.Varchar2, 50).Value = root.DeptId;
				cmd.Parameters.Add("phod_id", OracleDbType.Varchar2, 50).Value = root.HODId;
				cmd.Parameters.Add("purl_id", OracleDbType.Varchar2, 50).Value = root.UrlId;
				cmd.Parameters.Add("pinput", OracleDbType.Varchar2, 2000).Value = root.InputData;
				cmd.Parameters.Add("pstatus", OracleDbType.Varchar2, 50).Value = root.Status;
				cmd.Parameters.Add("premarks", OracleDbType.Varchar2, 500).Value = root.Remarks;
				cmd.Parameters.Add("puser_id", OracleDbType.Varchar2, 50).Value = root.Loginid;
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtLogin = GetgswsDataAdapter(cmd);
				if (dtLogin != null && dtLogin.Rows.Count > 0 && dtLogin.Rows[0][0].ToString()=="1")
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("APITRacking_SPExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From APITRacking_SP:" + ex.Message.ToString()));
				return false;
			}


		}



		public bool Affidavit_Sp(Affidavitmodel root)
		{
			try
			{

				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_AFFIDAVIT_PROC";
				cmd.Parameters.Add("p_type", OracleDbType.Varchar2, 50).Value = root.FType;
				cmd.Parameters.Add("P_DISTRICT", OracleDbType.Varchar2, 50).Value = root.DistCode;
				cmd.Parameters.Add("P_MANDAL", OracleDbType.Varchar2, 10).Value = root.MCode;
				cmd.Parameters.Add("P_SECRETARIAT", OracleDbType.Varchar2, 50).Value = root.SecCode;
				cmd.Parameters.Add("P_LOGIN_USER", OracleDbType.Varchar2, 50).Value = root.Loginuser;
				cmd.Parameters.Add("P_FILE_PATH", OracleDbType.Varchar2, 1000).Value = root.FilePath;		
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtLogin = GetgswsDataAdapter(cmd);
				if (dtLogin != null && dtLogin.Rows.Count > 0 && dtLogin.Rows[0][0].ToString() == "1")
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("Affidavit_Sp_SPExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From APITRacking_SP:" + ex.Message.ToString()));
				return false;
			}


		}

		#endregion

		#region Incharge Change
		public DataTable InchargeChange_data_helper(Digitalmodels ObjLGD)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_INCHARGE_LOGINS_PROC";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2, 10).Value = ObjLGD.P_TYPE;
				cmd.Parameters.Add("P_USER_ID", OracleDbType.Varchar2, 50).Value = ObjLGD.USER_ID;
				cmd.Parameters.Add("P_PWD", OracleDbType.Varchar2, 50).Value = ObjLGD.PASSWORD;
				cmd.Parameters.Add("P_MOBILE", OracleDbType.Varchar2, 50).Value = ObjLGD.MOBILE_NO;
				cmd.Parameters.Add("P_FROM_DATE", OracleDbType.Varchar2, 50).Value = ObjLGD.FROMDATE;
				cmd.Parameters.Add("P_TO_DATE", OracleDbType.Varchar2, 50).Value = ObjLGD.TODATE;
				cmd.Parameters.Add("P_ROLE_ID", OracleDbType.Varchar2, 50).Value = ObjLGD.ROLL_ID;
				cmd.Parameters.Add("P_ENTRY_BY", OracleDbType.Varchar2, 50).Value = ObjLGD.ENTRY_BY;
				cmd.Parameters.Add("P_SEC_ID", OracleDbType.Varchar2, 50).Value = ObjLGD.SCRT_ID;
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
				string mappath = HttpContext.Current.Server.MapPath("InchargeChangeExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error For Incharge Change :" + ex.Message.ToString()));
				throw ex;
			}
		}

		public int SendPassword(string Mobile, string USERTYPE, ref string UserPassword)
		{
			int result = 0;
			var Password = "";
			try
			{
				Random random = new Random();
				string r = "";
				int i;
				for (i = 0; i < 4; i++)
				{
					r += random.Next(0, 6).ToString();
				}

				if (USERTYPE == "1")
					Password = "DA@" + r;
				else
					Password = "WEDS@" + r;


				SMSService.SMSService objsms = new SMSService.SMSService();
				string Status = objsms.SendSMS(Mobile, "Your Password for Grama Ward Sachivalayam is : " + Password);
				if (Status.Contains("402"))
					result = 1;
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("InchargeChangeExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Send Password Failed for Mobile : " + Mobile + " Message : " + ex.Message.ToString()));
			}
			UserPassword = Password;
			return result;
		}
		#endregion

		public DataTable GetCountsData_Helper_SP(LoginModel ObjLogin)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "gsws_dashboard_landing_sp";
				cmd.Parameters.Add("ptype", OracleDbType.Varchar2, 10).Value = ObjLogin.Ftype;
				cmd.Parameters.Add("pdist", OracleDbType.Varchar2, 50).Value = ObjLogin.FUsername;
				cmd.Parameters.Add("pmandal", OracleDbType.Varchar2, 100).Value = ObjLogin.Newpassword;
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtLogin = GetProdgswsDataAdapter(cmd);
				return dtLogin;

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("LandPageCountsLog");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From gsws_dashboard_landing_sp:" + ex.Message.ToString()));
				throw ex;
			}
		}



		public DataTable Save_Exception_Data(ExceptionDataModel ObjLogin)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_IN_EXCEPTION_PROC";
				cmd.Parameters.Add("F_TYPE", OracleDbType.Varchar2, 100).Value = "1";
				cmd.Parameters.Add("P_DEPTCODE", OracleDbType.Varchar2, 10).Value = ObjLogin.E_DEPTID;
				cmd.Parameters.Add("P_HODCODE", OracleDbType.Varchar2, 50).Value = ObjLogin.E_HODID;
				cmd.Parameters.Add("P_URLID", OracleDbType.Varchar2, 100).Value = ObjLogin.E_URLID;
				cmd.Parameters.Add("P_SECRETARIATCODE", OracleDbType.Varchar2, 500).Value = ObjLogin.E_SECRETARIATCODE;
				cmd.Parameters.Add("P_ERRORMESSAGE", OracleDbType.Varchar2, 500).Value = ObjLogin.E_ERRORMESSAGE;
				cmd.Parameters.Add("P_SERVICEAPIURL", OracleDbType.Varchar2, 500).Value = ObjLogin.E_SERVICEAPIURL;
				cmd.Parameters.Add("P_ERRORTYPE", OracleDbType.Varchar2, 100).Value = ObjLogin.E_ERRORTYPE;								
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtLogin = GetProdgswsDataAdapter(cmd);
				return dtLogin;

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("Save_Exception_DataLog");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From gsws_dashboard_landing_sp:" + ex.Message.ToString()));
				throw ex;
			}
		}

		//GSWS Dashboard
		public DataTable SecretariatReport_sphel(Profilemodel Obj)
		{
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_DSHBORD_REV_NEW_COUNTS1";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2, 10).Value = Obj.TYPE;
				cmd.Parameters.Add("P_DISTRICT_ID", OracleDbType.Varchar2, 50).Value = Obj.DISTRICT;
				cmd.Parameters.Add("P_MANDAL_ID", OracleDbType.Varchar2, 50).Value = Obj.MANDAL;
				cmd.Parameters.Add("P_SEC_ID", OracleDbType.Varchar2, 50).Value = Obj.SECRETARIAT;
				cmd.Parameters.Add("P_DEPARTMENT_ID", OracleDbType.Varchar2, 50).Value = Obj.DEPARTMENT;
				cmd.Parameters.Add("P_SERVICE_ID", OracleDbType.Varchar2, 50).Value = Obj.SERVICE;
				cmd.Parameters.Add("P_RURAL_OR_URBAN", OracleDbType.Varchar2, 50).Value = Obj.FLAG;
				cmd.Parameters.Add("P_FROM_DATE", OracleDbType.Varchar2, 100).Value = Obj.FROMDATE;
				cmd.Parameters.Add("P_TO_DATE", OracleDbType.Varchar2, 100).Value = Obj.TODATE;
				cmd.Parameters.Add("P_MEESEVA_OR_OTHER", OracleDbType.Varchar2, 100).Value = Obj.SERVICE_TYPE;
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtLogin = GetProdgswsDataAdapter(cmd);
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
				string mappath = HttpContext.Current.Server.MapPath("LoginSPExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetLogin_SP:" + ex.Message.ToString()));
				throw ex;
			}
		}
	}
}