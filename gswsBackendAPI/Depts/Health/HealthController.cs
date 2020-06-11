using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Dynamic;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using gswsBackendAPI.DL.DataConnection;
using gswsBackendAPI.DL.CommonHel;

namespace gswsBackendAPI.Depts.Health
{
    [RoutePrefix("api/Health")]
    public class HealthController : ApiController
    {
        dynamic CatchData = new ExpandoObject();
        HealthHelper heahel = new HealthHelper();

		#region Arogya Raksha
		[HttpPost]
		[Route("GetArogyaRakshaStatus")]
		public IHttpActionResult GetArogyaRakshaStatus(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				AppStatus rootobj = JsonConvert.DeserializeObject<AppStatus>(value);
				return Ok(heahel.GetArogyaRakshaStatus_helper(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = "Error Occured";
				return Ok(CatchData);
			}
		}

		#endregion



		#region Saderem
		[HttpPost]
		[Route("GetSadaremCertificate")]
		public IHttpActionResult GetSadaremCertificate(dynamic jsonodata)
		{
			dynamic sadaremresponse = new ExpandoObject();
			string value = token_gen.Authorize_aesdecrpty(jsonodata);
			try
			{
				dynamic data = JsonConvert.DeserializeObject<dynamic>(value);

                if (Utils.IsAlphaNumeric((string)data.personcode))
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3 |
                                                           SecurityProtocolType.Tls | SecurityProtocolType.Tls11;


                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

				Sadarem.ValidSadaremidService objsadaram = new Sadarem.ValidSadaremidService();
				dynamic sadaremcert = objsadaram.getcertificatvalid(Convert.ToString(data.personcode));


                  
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(sadaremcert);

                    string json = JsonConvert.SerializeXmlNode(doc);

                    sadaremresponse.Status = "Success";
                    sadaremresponse.data = JsonConvert.DeserializeObject<SadaramResponse>(json);

                    return Ok(sadaremresponse);
                }
                else
                {
                    CatchData.Status = 102;
                    CatchData.Reason = "Student Number does not contain special charactes";
                    return Ok(CatchData);
                }

			}
			catch (Exception ex)
			{
				sadaremresponse.Status = "Failure";
				sadaremresponse.Reason = HealthHelper.ThirdpartyMessage;
				sadaremresponse.data = "";
				return Ok(sadaremresponse);
			}
		}

		#endregion

		#region YSR Kanti Velugu

		[HttpPost]
		[Route("GetKantiVeluguStatus")]
		public IHttpActionResult GetKantiVeluguStatus(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				AppStatus rootobj = JsonConvert.DeserializeObject<AppStatus>(value);
                if (Utils.IsAlphaNumeric(rootobj.student_id))
                    return Ok(heahel.GetMethod("http://drysrkv.ap.gov.in/API/KantiVelugu/API_GS_APKV_STUDENT_REPORT/" + rootobj.student_id));
                else
                {
                    CatchData.Status = 102;
                    CatchData.Reason = "Student Number does not contain special charactes";
                    return Ok(CatchData);
                }
			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = HealthHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}
		#endregion
	}
}
