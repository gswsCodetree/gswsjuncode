using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Payments.Backend
{
    public class walletOnePaymentModel
    {
        public string district_code { get; set; }
        public string gsws_code { get; set; }
        public string arg_type { get; set; }
        public string center_type { get; set; }
        public string applicant_name { get; set; }
        public string applicant_no { get; set; }
        public string application_date { get; set; }
        public string application_dept_no { get; set; }
        public string department_application_date { get; set; }
        public string application_receipt_no { get; set; }
        public string sachivalayam_distric_code { get; set; }
        public string status { get; set; }
        public string pay_mode { get; set; }
        public string service_code { get; set; }
        public string staff_code { get; set; }
        public string bill_amount { get; set; }
        public string transaction_date { get; set; }
        public string transaction_state { get; set; }
        public string user_charges { get; set; }
        public string user_id { get; set; }
    }
    public class MakePaymentModel
    {
        public string encryptedData { get; set; }
        public string iv { get; set; }
        public string type { get; set; }
        public string otp { get; set; }
    }

    public class paymentModel : orderDetailsModel
    {
        public string ipAddress { get; set; }
        public string gswsCode { get; set; }
        public string applicantName { get; set; }
        public string deptCode { get; set; }
        public string deptRecieptCode { get; set; }
        public string districtCode { get; set; }
        public string serviceCode { get; set; }
        public string staffCode { get; set; }
        public string callbackUrl { get; set; }
        public string merchantId { get; set; }
        public string UniqueTxnId { get; set; }
    }

    public class meesevaResponse
    {
        public string deptOrderId { get; set; }
        public string txnId { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public string merchantId { get; set; }
    }


    public class taWalletPaymentResponse
    {
        public string Response_Code { get; set; }
        public string Message { get; set; }
        public string Balance { get; set; }
        public string Trxn_Ref_No { get; set; }
        public string Merchant_Name { get; set; }
        public string OrderID { get; set; }
    }

    public class gswsPaymentResponseModel
    {

        public string type { get; set; }
        public string balance { get; set; }
        public string inserted_on { get; set; }
        public string merchant_name { get; set; }
        public string message { get; set; }
        public string orderId { get; set; }
        public string responseCode { get; set; }
        public string txnRefNo { get; set; }
        public string uniqueTxnId { get; set; }
        public string status { get; set; }
    }
    public class orderDetailsModel
    {
        public string iv { get; set; }
        public string encrypttext { get; set; }
        public string userName { get; set; }
        public string mobileNumber { get; set; }
        public string orderId { get; set; }
        public string TxnDate { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string userCharges { get; set; }
        public string totalAmount { get; set; }
        public string walletType { get; set; }

    }

    public class TAWalletpaymentModel
    {
        public string order_id { get; set; }
        public string amount { get; set; }
        public string callback_url { get; set; }
        public string service_name { get; set; }
        public string transaction_desc { get; set; }
        public string user_id { get; set; }
        public string encrypted_data { get; set; }
    }

    public class TAWalletResponseModel
    {
        public string OrderId { get; set; }
        public string Amount { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Statusdesc { get; set; }
        public string RefUniqueid { get; set; }
    }
}