using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Depts.SocialWelfare_Tribal
{
	public class Models
	{
	}
	public class MrgCert
	{
		public string Aadhaar { set; get; }
	}

	//Aadhaar: scope.txtcertid, Acadamicyear: scope.ddlacdyear, Scheme: type
	public class S_W_Education
	{
		public string Aadhaar { set; get; }
		public string Acadamicyear { set; get; }
		public string Scheme { set; get; }
	}
}