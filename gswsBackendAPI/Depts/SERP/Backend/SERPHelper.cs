using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using gswsBackendAPI.transactionModule;
using gswsBackendAPI.DL.CommonHel;

namespace gswsBackendAPI.Dept.SERP.Backend
{
	
	public class SERPHelper : SERPSPHelper
	{
		PensionResponse objResponse = new PensionResponse();

		#region PSS
		public dynamic GetApplicantStatus(PENSIONModel oj)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				DataTable dt = GetApplicantStatus_helper(oj);
				if (dt != null && dt.Rows.Count > 0)
				{
					obj.Status = "Success";
					obj.Reason = "";
					obj.Details = dt;
				}
				else
				{
					obj.Status = "Failure";
					obj.Reason = "No Data Found";
				}
			}
			catch (Exception ex)
			{
				obj.Status = "Failure";
				obj.Reason = ThirdpartyMessage;
			}
			return obj;
		}
        #endregion

        #region Bhim
        public dynamic GetClaimStatus(Datum oj)
        {
            dynamic obj = new ExpandoObject();
            SERPSPHelper SERPsphel = new SERPSPHelper();
            try
            {
                SerpBima.vvclaimstatus serpbima = new SerpBima.vvclaimstatus();
                var data = serpbima.checkcbclaim("CIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL", oj.claimuid);
                ClaimObject objroot = JsonConvert.DeserializeObject<ClaimObject>(data);
                var sts = objroot.Data[0].Sts;


                if (sts != null && sts != "")
                {
                    obj.Status = "Success";
                    obj.Reason = "";
                    obj.Details = objroot;
                    obj.TrackDetails = objroot.Data[0].TrackingData;
                    obj.ClaimStatus = sts;
                }
                else
                {
                    obj.Status = "Failure";
                    obj.Reason = "No Data Found";
                }
            }
            catch (WebException wex)
            {
                string mappath = HttpContext.Current.Server.MapPath("BimaClaimStatusLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error from GetClaimStatus API:" + wex.Message.ToString()));
                throw wex;
            }
            return obj;
        }
        #endregion

        #region BhimReg
        public dynamic GetDistricts(Datum oj)
        {
            dynamic obj = new ExpandoObject();
            SERPSPHelper SERPsphel = new SERPSPHelper();
            try
            {
                ClaimRegistration.ClaimRegistration district = new ClaimRegistration.ClaimRegistration();
                var data = district.GetDistricts("f0ebb6327edf9c44b");
                ClaimObject objroot = JsonConvert.DeserializeObject<ClaimObject>(data);
                var sts = objroot.Data;


                if (sts != null)
                {
                    obj.Status = "Success";
                    obj.Reason = "";
                    obj.Details = objroot.Data;
                }
                else
                {
                    obj.Status = "Failure";
                    obj.Reason = "No Data Found";
                }
            }
            catch (WebException wex)
            {
                string mappath = HttpContext.Current.Server.MapPath("BimaRegistrationLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error from GetDistricts API:" + wex.Message.ToString()));
                throw wex;
            }
            return obj;
        }

        public dynamic GetMandals(Datum oj)
        {
            dynamic obj = new ExpandoObject();
            SERPSPHelper SERPsphel = new SERPSPHelper();
            try
            {
                ClaimRegistration.ClaimRegistration mandals = new ClaimRegistration.ClaimRegistration();
                var data = mandals.GetMandals("f0ebb6327edf9c44b", oj.DISTRICT_ID);
                ClaimObject objroot = JsonConvert.DeserializeObject<ClaimObject>(data);
                var sts = objroot.Data;


                if (sts != null)
                {
                    obj.Status = "Success";
                    obj.Reason = "";
                    obj.Details = objroot.Data;
                }
                else
                {
                    obj.Status = "Failure";
                    obj.Reason = "No Data Found";
                }
            }
            catch (WebException wex)
            {
                string mappath = HttpContext.Current.Server.MapPath("BimaRegistrationLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error from GetMandals API:" + wex.Message.ToString()));
                throw wex;
            }
            return obj;
        }

        public dynamic GetPanchayats(Datum oj)
        {
            dynamic obj = new ExpandoObject();
            SERPSPHelper SERPsphel = new SERPSPHelper();
            try
            {
                ClaimRegistration.ClaimRegistration panchayat = new ClaimRegistration.ClaimRegistration();
                var data = panchayat.GetPanchayats("f0ebb6327edf9c44b", oj.DISTRICT_ID, oj.MANDAL_ID);
                ClaimObject objroot = JsonConvert.DeserializeObject<ClaimObject>(data);
                var sts = objroot.Data;


                if (sts != null)
                {
                    obj.Status = "Success";
                    obj.Reason = "";
                    obj.Details = objroot.Data;
                }
                else
                {
                    obj.Status = "Failure";
                    obj.Reason = "No Data Found";
                }
            }
            catch (WebException wex)
            {
                string mappath = HttpContext.Current.Server.MapPath("BimaRegistrationLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error from GetPanchayats API:" + wex.Message.ToString()));
                throw wex;
            }
            return obj;
        }
  
        public dynamic GetVillages(Datum oj)
        {
            dynamic obj = new ExpandoObject();
            SERPSPHelper SERPsphel = new SERPSPHelper();
            try
            {
                ClaimRegistration.ClaimRegistration villages = new ClaimRegistration.ClaimRegistration();
                var data = villages.GetVillages("f0ebb6327edf9c44b", oj.DISTRICT_ID, oj.MANDAL_ID);
                ClaimObject objroot = JsonConvert.DeserializeObject<ClaimObject>(data);
                var sts = objroot.Data;


                if (sts != null)
                {
                    obj.Status = "Success";
                    obj.Reason = "";
                    obj.Details = objroot.Data;
                }
                else
                {
                    obj.Status = "Failure";
                    obj.Reason = "No Data Found";
                }
            }
            catch (WebException wex)
            {
                string mappath = HttpContext.Current.Server.MapPath("BimaRegistrationLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error from GetVillages API:" + wex.Message.ToString()));
                throw wex;
            }
            return obj;
        }

        public dynamic GetClaimtypes(Datum oj)
        {
            dynamic obj = new ExpandoObject();
            SERPSPHelper SERPsphel = new SERPSPHelper();
            try
            {
                ClaimRegistration.ClaimRegistration Claimtypes = new ClaimRegistration.ClaimRegistration();
                var data = Claimtypes.GetClaimtype("f0ebb6327edf9c44b");
                ClaimObject objroot = JsonConvert.DeserializeObject<ClaimObject>(data);
                var sts = objroot.Data;


                if (sts != null)
                {
                    obj.Status = "Success";
                    obj.Reason = "";
                    obj.Details = objroot.Data;
                }
                else
                {
                    obj.Status = "Failure";
                    obj.Reason = "No Data Found";
                }
            }
            catch (WebException wex)
            {
                string mappath = HttpContext.Current.Server.MapPath("BimaRegistrationLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error from GetClaimtypes API:" + wex.Message.ToString()));
                throw wex;
            }
            return obj;
        }

        public dynamic GetCauses(Datum oj)
        {
            dynamic obj = new ExpandoObject();
            SERPSPHelper SERPsphel = new SERPSPHelper();
            try
            {
                ClaimRegistration.ClaimRegistration causes = new ClaimRegistration.ClaimRegistration();
                var data = causes.GetCause("f0ebb6327edf9c44b", oj.Code);
                ClaimObject objroot = JsonConvert.DeserializeObject<ClaimObject>(data);
                var sts = objroot.Data;


                if (sts != null)
                {
                    obj.Status = "Success";
                    obj.Reason = "";
                    obj.Details = objroot.Data;
                }
                else
                {
                    obj.Status = "Failure";
                    obj.Reason = "No Data Found";
                }
            }
            catch (WebException wex)
            {
                string mappath = HttpContext.Current.Server.MapPath("BimaRegistrationLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error from GetCauses API:" + wex.Message.ToString()));
                throw wex;
            }
            return obj;
        }

        public dynamic GetIncidenttypes(Datum oj)
        {
            dynamic obj = new ExpandoObject();
            SERPSPHelper SERPsphel = new SERPSPHelper();
            try
            {
                ClaimRegistration.ClaimRegistration incidentby = new ClaimRegistration.ClaimRegistration();
                var data = incidentby.GetIncidentType("f0ebb6327edf9c44b");
                ClaimObject objroot = JsonConvert.DeserializeObject<ClaimObject>(data);
                var sts = objroot.Data;


                if (sts != null)
                {
                    obj.Status = "Success";
                    obj.Reason = "";
                    obj.Details = objroot.Data;
                }
                else
                {
                    obj.Status = "Failure";
                    obj.Reason = "No Data Found";
                }
            }
            catch (WebException wex)
            {
                string mappath = HttpContext.Current.Server.MapPath("BimaRegistrationLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error from GetIncidenttypes API:" + wex.Message.ToString()));
                throw wex;
            }
            return obj;
        }

        public dynamic GetEntryBy(Datum oj)
        {
            dynamic obj = new ExpandoObject();
            SERPSPHelper SERPsphel = new SERPSPHelper();
            try
            {
                ClaimRegistration.ClaimRegistration entryby = new ClaimRegistration.ClaimRegistration();
                var data = entryby.GetEntryBy("f0ebb6327edf9c44b");
                ClaimObject objroot = JsonConvert.DeserializeObject<ClaimObject>(data);
                var sts = objroot.Data;


                if (sts != null)
                {
                    obj.Status = "Success";
                    obj.Reason = "";
                    obj.Details = objroot.Data;
                }
                else
                {
                    obj.Status = "Failure";
                    obj.Reason = "No Data Found";
                }
            }
            catch (WebException wex)
            {
                string mappath = HttpContext.Current.Server.MapPath("BimaRegistrationLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error from GetEntryBy API:" + wex.Message.ToString()));
                throw wex;
            }
            return obj;
        }


        public dynamic BimaRegistration(BimaRegistration oj)
        {
            dynamic obj = new ExpandoObject();
            SERPSPHelper SERPsphel = new SERPSPHelper();
            try
            {
                string incidentdate = Convert.ToDateTime(oj.Incident_Date).ToString("MM/dd/yyyy");
                string accidentdate = Convert.ToDateTime(oj.Accident_Date).ToString("MM/dd/yyyy");

                ClaimRegistration.ClaimRegistration bhimareg = new ClaimRegistration.ClaimRegistration();

                var data = bhimareg.Insertclaimdata("f0ebb6327edf9c44b", oj.Uid, oj.Did, incidentdate, oj.Incident_Time, accidentdate, oj.Accident_Time, oj.Claimtype, oj.Cause,
                    oj.CauseSubReason, oj.Nominee_Phone, oj.Nominee_Name, oj.Nom_Uid, oj.Incident_Type, oj.incident_Place, oj.Accident_Place, oj.Informer_Phone, oj.Informed_By,
                    oj.Reg_User, oj.Remarks, oj.Street, oj.Door_Num, oj.Ward, oj.Village_Name, oj.Gram_Panchayat, oj.Mandal_Name, oj.EID, oj.Pol_Pincode,
                    oj.Nom_EID, oj.Nom_Pincode, oj.VV_Name, oj.VV_Phone, oj.VS_id, oj.VS_Location, oj.VS_Panchayat, oj.EntryBy,oj.GSWS_ID);
                ClaimObject objroot = JsonConvert.DeserializeObject<ClaimObject>(data);
                var sts = objroot.Data;


                if (sts != null)
                {
					transactionModel objtrans = new transactionModel();
					objtrans.TYPE = "2";
					objtrans.TXN_ID = oj.GSWS_ID;
					objtrans.DEPT_ID = "3103";
					objtrans.DEPT_TXN_ID = objroot.Data[0].claimuid;
					objtrans.BEN_ID= objroot.Data[0].Uid_Num;
					objtrans.STATUS_CODE = objroot.Data[0].Sts;
					objtrans.REMARKS = objroot.Data[0].Msg;
					DataTable dt = new transactionHelper().transactionInsertion(objtrans);
					if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
					{
						obj.Status = "Success";
						obj.Reason = objroot.Data[0].Msg;
						obj.Details = objroot.Data;
						obj.Registrationsts = objroot.Data[0].Sts;
					}
					else
					{
						obj.Status = "Success";
						obj.Reason = objroot.Data[0].Msg;
						obj.Details = objroot.Data;
						obj.Registrationsts = objroot.Data[0].Sts;
					}
                }
                else
                {
                    obj.Status = "Failure";
                    obj.Reason = "No Data Found";
                }
            }
            catch (WebException wex)
            {
                string mappath = HttpContext.Current.Server.MapPath("BimaRegistrationLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error from BimaRegistration API:" + wex.Message.ToString()));
                throw wex;
            }
            return obj;
        }

        public dynamic GetPensionMaster()
        {
              string textFile = @"http://prajasachivalayam.ap.gov.in/PSTESTAPP/Depts/SERP/UI/PensionMaster.txt";
            
            var jsonText = new WebClient().DownloadString(textFile);
            var sponsors = JsonConvert.DeserializeObject<dynamic>(jsonText);

            return sponsors;
        }
		#endregion

		#region GLSVerification
		public dynamic GetAllDistricts_helper(VillageOrg root)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				var val = PostData("https://www.ikp.serp.ap.gov.in/VISERP/Api/SerpBL/GetAllDistricts", root);
				var data = GetSerialzedData<dynamic>(val);
				obj.Status = 100;
				obj.Reason = "Data Loaded Successfully.";
				obj.Details = data;
				return obj;
			}
			catch (Exception ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://www.ikp.serp.ap.gov.in/VISERP/Api/SerpBL/GetAllDistricts", "2");
                obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
				return obj;
			}

		}

		public dynamic LoadaAllBanks_helper(VillageOrg root)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				var val = PostData("https://www.ikp.serp.ap.gov.in/VISERP/Api/SerpBL/GetAllBanks", root);
				var data = GetSerialzedData<dynamic>(val);
				obj.Status = 100;
				obj.Reason = "Data Loaded Successfully.";
				obj.Details = data;
				return obj;
			}
			catch (Exception ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://www.ikp.serp.ap.gov.in/VISERP/Api/SerpBL/GetAllBanks", "2");
                obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
				return obj;
			}

		}

		public dynamic GetAllMandals_helper(VillageOrg root)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				var val = PostData("https://www.ikp.serp.ap.gov.in/VISERP/Api/SerpBL/GetAllMandals", root);
				var data = GetSerialzedData<dynamic>(val);
				obj.Status = 100;
				obj.Reason = "Data Loaded Successfully.";
				obj.Details = data;
				return obj;
			}
			catch (Exception ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://www.ikp.serp.ap.gov.in/VISERP/Api/SerpBL/GetAllMandals", "2");
                obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
				return obj;
			}

		}

		public dynamic GetVillageOrganisation(VillageOrg root)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				var val = PostData("https://www.ikp.serp.ap.gov.in/VISERP/Api/SerpBL/GetAllVOs", root);
				var data = GetSerialzedData<dynamic>(val);
				obj.Status = 100;
				obj.Reason = "Data Loaded Successfully.";
				obj.Details = data;
				return obj;
			}
			catch (Exception ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://www.ikp.serp.ap.gov.in/VISERP/Api/SerpBL/GetAllVOs", "2");
                obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
				return obj;
			}

		}

		public dynamic GetAllBLLoans(BankLoans root)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				var val = PostData("https://www.ikp.serp.ap.gov.in/VISERP/Api/SerpBL/GetAllBLLoans", root);
				var data = GetSerialzedData<dynamic>(val);
				obj.Status = 100;
				obj.Reason = "Data Loaded Successfully.";
				obj.Details = data;
				return obj;
			}
			catch (Exception ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://www.ikp.serp.ap.gov.in/VISERP/Api/SerpBL/GetAllBLLoans", "2");
                obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
				return obj;
			}

		}

		public dynamic LoadHelpGroups(VillageOrg root)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				var val = PostData("https://www.ikp.serp.ap.gov.in/VISERP/Api/SerpBL/GetAllShgs", root);
				var data = GetSerialzedData<dynamic>(val);
				obj.Status = 100;
				obj.Reason = "Data Loaded Successfully.";
				obj.Details = data;
				return obj;
			}
			catch (Exception ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://www.ikp.serp.ap.gov.in/VISERP/Api/SerpBL/GetAllShgs", "2");
                obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
				return obj;
			}

		}
		#endregion

		#region SRSVerification

		public dynamic GetSRSDistricts_helper(SRSGeoGraphical root)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				var val = PostData("https://www.ikp.serp.ap.gov.in/VIService/Api/Sthreenidhi/GetDistrict", root);
				var data = GetSerialzedData<dynamic>(val);
				obj.Status = 100;
				obj.Reason = "Data Loaded Successfully.";
				obj.Details = data;
				return obj;
			}
			catch (Exception ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://www.ikp.serp.ap.gov.in/VIService/Api/Sthreenidhi/GetDistrict", "2");
                obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
				return obj;
			}

		}

		public dynamic GetSRSMandals_helper(SRSGeoGraphical root)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				var val = PostData("https://www.ikp.serp.ap.gov.in/VIService/Api/Sthreenidhi/GetMandals", root);
				var data = GetSerialzedData<dynamic>(val);
				obj.Status = 100;
				obj.Reason = "Data Loaded Successfully.";
				obj.Details = data;
				return obj;
			}
			catch (Exception ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://www.ikp.serp.ap.gov.in/VIService/Api/Sthreenidhi/GetMandals", "2");
                obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
				return obj;
			}

		}

		public dynamic GetSRSVillageOrganisation(SRSGeoGraphical root)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				var val = PostData("https://www.ikp.serp.ap.gov.in/VIService/Api/Sthreenidhi/GetVillageOrganisation", root);
				var data = GetSerialzedData<dynamic>(val);
				obj.Status = 100;
				obj.Reason = "Data Loaded Successfully.";
				obj.Details = data;
				return obj;
			}
			catch (Exception ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://www.ikp.serp.ap.gov.in/VIService/Api/Sthreenidhi/GetVillageOrganisation", "2");
                obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
				return obj;
			}

		}

		public dynamic GetSRSHelpGroups(SRSGeoGraphical root)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				var val = PostData("https://www.ikp.serp.ap.gov.in/VIService/Api/Sthreenidhi/GetSHGS", root);
				var data = GetSerialzedData<dynamic>(val);
				obj.Status = 100;
				obj.Reason = "Data Loaded Successfully.";
				obj.Details = data;
				return obj;
			}
			catch (Exception ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://www.ikp.serp.ap.gov.in/VIService/Api/Sthreenidhi/GetSHGS", "2");
                obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
				return obj;
			}

		}

		public dynamic GetProjectCategory(SRSGeoGraphical root)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				var val = PostData("https://www.ikp.serp.ap.gov.in/VIService/Api/Sthreenidhi/GetProjectCategory", root);
				var data = GetSerialzedData<dynamic>(val);
				obj.Status = 100;
				obj.Reason = "Data Loaded Successfully.";
				obj.Details = data;
				return obj;
			}
			catch (Exception ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://www.ikp.serp.ap.gov.in/VIService/Api/Sthreenidhi/GetProjectCategory", "2");
                obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
				return obj;
			}

		}

		public dynamic GetRequestType(SRSGeoGraphical root)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				var val = PostData("https://www.ikp.serp.ap.gov.in/VIService/Api/Sthreenidhi/GetRequestType", root);
				var data = GetSerialzedData<dynamic>(val);
				obj.Status = 100;
				obj.Reason = "Data Loaded Successfully.";
				obj.Details = data;
				return obj;
			}
			catch (Exception ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://www.ikp.serp.ap.gov.in/VIService/Api/Sthreenidhi/GetRequestType", "2");
                obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
				return obj;
			}

		}

		public dynamic GetMemberDetails(SRSGeoGraphical root)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				var val = PostData("https://www.ikp.serp.ap.gov.in/VIService/Api/Sthreenidhi/GetMemberDetails", root);
				var data = GetSerialzedData<dynamic>(val);
				obj.Status = 100;
				obj.Reason = "Data Loaded Successfully.";
				obj.Details = data;
			}
			catch (Exception ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://www.ikp.serp.ap.gov.in/VIService/Api/Sthreenidhi/GetMemberDetails", "2");
                obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
			}

            return obj;

        }

		public dynamic GetEligibleDetails(SRSGeoGraphical root)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				var val = PostData("https://www.ikp.serp.ap.gov.in/VIService/Api/Sthreenidhi/GetEligibleDetails", root);
				var data = GetSerialzedData<dynamic>(val);
				obj.Status = 100;
				obj.Reason = "Data Loaded Successfully.";
				obj.Details = data;
				return obj;
			}
			catch (Exception ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://www.ikp.serp.ap.gov.in/VIService/Api/Sthreenidhi/GetEligibleDetails", "2");
                obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
				return obj;
			}

		}

		public dynamic LoanRequestInsert(SRSLoanRequest root)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				var val = PostData("https://www.ikp.serp.ap.gov.in/VIService/Api/Sthreenidhi/LoanRequestInsert", root);
				var data = GetSerialzedData<dynamic>(val);
				obj.Status = 100;
				obj.Reason = "Data Loaded Successfully.";
				obj.Details = data;
				return obj;
			}
			catch (Exception ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://www.ikp.serp.ap.gov.in/VIService/Api/Sthreenidhi/LoanRequestInsert", "2");
                obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
				return obj;
			}

		}

		public dynamic GetActivityDetails(SRSActivities root)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				var val = PostData("https://www.ikp.serp.ap.gov.in/VIService/Api/Sthreenidhi/LoanRequestInsert", root);
				var data = GetSerialzedData<dynamic>(val);
				obj.Status = 100;
				obj.Reason = "Data Loaded Successfully.";
				obj.Details = data;
				return obj;
			}
			catch (Exception ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://www.ikp.serp.ap.gov.in/VIService/Api/Sthreenidhi/LoanRequestInsert", "2");
                obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
				return obj;
			}

		}


		#endregion

		#region YSR Asara Status & Verification
		public dynamic GetAllYSRLoans(BankLoans root)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				var val = PostData("https://www.ikp.serp.ap.gov.in/VISERP/Api/SerpBL/GetAllYSRLoans", root);
				var data = GetSerialzedData<dynamic>(val);
				obj.Status = 100;
				obj.Reason = "Data Loaded Successfully.";
				obj.Details = data;
				return obj;
			}
			catch (Exception ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://www.ikp.serp.ap.gov.in/VISERP/Api/SerpBL/GetAllYSRLoans", "2");
                obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
				return obj;
			}

		}
		#endregion

		#region VLR Status & Verification
		public dynamic GetAllVLRLoans(BankLoans root)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				var val = PostData("https://www.ikp.serp.ap.gov.in/VISERP/Api/SerpBL/GetAllVLRLoans", root);
				var data = GetSerialzedData<dynamic>(val);
				obj.Status = 100;
				obj.Reason = "Data Loaded Successfully.";
				obj.Details = data;
				return obj;
			}
			catch (Exception ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://www.ikp.serp.ap.gov.in/VISERP/Api/SerpBL/GetAllVLRLoans", "2");
                obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
				return obj;
			}

		}

		#endregion

		#region Pension Kanuka Apllication
		public dynamic ValidatePensionLogin(dynamic jsonData)
		{

			try
			{
				using (var client = new HttpClient())
				{
					PENSIONApplicationLogin objLogin = new PENSIONApplicationLogin();
					objLogin.userName = jsonData.userName.ToString();
					objLogin.password = "fe01ce2a7fbac8fafaed7c982a04e229";

					ServicePointManager.Expect100Continue = true;
					ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

					System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

					var req = (HttpWebRequest)WebRequest.Create("https://sspensions.ap.gov.in/APTSPensionNewApp/rest/login/loginValidate");
					req.ContentType = "application/json";
					req.Method = "POST";

					req.AllowAutoRedirect = false;
					var _jsonObject = JsonConvert.SerializeObject(objLogin);

					if (!String.IsNullOrEmpty(_jsonObject))
					{
						using (System.IO.Stream s = req.GetRequestStream())
						{
							using (System.IO.StreamWriter sw = new System.IO.StreamWriter(s))
								sw.Write(_jsonObject);
						}
					}

					var resp = (HttpWebResponse)req.GetResponse();
					var sr = new StreamReader(resp.GetResponseStream());

					if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
						(resp.StatusCode == HttpStatusCode.RedirectMethod))
					{
						objResponse.Status = "Failed";
						objResponse.data = "";
					}
					else
					{

						string data = sr.ReadToEnd().Trim();
						objResponse.Status = "Success";
						objResponse.data = JsonConvert.DeserializeObject<PensionLogin>(data);
					}


				}
			}
			catch (WebException ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://sspensions.ap.gov.in/APTSPensionNewApp/rest/login/loginValidate", "2");
                string mappath = HttpContext.Current.Server.MapPath("PENSIONExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error Pension Login :" + ex.Message.ToString()));

				objResponse.Status = "Failed";
				objResponse.data = ThirdpartyMessage;
			}
			return objResponse;

		}

		public dynamic ValidatePensionUID(dynamic jsonData)
		{

			try
			{
				using (var client = new HttpClient())
				{
					//jsonData.loginId = "0114010100001";

					ServicePointManager.Expect100Continue = true;
					ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

					System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

					var req = (HttpWebRequest)WebRequest.Create("https://sspensions.ap.gov.in/APTSPensionNewApp/rest/newApplicationRaise/validateAadhaar");
					req.ContentType = "application/json";
					req.Method = "POST";

					req.AllowAutoRedirect = false;
					var _jsonObject = JsonConvert.SerializeObject(jsonData);

					if (!String.IsNullOrEmpty(_jsonObject))
					{
						using (System.IO.Stream s = req.GetRequestStream())
						{
							using (System.IO.StreamWriter sw = new System.IO.StreamWriter(s))
								sw.Write(_jsonObject);
						}
					}

					var resp = (HttpWebResponse)req.GetResponse();
					var sr = new StreamReader(resp.GetResponseStream());

					if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
						(resp.StatusCode == HttpStatusCode.RedirectMethod))
					{
						objResponse.Status = "Failed";
						objResponse.data = "";
					}
					else
					{

						string data = sr.ReadToEnd().Trim();
						objResponse.Status = "Success";
						objResponse.data = JsonConvert.DeserializeObject<dynamic>(data);
					}


				}
			}
			catch (WebException ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://sspensions.ap.gov.in/APTSPensionNewApp/rest/newApplicationRaise/validateAadhaar", "2");
                string mappath = HttpContext.Current.Server.MapPath("PENSIONExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error Pension UID :" + ex.Message.ToString()));

				objResponse.Status = "Failed";
				objResponse.data = ThirdpartyMessage;
			}
			return objResponse;

		}

		public dynamic ValidatePensionRation(dynamic jsonData)
		{

			try
			{
				using (var client = new HttpClient())
				{
					//jsonData.loginId = "0114010100001";

					ServicePointManager.Expect100Continue = true;
					ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

					System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

					var req = (HttpWebRequest)WebRequest.Create("https://sspensions.ap.gov.in/APTSPensionNewApp/rest/newApplicationRaise/validateRationCard");
					req.ContentType = "application/json";
					req.Method = "POST";

					req.AllowAutoRedirect = false;
					var _jsonObject = JsonConvert.SerializeObject(jsonData);

					if (!String.IsNullOrEmpty(_jsonObject))
					{
						using (System.IO.Stream s = req.GetRequestStream())
						{
							using (System.IO.StreamWriter sw = new System.IO.StreamWriter(s))
								sw.Write(_jsonObject);
						}
					}

					var resp = (HttpWebResponse)req.GetResponse();
					var sr = new StreamReader(resp.GetResponseStream());

					if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
						(resp.StatusCode == HttpStatusCode.RedirectMethod))
					{
						objResponse.Status = "Failed";
						objResponse.data = "";
					}
					else
					{

						string data = sr.ReadToEnd().Trim();
						objResponse.Status = "Success";
						objResponse.data = JsonConvert.DeserializeObject<RationResponse>(data);
					}


				}
			}
			catch (WebException ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://sspensions.ap.gov.in/APTSPensionNewApp/rest/newApplicationRaise/validateRationCard", "2");
                string mappath = HttpContext.Current.Server.MapPath("PENSIONExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetApplicantStatus:" + ex.Message.ToString()));

				objResponse.Status = "Failed";
				objResponse.data = ThirdpartyMessage;
			}
			return objResponse;

		}

		public dynamic ValidateSadaram(dynamic jsonData)
		{

			try
			{
				using (var client = new HttpClient())
				{
					//jsonData.loginId = "0114010100001";

					ServicePointManager.Expect100Continue = true;
					ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

					System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

					var req = (HttpWebRequest)WebRequest.Create("https://sspensions.ap.gov.in/APTSPensionNewApp/rest/newApplicationRaise/validateSadarem");
					req.ContentType = "application/json";
					req.Method = "POST";

					req.AllowAutoRedirect = false;
					var _jsonObject = JsonConvert.SerializeObject(jsonData);

					if (!String.IsNullOrEmpty(_jsonObject))
					{
						using (System.IO.Stream s = req.GetRequestStream())
						{
							using (System.IO.StreamWriter sw = new System.IO.StreamWriter(s))
								sw.Write(_jsonObject);
						}
					}

					var resp = (HttpWebResponse)req.GetResponse();
					var sr = new StreamReader(resp.GetResponseStream());

					if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
						(resp.StatusCode == HttpStatusCode.RedirectMethod))
					{
						objResponse.Status = "Failed";
						objResponse.data = "";
					}
					else
					{

						string data = sr.ReadToEnd().Trim();
						objResponse.Status = "Success";
						objResponse.data = JsonConvert.DeserializeObject<SadaramResponse>(data);
					}


				}
			}
			catch (WebException ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://sspensions.ap.gov.in/APTSPensionNewApp/rest/newApplicationRaise/validateSadarem", "2");
                string mappath = HttpContext.Current.Server.MapPath("PENSIONExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetApplicantStatus:" + ex.Message.ToString()));

				objResponse.Status = "Failed";
				objResponse.data = ThirdpartyMessage;
			}
			return objResponse;

		}

		public dynamic savePensiondata(dynamic jsonData)
		{

			try
			{
				using (var client = new HttpClient())
				{
					//jsonData.loginId = "0114010100001";
					string data = "";


					try
					{
						jsonData.imeiNumber = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
					}
					catch (Exception)
					{

						jsonData.imeiNumber = "";
					}



					//jsonData.imeiNumber = GetIPAddress(Dns.GetHostName());

					ServicePointManager.Expect100Continue = true;
					ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

					System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

					var req = (HttpWebRequest)WebRequest.Create("https://sspensions.ap.gov.in/APTSPensionNewApp/rest/validateNewApplication/newApplicationRegistration");
					req.ContentType = "application/json";
					req.Method = "POST";

					req.AllowAutoRedirect = false;
					var _jsonObject = JsonConvert.SerializeObject(jsonData);

					if (!String.IsNullOrEmpty(_jsonObject))
					{
						using (System.IO.Stream s = req.GetRequestStream())
						{
							using (System.IO.StreamWriter sw = new System.IO.StreamWriter(s))
								sw.Write(_jsonObject);
						}
					}

					string mappath2 = HttpContext.Current.Server.MapPath("PENSIONSaveLog");
					Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath2, "Input Data  : " + _jsonObject));


					var resp = (HttpWebResponse)req.GetResponse();
					var sr = new StreamReader(resp.GetResponseStream());

					if ((resp.StatusCode == HttpStatusCode.Redirect) || (resp.StatusCode == HttpStatusCode.SeeOther) ||
						(resp.StatusCode == HttpStatusCode.RedirectMethod))
					{
						objResponse.Status = "Failed";
						objResponse.data = "";
					}
					else
					{

						data = sr.ReadToEnd().Trim();
						SavPensionResponse objpesniom = JsonConvert.DeserializeObject<SavPensionResponse>(data);

						transactionModel objtrans = new transactionModel();
						objtrans.TYPE = "2";
						objtrans.TXN_ID = jsonData.gsws_Id.ToString();
						objtrans.DEPT_ID = "3103";
						objtrans.DEPT_TXN_ID = objpesniom.success.Replace("New Application Registered Successfully. Application ID", "");
						objtrans.STATUS_CODE = "01";
						objtrans.REMARKS = objpesniom.success;
						DataTable dt = new transactionHelper().transactionInsertion(objtrans);
						if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
						{
							objResponse.Status = "Success";
							objResponse.data = JsonConvert.DeserializeObject<SavPensionResponse>(data);
						}
					}

					string mappath3 = HttpContext.Current.Server.MapPath("PENSIONSaveResponseTaxDataLog");
					Task WriteTask3 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath3, "Respose Status : " + objResponse.Status + "Respose Data :" + data));
				}
			}
			catch (WebException ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://sspensions.ap.gov.in/APTSPensionNewApp/rest/validateNewApplication/newApplicationRegistration", "2");
                string mappath = HttpContext.Current.Server.MapPath("PENSIONExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error :" + ex.Message.ToString()));

				objResponse.Status = "Failed";
				objResponse.data = ThirdpartyMessage;
			}
			return objResponse;

		}

		public dynamic GetPensionAppStatus(PensionAppCls root)
		{
			dynamic obj = new ExpandoObject();
			try
			{
				root.userName = "SSPPENSIONS";
				root.password = "B205A8A530701C7824C52D6AE65B2BAD";
				root.type = "gre";

				var val = PostData("https://sspensions.ap.gov.in/APTSPensionNewApp/rest/AptsNewApplicationStatus/getNewApplicationStatus", root);
				var data = GetSerialzedData<dynamic>(val);
				obj.Status = 100;
				obj.Reason = "Data Loaded Successfully.";
				obj.Details = data;
				return obj;
			}
			catch (Exception ex)
			{
                Common_SERP_Error(ex.Message.ToString(), "https://sspensions.ap.gov.in/APTSPensionNewApp/rest/AptsNewApplicationStatus/getNewApplicationStatus", "2");
                obj.Status = 102;
				obj.Reason = CommonSPHel.ThirdpartyMessage;
				return obj;
			}

		}
        public bool Common_SERP_Error(string msg, string url, string etype)
        {
            ExceptionDataModel objex = new ExceptionDataModel();
            try
            {
                objex.E_DEPTID = DepartmentEnum.Department.Social_Tribal_Welfare.ToString();
                objex.E_HODID = DepartmentEnum.HOD.Social_Welfare.ToString();
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

        #endregion
    }
}