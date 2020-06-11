using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gswsBackendAPI.DL.CommonHel;
namespace gswsBackendAPI.Depts.AgriCulture
{
    public class DemoModel
    {
        public string Ftype { get; set; }
		public string FDistrict { get; set; }
		public string FMandal { get; set; }
		public string FVillage { get; set; }
		public string FKathano { get; set; }
		public string FUID { get; set; }
	}

	public class EPantaCls
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public string District { get; set; }
		public string Mandal { get; set; }
		public string Village { get; set; }
		public string SurveyNo { get; set; }
		public string Aadhar { get; set; }
	}

	public class FMAppSts:APITRACKMODEL
	{
		public string Application { get; set; }
	}

	public class FMReg
	{
		public string Key { get; set; }
		public string StateCode { get; set; }
		public string DistrictCode { get; set; }
		public string BlockCode { get; set; }
		public string SubDistrictCode { get; set; }
		public string PanchayatCode { get; set; }
		public string VillageCode { get; set; }
		public string AadharNo { get; set; }
		public string MobileNo { get; set; }
		public string FarmerName { get; set; }
		public string FatherHusbandName { get; set; }
		public string DOB { get; set; }
		public string Gender { get; set; }
		public string CasteCategory { get; set; }
		public string FarmerType { get; set; }
		public string Phone { get; set; }
		public string EmailId { get; set; }
		public string PinCode { get; set; }
		public string Address { get; set; }
		public string UserID { get; set; }
		public string Password { get; set; }
		public bool AadharConcent { get; set; }
		public string PAN { get; set; }
		public string CentralUnique_BenID { get; set; }
	}

}
