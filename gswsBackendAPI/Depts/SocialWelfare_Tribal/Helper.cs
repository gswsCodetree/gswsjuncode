using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Threading.Tasks;
using System.Web;


namespace gswsBackendAPI.Depts.SocialWelfare_Tribal
{
	public class Helper : CommonSPHel
    {
		public dynamic Certificate_Download(string Certid, string Cert_type)
		{
			dynamic objdynamic = new ExpandoObject();
			try
			{
				var obj = new { strIntegratedID = Certid, CertType = Cert_type };
				var CertData = PostData_Auth_Certificate("http://ysrpk.ap.gov.in/Services/api/Mobile/Get_MeesevaCert_PDF_File_Sachivalayam", obj);
				var ResultData = JsonConvert.DeserializeObject(CertData);

				objdynamic.Status = ResultData.Status;
				objdynamic.Reason = ResultData.Reason;
				objdynamic.Data = ResultData.PDFPath;


			}
			catch (Exception ex)
			{
				objdynamic.Status = "Failure";
				objdynamic.Reason = ThirdpartyMessage;
				objdynamic.Data = "";
			}

			return objdynamic;
		}
		public dynamic Mrgcertificate_Download(dynamic UID)
		{
			dynamic objdynamic = new ExpandoObject();
			try
			{
				var obj = new { Aadhaar = UID.Aadhaar.ToString() };
				var CertData = PostData_Auth_MarriageCert("http://ysrpk.ap.gov.in/Services/api/Mobile/Get_MarriageCert_File_Sachivalayam", obj);
				var ResultData = JsonConvert.DeserializeObject(CertData);

				objdynamic.Status = ResultData.Status;
				objdynamic.Reason = ResultData.Reason;
				objdynamic.Data = ResultData.MarriageCertPath;


			}
			catch (Exception ex)
			{
				objdynamic.Status = "Failure";
				objdynamic.Reason = ThirdpartyMessage;
				objdynamic.Data = "";
			}

			return objdynamic;
		}

		public dynamic PK_StatusCheck(dynamic input)
		{
			dynamic objdynamic = new ExpandoObject();
			try
			{
				var InputAadhaar = encrypt(input.Aadhaar.ToString());
				var objinputs = new { Aadhaar = InputAadhaar };
				var obj = new
				{

					URL = "PelliKanukaDetailsForSachivalayam",
					strKey = "fe782860648f02893ac3438c5e9565f5",
					strUsername = "sachivalayam",
					strPswd = "sachivalayampellikanukadetails",
					strtype = "Statuscheck"
				};
				var CertData = PostData_Auth("", obj, objinputs);
				var ResultData = JsonConvert.DeserializeObject(CertData);

				objdynamic.Status = ResultData.Status;
				objdynamic.Reason = ResultData.Reason;
				objdynamic.Data = ResultData.PelliKanukaDetails;


			}
			catch (Exception ex)
			{
				objdynamic.Status = "Failure";
				objdynamic.Reason = ThirdpartyMessage;
				objdynamic.Data = "";
			}

			return objdynamic;
		}

		public dynamic Education_app_Check(S_W_Education objdata)
		{
			dynamic objdynamic = new ExpandoObject();
			try
			{
				var ObjCertificatecheck = "aadhar=" + objdata.Aadhaar.ToString() + "&acYear=" + objdata.Acadamicyear.ToString() + "&transId=12345&schemeId=" + objdata.Scheme.ToString();
				var CertData = GetData<dynamic>("https://jnanabhumi.ap.gov.in/jnbWebservices/services/prajaSachivalayam/getBillStatus?userId=admin&password=jnb@dmin20!9&" + ObjCertificatecheck);
				var ResultData = CertData;// JsonConvert.DeserializeObject(CertData);


				if (ResultData.errorMsg == "No Data Found")
				{
					objdynamic.Status = "Failure";
					objdynamic.Reason = ResultData.errorMsg;
					objdynamic.TrackingStatus = "";
					objdynamic.Deapartment = "";
					objdynamic.BeneficiaryID = "";
					objdynamic.BeneficiaryName = "";
				}
				else
				{
					objdynamic.Status = "Success";
					objdynamic.Reason = "Success";
					objdynamic.TrackingStatus = ResultData.status;
					objdynamic.Deapartment = ResultData.department;

					objdynamic.BeneficiaryID = ResultData.benfificiaryId;
					objdynamic.BeneficiaryName = ResultData.benfificiaryName;
				}


			}
			catch (Exception ex)
			{
				objdynamic.Status = "Failure";
				objdynamic.Reason = ThirdpartyMessage;
				objdynamic.TrackingStatus = "";
				objdynamic.Deapartment = "";
				objdynamic.BeneficiaryID = "";
				objdynamic.BeneficiaryName = "";
			}

			return objdynamic;
		}

		public dynamic GetFyn_Year()
		{
			dynamic Objres = new ExpandoObject();
			try
			{
				DataTable dt_Fy_Data = new DataTable();
				dt_Fy_Data.Clear();
				dt_Fy_Data.Columns.Add("Fny_ID");
				dt_Fy_Data.Columns.Add("Fny");
				int year = 2010, count = 1;
				int currentYear = DateTime.Now.Year;

				for (int i = year; i < currentYear + 1; i++)
				{
					DataRow _datarow = dt_Fy_Data.NewRow();
					_datarow["Fny_ID"] = i.ToString() + "-" + (i + 1).ToString().Substring(2, 2);
					_datarow["Fny"] = i.ToString() + "-" + (i + 1).ToString().Substring(2, 2);
					dt_Fy_Data.Rows.Add(_datarow);
					count++;
				}
				if (dt_Fy_Data != null)
				{
					Objres.Status = "Success";
					Objres.Reason = "Success";
					Objres.Data = dt_Fy_Data;
				}
				else
				{
					Objres.Status = "Failure";
					Objres.Reason = "Acadamic Years Not Generated";
					Objres.Data = "";
				}

			}
			catch (Exception)
			{
				Objres.Status = "Exception";
				Objres.Reason = ThirdpartyMessage;
				Objres.Data = "";

			}
			return Objres;
		}

		#region "Service Consuming cODE"

		public T GetData<T>(string url)
		{
			try
			{
				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				var req = (HttpWebRequest)WebRequest.Create(url);
				req.ContentType = "application/json; charset=utf-8";
				req.AllowAutoRedirect = false;
				var resp = req.GetResponse();
				var sr = new StreamReader(resp.GetResponseStream());
				var response = sr.ReadToEnd().Trim();

				var data = JsonConvert.DeserializeObject<T>(response);
				// data = Json.DeserializeObject<dynamic>(response);

				return data;
			}
			catch (Exception wex)
			{
                string mappath = HttpContext.Current.Server.MapPath("SocialWelfareExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + wex.Message.ToString()));

                throw wex;
			}
		}

		public dynamic PostData_Auth_Certificate(string url, dynamic jsonData)
		{
			var da = String.Empty;
			try
			{
				using (var client = new HttpClient())
				{
					string strKey = "fe782860648f02893ac3438c5e9565f5";
					Random rn = new Random();
					int RandomNumber = rn.Next(100000, 999999);
					client.BaseAddress = new Uri("http://ysrpk.ap.gov.in/Services/api/Mobile/");//
					client.DefaultRequestHeaders.Accept.Clear();

					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					string strpswd = sha256_hash(strKey + "sachivalayam@123" + RandomNumber.ToString());
					client.DefaultRequestHeaders.TryAddWithoutValidation("Username", "sachivalayam");
					client.DefaultRequestHeaders.TryAddWithoutValidation("Password", strpswd);
					client.DefaultRequestHeaders.TryAddWithoutValidation("Key", strKey);
					client.DefaultRequestHeaders.TryAddWithoutValidation("RandomNumber", RandomNumber.ToString());

					clsMeesevaCert_PDF_File objclsMeesevaCert_PDF_File = new clsMeesevaCert_PDF_File();
					objclsMeesevaCert_PDF_File.strIntegratedID = jsonData.strIntegratedID;
					objclsMeesevaCert_PDF_File.strType = jsonData.CertType;


					string strResult = JsonConvert.SerializeObject(objclsMeesevaCert_PDF_File);

					var content = new StringContent(strResult, Encoding.UTF8, "application/json");

					var response = client.PostAsync("Get_MeesevaCert_PDF_File_Sachivalayam", content).Result;

					da = response.Content.ReadAsStringAsync().Result;

				}
			}
			catch (WebException wex)
			{
                string mappath = HttpContext.Current.Server.MapPath("SocialWelfareExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + wex.Message.ToString()));

                throw wex;
			}

			return da;
		}

		public dynamic PostData_Auth_MarriageCert(string url, dynamic jsonData)
		{
			var da = String.Empty;
			try
			{
				using (var client = new HttpClient())
				{
					string strKey = "fe782860648f02893ac3438c5e9565f5";
					Random rn = new Random();
					int RandomNumber = rn.Next(100000, 999999);
					client.BaseAddress = new Uri("http://ysrpk.ap.gov.in/Services/api/Mobile/");//
					client.DefaultRequestHeaders.Accept.Clear();

					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					string strpswd = sha256_hash(strKey + "sachivalayam@MarriageCert" + RandomNumber.ToString());
					client.DefaultRequestHeaders.TryAddWithoutValidation("Username", "sachivalayam");
					client.DefaultRequestHeaders.TryAddWithoutValidation("Password", strpswd);
					client.DefaultRequestHeaders.TryAddWithoutValidation("Key", strKey);
					client.DefaultRequestHeaders.TryAddWithoutValidation("RandomNumber", RandomNumber.ToString());

					clsMarriageCert_PDF_File objclsMeesevaCert_PDF_File = new clsMarriageCert_PDF_File();
					objclsMeesevaCert_PDF_File.Aadhaar = jsonData.Aadhaar;


					string strResult = JsonConvert.SerializeObject(objclsMeesevaCert_PDF_File);

					var content = new StringContent(strResult, Encoding.UTF8, "application/json");

					var response = client.PostAsync("Get_MarriageCert_File_Sachivalayam", content).Result;

					da = response.Content.ReadAsStringAsync().Result;

				}
			}
			catch (WebException wex)
			{
                string mappath = HttpContext.Current.Server.MapPath("SocialWelfareExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + wex.Message.ToString()));

                throw wex;
			}

			return da;
		}

        public dynamic PostData_Auth(string url, dynamic jsonData, dynamic jsonCredentials)
		{
			var da = String.Empty;
			try
			{

				using (var client = new HttpClient())
				{
					//string strKey = "fe782860648f02893ac3438c5e9565f5";
					Random rn = new Random();
					int RandomNumber = rn.Next(100000, 999999);
					client.BaseAddress = new Uri("http://ysrpk.ap.gov.in/Services/api/Mobile/");//
					client.DefaultRequestHeaders.Accept.Clear();

					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					string strpswd = sha256_hash(jsonData.strKey + jsonData.strPswd + RandomNumber.ToString());
					client.DefaultRequestHeaders.TryAddWithoutValidation("Username", jsonData.strUsername);
					client.DefaultRequestHeaders.TryAddWithoutValidation("Password", strpswd);
					client.DefaultRequestHeaders.TryAddWithoutValidation("Key", jsonData.strKey);
					client.DefaultRequestHeaders.TryAddWithoutValidation("RandomNumber", RandomNumber.ToString());

					string strResult = string.Empty;
					if (jsonData.strtype == "Mrgcert")
					{
						clsMarriageCert_PDF_File objclsMeesevaCert_PDF_File = new clsMarriageCert_PDF_File();
						objclsMeesevaCert_PDF_File.Aadhaar = jsonCredentials.Aadhaar;

						strResult = JsonConvert.SerializeObject(objclsMeesevaCert_PDF_File);
					}
					else
						if (jsonData.strtype == "Statuscheck")
					{
						clsMarriageCert_PDF_File objclsMeesevaCert_PDF_File = new clsMarriageCert_PDF_File();
						objclsMeesevaCert_PDF_File.Aadhaar = jsonCredentials.Aadhaar;

						strResult = JsonConvert.SerializeObject(objclsMeesevaCert_PDF_File);
					}


					var content = new StringContent(strResult, Encoding.UTF8, "application/json");

					var response = client.PostAsync(jsonData.URL, content).Result;

					da = response.Content.ReadAsStringAsync().Result;

				}

			}
			catch (WebException wex)
			{
                string mappath = HttpContext.Current.Server.MapPath("SocialWelfareExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + wex.Message.ToString()));

                throw wex;
			}

			return da;
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

        #region "AES (Cryptography) algorithm ::"Encryption and decryption""
        public string encrypt(string encryptString)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }


        #endregion

        class clsMeesevaCert_PDF_File
		{
			public string strIntegratedID { set; get; }
			public string strType { set; get; }
		}

		class clsMarriageCert_PDF_File
		{
			public string Aadhaar { set; get; }
		}
		#endregion

	}
}