using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Depts.Pension.Backend
{
    public class FamilyDetail
    {
        public string CITIZEN_NAME { get; set; }
        public string DOB_DT { get; set; }
        public string CITIZEN_AADHAAR { get; set; }
    }

    public class Details
    {
        public string CITIZEN_NAME { get; set; }
        public string DISTRTICT_CODE { get; set; }
        public string SACHIVALAYAM_NAME { get; set; }
        public string CLUSTER_ID { get; set; }
        public string VOLUNTEER_NAME { get; set; }
        public string caste { get; set; }
        public string HH_ID { get; set; }
        public string MANDAL_CODE { get; set; }
        public string DOB_DT { get; set; }
        public string SACHIVALAYAM_CODE { get; set; }
        public string MANDAL_NAME { get; set; }
        public string subCaste { get; set; }
        public string base64 { get; set; }
        public string CFMS { get; set; }
        public string CLUSTER_NAME { get; set; }
        public string GENDER { get; set; }
        public string VOLUNTEER_MOBILE { get; set; }
        public string DISTRICT_NAME { get; set; }
        public string VOLUNTEER_AADHAAR { get; set; }
        public string CITIZEN_AADHAAR { get; set; }
    }

    public class pensionResponseModel
    {
        public string responseCode { get; set; }
        public IList<FamilyDetail> familyDetails { get; set; }
        public Details details { get; set; }
        public string reason { get; set; }
    }

    public class pensionDeptModel
    {
        public string uid { get; set; }
        public string secId { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string pensionType { get; set; }
        public string sadaremId { get; set; }
    }

    public class PanchayatMastersModel
    {
        public string secretariatId { get; set; }
        public string flag { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
    }

    public class habitationMasterModel : PanchayatMastersModel
    {
        public string panchayatId { get; set; }
    }

    public class PanchayatList
    {
        public string PanchayatCode { get; set; }
        public string PanchayatName { get; set; }
    }

    public class panchayatResponseModel
    {
        public IList<PanchayatList> PanchayatList { get; set; }
    }

    public class HabitationList
    {
        public string HabitationCode { get; set; }
        public string HabitationName { get; set; }
    }

    public class habResponseModel
    {
        public IList<HabitationList> HabitationList { get; set; }
    }

    public class casteModel
    {
        public string type { get; set; }
        public string casteId { get; set; }
        public string subCasteId { get; set; }
    }

    public class tokenModel
    {
        public string username { get; set; }
        public string wdsLoginId { get; set; }
        public string password { get; set; }
    }

    public class tokenResponseModel
    {
        public string Token { get; set; }
    }

    public class pensionAppSubModel
    {
        public string userName { get; set; }
        public string password { get; set; }
        public string loginId { get; set; }
        public string token { get; set; }
        public string transactionId { get; set; }
        public string aadharNumber { get; set; }
        public string panchayatOrMcid { get; set; }
        public string habitationOrWardId { get; set; }
        public string address { get; set; }
        public string anotherAddress { get; set; }
        public string age { get; set; }
        public string dateOfBirth { get; set; }
        public string applicantContactNum { get; set; }
        public string familyIncomeforMonth { get; set; }
        public string familyDryLand { get; set; }
        public string familyWetLand { get; set; }
        public string fourWheelerFamilyYesOrNo { get; set; }
        public string govtEmployeeYesOrNo { get; set; }
        public string electricConsumptionUnits { get; set; }
        public string incomeTaxPayeeYesOrNo { get; set; }
        public string residentPropMuniInSqFt { get; set; }
        public string otherPensionInFamilyYesOrNo { get; set; }
        public string otherPensionFamilyDetails { get; set; }
        public string aadharFileName { get; set; }
        public string aadharProof { get; set; }
        public string schemeFileName { get; set; }
        public string schemeProof { get; set; }
        public string pensionType { get; set; }
        public string maritalStatus { get; set; }
        public string caste { get; set; }
        public string subCaste { get; set; }
        public string systemIp { get; set; }
        public string encrypted_data { get; set; }
        public string iv { get; set; }
        public string relationName { get; set; }
        public string relationType { get; set; }
        public string disabilityId { get; set; }
        public string fourWheelerFamilyDEtails { get; set; }
        public string govtEmployeeDetails { get; set; }
        public string incomeTaxDetails { get; set; }
		public string mpdoremarks { get; set; }
    }

    public class Response
    {
        public string status_Code { get; set; }
        public string remarks { get; set; }
        public string BenTransId { get; set; }
        public string TransactionId { get; set; }
    }

    public class subRespModel
    {
        public Response Response { get; set; }
    }

    public class grevianceListModeL
    {
        public string userName { get; set; }
        public string password { get; set; }
        public string secretariatId { get; set; }
    }

    public class Sectraite
    {
        public string hh_id { get; set; }
        public string uid_no { get; set; }
        public string sub_caste { get; set; }
        public string dob { get; set; }
        public string age { get; set; }
        public string caste { get; set; }
        public string Transactionid { get; set; }
        public string citizen_name { get; set; }
        public string grievance_id { get; set; }
        public string scheme { get; set; }
        public string entered_date { get; set; }
        public string verification_type { get; set; }
    }

    public class greRespModel
    {
        public string response { get; set; }
        public IList<Sectraite> sectraite { get; set; }
        public string sectraiteList { get; set; }
    }

    public class personGrevDetailsModel
    {
        public string grievanceId { get; set; }
        public string transid { get; set; }
        public string password { get; set; }
        public string userName { get; set; }
    }

    public class GrevFamilyDetail
    {
        public string dateOfBirth { get; set; }
        public string name { get; set; }
        public string UID_NO { get; set; }
    }

    public class GrevDetails
    {
        public string FAMILYDRYLAND { get; set; }
        public string SUB_CASTE { get; set; }
        public string Relation_name { get; set; }
        public string sadaremcode { get; set; }
        public string ELECTRICCONSUMPINUNITS { get; set; }
        public string SCHEMEPROOF { get; set; }
        public string aadhar { get; set; }
        public string AADHARPROOF { get; set; }
        public string SECRETARIAT_NAME { get; set; }
        public string ADDRESS2 { get; set; }
        public string RTGS_DRY_LAND { get; set; }
        public string Pensioner_name { get; set; }
        public string otherpensionsyesorno { get; set; }
        public string ADDRESS1 { get; set; }
        public string Panchayat { get; set; }
        public string CLUSTER_NAME { get; set; }
        public string FAMILYWETLAND { get; set; }
        public string age { get; set; }
        public string VOLUNTEER_MOBILE { get; set; }
        public string Data_of_birth { get; set; }
        public string RTGS_LAST6MONTHS_ELECTRICITY { get; set; }
        public string GOVTEMPLOYEEDETAILS { get; set; }
        public string Habitation { get; set; }
        public string govtyesorno { get; set; }
        public string Mandal { get; set; }
        public string INCOMETAXPAYEEYESORNO { get; set; }
        public string OTHERPENSIONINFAMILYDETAILS { get; set; }
        public string PROPERTY_TAX { get; set; }
        public string CLUSTER_ID { get; set; }
        public string RTGS_WET_LAND { get; set; }
        public string CASTE { get; set; }
        public string VOLUNTEER_NAME { get; set; }
        public string fourWhelleryesorno { get; set; }
        public string RTGS_PROPERTY_TAX { get; set; }
        public string RTGS_INCOMETAX_YESORNO { get; set; }
        public string FAMILYINCOMEPERMONTH { get; set; }
        public string FOURWHEELERFAMILYDETAILS { get; set; }
        public string category { get; set; }
        public string RTGS_GOV_EMP_STATUS { get; set; }
        public string District { get; set; }
        public string MOBILE_NO { get; set; }
        public string GENDER { get; set; }
        public string RTGS_INCOME_TAX { get; set; }
        public string VOLUNTEER_ID { get; set; }
        public string INCOMETAXDETAILS { get; set; }
        public string RTGS_INCOME { get; set; }
        public string RTGS_FOUR_WHEELER { get; set; }
        public string SECRETERIAT_CODE { get; set; }
    }

    public class grevDetailsRespModel
    {
        public string responseCode { get; set; }
        public IList<GrevFamilyDetail> familyDetails { get; set; }
        public GrevDetails details { get; set; }
        public string reason { get; set; }
    }

    public class subWEADataModel
    {
        public string userName { get; set; }
        public string password { get; set; }
        public string loginId { get; set; }
        public string aadharNumber { get; set; }
        public string familyIncomeforMonth { get; set; }
        public string familyDryLand { get; set; }
        public string familyWetLand { get; set; }
        public string fourWheelerFamilyYesOrNo { get; set; }
        public string fourWheelerFamilyDEtails { get; set; }
        public string govtEmployeeYesOrNo { get; set; }
        public string govtEmployeeDetails { get; set; }
        public string electricConsumptionUnits { get; set; }
        public string incomeTaxPayeeYesOrNo { get; set; }
        public string incomeTaxDetails { get; set; }
        public string residentPropMuniInSqFt { get; set; }
        public string otherPensionInFamilyYesOrNo { get; set; }
        public string otherPensionFamilyDetails { get; set; }
        public string aadharFileName { get; set; }
        public string aadharProof { get; set; }
        public string schemeFileName { get; set; }
        public string schemeProof { get; set; }
        public string systemIp { get; set; }
        public string grievanceId { get; set; }
        public string remarks { get; set; }
    }


    public class subWEAModel
    {
        public subWEADataModel data { get; set; }
        public string txnId { get; set; }
    }

    public class subWEARespModel
    {
        public WEASubDataRespModel Response { get; set; }
    }

    public class WEASubDataRespModel
    {
        public string Remarks { get; set; }
        public string status_Code { get; set; }

    }

}