using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Depts.CommercialTax
{
    public class CommercialTaxModel
    {

    }

    public class DocCls
    {
        public string action { get; set; }
        public string data { get; set; }
        public string dty { get; set; }
        public string cty { get; set; }
        public string apty { get; set; }
        public string tin { get; set; }
    }


    public class SubCls
    {
        public string action { get; set; }
        public string data { get; set; }
    }


    public class Enterprisedetails
    {
        public string divisioncode { get; set; }
        public string circlecode { get; set; }
        public string tin_grn { get; set; }
        public string enterprise_name { get; set; }
        public string pan_number { get; set; }
        public string email_id { get; set; }
        public string phone_number { get; set; }
        public string mobile_number { get; set; }
        public string door_no { get; set; }
        public string street { get; set; }
        public string location { get; set; }
        public string mandal_code { get; set; }
        public string district_name { get; set; }
        public string state_name { get; set; }
        public string pin { get; set; }
    }

    public class Ownerdetails
    {
        public string serial_no { get; set; }
        public string partner_type { get; set; }
        public string name { get; set; }
        public string father_name { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string email_id { get; set; }
        public string pan_number { get; set; }
        public string uid { get; set; }
        public string phone_number { get; set; }
        public string mobile_number { get; set; }
        public string door_no { get; set; }
        public string street { get; set; }
        public string location { get; set; }
        public string city { get; set; }
        public string country_code { get; set; }
        public string district_name { get; set; }
        public string state_name { get; set; }
        public string pin { get; set; }
    }

    public class Bankdetail
    {
        public string bankname { get; set; }
        public string ifsccode { get; set; }
        public string BankBranchCode { get; set; }
        public string accountnumber { get; set; }
    }

    public class Partnerdetail
    {
        public string serial_no { get; set; }
        public string partner_type { get; set; }
        public string name { get; set; }
        public string father_name { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string email_id { get; set; }
        public string pan_number { get; set; }
        public string uid { get; set; }
        public string phone_number { get; set; }
        public string mobile_number { get; set; }
        public string door_no { get; set; }
        public string street { get; set; }
        public string location { get; set; }
        public string city { get; set; }
        public string country_code { get; set; }
        public string district_name { get; set; }
        public string state_name { get; set; }
        public string pin { get; set; }
    }

    public class PTDetails
    {
        public Enterprisedetails enterprisedetails { get; set; }
        public Ownerdetails ownerdetails { get; set; }
        public object autheriseddetails { get; set; }
        public List<Bankdetail> bankdetails { get; set; }
        public List<Partnerdetail> partnerdetails { get; set; }
    }

    public class PTStatus
    {
        public string action { get; set; }
        public string gen_date { get; set; }
        public string to_date { get; set; }
        public string rnr { get; set; }
        
    }

}