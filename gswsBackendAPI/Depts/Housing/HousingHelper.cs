using gswsBackendAPI.DL.DataConnection;
using gswsBackendAPI.transactionModule;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using gswsBackendAPI.DL.CommonHel;
using static gswsBackendAPI.Depts.Housing.HousingModel;

namespace gswsBackendAPI.Depts.Housing
{
    public class HousingHelper : HousingSPHelper
    {
		dynamic obj = new ExpandoObject();

		#region Housing
		public dynamic GetMethod(string url)
        {
            
            try
            {
                var val = GetData(url);

                obj.Status = 100;
                obj.Reason = "Data Loaded Successfully.";
                obj.Details = val;

                return obj;
            }
            catch (Exception ex)
            {
                obj.Status = 102;
                obj.Reason = "Error Occured While Load Data";
                return obj;
            }

        }

		#endregion

		#region "House sites"

		public dynamic GetHousesiteToken()
		{
			try
			{
				HousesiteWebservice.API _obj = new HousesiteWebservice.API();
				var response = _obj.GetToken("HOUSESITESAPOL", "H@US3SIT3S@21122019");
				string mappath = HttpContext.Current.Server.MapPath("HousingSitesTokenLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Generate Token :" + response));

				dynamic rootobj = JsonConvert.DeserializeObject<dynamic>(response);

				obj.Status = 100;
				obj.Reason = rootobj.remarks.ToString();
				obj.status = rootobj.status;
				obj.tokenID = rootobj.tokenID.ToString();
			}
			catch (Exception ex)
			{
				obj.Status = 102;
				obj.Reason = ThirdpartyMessage+"( Generate Token:"+ex.Message.ToString()+")";

				string mappath = HttpContext.Current.Server.MapPath("HousingSitesErrorLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Generate Token :" + ex.Message.ToString()));
			}

			return obj;
		}

		public dynamic GetHouseSiteStatusApp(ApplicationSta rootobj)
		{
			try
			{
				//HousesiteWebservice.API _obj2 = new HousesiteWebservice.API();
				//var response2 = _obj2.GetHSP_APP_Status("HOUSESITESAPOL", "H@US3SIT3S@21122019", "3xD1FE74", rootobj.AppNo);

				//obj.Status = 100;
				//obj.Reason = "Data Loaded Successfully.";
				//obj.Details = response2;

				//string mappath2 = HttpContext.Current.Server.MapPath("HousingSitesResponseLogs");
				//Task WriteTask2 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath2, "Response  from application status services :  " + response2));


				var tokenres = GetHousesiteToken();

				if (tokenres.Status == 100)
				{
					if (tokenres.status == "0")
					{
						string value = JsonConvert.SerializeObject(rootobj);
						string mappath = HttpContext.Current.Server.MapPath("HousingSitesSavingLogs");
						Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Application Status Search Criteria Data :" + value));

						HousesiteWebservice.API _obj = new HousesiteWebservice.API();
						var response = _obj.GetHSP_APP_Status("HOUSESITESAPOL", "H@US3SIT3S@21122019", tokenres.tokenID, rootobj.AppNo);

						//dynamic res = JsonConvert.DeserializeObject<dynamic>(response); 
						obj.Status = 100;
						obj.Reason = "Data Loaded Successfully.";
						obj.Details = response;
					
						string mappath1 = HttpContext.Current.Server.MapPath("HousingSitesResponseLogs");
						Task WriteTask1 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath1, "Response  from application status services :  " + response));

					}
					else
					{
						obj.Status = 102;
						obj.Reason = tokenres.Reason;
					}
				}
				else
				{
					obj.Status = 102;
					obj.Reason = tokenres.Reason;
				}
			}
			catch (Exception ex)
			{
				obj.Status = 102;
				obj.Reason = ThirdpartyMessage;

				string mappath = HttpContext.Current.Server.MapPath("HousingSitesExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Getting Application Status :" + ex.Message.ToString()));
			}

			return obj;
		}

		public dynamic GetHouseSitePattapplicationAdd(HousePattaCls root)
		{
			try
			{
				string mappath = HttpContext.Current.Server.MapPath("HousingSitesSavingLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Submit Application Data :" + JsonConvert.SerializeObject(root)));

				obj.Status = 102;
				obj.Reason = "House Site Application has been Closed.";
				return obj;
				if (string.IsNullOrEmpty(root.AHPAllotmentDetails))
				{
					root.AHPAllotmentDetails = "NA";
				}
				
				if (root.MandaltypeID == "U")
				{
					root.isIncome = 0;
					root.isPMAY = 0;
					root.PMAYBenefitSchemeID = 0;
					root.isAHPAllotmentReceived = 0;
					root.AHPAllotmentDetails = "0";

				}

				var tokenres = GetHousesiteToken();

				string mappath2 = HttpContext.Current.Server.MapPath("HousingSitesTokenLogs");
				Task WriteTask2 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath2, "Submit Application Data :" + tokenres));

				if (tokenres.Status == 100)
				{
					if (tokenres.status == "0")
					{
						HousesiteWebservice.HousePattaBO obj2 = new HousesiteWebservice.HousePattaBO();
						obj2.VolunteerId = root.VolunteerId;
						obj2.VName = root.VName;
						obj2.VMobile = root.VMobile;
						obj2.VAadhaar = root.VAadhaar;
						obj2.SachivalayamCodeno = root.SachivalayamCodeno;
						obj2.SachivalayamName = root.SachivalayamName;

						obj2.BAdhaarno = root.BAdhaarno;
						obj2.BenficiaryName = root.BenficiaryName;
						obj2.Appno = root.Appno;
						obj2.Bmobile = root.Bmobile;
						obj2.RelationID = root.RelationID;
						obj2.RelationName = root.RelationName;
						obj2.Age = root.Age;
						obj2.Gender = root.Gender;
						obj2.Religion = root.Religion;
						obj2.isPhysChall = root.isPhysChall;
						obj2.CasteID = root.CasteID;
						obj2.SubCasteID = root.SubCasteID;
						obj2.Occupation = root.Occupation;
						obj2.OtherOccupation = root.OtherOccupation;

						obj2.Houseno = root.Houseno;
						obj2.Street = root.Street;
						obj2.DistrictID = root.DistrictID;
						obj2.MandalID = root.MandalID;
						obj2.VillageID = root.VillageID;
						obj2.PanchayathID = root.PanchayathID;
						obj2.Pincode = root.Pincode;

						obj2.isRation = root.isRation;
						obj2.Rationcardno = root.Rationcardno;
						obj2.isHouse = root.isHouse;
						obj2.isHouseSite = root.isHouseSite;
						obj2.isHousingScheme = root.isHousingScheme;
						obj2.isHouseSiteScheme = root.isHouseSiteScheme;
						obj2.isLand = root.isLand;
						obj2.isIncome = root.isIncome;
						obj2.isPMAY = root.isPMAY;
						obj2.PMAYBenefitSchemeID = root.PMAYBenefitSchemeID;
						obj2.isAHPAllotmentReceived = root.isAHPAllotmentReceived;
						obj2.AHPAllotmentDetails = root.AHPAllotmentDetails;
						obj2.USERID = root.USERID;
						obj2.file = root.file;
						obj2.SystemIP = GetIPAddress();
						obj2.MandaltypeID = root.MandaltypeID;
						string value = JsonConvert.SerializeObject(root);

						
						HousesiteWebservice.API _obj = new HousesiteWebservice.API();
						var response = _obj.HouseSitePattaApplication_Add("HOUSESITESAPOL", "H@US3SIT3S@21122019", tokenres.tokenID, obj2);

						dynamic rootobj = JsonConvert.DeserializeObject<dynamic>(response);

						if (rootobj.status == "0")
						{
							try
							{
								transactionModel objtrans = new transactionModel();
								objtrans.TYPE = "2";
								objtrans.TXN_ID = obj2.Appno;
								objtrans.DEPT_ID = "3301";
								objtrans.DEPT_TXN_ID = rootobj.appno.ToString();
								objtrans.STATUS_CODE = "01";
								objtrans.REMARKS = "";
								DataTable dt = new transactionHelper().transactionInsertion(objtrans);
							}
							catch (Exception ex)
							{
								string mappath3 = HttpContext.Current.Server.MapPath("HousingSitesExceptionLogs");
								Task WriteTask3 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath3, "Saving Dept Response Error:" + ex.Message.ToString()));
							}
						}

						obj.Status = 100;
						obj.Reason = "Application Registered Successfully.";
						obj.Details = response;

						string mappath1 = HttpContext.Current.Server.MapPath("HousingSitesResponseLogs");
						Task WriteTask1 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath1, "Response from Register services :  " + response));
					}
					else
					{
						obj.Status = 102;
						obj.Reason = tokenres.Reason;
					}
				}
				else
				{
					obj.Status = 102;
					obj.Reason = tokenres.Reason;
				}
			}
			catch (Exception ex)
			{

				string mappath = HttpContext.Current.Server.MapPath("HousingSitesErrorLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Submit Application :" + ex.Message.ToString()+ JsonConvert.SerializeObject(root)));

				obj.Status = 102;
				obj.Reason = ThirdpartyMessage+ "( GetHouseSitePattapplicationAdd:" + ex.Message.ToString()+")";
			}

			return obj;
		}

		public string GetIPAddress()
		{
			System.Web.HttpContext context = System.Web.HttpContext.Current;
			string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

			if (!string.IsNullOrEmpty(ipAddress))
			{
				string[] addresses = ipAddress.Split(',');
				if (addresses.Length != 0)
				{
					return addresses[0];
				}
			}

			return context.Request.ServerVariables["REMOTE_ADDR"];
		}
        #endregion

       
    }
}