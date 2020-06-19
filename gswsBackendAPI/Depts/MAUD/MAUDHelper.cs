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
using gswsBackendAPI.DL.CommonHel;

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
                Common_MAUD_Error(ex.Message.ToString(), "https://apdpms.ap.gov.in/AutoDCR.APServices/PuraSeva/PuraSeva.svc/GetDetailByFileNo/", "2");
                objdynamic.Status = "Failure";
				objdynamic.Reason = CommonSPHel.ThirdpartyMessage;
				objdynamic.Data = "";
			}

			return objdynamic;
		}
        public bool Common_MAUD_Error(string msg, string url, string etype)
        {
            ExceptionDataModel objex = new ExceptionDataModel();
            try
            {
                objex.E_DEPTID = DepartmentEnum.Department.Municipal_Administration_and_Urban_Development.ToString();
                objex.E_HODID = DepartmentEnum.HOD.Municipal_Administration.ToString();
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