using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Depts.Housing
{
    public class HousingModel : HousingSPHelper
    {
        public class AppSta
        {
            public string ref_no { get; set; }
            public string value { get; set; }
        }

		public class ApplicationSta
		{
			public string SearchType { get; set; }
			public string AppNo { get; set; }
			public string FromDate { get; set; }
			public string Todate { get; set; }
		}

		public class HousePattaCls
		{
			public string VolunteerId { get; set; }
			public string VName { get; set; }
			public long VMobile { get; set; }
			public long VAadhaar { get; set; }
			public string SachivalayamCodeno { get; set; }
			public string SachivalayamName { get; set; }
			public long BAdhaarno { get; set; }
			public string BenficiaryName { get; set; }
			public string Appno { get; set; }
			public long Bmobile { get; set; }
			public string RelationID { get; set; }
			public string RelationName { get; set; }
			public int Age { get; set; }
			public char Gender { get; set; }
			public string Religion { get; set; }
			public int isPhysChall { get; set; }
			public string CasteID { get; set; }
			public string SubCasteID { get; set; }
			public string Occupation { get; set; }
			public string OtherOccupation { get; set; }
			public string Houseno { get; set; }
			public string Street { get; set; }
			public int DistrictID { get; set; }
			public int MandalID { get; set; }
			public int VillageID { get; set; }
			public int PanchayathID { get; set; }
			public int Pincode { get; set; }
			public int isRation { get; set; }
			public string Rationcardno { get; set; }
			public int isHouse { get; set; }
			public int isHouseSite { get; set; }
			public int isHousingScheme { get; set; }
			public int isHouseSiteScheme { get; set; }
			public int isLand { get; set; }
			public int isIncome { get; set; }
			public int isPMAY { get; set; }
			public int PMAYBenefitSchemeID { get; set; }
			public int isAHPAllotmentReceived { get; set; }
			public string AHPAllotmentDetails { get; set; }
			public string USERID { get; set; }
			public string file { get; set; }
			public string SystemIP { get; set; }
			public string MandaltypeID { get; set; }
		}
	}
}