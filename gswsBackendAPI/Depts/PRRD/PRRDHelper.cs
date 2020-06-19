
using gswsBackendAPI.Depts.SocialWelfare_Tribal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using gswsBackendAPI.DL.CommonHel;
using System.Threading.Tasks;
using gswsBackendAPI.DL.DataConnection;
using gswsBackendAPI.transactionModule;

namespace gswsBackendAPI.Depts.PRRD
{
    public class PRRDHelper
    {
        Helper objHelper = new Helper();

        PRRDResponse objResponse = new PRRDResponse();

		public dynamic fetchPanchayatsData(dynamic objdata)
		{
			try
			{
				var data = PRRD_Tax_Search("https://pris.ap.gov.in/PRISAPI/api2.php?getPanchayats=1", objdata.Secccode.ToString());

				if (data != null && data != "")
				{
					objResponse.Status = "Success";
					objResponse.data = JsonConvert.DeserializeObject<dynamic>(data);
				}
				else
				{
					objResponse.Status = "No Data Found";
					objResponse.data = "Invalid Certificate Number";
				}
			}
			catch (Exception ex)
			{

				objResponse.Status = "Failed";
				objResponse.data = CommonSPHel.ThirdpartyMessage;
                Common_PRRD_Error(ex.Message.ToString(), "https://pris.ap.gov.in/PRISAPI/api2.php?getPanchayats=1", "2");
            }
			return objResponse;
		}

		public dynamic GetTaxAppStatusData(dynamic objdata)
		{
			try
			{
				var data = PRRD_Tax_Search("https://pris.ap.gov.in/PRISAPI/api2.php?trackApplication=1&unique=" + objdata.AppNo.ToString() + "&application_type=" + objdata.AppType.ToString(), objdata.Secccode.ToString());

				if (data != null && data != "")
				{
					objResponse.Status = "Success";
					objResponse.data = JsonConvert.DeserializeObject<dynamic>(data);
				}
				else
				{
					objResponse.Status = "No Data Found";
					objResponse.data = "Invalid Certificate Number";
				}
			}
			catch (Exception ex)
			{
				objResponse.Status = "Failed";
				objResponse.data = CommonSPHel.ThirdpartyMessage;
                Common_PRRD_Error(ex.Message.ToString(), "https://pris.ap.gov.in/PRISAPI/api2.php?trackApplication=1&unique=", "2");
            }
			return objResponse;
		}

		public dynamic fetchTransactionData(dynamic objdata)
		{
			try
			{

				var data = PRRD_Tax_Search("https://pris.ap.gov.in/PRISAPI/api2.php?fetchTransactionData=1&taxType=" + objdata.taxType.ToString() + "&searchBy=" + objdata.searchBy.ToString() + "&value=" + objdata.value.ToString() + "", objdata.Secccode.ToString());

				if (data != null && data != "")
				{
					objResponse.Status = "Success";
					objResponse.data = JsonConvert.DeserializeObject<TransactionsData>(data);
				}
				else
				{
					objResponse.Status = "No Data Found";
					objResponse.data = "Invalid Certificate Number";
				}
			}
			catch (Exception ex)
			{
				objResponse.Status = "Failed";
				objResponse.data = CommonSPHel.ThirdpartyMessage;
                Common_PRRD_Error(ex.Message.ToString(), "https://pris.ap.gov.in/PRISAPI/api2.php?fetchTransactionData=1&taxType=", "2");
            }
			return objResponse;
		}

		public dynamic LoadNICPanchayats_helper(dynamic objdata)
		{
			try
			{
				PRRDSPHelper spobj = new PRRDSPHelper();

				LGDMasterModel objl = new LGDMasterModel();
				objl.FTYPE = "6";
				objl.DISTCODE = objdata.seccode;

				var data = spobj.GetGSWS_SecretariatMaster_LGD_SP(objl);

				objResponse.Status = "Success";
				objResponse.data = data;
			}
			catch (Exception ex)
			{
				objResponse.Status = "Failed";
				objResponse.data = CommonSPHel.ThirdpartyMessage;
			}
			return objResponse;
		}

		public dynamic PRRD_Tax_Search(string url, string scode)
		{
			var response = String.Empty;

			try
			{
				using (var client = new HttpClient())
				{
					ServicePointManager.Expect100Continue = true;
					ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

					System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

					var req = (HttpWebRequest)WebRequest.Create(url);
					req.ContentType = "application/json";

					req.Method = "POST";
					req.Headers.Add("ApiKey", "20191108");
					req.Headers.Add("gpcode", scode);

					req.AllowAutoRedirect = false;

					var resp = req.GetResponse();
					var sr = new StreamReader(resp.GetResponseStream());
					response = sr.ReadToEnd().Trim();//.Replace("<br />\n<b>Notice</b>:  Undefined index: HTTP_USER_AGENT in <b>/var/www/html/PRISAPI/api2.php</b> on line <b>2147</b><br />\n", "");

					string mappath = HttpContext.Current.Server.MapPath("TaxPaymentsResponseLogs");
					Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "url : " + url + ", scode : " + scode + ", Response Data : " + response));

				}
			}
			catch (WebException wex)
			{
				string mappath = HttpContext.Current.Server.MapPath("TaxPaymentsExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "url : " + url + ", scode : " + scode + ", Error Message : " + wex.Message.ToString()));

				throw wex;
			}

			return response;
		}

		public dynamic SendTransactionRequest(dynamic objdata)
		{
			try
			{
				//https://pris.ap.gov.in/PRISAPI/api2.php?requestMutation=1&aadhar=234172205439&owner_name=supriya&fname=srinivas&dob=1071995&mobile=7288877931&gender=0&unique=113450511


				string url = string.Empty;
				if (objdata.RequestType == 1)
				{
					url = "https://pris.ap.gov.in/PRISAPI/api2.php?requestMutation=1&aadhar=" + objdata.aadhar.ToString() + "&owner_name=" + objdata.owner_name.ToString() + "&fname=" + objdata.fname.ToString() + "&dob=" + objdata.dob.ToString() + "&mobile=" + objdata.mobile
						+ "&gender=" + objdata.gender.ToString() + "&unique=" + objdata.unique.ToString() + "&GSWS_ID=" + objdata.GSWS_ID.ToString() + "";
				}
				else if (objdata.RequestType == 2)
				{
					//https://pris.ap.gov.in/PRISAPI/api2.php?requestTradeLicence=1&trade_name=test&owner_name=supriya&mobile=7288877931&aadhar=234172205439&assessment_no=9998&trade_type=1
					url = "https://pris.ap.gov.in/PRISAPI/api2.php?requestTradeLicence=1&trade_name=" + objdata.trade_name.ToString() + "&owner_name=" + objdata.owner_name.ToString() + "&mobile=" + objdata.mobile.ToString() + "&aadhar=" + objdata.aadhar.ToString() + "&assessment_no=" + objdata.assessment_no.ToString() + "&trade_type=" + objdata.trade_type.ToString() + "&GSWS_ID=" + objdata.GSWS_ID.ToString() + "";
				}
				else if (objdata.RequestType == 3)
				{
					//https://pris.ap.gov.in/PRISAPI/api2.php?requestPrivateTap=true&tap_size=1&usage=2&%20noofconn=1&unique=1134505409998
					url = "https://pris.ap.gov.in/PRISAPI/api2.php?requestPrivateTap=true&tap_size=" + objdata.tap_size.ToString() + "&usage=" + objdata.usage.ToString() + "&tap_assessment=" + objdata.noofconn.ToString() + "&unique=" + objdata.unique.ToString() + "&GSWS_ID=" + objdata.GSWS_ID.ToString() + "";
				}
				else if (objdata.RequestType == 4)
				{
					//https://pris.ap.gov.in/PRISAPI/api2.php?requestPropertyValuation=true&elTransactionId=1134505&unique=1134505340
					url = "https://pris.ap.gov.in/PRISAPI/api2.php?requestPropertyValuation=true&elTransactionId=" + objdata.elTransactionId.ToString() + "&unique=" + objdata.unique.ToString() + "&GSWS_ID = " + objdata.GSWS_ID.ToString() + "";

				}
				var data = PRRD_Tax_Search(url, objdata.Secccode.ToString());

				if (data != null && data != "")
				{
					TransactionResponse objres = JsonConvert.DeserializeObject<TransactionResponse>(data);
					if (objres.status == "0")
					{
						try
						{
							transactionModel objtrans = new transactionModel();
							objtrans.TYPE = "2";
							objtrans.TXN_ID = objdata.GSWS_ID;
							objtrans.DEPT_ID = "3101";
							objtrans.DEPT_TXN_ID = objres.transactionNo;
							objtrans.STATUS_CODE = "01";
							objtrans.REMARKS = objres.msg;

							DataTable dt = new transactionHelper().transactionInsertion(objtrans);
							if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
							{
								objResponse.Status = "Success";
								objResponse.data = JsonConvert.DeserializeObject<TransactionResponse>(data);
							}
						}
						catch (Exception ex)
						{
							string mappath = HttpContext.Current.Server.MapPath("PRRDExceptionLogs");
							Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Transaction Error API:" + ex.Message.ToString()));

							objResponse.Status = "Success";
							objResponse.data = JsonConvert.DeserializeObject<TransactionResponse>(data);
                            Common_PRRD_Error(ex.Message.ToString(), url, "2");
                        }

						objResponse.Status = "Success";
						objResponse.data = JsonConvert.DeserializeObject<TransactionResponse>(data);
					}
					else
					{
						objResponse.Status = "Success";
						objResponse.data = JsonConvert.DeserializeObject<TransactionResponse>(data);
					}

				}
				else
				{
					objResponse.Status = "No Data Found";
					objResponse.data = "Invalid Certificate Number";
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("PRRDExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error TAX SEARCH Data API:" + ex.Message.ToString()));
				objResponse.Status = "Failed";
				objResponse.data = CommonSPHel.ThirdpartyMessage;
            }
			return objResponse;
		}


		public dynamic PostData_Basic_Auth_PaymentDetails(dynamic jsonData)
        {
           
            try
            {
                using (var client = new HttpClient())
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };


                    var req = (HttpWebRequest)WebRequest.Create("http://dbtrd.ap.gov.in/NregaDashBoardService/rest/Jobcard/PaymentDetails");
                    req.ContentType = "application/json";
                    req.Method = "POST";

                    ////BASIC AUTH USERNAME = gramasach Password : gram@c90
                    req.Headers.Add("Authorization", "Basic Z3JhbWFzYWNoOmdyYW1AYzkw");

                    req.AllowAutoRedirect = false;

                    var _jsonObject = JsonConvert.SerializeObject(jsonData);

                    if (!String.IsNullOrEmpty(_jsonObject))
                    {
                        using (System.IO.Stream s = req.GetRequestStream())
                        {
                            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(s))
                                sw.Write(_jsonObject);
                        }
                    }


                    var resp = (HttpWebResponse)req.GetResponse();
                    var sr = new StreamReader(resp.GetResponseStream());

                    if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
                        (resp.StatusCode == HttpStatusCode.RedirectMethod))
                    {
                        objResponse.Status = "Failed";
                        objResponse.data = "";
                    }
                    else
                    {

                        string data = sr.ReadToEnd().Trim();
                        objResponse.Status = "Success";
                        objResponse.data = JsonConvert.DeserializeObject<PaymentDetailList>(data); ;
                    }


                }
            }
            catch (WebException ex)
            {
                string mappath = HttpContext.Current.Server.MapPath("PRRDExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Volunteer Mapping Data API:" + ex.Message.ToString()));

                objResponse.Status = "Failed";
                objResponse.data = CommonSPHel.ThirdpartyMessage;
                Common_PRRD_Error(ex.Message.ToString(), "http://dbtrd.ap.gov.in/NregaDashBoardService/rest/Jobcard/PaymentDetails", "2");
            }
            return objResponse;

        }

        public dynamic PostData_Basic_Auth_PaymentDetails_BY_UID(dynamic jsonData)
        {

            try
            {
                using (var client = new HttpClient())
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };


                    var req = (HttpWebRequest)WebRequest.Create("http://dbtrd.ap.gov.in/NregaDashBoardService/rest/Uid/PaymentDetails");
                    req.ContentType = "application/json";
                    req.Method = "POST";

                    ////BASIC AUTH USERNAME = gramasach Password : gram@c90
                    req.Headers.Add("Authorization", "Basic Z3JhbWFzYWNoOmdyYW1AYzkw");

                    req.AllowAutoRedirect = false;

                    var _jsonObject = JsonConvert.SerializeObject(jsonData);

                    if (!String.IsNullOrEmpty(_jsonObject))
                    {
                        using (System.IO.Stream s = req.GetRequestStream())
                        {
                            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(s))
                                sw.Write(_jsonObject);
                        }
                    }


                    var resp = (HttpWebResponse)req.GetResponse();
                    var sr = new StreamReader(resp.GetResponseStream());

                    if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
                        (resp.StatusCode == HttpStatusCode.RedirectMethod))
                    {
                        objResponse.Status = "Failed";
                        objResponse.data = "";
                    }
                    else
                    {

                        string data = sr.ReadToEnd().Trim();
                        objResponse.Status = "Success";
                        objResponse.data = JsonConvert.DeserializeObject<UIDPaymentDetailList>(data); ;
                    }


                }
            }
            catch (WebException ex)
            {
                string mappath = HttpContext.Current.Server.MapPath("PRRDExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Volunteer Mapping Data API:" + ex.Message.ToString()));

                objResponse.Status = "Failed";
                objResponse.data = CommonSPHel.ThirdpartyMessage;
                Common_PRRD_Error(ex.Message.ToString(), "http://dbtrd.ap.gov.in/NregaDashBoardService/rest/Uid/PaymentDetails", "2");
            }
            return objResponse;

        }

        public dynamic Project_Work_MasterData()
        {

            try
            {
                using (var client = new HttpClient())
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };


                    var req = (HttpWebRequest)WebRequest.Create("http://125.17.121.166:8080/WebAPIGSWorks/api/Works/GetWorksMasterData");
                    req.ContentType = "application/json";
                    req.Method = "GET";

                    //BASIC AUTH USERNAME = Test Password : Test@123
                    req.Headers.Add("Authorization", "Basic VGVzdDpUZXN0QDEyMw==");

                    req.AllowAutoRedirect = false;

                    
                    var resp = (HttpWebResponse)req.GetResponse();
                    var sr = new StreamReader(resp.GetResponseStream());

                    if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
                        (resp.StatusCode == HttpStatusCode.RedirectMethod))
                    {
                        objResponse.Status = "Failed";
                        objResponse.data = "";
                    }
                    else
                    {

                        string data = sr.ReadToEnd().Trim();
                        objResponse.Status = "Success";
                        objResponse.data = JsonConvert.DeserializeObject<List<WorkProjectMaster>>(data); ;
                    }


                }
            }
            catch (WebException ex)
            {
                string mappath = HttpContext.Current.Server.MapPath("PRRDExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Volunteer Mapping Data API:" + ex.Message.ToString()));

                objResponse.Status = "Failed";
                objResponse.data = CommonSPHel.ThirdpartyMessage;
                Common_PRRD_Error(ex.Message.ToString(), "http://125.17.121.166:8080/WebAPIGSWorks/api/Works/GetWorksMasterData", "2");
            }
            return objResponse;

        }

        public dynamic Get_FarmerData(dynamic jsonData)
        {

            try
            {
                using (var client = new HttpClient())
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };


                    var req = (HttpWebRequest)WebRequest.Create("http://125.17.121.166:8080/WebAPIGSWorks/api/Works/GetWageSeekerDetails?strWageseekerId=" + jsonData);
                    req.ContentType = "application/json";
                    req.Method = "GET";

                    //BASIC AUTH USERNAME = Test Password : Test@123
                    req.Headers.Add("Authorization", "Basic VGVzdDpUZXN0QDEyMw==");

                    req.AllowAutoRedirect = false;

                    
                    var resp = (HttpWebResponse)req.GetResponse();
                    var sr = new StreamReader(resp.GetResponseStream());

                    if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
                        (resp.StatusCode == HttpStatusCode.RedirectMethod))
                    {
                        objResponse.Status = "Failed";
                        objResponse.data = "";
                    }
                    else
                    {

                        string data = sr.ReadToEnd().Trim();
                        objResponse.Status = "Success";
                        objResponse.data = JsonConvert.DeserializeObject<FarmerData>(data); ;
                    }


                }
            }
            catch (WebException ex)
            {
                string mappath = HttpContext.Current.Server.MapPath("PRRDExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Volunteer Mapping Data API:" + ex.Message.ToString()));

                objResponse.Status = "Failed";
                objResponse.data = CommonSPHel.ThirdpartyMessage;
                Common_PRRD_Error(ex.Message.ToString(), "http://125.17.121.166:8080/WebAPIGSWorks/api/Works/GetWageSeekerDetails?strWageseekerId=", "2");
            }
            return objResponse;

        }

        public dynamic Save_FarmerData(dynamic jsonData)
        {

            try
            {
                using (var client = new HttpClient())
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };




                    var req = (HttpWebRequest)WebRequest.Create("http://125.17.121.166:8080/WebAPIGSWorks/api/Works/SaveFarmerData?strJson="+jsonData);
                    req.ContentType = "application/json";
                    req.Method = "GET";

                    //BASIC AUTH USERNAME = Test Password : Test@123
                    req.Headers.Add("Authorization", "Basic VGVzdDpUZXN0QDEyMw==");

                    req.AllowAutoRedirect = false;


                    var resp = (HttpWebResponse)req.GetResponse();
                    var sr = new StreamReader(resp.GetResponseStream());

                    if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
                        (resp.StatusCode == HttpStatusCode.RedirectMethod))
                    {
                        objResponse.Status = "Failed";
                        objResponse.data = "";
                    }
                    else
                    {

                        string data = sr.ReadToEnd().Trim();
                        objResponse.Status = "Success";
                        objResponse.data = JsonConvert.DeserializeObject<List<FarmerData>>(data); ;
                    }


                }
            }
            catch (WebException ex)
            {
                string mappath = HttpContext.Current.Server.MapPath("PRRDExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Volunteer Mapping Data API:" + ex.Message.ToString()));

                objResponse.Status = "Failed";
                objResponse.data = CommonSPHel.ThirdpartyMessage;
                Common_PRRD_Error(ex.Message.ToString(), "http://125.17.121.166:8080/WebAPIGSWorks/api/Works/SaveFarmerData?strJson=", "2");
            }
            return objResponse;

        }



        public dynamic DemandCapture(dynamic jsonData)
        {

            try
            {
                using (var client = new HttpClient())
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                    var req = (HttpWebRequest)WebRequest.Create("http://125.17.121.166:8080/DemandCaptureAPI/api/Demand/PostRequest");
                    req.ContentType = "application/json";
                    req.Method = "POST";

                    //BASIC AUTH USERNAME = VillageSecretary Password : Sachivalaya@!@#45
                    req.Headers.Add("Authorization", "Basic VmlsbGFnZVNlY3JldGFyeTpTYWNoaXZhbGF5YUAhQCM0NQ==");

                    req.AllowAutoRedirect = false;
                    var _jsonObject = JsonConvert.SerializeObject(jsonData);

                    if (!String.IsNullOrEmpty(_jsonObject))
                    {
                        using (System.IO.Stream s = req.GetRequestStream())
                        {
                            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(s))
                                sw.Write(_jsonObject);
                        }
                    }

                    var resp = (HttpWebResponse)req.GetResponse();
                    var sr = new StreamReader(resp.GetResponseStream());

                    if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
                        (resp.StatusCode == HttpStatusCode.RedirectMethod))
                    {
                        objResponse.Status = "Failed";
                        objResponse.data = "";
                    }
                    else
                    {

                        string data = sr.ReadToEnd().Trim();
                        objResponse.Status = "Success";
                        objResponse.data = JsonConvert.DeserializeObject<DemandCaptureResponse>(data); ;
                    }


                }
            }
            catch (WebException ex)
            {
                string mappath = HttpContext.Current.Server.MapPath("PRRDExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Volunteer Mapping Data API:" + ex.Message.ToString()));

                objResponse.Status = "Failed";
                objResponse.data = CommonSPHel.ThirdpartyMessage;
                Common_PRRD_Error(ex.Message.ToString(), "http://125.17.121.166:8080/DemandCaptureAPI/api/Demand/PostRequest", "2");
            }
            return objResponse;

        }

        public dynamic ConfirmDemand(dynamic jsonData)
        {

            try
            {
                using (var client = new HttpClient())
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                    var req = (HttpWebRequest)WebRequest.Create("http://125.17.121.166:8080/DemandCaptureAPI/api/ConfirmDemand/ConfirmRequest");
                    req.ContentType = "application/json";
                    req.Method = "POST";

                    //BASIC AUTH USERNAME = VillageSecretary Password : Sachivalaya@!@#45
                    req.Headers.Add("Authorization", "Basic VmlsbGFnZVNlY3JldGFyeTpTYWNoaXZhbGF5YUAhQCM0NQ==");

                    req.AllowAutoRedirect = false;
                    var _jsonObject = JsonConvert.SerializeObject(jsonData);

                    if (!String.IsNullOrEmpty(_jsonObject))
                    {
                        using (System.IO.Stream s = req.GetRequestStream())
                        {
                            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(s))
                                sw.Write(_jsonObject);
                        }
                    }

                    var resp = (HttpWebResponse)req.GetResponse();
                    var sr = new StreamReader(resp.GetResponseStream());

                    if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
                        (resp.StatusCode == HttpStatusCode.RedirectMethod))
                    {
                        objResponse.Status = "Failed";
                        objResponse.data = "";
                    }
                    else
                    {

                        string data = sr.ReadToEnd().Trim();
                        objResponse.Status = "Success";
                        objResponse.data = JsonConvert.DeserializeObject<ConfirmDemand>(data) ;
                    }


                }
            }
            catch (WebException ex)
            {
                string mappath = HttpContext.Current.Server.MapPath("PRRDExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Volunteer Mapping Data API:" + ex.Message.ToString()));

                objResponse.Status = "Failed";
                objResponse.data = CommonSPHel.ThirdpartyMessage;
                Common_PRRD_Error(ex.Message.ToString(), "http://125.17.121.166:8080/DemandCaptureAPI/api/ConfirmDemand/ConfirmRequest", "2");
            }
            return objResponse;

        }

		public dynamic BuildingPlanApplicationStatus(dynamic jsonData)
		{

			try
			{
				using (var client = new HttpClient())
				{
                    if (Utils.IsAlphaNumeric(jsonData.id))
                    {

                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                        var req = (HttpWebRequest)WebRequest.Create("http://pris.ap.gov.in/bpl/api.php?getStatus=" + jsonData.getStatus + "&id=" + jsonData.id + "");
                        req.ContentType = "application/json";
                        req.Method = "GET";

                        req.Headers.Add("ApiKey", "bpl@$@123");
                        req.Headers.Add("gpcode", "215087");

                        req.AllowAutoRedirect = false;
                        //var _jsonObject = JsonConvert.SerializeObject(jsonData);

                        //if (!String.IsNullOrEmpty(_jsonObject))
                        //{
                        //    using (System.IO.Stream s = req.GetRequestStream())
                        //    {
                        //        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(s))
                        //            sw.Write(_jsonObject);
                        //    }
                        //}

                        var resp = (HttpWebResponse)req.GetResponse();
                        var sr = new StreamReader(resp.GetResponseStream());

                        if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
                            (resp.StatusCode == HttpStatusCode.RedirectMethod))
                        {
                            objResponse.Status = "Failed";
                            objResponse.data = "";
                        }
                        else
                        {

                            string data = sr.ReadToEnd().Trim();
                            objResponse.Status = "Success";
                            objResponse.data = JsonConvert.DeserializeObject<BuildingAppStatus>(data);
                        }
                    }
                    else
                    {
                        objResponse.Status = "Failed";
                        objResponse.data = "Application Number Should not Contain Special Characters";
                    }

				}
			}
			catch (WebException ex)
			{
                string mappath = HttpContext.Current.Server.MapPath("PRRDExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Volunteer Mapping Data API:" + ex.Message.ToString()));

                objResponse.Status = "Failed";
				objResponse.data = CommonSPHel.ThirdpartyMessage;
                Common_PRRD_Error(ex.Message.ToString(), "http://pris.ap.gov.in/bpl/api.php?getStatus=", "2");
            }
			return objResponse;

		}
		public dynamic LayoutPlanApplicationStatus(dynamic jsonData)
		{

			try
			{
				using (var client = new HttpClient())
				{
					ServicePointManager.Expect100Continue = true;
					ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

					System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

					var req = (HttpWebRequest)WebRequest.Create("http://pris.ap.gov.in/layout/api.php?getStatus=" + jsonData.getStatus + "&id=" + jsonData.id + "");
					req.ContentType = "application/json";
					req.Method = "GET";

					req.Headers.Add("ApiKey", "layout@$@123");
					req.Headers.Add("gpcode", "215087");

					req.AllowAutoRedirect = false;
					var resp = (HttpWebResponse)req.GetResponse();
					var sr = new StreamReader(resp.GetResponseStream());

					if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
						(resp.StatusCode == HttpStatusCode.RedirectMethod))
					{
						objResponse.Status = "Failed";
						objResponse.data = "";
					}
					else
					{

						string data = sr.ReadToEnd().Trim();
						objResponse.Status = "Success";
						objResponse.data = JsonConvert.DeserializeObject<BuildingAppStatus>(data);
					}


				}
			}
			catch (WebException ex)
			{
                string mappath = HttpContext.Current.Server.MapPath("PRRDExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Volunteer Mapping Data API:" + ex.Message.ToString()));

                objResponse.Status = "Failed";
				objResponse.data = CommonSPHel.ThirdpartyMessage;
                Common_PRRD_Error(ex.Message.ToString(), "http://pris.ap.gov.in/layout/api.php?getStatus=", "2");
            }
			return objResponse;

		}

		public dynamic LayoutPlanApplicationInformation(dynamic jsonData)
		{

			try
			{
				using (var client = new HttpClient())
				{
					ServicePointManager.Expect100Continue = true;
					ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

					System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

					var req = (HttpWebRequest)WebRequest.Create("http://pris.ap.gov.in/layout/api.php?search=" + jsonData.search + "&search_val=" + jsonData.search_val + "");
					req.ContentType = "application/json";
					req.Method = "GET";

					req.Headers.Add("ApiKey", "layout@$@123");
					req.Headers.Add("gpcode", "215087");

					req.AllowAutoRedirect = false;
					var resp = (HttpWebResponse)req.GetResponse();
					var sr = new StreamReader(resp.GetResponseStream());

					if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
						(resp.StatusCode == HttpStatusCode.RedirectMethod))
					{
						objResponse.Status = "Failed";
						objResponse.data = "";
					}
					else
					{

						string data = sr.ReadToEnd().Trim();
						objResponse.Status = "Success";
						objResponse.data = JsonConvert.DeserializeObject<LayoutInformation>(data);
					}


				}
			}
			catch (WebException ex)
			{
                string mappath = HttpContext.Current.Server.MapPath("PRRDExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Volunteer Mapping Data API:" + ex.Message.ToString()));

                objResponse.Status = "Failed";
				objResponse.data = CommonSPHel.ThirdpartyMessage;
                Common_PRRD_Error(ex.Message.ToString(), "http://pris.ap.gov.in/layout/api.php?search=", "2");
            }
			return objResponse;

		}


		public dynamic TransportDrinkingWater()
		{

			try
			{
				using (var client = new HttpClient())
				{
					ServicePointManager.Expect100Continue = true;
					ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

					System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

					var req = (HttpWebRequest)WebRequest.Create("http://rwss.ap.nic.in/RwsRest/RestFul/RwsWaterTankerService/getWaterTankerTrips");
					req.ContentType = "application/json";
					req.Method = "GET";

					//BASIC AUTH USERNAME = Admin Password : 1234
					req.Headers.Add("Authorization", "Basic QWRtaW46MTIzNA==");

					req.AllowAutoRedirect = false;
					var resp = (HttpWebResponse)req.GetResponse();
					var sr = new StreamReader(resp.GetResponseStream());

					if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
						(resp.StatusCode == HttpStatusCode.RedirectMethod))
					{
						objResponse.Status = "Failed";
						objResponse.data = "";
					}
					else
					{

						string data = sr.ReadToEnd().Trim();
						objResponse.Status = "Success";
						objResponse.data = JsonConvert.DeserializeObject<TankarWaterResponse>(data);
					}


				}
			}
			catch (WebException ex)
			{
                string mappath = HttpContext.Current.Server.MapPath("PRRDExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Volunteer Mapping Data API:" + ex.Message.ToString()));

                objResponse.Status = "Failed";
				objResponse.data = CommonSPHel.ThirdpartyMessage ;
                Common_PRRD_Error(ex.Message.ToString(), "http://rwss.ap.nic.in/RwsRest/RestFul/RwsWaterTankerService/getWaterTankerTrips", "2");
            }
			return objResponse;

		}
		public static String sha256_hash(String value)
		{
			StringBuilder Sb = new StringBuilder();

			using (SHA256 hash = SHA256Managed.Create())
			{
				Encoding enc = Encoding.UTF8;
				Byte[] result = hash.ComputeHash(enc.GetBytes(value));

				foreach (Byte b in result)
					Sb.Append(b.ToString("x2"));
			}

			return Sb.ToString();
		}

		public dynamic AuthentiateCall(dynamic jsonData)
		{
			dynamic responsedata = new ExpandoObject();

			try
			{
				PRRDSPHelper spobj = new PRRDSPHelper();
				LGDMasterModel objl = new LGDMasterModel();
				
				var random = new Random();
				int randomnumber = random.Next();


				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };


				string url = "https://mpanchayat.ap.gov.in/PIM/authenticateGs.do";
				HttpClient client = new HttpClient();
				client.BaseAddress = new Uri(url);
				client.DefaultRequestHeaders.Accept.Clear();


				MultipartFormDataContent content = new MultipartFormDataContent();

				StringContent username = new StringContent("APITECGS", Encoding.UTF8, "multipart/form-data");
				StringContent password = new StringContent(sha256_hash(sha256_hash("GSCPRRD#@2020") + randomnumber + Convert.ToString(jsonData.granttype) + Convert.ToString(jsonData.servicetype)), Encoding.UTF8, "multipart/form-data");
				//StringContent password = new StringContent("GSCPRRD#@2020", Encoding.UTF8, "multipart/form-data");
				StringContent gs_applid = new StringContent(randomnumber.ToString(), Encoding.UTF8, "multipart/form-data");
				StringContent granttype = new StringContent(Convert.ToString(jsonData.granttype), Encoding.UTF8, "multipart/form-data");
				StringContent servicetype = new StringContent(Convert.ToString(jsonData.servicetype), Encoding.UTF8, "multipart/form-data");

				content.Add(username, "username");
				content.Add(password, "password");
				content.Add(gs_applid, "gs_applid");
				content.Add(granttype, "granttype");
				content.Add(servicetype, "servicetype");


				HttpResponseMessage resp = client.PostAsync(url, content).Result;

				string returnString = resp.Content.ReadAsStringAsync().Result;

				if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
					(resp.StatusCode == HttpStatusCode.RedirectMethod))
				{
					responsedata.Status = "Failed";
					responsedata.data = "";
					responsedata.Reason = "Invalid request Service is not Respond";
				}
				else
				{

					string data = returnString;
					responsedata.Status = "Success";


					responsedata.username = "APITECGS";
					responsedata.servicetype = Convert.ToString(jsonData.servicetype);
					responsedata.gs_applid = randomnumber;
					//PRRDSPHelper spobj = new PRRDSPHelper();
					//LGDMasterModel objl = new LGDMasterModel();
					objl.FTYPE = "6";
					objl.DISTCODE = jsonData.seccode;
					//DataTable dt1=spobj.GetGSWS_SecretariatMaster_LGD_SP(objl);
					//responsedata.lgdgpcode = spobj.GetGSWS_SecretariatMaster_LGD_SP(objl);
					responsedata.data = JsonConvert.DeserializeObject<List<AuthenticateNOCMarriage>>(data);
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("PRRDExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting AuthentiateCall NOC Application Data API:" + ex.Message.ToString()));
				//Logfile("Program:AuthentiateCall::Exception:CPID:" + cpkid, ex.Message, strPath);
				responsedata.Status = "Failed";
				responsedata.data = "";
				responsedata.Reason = "Department Service is not Working.Please Try Again.";
                Common_PRRD_Error(ex.Message.ToString(), "https://mpanchayat.ap.gov.in/PIM/authenticateGs.do", "2");
                //return ex;

            }
			return responsedata;
		}

		#region Volunteer

		public dynamic GetValunteerMapping_helper(VolunteerCls root)
		{
			dynamic obj = new ExpandoObject();
			PRRDSPHelper spobj = new PRRDSPHelper();
			try
			{
				DataTable data = spobj.GetValunteerMapping_data_helper(root);
				if (data != null)
				{
					obj.Status = 100;
					obj.Reason = "Data Loaded Successfully.";
					obj.Details = data;
				}
				else
				{
					obj.Status = 102;
					obj.Reason = "No Data Found";
				}

			}
			catch (Exception ex)
			{
				obj.Status = 102;
				obj.Reason = "Error Occured While Getting Volunteer Data";

			}

			return obj;

		}

		public dynamic getSecretriattovolunteer(SecretriatVVModel root)
		{
			dynamic obj = new ExpandoObject();
			PRRDSPHelper spobj = new PRRDSPHelper();
			try
			{
				DataTable data = spobj.GetSecretraiatToVolunteer_Sp(root);
				if (data != null)
				{
					obj.Status = 100;
					obj.Reason = "Data Loaded Successfully.";
					obj.Details = data;
				}
				else
				{
					obj.Status = 102;
					obj.Reason = "No Data Found";
				}

			}
			catch (Exception ex)
			{
				obj.Status = 102;
				obj.Reason = "Error Occured While Getting Volunteer Data";

			}

			return obj;

		}

		#endregion

		#region Birth or Death Registration
		public dynamic LoadDistricts(DistrictsCls oj)
		{
			dynamic obj = new ExpandoObject();
			PRRDSPHelper spobj = new PRRDSPHelper();
			try
			{
				DataTable dt = spobj.LoadDistricts_helper(oj);
				if (dt != null && dt.Rows.Count > 0)
				{
					obj.Status = 100;
					obj.Reason = "";
					obj.Details = dt;
				}
				else
				{
					obj.Status = 101;
					obj.Reason = "No Data Found";
				}
			}
			catch (Exception ex)
			{
				obj.Status = 102;
				obj.Reason = "Error Occued While Loading Data";
				return obj;
			}
			return obj;
		}
        #endregion

        public bool Common_PRRD_Error(string msg, string url, string etype)
        {
            ExceptionDataModel objex = new ExceptionDataModel();
            try
            {
                objex.E_DEPTID = DepartmentEnum.Department.Panchayat_Raj_and_Rural_Development.ToString();
                objex.E_HODID = DepartmentEnum.HOD.Panchayati_Raj.ToString();
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
    }
}