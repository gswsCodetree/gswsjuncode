using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using gswsBackendAPI.DL.CommonHel;
using Newtonsoft.Json;
using Microsoft.SqlServer.Server;
using System.Net.Mime;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using gswsBackendAPI.DL.DataConnection;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using gswsBackendAPI.transactionModule;

namespace gswsBackendAPI.Depts.Home.Backend
{
	public class HomeWebLHMS
	{
		OracleCommand cmd;
		CommonSPHel clda = new CommonSPHel();
		public dynamic GetLHMSCMDashboard()
		{
			try
			{
				var data = new EncryptDecrypt().GetData(LHMSURLS.CMDashboard);
				return JsonConvert.DeserializeObject<dynamic>(data);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public dynamic GetVehicleStatus(dynamic Objddata)
		{
			try
			{
               
                    dynamic objecthead = new ExpandoObject();
                    dynamic objecthead1 = new ExpandoObject();
                    //Basic Auth User Name="cctns" and Password="(ctn$(p@1"
                    objecthead.id = "Authorization";
                    objecthead.value = "Basic Y2N0bnM6KGN0biQocEAx";
                    var data = PostData_BasicAuth("http://61.0.227.152:8080/CPWebServices/services/StolenPropVehicleSearch", Objddata, objecthead);
				//  objecthead1.id = "EncryptString";
				//objecthead1.value = data;
				objecthead1.encryptString = data;
				objecthead1.key = "Q9qpYHMyrPxo9FCO5GTrRFrGs7TT2rgM3T6OQMTV8mI=";
				var ObjData = PostData("http://giripragati.ap.gov.in/GSWS/GSWS/GSWS_Resource/getdecryptString", objecthead1);
                    return JsonConvert.DeserializeObject<dynamic>(ObjData);
                
              
			}
			catch (Exception ex)
			{
				dynamic obj = new ExpandoObject();
				obj.Status = 102;
				obj.Reason = CommonSPHel.ThirdpartyMessage;
				string mappath = HttpContext.Current.Server.MapPath("HomeExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "GetVehicleStatus API:" + ex.Message.ToString()));

				return obj;
			}
		}

		public dynamic GetPropertyStatus(dynamic Objddata)
		{
			try
			{
				dynamic objecthead = new ExpandoObject();
				dynamic objecthead1 = new ExpandoObject();
				//Basic Auth User Name="cctns" and Password="(ctn$(p@1"
				objecthead.id = "Authorization";
				objecthead.value = "Basic Y2N0bnM6KGN0biQocEAx";
				var data = PostData_BasicAuth("http://61.0.227.152:8080/CPWebServices/services/StolenPropPropertySearch", Objddata, objecthead);
				//	objecthead1.id = "EncryptString";
				//	objecthead1.value = data;
				objecthead1.encryptString = data;
				objecthead1.key = "Q9qpYHMyrPxo9FCO5GTrRFrGs7TT2rgM3T6OQMTV8mI=";
				var ObjData = PostData("http://giripragati.ap.gov.in/GSWS/GSWS/GSWS_Resource/getdecryptString", objecthead1);
				return JsonConvert.DeserializeObject<dynamic>(ObjData);
			}
			catch (Exception ex)
			{
				dynamic obj = new ExpandoObject();
				obj.Status = 102;
				obj.Reason = CommonSPHel.ThirdpartyMessage;
				string mappath = HttpContext.Current.Server.MapPath("HomeExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "GetPropertyStatus API:" + ex.Message.ToString()));

				return obj;
			}
		}

		public dynamic GetPetitionStatus(dynamic Objddata)
		{
			dynamic objresult = new ExpandoObject();
			try
			{
				dynamic objecthead = new ExpandoObject();
				dynamic objecthead1 = new ExpandoObject();
				objecthead.id = "Authorization";
				objecthead.value = "Basic Y2N0bnM6KGN0biQocEAx";
				var data = PostData_BasicAuth("http://61.0.227.152:8080/CPWebServices/services/checkPetitionStatus", Objddata, objecthead);
				//objecthead1.id = "EncryptString";
				//objecthead1.value = data;
				objecthead1.encryptString = data;
				objecthead1.key = "Q9qpYHMyrPxo9FCO5GTrRFrGs7TT2rgM3T6OQMTV8mI=";
				var ObjData = PostData("http://giripragati.ap.gov.in/GSWS/GSWS/GSWS_Resource/getdecryptString", objecthead1);
				var resdata = JsonConvert.DeserializeObject<dynamic>(ObjData);
				var resultResponse = resdata[0];
				objresult.status = resultResponse.status;
				objresult.Data = resultResponse.viewPetitionReponse;
				objresult.Reason = resultResponse.errMsg;
			}
			catch (Exception ex)
			{
				dynamic obj = new ExpandoObject();
				string mappath = HttpContext.Current.Server.MapPath("HomeExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "GetPropertyStatus API:" + ex.Message.ToString()));

				obj.status = "Failure";
				obj.Data = "";
				obj.Reason = CommonSPHel.ThirdpartyMessage;

				return obj;
			}
			return objresult;
		}

		public dynamic GetEchallanStatus(EchallanModel obj)
		{
			try
			{

				var data = GetEchallansData("https://echallanapp.com/api/RTG/searchRc/" + obj.VehicleNum, "82185fc39c0201c8513041f288936a8a8c4a09707bf55bb0adc9207b42d22872");


				return JsonConvert.DeserializeObject<dynamic>(data);
			}
			catch (Exception ex)
			{
				dynamic obj1 = new ExpandoObject();
				string mappath = HttpContext.Current.Server.MapPath("HomeExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "GetPropertyStatus API:" + ex.Message.ToString()));

				obj1.status = "Failure";
				obj1.Data = "";
				obj1.Reason = CommonSPHel.ThirdpartyMessage;
				return obj;
			}
		}

		StreamReader reader = null;

		static WebClient client = null;

		public dynamic GetPetition_Print(dynamic Objddata)
		{
			dynamic objresult = new ExpandoObject();
			try
			{

				string urlAddress = Objddata.id; //"http://61.0.227.152:8080/CPWebServices/services/printPetition/2014012193900004";
				string filename = Objddata.FileName;

				var respath = DownloadFile(urlAddress, filename);
				objresult.Status = "Success";
				objresult.Reason = "File Saved Successfully";
				objresult.Path = respath;

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("HomeExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "GetPropertyStatus API:" + ex.Message.ToString()));

				objresult.Status = "Failure";
				objresult.Reason = CommonSPHel.ThirdpartyMessage;
				objresult.Path = "";

			}
			return objresult;
		}

		//Districts Load
		public dynamic Getdistricts(HomeSPModel objinpt)
		{
			dynamic objdist = new ExpandoObject();
			DataTable dt_null = new DataTable();
			try
			{
				DataTable dt_dist = GetHomeMaster_SP(objinpt);

				if (dt_dist != null)
				{
					objdist.Status = "Success";
					objdist.Reason = "";
					objdist.Data = dt_dist;
				}
				else
				{
					objdist.Status = "Failure";
					objdist.Reason = "";
					objdist.Data = dt_null;
				}
			}
			catch (Exception ex)
			{
				objdist.Status = "Failure";
				objdist.Reason = "";
				objdist.Data = dt_null;
			}
			return objdist;
		}

		#region
		public string DownloadFile(string urlAddress, string filename)
		{

			client = new WebClient();

			//DownlaodFile method directely downlaod file on you given specific path ,Here i've saved in E: Drive
			client.Headers[HttpRequestHeader.Authorization] = "Basic Y2N0bnM6KGN0biQocEAx";
			string fpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + "1234567890" + ".pdf";//Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "File.pdf";
																													  //client.DownloadFileAsync(new Uri(urlAddress), fpath);
			client.DownloadFile("http://61.0.227.152:8080/CPWebServices/services/printPetition/2014012193900004", fpath);
			////WebClient Client = new WebClient();


			return fpath;
		}



		public void dwnld()
		{
			string url = string.Empty;// Request.QueryString["DownloadUrl"];
			if (url == null || url.Length == 0)
			{
				url = "http://61.0.227.152:8080/CPWebServices/services/printPetition/2014012193900004";
			}

			//Initialize the input stream
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
			req.Headers.Add("Authorization", "Basic Y2N0bnM6KGN0biQocEAx");
			HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
			int bufferSize = 1;

			//Initialize the output stream
			System.Web.HttpContext.Current.Response.Clear();
			System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition:", "attachment; filename=download.jpg");
			System.Web.HttpContext.Current.Response.AppendHeader("Content-Length", resp.ContentLength.ToString());
			System.Web.HttpContext.Current.Response.ContentType = "application/download";

			//Populate the output stream
			byte[] ByteBuffer = new byte[bufferSize + 1];
			MemoryStream ms = new MemoryStream(ByteBuffer, true);
			Stream rs = req.GetResponse().GetResponseStream();
			byte[] bytes = new byte[bufferSize + 1];
			while (rs.Read(ByteBuffer, 0, ByteBuffer.Length) > 0)
			{
				System.Web.HttpContext.Current.Response.BinaryWrite(ms.ToArray());
				System.Web.HttpContext.Current.Response.Flush();
			}

			//Cleanup
			System.Web.HttpContext.Current.Response.End();
			ms.Close();
			ms.Dispose();
			rs.Dispose();
			ByteBuffer = null;
		}



		public static void DownloadData(string strFileUrlToDownload)

		{

			byte[] myDataBuffer = client.DownloadData((new Uri(strFileUrlToDownload)));



			MemoryStream storeStream = new MemoryStream();



			storeStream.SetLength(myDataBuffer.Length);

			storeStream.Write(myDataBuffer, 0, (int)storeStream.Length);



			storeStream.Flush();



			//TO save into certain file must exist on Local

			SaveMemoryStream(storeStream, "C:\\TestFile.txt");



			//The below Getstring method to get data in raw format and manipulate it as per requirement

			string download = Encoding.ASCII.GetString(myDataBuffer);



			Console.WriteLine(download);

			Console.ReadLine();

		}

		public static void SaveMemoryStream(MemoryStream ms, string FileName)

		{

			FileStream outStream = File.OpenWrite(FileName);

			ms.WriteTo(outStream);

			outStream.Flush();

			outStream.Close();

		}
		#endregion


		public dynamic GetCaseStatus(dynamic Objddata)
		{
			try
			{
				dynamic objecthead = new ExpandoObject();
				dynamic objecthead1 = new ExpandoObject();
				//Basic Auth User Name="cctns" and Password="(ctn$(p@1"
				objecthead.id = "Authorization";
				objecthead.value = "Basic Y2N0bnM6KGN0biQocEAx";
				var data = PostData_BasicAuth("http://61.0.227.152:8080/CPWebServices/services/viewCaseStatus", Objddata, objecthead);
				objecthead1.encryptString=data;
				//objecthead1.value = data;
				objecthead1.key = "Q9qpYHMyrPxo9FCO5GTrRFrGs7TT2rgM3T6OQMTV8mI=";
				var ObjData = PostData("http://giripragati.ap.gov.in/GSWS/GSWS/GSWS_Resource/getdecryptString", objecthead1);
				return JsonConvert.DeserializeObject<dynamic>(ObjData);
			}
			catch (Exception ex)
			{
				dynamic objresult = new ExpandoObject();
				string mappath = HttpContext.Current.Server.MapPath("HomeExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "GetCaseStatus:" + ex.Message.ToString()));
				objresult.Status = "Failure";
				objresult.Reason = CommonSPHel.ThirdpartyMessage;
				return objresult;
			}
		}
		public dynamic GetFIRStatus(dynamic Objddata)
		{
			try
			{
				dynamic objecthead = new ExpandoObject();
				dynamic objecthead1 = new ExpandoObject();
				//Basic Auth User Name="cctns" and Password="(ctn$(p@1"
				objecthead.id = "Authorization";
				objecthead.value = "Basic Y2N0bnM6KGN0biQocEAx";
				var data = PostData_BasicAuth("http://61.0.227.152:8080/CPWebServices/services/viewApprovedFIR", Objddata, objecthead);
				objecthead1.encryptString=data;
				objecthead1.key = "Q9qpYHMyrPxo9FCO5GTrRFrGs7TT2rgM3T6OQMTV8mI=";
				//objecthead1.value = data;
				var ObjData = PostData("http://giripragati.ap.gov.in/GSWS/GSWS/GSWS_Resource/getdecryptString", objecthead1);
				return JsonConvert.DeserializeObject<dynamic>(ObjData);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public dynamic GetArrestParticulars(dynamic Objddata)
		{
			try
			{
				dynamic objecthead = new ExpandoObject();
				dynamic objecthead1 = new ExpandoObject();
				//Basic Auth User Name="cctns" and Password="(ctn$(p@1"
				objecthead.id = "Authorization";
				objecthead.value = "Basic Y2N0bnM6KGN0biQocEAx";
				var data = PostData_BasicAuth("http://61.0.227.152:8080/CPWebServices/services/arrestParticulars", Objddata, objecthead);
				//objecthead1.id = "EncryptString";
				//objecthead1.value = data;
				objecthead1.encryptString = data;
				objecthead1.key = "Q9qpYHMyrPxo9FCO5GTrRFrGs7TT2rgM3T6OQMTV8mI=";
				var ObjData = PostData("http://giripragati.ap.gov.in/GSWS/GSWS/GSWS_Resource/getdecryptString", objecthead1);
				return JsonConvert.DeserializeObject<dynamic>(ObjData);
			}
			catch (Exception ex)
			{
				dynamic objresult = new ExpandoObject();
				string mappath = HttpContext.Current.Server.MapPath("HomeExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "GetArrestParticulars:" + ex.Message.ToString()));
				objresult.Status = "Failure";
				objresult.Reason = CommonSPHel.ThirdpartyMessage;
				return objresult;
			}
		}

		public dynamic GetWantedCriminals(dynamic Objddata)
		{
			try
			{
				dynamic objecthead = new ExpandoObject();
				dynamic objecthead1 = new ExpandoObject();
				//Basic Auth User Name="cctns" and Password="(ctn$(p@1"
				objecthead.id = "Authorization";
				objecthead.value = "Basic Y2N0bnM6KGN0biQocEAx";
				var data = PostData_BasicAuth("http://61.0.227.152:8080/CPWebServices/services/WantedPersonDetails", Objddata, objecthead);
				//objecthead1.id = "EncryptString";
				//objecthead1.value = data;
				objecthead1.encryptString = data;
				objecthead1.key = "Q9qpYHMyrPxo9FCO5GTrRFrGs7TT2rgM3T6OQMTV8mI=";
				var ObjData = PostData("http://giripragati.ap.gov.in/GSWS/GSWS/GSWS_Resource/getdecryptString", objecthead1);
				return JsonConvert.DeserializeObject<dynamic>(ObjData);
			}
			catch (Exception ex)
			{
				dynamic objresult = new ExpandoObject();
				string mappath = HttpContext.Current.Server.MapPath("HomeExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "GetWantedCriminals:" + ex.Message.ToString()));
				objresult.Status = "Failure";
				objresult.Reason = CommonSPHel.ThirdpartyMessage;
				return objresult;
			}
		}
		public dynamic GetUnknownBodies(dynamic Objddata)
		{
			try
			{
				dynamic objecthead = new ExpandoObject();
				dynamic objecthead1 = new ExpandoObject();
				//Basic Auth User Name="cctns" and Password="(ctn$(p@1"
				objecthead.id = "Authorization";
				objecthead.value = "Basic Y2N0bnM6KGN0biQocEAx";
				var data = PostData_BasicAuth("http://61.0.227.152:8080/CPWebServices/services/unknownDeadBodies", Objddata, objecthead);
				//objecthead1.id = "EncryptString";
				//objecthead1.value = data;
				objecthead1.encryptString = data;
				objecthead1.key = "Q9qpYHMyrPxo9FCO5GTrRFrGs7TT2rgM3T6OQMTV8mI=";
				var ObjData = PostData("http://giripragati.ap.gov.in/GSWS/GSWS/GSWS_Resource/getdecryptString", objecthead1);
				return JsonConvert.DeserializeObject<dynamic>(ObjData);
			}
			catch (Exception ex)
			{
				dynamic objresult = new ExpandoObject();
				string mappath = HttpContext.Current.Server.MapPath("HomeExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "GetUnknownBodies:" + ex.Message.ToString()));
				objresult.Status = "Failure";
				objresult.Reason = CommonSPHel.ThirdpartyMessage;
				return objresult;
			}
		}
		public dynamic GetMissedPersons(dynamic Objddata)
		{
			try
			{
				dynamic objecthead = new ExpandoObject();
				dynamic objecthead1 = new ExpandoObject();
				//Basic Auth User Name="cctns" and Password="(ctn$(p@1"
				objecthead.id = "Authorization";
				objecthead.value = "Basic Y2N0bnM6KGN0biQocEAx";
				var data = PostData_BasicAuth("http://61.0.227.152:8080/CPWebServices/services/MissingPersonDetails", Objddata, objecthead);
				//	objecthead1.id = "EncryptString";
				//	objecthead1.value = data;
				objecthead1.encryptString = data;
				objecthead1.key = "Q9qpYHMyrPxo9FCO5GTrRFrGs7TT2rgM3T6OQMTV8mI=";
				var ObjData = PostData("http://giripragati.ap.gov.in/GSWS/GSWS/GSWS_Resource/getdecryptString", objecthead1);
				return JsonConvert.DeserializeObject<dynamic>(ObjData);
			}
			catch (Exception ex)
			{
				dynamic objresult = new ExpandoObject();
				string mappath = HttpContext.Current.Server.MapPath("HomeExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "GetMissedPersons:" + ex.Message.ToString()));
				objresult.Status = "Failure";
				objresult.Reason = CommonSPHel.ThirdpartyMessage;
				return objresult;
			}
		}



		public dynamic GetLHMSCMDashboardbydate(string startdate, string enddate)
		{
			try
			{
				var data = new EncryptDecrypt().GetData(LHMSURLS.CMDashboard + "&start=" + startdate.Split('T')[0].Replace("/", "-") + "&end=" + enddate.Split('T')[0].Replace("/", "-"));
				return JsonConvert.DeserializeObject<dynamic>(data);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}



		//Procedures
		//procedure details
		public DataTable GetHomeMaster_SP(HomeSPModel ObjLGD) //ftype=4,district,5-mandal 6-gp
		{
			try
			{
				cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_GET_HOME_DETAILS";
				cmd.Parameters.Add("PTYPE", OracleDbType.Int32).Value = Convert.ToInt32(ObjLGD.PTYPE);
				cmd.Parameters.Add("PDIST", OracleDbType.Int32).Value = Convert.ToInt32(ObjLGD.PDIST);
				cmd.Parameters.Add("PRANGE_CD", OracleDbType.Int32).Value = Convert.ToInt32(ObjLGD.PRANGE_CD);

				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtLogin = clda.GetgswsDataAdapter(cmd);
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
				string mappath = HttpContext.Current.Server.MapPath("HOME_MAstersSP");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetHomeMaster_SP:" + ex.Message.ToString()));
				throw ex;
			}
		}


		//Consume APIs
		public dynamic PostData_BasicAuth(string url, dynamic jsonData, dynamic headersvalues)
		{
			var response = String.Empty;
			try
			{
				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				var req = (HttpWebRequest)WebRequest.Create(url);
				req.Credentials = CredentialCache.DefaultCredentials;
				WebProxy myProxy = new WebProxy();
				//myProxy.Address = new Uri("http://proxy.uk.research-int.com:8080"); ;
				//myProxy.UseDefaultCredentials = true;
				req.Proxy = myProxy;

				req.Method = "POST";

				req.Headers.Add("Authorization", "Basic Y2N0bnM6KGN0biQocEAx");
				req.Headers.Add("Access-Control-Allow-Origin", "*");
				//req.Headers.Add(headersvalues.id, headersvalues.value);
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
					// response = GetData(resp.Headers["Location"]);
				}
				else
				{
					response = sr.ReadToEnd().Trim();
				}


			}
			catch (WebException wex)
			{
				throw new Exception(wex.Message);
			}

			return response;
		}



		public dynamic GetEchallansData(string url, string token)
		{
			var response1 = string.Empty;
			try
			{

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Method = "GET";
				request.ContentType = "application/json";
				request.Headers.Add("Authorization", "Bearer " + token);
				WebResponse response = request.GetResponse();
				using (var reader = new StreamReader(response.GetResponseStream()))
				{
					response1 = reader.ReadToEnd();

				}


			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return response1;

		}

		public dynamic PostData(string url, dynamic jsonData)
		{
			var response = String.Empty;
			try
			{
				//System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

				var req = (HttpWebRequest)WebRequest.Create(url);
				req.Credentials = CredentialCache.DefaultCredentials;
				WebProxy myProxy = new WebProxy();
				//myProxy.Address = new Uri("http://proxy.uk.research-int.com:8080"); ;
				//myProxy.UseDefaultCredentials = true;
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
					// response = GetData(resp.Headers["Location"]);
				}
				else
				{
					response = sr.ReadToEnd().Trim();
				}


			}
			catch (WebException wex)
			{
				//	Logfile_eX("VERIFYOTP ELIGIBILITY CHECK SERVICE EXCEPTION::", wex.Message);
				throw new Exception(wex.Message);
			}

			return response;
		}

		public dynamic DownloadFIRFile(dynamic Objddata)
		{
			dynamic objdata = new ExpandoObject();
			try
			{

				dynamic objecthead = new ExpandoObject();
				dynamic objecthead1 = new ExpandoObject();
				//Basic Auth User Name="cctns" and Password="(ctn$(p@1"
				objecthead.id = "Authorization";
				objecthead.value = "Basic Y2N0bnM6KGN0biQocEAx";
				string response = PostData_DownloadFile("http://61.0.227.152:8080/CPWebServices/services/printFIR", Objddata, objecthead);
				if (string.IsNullOrEmpty(response))
				{
					objdata.status = 400;
					objdata.result = "Service Returns Empty Response";
				}
				else
				{
					objdata.status = 200;
					objdata.result = response;
				}
				//return JsonConvert.DeserializeObject<dynamic>(ObjData);
			}
			catch (Exception ex)
			{
				objdata.status = 500;
				objdata.result = ex.Message.ToString();
			}
			return objdata;
		}

		public dynamic PostData_DownloadFile(string url, dynamic jsonData, dynamic headersvalues)
		{
			var response = String.Empty;
			try
			{
				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				var req = (HttpWebRequest)WebRequest.Create(url);
				req.Credentials = CredentialCache.DefaultCredentials;
				WebProxy myProxy = new WebProxy();
				//myProxy.Address = new Uri("http://proxy.uk.research-int.com:8080"); ;
				//myProxy.UseDefaultCredentials = true;
				req.Proxy = myProxy;

				req.Method = "POST";

				req.Headers.Add("Authorization", "Basic Y2N0bnM6KGN0biQocEAx");
				req.Headers.Add("Access-Control-Allow-Origin", "*");
				//req.Headers.Add(headersvalues.id, headersvalues.value);
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
					// response = GetData(resp.Headers["Location"]);
				}
				else
				{
					var bytes = default(byte[]);
					using (var memstream = new MemoryStream())
					{
						sr.BaseStream.CopyTo(memstream);
						bytes = memstream.ToArray();
					}
					response = Convert.ToBase64String(bytes);
				}
			}
			catch (WebException wex)
			{
				throw new Exception(wex.Message);
			}

			return response;
		}

		#region LHMS

		public dynamic GetLHMSDistrictData(HomeSPModel obj)
		{
			try
			{

				var data = GetEchallansData("http://appolice.co.in/lhms/api/v1/getAppMasterService?token=" + "IU56jhgu567JGJH7", "");
				return JsonConvert.DeserializeObject<dynamic>(data);
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("HomeExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "GetMissedPersons:" + ex.Message.ToString()));

				throw ex;
			}
		}

		//Save LHMS Data
		public dynamic SaveLHMSData(LHMSMODELNEW Objdata)
		{
			dynamic obj = new ExpandoObject();
			try
			{

				var Data = LHMSPostData("http://appolice.co.in/lhms/api/v1/addNewOwner", Objdata);
				//return JsonConvert.DeserializeObject<dynamic>(Data);

				dynamic val = JsonConvert.DeserializeObject<dynamic>(Data);
				if (val.success == true)
				{
					transactionModel objtrans = new transactionModel();
					objtrans.TYPE = "2";
					objtrans.TXN_ID = Objdata.gsws_id;
					objtrans.DEPT_ID = "2101";
					objtrans.BEN_ID = val.owner.id;
					objtrans.DEPT_TXN_ID = val.owner.owner_id;
					objtrans.STATUS_CODE = "01";
					objtrans.REMARKS = val.message;
					try
					{
						DataTable dt = new transactionHelper().transactionInsertion(objtrans);
						if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
						{
							return JsonConvert.DeserializeObject<dynamic>(Data);
						}
						else
						{
							return JsonConvert.DeserializeObject<dynamic>(Data);
						}
					}
					catch (Exception ex)
					{
						string mappath = HttpContext.Current.Server.MapPath("LHMSResponseLogs");
						Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "SaveLHMSData:" + ex.Message.ToString()+ Data));

						return JsonConvert.DeserializeObject<dynamic>(Data);
					}
				}
				else
				{
					obj.success = false;
					obj.message = "Submit Failure";
					return obj;
				}
			}
			catch (Exception ex)
			{
				obj.success = false;
				obj.message = CommonSPHel.ThirdpartyMessage;
				string mappath = HttpContext.Current.Server.MapPath("HomeExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "SaveLHMSData:" + ex.Message.ToString()));

				return obj;
			}
		}

		//Save LHMS Watch Request Data
		public dynamic SaveLHMSWatchRequestData(LHMSWATCHREQ Objdata)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				string strtdate = Convert.ToDateTime(Objdata.start_date).ToString("yyyy-MM-dd").ToString();
				string enddate = Convert.ToDateTime(Objdata.end_date).ToString("yyyy-MM-dd").ToString();
				string starttime = Convert.ToDateTime(Objdata.start_time).ToString("HH:mm:ss").ToString();
				string endtime = Convert.ToDateTime(Objdata.end_time).ToString("HH:mm:ss").ToString();

				Objdata.start_date = strtdate + " " + starttime;
				Objdata.end_date = enddate + " " + endtime;

				var ObjData = LHMSPostData("http://appolice.co.in/lhms/api/v1/addWatchRequest", Objdata);
				return JsonConvert.DeserializeObject<dynamic>(ObjData);
			}
			catch (Exception ex)
			{

				obj.success = false;
				obj.message = CommonSPHel.ThirdpartyMessage;
				string mappath = HttpContext.Current.Server.MapPath("HomeExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "SaveLHMSWatchRequestData:" + ex.Message.ToString()));

				throw ex;
			}
		}

		public dynamic LHMSPostData(string url, dynamic jsonData)
		{
			var response = String.Empty;
			try
			{
				//System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

				var req = (HttpWebRequest)WebRequest.Create(url);
				req.Credentials = CredentialCache.DefaultCredentials;
				WebProxy myProxy = new WebProxy();
				//myProxy.Address = new Uri("http://proxy.uk.research-int.com:8080"); ;
				//myProxy.UseDefaultCredentials = true;
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
					// response = GetData(resp.Headers["Location"]);
				}
				else
				{
					response = sr.ReadToEnd().Trim();
				}


			}
			catch (WebException wex)
			{
				//	Logfile_eX("VERIFYOTP ELIGIBILITY CHECK SERVICE EXCEPTION::", wex.Message);
				throw new Exception(wex.Message);
			}

			return response;
		}

		#endregion

		//CrimePetition
		public dynamic SaveHome_Crime_Petition(dynamic Objdata)
		{
			try
			{
				dynamic objecthead = new ExpandoObject();
				dynamic objecthead1 = new ExpandoObject();
				//Basic Auth User Name="cctns" and Password="((%N$@!(j$"
				objecthead.id = "Authorization";
				objecthead.value = "Basic Y2N0bnM6KGN0biQocEAx";
				var data = PostDataServ_BasicAuth("http://61.0.227.152:8080/ICJSService/rest/Service/crimePetitionDetails", Objdata, objecthead);
				objecthead1.encryptString = data;
				objecthead1.key = "yxvuicidunccxnxzquidxstraitsdxsunmaitrxadxtruityharmunix";
				var ObjData = PostData("http://giripragati.ap.gov.in/GSWS/GSWS/GSWS_Resource/getdecryptString", objecthead1);

				string mappath = HttpContext.Current.Server.MapPath("CrimePetitionResponseLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Response for Transaction  :" + Objdata.gws_id.ToString() + " , Response Message : " + ObjData.ToString()));

				var response = JsonConvert.DeserializeObject<dynamic>(ObjData);
				if (response.success == "true")
				{
					try
					{
						transactionModel objtrans = new transactionModel();
						objtrans.TYPE = "2";
						objtrans.TXN_ID = Objdata.gws_id.ToString();
						objtrans.DEPT_ID = "2101";
						objtrans.DEPT_TXN_ID = response.cctns_pet_id;
						objtrans.STATUS_CODE = response.statuscode;
						objtrans.REMARKS = response.message.ToString() + " " + response.cctns_pet_subject.ToString();
						DataTable dt = new transactionHelper().transactionInsertion(objtrans);
					}
					catch (Exception ex)
					{
						string mappath2 = HttpContext.Current.Server.MapPath("CrimePetitionErrorLogs");
						Task WriteTask2 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath2, "Error  Message :" + ex.Message.ToString()));
					}
				}

				return response;
			}
			catch (Exception ex)
			{
				string mappath3 = HttpContext.Current.Server.MapPath("CrimePetitionErrorLogs");
				Task WriteTask3 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath3, "Error  Message :" + ex.Message.ToString()));
				throw ex;
			}
		}

		public dynamic PostDataServ_BasicAuth(string url, dynamic jsonData, dynamic headersvalues)
		{
			var response = String.Empty;
			try
			{
				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				var req = (HttpWebRequest)WebRequest.Create(url);
				req.Credentials = CredentialCache.DefaultCredentials;
				WebProxy myProxy = new WebProxy();
				//myProxy.Address = new Uri("http://proxy.uk.research-int.com:8080"); ;
				//myProxy.UseDefaultCredentials = true;
				req.Proxy = myProxy;

				req.Method = "POST";

				req.Headers.Add("Authorization", "Basic Y2N0bnM6KCglTiRAIShqJA==");
				req.Headers.Add("Access-Control-Allow-Origin", "*");
				//req.Headers.Add("SecurityKey", "yxvuicidunccxnxzquidxstraitsdxsunmaitrxadxtruityharmunix");
				//req.Headers.Add(headersvalues.id, headersvalues.value);
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
					// response = GetData(resp.Headers["Location"]);
				}
				else
				{
					response = sr.ReadToEnd().Trim();
				}


			}
			catch (WebException wex)
			{
				throw new Exception(wex.Message);
			}

			return response;
		}


	}

	public static class LHMSURLS
	{
		public static string CMDashboard = "http://appolice.co.in/lhms/api/v1/getCmDashboardData?token=jhG567GJ654FH7J987";


	}

	public class LHMSMODEL
	{
		public string Ftype { get; set; }
		public string Startdate { get; set; }
		public string enddate { get; set; }
	}

	public class EchallanModel
	{
		public string VehicleNum { get; set; }
		public string token { get; set; }
	}

	public class HomeSPModel
	{
		public string PTYPE { set; get; }
		public string PDIST { set; get; }
		public string PRANGE_CD { set; get; }
	}

	public class ArrestParticulars
	{
		public string psCd { set; get; }
		public string districtCd { set; get; }
		public string majorHead { set; get; }
		public string fromDt { set; get; }
		public string toDt { set; get; }
		public string gender { set; get; }
		public string age { set; get; }
		public string name { set; get; }
	}
	public class HOMEKnowCaseStatus
	{
		public string districtCd { set; get; }
		public string psCd { set; get; }
		public string firNo { set; get; }
	}
	public class HOMEFIRStatus
	{
		public string districtCd { set; get; }
		public string psCd { set; get; }
		public string firNo { set; get; }
		public string firYear { set; get; }
		public string firDate { set; get; }
	}
	public class HOMEMissedKidnapped
	{
		public string age { set; get; }
		public string gender { set; get; }
		public string fromDt { set; get; }
		public string toDt { set; get; }
		public string name { set; get; }
		public string missingPlace { set; get; }
		public string heightFeet { set; get; }
		public string heightInch { set; get; }
	}

	public class HOMEWantedCriminals
	{
		public string psCd { set; get; }
		public string districtCd { set; get; }
		public string majorHead { set; get; }
		public string gender { set; get; }
		public string age { set; get; }
		public string name { set; get; }
	
	}

	public class HOMEUnknownBodies
	{
		public string age { set; get; }
		public string gender { set; get; }
		public string fromDt { set; get; }
		public string toDt { set; get; }
		public string heightFeet { set; get; }
		public string heightInch { set; get; }
	}
	
	public class HOMERecoverVehcle
	{
		public string toDt { set; get; }
		public string fromDt { set; get; }
		public string vehicleRegnum { set; get; }
	}

	public class HOMEPetitionCheck
	{
		public string petitionId { set; get; }
		public string petName { set; get; }
	}


	public class LHMSMODELNEW
	{

		public string name { get; set; }
		public string address { get; set; }
		public string mobile_number { get; set; }
		public string latitude { get; set; }
		public string longitude { get; set; }
		public string town_id { get; set; }
		public string municipal_id { get; set; }
		public string gsws_id { get; set; }
		public string token { get; set; }
	}

	public class LHMSWATCHREQ
	{
		public string token { get; set; }
		public string owner_id { get; set; }
		public string start_date { get; set; }
		public string start_time { get; set; }
		public string end_date { get; set; }
		public string end_time { get; set; }
	}

	public class HOMEFIRDownload
	{
		public string districtCd { set; get; }
		public string psCd { set; get; }
		public string firRegNum { set; get; }
		public string sdpoCd { set; get; }
		public string circleCd { set; get; }
		public string firNo { set; get; }
	}

}