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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace gswsBackendAPI.DL.DataConnection
{
	
	public class token_gen
	{

		public static int expiry_seconds { set; get; }
		public static int expiry_minutes { set; get; }
		public static int expiry_hours { set; get; }
		public static int expiry_days { set; get; }
		public static int expiry_months { set; get; }
		public static int expiry_year { set; get; }
		public static string PRIMARY_MACHINE_KEY { set; get; }
		public static string SECONDARY_MACHINE_KEY { set; get; }
		private static string token_issuer { set; get; }
		private static string token_audience { set; get; }
		private static List<string> roles { get; set; }
		private static string verify_issuer { get; set; }
		private static string verify_audience { get; set; }
		private static Dictionary<string, string> Response_Dictionary { get; set; }


		public static void initialize(string token_issuer = "", string token_audience = "")
		{
			try
			{
				token_gen.Response_Dictionary = new Dictionary<string, string>();
				token_gen.roles = new List<string>();
				token_gen.token_issuer = token_issuer;
				token_gen.token_audience = token_audience;
				token_gen.check_token_status();
			}
			catch (Exception ex)
			{
				throw new HttpResponseException(HttpStatusCode.BadRequest);
			}
		}

		public static string Authorize(string data, List<string> authorize_roles = null)
		{
			string[] values = data.Split('.');
			byte[] buffer = FromBase64Url(values[0]);
			string KEY = Encoding.UTF8.GetString(buffer);
			byte[] buffer_1 = FromBase64Url(values[1]);
			string PAY_LOAD = Encoding.UTF8.GetString(buffer_1);
			byte[] buffer_2 = FromBase64Url(values[2]);
			string signature = Encoding.UTF8.GetString(buffer_2).ToString();
			if (!signature_check(KEY, PAY_LOAD, signature))
				throw new HttpResponseException(HttpStatusCode.Unauthorized);
			string token_json_format = Decrypt_Data(KEY, PRIMARY_MACHINE_KEY, SECONDARY_MACHINE_KEY);
			request_model token_params = JsonConvert.DeserializeObject<request_model>(token_json_format);
			if (!expiry_time_check(token_params.expiry_time))
				throw new HttpResponseException(get_status(428, "Session Expired !!!"));
			if (!roles_check(token_params.roles, authorize_roles))
				throw new HttpResponseException(get_status(401, "Unauthorized Request !!!"));
			if (!issuer_check(token_params.issuer))
				throw new HttpResponseException(get_status(401, "Unauthorized Request !!!"));
			if (!audience_check(token_params.audience))
				throw new HttpResponseException(get_status(401, "Unauthorized Request !!!"));
			return PAY_LOAD;
		}

		public static string Authorize_aesdecrpty (string data, List<string> authorize_roles = null)
		{
			string[] values = data.Split('.');
			byte[] buffer =Encoding.UTF8.GetBytes(Decrypt_Data(values[0], "8080808080808080", "8080808080808080"));
			string KEY = Encoding.UTF8.GetString(buffer);
			byte[] buffer_1 =  Encoding.UTF8.GetBytes(Decrypt_Data(values[1], "8080808080808080", "8080808080808080"));//FromBase64Url(values[1]);
			string PAY_LOAD = Encoding.UTF8.GetString(buffer_1);
			byte[] buffer_2 =  FromBase64Url(values[2]);
			//string signature = Encoding.UTF8.GetString(buffer_2).ToString();
			//if (!signature_check(KEY, PAY_LOAD, signature))
			//	throw new HttpResponseException(HttpStatusCode.Unauthorized);
			string token_json_format = Decrypt_Data(KEY, PRIMARY_MACHINE_KEY, SECONDARY_MACHINE_KEY);
			request_model token_params = JsonConvert.DeserializeObject<request_model>(token_json_format);
			if (!expiry_time_check(token_params.expiry_time))
				throw new HttpResponseException(get_status(428, "Session Expired !!!"));
			if (!roles_check(token_params.roles, authorize_roles))
				throw new HttpResponseException(get_status(401, "Unauthorized Request !!!"));
			if (!issuer_check(token_params.issuer))
				throw new HttpResponseException(get_status(401, "Unauthorized Request !!!"));
			if (!audience_check(token_params.audience))
				throw new HttpResponseException(get_status(401, "Unauthorized Request !!!"));
			return PAY_LOAD;
		}

		public static string Authorize_Logstr(string data, List<string> authorize_roles = null)
		{
			string[] values = data.Split('.');
			byte[] buffer = FromBase64Url(values[0]);
			string KEY = Encoding.UTF8.GetString(buffer);
			byte[] buffer_1 = FromBase64Url(values[1]);
			string PAY_LOAD = Encoding.UTF8.GetString(buffer_1);
			byte[] buffer_2 = FromBase64Url(values[2]);
			string signature = Encoding.UTF8.GetString(buffer_2).ToString();
			//if (!signature_check(KEY, PAY_LOAD, signature))
			//	throw new HttpResponseException(HttpStatusCode.Unauthorized);
			string token_json_format = Decrypt_Data(KEY, PRIMARY_MACHINE_KEY, SECONDARY_MACHINE_KEY);
			request_model token_params = JsonConvert.DeserializeObject<request_model>(token_json_format);
			if (!expiry_time_check(token_params.expiry_time))
				throw new HttpResponseException(get_status(428, "Session Expired !!!"));
			if (!roles_check(token_params.roles, authorize_roles))
				throw new HttpResponseException(get_status(401, "Unauthorized Request !!!"));
			if (!issuer_check(token_params.issuer))
				throw new HttpResponseException(get_status(401, "Unauthorized Request !!!"));
			if (!audience_check(token_params.audience))
				throw new HttpResponseException(get_status(401, "Unauthorized Request !!!"));
			return PAY_LOAD;
		}
		public static dynamic generate_token()
		{
			try
			{
				dynamic objdata = new ExpandoObject();
				objdata.issued_time = DateTime.UtcNow;
				objdata.expiry_time = generate_expiry_time();
				objdata.unique_number = GetUniqueKey(16);
				objdata.issuer = get_issuer();
				objdata.audience = get_audience();
				List<string> value = get_user_roles();
				objdata.roles = value;
				string token_data = JsonConvert.SerializeObject(objdata);
				string encrypted_value = Encrypt_Data(token_data, PRIMARY_MACHINE_KEY, SECONDARY_MACHINE_KEY);
				dynamic response = new ExpandoObject();
				Response_Dictionary.Add("access_token", encrypted_value);
				return Response_Dictionary;
			}
			catch (Exception ex)
			{
				throw new HttpResponseException(HttpStatusCode.BadRequest);
			}

		}
		public static void addResponse(string key, string value)
		{
			Response_Dictionary.Add(key, value);
		}
		public static void addClaim(string claim)
		{
			addRole(claim);
		}
		public static void check_token_status(string token_issuer = "", string token_audience = "")
		{
			token_origin_check(token_issuer, token_audience);
		}
		private static void token_origin_check(string issuer, string audience)
		{
			token_gen.verify_issuer = issuer;
			token_gen.verify_audience = audience;
		}
		private static dynamic get_user_roles()
		{
			if (roles != null && roles.Count > 0)
				return roles;
			else
				return new List<string>();
		}
		private static void addRole(string role)
		{
			if (string.IsNullOrEmpty(role))
				throw new NoNullAllowedException("Input Data Not Found");
			else
				roles.Add(role);
		}
		private static string get_audience()
		{
			if (string.IsNullOrEmpty(token_audience))
				return "";
			else
				return token_audience;
		}
		private static string get_issuer()
		{
			if (string.IsNullOrEmpty(token_issuer))
				return "";
			else
				return token_issuer;
		}
		private static string GetUniqueKey(int maxSize)
		{
			char[] chars = new char[62];
			chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
			byte[] data = new byte[1];
			using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
			{
				crypto.GetNonZeroBytes(data);
				data = new byte[maxSize];
				crypto.GetNonZeroBytes(data);
			}
			StringBuilder result = new StringBuilder(maxSize);
			foreach (byte b in data)
			{
				result.Append(chars[b % (chars.Length)]);
			}
			return result.ToString();
		}
		private static string generate_expiry_time()
		{
			string date = "";
			if (expiry_seconds != 0)
				date = DateTime.UtcNow.AddSeconds(expiry_seconds).ToString();
			else if (expiry_minutes != 0)
				date = DateTime.UtcNow.AddMinutes(expiry_minutes).ToString();
			else if (expiry_hours != 0)
				date = DateTime.UtcNow.AddHours(expiry_hours).ToString();
			else if (expiry_days != 0)
				date = DateTime.UtcNow.AddDays(expiry_days).ToString();
			else if (expiry_months != 0)
				date = DateTime.UtcNow.AddMonths(expiry_months).ToString();
			else if (expiry_year != 0)
				date = DateTime.UtcNow.AddYears(expiry_year).ToString();
			else
				new NullReferenceException("No Expiry Time initialized");
			return date;
		}
		private static byte[] FromBase64Url(string base64Url)
		{
			string padded = base64Url.Length % 4 == 0
				? base64Url : base64Url + "====".Substring(base64Url.Length % 4);
			string base64 = padded.Replace("_", "/")
								  .Replace("-", "+");
			return Convert.FromBase64String(base64);
		}
		private static string HMAC_SHA256(string message, string key)
		{
			var encoding = new System.Text.ASCIIEncoding();
			byte[] keyByte = encoding.GetBytes(key);
			byte[] messageBytes = encoding.GetBytes(message);
			using (var hmacsha256 = new HMACSHA256(keyByte))
			{
				byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
				return Convert.ToBase64String(hashmessage);
			}
		}
		private static string Encrypt_Data(string data, string key_value, string iv_value)
		{
			var keybytes = Encoding.UTF8.GetBytes(key_value);
			var iv = Encoding.UTF8.GetBytes(iv_value);
			var encryptStringToBytes = EncryptStringToBytes(data, keybytes, iv);
			return Convert.ToBase64String(encryptStringToBytes);
		}
		private static string Decrypt_Data(string data, string key_value, string iv_value)
		{
			var enc_value = Convert.FromBase64String(data);
			var keybytes = Encoding.UTF8.GetBytes(key_value);
			var iv = Encoding.UTF8.GetBytes(iv_value);
			var roundtrip = DecryptStringFromBytes(enc_value, keybytes, iv);
			return roundtrip;
		}
		private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
		{
			if (cipherText == null || cipherText.Length <= 0)
			{
				throw new ArgumentNullException("cipherText");
			}
			if (key == null || key.Length <= 0)
			{
				throw new ArgumentNullException("key");
			}
			if (iv == null || iv.Length <= 0)
			{
				throw new ArgumentNullException("key");
			}

			string plaintext = null;
			using (var rijAlg = new RijndaelManaged())
			{
				rijAlg.Mode = CipherMode.CBC;
				rijAlg.Padding = PaddingMode.PKCS7;
				rijAlg.FeedbackSize = 128;
				rijAlg.Key = key;
				rijAlg.IV = iv;
				var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
				using (var msDecrypt = new MemoryStream(cipherText))
				{
					using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
					{
						using (var srDecrypt = new StreamReader(csDecrypt))
						{
							plaintext = srDecrypt.ReadToEnd();
						}
					}
				}
			}

			return plaintext;
		}
		private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
		{
			if (plainText == null || plainText.Length <= 0)
			{
				throw new ArgumentNullException("plainText");
			}
			if (key == null || key.Length <= 0)
			{
				throw new ArgumentNullException("key");
			}
			if (iv == null || iv.Length <= 0)
			{
				throw new ArgumentNullException("key");
			}

			byte[] encrypted;
			using (var rijAlg = new RijndaelManaged())
			{
				rijAlg.Mode = CipherMode.CBC;
				rijAlg.Padding = PaddingMode.PKCS7;
				rijAlg.FeedbackSize = 128;
				rijAlg.Key = key;
				rijAlg.IV = iv;
				var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
				using (var msEncrypt = new MemoryStream())
				{
					using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						using (var swEncrypt = new StreamWriter(csEncrypt))
						{
							swEncrypt.Write(plainText);
						}
						encrypted = msEncrypt.ToArray();
					}
				}
			}

			return encrypted;
		}
		private static bool expiry_time_check(string date)
		{
			DateTime expiry_date = DateTime.Parse(date);
			DateTime current_date = DateTime.UtcNow;
			int date_difference = Convert.ToInt32((expiry_date - current_date).TotalSeconds);
			if (date_difference > 0)
				return true;
			return false;
		}
		private static bool signature_check(string key, string pay_load, string signature)
		{
			string generated_string = HMAC_SHA256(pay_load, key);
			if (generated_string != signature)
				return false;
			else
				return true;
		}

		private static bool roles_check(List<string> roles, List<string> authorize_roles)
		{
			if (roles == null || roles.Count < 1)
				return true;
			if (authorize_roles != null && authorize_roles.Count > 0)
			{
				for (int i = 0; i < authorize_roles.Count; i++)
				{
					if (roles.Contains(authorize_roles[i]))
						return true;
				}
				return false;
			}

			return true;
		}



		private static bool issuer_check(string issuer)
		{
			if (issuer == verify_issuer)
				return true;
			return false;
		}
		private static bool audience_check(string audience)
		{
			if (audience == verify_audience)
				return true;
			return false;
		}
		private class request_model
		{
			public string issued_time { get; set; }
			public string expiry_time { get; set; }
			public string unique_number { get; set; }
			public string issuer { get; set; }
			public string audience { get; set; }
			public List<string> roles { get; set; }

		}
		private static HttpResponseMessage get_status(int status_code, string message)
		{
			dynamic objdata = new ExpandoObject();
			objdata.Status = status_code;
			objdata.Reason = message;
			string rootdata = JsonConvert.SerializeObject(objdata);
			HttpResponseMessage responseMessage = new HttpResponseMessage()
			{
				Content = new StringContent(rootdata)
			};
			responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			return responseMessage;
		}

		internal static string Authorize_aesdecrpty(object data)
		{
			throw new NotImplementedException();
		}
	}
}
