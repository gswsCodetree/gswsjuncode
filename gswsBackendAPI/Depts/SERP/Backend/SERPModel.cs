using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Dept.SERP.Backend
{
	public class SERPModel
	{
	}

	#region PENSION
	public class PENSIONModel
	{
		public string CRI { get; set; }
		public string INPUT { get; set; }
		public string UID { get; set; }
		public string PID { get; set; }
	}
    #endregion

    public class TrackingData
    {
        public string Entry_Date { get; set; }
        public string Stage { get; set; }
    }

    public class Datum
    {
        public string claimuid { get; set; }
        public string Uid_Num { get; set; }
        public string Citizen_Name { get; set; }
        public string Father_Hus_Name { get; set; }
        public string Death_Date { get; set; }
        public string Death_Type { get; set; }
        public string Nominee_Name { get; set; }
        public string Nominee_Aadhaar { get; set; }
        public string Mobile_Number { get; set; }
        public string Reg_Date { get; set; }
        public string Sts { get; set; }
        public string Msg { get; set; }
        public string StatusName { get; set; }
        public List<TrackingData> TrackingData { get; set; }
        //reg
        public string DISTRICT_ID { get; set; }
        public string DISTRICT_NAME { get; set; }
        public string MANDAL_ID { get; set; }
        public string MANDAL_NAME { get; set; }
        public string pancode { get; set; }
        public string panname { get; set; }
        public string VILLAGE_ID { get; set; }
        public string VILLAGE_NAME { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string id { get; set; }
        public string CauseID { get; set; }
        public string CauseName { get; set; }
    }

    public class BimaRegistration
    {
        public string Uid { get; set; }
        public string Did { get; set; }

        public string Incident_Date { get; set; }

        public string Incident_Time { get; set; }

        public string Accident_Date { get; set; }

        public string Accident_Time { get; set; }

        public string Claimtype { get; set; }

        public string Cause { get; set; }

        public string CauseSubReason { get; set; }

        public string Nominee_Phone { get; set; }

        public string Nominee_Name { get; set; }
        public string Nom_Uid { get; set; }

        public string Incident_Type { get; set; }

        public string incident_Place { get; set; }

        public string Accident_Place { get; set; }

        public string Informer_Phone { get; set; }

        public string Informed_By { get; set; }

        public string Reg_User { get; set; }
        public string Remarks { get; set; }

        public string Street { get; set; }

        public string Door_Num { get; set; }

        public string Ward { get; set; }

        public string Village_Name { get; set; }

        public string Gram_Panchayat { get; set; }

        public string Mandal_Name { get; set; }

        public string EID { get; set; }

        public string Pol_Pincode { get; set; }

        public string Nom_EID { get; set; }
        public string Nom_Pincode { get; set; }
        public string VV_Name { get; set; }

        public string VV_Phone { get; set; }
        public string VS_id { get; set; }
        public string VS_Location { get; set; }

        public string VS_Panchayat { get; set; }
        public string EntryBy { get; set; }
		public string GSWS_ID { get; set; }
	}

    public class ClaimObject
    {
        public List<Datum> Data { get; set; }
    }

    #region GLSVerification
    public class VillageOrg
    {
        public string village_id { get; set; }
        public string District_id { get; set; }
        public string Vo_id { get; set; }
    }

    public class BankLoans
    {
        public string Shg_Id { get; set; }
        public string SB_Account_No { get; set; }
        public string Loan_Account_No { get; set; }
        public string Bank_Code { get; set; }
    }

    public class Hearders
    {
        public string key { get; set; }
        public string value { get; set; }
    }
    #endregion

    #region SRSVerification
    public class SRSGeoGraphical
    {
        public string DISTRICT_ID { get; set; }
        public string MANDAL_ID { get; set; }
        public string VO_ID { get; set; }
        public string SHG_ID { get; set; }
        public string MEMBER_ID { get; set; }
        public string PROJECT_CATEGORY { get; set; }
    }

    public class SRSLoanRequest
    {
        public string VO_ID { get; set; }
        public string SHG_ID { get; set; }
        public string MEMBER_ID { get; set; }
        public string PROJECT_CATEGORY { get; set; }
        public string ACTIVITY_CODE { get; set; }
        public string ACTIVITY_NAME { get; set; }
        public string LOAN_AMOUNT { get; set; }
        public string MOBILE_NO { get; set; }
        public string CREATED_BY { get; set; }
        public string REGISTRATION_CODE { get; set; }

    }

    public class SRSActivities
    {
        public string MAJOR_ACTIVITY_TYPE { get; set; }
        public string PROJECT_CATEGORY { get; set; }
        public string MAJOR_ACTIVITY { get; set; }
    }

    public class SRSEligible
    {
        public string MAJOR_ACTIVITY_TYPE { get; set; }
        public string PROJECT_CATEGORY { get; set; }
        public string MAJOR_ACTIVITY { get; set; }
    }

	#endregion

	

	public class PensionTypeList
	{
		public string typeId { get; set; }
		public string typeName { get; set; }
	}

	public class PensionLogin
	{
		public string distcode { get; set; }
		public string distname { get; set; }
		public string mndcode { get; set; }
		public string mndname { get; set; }
		public string pancode { get; set; }
		public string panname { get; set; }
		public string villagecode { get; set; }
		public string vilalgename { get; set; }
		public string habcode { get; set; }
		public string habname { get; set; }
		public string volunteerName { get; set; }
		public List<PensionTypeList> pensionTypeList { get; set; }
		public List<Panchayat> panchayat { get; set; }
		public string response { get; set; }
		public string token { get; set; }
	}

	public class Panchayat
	{
		public string pancode { get; set; }
		public string panname { get; set; }
		public string habcode { get; set; }
		public string habname { get; set; }
	}

	public class PensionResponse
	{
		public string Status { get; set; }
		public string REASON { get; set; }
		public dynamic data { get; set; }
	}
	public class PENSIONApplicationLogin
	{
		public string userName { get; set; }
		public string password { get; set; }

	}
	public class Aadhaar
	{
        public List<string> aadharValidations { get; set; }
        public string webland { get; set; }
        public string caste { get; set; }
        public string subCaste { get; set; }
        public string houseHoldId { get; set; }
        public string aadhaarRationCardNumber { get; set; }
        public string aadhaarDistrictId { get; set; }
        public string aadhaarDistrictName { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string dateOfBirth { get; set; }
        public string age { get; set; }
        public string photo { get; set; }
        public string relationName { get; set; }
        public string gender { get; set; }
		public string response { get; set; }
	}

	public class PensionAadhardata
	{
		public List<Aadhaar> aadhaar { get; set; }
	}
	public class RationMembers
	{
		public string slNo { get; set; }
		public string relationName { get; set; }
		public string dob { get; set; }
		public int age { get; set; }
		public string gender { get; set; }
		public string memberName { get; set; }
		public string district { get; set; }
		public string aadharcardNo { get; set; }
		public string rationcardDistrict { get; set; }

	}
	public class RationResponse
	{
		public string aadhaarNumber { get; set; }
		public string rationCardNumber { get; set; }
		public string aadhaarRationCardNumber { get; set; }
		public string dateOfBirth { get; set; }
		public int age { get; set; }
		public string firstName { get; set; }
		public string middleName { get; set; }
		public string relationName { get; set; }
		public string pensionType { get; set; }
		public string gender { get; set; }
		public string houseHoldId { get; set; }
		public string webland { get; set; }
		public List<RationMembers> rationcardList { get; set; }
		public string s_districtId { get; set; }
		public string s_districtName { get; set; }
		public string s_mandalId { get; set; }
		public string s_mandalName { get; set; }
		public string s_panchayatId { get; set; }
		public string s_panchayatName { get; set; }
		public string s_villageId { get; set; }
		public string s_villageName { get; set; }
		public string s_habitationId { get; set; }
		public string s_habitationName { get; set; }
		public string response { get; set; }
	}
	public class SadaramRequest
	{
		public string aadhaarNumber { get; set; }
		public string rationCardNumber { get; set; }
		public string aadhaarRationCardNumber { get; set; }
		public string dateOfBirth { get; set; }
		public int age { get; set; }
		public string firstName { get; set; }
		public string middleName { get; set; }
		public string relationName { get; set; }
		public string pensionType { get; set; }
		public string gender { get; set; }
		public string houseHoldId { get; set; }
		public string webland { get; set; }
		public List<RationMembers> rationcardList { get; set; }
		public string s_districtId { get; set; }
		public string s_districtName { get; set; }
		public string s_mandalId { get; set; }
		public string s_mandalName { get; set; }
		public string s_panchayatId { get; set; }
		public string s_panchayatName { get; set; }
		public string s_villageId { get; set; }
		public string s_villageName { get; set; }
		public string s_habitationId { get; set; }
		public string s_habitationName { get; set; }
		public string flag { get; set; }
		public string sadaremId { get; set; }
	}
	public class RationRequest
	{
		public string webland { get; set; }
		public string caste { get; set; }
		public string subCaste { get; set; }
		public string houseHoldId { get; set; }
		public string aadhaarRationCardNumber { get; set; }
		public string aadhaarDistrictId { get; set; }
		public string aadhaarDistrictName { get; set; }
		public string firstName { get; set; }
		public string middleName { get; set; }
		public string dateOfBirth { get; set; }
		public string age { get; set; }
		public string photo { get; set; }
		public string relationName { get; set; }
		public string gender { get; set; }
		public string pensionType { get; set; }
		public string rationCardNumber { get; set; }
		public string s_districtId { get; set; }
		public string s_districtName { get; set; }
		public string s_mandalId { get; set; }
		public string s_mandalName { get; set; }
		public string s_habitationId { get; set; }
		public string s_habitationName { get; set; }
		public string s_panchayatId { get; set; }
		public string s_panchayatName { get; set; }
		public string s_villageId { get; set; }
		public string s_villageName { get; set; }
		public string flag { get; set; }
		public string aadhaarNumber { get; set; }
	}
	public class SadaramResponse
	{
		public string aadhaarNumber { get; set; }
		public string rationCardNumber { get; set; }
		public string aadhaarRationCardNumber { get; set; }
		public string dateOfBirth { get; set; }
		public string age { get; set; }
		public string firstName { get; set; }
		public string middleName { get; set; }
		public string relationName { get; set; }
		public string pensionType { get; set; }
		public string gender { get; set; }
		public string houseHoldId { get; set; }
		public string webland { get; set; }
		public string s_districtId { get; set; }
		public string s_districtName { get; set; }
		public string s_mandalId { get; set; }
		public string s_mandalName { get; set; }
		public string s_panchayatId { get; set; }
		public string s_panchayatName { get; set; }
		public string s_villageId { get; set; }
		public string s_villageName { get; set; }
		public string s_habitationId { get; set; }
		public string s_habitationName { get; set; }
		public string sadaremId { get; set; }
		public string sadaremRationCard { get; set; }
		public string sadaremUid { get; set; }
		public string disabilityType { get; set; }
		public string percentage { get; set; }
		public string sadaremName { get; set; }
		public string disabilityStatus { get; set; }
		public string response { get; set; }
	}
	public class PensionApplicationRequest
	{
		public string aadhaarNumber { get; set; }
		public string rationCardNumber { get; set; }
		public string aadhaarRationCardNumber { get; set; }
		public string dateOfBirth { get; set; }
		public string age { get; set; }
		public string firstName { get; set; }
		public string middleName { get; set; }
		public string relationName { get; set; }
		public string pensionType { get; set; }
		public string gender { get; set; }
		public string houseHoldId { get; set; }
		public string webland { get; set; }
		public string s_districtId { get; set; }
		public string s_districtName { get; set; }
		public string s_mandalId { get; set; }
		public string s_mandalName { get; set; }
		public string s_panchayatId { get; set; }
		public string s_panchayatName { get; set; }
		public string s_villageId { get; set; }
		public string s_villageName { get; set; }
		public string s_habitationId { get; set; }
		public string s_habitationName { get; set; }
		public string sadaremId { get; set; }
		public string disabilityType { get; set; }
		public string percentage { get; set; }
		public string disabilityStatus { get; set; }
		public string fileSize { get; set; }
		public string imeiNumber { get; set; }
		public string appliedMonth { get; set; }
		public string hNo { get; set; }
		public string i_caste { get; set; }
		public string sadaremSelection { get; set; }
		public string street { get; set; }
		public string loginId { get; set; }
		public string remarks { get; set; }
		public string rationSelection { get; set; }
		public string phoneNumber { get; set; }
		public string caste { get; set; }
		public string subCaste { get; set; }
		public string uploadDocument { get; set; }
	}
	public class SavPensionResponse
	{
		public string districtidRemarks { get; set; }
		public string MandalIdRemarks { get; set; }
		public string PanchayatIdRemarks { get; set; }
		public string PensionSchemeRemarks { get; set; }
		public string pensionerFirstNameRemarks { get; set; }
		public string pensionerMiddleNameRemarks { get; set; }
		public string pensionerRelationNameRemarks { get; set; }
		public string hNoRemarks { get; set; }
		public string StreetRemarks { get; set; }
		public string dobRemarks { get; set; }
		public string ageRemarks { get; set; }
		public string genderRemarks { get; set; }
		public string casteRemarks { get; set; }
		public string HabitationIdRemarks { get; set; }
		public string AppliedMonthRemarks { get; set; }
		public string rationcardRemarks { get; set; }
		public string uidRemarks { get; set; }
		public string SadaremcodeRemarks { get; set; }
		public string DisabilityTypeRemarks { get; set; }
		public string disablityPercentageRemarks { get; set; }
		public string mobileRemarks { get; set; }
		public string usernameRemarks { get; set; }
		public string systemIpRemarks { get; set; }
		public string ApoRemarks { get; set; }
		public string Pss_casteRemarks { get; set; }
		public string Pss_subCasteRemarks { get; set; }
		public string WeblandRemarks { get; set; }
		public string HouseholdIdRemarks { get; set; }
		public string error { get; set; }
		public string success { get; set; }
	}

	public class PensionAppCls
	{
		public string userName { get; set; }
		public string password { get; set; }
		public string districtId { get; set; }
		public string applicationId { get; set; }
		public string type { get; set; }
	}
}