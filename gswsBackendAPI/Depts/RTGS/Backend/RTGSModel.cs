using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Dept.RTGS.Backend
{
	public class RTGSModel
	{
	}

	#region PSS
	public class PSSModel
	{
		public string CRI { get; set; }
		public string INPUT { get; set; }
	}

	public class UnSurveyRequestmodel
	{
		public string UID { get; set; }
		public string MOBILE_NUMBER { get; set; }
		public string DISTRICT_NAME { get; set; }
		public string DISTRICT_ID { get; set; }
		public string MANDAL_NAME { get; set; }
		public string MANDAL_ID { get; set; }
		public string VT_NAME { get; set; }
		public string VT_ID { get; set; }
		public string RURAL_URBAN_FLAG { get; set; }
		public string REQUEST_TYPE { get; set; }
		public string EMAIL { get; set; }
		public string UNSURVEYED_MEMBER_COUNT { get; set; }
		public string NETWORK_STATUS { get; set; }
	}
	#endregion
}