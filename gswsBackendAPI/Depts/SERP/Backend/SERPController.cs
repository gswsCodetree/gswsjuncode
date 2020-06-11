using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using gswsBackendAPI.DL.CommonHel;
using System.Web;
using System.Threading.Tasks;

namespace gswsBackendAPI.Dept.SERP.Backend
{
	[RoutePrefix("api/SERP")]
	public class SERPController : ApiController
    {
		dynamic CatchData = new ExpandoObject();
		SERPHelper SERPhel = new SERPHelper();

		#region PESION
		//Get Applicants Status
		[HttpPost]
		[Route("GetApplicantStatus")]
		public IHttpActionResult GetApplicantStatus(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				//
				//string value = JsonConvert.SerializeObject(data);
				PENSIONModel rootobj = JsonConvert.DeserializeObject<PENSIONModel>(value);
				return Ok(SERPhel.GetApplicantStatus(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}
        #endregion

        #region SerpBima
        [HttpPost]
        [Route("GetClaimStatus")]
        public IHttpActionResult GetClaimStatus(dynamic data)
        {
			string value = token_gen.Authorize_aesdecrpty(data);
			try
            {
                //
                //string value = JsonConvert.SerializeObject(data);
                Datum rootobj = JsonConvert.DeserializeObject<Datum>(value);
                return Ok(SERPhel.GetClaimStatus(rootobj));
            }
            catch (Exception ex)
            {
                CatchData.Status = "Failure";
                CatchData.Reason = SERPHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }
        #endregion

        #region SerpBimaRegistration
        [HttpPost]
        [Route("GetDistricts")]
        public IHttpActionResult GetDistricts(dynamic data)
        {
			string value = token_gen.Authorize_aesdecrpty(data);
			try
            {
              //  string value = JsonConvert.SerializeObject(data);
                Datum rootobj = JsonConvert.DeserializeObject<Datum>(value);
                return Ok(SERPhel.GetDistricts(rootobj));
            }
            catch (Exception ex)
            {
                CatchData.Status = "Failure";
                CatchData.Reason = SERPHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        [HttpPost]
        [Route("GetMandals")]
        public IHttpActionResult GetMandals(dynamic data)
        {
			string value = token_gen.Authorize_aesdecrpty(data);
			try
            {
               // string value = JsonConvert.SerializeObject(data);
                Datum rootobj = JsonConvert.DeserializeObject<Datum>(value);
                return Ok(SERPhel.GetMandals(rootobj));
            }
            catch (Exception ex)
            {
                CatchData.Status = "Failure";
                CatchData.Reason = SERPHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }


        [HttpPost]
        [Route("GetPanchayats")]
        public IHttpActionResult GetPanchayats(dynamic data)
        {
			string value = token_gen.Authorize_aesdecrpty(data);
			try
            {
                //string value = JsonConvert.SerializeObject(data);
                Datum rootobj = JsonConvert.DeserializeObject<Datum>(value);
                return Ok(SERPhel.GetPanchayats(rootobj));
            }
            catch (Exception ex)
            {
                CatchData.Status = "Failure";
                CatchData.Reason = SERPHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }


        [HttpPost]
        [Route("GetVillages")]
        public IHttpActionResult GetVillages(dynamic data)
        {
			string value = token_gen.Authorize_aesdecrpty(data);
			try
            {
               // string value = JsonConvert.SerializeObject(data);
                Datum rootobj = JsonConvert.DeserializeObject<Datum>(value);
                return Ok(SERPhel.GetVillages(rootobj));
            }
            catch (Exception ex)
            {
                CatchData.Status = "Failure";
                CatchData.Reason = SERPHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        [HttpPost]
        [Route("GetClaimtypes")]
        public IHttpActionResult GetClaimtypes(dynamic data)
        {
			string value = token_gen.Authorize_aesdecrpty(data);
			try
            {
               // string value = JsonConvert.SerializeObject(data);
                Datum rootobj = JsonConvert.DeserializeObject<Datum>(value);
                return Ok(SERPhel.GetClaimtypes(rootobj));
            }
            catch (Exception ex)
            {
                CatchData.Status = "Failure";
                CatchData.Reason = SERPHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        [HttpPost]
        [Route("GetCauses")]
        public IHttpActionResult GetCauses(dynamic data)
        {
			string value = token_gen.Authorize_aesdecrpty(data);
			try
            {
               // string value = JsonConvert.SerializeObject(data);
                Datum rootobj = JsonConvert.DeserializeObject<Datum>(value);
                return Ok(SERPhel.GetCauses(rootobj));
            }
            catch (Exception ex)
            {
                CatchData.Status = "Failure";
                CatchData.Reason = SERPHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        [HttpPost]
        [Route("GetIncidenttypes")]
        public IHttpActionResult GetIncidenttypes(dynamic data)
        {
			string value = token_gen.Authorize_aesdecrpty(data);
			try
            {
                //string value = JsonConvert.SerializeObject(data);
                Datum rootobj = JsonConvert.DeserializeObject<Datum>(value);
                return Ok(SERPhel.GetIncidenttypes(rootobj));
            }
            catch (Exception ex)
            {
                CatchData.Status = "Failure";
                CatchData.Reason = SERPHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        [HttpPost]
        [Route("GetEntryBy")]
        public IHttpActionResult GetEntryBy(dynamic data)
        {
			string value = token_gen.Authorize_aesdecrpty(data);
			try
            {
               // string value = JsonConvert.SerializeObject(data);
                Datum rootobj = JsonConvert.DeserializeObject<Datum>(value);
                return Ok(SERPhel.GetEntryBy(rootobj));
            }
            catch (Exception ex)
            {
                CatchData.Status = "Failure";
                CatchData.Reason = SERPHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        [HttpPost]
        [Route("BimaRegistration")]
        public IHttpActionResult BimaRegistration(dynamic data)
        {
			string value = token_gen.Authorize_aesdecrpty(data);
			try
            {
                //string value = JsonConvert.SerializeObject(data);
                BimaRegistration rootobj = JsonConvert.DeserializeObject<BimaRegistration>(value);
                return Ok(SERPhel.BimaRegistration(rootobj));
            }
            catch (Exception ex)
            {
                CatchData.Status = "Failure";
                CatchData.Reason = SERPHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        [HttpPost]
        [Route("GetPensionMaster")]
        public IHttpActionResult GetPensionMaster(dynamic data)
        {
			string value = token_gen.Authorize_aesdecrpty(data);
			try
            {
                
                return Ok(SERPhel.GetPensionMaster());
            }
            catch (Exception ex)
            {
                CatchData.Status = "Failure";
                CatchData.Reason = SERPHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }
		#endregion

		#region BLSVerification
		// Get District
		[HttpPost]
		[Route("GetAllDistricts")]
		public IHttpActionResult GetAllDistricts(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				VillageOrg rootobj = JsonConvert.DeserializeObject<VillageOrg>(value);
				return Ok(SERPhel.GetAllDistricts_helper(rootobj));

			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}

        // Get District
        [HttpPost]
        [Route("LoadaAllBanks")]
        public IHttpActionResult LoadaAllBanks(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {

                //string value = JsonConvert.SerializeObject(data);
                VillageOrg rootobj = JsonConvert.DeserializeObject<VillageOrg>(value);
                return Ok(SERPhel.LoadaAllBanks_helper(rootobj));
                

			}
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = SERPHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }
        }

        // Get Mandal
        [HttpPost]
		[Route("GetAllMandals")]
		public IHttpActionResult GetAllMandals(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
			
				//string value = JsonConvert.SerializeObject(data);
				VillageOrg rootobj = JsonConvert.DeserializeObject<VillageOrg>(value);
				return Ok(SERPhel.GetAllMandals_helper(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}

		// Get Village Organisations
		[HttpPost]
		[Route("GetVillageOrganisation")]
		public IHttpActionResult GetVillageOrganisation(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				VillageOrg rootobj = JsonConvert.DeserializeObject<VillageOrg>(value);
				return Ok(SERPhel.GetVillageOrganisation(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		// Get Help Groups
		[HttpPost]
		[Route("LoadHelpGroups")]
		public IHttpActionResult LoadHelpGroups(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				VillageOrg rootobj = JsonConvert.DeserializeObject<VillageOrg>(value);
				return Ok(SERPhel.LoadHelpGroups(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		// Get All BL Loans
		[HttpPost]
		[Route("GetAllBLLoans")]
		public IHttpActionResult GetAllBLLoans(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				BankLoans rootobj = JsonConvert.DeserializeObject<BankLoans>(value);
                if (Utils.IsAlphaNumeric(rootobj.SB_Account_No) && Utils.IsAlphaNumeric(rootobj.Loan_Account_No))
                    return Ok(SERPhel.GetAllBLLoans(rootobj));
                else
                {
                    dynamic RData = new ExpandoObject();
                    RData.status = 102;
                    RData.Reason = "Special Characters Not Allowed.";
                    return Ok(RData);
                }
            }
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}


		#endregion

		#region SRSVerification
		// Get District
		[HttpPost]
		[Route("GetSRSDistricts")]
		public IHttpActionResult GetSRSDistricts(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				SRSGeoGraphical rootobj = JsonConvert.DeserializeObject<SRSGeoGraphical>(value);
				return Ok(SERPhel.GetSRSDistricts_helper(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}

		// Get Mandal
		[HttpPost]
		[Route("GetSRSMandals")]
		public IHttpActionResult GetSRSMandals(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				SRSGeoGraphical rootobj = JsonConvert.DeserializeObject<SRSGeoGraphical>(value);
				return Ok(SERPhel.GetSRSMandals_helper(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}

		// Get Village Organisations
		[HttpPost]
		[Route("GetSRSVillageOrganisation")]
		public IHttpActionResult GetSRSVillageOrganisation(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				SRSGeoGraphical rootobj = JsonConvert.DeserializeObject<SRSGeoGraphical>(value);
				return Ok(SERPhel.GetSRSVillageOrganisation(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		// Get Help Groups
		[HttpPost]
		[Route("GetSRSHelpGroups")]
		public IHttpActionResult GetSRSHelpGroups(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				SRSGeoGraphical rootobj = JsonConvert.DeserializeObject<SRSGeoGraphical>(value);
				return Ok(SERPhel.GetSRSHelpGroups(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		// Get All BL Loans
		[HttpPost]
		[Route("GetProjectCategory")]
		public IHttpActionResult GetProjectCategory(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				SRSGeoGraphical rootobj = JsonConvert.DeserializeObject<SRSGeoGraphical>(value);
				return Ok(SERPhel.GetProjectCategory(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}

		[HttpPost]
		[Route("GetRequestType")]
		public IHttpActionResult GetRequestType(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				SRSGeoGraphical rootobj = JsonConvert.DeserializeObject<SRSGeoGraphical>(value);
				return Ok(SERPhel.GetRequestType(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}

		[HttpPost]
		[Route("GetMemberDetails")]
		public IHttpActionResult GetMemberDetails(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				SRSGeoGraphical rootobj = JsonConvert.DeserializeObject<SRSGeoGraphical>(value);
				return Ok(SERPhel.GetMemberDetails(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}

		[HttpPost]
		[Route("GetEligibleDetails")]
		public IHttpActionResult GetEligibleDetails(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				SRSGeoGraphical rootobj = JsonConvert.DeserializeObject<SRSGeoGraphical>(value);
				return Ok(SERPhel.GetEligibleDetails(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}

		[HttpPost]
		[Route("LoanRequestInsert")]
		public IHttpActionResult LoanRequestInsert(dynamic data)
		{

			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				//string value = JsonConvert.SerializeObject(data);
				SRSLoanRequest rootobj = JsonConvert.DeserializeObject<SRSLoanRequest>(value);
				return Ok(SERPhel.LoanRequestInsert(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}

		[HttpPost]
		[Route("GetActivityDetails")]
		public IHttpActionResult GetActivityDetails(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				SRSActivities rootobj = JsonConvert.DeserializeObject<SRSActivities>(value);
				return Ok(SERPhel.GetActivityDetails(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}

		#endregion

		#region YSR Asara Status & Verification 
		//Get Applicants Status
		[HttpPost]
		[Route("GetAllYSRLoans")]
		public IHttpActionResult GetAllYSRLoans(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				BankLoans rootobj = JsonConvert.DeserializeObject<BankLoans>(value);
				return Ok(SERPhel.GetAllYSRLoans(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}
		#endregion

		#region VLR Status & Verification 
		//Get Applicants Status
		[HttpPost]
		[Route("GetAllVLRLoans")]
		public IHttpActionResult GetAllVLRLoans(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				//string value = JsonConvert.SerializeObject(data);
				BankLoans rootobj = JsonConvert.DeserializeObject<BankLoans>(value);
				return Ok(SERPhel.GetAllVLRLoans(rootobj));
			}
			catch (Exception ex)
			{
				CatchData.Status = 102;
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}
		#endregion


		#region PESION
		//Get Applicants Status
		[HttpPost]
		[Route("ValidatePensionLogin")]
		public IHttpActionResult ValidatePensionLogin(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				dynamic obj = JsonConvert.DeserializeObject<dynamic>(value);
				return Ok(SERPhel.ValidatePensionLogin(obj));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		//Get Applicants Status
		[HttpPost]
		[Route("ValidatePensionUID")]
		public IHttpActionResult ValidatePensionUID(dynamic JSONdata)
		{
			string value = token_gen.Authorize_aesdecrpty(JSONdata);
			try
			{
				dynamic data = JsonConvert.DeserializeObject<dynamic>(value);
				return Ok(SERPhel.ValidatePensionUID(data));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		//Get Applicants Status
		[HttpPost]
		[Route("ValidatePensionRation")]
		public IHttpActionResult ValidatePensionRation(dynamic JSONdata)
		{
			string value = token_gen.Authorize_aesdecrpty(JSONdata);
			try
			{
				dynamic data = JsonConvert.DeserializeObject<dynamic>(value);
				return Ok(SERPhel.ValidatePensionRation(data));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		//Get Applicants Status
		[HttpPost]
		[Route("ValidateSadaram")]
		public IHttpActionResult ValidateSadaram(dynamic JSONdata)
		{
			string value = token_gen.Authorize_aesdecrpty(JSONdata);
			try
			{
				dynamic data = JsonConvert.DeserializeObject<dynamic>(value);
				return Ok(SERPhel.ValidateSadaram(data));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}
		
		//Get Applicants Status
		[HttpPost]
		[Route("savePensiondata")]
		public IHttpActionResult savePensiondata(dynamic JSONdata)
		{
			string value = token_gen.Authorize_aesdecrpty(JSONdata);
			try
			{
				dynamic data = JsonConvert.DeserializeObject<dynamic>(value);
				string mappath2 = HttpContext.Current.Server.MapPath("PENSIONSaveDataLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath2, value));

				return Ok(SERPhel.savePensiondata(data));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		//Pension Application Status
		[HttpPost]
		[Route("GetPensionAppStatus")]
		public IHttpActionResult GetPensionAppStatus(dynamic JSONdata)
		{
			string value = token_gen.Authorize_aesdecrpty(JSONdata);
			try
			{
				PensionAppCls data = JsonConvert.DeserializeObject<PensionAppCls>(value);
				if (Utils.IsAlphaNumeric(data.districtId) && Utils.IsAlphaNumeric(data.applicationId))
					return Ok(SERPhel.GetPensionAppStatus(data));
				else
				{
					CatchData.Status = 102;
					CatchData.Reason = "Special Characters Are Not Allowed.";
					return Ok(CatchData);
				}
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = SERPHelper.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}
		#endregion


	}
}
