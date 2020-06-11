using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Depts.PRRD
{
    public class PRRDModel
    {
    }

	public class Housedata
	{
		public string assessment_no { get; set; }
		public string unique { get; set; }
		public string citizenName { get; set; }
		public string citizenFatherName { get; set; }
		public string citizenAadhar { get; set; }
		public List<DemandData> demandData { get; set; }
		public string tradeName { get; set; }

		public string tradeType { get; set; }
	}
	public class DemandData
	{
		public string uniqueID { get; set; }
		public string pay_amount { get; set; }
		public string due_year { get; set; }
	}
	public class TransactionsData
    {
        public int status { get; set; }
        public string msg { get; set; }
        public string panchayatName { get; set; }
        public string mandalName { get; set; }
        public string districtName { get; set; }
        public string searchBy { get; set; }
        public string panchayatId { get; set; }
        public List<Housedata> housedata { get; set; }
        public string eLTransactionId { get; set; }
    }
    public class PRRDResponse
    {
        public string Status { get; set; }
        public string REASON { get; set; }
        public dynamic data { get; set; }
    }

    public class TransactionResponse
    {
        public string status { get; set; }
        public string msg { get; set; }
        public string panchayatName { get; set; }
        public string mandalName { get; set; }
        public string districtName { get; set; }
        public string panchayatId { get; set; }
		public string transactionNo { get; set; }

	}
    public class PaymentDetail
    {
        public string hhCode { get; set; }
        public string name { get; set; }
        public string workName { get; set; }
        public string daysWorked { get; set; }
        public string musterRollsId { get; set; }
        public string toDate { get; set; }
        public string payOrderNo { get; set; }
        public string epayOrderNo { get; set; }
        public string sentDate { get; set; }
        public string ftoDepostedDate { get; set; }
        public string disbursedTimeandDate { get; set; }
        public string disbursedUploadedDate { get; set; }
        public string payOrderAmount { get; set; }
        public string disbursedAmount { get; set; }
        public string outStanding { get; set; }
        public string modeOfPayment { get; set; }
    }

    public class PaymentDetailList
    {
        public List<PaymentDetail> paymentDetails { get; set; }
    }
    public class UidDetail
    {
        public string hhCode { get; set; }
        public string workerCode { get; set; }
        public string bnfName { get; set; }
        public string epayOrderNo { get; set; }
        public string amount { get; set; }
        public string uidNo { get; set; }
        public string nregaAccNo { get; set; }
        public string fileSentDate { get; set; }
        public string creditStatus { get; set; }
        public string creditedAccNo { get; set; }
        public string bankName { get; set; }
        public string cbankIINNo { get; set; }
        public string cbankUTRNo { get; set; }
        public string remarks { get; set; }
    }

    public class UIDPaymentDetailList
    {
        public List<UidDetail> uidDetails { get; set; }
    }

    public class WorkProjectMaster
    {
        public string WorkTypeCode { get; set; }
        public string WorkName { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
    public class DemandCaptureResponse
    {
        public string GroupName { get; set; }
        public string DemandFromDate { get; set; }
        public string DemandToDate { get; set; }
        public string NoOfDemandedDays { get; set; }
        public string SequenceID { get; set; }
        public string Remarks { get; set; }
    }
    public class FarmerData
    {
        public string FarmerId { get; set; }
        public string UID { get; set; }
        public string FarmerName { get; set; }
        public string SurveyNo { get; set; }
        public string Extent { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string WorkTypeCode { get; set; }
        public string WorkName { get; set; }
        public string GSUniqueId { get; set; }
        public string Remarks { get; set; }
        public string IsActive { get; set; }
    }
    public class FarmerDataRequest
    {
        public List<FarmerData> Item;
    }
    public class ConfirmDemand
    {
        public string Remarks { get; set; }
    }
	public class BuildingAppStatus
	{
		public int api_status { get; set; }
		public string api_reason { get; set; }
		public string applicationSubmittedDate { get; set; }
		public string applicationStage { get; set; }
		public string applicationStatus { get; set; }
		public string paymentStatus { get; set; }
		public string downloadPlan { get; set; }
		public string downloadProceedings { get; set; }
		public string result { get; set; }
		public string error { get; set; }

	}

	//Layout Plan Application Data
	public class PersonalData
	{
		public string id { get; set; }
		public string surname { get; set; }
		public string name { get; set; }
		public string relation { get; set; }
		public string surname1 { get; set; }
		public string name1 { get; set; }
		public string aadhar { get; set; }
		public string email { get; set; }
		public string mobile { get; set; }
		public string address { get; set; }
		public string application_no { get; set; }
		public string timestamp { get; set; }
	}

	public class LocationData
	{
		public string id { get; set; }
		public string uid { get; set; }
		public string extent { get; set; }
		public string layout_acres { get; set; }
		public string survey_no { get; set; }
		public string location { get; set; }
		public string ward_no { get; set; }
		public string block_no { get; set; }
		public string road_width { get; set; }
		public string no_of_plots { get; set; }
		public string area_for_openspace_sq { get; set; }
		public string area_for_openspace { get; set; }
		public string area_for_roads { get; set; }
		public string plotarea_percentage_sq { get; set; }
		public string plotarea_percentage { get; set; }
		public string area_for_amenties_sq { get; set; }
		public string area_for_amenties { get; set; }
		public string layout_type { get; set; }
		public string court_case { get; set; }
		public string court_case_remarks { get; set; }
		public string application_no { get; set; }
		public string stage { get; set; }
		public string status { get; set; }
		public string payment_status { get; set; }
		public string timestamp { get; set; }
	}

	public class OwnerData
	{
		public string id { get; set; }
		public string uid { get; set; }
		public string person_type { get; set; }
		public string surname { get; set; }
		public string name { get; set; }
		public string mobile { get; set; }
		public string email { get; set; }
		public string license_no { get; set; }
		public string validity_date { get; set; }
		public string address { get; set; }
		public string timestamp { get; set; }
	}

	public class LandData
	{
		public string id { get; set; }
		public string uid { get; set; }
		public string sitearea { get; set; }
		public string sitearea_ground { get; set; }
		public string encumbrance_no { get; set; }
		public string encumbrance_date { get; set; }
		public string proceeding_no { get; set; }
		public string proceeding_date { get; set; }
		public string other_remarks { get; set; }
		public string timestamp { get; set; }
	}

	public class TopographicalData
	{
		public string id { get; set; }
		public string uid { get; set; }
		public string site_abutting { get; set; }
		public string site_abutting_desc { get; set; }
		public string noc { get; set; }
		public string noc_desc { get; set; }
		public string site_line { get; set; }
		public string site_line_desc { get; set; }
		public string survey_nos { get; set; }
		public string field_numbers { get; set; }
		public string site_acquited { get; set; }
		public string site_acquired_desc { get; set; }
		public string site_reference { get; set; }
		public string site_reference_desc { get; set; }
		public string timestamp { get; set; }
	}

	public class DocDetail
	{
		public string id { get; set; }
		public string uid { get; set; }
		public string doc_no { get; set; }
		public string date { get; set; }
		public string year { get; set; }
		public string sro_location { get; set; }
		public string vendor { get; set; }
		public string vendee { get; set; }
		public string plot_no { get; set; }
		public string extent { get; set; }
		public string east { get; set; }
		public string south { get; set; }
		public string north { get; set; }
		public string west { get; set; }
		public string doc_upload { get; set; }
	}

	public class RequiredCertificates
	{
		public string id { get; set; }
		public string uid { get; set; }
		public string cert1 { get; set; }
		public string remarks1 { get; set; }
		public string cert2 { get; set; }
		public string remarks2 { get; set; }
		public string cert3 { get; set; }
		public string remarks3 { get; set; }
		public string cert4 { get; set; }
		public string remarks4 { get; set; }
		public string cert5 { get; set; }
		public string remarks5 { get; set; }
		public string cert6 { get; set; }
		public string remarks6 { get; set; }
		public string cert7 { get; set; }
		public string remarks7 { get; set; }
		public string cert8 { get; set; }
		public string remarks8 { get; set; }
		public string cert9 { get; set; }
		public string remarks9 { get; set; }
		public string cert10 { get; set; }
		public string remarks10 { get; set; }
		public string cert11 { get; set; }
		public string remarks11 { get; set; }
		public string cert12 { get; set; }
		public string remarks12 { get; set; }
		public string timestamp { get; set; }
	}

	public class PaymentData
	{
		public string id { get; set; }
		public string uid { get; set; }
		public string layout_fee { get; set; }
		public string inspection_fee { get; set; }
		public string security_deposit { get; set; }
		public string total { get; set; }
		public string timestamp { get; set; }
	}

	public class Detail
	{
		public PersonalData personal_data { get; set; }
		public string key { get; set; }
		public LocationData location_data { get; set; }
		public List<OwnerData> owner_data { get; set; }
		public LandData land_data { get; set; }
		public TopographicalData topographical_data { get; set; }
		public List<DocDetail> doc_details { get; set; }
		public RequiredCertificates required_certificates { get; set; }
		public PaymentData payment_data { get; set; }
	}

	public class LayoutInformation
	{
		public bool search { get; set; }
		public List<Detail> details { get; set; }
	}


	public class Datum
	{
		public string TANKER_TRIP_START_DATE_TIME { get; set; }
		public string SOURCEHABCODE { get; set; }
		public string TRIP_ID { get; set; }
		public string TANKER_OWNER_NAME { get; set; }
		public string TANKER_REG_NO { get; set; }
		public string TANKER_OWNER_MOBILE { get; set; }
		public string DESTHABCODE { get; set; }
		public string TANKER_TRIP_END_DATE_TIME { get; set; }
	}

	public class TankarWaterResponse
	{
		public string STATUS { get; set; }
		public List<Datum> DATA { get; set; }
		public string ERROR_MSG { get; set; }
		public string ERROR_CODE { get; set; }
	}

	public class PRRDMRGCHECK
	{
		public string status { set; get; }
		public string Reason { set; get; }

	}

	public class PRRDINFO
	{
		public List<PRRDMRGCHECK> PRRDMRGCHECK;
	}
	public class AuthenticateNOCMarriage
	{
		public string msg { set; get; }
		public string token { set; get; }
		public string status { set; get; }
		public string granttype { set; get; }
		public string token_sno { set; get; }
		public string username { set; get; }
		public string servicetype { set; get; }
		public string gs_applid { set; get; }
	}

	public class VolunteerCls
	{
		public string ftype { set; get; }
		public string fuid_num { set; get; }
		public string fvv_id { set; get; }
	}

	public class DistrictsCls
	{
		public string TYPE { get; set; }
		public string DEPARTMENT { get; set; }
		public string HOD { get; set; }

		public string DISTRICT { get; set; }
		public string MANDAL { get; set; }
		public string PANCHAYAT { get; set; }
		public string RUFLAG { get; set; }


	}
	public class SecretriatVVModel
	{
		public string TYPE { get; set; }
		public string DISTRICTCODE { get; set; }
		public string MANDALCODE { get; set; }
		public string SECRETRIATCODE { get; set; }
		public string VVID { get; set; }
		public string HHID { get; set; }
		public string TEMPID { get; set; }
		public string UpdateBy { get; set; }
		public string UpdateDate { get; set; }
		public string Status { get; set; }
	}

}