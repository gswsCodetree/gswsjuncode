using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Depts.paymentChallan.BackEnd
{
    public class serviceRequestModel 
    {
        public string type { get; set; }
        public string districtId { get; set; }
        public string mandalId { get; set; }
        public string secId { get; set; }
        public string deptTxnId { get; set; }
        public string challanId { get; set; }
        public string challanDate { get; set; }
		public string ifsc_code { get; set; }
		public string transaction_status { get; set; }
		public string valid_upto { get; set; }
	}

    public class ChallanValue
    {
        public string DeptCode { get; set; }
        public string HOA { get; set; }
        public string DDOCode { get; set; }
        public string ServiceCode { get; set; }
        public string ChallanAmount { get; set; }
    }

    public class Header
    {
        public string DeptTransID { get; set; }
        public string RemitterName { get; set; }
        public string RemittersID { get; set; }
        public string TotalAmount { get; set; }
        public string MobileNumber { get; set; }
        public string EmailID { get; set; }
        public IList<ChallanValue> ChallanValues { get; set; }
    }

    public class cfmsReqModel
    {
        public Header Header { get; set; }
    }

	public class Response
	{
		public string CFMS_ID { get; set; }
		public string IFSC_Code { get; set; }
		public string Transaction_Status { get; set; }
		public string Valid_Upto { get; set; }
		public string DeptTransID { get; set; }
		public string TotalAmount { get; set; }
		public string Message { get; set; }
	}

	public class RootCfMSResponse
	{
		public Response Response { get; set; }
	}

}