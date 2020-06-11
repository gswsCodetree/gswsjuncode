using gswsBackendAPI.Depts.REVENUE.Backend;
using gswsBackendAPI.transactionModule;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace gswsBackendAPI.DL.CommonHel
{
	public class EncryptDecrypt
	{
		private dynamic spandamurlmodel;

		public dynamic Encypt_dataRB(LoginModelrb obj)
		{
			dynamic objenc = new ExpandoObject();
			try
			{
				obj.Ftype = "1";
				obj.Fusername = "9666445262";
				obj.FPassword = "8688044922";
				string json = JsonConvert.SerializeObject(obj);

				string iv = CryptLib.GenerateRandomIV(16);
				string key = CryptLib.getHashSha256("Epragathi Rythuborasa", 31);

				string encrypttext = new CryptLib().encrypt(json, key, iv);

				objenc.Status = 100;
				objenc.encrypttext = encrypttext;
				objenc.key = iv;
				objenc.Reason = "";
				return objenc;
			}
			catch (Exception ex)
			{
				objenc.Status = 102;
				objenc.Reason = ex.Message.ToString();
				return objenc;

			}
		}

		public dynamic apEpdcl_Encryption(apEpdclModel dataobj)
		{
			dynamic objenc = new ExpandoObject();
			try
			{
				encryptionDataModel obj = new encryptionDataModel();
				obj.USERNAME = "admin";
				obj.PASSWORD = "admin@123";
				obj.PS_TXN_ID = getTransactionId(dataobj.secratariat_id);
				obj.RETURN_URL = "http://prajasachivalayam.ap.gov.in/PSTESTAPP/#!/Login";
				string json = JsonConvert.SerializeObject(obj);
				string iv = CryptLib.GenerateRandomIV(16);
				string key = "3fee5395f01bee349feed65629bd442a";

				string encrypttext = new CryptLib().encrypt(json, key, iv);

				objenc.Status = 100;
				objenc.encrypttext = encrypttext;
				objenc.key = iv;
				objenc.Reason = "";
				return objenc;
			}
			catch (Exception ex)
			{
				objenc.Status = 102;
				objenc.Reason = ex.Message.ToString();
				return objenc;

			}
		}

		public string getTransactionId(string transaction_id)
		{
			Random rd = new Random();
			return transaction_id + DateTime.Now.ToString("yyMMddHHmm") + rd.Next(111, 999).ToString();
		}



		public dynamic Encypt_data(EncryptDataModel obj)
		{
			dynamic objenc = new ExpandoObject();
			try
			{
				string json = JsonConvert.SerializeObject(obj);

				string iv = CryptLib.GenerateRandomIV(16);
				string key = CryptLib.getHashSha256("GSWS TEST", 32);

				string encrypttext = EncryptDecryptAlgoritham.EncryptStringAES(json, key, iv);

				objenc.Status = 100;
				objenc.encrypttext = encrypttext;
				objenc.key = iv;
				objenc.Reason = "";
				return objenc;
			}
			catch (Exception ex)
			{
				objenc.Status = 102;
				objenc.Reason = ex.Message.ToString();
				return objenc;

			}
		}

        public dynamic Encypt_datathird(string obj)
        {
            dynamic objenc = new ExpandoObject();
            try
            {
               // string json = JsonConvert.SerializeObject(obj);

                string iv = CryptLib.GenerateRandomIV(16);
                string key = CryptLib.getHashSha256("GSWS TEST", 32);

                string encrypttext = EncryptDecryptAlgoritham.EncryptStringAES(obj, key, iv);

				object obj1 = new
				{
					UserID = "codetreevs",
					Mobile = "",
					Password = "rtgsrc@123",
					RegMail = "",
					AppType = "codetreeration"
				};

				var data = new EncryptDecrypt().PostData(sapandanaurl.tokenurl, obj1);

				spandamurlmodel objspadana  = JsonConvert.DeserializeObject<spandamurlmodel>(data);

				if (objspadana.StatusCode == 200 && objspadana.Status == "Success")
				{
					objenc.Status = 100;
					objenc.encrypttext = encrypttext;
					objenc.key = iv;
					objenc.Reason = "";
					objenc.url = objspadana.url;
					objenc.SToken = objspadana.Token;
				}
				else
				{
					objenc.Status = 102;					
					objenc.Reason = objspadana.Message;
					//objenc.url = objspadana.url;
					objenc.SToken = objspadana.Token;
					
				}
                return objenc;
            }
            catch (Exception ex)
            {
                objenc.Status = 102;
                objenc.Reason = ex.Message.ToString();
                return objenc;

            }
        }

		

		public dynamic decypt_data(Decryptdatamodel obj)
		{
			dynamic objenc = new ExpandoObject();
			try
			{
			//	string json = JsonConvert.SerializeObject(obj);

				//string iv = CryptLib.GenerateRandomIV(16);
				//string key = CryptLib.getHashSha256("GSWS TEST", 31);

				string encrypttext = EncryptDecryptAlgoritham.DecryptStringAES(obj.encryprtext, obj.key, obj.Ivval);
				objenc.Status = "100";
				objenc.Result = encrypttext;
				return objenc;
			}
			catch (Exception ex)
			{
				objenc.Status = 102;
				objenc.Reason = ex.Message.ToString();
				return objenc;

			}
		}
		public dynamic Decry_data(Decryptdatamodel obj)
		{
			ResponseModel _objres = new ResponseModel();
			try
			{
				//string json = JsonConvert.SerializeObject(obj);

				//string iv = CryptLib.GenerateRandomIV(16);
				string key = CryptLib.getHashSha256("", 31);

				string encrypttext = new CryptLib().decrypt(obj.encryprtext, key, obj.Ivval);
				LoginModel objl = JsonConvert.DeserializeObject<LoginModel>(encrypttext);

				DataTable dtl = new DataTable();// GetLoginData_Sp(objl);
				if (dtl != null && dtl.Rows.Count > 0)
				{
					_objres.Status = 100;
					_objres.DataList = dtl;
				}
				else
				{
					_objres.Status = 102;
					_objres.DataList = dtl;
					_objres.Reason = "Invalid Username or Password";

				}
				return _objres;

			}
			catch (Exception ex)
			{
				_objres.Status = 102;

				_objres.Reason = ex.Message.ToString();
				return _objres;

			}
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



        public dynamic GetData(string url)
        {
            var response1=string.Empty;
            try
            { 
          
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "application/json";
				
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                     response1 = reader.ReadToEnd();
                 
                }
         
               
              }  
            catch(Exception ex)
            {
				throw new Exception(ex.Message);
			}
            return response1;
            
        }
		public dynamic GetspandanaData(string url, string token)
		{
			var response1 = string.Empty;
			try
			{
				
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Method = "GET";
				request.ContentType = "application/json";
				request.Headers.Add("Authorization",token);
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

		public dynamic SpandanaPostData(string url, dynamic jsonData,string token)
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
				req.Headers.Add("Authorization", token);
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

	}

	public class InputJsonClass
	{
		public string USERNAME { get; set; }
		public string PASSWORD { get; set; }
		public string PS_TXN_ID { get; set; }
		public string RETURN_URL { get; set; } 
		
	}
}