using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System.Dynamic;
using Oracle.ManagedDataAccess.Client;
namespace gswsBackendAPI.MeesevaService.Backend
{
    public class EncryptMeeseva: CommonSPHel
	{

		//MeesevaWebUAT.MeeSevaWebService _objmweb = new MeesevaWebUAT.MeeSevaWebService();
		MeesevaProductionService.MeeSevaWebService _objmweb = new MeesevaProductionService.MeeSevaWebService();
        public dynamic MeesevaEncryptData(MeesevaModel obj2)
        {
            try
            {

                var data =_objmweb.VSWS_GETTOKEN("VSWS-APTS", "P$W$@13112019"); //[{"Status":"100","token":"asd$#@4568"}]

				
				var data2 =JsonConvert.DeserializeObject<dynamic>(data);
				//_objmweb.VSWS_GETAPPDETAILS("VSWS-APTS", "P$W$@13112019", data2[0].token,obj2.PARAM1);
				MeesevaModel _OBJMES = new MeesevaModel();
                if (data2[0].Status == "100")
                {
                    _OBJMES.STATUS = "100";
                    _OBJMES.TOKEN = data2[0].token;
                    _OBJMES.CHANNELID = "CODETREE";
                    _OBJMES.LANDINGID = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999).ToString();
					_OBJMES.SCAID = "35";
                    _OBJMES.OPERATORID = "CODETREE-1";
                    _OBJMES.OPERATOR_UNIQUENO = "CCSP35CODETREE1";
					_OBJMES.SERVICEID = obj2.SERVICEID;//"818";
					_OBJMES.SECRETARIATCODE = obj2.SECRETARIATCODE;
					string strjson = _OBJMES.TOKEN + "|" + _OBJMES.LANDINGID + "|" + _OBJMES.SCAID + "|" + _OBJMES.CHANNELID + "|" + _OBJMES.OPERATORID + "|" + _OBJMES.OPERATOR_UNIQUENO + "|" + _OBJMES.SERVICEID;

                    //TripleDESCryptoServiceProvider _objtds = new TripleDESCryptoServiceProvider();
                    byte[] key = { 0xA2, 0x15, 0x37, 0x07, 0xCB, 0x62, 0xC1, 0xD3, 0xF8, 0xF1, 0x97, 0xDF, 0xD0, 0x13, 0x4F, 0x79, 0x01, 0x67, 0x7A, 0x85, 0x94, 0x16, 0x31, 0x92 };
                    byte[] iv = { 50, 51, 52, 53, 54, 55, 56, 57 };
                    string encdata = new cTripleDES(key, iv).Encrypt(strjson);
					string serialized_data = JsonConvert.SerializeObject(_OBJMES);
					string mappath = HttpContext.Current.Server.MapPath("MeesevaInitiateLogs");
					Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, serialized_data));
					GetMeesevaInitiate(_OBJMES);

					_OBJMES.ENCDATA = encdata;
                    return _OBJMES;
                }
                else
                {
                    _OBJMES.STATUS = "102";
                    _OBJMES.REASON = "Invalid Request";
                    return _OBJMES;

                }

            }
            catch(Exception ex)
            {
				string mappath = HttpContext.Current.Server.MapPath("MeesevaInitiateExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, ex.Message.ToString()));
				throw ex;
            }
        }


		public dynamic MeesevaVROEncryptData(MeesevaModel obj2)
		{
			try
			{

				var data = _objmweb.VSWS_GETTOKEN("VSWS-APTS", "P$W$@13112019");//VSWS_GETTOKEN("VSWS-APTS", "P$W$@13112019"); //[{"Status":"100","token":"asd$#@4568"}]


				var data2 = JsonConvert.DeserializeObject<dynamic>(data);
				
				//_objmweb.VSWS_GETAPPDETAILS("VSWS-APTS", "P$W$@13112019", data2[0].token,obj2.PARAM1);
				MeesevaModel _OBJMES = new MeesevaModel();
				if (data2[0].status == "100")
				{
					_OBJMES.STATUS = "100";
					_OBJMES.TOKEN = data2[0].token;					
					_OBJMES.LANDINGID = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999).ToString();
					_OBJMES.OPERATORID = obj2.OPERATORID;
					_OBJMES.SECRETARIATCODE = obj2.SECRETARIATCODE;


					string strjson = _OBJMES.TOKEN + "|" + _OBJMES.LANDINGID + "|" + _OBJMES.OPERATORID;

					//TripleDESCryptoServiceProvider _objtds = new TripleDESCryptoServiceProvider();
					byte[] key = { 0xA2, 0x15, 0x37, 0x07, 0xCB, 0x62, 0xC1, 0xD3, 0xF8, 0xF1, 0x97, 0xDF, 0xD0, 0x13, 0x4F, 0x79, 0x01, 0x67, 0x7A, 0x85, 0x94, 0x16, 0x31, 0x92 };
					byte[] iv = { 50, 51, 52, 53, 54, 55, 56, 57 };
					string encdata = new cTripleDES(key, iv).Encrypt(strjson);
					string serialized_data = JsonConvert.SerializeObject(_OBJMES);
					string mappath1 = HttpContext.Current.Server.MapPath("MeesevaInitiateLogs");
					Task WriteTask1 = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath1, serialized_data));
					GetMeesevaInitiate(_OBJMES);

					_OBJMES.ENCDATA = encdata;
					return _OBJMES;
				}
				else
				{
					_OBJMES.STATUS = "102";
					_OBJMES.REASON = "Invalid Request";
					return _OBJMES;

				}

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("MeesevaInitiateExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, ex.Message.ToString()));
				throw ex;
			}
		}
	
		public dynamic MeesevaHousesiteVROEncryptData(MeesevaModel obj2)
		{
			try
			{
				HousesiteWebservice.API _obj = new HousesiteWebservice.API();
				var data = _obj.GetTokenSSO("VSWS-HOUSING", "#@pts@04062020");//VSWS_GETTOKEN("VSWS-APTS", "P$W$@13112019"); //[{"Status":"100","token":"asd$#@4568"}]


				var data2 = JsonConvert.DeserializeObject<dynamic>(data);

				//_objmweb.VSWS_GETAPPDETAILS("VSWS-APTS", "P$W$@13112019", data2[0].token,obj2.PARAM1);
				MeesevaHOuseSitesModel _OBJMES = new MeesevaHOuseSitesModel();
				if (data2[0].status == "100")
				{
					_OBJMES.STATUS = "100";
					_OBJMES.TOKEN = data2[0].token;
					_OBJMES.LANDINGID = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999).ToString();
					_OBJMES.USERID = obj2.OPERATORID;
					//_OBJMES.SECRETARIATCODE = obj2.SECRETARIATCODE;


					string strjson = _OBJMES.TOKEN + "|" + _OBJMES.LANDINGID + "|" + _OBJMES.USERID;

					//TripleDESCryptoServiceProvider _objtds = new TripleDESCryptoServiceProvider();
					byte[] key = { 0xA2, 0x15, 0x37, 0x07, 0xCB, 0x62, 0xC1, 0xD3, 0xF8, 0xF1, 0x97, 0xDF, 0xD0, 0x13, 0x4F, 0x79, 0x01, 0x67, 0x7A, 0x85, 0x94, 0x16, 0x31, 0x92 };
					byte[] iv = { 50, 51, 52, 53, 54, 55, 56, 57 };
					string encdata = new cTripleDES(key, iv).Encrypt(strjson);
					string serialized_data = JsonConvert.SerializeObject(_OBJMES);
					string mappath1 = HttpContext.Current.Server.MapPath("MeesevaInitiateLogs");
					Task WriteTask1 = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath1, serialized_data));
					//GetMeesevaInitiate(_OBJMES);
					_OBJMES.RedirectUrl = "http://navaratnalu-housesites1.ap.gov.in/GVSPortal/UserInterface/VSWSRedirection.aspx";
					_OBJMES.ENCDATA = encdata;
					return _OBJMES;
				}
				else
				{
					_OBJMES.STATUS = "102";
					_OBJMES.REASON = "Invalid Request";
					return _OBJMES;

				}

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("MeesevaInitiateExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, ex.Message.ToString()));
				throw ex;
			}
		}
		public dynamic GetMeesevaApplicationStatus(MeesevaModel objmeeseva)
		{
			try
			{
				var data = _objmweb.VSWS_GETTOKEN("VSWS-APTS", "P$W$@13112019"); //[{"Status":"100","token":"asd$#@4568"}]


				var data2 = JsonConvert.DeserializeObject<dynamic>(data);
				//
				MeesevaModel _OBJMES = new MeesevaModel();
				if (data2[0].status == "100")
				{
					//var result = _objmweb.VSWS_GETAPPDETAILS("VSWS-APTS", "P$W$@13112019", data2[0].token.ToString(), objmeeseva.PARAM1);
					var result = _objmweb.VSWS_GETAPPDETAILS("VSWS-APTS", "P$W$@13112019", data2[0].token.ToString(), objmeeseva.PARAM1);
					string mappath = HttpContext.Current.Server.MapPath("MeesevaStatusLogs");
					Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, result));
					//string json = JsonConvert.SerializeObject(result);
					return  JsonConvert.DeserializeObject<dynamic>(result);
					//return result;
				}
				else
				{

					_OBJMES.STATUS = "102";
					_OBJMES.REASON = "Invalid Token Data";
					return _OBJMES;
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("GetMeesevaApplicationStatusExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, ex.Message.ToString()));
				throw ex;
				
			}
		}

		public dynamic GetMeesevaAppStatucData(MEESEVASTATUSMODEL objStatus)
		{
			dynamic objdata = new ExpandoObject();
			try
			{
				DataTable dt1 = GetMeesevaStatusCheck_SP(objStatus);
				if (dt1 != null && dt1.Rows.Count > 0)
				{
					objdata.Status = 100;
					objdata.StatusList = dt1;
					objdata.Reason = "";
				}
				else
				{
					objdata.Status = 102;
					objdata.StatusList = new DataTable();
					objdata.Reason = "This Application Number Data is Not Available";
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("MeesevaInitiateExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, ex.Message.ToString()));

				objdata.Status = 102;
				objdata.StatusList = new DataTable();
				objdata.Reason = ErrMessage;
			}
			return objdata;
		}
		public DataTable GetMeesevaInitiate(MeesevaModel obj3)
		{
			OracleCommand cmd = new OracleCommand();
			
			try
			{
								
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_IN_MEESEVA_REQ_INIT";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2).Value = "1";
				cmd.Parameters.Add("PTOKEN", OracleDbType.Varchar2).Value = obj3.TOKEN;
				cmd.Parameters.Add("PCHANNELID", OracleDbType.Varchar2).Value = obj3.CHANNELID;
				cmd.Parameters.Add("PLANDINGID", OracleDbType.Varchar2).Value = obj3.LANDINGID;
				cmd.Parameters.Add("PSCAID", OracleDbType.Varchar2).Value = obj3.SCAID;
				cmd.Parameters.Add("POPERATORID", OracleDbType.Varchar2).Value = obj3.OPERATORID;
				cmd.Parameters.Add("POPERATOR_UNIQUENO", OracleDbType.Varchar2).Value = obj3.OPERATOR_UNIQUENO;
				cmd.Parameters.Add("PSERVICE_ID", OracleDbType.Varchar2).Value = obj3.SERVICEID;
				cmd.Parameters.Add("GSWS_CODE", OracleDbType.Varchar2).Value = obj3.SECRETARIATCODE;
				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dt = GetgswsDataAdapter(cmd);
				return dt;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		public DataTable GetMeesevaStatusCheck_SP(MEESEVASTATUSMODEL obj3)
		{
			OracleCommand cmd = new OracleCommand();

			try
			{

				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_MEESEVA_STATUS_CHECK";
				cmd.Parameters.Add("ptype", OracleDbType.Varchar2).Value = obj3.Ftype;
				cmd.Parameters.Add("pDEPTRECIEPTCODE", OracleDbType.Varchar2).Value = obj3.DeptrecieptCode;
				cmd.Parameters.Add("pstatus_code", OracleDbType.Varchar2).Value = obj3.Statuscode;
				cmd.Parameters.Add("pstatus_msg", OracleDbType.Varchar2).Value = obj3.Statusmsg;
				cmd.Parameters.Add("p_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dt = GetgswsDataAdapter(cmd);
				return dt;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
    public class cTripleDES
    {
        // define the triple des provider
        private TripleDESCryptoServiceProvider m_des =
                 new TripleDESCryptoServiceProvider();

        // define the string handler
        private UTF8Encoding m_utf8 = new UTF8Encoding();

        // define the local property arrays
        private byte[] m_key;
        private byte[] m_iv;

        public cTripleDES(byte[] key, byte[] iv)
        {
            this.m_key = key;
            this.m_iv = iv;
        }

        public byte[] Encrypt(byte[] input)
        {
            return Transform(input,
                   m_des.CreateEncryptor(m_key, m_iv));
        }

        public byte[] Decrypt(byte[] input)
        {
            return Transform(input,
                   m_des.CreateDecryptor(m_key, m_iv));
        }

        public string Encrypt(string text)
        {
            byte[] input = m_utf8.GetBytes(text);
            byte[] output = Transform(input,
                            m_des.CreateEncryptor(m_key, m_iv));
            return Convert.ToBase64String(output);
        }

        public string Decrypt(string text)
        {
            byte[] input = Convert.FromBase64String(text);
            byte[] output = Transform(input,
                            m_des.CreateDecryptor(m_key, m_iv));
            return m_utf8.GetString(output);
        }

        private byte[] Transform(byte[] input,
                       ICryptoTransform CryptoTransform)
        {
            // create the necessary streams
            MemoryStream memStream = new MemoryStream();
            CryptoStream cryptStream = new CryptoStream(memStream,
                         CryptoTransform, CryptoStreamMode.Write);
            // transform the bytes as requested
            cryptStream.Write(input, 0, input.Length);
            cryptStream.FlushFinalBlock();
            // Read the memory stream and
            // convert it back into byte array
            memStream.Position = 0;
            byte[] result = memStream.ToArray();
            // close and release the streams
            memStream.Close();
            cryptStream.Close();
            // hand back the encrypted buffer
            return result;
        }
    
}
}


