using gswsBackendAPI.DL.CommonHel;
using gswsBackendAPI.DL.DataConnection;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using gswsBackendAPI.Depts.REVENUE.Backend;
using System.Globalization;

namespace gswsBackendAPI.transactionModule
{
    public class transactionHelper
    {
        static DL.DataConnection.ConnectionHelper _conHel = new DL.DataConnection.ConnectionHelper();
        static string oradb = _conHel.Congsws;
        OracleConnection con = new OracleConnection(oradb);

        public dynamic initiateTransaction(transactionModel obj)
        {

            dynamic objdata = new ExpandoObject();
            try
            {
                obj.TYPE = "1";
                obj.IP_ADDRESS = HttpContext.Current.Request.UserHostAddress;
                obj.SYS_NAME =System.Environment.MachineName;
                obj.TXN_ID = obj.SECRETRAINT_CODE + DateTime.Now.ToString("yymmddHHmm") + new Random().Next(1000, 9999);
                DataTable dt = transactionInsertion(obj);

                if (dt != null && dt.Rows.Count > 0 )
                {
					string encrypttext = "";
					string iv = "";
					if (obj.TYPE_OF_SERVICE == "1")
					{
						iv = CryptLib.GenerateRandomIV(16);
						string key = CryptLib.getHashSha256("GSWS TEST", 32);
						string obj2 = GetInputJsonFormat(obj);
						encrypttext = EncryptDecryptAlgoritham.EncryptStringAES(obj2, key, iv);
					}
					else if (obj.URL_ID == "110401301" || obj.URL_ID == "110102501" || obj.URL_ID== "110102601" || obj.URL_ID=="310300104")
					{
						iv = CryptLib.GenerateRandomIV(16);
						string key = CryptLib.getHashSha256("GSWS TEST", 32);
						string obj2 = GetInputJsonFormat(obj);
						encrypttext = EncryptDecryptAlgoritham.EncryptStringAES(obj2, key, iv);
					}
					else if (obj.URL_ID == "200301401" || obj.URL_ID == "360201701" || obj.URL_ID == "360201401" || obj.URL_ID == "130101101" || obj.URL_ID == "280101201" || obj.URL_ID == "280101401" || obj.URL_ID == "280101301" || obj.URL_ID == "360201801" || obj.URL_ID == "3602018501" || obj.URL_ID == "170100102" || obj.URL_ID == "130101401" || obj.URL_ID == "130101501" || obj.URL_ID == "240200101")
					{
						iv = CryptLib.GenerateRandomIV(16);
						string key = CryptLib.getHashSha256("GSWS TEST", 32);
						string obj2 = GetInputJsonFormat(obj);
						encrypttext = EncryptDecryptAlgoritham.EncryptStringAES(obj2, key, iv);
					}
					else
					{
					}
					objdata.status = 200;
                    //objdata.Translist = dt;
					objdata.encrypttext = encrypttext;
					objdata.key = iv;
					objdata.TransId = obj.TXN_ID;
					objdata.Reason = "Record Inserted Successfully !!!";
                }
                else
                {
                    objdata.status = 400;
                    objdata.Reason = "Failed to Insert Record, Please Try Again !!! ";
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
				objdata.Reason = "Something Went Wrong.Please Try Again";//ex.Message.ToString();
            }

            return objdata;
        }

		public dynamic initiateSpandanaTransaction(transactionModel obj)
		{
			
			dynamic objdata = new ExpandoObject();
			try
			{
				obj.TYPE = "1";
				obj.IP_ADDRESS = HttpContext.Current.Request.UserHostAddress;
				obj.SYS_NAME = System.Environment.MachineName;
				obj.TXN_ID = obj.SECRETRAINT_CODE + DateTime.Now.ToString("yymmddHHmm") + new Random().Next(1000, 9999);
				DataTable dt = transactionInsertion(obj);

				if (dt != null && dt.Rows.Count > 0)
				{
					string encrypttext = "";
					string iv = "";
					
						iv = CryptLib.GenerateRandomIV(16);
						string key = CryptLib.getHashSha256("GSWS TEST", 32);
						string obj2 = GetInputJsonFormat(obj);
						encrypttext = EncryptDecryptAlgoritham.EncryptStringAES(obj2, key, iv);
					
					objdata.status = 200;			
					if (obj.URL_ID == "340200101")
						objdata.URL = "https://www.spandana.ap.gov.in/gsws/servicegrievance_registration?accessToken="+Token() + "&Volunteerid=2255667788&AadhaarNo=" + obj.UID + "&vvstype=VVS2&DistId=" + obj.Sdistcode + "&MandalId=" + obj.Smcode + "&GpId=" + obj.Svtcode + "&GpFlag=" + obj.SRuflag + "&encryptId=" + encrypttext + "&KEY=" + key + "&IV=" + iv;
					else
						objdata.URL = "https://www.spandana.ap.gov.in/gsws/servicerequest_registration?HodId="+obj.SERVICE_CODE+"&accessToken= "+Token() + "&Volunteerid=2255667788&AadhaarNo=" + obj.UID + "&vvstype=VVS2&DistId=" + obj.Sdistcode + "&MandalId=" + obj.Smcode + "&GpId=" + obj.Svtcode + "&GpFlag=" + obj.SRuflag + "&encryptId=" + encrypttext + "&KEY=" + key + "&IV=" + iv;


					objdata.Reason = "Record Inserted Successfully !!!";
				}
				else
				{
					objdata.status = 400;
					objdata.Reason = "Failed to Insert Record, Please Try Again !!! ";
				}
			}
			catch (Exception ex)
			{
				objdata.status = 500;
				objdata.Reason = "Something Went Wrong.Please Try Again";//ex.Message.ToString();
			}

			return objdata;
		}

		public string Token()
		{
			object obj = new
			{
				UserID = "codetreevs",
				Mobile = "",
				Password = "rtgsrc@123",
				RegMail = "",
				AppType = "codetreeration"
			};

			var data = new EncryptDecrypt().PostData(sapandanaurl.tokenurl, obj);

			spandamurlmodel objdata = JsonConvert.DeserializeObject<spandamurlmodel>(data);

			return objdata.Token;
		}

		public dynamic closingTransaction(transactionModel obj)
        {

            dynamic objdata = new ExpandoObject();
            try
            {
                obj.TYPE = "2";
                DataTable dt = transactionInsertion(obj);

                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
					SMSService.SMSService objsms = new SMSService.SMSService();

					string Status = "402"; //objsms.SendSMS("8096424075", "Dear User \n Your Service Request Id is :" + obj.TXN_ID + "For Service" + obj.SERVICE_NAME + "on Date:" + DateTime.Now.ToString() + " at" + obj.SECRETRAINT_CODE);

					if (Status.Contains("402"))
					{
						objdata.status = 200;
						objdata.result = "Record Inserted Successfully !!!";
					}
					else
					{
						objdata.status = 200;
						objdata.result = "Record Inserted Successfully !!!";
					}

				}
                else
                {
                    objdata.status = 400;
                    objdata.result = "Failed to Insert Record, Please Try Again !!! ";
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }

            return objdata;
        }

        public DataTable transactionInsertion(transactionModel obj)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        cmd.Connection = con;
                        cmd.InitialLONGFetchSize = 1000;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GSWS_IN_DEPT_RESPONSES";
                        cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2).Value = obj.TYPE;
                        cmd.Parameters.Add("P_TRANSACTION_ID", OracleDbType.Varchar2).Value = obj.TXN_ID;
                        cmd.Parameters.Add("P_DEPT_TRANS_ID", OracleDbType.Varchar2).Value = obj.DEPT_TXN_ID;
                        cmd.Parameters.Add("P_DEPT_ID", OracleDbType.Varchar2).Value = obj.DEPT_ID;                       
                        cmd.Parameters.Add("P_BEN_TRANS_ID", OracleDbType.Varchar2).Value = obj.BEN_ID;
                        cmd.Parameters.Add("P_SERVICE_NAME", OracleDbType.Varchar2).Value = obj.SERVICE_NAME;
                        cmd.Parameters.Add("P_STATUS_CODE", OracleDbType.Varchar2).Value = obj.STATUS_CODE;
                        cmd.Parameters.Add("P_REMARKS", OracleDbType.Varchar2).Value = obj.REMARKS;
                        cmd.Parameters.Add("P_IP_ADDRESS", OracleDbType.Varchar2).Value = obj.IP_ADDRESS;
                        cmd.Parameters.Add("P_SYSTEM_NAME", OracleDbType.Varchar2).Value = obj.SYS_NAME;
                        cmd.Parameters.Add("P_DISTRICT_ID", OracleDbType.Varchar2).Value = obj.DISTRICT_ID;
                        cmd.Parameters.Add("P_MANDAL_ID", OracleDbType.Varchar2).Value = obj.MANDAL_ID;
                        cmd.Parameters.Add("P_GP_WARD_ID", OracleDbType.Varchar2).Value = obj.GP_WARD_ID;
                        cmd.Parameters.Add("P_GSWS_ID", OracleDbType.Varchar2).Value = obj.GSWS_ID;
                        cmd.Parameters.Add("P_SERVICE_ID", OracleDbType.Varchar2).Value = obj.SERVICE_ID;
                        cmd.Parameters.Add("P_LOGIN_USER", OracleDbType.Varchar2).Value = obj.LOGIN_USER;
                        cmd.Parameters.Add("P_TYPE_OF_REQUEST", OracleDbType.Varchar2).Value = obj.TYPE_OF_REQUEST;
                        cmd.Parameters.Add("P_HOD_ID", OracleDbType.Varchar2).Value = obj.HOD_ID;
                        cmd.Parameters.Add("P_URL_ID", OracleDbType.Varchar2).Value = obj.URL_ID;
						cmd.Parameters.Add("P_DESIGNATION_ID", OracleDbType.Varchar2).Value = obj.DESIGNATION_ID;
						cmd.Parameters.Add("P_CITIZEN_NAME", OracleDbType.Varchar2).Value = obj.CITIZENNAME;
						cmd.Parameters.Add("P_MOBILE",OracleDbType.Varchar2).Value = obj.MOBILENUMBER;
						cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        OracleDataAdapter oda = new OracleDataAdapter(cmd);
                        DataTable data = new DataTable();
						   oda.Fill(data);
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
				string mappath = HttpContext.Current.Server.MapPath("TransactionExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "error:transactionInsertion:"+ex.Message+":"+JsonConvert.SerializeObject(obj)));
				
				throw ex;
            }

        }

		public DataTable TransactionSchedule_TaskSP(ScheduleTransactionModel obj)
		{
			try
			{
				using (OracleConnection con = new OracleConnection(oradb))
				{
					using (OracleCommand cmd = new OracleCommand())
					{
						cmd.Connection = con;
						cmd.InitialLONGFetchSize = 1000;
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.CommandText = "GSWS_IN_DEPT_RESP_TRACKING";
						cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2).Value = obj.TYPE;
						cmd.Parameters.Add("P_GSWS_TXN_ID", OracleDbType.Varchar2).Value = obj.GSWS_TRANS_ID;
						cmd.Parameters.Add("P_DEPT_APPLICATION_ID", OracleDbType.Varchar2).Value = obj.DEPARTMENT_APPLICATION_ID;
						cmd.Parameters.Add("P_DEPT_TXN_ID", OracleDbType.Varchar2).Value = obj.DEPARTMENT_Transaction_ID;
						cmd.Parameters.Add("P_SERVICE_NAME", OracleDbType.Varchar2).Value = obj.SERVICE_NAME;
						cmd.Parameters.Add("P_STATUS_CODE", OracleDbType.Varchar2).Value = obj.STATUS_CODE;
						cmd.Parameters.Add("P_STATUS_MESSAGE", OracleDbType.Varchar2).Value = obj.STATUS_MESSAGE;
						cmd.Parameters.Add("P_CITIZEN_NAME", OracleDbType.Varchar2).Value = obj.CITIZEN_NAME;
						cmd.Parameters.Add("P_GENDER", OracleDbType.Varchar2).Value = obj.GENDER;
						cmd.Parameters.Add("P_DISTRICT", OracleDbType.Varchar2).Value = obj.DISTRICT;
						cmd.Parameters.Add("P_VILLAGE", OracleDbType.Varchar2).Value = obj.VILLAGE;
						cmd.Parameters.Add("P_TXN_UPDATED_DATE", OracleDbType.Date).Value = string.IsNullOrEmpty(obj.APPLICATION_LAST_UPDATE_DATE) ? DateTime.Now : GetDateConvert(obj.APPLICATION_LAST_UPDATE_DATE);
						cmd.Parameters.Add("P_MANDAL", OracleDbType.Varchar2).Value = obj.MANDAL;
						cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
						OracleDataAdapter oda = new OracleDataAdapter(cmd);
						DataTable data = new DataTable();
						oda.Fill(data);
						return data;
						
					}
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("TransactionExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "error:transactionInsertion:" + ex.Message + ":" + JsonConvert.SerializeObject(obj)));

				throw ex;
			}

		}
		public DateTime GetDateConvert(string strdate)
		{
			try
			{
				strdate = strdate.Replace("/", "-");
				DateTime dateTime = DateTime.ParseExact(strdate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
				return dateTime;
			}
			catch (Exception ex)
			{
				return DateTime.Parse(strdate, CultureInfo.InvariantCulture);
			}
		}

		public dynamic authenticateUserSendOTP(userAuthenticationModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.TYPE = "2";
                DataTable dt = autheticationProcedure(obj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    bool test = false;
                    int OTP = 0;
                    if (dt.Rows[0]["TEST"].ToString() == "1")
                    {
                        test = true;
                        OTP = 123456;
                    }
                    else
                    {
                        Random obj_random = new Random();
                        OTP = obj_random.Next(111111, 999999);
                    }
                    if(send_otp_using_mobile_number(dt.Rows[0]["MOBILE_NUMBER"].ToString(), OTP, "0", obj.PS_ID, obj.EMP_ID, test))
                    {
                        objdata.status = 200;
                        objdata.result = "OTP Successfully sent to registered moblie number";
                    }
                    else
                    {
                        objdata.status = 400;
                        objdata.result = "OTP Sending Failed,Please try again...";
                    }
                }
                else
                {
                    objdata.status = 400;
                    objdata.result = "No Mobile Number found to send OTP Please contact administrator";
                }

            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }

            return objdata;

        }

        public bool send_otp_using_mobile_number(string mobile_number, int random_number, string otp_type, string gsws_id, string emp_id, bool test)
        {
            SMSService.SMSService obj = new SMSService.SMSService();
            string message = "PRAJA SECRETARIAT SERVICES OTP : " + random_number;
            try
            {
                con.Open();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandText = @"INSERT INTO GSWS_AUTH_OTP_LOG (MOBILE_NUM,OTP,OTP_TYPE,GSWS_ID,EMP_ID) VALUES(:mobile_number,:otp,:OTP_TYPE,:GSWS_ID,:EMP_ID)";
                cmd.Parameters.Add(":mobile_number", OracleDbType.Varchar2).Value = mobile_number;
                cmd.Parameters.Add(":otp", OracleDbType.Varchar2).Value = random_number;
                cmd.Parameters.Add(":OTP_TYPE", OracleDbType.Varchar2).Value = otp_type;
                cmd.Parameters.Add(":GSWS_ID", OracleDbType.Varchar2).Value = gsws_id;
                cmd.Parameters.Add(":EMP_ID", OracleDbType.Varchar2).Value = emp_id;
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    if (!test)
                    {
                        string response = obj.SendSMS(mobile_number, message);
                        if (response.Contains("402"))
                            return true;
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable autheticationProcedure(userAuthenticationModel obj)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        cmd.Connection = con;
                        cmd.InitialLONGFetchSize = 1000;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "gsws_sp_auth_otp_log";
                        cmd.Parameters.Add("ftype", OracleDbType.Varchar2).Value = obj.TYPE;
                        cmd.Parameters.Add("fmobile_number", OracleDbType.Varchar2).Value = obj.MOBILE_NUMBER;
                        cmd.Parameters.Add("fotp", OracleDbType.Varchar2).Value = obj.OTP;
                        cmd.Parameters.Add("fgsws_id", OracleDbType.Varchar2).Value = obj.PS_ID;
                        cmd.Parameters.Add("femp_id", OracleDbType.Varchar2).Value = obj.EMP_ID;
                        cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        OracleDataAdapter oda = new OracleDataAdapter(cmd);
                        DataTable data = new DataTable();
                        oda.Fill(data);
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public dynamic authenticateUserVerifyOTP(userAuthenticationModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.TYPE = "1";
                DataTable dt = autheticationProcedure(obj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    try
                    {
                        con.Open();
                        OracleCommand cmd = con.CreateCommand();
                        cmd.CommandText = @"UPDATE GSWS_AUTH_OTP_LOG SET OTP_TYPE=1 where GSWS_ID=:GSWS_ID and EMP_ID=:EMP_ID";
                        cmd.Parameters.Add(":GSWS_ID", OracleDbType.Varchar2).Value = obj.PS_ID;
                        cmd.Parameters.Add(":EMP_ID", OracleDbType.Varchar2).Value = obj.EMP_ID;
                        cmd.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {

                    }
                    objdata.status = 200;
                    objdata.result = "OTP Verified Successfully";


                }
                else
                {
                    objdata.status = 400;
                    objdata.result = "Invalid OTP, Please Enter Valid OTP";
                }

            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }

            return objdata;

        }

        public dynamic authenticatedUserDetails(userAuthenticationModel obj)
        {
            dynamic objdata = new ExpandoObject();

            //string json = JsonConvert.SerializeObject(obj);

            //string iv = CryptLib.GenerateRandomIV(16);
            //string key = CryptLib.getHashSha256(obj.DeptCode, 31);

            //string encrypttext = new CryptLib().encrypt(json, key, iv);


            try
            {
                string decrypted_text = string.Empty;
                if (string.IsNullOrEmpty(obj.ENCRYPTED_DATA) || string.IsNullOrEmpty(obj.KEY) || string.IsNullOrEmpty(obj.IV))
                {
                    objdata.status = 400;
                    objdata.result = null;
                }
                else
                {
					decrypted_text = EncryptDecryptAlgoritham.DecryptStringAES(obj.ENCRYPTED_DATA, obj.KEY, obj.IV);//new CryptLib().decrypt(obj.ENCRYPTED_DATA, obj.KEY, obj.IV);
					objdata.status = 200;
                    objdata.result = decrypted_text;
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex;
            }

            return objdata;
        }

		public string SavePaymentConfirmation(RootMeesevaObject objroot)
		{
			try
			{
				DataTable dt = MeesevaPaymentRequest_SP(objroot);
				if (dt != null && dt.Rows.Count > 0)
				{
					return "0#Success#" + dt.Rows[0][0].ToString();
				}
				else
				{
					return "2#Failure#Invalid Request";
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("MeesevaExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "SavePaymentConfirmation:"+ex.Message.ToString()));
				return "2#Failure#"+ex.Message.ToString();
			}
		}

		public dynamic SavePaymentResponse(RootMeesevaResponseObject objres)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				DataTable dt = Meeseva_Response_SP(objres);
				if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString()=="1")
				{
					obj.Status = "100";
					obj.Reason = "Data Submitted Successfully";

				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("MeesevaExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "SavePaymentResponse:"+ex.Message.ToString()));
				obj.Status = "102";
				obj.Reason = ex.Message.ToString();
			}
			return obj;
		}
		public DataTable MeesevaPaymentRequest_SP(RootMeesevaObject objmeeseva)
		{
			OracleCommand cmd = new OracleCommand();
			try
			{
				objmeeseva.PAYMENTTRANSID="GSWS" + DateTime.Now.ToString("ddMMyyyyHHmmss") + new Random().Next(1000, 9999).ToString();
				con = new OracleConnection(oradb);
				cmd.Connection = con;
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_IN_MEESEVA_PMT_REQ";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2).Value = "1";
				cmd.Parameters.Add("PUNIQUE_ID", OracleDbType.Varchar2).Value = objmeeseva.UNIQUENO;
				cmd.Parameters.Add("PSCA_USER_ID", OracleDbType.Varchar2).Value = objmeeseva.SCAUSERID;
				cmd.Parameters.Add("PPAYMENT_MODE", OracleDbType.Varchar2).Value = objmeeseva.PAYMENTMODE;
				cmd.Parameters.Add("POPERATOR_ID", OracleDbType.Varchar2).Value = objmeeseva.OPERATORID;
				cmd.Parameters.Add("PCHANNEL_ID", OracleDbType.Varchar2).Value = objmeeseva.CHANNELID;
				cmd.Parameters.Add("PMEE_SEVA_APP_NO", OracleDbType.Varchar2).Value = objmeeseva.MeeSevaAppNo;
				cmd.Parameters.Add("PREQUEST_ID", OracleDbType.Varchar2).Value = objmeeseva.REQUESTID;
				cmd.Parameters.Add("PSERVICE_ID", OracleDbType.Varchar2).Value = objmeeseva.SERVICEID;
				cmd.Parameters.Add("PTRANSAC_PARAMS_KEY", OracleDbType.Varchar2).Value = objmeeseva.TRANSACTIONPARAMSDESC;
				cmd.Parameters.Add("PTRANSAC_PARAMS_VALUE", OracleDbType.Varchar2).Value = objmeeseva.TRANSACTIONDETAILS;
				cmd.Parameters.Add("PUSER_CHARGES", OracleDbType.Varchar2).Value = objmeeseva.ARRAMOUNT.Split('#')[0]; //usercharge
				cmd.Parameters.Add("PSERVICE_CHARGES", OracleDbType.Varchar2).Value = objmeeseva.ARRAMOUNT.Split('#')[1]; //usercharge		
				cmd.Parameters.Add("PCHALLAN_AMOUNT", OracleDbType.Varchar2).Value = objmeeseva.ARRAMOUNT.Split('#')[3];
				cmd.Parameters.Add("PPOSTAL_CHARGES", OracleDbType.Varchar2).Value = objmeeseva.ARRAMOUNT.Split('#')[2];				
				cmd.Parameters.Add("PPRINT_CHARGES", OracleDbType.Varchar2).Value = objmeeseva.ARRAMOUNT.Split('#').Length >4 ? objmeeseva.ARRAMOUNT.Split('#')[4]:"0" ;
				cmd.Parameters.Add("PSUR_CHARGES", OracleDbType.Varchar2).Value = objmeeseva.ARRAMOUNT.Split('#').Length > 5 ?  objmeeseva.ARRAMOUNT.Split('#')[5]: "0";
				cmd.Parameters.Add("PCARD_TRANS_ID", OracleDbType.Varchar2).Value = objmeeseva.CARDTRANSID;
				cmd.Parameters.Add("PCARD_AUTH_CODE", OracleDbType.Varchar2).Value = objmeeseva.CARDAUTHCODE;
				cmd.Parameters.Add("PPAYMENT_TRANSID", OracleDbType.Varchar2).Value = objmeeseva.PAYMENTTRANSID;

				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				OracleDataAdapter oda = new OracleDataAdapter(cmd);
				DataTable data = new DataTable();
				oda.Fill(data);
				return data;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				con.Close();
				cmd.Dispose();
			}
		}


		public DataTable Meeseva_Response_SP(RootMeesevaResponseObject objmeeseva)
		{
			OracleCommand cmd = new OracleCommand();
			try
			{
				con = new OracleConnection(oradb);
				cmd.Connection = con;
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_IN_MEESEVA_PMT_RESP";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2).Value = "1";
				cmd.Parameters.Add("PTRANS_ID", OracleDbType.Varchar2).Value = objmeeseva.TRANSID;
				cmd.Parameters.Add("PMEESEVA_APPLICATION_NO", OracleDbType.Varchar2).Value = objmeeseva.MEESEVAAPPLNO;
				cmd.Parameters.Add("PREQUEST_ID", OracleDbType.Varchar2).Value = objmeeseva.REQUESTID;
				cmd.Parameters.Add("PERROR_NO", OracleDbType.Varchar2).Value = objmeeseva.ERRORNO;
				cmd.Parameters.Add("PERROR_MESSAGE", OracleDbType.Varchar2).Value = objmeeseva.ERRORMESSAGE;
				cmd.Parameters.Add("PRECEIPT_PARAM_KEY", OracleDbType.Varchar2).Value = objmeeseva.RECEIPTPARAMDESC;
				cmd.Parameters.Add("PRECEIPT_PARAM_VALUE", OracleDbType.Varchar2).Value = objmeeseva.RECEIPTDETAILS;			
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				OracleDataAdapter oda = new OracleDataAdapter(cmd);
				DataTable data = new DataTable();
				oda.Fill(data);
				return data;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				con.Close();
				cmd.Dispose();
			}
		}

		public string GetInputJsonFormat(transactionModel objtrans)
		{
			InputJsonClass objdata = new InputJsonClass();
			if (objtrans.HOD_ID == "1601")
			{
				if (objtrans.DISTRICT_ID == "505" || objtrans.DISTRICT_ID == "519" || objtrans.DISTRICT_ID == "520" || objtrans.DISTRICT_ID == "521" || objtrans.DISTRICT_ID == "523")
				{
					objdata.USERNAME = "EP_PS_INTEGRATION"; objdata.PASSWORD = "Ep@QAZ#wsx"; objdata.PS_TXN_ID = objtrans.TXN_ID; objdata.RETURN_URL = "http://gramawardsachivalayam.ap.gov.in/GSWS/#!/Login";
				}
				else
				{
					objdata.USERNAME = "EP_PS_INTEGRATION"; objdata.PASSWORD = "Ep@QAZ#wsx"; objdata.PS_TXN_ID = objtrans.TXN_ID; objdata.RETURN_URL = "http://gramawardsachivalayam.ap.gov.in/GSWS/#!/Login";
				}
				return JsonConvert.SerializeObject(objdata);
			}
			
			else if (objtrans.HOD_ID == "3501" || objtrans.HOD_ID == "3101" || objtrans.URL_ID == "110102401" || objtrans.URL_ID == "330104101" || objtrans.HOD_ID == "2701" || objtrans.HOD_ID == "2703")
			{
				objdata.USERNAME = "admin"; objdata.PASSWORD = "admin@123"; objdata.PS_TXN_ID = objtrans.TXN_ID; objdata.RETURN_URL = "http://gramawardsachivalayam.ap.gov.in/GSWS/#!/Login";
			}
			else if (objtrans.HOD_ID == "1101")
			{
				objdata.USERNAME = "9666445262"; objdata.PASSWORD = "666556"; objdata.PS_TXN_ID = objtrans.TXN_ID; objdata.RETURN_URL = "http://gramawardsachivalayam.ap.gov.in/GSWS/#!/Login";

			}
			else if (objtrans.URL_ID == "200300101")
			{
				objdata.USERNAME = "9121280704"; objdata.PASSWORD = "12354"; objdata.PS_TXN_ID = objtrans.TXN_ID; objdata.RETURN_URL = "http://gramawardsachivalayam.ap.gov.in/GSWS/#!/Login";

			}
			else if (objtrans.HOD_ID =="2002" )
			{
				objdata.USERNAME = objtrans.LOGIN_USER; objdata.PASSWORD = objtrans.LOGIN_USER; objdata.PS_TXN_ID = objtrans.TXN_ID; objdata.RETURN_URL = "http://gramawardsachivalayam.ap.gov.in/GSWS/#!/Login";

			}
			else if (objtrans.DEPT_ID == "13" || objtrans.DEPT_ID == "20" || objtrans.DEPT_ID == "28" || objtrans.DEPT_ID == "36" || objtrans.DEPT_ID == "38")
			{
				objdata.USERNAME = "jnbAdmin"; objdata.PASSWORD = "jnb@dmin2019"; objdata.PS_TXN_ID = objtrans.TXN_ID; objdata.RETURN_URL = "http://gramawardsachivalayam.ap.gov.in/GSWS/#!/Login";

			}
			else if (objtrans.HOD_ID == "2201")
			{
				objdata.USERNAME = objtrans.LOGIN_USER; objdata.PASSWORD = "f542540f719f46833b51ce88d8cc5112"; objdata.PS_TXN_ID = objtrans.TXN_ID; objdata.RETURN_URL = "http://gramawardsachivalayam.ap.gov.in/GSWS/#!/Login";

			}
			else if (objtrans.URL_ID == "310101401" || objtrans.URL_ID == "310101404")
			{
				objdata.USERNAME = "10890152"; objdata.PASSWORD = "10890152"; objdata.PS_TXN_ID = objtrans.TXN_ID; objdata.RETURN_URL = "http://gramawardsachivalayam.ap.gov.in/GSWS/#!/Login";

			}
			else if (objtrans.URL_ID == "370300102")
			{
				objdata.USERNAME = "sreenulakkakula"; objdata.PASSWORD = "sreenu@12"; objdata.PS_TXN_ID = objtrans.TXN_ID; objdata.RETURN_URL = "http://gramawardsachivalayam.ap.gov.in/GSWS/#!/Login";

			}
			else if (objtrans.URL_ID == "200301401" || objtrans.URL_ID == "360201701" || objtrans.URL_ID == "360201401" || objtrans.URL_ID == "130101101" || objtrans.URL_ID == "280101201" || objtrans.URL_ID == "280101401" || objtrans.URL_ID == "280101301" || objtrans.URL_ID == "360201801" || objtrans.URL_ID == "3602018501" || objtrans.URL_ID == "170100102" || objtrans.URL_ID == "130101401" || objtrans.URL_ID == "130101501" || objtrans.URL_ID =="240200101")
			{
				objdata.USERNAME = objtrans.LOGIN_USER; objdata.PASSWORD = objtrans.LOGIN_USER; objdata.PS_TXN_ID = objtrans.TXN_ID; objdata.RETURN_URL = "http://gramawardsachivalayam.ap.gov.in/GSWS/#!/Login";
			}
			else
			{
				objdata.USERNAME = objtrans.LOGIN_USER; objdata.PASSWORD = objtrans.LOGIN_USER; objdata.PS_TXN_ID = objtrans.TXN_ID; objdata.RETURN_URL = "http://gramawardsachivalayam.ap.gov.in/GSWS/#!/Login";
			}
			
			return JsonConvert.SerializeObject(objdata); ;

		}
	}
}
