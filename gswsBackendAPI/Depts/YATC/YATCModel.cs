using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Depts.YATC
{
    public class YATCModel
    {
    }

    public class BatchesCls
    {
        public string appKey { get; set; }
        public string tcId { get; set; }
        public string programId { get; set; }
        public string applicationType { get; set; }
        public string batchId { get; set; }
        public string userMasterId { get; set; }
    }

    public class usercred
    {
        public string username { get; set; }
        public string password { get; set; }
        public string type { get; set; }
    }

    public class Hearders
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class inputParams
    {
        public string key { get; set; }
        public string ref_id { get; set; }
        public string userMasterId { get; set; }
        public string JobId { get; set; }
        public string appKey { get; set; }
        public string district { get; set; }
        public string batchId { get; set; }
    }

    public class UserMaster
    {
        public string contact { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string confirmpassword { get; set; }
        public string username { get; set; }
    }

    public class CanReg
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string gender { get; set; }
        public string casteId { get; set; }
        public string religion { get; set; }
        public string dob { get; set; }
        public string fatherName { get; set; }
        public string maritalstatus { get; set; }
        public string bloodGrop { get; set; }
        public string registrationId { get; set; }
        public string addressline { get; set; }
        public string pincode { get; set; }
        public string mndlMunc { get; set; }
        public string village { get; set; }
        public string panchayat { get; set; }
        public string isDomicile { get; set; }
        public string aadharNumber { get; set; }
        public string isRural { get; set; }
        public string state { get; set; }
        public string district { get; set; }
        public string ward { get; set; }
        public string street { get; set; }
        public string isUnEmployed { get; set; }
        public string alternatecontact { get; set; }
        public string constititionId { get; set; }
        public string fmlyAnnualIncome { get; set; }
        public string physicallyChallenged { get; set; }
		public string gsws_id { get; set; }
		public UserMaster userMaster { get; set; }
    }

    public class JobIds
    {
        public List<string> data { get; set; }
    }

    public class JobsCls
    {
        public string appKey { get; set; }
        public string userMasterId { get; set; }
        public JobIds JobIds { get; set; }
		public string GSWS_ID { get; set; }
	}
}