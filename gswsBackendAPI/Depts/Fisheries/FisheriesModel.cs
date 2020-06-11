using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Depts.Fisheries
{
    public class FisheriesModel
    {

        public class UserCred
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        public class AppSta
        {
            public string rtype { get; set; }
            public string ref_no { get; set; }
        }

        public class AnimalCls
        {
            public string UniqueNo { get; set; }
            public string Type { get; set; }
        }

		public class AppCls
		{
			public string transactionId { get; set; }
		}
	}
}