using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Depts.Industries
{
    public class IndustriesModel
    {
    }

    public class NavodayamOTR
    {
        public string Unit_Name { get; set; }
        public string Unit_District_ID { get; set; }
        public string Unit_Address { get; set; }
        public string Mobile_No { get; set; }
        public string Plant_Machinery_Investment { get; set; }
        public string Other_Investment { get; set; }
        public string DOCP { get; set; }
        public string Total_Employment { get; set; }
        public string Unit_Category { get; set; }
        public string Caste { get; set; }
        public string Sector { get; set; }
        public string LOA { get; set; }
        public string UAM_No { get; set; }
        public string Mandal_ID { get; set; }
        public string EMail_ID { get; set; }
        public string Government_Scheme { get; set; }
        public string Monthly_Income { get; set; }
        public string IFSC_Code { get; set; }
        public string Bank_Name { get; set; }
        public string Branch_Name { get; set; }
        public string Bank_District_ID { get; set; }
        public string Loan_Account { get; set; }
        public string Loan_Amount { get; set; }
        public string Loan_Date { get; set; }
        public string Loan_Type { get; set; }
        public string PAN_No { get; set; }
        public string UAM_Enclosure { get; set; }
        public string Other_Enclosure { get; set; }
    }

    public class Hearders
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class usercred
    {
        public string username { get; set; }
        public string password { get; set; }
        public string type { get; set; }
    }
}