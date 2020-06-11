using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Depts.Minority
{
    public class MinorityModel
    {
        public string Status { get; set; }
        public string REASON { get; set; }
        public dynamic data { get; set; }
    }
    public class WomenDiverced
    {
        public string Petitioner { get; set; }
        public string MCNo { get; set; }
        public string OrderDate { get; set; }
        public string MaintancePerMonth { get; set; }
        public string TotalAmountToBeReleased { get; set; }
        public string CurrentAmountReleased { get; set; }
        public string Aadhaar { get; set; }
        public string ContactNumber { get; set; }
    }
    public class IMAMANDMOUZANS
    {
        public string BENEFICIARYCODE { get; set; }
        public string INSTITUTIONNAME { get; set; }
        public string PHASE { get; set; }
        public string DISTRICT { get; set; }
        public string VILLAGE { get; set; }
        public string MANDAL { get; set; }
        public string CONSTITUENCY { get; set; }
        public string PINCODE { get; set; }
        public string INSTITUTIONNAMEASPERPASSBOOK { get; set; }
        public string ACCOUNTNUMBER { get; set; }
        public string BANKNAME { get; set; }
        public string BRANCHNAME { get; set; }
        public string IFSCCODE { get; set; }
        public string IMAMNAMEASPERPASSBOOK { get; set; }
        public string AADHARNUMBER { get; set; }
        public string CONTACTNUMBER { get; set; }
        public string IMAMACCOUNTNUMBER { get; set; }
        public string IMAMBANKNAME { get; set; }
        public string IMAMBRANCHNAME { get; set; }
        public string IMAMIFSCCODE { get; set; }
        public string MOUZANNAMEPASSBOOK { get; set; }
        public string MOUZANAADHAARNUMBER { get; set; }
        public string MOUZANCONTACTNUMBER { get; set; }
        public string MOUZANACCOUNTNUMBER { get; set; }
        public string MOUZANBANKNAME { get; set; }
        public string MOUZANBRANCHNAME { get; set; }
        public string MOUZANIFSCCODE { get; set; }
        public string PRESIDENTMUTHAWALI { get; set; }
        public string PRESIDENTMUTHAWALIAADHARNUMBER { get; set; }
        public string PRESIDENTMUTHAWALICONTACTNUMBER { get; set; }
        public string CHEQUENO { get; set; }
        public string CHEQUEDATE { get; set; }
        public string CHEQUEPERIOD { get; set; }
        public string CHEQUEAMOUNT { get; set; }
        public string DISBURSEDPERIOD { get; set; }
        public string DISBURSEDAMOUNT { get; set; }
    }
}