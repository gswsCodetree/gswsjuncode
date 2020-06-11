using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Depts.REVENUE.Backend
{

	#region REVENUE
	public class RevenueModel
	{
		public string CRI { get; set; }
		public string INPUT { get; set; }
		public string UID { get; set; }
		public string RID { get; set; }
        public string RequestType { get; set; }
    }
	public class Seccmastermodel
	{
		public string Ftype { get; set; }
		public string Fdistrict { get; set; }
		public string Fmandal { get; set; }
		public string Fvillage { get; set; }
		public string Fruflag { get; set; }
	}

	public class RootSpandaObject
	{
		public string AadhaarNumber { get; set; }
		public string HouseHoldId { get; set; }
		public string ApplicantName { get; set; }
		public string CareOf { get; set; }
		public string Age { get; set; }
		public string Gender { get; set; }
		public string PinCode { get; set; }
		public string Mobile { get; set; }
		public string Email { get; set; }
		public string Income { get; set; }
		public string Occupation { get; set; }
		public string AppTypeInfo { get; set; }
		public string AppCreator { get; set; }
		public string MkmDistCode { get; set; }
		public string MkmMandalCode { get; set; }
		public string MkmVillageCode { get; set; }
		public string PssDistCode { get; set; }
		public string PssMandalCode { get; set; }
		public string PssVillageCode { get; set; }
		public string HodId { get; set; }
		public string FormID { get; set; }
		public string ProblemDetails { get; set; }
		public string OtherDetails { get; set; }
		public string Habitation { get; set; }
		public string PresentAddress { get; set; }
		public string token { get; set; }
		public string Loginuser { get; set; }
	}
	#endregion

	#region REVENUE - Excise
	public class ExciseModel
	{
		public string applicantAadhaar { get; set; }
		public string applicantMobileNumber { get; set; }
		public string applicantName { get; set; }
		public string complaintDetails { get; set; }
		public string districtCode { get; set; }

		public string mandalCode { get; set; }
		public string transactionID { get; set; }
		public string villageCode { get; set; }
		public string GSWS_ID { get; set; }
		
		public List<Uploaddoc> UploadDoc { set; get; }
	}

	public class Uploaddoc
	{
		public string comments { set; get; }
		public string fileName { set; get; }
		public string image { set; get; }
	}
	#endregion
}