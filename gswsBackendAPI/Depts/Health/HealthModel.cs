using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Depts.Health
{
    public class HealthModel
    {
    }

    public class AppStatus
    {
        public string ftype { get; set; }
        public string fdpart_id { get; set; }
        public string fadhar_no { get; set; }
		public string student_id { get; set; }

	}
	public class DEVICEDATA
	{
		public string Sadaremcode { get; set; }
		public string PERSONNAME { get; set; }
		public string gender { get; set; }
		public string date_of_birth { get; set; }
		public string PensionID { get; set; }
		public string rationcard_number { get; set; }
		public string AadharNumber { get; set; }
		public string Address { get; set; }
		public string TYPEOFDISABILITY { get; set; }
		public string percentage { get; set; }
		public string DateOfIssue { get; set; }
		public string reasonforpersonstatus { get; set; }
		public string ARstatus { get; set; }
		public string CertHashMessage { get; set; }
		public string IDCardHashMessage { get; set; }
	}

	public class List
	{
		public DEVICEDATA DEVICEDATA { get; set; }
	}

	public class SadaramResponse
	{
		public List list { get; set; }
	}
}