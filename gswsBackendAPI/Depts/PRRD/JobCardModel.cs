using gswsBackendAPI.DL.CommonHel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Depts.PRRD
{
	public class JobCardBankModel : LogModel
	{
		public string TYPE { get; set; }
		public string BankName { get; set; }
		public string BranchName { get; set; }
		public string IFSCCode { get; set; }
	}
	public class JobCardModel : LogModel
	{
		public string P_TYPE { get; set; }
		public string P_JC_ID { get; set; }
		public string P_LGD_DISTRICT_CODE { get; set; }
		public string P_LGD_MANDAL_CODE { get; set; }
		public string P_GP_ID { get; set; }
		public string P_GP_NAME { get; set; }
		public string P_HB_CODE { get; set; }
		public string P_HB_NAME { get; set; }
		public DateTime P_REGD_DATE { get; set; }
		public string P_APPLICATION_NUMBER { get; set; }
		public string P_GP_SECRETARY_NAME { get; set; }
		public string P_CASTE { get; set; }
		public string P_ADDRESS { get; set; }
		public string P_ADDRESS_TEL { get; set; }
		public string P_RATIONCARD_NO { get; set; }
		public string P_BPL_NO { get; set; }
		public string P_KHATA_NO { get; set; }
		public string P_LAND_OWNER { get; set; }
		public string P_LAND_OWNER_ACRES { get; set; }
		public string P_ASSIGNED_LAND_BENEFICIARY { get; set; }
		public string P_IAY_BENEFICIARY { get; set; }
		public string P_MEMBER_NAME { get; set; }
		public string P_MEMBER_NAME_TEL { get; set; }
		public string P_SUR_NAME { get; set; }
		public string P_SUR_NAME_TEL { get; set; }
		public string P_FAMILY_HEAD { get; set; }
		public string P_RELATION_HOF { get; set; }
		public string P_GENDER { get; set; }
		public string P_AGE { get; set; }
		public string P_HIV { get; set; }
		public string P_DISABLED { get; set; }
		public string P_SHG_MEMBER { get; set; }
		public string P_SHG_ID { get; set; }
		public string P_SHG_NAME { get; set; }
		public string P_PERMANENT_JOB_CARD { get; set; }
		public string P_MPHSS_ID { get; set; }
		public string P_PAYING_AGENCY_TYPE { get; set; }
		public string P_PAYING_AGENCY_NAME { get; set; }
		public string P_BRANCH_NAME { get; set; }
		public string P_IFSC_CODE { get; set; }
		public string P_BANK_ACC_NO { get; set; }
		public string P_BANK_ACC_NAME { get; set; }
		public string P_VOTER_ID { get; set; }
		public string P_UID_NO { get; set; }
		public string P_MOBILE_NO { get; set; }
		public string P_USER_ID { get; set; }
		public string P_SACHIVALAYAM_ID { get; set; }
		public string P_STATUS { get; set; }
		public string P_UID_PATH { get; set; }
		public string P_PASSBOOK_PATH { get; set; }
		public string P_F1_PATH { get; set; }
		public string P_PENSION_NUMBER { get; set; }
		public string P_SMARTCARD_NUMBER { get; set; }
	}

	public class JobCardAPIModel : LogModel
	{
		public string DistrictId { get; set; }
		public string MandalId { get; set; }
		public string PanchayatId { get; set; }
		public string HabCode { get; set; }
		public string PCCInfoCode { get; set; }
		public string Category { get; set; }
		public string Applnnumber { get; set; }
		public string RegDate { get; set; }
		public string Cast { get; set; }
		public string Address { get; set; }
		public string AddressEngToTelugu { get; set; }
		public string RationCard { get; set; }
		public string LandOwner { get; set; }
		public string LandOwned { get; set; }
		public string BplNum { get; set; }
		public string KathaNum { get; set; }
		public string AssignedLandReforms { get; set; }
		public string IayBenf { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Headoffamily { get; set; }
		public string Relationwithhead { get; set; }
		public string Gender { get; set; }
		public string HIVeffected { get; set; }
		public string Age { get; set; }
		public string MemberofSHG { get; set; }
		public string SHG_Name { get; set; }
		public string SHG_ID { get; set; }
		public string MPHSS_ID { get; set; }
		public string TypeOfPayingAgency { get; set; }
		public string PayingAgencyName { get; set; }
		public string BranchName { get; set; }
		public string IFSCcode { get; set; }
		public string NameAsPerBank { get; set; }
		public string BankAccountNumber { get; set; }
		public string VoterId { get; set; }
		public string UID { get; set; }
		public string Mobilenum { get; set; }
		public string Postalaccnum { get; set; }
		public string Solid { get; set; }
		public string Remarks { get; set; }
		public string Transactionid { get; set; }
		public string Disabled { get; set; }
		public string BankName { get; set; }
		public string BankPassbookName { get; set; }
		public string SmartCardNo { get; set; }
		public string PensionNumber { get; set; }
		public string PMJobCardId { get; set; }
		public string IsBenfcielingLand { get; set; }
		public string CategoryCode { get; set; }
		public string MobNoRequired { get; set; }


	}
	public class TranslationModel : LogModel
	{
		public string translitaration { get; set; }
		public string itext { get; set; }
		public string locale { get; set; }
		public string transRev { get; set; }
	}
	public class Jobmultiplejobcard
	{
		public List<JobCardModel> multiplejobcard;
	}

}