using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Internal.Backend
{

	#region issuetracking
	public class issueTrackingModel
	{
		public string TYPE { get; set; }
		public string SECRETARIAT_ID { set; get; }
		public string DISTRICT_ID { get; set; }
		public string MANDAL_ID { get; set; }
		public string USER { get; set; }
		public string REPORT_ID { get; set; }
	}

	public class commentAddition
	{
		public string comment { get; set; }
		public string issue_status { get; set; }
		public string image { get; set; }
		public string report_id { get; set; }
		public string username { get; set; }
		public bool message_flag { get; set; }
		public string image_path { get; set; }
		public string comment_id { get; set; }
	}

	#endregion


	public class InternalModel
	{
	}
	#region Internal URL
	public class InternalURL
	{
		public string TYPE { get; set; }
		public string DEPARTMENT { get; set; }
		public string HOD { get; set; }
		public string SERVICE { get; set; }
		public string REQUESTTYPE { get; set; }
		public string URL_ID { get; set; }
		public string URL { get; set; }
		public string URLDESCRIPTION { get; set; }
		public string USERNAME { get; set; }
		public string PASSWORD { get; set; }
		public string ENCRYPT_PASSWORD { get; set; }
		public string SERVICETYPE { get; set; }
		public string ACCESSLEVEL { get; set; }
		public string DISTRICT { get; set; }
		public string MANDAL { get; set; }
		public string PANCHAYAT { get; set; }
		public string RUFLAG { get; set; }
		public string P_URL_DESC_TEL { get; set; }
		public string P_DESIGN_R { get; set; }
		public string P_DESIGN_U { get; set; }
		public string RURALDESIGNATION { get; set; }
		public string URBANDESIGNATION { get; set; }

	}

	public class UpdateUserManualUrlModel
	{
		public string DEPTID { get; set; }
		public string HODID { get; set; }
		public string SERVICEID { get; set; }
		public string URL_ID { get; set; }
		public string USERMAUALID { get; set; }
		public string USER { get; set; }
		public string NEWURL { get; set; }

		public string TELUGUDESCRIPTION { get; set; }
		public string ENGLISHDESCRIPTION { get; set; }
	}

	public class SecretraintModel
	{
		public string DISTRICTID { get; set; }
		public string DISTRICTNAME { get; set; }
		public string MANDALID { get; set; }
		public string MANDALNAME { get; set; }
		public string PANCHAYATID { get; set; }
		public string PANCHAYATNAME { get; set; }
		public string RUFLAG { get; set; }
		public string SECRETRIATNAME { get; set; }
		public string SECRETRIATID { get; set; }
	}

	public class FeedBackReport : UpdateUserManualUrlModel
	{
		public string DISTRICTID { get; set; }
		public string MANDALID { get; set; }
		public string SECRETRIATID { get; set; }
		public string REPORTID { get; set; }
		public string REMARKS { get; set; }
		public string IMAGE1URL { get; set; }
		public string IMAGE2URL { get; set; }
		public string IMAGE3URL { get; set; }
		public string SUBJECT { get; set; }
		public string SUBSUBJECT { get; set; }
		public string SOURCE { get; set; }
	}
	#endregion

	public class IssueType
	{
		public int Type { get; set; }
		public int DID { get; set; }
		public string CategoryID { get; set; }
		public string UpdatedBy { get; set; }
		public string Reason { get; set; }
		public string UniqueID { get; set; }
		public string ROLE { get; set; }
		public string ASSET { get; set; }


		public string DISTRICTID { get; set; }
		public string MANDALID { get; set; }
		public string SECRETARIAT { get; set; }
		public string ASSETID { get; set; }
		public string ACTIVE_STATUS { get; set; }
	}
}