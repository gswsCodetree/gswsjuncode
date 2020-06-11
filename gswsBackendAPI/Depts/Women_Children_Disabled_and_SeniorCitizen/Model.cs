using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Depts.Women_Children_Disabled_and_SeniorCitizen
{
	public class Model
	{
		public class GetRegData
		{
			public string name { set; get; }
			public string email { set; get; }
			public string password { set; get; }
			public string confirmpassword { set; get; }
			public string mobile { set; get; }
			public string aadhaar { set; get; }
			public string GSWS_ID { set; get; }
			public string gsws_user_email { set; get; }
			public string gsws_user_password { set; get; }
		}

		public class GetUserLoginToken
		{
			public string Email { set; get; }
			public string Pswd { set; get; }
		}

		public class PerDetails
		{
			public string token { get; set; }
			public string applicant { get; set; }
			public string father { get; set; }
			public string aadhar { get; set; }
			public string dob { get; set; }
			public string gender { get; set; }
			public string maritalstatus { get; set; }
			public string femaletype { get; set; }
			public string minority { get; set; }
			public string religion { get; set; }
			public string district { get; set; }
			public string mandal { get; set; }
			public string village { get; set; }
			public string othervillage { get; set; }
			public string habitation { get; set; }
			public string wardnumber { get; set; }
			public string housenumber { get; set; }
			public string pincode { get; set; }
			public string nativity { get; set; }

			public string sameaddress { get; set; }
			public string present_district { get; set; }
			public string present_mandal { get; set; }
			public string present_village { get; set; }
			public string present_othervillage { get; set; }
			public string present_habitation { get; set; }
			public string present_wardnumber { get; set; }
			public string present_housenumber { get; set; }
			public string present_pincode { get; set; }
			public string nativity_present { get; set; }

			public string annualincomeoffamily { get; set; }
			public string incomeissueddistrict { get; set; }
			public string dateofincome { get; set; }
			public string pensioner { get; set; }
			public string pensiontype { get; set; }
			public string pensionnumber { get; set; }
			public string pensionissueddistrict { get; set; }

			public string whiteration { get; set; }
			public string rationnumber { get; set; }
			public string rationdistrict { get; set; }
			public string licencetype { get; set; }
			public string drivinglicenceno { get; set; }
			public string licensevalidity { get; set; }
			public string llrno { get; set; }
			public string llrdate { get; set; }

			public string sportsman { get; set; }
			public string sportslevel { get; set; }
			public string sportsorg { get; set; }
			public string sportevent { get; set; }
			public string sportsdate { get; set; }
		}

		public class EduDetails
		{
			public string token { get; set; }
			public string literate { get; set; }
			public string employmenttype { get; set; }
			public string studying { get; set; }
			public string highesteducation { get; set; }
			public string selfemployment { get; set; }
			public string wageemployment { get; set; }

			public string unitname { get; set; }
			public string noofemployees { get; set; }
			public string annualturnover { get; set; }
			public string governmentsubsidy { get; set; }
			public string bankloan { get; set; }

			public string self_sameaddress { get; set; }
			public string self_district { get; set; }
			public string self_mandal { get; set; }
			public string self_village { get; set; }
			public string self_othervillage { get; set; }
			public string self_habitation { get; set; }
			public string self_wardnumber { get; set; }
			public string self_housenumber { get; set; }
			public string self_pincode { get; set; }

			public string currenteducation { get; set; }
			public string collegename { get; set; }
			public string admissionnumber { get; set; }
			public string coursename { get; set; }
			public string studyingprofessionalcourses { get; set; }
			public string university { get; set; }
			public string otheruniversity { get; set; }
			public string courseduration { get; set; }
			public string coursedurationto { get; set; }
			public string schoolname { get; set; }
			public string schooladmissionnumber { get; set; }

			public string college_district { get; set; }
			public string college_state { get; set; }
			public string college_state_district { get; set; }
			public string college_state_address { get; set; }
			public string college_mandal { get; set; }
			public string college_village { get; set; }
			public string college_othervillage { get; set; }
			public string college_habitation { get; set; }
			public string college_wardnumber { get; set; }
			public string college_housenumber { get; set; }
			public string college_pincode { get; set; }

			public string organizationname { get; set; }
			public string designation { get; set; }
			public string wage_district { get; set; }
			public string wage_mandal { get; set; }
			public string wage_village { get; set; }
			public string wage_othervillage { get; set; }
			public string wage_habitation { get; set; }
			public string wage_wardnumber { get; set; }
			public string wage_pincode { get; set; }
			public string wage_housenumber { get; set; }
		}

		public class DisableDetails
		{
			public string token { get; set; }
			public string natureofdisability { get; set; }
			public string natureofdisability_m { get; set; }
			public string percentageofdisability { get; set; }
			public string sadaremnumber { get; set; }
		}

		public class PrevRecList
		{
			public string token { get; set; }
			public List<string> List { get; set; }
		}

		public class UploadDocuments
		{
			public string token { get; set; }
			public string photo_aadhar { get; set; }
			public string photo_dob { get; set; }
			public string photo_disability { get; set; }
			public string photo_sadarem { get; set; }
			public string photo_income { get; set; }
			public string photo_caste { get; set; }
		}

		public class ApplyList
		{
			public string token { get; set; }
			public List<string> List { get; set; }
		}

		public class WCDWCLS
		{
			public string GSWS_ID { get; set; }
			public PerDetails PerDetails { get; set; }
			public EduDetails EduDetails { get; set; }
			public DisableDetails DisableDetails { get; set; }
			public PrevRecList PrevRecList { get; set; }
			public UploadDocuments UploadDocuments { get; set; }
			public ApplyList ApplyList { get; set; }
		}

		public class DeptResponse
		{
			public string status { get; set; }
			public string Response { get; set; }
			public string GSWSID { get; set; }
			public string APPLICATIONID { get; set; }

		}

		public class AppStatCls
		{
			public string input { get; set; }
		}
	}
}