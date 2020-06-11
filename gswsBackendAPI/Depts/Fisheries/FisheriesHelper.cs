using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using static gswsBackendAPI.Depts.Fisheries.FisheriesModel;
using gswsBackendAPI.DL.CommonHel;
using System.Net;
using System.Threading.Tasks;
using gswsBackendAPI.DL.DataConnection;

namespace gswsBackendAPI.Depts.Fisheries
{
    public class FisheriesHelper : FisheriesSPHelper
    {
		#region Fisheries
		public dynamic GetMethod(AppSta aobj)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				if (Utils.IsAlphaNumeric(aobj.ref_no))
				{
					UserCred cred = new UserCred { username = "clientuser", password = "e1vWaKKo" };
					string Token = GetToken("https://gvgs.ap.gov.in/integration-service/integration/api/v1.0/getAccessTokenForDepts", cred);

					//Uat Details
					//UserCred cred = new UserCred { username = "deptuser", password = "y9G4NMyV" };
					//string Token = GetToken("https://qah1-vs-myap.e-pragati.in/integration-service/integration/api/v1.0/getAccessTokenForDepts", cred);

					dynamic val = null;
					if (aobj.rtype == "App")
					{
						AppCls bobj = new AppCls();
						bobj.transactionId = aobj.ref_no;
						var data = PostData("https://gvgs.ap.gov.in/integration-service/prajasachivalayam/api/v1.0/getApplicationStatus", Token, bobj);
						val = GetSerialzedData<dynamic>(data);
						//val = GetData("https://qah1-vs-myap.e-pragati.in/integration-service/prajasachivalayam/api/v1.0/getApplicationStatus?applicationId=" + aobj.ref_no, Token);
					}
					else
						val = GetData("https://gvgs.ap.gov.in/integration-service/prajasachivalayam/api/v1.0/getApplicationStatus?transactionId=" + aobj.ref_no, Token);
					//val = GetData("https://qah1-vs-myap.e-pragati.in/integration-service/prajasachivalayam/api/v1.0/getApplicationStatus?transactionId=" + aobj.ref_no, Token);

					obj.Status = 100;
					obj.Reason = "Data Loaded Successfully.";
					obj.Details = val;
				}
				else
				{
					obj.Status = 102;
					obj.Reason = "Error Occured While Load Data";
				}

				return obj;
			}
			catch (Exception ex)
			{
				obj.Status = 102;
				obj.Reason = FisheriesHelper.ThirdpartyMessage;

				return obj;
			}

		}

		#endregion

		#region Animal Husbandry
		public dynamic RIDSAppStatus(AnimalCls oj)
        {
            dynamic obj = new ExpandoObject();

            try
            {
                Animal.AHAServices ser = new Animal.AHAServices();
                var data = ser.GetRIDSApplicationDetails(oj.UniqueNo, oj.Type);
                dynamic objroot = JsonConvert.DeserializeObject<dynamic>(data);

                if (objroot != null)
                {
                    obj.Status = 100;
                    obj.Reason = "Data Getting Successfully.";
                    obj.Details = objroot;
                }
                else
                {
                    obj.Status = 101;
                    obj.Reason = "No Data Found";
                }
            }
            catch (WebException wex)
            {
                string mappath = HttpContext.Current.Server.MapPath("AnimalHusbandryLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error Gettting RIDS Application Status Data API:" + wex.Message.ToString()));

                obj.Status = 102;
				obj.Reason = FisheriesHelper.ThirdpartyMessage;

			}

            return obj;
        }

        public dynamic LLCSAppStatus(AnimalCls oj)
        {
            dynamic obj = new ExpandoObject();

            try
            {
                Animal.AHAServices ser = new Animal.AHAServices();
                var data = ser.GetLLCSApplicationDetails(oj.UniqueNo, oj.Type);
                dynamic objroot = JsonConvert.DeserializeObject<dynamic>(data);

                if (objroot != null)
                {
                    obj.Status = 100;
                    obj.Reason = "Data Getting Successfully.";
                    obj.Details = objroot;
                }
                else
                {
                    obj.Status = 101;
                    obj.Reason = "No Data Found";
                }
            }
            catch (WebException wex)
            {
                string mappath = HttpContext.Current.Server.MapPath("AnimalHusbandryLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error Gettting LLCS Application Status Data API:" + wex.Message.ToString()));

                obj.Status = 102;
				obj.Reason = FisheriesHelper.ThirdpartyMessage;

			}

            return obj;
        }
        #endregion
    }
}