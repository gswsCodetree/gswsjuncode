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
using static gswsBackendAPI.Depts.Women_Children_Disabled_and_SeniorCitizen.Model;
using gswsBackendAPI.transactionModule;

namespace gswsBackendAPI.Depts.Women_Children_Disabled_and_SeniorCitizen
{
	public class Helper : CommonSPHel
	{

		public string emailID = "vamsi.codetree@gmail.com"; public string password = "test123";
		public dynamic Reg(GetRegData Obj)
		{
			dynamic objdynamic = new ExpandoObject();
			try
			{
				var CertData = Reg_User(Obj.name, Obj.email, Obj.password, Obj.confirmpassword, Obj.mobile, Obj.aadhaar, Obj.GSWS_ID, Obj.gsws_user_email, Obj.gsws_user_password);

				var ResultData = JsonConvert.DeserializeObject(CertData);
				if (ResultData.status == "200")
				{
					if (!string.IsNullOrEmpty((ResultData.Token ?? "").ToString()))
					{
						objdynamic.Status = 100;
						objdynamic.Token = (ResultData.Token ?? "").ToString();
					}
					else
						objdynamic.Status = 101;

					objdynamic.Reason = ResultData.Response;
				}
				else
				{
					objdynamic.Status = 102;
					objdynamic.Reason = ResultData.Response;
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("WCDSExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + ex.Message.ToString()));

				objdynamic.Status = 101;
				objdynamic.Reason = ThirdpartyMessage;
			}

			return objdynamic;
		}

		#region "Sanctioned vehicles Methods"
		public dynamic GetToken()
		{
			dynamic objdata = new ExpandoObject(); dynamic objinput = new ExpandoObject();
			try
			{
				objinput.type = "1"; objinput.username = "vamsi.codetree@gmail.com"; objinput.pswd = "test123";
				var LoginData = API_Token(objinput);

				var ResultData = JsonConvert.DeserializeObject(LoginData);
				if (ResultData.status == "200")
				{
					objdata.Status = "Success";
					objdata.Reason = "";
					objdata.Token = ResultData.Token;
					HttpContext.Current.Session.Add("Token", ResultData.Token.ToString());
				}
				else
				{
					objdata.Status = "Failure";
					objdata.Reason = "";
					objdata.Token = "";
					HttpContext.Current.Session.Add("Token", "");
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("WCDSExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + ex.Message.ToString()));

				objdata.Status = "Exception";
				objdata.Reason = ThirdpartyMessage;
				objdata.Token = "";
				HttpContext.Current.Session.Add("Token", "");

			}
			return objdata;
		}

		public dynamic GetCommonData(dynamic objinput)
		{
			dynamic objdata = new ExpandoObject();
			try
			{
				var dist_result = GetData_Headers<dynamic>(URLS(objinput.type.ToString()), (objinput.wcdwtoken ?? "").ToString());
				if (dist_result.status == "200")
				{
					objdata.Status = 100;
					objdata.Data = dist_result.Response;
					objdata.Reason = "";
				}
				else
				{
					objdata.Status = 102;
					objdata.Data = "";
					objdata.Reason = "No Districts Data Found";
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("WCDSExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + ex.Message.ToString()));

				objdata.Status = 102;
				objdata.Reason = ThirdpartyMessage;
				objdata.Token = "";

			}
			return objdata;
		}

		public dynamic GetVillages(dynamic obj)
		{
			dynamic objdata = new ExpandoObject();
			dynamic objinput = new ExpandoObject();
			try
			{
				objinput.type = "4"; objinput.mandalid = obj.Mandalid; objinput.token = obj.wcdwtoken;
				var LoginData = API_Token(objinput);

				var ResultData = JsonConvert.DeserializeObject(LoginData);
				if (ResultData.status == "200")
				{
					objdata.Status = 100;
					objdata.Reason = "";
					objdata.Data = ResultData.Response;
				}
				else
				{
					objdata.Status = 102;
					objdata.Reason = "";
					objdata.VillageData = "";
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("WCDSExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + ex.Message.ToString()));

				objdata.Status = 102;
				objdata.Reason = ThirdpartyMessage;
				objdata.Token = "";
				HttpContext.Current.Session.Add("Token", "");

			}
			return objdata;
		}

		public dynamic GetFyn_Year()
		{
			dynamic Objres = new ExpandoObject();
			try
			{
				DataTable dt_Fy_Data = new DataTable();
				dt_Fy_Data.Clear();
				dt_Fy_Data.Columns.Add("year_ID");
				dt_Fy_Data.Columns.Add("year");
				int count = 1;
				int currentYear = DateTime.Now.Year;

				for (int i = currentYear - 2; i < 2100 + 1; i++)
				{
					DataRow _datarow = dt_Fy_Data.NewRow();
					_datarow["year_ID"] = i.ToString();
					_datarow["year"] = i.ToString();
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
					Objres.Reason = "Years Not Generated";
					Objres.Data = "";
				}

			}
			catch (Exception wex)
			{
				Objres.Status = "Exception";
				Objres.Reason = ThirdpartyMessage;
				Objres.Data = "";

				string mappath = HttpContext.Current.Server.MapPath("WCDSExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + wex.Message.ToString()));
			}
			return Objres;
		}

		public dynamic SaveWCDWApplication_SP(WCDWCLS rootobj)
		{
			dynamic objdata = new ExpandoObject();
			try
			{
				var perresponse = SaveDataPersonalDetails(rootobj.PerDetails);
				if (perresponse.status != "200")
				{
					objdata.Status = 102;
					objdata.Reason = perresponse.Response;
					return objdata;
				}

				var eduresponse = SaveDataEducationDetails(rootobj.EduDetails);
				if (eduresponse.status != "200")
				{
					objdata.Status = 102;
					objdata.Reason = eduresponse.Response;
					return objdata;
				}
				var disponse = SaveDataDisableDetails(rootobj.DisableDetails);
				if (disponse.status != "200")
				{
					objdata.Status = 102;
					objdata.Reason = disponse.Response;
					return objdata;
				}
				if (rootobj.PrevRecList.List.Count > 0)
				{
					var preponse = SaveDataPreviousDetails(rootobj.PrevRecList);
					if (preponse.status != "200")
					{
						objdata.Status = 102;
						objdata.Reason = preponse.Response;
						return objdata;
					}
				}

				var appresponse = SaveDataSchemesDetails(rootobj.ApplyList);
				if (appresponse.status != "200")
				{
					objdata.Status = 102;
					objdata.Reason = appresponse.Response;
					return objdata;
				}

				var docesponse = SaveDataUploadDocuments(rootobj.UploadDocuments);
				if (docesponse.status != "200")
				{
					objdata.Status = 102;
					objdata.Reason = docesponse.Response;
					return objdata;
				}
				var preresponse = SaveDataPreviewApplication(rootobj.ApplyList);
				if (preresponse.status == "200")
				{
					try
					{
						transactionModel objtrans = new transactionModel();
						objtrans.TYPE = "2";
						objtrans.TXN_ID = rootobj.GSWS_ID;
						objtrans.DEPT_ID = "3802";
						objtrans.DEPT_TXN_ID = preresponse.APPLICATIONID.ToString();
						objtrans.STATUS_CODE = "01";
						objtrans.REMARKS = preresponse.Response;
						DataTable dt = new transactionHelper().transactionInsertion(objtrans);
					}
					catch (Exception ex)
					{
						string mappath = HttpContext.Current.Server.MapPath("WCDSExceptionLogs");
						Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Saving Dept Response Error:" + ex.Message.ToString()));
					}

					objdata.Status = 100;
					objdata.Reason = preresponse.Response;
					objdata.Data = preresponse.APPLICATIONID;
					return objdata;
				}
				else
				{
					objdata.Status = 102;
					objdata.Reason = preresponse.Response;
					return objdata;
				}

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("WCDSExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + ex.Message.ToString()));

				objdata.Status = 102;
				objdata.Reason = ThirdpartyMessage;
				objdata.Token = "";
				return objdata;
			}

		}

		public dynamic GetApplicationStatus(AppStatCls rootobj)
		{
			dynamic objdata = new ExpandoObject();
			try
			{
				var response = GetApplicationStatus_Helper(rootobj);
				if (response.status == "200")
				{
					objdata.Status = 100;
					objdata.Reason = response.Response;
				}
				else
				{
					objdata.Status = 102;
					objdata.Reason = response.Response;
				}

				return objdata;

			}
			catch (Exception ex)
			{
				objdata.Status = 102;
				objdata.Reason = ThirdpartyMessage;
				objdata.Token = "";
				return objdata;
			}

		}

		#endregion

		#region "Service Consuming cODE"


		//Get Headers API
		public T GetData_Headers<T>(string url, string token)
		{
			try
			{
				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				var req = (HttpWebRequest)WebRequest.Create(url);
				req.ContentType = "application/json; charset=utf-8";
				req.Headers.Add("Token", token);
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
				string mappath = HttpContext.Current.Server.MapPath("WCDSExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + wex.Message.ToString()));

				throw wex;
			}
		}

		#region "Registration Form"
		public dynamic Reg_User(string name, string emailid, string pswd, string confirmpswd, string mobileno, string aadhaar, string gswsdid, string gswsemail, string gswspassword)
		{

			try
			{
				string mappath = HttpContext.Current.Server.MapPath("WCDSSaveLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, " User Registration Data API: " + " name : " + name + ", emailid : " + emailid + ", pswd : " + pswd + ", confirmpswd : " + confirmpswd + ", mobileno : " + mobileno + ", aadhaar : " + aadhaar + ", gswsdid : " + gswsdid + ", gswsemail : " + gswsemail + ", gswspassword : " + gswspassword));


				string url = "https://apdascac.com/User-Register";
				HttpClient client = new HttpClient();
				client.BaseAddress = new Uri(url);
				client.DefaultRequestHeaders.Accept.Clear();


				MultipartFormDataContent content = new MultipartFormDataContent();

				StringContent firstname = new StringContent(name, Encoding.UTF8, "multipart/form-data");
				StringContent email = new StringContent(emailid, Encoding.UTF8, "multipart/form-data");
				StringContent password = new StringContent(pswd, Encoding.UTF8, "multipart/form-data");
				StringContent confpassword = new StringContent(confirmpswd, Encoding.UTF8, "multipart/form-data");
				StringContent mobile = new StringContent(mobileno, Encoding.UTF8, "multipart/form-data");
				StringContent aadhar = new StringContent(aadhaar, Encoding.UTF8, "multipart/form-data");
				StringContent GSWS_ID = new StringContent(gswsdid, Encoding.UTF8, "multipart/form-data");
				StringContent gsws_user_email = new StringContent(gswsemail, Encoding.UTF8, "multipart/form-data");
				StringContent gsws_user_password = new StringContent(gswspassword, Encoding.UTF8, "multipart/form-data");

				content.Add(firstname, "firstname");
				content.Add(email, "email");
				content.Add(password, "password");
				content.Add(confpassword, "confpassword");
				content.Add(mobile, "mobile");
				content.Add(aadhar, "aadhar");
				content.Add(GSWS_ID, "GSWS_ID");
				content.Add(gsws_user_email, "gsws_user_email");
				content.Add(gsws_user_password, "gsws_user_password");


				HttpResponseMessage response = client.PostAsync(url, content).Result;

				string returnString = response.Content.ReadAsStringAsync().Result;



				return returnString;
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("WCDSExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Registration Form API:" + ex.Message.ToString()));

				throw ex;
			}

		}
		#endregion

		#region "Sanctioned vehicles 3 Wheelers"
		public dynamic API_Token(dynamic objinput)
		{
			try
			{
				string url = URLS(objinput.type.ToString());
				HttpClient client = new HttpClient();
				client.BaseAddress = new Uri(url);
				client.DefaultRequestHeaders.Accept.Clear();
				if (objinput.type != "1")
				{
					client.DefaultRequestHeaders.Add("Token", objinput.token.ToString());
				}
				MultipartFormDataContent content = new MultipartFormDataContent();
				//Login Token Inputs
				if (objinput.type == "1")
				{
					StringContent usermail = new StringContent(objinput.username.ToString(), Encoding.UTF8, "multipart/form-data");
					StringContent userpswd = new StringContent(objinput.pswd.ToString(), Encoding.UTF8, "multipart/form-data");

					content.Add(usermail, "email");
					content.Add(userpswd, "password");
				}
				//Get Villages
				if (objinput.type == "4")
				{

					StringContent mandalid = new StringContent(objinput.mandalid.ToString(), Encoding.UTF8, "multipart/form-data");

					content.Add(mandalid, "mandal_id");

				}

				HttpResponseMessage response = client.PostAsync(url, content).Result;
				string returnString = response.Content.ReadAsStringAsync().Result;

				return returnString;
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("WCDSExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + ex.Message.ToString()));

				throw ex;
			}

		}

		public dynamic SaveDataPersonalDetails(PerDetails objinput)
		{
			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

				string url = "https://apdascac.com/Personal-Details";
				HttpClient client = new HttpClient();
				client.BaseAddress = new Uri(url);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Add("Token", objinput.token.ToString());

				MultipartFormDataContent content = new MultipartFormDataContent();

				StringContent applicant = new StringContent(objinput.applicant ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent father = new StringContent(objinput.father ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent aadhar = new StringContent(objinput.aadhar ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent dob = new StringContent(objinput.dob ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent gender = new StringContent(objinput.gender ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent maritalstatus = new StringContent(objinput.maritalstatus ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent femaletype = new StringContent(objinput.femaletype ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent minority = new StringContent(objinput.minority ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent religion = new StringContent(objinput.religion ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent district = new StringContent(objinput.district ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent mandal = new StringContent(objinput.mandal ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent village = new StringContent(objinput.village ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent othervillage = new StringContent(objinput.othervillage ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent habitation = new StringContent(objinput.habitation ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent wardnumber = new StringContent(objinput.wardnumber ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent housenumber = new StringContent(objinput.housenumber ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent pincode = new StringContent(objinput.pincode ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent nativity = new StringContent(objinput.nativity ?? "", Encoding.UTF8, "multipart/form-data");

				StringContent sameaddress = new StringContent(objinput.sameaddress ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent present_district = new StringContent(objinput.present_district ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent present_mandal = new StringContent(objinput.present_mandal ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent present_village = new StringContent(objinput.present_village ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent present_othervillage = new StringContent(objinput.present_othervillage ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent present_habitation = new StringContent(objinput.present_habitation ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent present_wardnumber = new StringContent(objinput.present_wardnumber ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent present_housenumber = new StringContent(objinput.present_housenumber ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent present_pincode = new StringContent(objinput.present_pincode ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent nativity_present = new StringContent(objinput.nativity_present ?? "", Encoding.UTF8, "multipart/form-data");

				StringContent annualincomeoffamily = new StringContent(objinput.annualincomeoffamily ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent incomeissueddistrict = new StringContent(objinput.incomeissueddistrict ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent dateofincome = new StringContent(objinput.dateofincome ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent pensioner = new StringContent(objinput.pensioner ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent pensiontype = new StringContent(objinput.pensiontype ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent pensionnumber = new StringContent(objinput.pensionnumber ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent pensionissueddistrict = new StringContent(objinput.pensionissueddistrict ?? "", Encoding.UTF8, "multipart/form-data");

				StringContent whiteration = new StringContent(objinput.whiteration ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent rationnumber = new StringContent(objinput.rationnumber ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent rationdistrict = new StringContent(objinput.rationdistrict ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent licencetype = new StringContent(objinput.licencetype ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent drivinglicenceno = new StringContent(objinput.drivinglicenceno ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent licensevalidity = new StringContent(objinput.licensevalidity ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent llrno = new StringContent(objinput.llrno ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent llrdate = new StringContent(objinput.llrdate ?? "", Encoding.UTF8, "multipart/form-data");

				StringContent sportsman = new StringContent(objinput.sportsman ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent sportslevel = new StringContent(objinput.sportslevel ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent sportsorg = new StringContent(objinput.sportsorg ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent sportevent = new StringContent(objinput.sportevent ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent sportsdate = new StringContent(objinput.sportsdate ?? "", Encoding.UTF8, "multipart/form-data");

				content.Add(applicant, "applicant");
				content.Add(father, "father");
				content.Add(aadhar, "aadhar");
				content.Add(dob, "dob");
				content.Add(gender, "gender");
				content.Add(maritalstatus, "maritalstatus");
				content.Add(femaletype, "femaletype");
				content.Add(minority, "minority");
				content.Add(religion, "religion");
				content.Add(district, "district");
				content.Add(mandal, "mandal");
				content.Add(village, "village");
				content.Add(othervillage, "othervillage");
				content.Add(habitation, "habitation");
				content.Add(wardnumber, "wardnumber");
				content.Add(housenumber, "housenumber");
				content.Add(pincode, "pincode");
				content.Add(nativity, "nativity");

				content.Add(sameaddress, "sameaddress");
				content.Add(present_district, "present_district");
				content.Add(present_mandal, "present_mandal");
				content.Add(present_village, "present_village");
				content.Add(present_othervillage, "present_othervillage");
				content.Add(present_habitation, "present_habitation");
				content.Add(present_wardnumber, "present_wardnumber");
				content.Add(present_housenumber, "present_housenumber");
				content.Add(present_pincode, "present_pincode");
				content.Add(nativity_present, "nativity_present");

				content.Add(annualincomeoffamily, "annualincomeoffamily");
				content.Add(incomeissueddistrict, "incomeissueddistrict");
				content.Add(dateofincome, "dateofincome");
				content.Add(pensioner, "pensioner");
				content.Add(pensiontype, "pensiontype");
				content.Add(pensionnumber, "pensionnumber");
				content.Add(pensionissueddistrict, "pensionissueddistrict");

				content.Add(whiteration, "whiteration");
				content.Add(rationnumber, "rationnumber");
				content.Add(rationdistrict, "rationdistrict");
				content.Add(licencetype, "licencetype");
				content.Add(drivinglicenceno, "drivinglicenceno");
				content.Add(licensevalidity, "licensevalidity");
				content.Add(llrno, "llrno");
				content.Add(llrdate, "llrdate");

				content.Add(sportsman, "sportsman");
				content.Add(sportslevel, "sportslevel");
				content.Add(sportsorg, "sportsorg");
				content.Add(sportevent, "sportevent");
				content.Add(sportsdate, "sportsdate");


				HttpResponseMessage response = client.PostAsync(url, content).Result;
				string returnString = response.Content.ReadAsStringAsync().Result;

				return JsonConvert.DeserializeObject<DeptResponse>(returnString);
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("WCDSExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + ex.Message.ToString()));

				throw ex;
			}

		}

		public dynamic SaveDataEducationDetails(EduDetails objinput)
		{
			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

				string url = "https://apdascac.com/Education-Details";
				HttpClient client = new HttpClient();
				client.BaseAddress = new Uri(url);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Add("Token", objinput.token);

				MultipartFormDataContent content = new MultipartFormDataContent();

				StringContent literate = new StringContent(objinput.literate ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent employmenttype = new StringContent(objinput.employmenttype ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent studying = new StringContent(objinput.studying ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent highesteducation = new StringContent(objinput.highesteducation ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent selfemployment = new StringContent(objinput.selfemployment ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent wageemployment = new StringContent(objinput.wageemployment ?? "", Encoding.UTF8, "multipart/form-data");

				StringContent unitname = new StringContent(objinput.unitname ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent noofemployees = new StringContent(objinput.noofemployees ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent annualturnover = new StringContent(objinput.annualturnover ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent governmentsubsidy = new StringContent(objinput.governmentsubsidy ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent bankloan = new StringContent(objinput.bankloan ?? "", Encoding.UTF8, "multipart/form-data");

				StringContent self_sameaddress = new StringContent(objinput.self_sameaddress ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent self_district = new StringContent(objinput.self_district ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent self_mandal = new StringContent(objinput.self_mandal ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent self_village = new StringContent(objinput.self_village ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent self_othervillage = new StringContent(objinput.self_othervillage ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent self_habitation = new StringContent(objinput.self_habitation ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent self_wardnumber = new StringContent(objinput.self_wardnumber ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent self_housenumber = new StringContent(objinput.self_housenumber ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent self_pincode = new StringContent(objinput.self_pincode ?? "", Encoding.UTF8, "multipart/form-data");

				StringContent currenteducation = new StringContent(objinput.currenteducation ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent collegename = new StringContent(objinput.collegename ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent admissionnumber = new StringContent(objinput.admissionnumber ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent studyingprofessionalcourses = new StringContent(objinput.studyingprofessionalcourses ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent coursename = new StringContent(objinput.coursename ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent university = new StringContent(objinput.university ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent otheruniversity = new StringContent(objinput.otheruniversity ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent courseduration = new StringContent(objinput.courseduration ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent coursedurationto = new StringContent(objinput.coursedurationto ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent schoolname = new StringContent(objinput.schoolname ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent schooladmissionnumber = new StringContent(objinput.schooladmissionnumber ?? "", Encoding.UTF8, "multipart/form-data");

				StringContent college_district = new StringContent(objinput.college_district ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent college_state = new StringContent(objinput.college_state ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent college_state_district = new StringContent(objinput.college_state_district ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent college_state_address = new StringContent(objinput.college_state_address ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent college_mandal = new StringContent(objinput.college_mandal ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent college_village = new StringContent(objinput.college_village ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent college_othervillage = new StringContent(objinput.college_othervillage ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent college_habitation = new StringContent(objinput.college_habitation ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent college_wardnumber = new StringContent(objinput.college_wardnumber ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent college_housenumber = new StringContent(objinput.college_housenumber ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent college_pincode = new StringContent(objinput.college_pincode ?? "", Encoding.UTF8, "multipart/form-data");

				StringContent organizationname = new StringContent(objinput.organizationname ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent designation = new StringContent(objinput.designation ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent wage_district = new StringContent(objinput.wage_district ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent wage_mandal = new StringContent(objinput.wage_mandal ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent wage_village = new StringContent(objinput.wage_village ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent wage_othervillage = new StringContent(objinput.wage_othervillage ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent wage_habitation = new StringContent(objinput.wage_habitation ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent wage_wardnumber = new StringContent(objinput.wage_wardnumber ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent wage_pincode = new StringContent(objinput.wage_pincode ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent wage_housenumber = new StringContent(objinput.wage_housenumber ?? "", Encoding.UTF8, "multipart/form-data");

				content.Add(literate, "literate");
				content.Add(employmenttype, "employmenttype");
				content.Add(studying, "studying");
				content.Add(highesteducation, "highesteducation");
				content.Add(selfemployment, "selfemployment");
				content.Add(wageemployment, "wageemployment");

				content.Add(unitname, "unitname");
				content.Add(noofemployees, "noofemployees");
				content.Add(annualturnover, "annualturnover");
				content.Add(governmentsubsidy, "governmentsubsidy");
				content.Add(bankloan, "bankloan");

				content.Add(self_sameaddress, "self_sameaddress");
				content.Add(self_district, "self_district");
				content.Add(self_mandal, "self_mandal");
				content.Add(self_village, "self_village");
				content.Add(self_othervillage, "self_othervillage");
				content.Add(self_habitation, "self_habitation");
				content.Add(self_wardnumber, "self_wardnumber");
				content.Add(self_housenumber, "self_housenumber");
				content.Add(self_pincode, "self_pincode");

				content.Add(currenteducation, "currenteducation");
				content.Add(collegename, "collegename");
				content.Add(admissionnumber, "admissionnumber");
				content.Add(coursename, "coursename");
				content.Add(studyingprofessionalcourses, "studyingprofessionalcourses");
				content.Add(university, "university");
				content.Add(otheruniversity, "otheruniversity");
				content.Add(courseduration, "courseduration");
				content.Add(coursedurationto, "coursedurationto");
				content.Add(schoolname, "schoolname");
				content.Add(schooladmissionnumber, "schooladmissionnumber");

				content.Add(college_district, "college_district");
				content.Add(college_state, "college_state");
				content.Add(college_state_district, "college_state_district");
				content.Add(college_state_address, "college_state_address");
				content.Add(college_mandal, "college_mandal");
				content.Add(college_village, "college_village");
				content.Add(college_othervillage, "college_othervillage");
				content.Add(college_habitation, "college_habitation");
				content.Add(college_wardnumber, "college_wardnumber");
				content.Add(college_housenumber, "college_housenumber");
				content.Add(college_pincode, "college_pincode");

				content.Add(organizationname, "organizationname");
				content.Add(designation, "designation");
				content.Add(wage_district, "wage_district");
				content.Add(wage_mandal, "wage_mandal");
				content.Add(wage_village, "wage_village");
				content.Add(wage_othervillage, "wage_othervillage");
				content.Add(wage_habitation, "wage_habitation");
				content.Add(wage_wardnumber, "wage_wardnumber");
				content.Add(wage_housenumber, "wage_housenumber");
				content.Add(wage_pincode, "wage_pincode");


				HttpResponseMessage response = client.PostAsync(url, content).Result;
				string returnString = response.Content.ReadAsStringAsync().Result;

				return JsonConvert.DeserializeObject<DeptResponse>(returnString);
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("WCDSExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + ex.Message.ToString()));

				throw ex;
			}

		}

		public dynamic SaveDataDisableDetails(DisableDetails objinput)
		{
			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

				string url = "https://apdascac.com/Disability-Details";
				HttpClient client = new HttpClient();
				client.BaseAddress = new Uri(url);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Add("Token", objinput.token);

				MultipartFormDataContent content = new MultipartFormDataContent();

				StringContent natureofdisability = new StringContent(objinput.natureofdisability ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent natureofdisability_m = new StringContent(objinput.natureofdisability_m ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent percentageofdisability = new StringContent(objinput.percentageofdisability ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent sadaremnumber = new StringContent(objinput.sadaremnumber ?? "", Encoding.UTF8, "multipart/form-data");

				content.Add(natureofdisability, "natureofdisability");
				content.Add(natureofdisability_m, "natureofdisability_m");
				content.Add(percentageofdisability, "percentageofdisability");
				content.Add(sadaremnumber, "sadaremnumber");

				HttpResponseMessage response = client.PostAsync(url, content).Result;
				string returnString = response.Content.ReadAsStringAsync().Result;

				return JsonConvert.DeserializeObject<DeptResponse>(returnString);
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("WCDSExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + ex.Message.ToString()));

				throw ex;
			}

		}

		public dynamic SaveDataPreviousDetails(PrevRecList objinput)
		{
			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

				string url = "https://apdascac.com/Previousreceived";
				HttpClient client = new HttpClient();
				client.BaseAddress = new Uri(url);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Add("Token", objinput.token.ToString());

				MultipartFormDataContent content = new MultipartFormDataContent();

				objinput.List.ForEach(x =>
				{
					StringContent invalue = new StringContent(x, Encoding.UTF8, "multipart/form-data");
					content.Add(invalue, "scheme[]");
				});

				HttpResponseMessage response = client.PostAsync(url, content).Result;
				string returnString = response.Content.ReadAsStringAsync().Result;

				return JsonConvert.DeserializeObject<DeptResponse>(returnString);
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("WCDSExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + ex.Message.ToString()));

				throw ex;
			}

		}

		public dynamic SaveDataSchemesDetails(ApplyList objinput)
		{
			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

				var url = "https://apdascac.com/Applyschemes";
				HttpClient client = new HttpClient();
				client.BaseAddress = new Uri(url);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Add("Token", objinput.token.ToString());

				MultipartFormDataContent content = new MultipartFormDataContent();

				objinput.List.ForEach(x =>
				{
					StringContent invalue = new StringContent(x, Encoding.UTF8, "multipart/form-data");
					content.Add(invalue, "scheme[]");
				});

				HttpResponseMessage response = client.PostAsync(url, content).Result;
				string returnString = response.Content.ReadAsStringAsync().Result;

				return JsonConvert.DeserializeObject<DeptResponse>(returnString);
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("WCDSExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + ex.Message.ToString()));

				throw ex;
			}

		}

		public dynamic SaveDataUploadDocuments(UploadDocuments objinput)
		{
			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

				var url = "https://apdascac.com/Upload-Documents";
				HttpClient client = new HttpClient();
				client.BaseAddress = new Uri(url);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Add("Token", objinput.token);

				MultipartFormDataContent content = new MultipartFormDataContent();

				StringContent photoaadhar = new StringContent(objinput.photo_aadhar ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent photodob = new StringContent(objinput.photo_dob ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent photodisability = new StringContent(objinput.photo_disability ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent photosadarem = new StringContent(objinput.photo_sadarem ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent photoincome = new StringContent(objinput.photo_income ?? "", Encoding.UTF8, "multipart/form-data");
				StringContent photocaste = new StringContent(objinput.photo_caste ?? "", Encoding.UTF8, "multipart/form-data");

				content.Add(photoaadhar, "photo_aadhar");
				content.Add(photodob, "photo_dob");
				content.Add(photodisability, "photo_disability");
				content.Add(photosadarem, "photo_sadarem");
				content.Add(photoincome, "photo_income");
				content.Add(photocaste, "photo_caste");

				HttpResponseMessage response = client.PostAsync(url, content).Result;
				string returnString = response.Content.ReadAsStringAsync().Result;

				return JsonConvert.DeserializeObject<DeptResponse>(returnString);
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("WCDSExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + ex.Message.ToString()));

				throw ex;
			}

		}

		public dynamic SaveDataPreviewApplication(ApplyList objinput)
		{
			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

				var url = "https://apdascac.com/Preview-Application";
				HttpClient client = new HttpClient();
				client.BaseAddress = new Uri(url);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Add("Token", objinput.token);

				MultipartFormDataContent content = new MultipartFormDataContent();

				StringContent Accept = new StringContent("1", Encoding.UTF8, "multipart/form-data");
				content.Add(Accept, "Accept");

				HttpResponseMessage response = client.PostAsync(url, content).Result;
				string returnString = response.Content.ReadAsStringAsync().Result;

				return JsonConvert.DeserializeObject<DeptResponse>(returnString);
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("WCDSExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + ex.Message.ToString()));

				throw ex;
			}

		}

		public dynamic GetApplicationStatus_Helper(AppStatCls objinput)
		{
			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

				var url = "https://apdascac.com/Get-Status";
				HttpClient client = new HttpClient();
				client.BaseAddress = new Uri(url);
				client.DefaultRequestHeaders.Accept.Clear();

				MultipartFormDataContent content = new MultipartFormDataContent();

				StringContent input = new StringContent(objinput.input, Encoding.UTF8, "multipart/form-data");
				content.Add(input, "input");

				HttpResponseMessage response = client.PostAsync(url, content).Result;
				string returnString = response.Content.ReadAsStringAsync().Result;

				return JsonConvert.DeserializeObject<dynamic>(returnString);
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("WCDSExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Data API:" + ex.Message.ToString()));

				throw ex;
			}

		}


		#endregion

		#endregion

		#region "All Service URLS"
		public string URLS(string type)
		{
			string targetURL = "";

			if (type == "1")
				targetURL = "https://apdascac.com/User-Login";
			else if (type == "2")
				targetURL = "https://apdascac.com/Get-Districts";
			else if (type == "3")
				targetURL = "https://apdascac.com/Get-Mandals";
			else if (type == "4")
				targetURL = "https://apdascac.com/Mandals-Village";
			else if (type == "5")
				targetURL = "https://apdascac.com/Get-Highesteducation";
			else if (type == "6")
				targetURL = "https://apdascac.com/Get-University";
			else if (type == "7")
				targetURL = "https://apdascac.com/Get-States";
			else if (type == "8")
				targetURL = "https://apdascac.com/Get-Disability";

			return targetURL;

		}
		#endregion

	}
}