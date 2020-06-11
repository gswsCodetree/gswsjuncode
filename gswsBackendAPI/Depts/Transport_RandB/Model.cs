using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Depts.Transport_RandB
{
	public class Model
	{
		public class Complaintdetails
		{
			public string Username { set; get; }
			public string Email { set; get; }
			public string MobileNumber { set; get; }
			public string PersonalAddress { set; get; }
			public string ComplaintAddress { set; get; }
			public string ComplaintTitle { set; get; }
			public string ComplaintDescription { set; get; }
			public string ReferenceId { set; get; }
			public string CreatedDate { set; get; }
			public string OtherCompRefId { set; get; }
			public string distname { set; get; }
			public string Status { set; get; }
			public string Category { set; get; }
		}

		public class RTAStatusCls
		{
			public string module { set; get; }
			public string applicationNo { set; get; }
			public string chassisNo { set; get; }
		}

		public class ApplicationStatus
		{
			public string applicationFormNo { get; set; }
			public string dob { get; set; }
		}
	}
}