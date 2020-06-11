using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

namespace gswsBackendAPI.Depts.MAUD
{
	public class MAUDHelper 
	{
		SocialWelfare_Tribal.Helper hlpservice = new SocialWelfare_Tribal.Helper();
		public dynamic CheckAppStatus(string Applicationid)
		{
			dynamic objdynamic = new ExpandoObject();
			try
			{
				var ObjCertificatecheck = Applicationid.Replace("/","$") + "/PraSaPADCR@123";
				var data = hlpservice.GetData<dynamic>("https://apdpms.ap.gov.in/AutoDCR.APServices/PuraSeva/PuraSeva.svc/GetDetailByFileNo/" + ObjCertificatecheck);

				var ResultData = data;

				objdynamic.Status = ResultData.Status;
				objdynamic.Reason = ResultData.error;
				objdynamic.Data = ResultData.response;
		


			}
			catch (Exception ex)
			{
				objdynamic.Status = "Failure";
				objdynamic.Reason = CommonSPHel.ThirdpartyMessage;
				objdynamic.Data = "";
			}

			return objdynamic;
		}
	}
}