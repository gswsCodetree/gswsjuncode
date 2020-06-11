using gswsBackendAPI.DL.CommonHel;
using gswsBackendAPI.walletOnePaymentService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using System.Data;
using gswsBackendAPI.DL.DataConnection;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;

namespace gswsBackendAPI.Payments.Backend
{
    public class paymentHelper
    {

        RijndaelManaged sessionKey = new RijndaelManaged();
        static ServicesRWMS servicesRWMS = new ServicesRWMS();
        LoginHelper _LoginHel = new LoginHelper();

        public dynamic OrderDetails(paymentModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.orderId = "80120201019362644";

                orderDetailsModel rootobj = new orderDetailsModel();
                rootobj.Amount = obj.Amount;
                rootobj.Description = obj.Description;
                rootobj.mobileNumber = obj.mobileNumber;
                rootobj.orderId = obj.orderId;
                rootobj.totalAmount = obj.totalAmount;
                rootobj.TxnDate = obj.TxnDate;
                rootobj.userCharges = obj.userCharges;
                rootobj.userName = obj.userName;
                rootobj.walletType = "";


                DataTable dt1 = gswsPaymentRequestProc(obj, "4", "");
                obj.gswsCode = dt1.Rows[0][0].ToString();
                obj.UniqueTxnId = obj.merchantId + obj.mobileNumber + DateTime.Now.ToString("yyyyMMddhhmmssmm");

                string json = JsonConvert.SerializeObject(obj);

                string iv = CryptLib.GenerateRandomIV(16);
                string key = CryptLib.getHashSha256("GSWS TEST", 32);

                string encrypttext = EncryptDecryptAlgoritham.EncryptStringAES(json, key, iv);

                obj.encrypttext = encrypttext;
                obj.iv = iv;

                rootobj.encrypttext = obj.encrypttext;
                rootobj.iv = obj.iv;


                if (obj.gswsCode == "10690589" || obj.gswsCode == "10690588" || obj.gswsCode == "10690590" || obj.gswsCode == "21073101")
                {
                    rootobj.walletType = "TA";
                }
                else if (obj.gswsCode == "10690567" || obj.gswsCode == "10690568" || obj.gswsCode == "21073097" || obj.gswsCode == "21073098" || obj.gswsCode == "21073099")
                {
                    rootobj.walletType = "WONE";
                }

                else if (obj.gswsCode == "10690581" || obj.gswsCode == "10690561" || obj.gswsCode == "10690574" || obj.gswsCode == "10690572" || obj.gswsCode == "10690573" || obj.gswsCode == "10690582" || obj.gswsCode == "21073095" || obj.gswsCode == "21073096" || obj.gswsCode == "21073082" || obj.gswsCode == "21073083" || obj.gswsCode == "21073085" || obj.gswsCode == "21073088")
                {
                    rootobj.walletType = "APW";
                }


                DataTable dt = gswsPaymentRequestProc(obj, "1", "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    token_gen.initialize();
                    token_gen.expiry_minutes = 60;
                    token_gen.addClaim("admin");
                    token_gen.PRIMARY_MACHINE_KEY = "10101010101010101010101010101010";
                    token_gen.SECONDARY_MACHINE_KEY = "1010101010101010";
                    token_gen.addResponse("status", "200");
                    token_gen.addResponse("result", JsonConvert.SerializeObject(rootobj));
                    return token_gen.generate_token();
                }
                else
                {
                    objdata.status = 400;
                    objdata.result = "Invalid Input";
                    string mappath = HttpContext.Current.Server.MapPath("gswsPaymentRequestProc.");
                    Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, JsonConvert.SerializeObject(obj)));
                }

            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public dynamic walletOnemakePayment(paymentModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {

                TransReqBean transReqBean = new TransReqBean();

                transReqBean.strPassWord = "GWSRWO@123";
                transReqBean.strUserId = "GWSRWO";
                transReqBean.strFStatus = "N";
                transReqBean.strPayMode = "C";
                transReqBean.strTransSt = "N";
                transReqBean.strAgType = "F";
                transReqBean.strConsName = obj.applicantName;
                transReqBean.strConsNo = obj.orderId;
                transReqBean.strDateTime = DateTime.Now.Date.ToString("dd-MM-yyyy"); // obj.application_date;
                transReqBean.strDeptCode = obj.deptCode;
                transReqBean.strDeptRcptDt = DateTime.Now.Date.ToString("dd-MM-yyyy"); // obj.department_application_date;
                transReqBean.strDeptRcptNo = obj.deptRecieptCode;
                transReqBean.strDistCode = obj.districtCode;
                transReqBean.strGWSCode = obj.gswsCode;
                transReqBean.strServCode = obj.serviceCode;
                transReqBean.strStaffCode = obj.staffCode;
                transReqBean.strTotAmt = obj.Amount;
                transReqBean.strTranDate = DateTime.Now.Date.ToString("dd-MM-yyyy"); // obj.transaction_date;
                transReqBean.strUserChrgs = obj.userCharges;

                DataTable dt = gswsPaymentRequestProc(obj, "2", "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    TransResBean transResBean = servicesRWMS.RWMSConfirmPayment(transReqBean);

                    gswsPaymentResponseModel resObj = new gswsPaymentResponseModel();

                    resObj.balance = "";
                    resObj.merchant_name = obj.merchantId;
                    resObj.message = transResBean.msg;
                    resObj.orderId = "";
                    resObj.responseCode = transResBean.errorCode;
                    resObj.status = transResBean.status;
                    resObj.txnRefNo = transResBean.strTransNo;
                    resObj.type = "2";
                    resObj.uniqueTxnId = obj.UniqueTxnId;

                    //DataTable resDt = gswsPaymentResponseProc(resObj);
                    //string mappath = HttpContext.Current.Server.MapPath("gswsPaymentResponseProc");
                    //Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, JsonConvert.SerializeObject(resObj)));

                    if (transResBean.errorCode == "0")
                    {
                        objdata.status = 200;
                        objdata.message = transResBean.msg;
                        objdata.transaction_no = transResBean.strTransNo;
                        objdata.callbackUrl = obj.callbackUrl;
                        objdata.deptOrderId = obj.deptRecieptCode;
                        objdata.txnId = obj.UniqueTxnId;
                        objdata.merchantId = obj.merchantId;
                    }

                    else
                    {
                        if (transResBean.errorCode == "1")
                            objdata.message = "invalid username or password";
                        else if (transResBean.errorCode == "2")
                            objdata.message = "user wallet not available";
                        else if (transResBean.errorCode == "3")
                            objdata.message = "duplicate application number";
                        else if (transResBean.errorCode == "4")
                            objdata.message = "required parameters are missing";
                        else if (transResBean.errorCode == "5")
                            objdata.message = transResBean.msg;
                        else if (transResBean.errorCode == "6")
                            objdata.message = "insufficient balance in account";
                        else
                            objdata.message = transResBean.msg;
                        objdata.status = transResBean.status;
                        objdata.transaction_no = transResBean.strTransNo;
                        objdata.callbackUrl = obj.callbackUrl;
                        objdata.deptOrderId = obj.deptRecieptCode;
                        objdata.txnId = obj.UniqueTxnId;
                        objdata.merchantId = obj.merchantId;
                    }


                }
                else
                {
                    objdata.status = 400;
                    objdata.result = "Invalid Input";
                    string mappath = HttpContext.Current.Server.MapPath("gswsPaymentRequestProc");
                    Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, JsonConvert.SerializeObject(obj)));
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.message = ex.Message.ToString();
            }

            return objdata;

        }

        public dynamic TAWalletMakePayment(paymentModel obj, string otp)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                string response = TAWalletPaymentGateWay(obj, otp);
                taWalletPaymentResponse responseObj = JsonConvert.DeserializeObject<taWalletPaymentResponse>(response);

                gswsPaymentResponseModel resObj = new gswsPaymentResponseModel();

                resObj.balance = responseObj.Balance;
                resObj.merchant_name = responseObj.Merchant_Name;
                resObj.message = responseObj.Message;
                resObj.orderId = responseObj.OrderID;
                resObj.responseCode = responseObj.Response_Code;
                resObj.status = "";
                resObj.txnRefNo = responseObj.Trxn_Ref_No;
                resObj.type = "1";
                resObj.uniqueTxnId = obj.UniqueTxnId;

                //DataTable resDt = gswsPaymentResponseProc(resObj);
                //string mappath = HttpContext.Current.Server.MapPath("gswsPaymentResponseProc");
                //Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, JsonConvert.SerializeObject(resObj)));



                if (responseObj.Response_Code.Length == 4)
                {
                    objdata.status = 200;
                    objdata.result = "Payment Successful";
                    objdata.callbackUrl = obj.callbackUrl;
                    objdata.reason = responseObj.Message;
                    objdata.deptOrderId = obj.deptRecieptCode;
                    objdata.txnId = obj.UniqueTxnId;
                    objdata.merchantId = obj.merchantId;
                }
                else
                {
                    objdata.status = 400;
                    objdata.result = "Payment Failed Please Try Again";
                    objdata.callbackUrl = obj.callbackUrl;
                    objdata.reason = responseObj.Message;
                    objdata.deptOrderId = obj.deptRecieptCode;
                    objdata.txnId = obj.UniqueTxnId;
                    objdata.merchantId = obj.merchantId;
                }

            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public string TAWalletPaymentGateWay(paymentModel obj, string otp)
        {

            //Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            //obj.orderId = unixTimestamp.ToString();
            //System.DateTime.Now.ToString()

            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
            "<Request type=\"DifMerchantPayments\" Terminal_Number=\"65\" Terminal_Name=\"TA\">" +
            "<Machine_Id>" + HttpUtility.HtmlEncode(obj.ipAddress) + "</Machine_Id>" +
            "<Mobile_num>" + HttpUtility.HtmlEncode(obj.mobileNumber) + "</Mobile_num>" +
            "<Order_Id>" + HttpUtility.HtmlEncode(obj.orderId) + "</Order_Id>" +
            "<Date>" + HttpUtility.HtmlEncode(DateTime.Now.ToString()) + "</Date>" +
            "<Amount>" + HttpUtility.HtmlEncode(obj.totalAmount) + "</Amount>" +
            "<MerchantCode>" + HttpUtility.HtmlEncode(obj.merchantId) + "</MerchantCode> " +
            "<Narration>" + HttpUtility.HtmlEncode(obj.Description) + "</Narration>" +
            "<BarcodeID></BarcodeID > " +
            "<QRCodeID></QRCodeID > " +
            "<OTP>" + HttpUtility.HtmlEncode(otp) + "</OTP > " +
            "<AgencyCode>900020</AgencyCode> " +
            "<Centre_Id>" + HttpUtility.HtmlEncode(obj.gswsCode) + "</Centre_Id>" +
            "</Request>";

            DataTable dt = gswsPaymentRequestProc(obj, "3", otp);
            if (dt != null && dt.Rows.Count > 0)
            {
                string url = "https://staging.transactionanalysts.com:444/TAAPI/process.aspx";
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                    string encxmldata = EncrptXML(xml);
                    byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes((encxmldata)); //HttpUtility.UrlEncode(xml_data)
                    req.Method = "POST";
                    req.Timeout = 300000;
                    req.ContentType = "text/xml";
                    req.ContentLength = requestBytes.Length;
                    Stream requestStream = req.GetRequestStream();
                    requestStream.Write(requestBytes, 0, requestBytes.Length);
                    requestStream.Close();
                    HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                    string Charset = res.CharacterSet;
                    Encoding encoding = Encoding.GetEncoding(Charset);
                    StreamReader sr = new StreamReader(res.GetResponseStream(), encoding);
                    //  StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.Default);

                    return sr.ReadToEnd();
                }
                catch (Exception error)
                {
                    return error.Message.ToString();
                }
            }
            string mappath = HttpContext.Current.Server.MapPath("gswsPaymentRequestProc.");
            Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, JsonConvert.SerializeObject(obj)));

            return "";

        }

        private string EncrptXML(string xml)
        {
            try
            {
                string skey_encryption = Encryption.GetEncryptedText(Convert.ToBase64String(sessionKey.Key),
                Decryption.Decrypt_usingpassword(System.Configuration.ConfigurationManager.AppSettings["TAPublic_Key"].ToString()));

                string encrypted_data = Encryption.AESEncryption(xml, Convert.ToBase64String(sessionKey.Key), false);


                XmlDocument Request_auth = new XmlDocument();

                XmlElement root_element = Request_auth.CreateElement("TA");
                Request_auth.AppendChild(root_element);

                XmlElement Data = Request_auth.CreateElement("Data");
                Data.InnerText = encrypted_data;
                root_element.AppendChild(Data);

                XmlElement Session_Key = Request_auth.CreateElement("S_KEY");
                Session_Key.InnerText = skey_encryption;
                root_element.AppendChild(Session_Key);


                return "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>" + Request_auth.OuterXml;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public dynamic TAWalletEncryptedKey(TAWalletpaymentModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                if (string.IsNullOrEmpty(obj.user_id) || string.IsNullOrEmpty(obj.service_name) || string.IsNullOrEmpty(obj.transaction_desc) || string.IsNullOrEmpty(obj.order_id) || string.IsNullOrEmpty(obj.amount) || string.IsNullOrEmpty(obj.callback_url))
                {
                    objdata.status = 200;
                    objdata.result = "Missing Input Parameters";
                    return objdata;
                }

                string description = obj.service_name + " $^$" + obj.transaction_desc + "$^$" + obj.user_id + "$^$";
                string strstring = "900020" + "&" + obj.order_id + "&" + description + "&" + obj.amount + "&" + obj.callback_url;

                string skey_encryption = Encryption.GetEncryptedText(Convert.ToBase64String(sessionKey.Key), Decryption.Decrypt_usingpassword(System.Configuration.ConfigurationManager.AppSettings["TAPublic_Key"].ToString()));
                string encrypted_data = Encryption.AESEncryption(strstring, Convert.ToBase64String(sessionKey.Key), false);

                objdata.status = 200;
                objdata.result = encrypted_data;
                objdata.sec_key = skey_encryption;
                objdata.url = "https://staging.transactionanalysts.com:444/TAPaymentGateway/RequestHandler.aspx";

            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return objdata;

        }

        public dynamic TAresponseDecrypt(TAWalletpaymentModel obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                if (string.IsNullOrEmpty(obj.encrypted_data) || string.IsNullOrEmpty(obj.user_id))
                {
                    objdata.status = 200;
                    objdata.result = "Missing Input Parameters";
                    return objdata;
                }
                else
                {
                    obj.encrypted_data = obj.encrypted_data.Replace(" ", "+");

                    string response_data = Decryption.AES_Decryption(obj.encrypted_data, Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(CalculateMD5Hash("TAGRAMSACHIVALAYAM" + obj.user_id))), false);
                    string strDecrypted_data = Encoding.Default.GetString(Convert.FromBase64String(response_data));

                    TAWalletResponseModel taResponseObj = JsonConvert.DeserializeObject<TAWalletResponseModel>(strDecrypted_data);
                    if (taResponseObj.Status == "Y")
                    {
                        objdata.status = 200;
                        objdata.result = "Payment made Successfully !!!!";
                    }
                    else
                    {
                        objdata.status = 400;
                        objdata.result = "Failed to make Payment";
                    }
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return objdata;

        }

        public DataTable gswsPaymentResponseProc(gswsPaymentResponseModel obj)
        {

            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GSWS_PAYMENT_REP_INSERT_PROC";
                cmd.Parameters.Add("ptype", OracleDbType.Varchar2).Value = obj.type;
                cmd.Parameters.Add("pBALANCE", OracleDbType.Varchar2).Value = obj.balance;
                cmd.Parameters.Add("pINSERTED_ON", OracleDbType.TimeStamp).Value = DateTime.Now;
                cmd.Parameters.Add("pMERCHANT_NAME", OracleDbType.Varchar2).Value = obj.merchant_name;
                cmd.Parameters.Add("pMESSAGE", OracleDbType.Varchar2).Value = obj.message;
                cmd.Parameters.Add("pORDERID", OracleDbType.Varchar2).Value = obj.orderId;
                cmd.Parameters.Add("pRESPONSE_CODE", OracleDbType.Varchar2).Value = obj.responseCode;
                cmd.Parameters.Add("pTRXN_REF_NO", OracleDbType.TimeStamp).Value = obj.txnRefNo;
                cmd.Parameters.Add("pUNIQUE_TXN_ID", OracleDbType.Varchar2).Value = obj.uniqueTxnId;
                cmd.Parameters.Add("pSTATUS", OracleDbType.Varchar2).Value = obj.status;
                cmd.Parameters.Add("pcur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                DataTable dtTrans = _LoginHel.GetgswsDataAdapter(cmd);
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
                string mappath = HttpContext.Current.Server.MapPath("WalletResponseException");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, JsonConvert.SerializeObject(obj)));
                throw ex;
            }
        }

        public DataTable gswsPaymentRequestProc(paymentModel obj, string type, string otp)
        {

            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GSWS_PAYMENT_INSERT_PROC";
                cmd.Parameters.Add("ptype", OracleDbType.Varchar2).Value = type;
                cmd.Parameters.Add("pAMOUNT", OracleDbType.Varchar2).Value = obj.Amount;
                cmd.Parameters.Add("pAPPLICANTNAME", OracleDbType.Varchar2).Value = obj.applicantName;
                cmd.Parameters.Add("pDEPTCODE", OracleDbType.Varchar2).Value = obj.deptCode;
                cmd.Parameters.Add("pDEPTRECIEPTCODE", OracleDbType.Varchar2).Value = obj.deptRecieptCode;
                cmd.Parameters.Add("pDESCRIPTION", OracleDbType.Varchar2).Value = obj.Description;
                cmd.Parameters.Add("pDISTRICTCODE", OracleDbType.Varchar2).Value = obj.districtCode;
                cmd.Parameters.Add("pINSERTED_ON", OracleDbType.TimeStamp).Value = DateTime.Now;
                cmd.Parameters.Add("pIPADDRESS", OracleDbType.Varchar2).Value = obj.ipAddress;
                cmd.Parameters.Add("pMOBILENUMBER", OracleDbType.Varchar2).Value = obj.mobileNumber;
                cmd.Parameters.Add("pORDERID", OracleDbType.Varchar2).Value = obj.orderId;
                cmd.Parameters.Add("pSERVICECODE", OracleDbType.Varchar2).Value = obj.serviceCode;
                cmd.Parameters.Add("pSTAFFCODE", OracleDbType.Varchar2).Value = obj.staffCode;
                cmd.Parameters.Add("pTOTALAMOUNT", OracleDbType.Varchar2).Value = obj.totalAmount;
                cmd.Parameters.Add("pTXNDATE", OracleDbType.Varchar2).Value = obj.TxnDate;
                cmd.Parameters.Add("pUSERCHARGES", OracleDbType.Varchar2).Value = obj.userCharges;
                cmd.Parameters.Add("pAPPLICATION_DATE", OracleDbType.TimeStamp).Value = DateTime.Now;
                cmd.Parameters.Add("pDEPARTMENT_APPLICATION_DATE", OracleDbType.TimeStamp).Value = DateTime.Now;
                cmd.Parameters.Add("pMERCHANTCODE", OracleDbType.Varchar2).Value = obj.merchantId;
                cmd.Parameters.Add("pOTP", OracleDbType.Varchar2).Value = otp;
                cmd.Parameters.Add("pUNIQUE_TXN_ID", OracleDbType.Varchar2).Value = "TXN" + obj.UniqueTxnId;
                cmd.Parameters.Add("pcur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                DataTable dtTrans = _LoginHel.GetgswsDataAdapter(cmd);
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
                string mappath = HttpContext.Current.Server.MapPath("WalletRequestException");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetRegReceivedDashboardCount:" + ex.Message.ToString()));
                throw ex;
            }
        }

        public string CalculateMD5Hash(string input)
        {


            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}