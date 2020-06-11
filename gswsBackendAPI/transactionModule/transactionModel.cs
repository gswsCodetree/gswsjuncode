using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gswsBackendAPI.transactionModule
{
    public class transactionModel:spandanaTransactionModel
    {
        public string TYPE { get; set; }
        public string TXN_ID { get; set; }
        public string DEPT_ID { get; set; }
        public string HOD_ID { get; set; }
        public string SERVICE_NAME { get; set; }
        public string SERVICE_ID { get; set; }
        public string SERVICE_CODE { get; set; }
        public string IP_ADDRESS { get; set; }
        public string SYS_NAME { get; set; }
        public string GSWS_ID { get; set; }
        public string DEPT_TXN_ID { get; set; }
        public string BEN_ID { get; set; }
        public string STATUS_CODE { get; set; }
        public string STATUS_MESSAGE { get; set; }
        public string REMARKS { get; set; }
        public string DISTRICT_ID { get; set; }
        public string MANDAL_ID { get; set; }
        public string GP_WARD_ID { get; set; }
        public string LOGIN_USER { get; set; }
        public string TYPE_OF_REQUEST { get; set; }
        public string URL_ID { get; set; }
        public string SECRETRAINT_CODE { get; set; }
		public string DESIGNATION_ID { get; set; }
		public string CITIZENNAME { get; set; }
		public string MOBILENUMBER { get; set; }
		public string TYPE_OF_SERVICE { get; set; }

	}

	public class spandanaTransactionModel
	{
		public string Sdistcode { get;set;}
		public string Smcode { get; set; }
		public string Svtcode { get; set; }
		public string SRuflag { get; set; }
		public string UID { get; set; }
	}
    public class userAuthenticationModel
    {
        public string PS_ID { get; set; }
        public string EMP_ID { get; set; }
        public string MOBILE_NUMBER { get; set; }
        public string OTP { get; set; }
        public string TYPE { get; set; }
        public string ENCRYPTED_DATA { get; set; }
        public string IV { get; set; }
        public string KEY { get; set; }
    }

	public class RootMeesevaObject
	{
		public string PAYMENTTRANSID { get; set; }
		public string UNIQUENO { get; set; }
		public string SCAUSERID { get; set; }
		public string PAYMENTMODE { get; set; }
		public string OPERATORID { get; set; }
		public string CHANNELID { get; set; }
		public string MeeSevaAppNo { get; set; }
		public string REQUESTID { get; set; }
		public string SERVICEID { get; set; }
		public string ARRAMOUNT { get; set; }
		public string TRANSACTIONPARAMSDESC { get; set; }
		public string TRANSACTIONDETAILS { get; set; }
		public object CARDTRANSID { get; set; }
		public object CARDAUTHCODE { get; set; }
	}

	public class RootMeesevaResponseObject
	{
		public string TRANSID { get; set; }
		public string MEESEVAAPPLNO { get; set; }
		public string REQUESTID { get; set; }
		public string RECEIPTPARAMDESC { get; set; }
		public string RECEIPTDETAILS { get; set; }
		public string ERRORNO { get; set; }
		public string ERRORMESSAGE { get; set; }
	}

	public class ScheduleTransactionModel
	{
		public string TYPE { get; set; }

		public string GSWS_TRANS_ID { get; set; }

		public string DEPARTMENT_APPLICATION_ID { get; set; }

		public string DEPARTMENT_Transaction_ID { get; set; }

		public string SERVICE_NAME { get; set; }

		public string STATUS_CODE { get; set; }

		public string STATUS_MESSAGE { get; set; }

		public string CITIZEN_NAME { get; set; }

		public string GENDER { get; set; }

		public string DISTRICT { get; set; }

		public string MANDAL { get; set; }

		public string VILLAGE { get; set; }

		public string APPLICATION_LAST_UPDATE_DATE { get; set; }

	}

}