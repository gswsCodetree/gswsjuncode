using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.DL.CommonHel
{
	public class LoginModel
	{
		public string FUsername { get; set; }
		public string Newpassword { get; set; }
		public string ConfirmPassword { get; set; }
		public string Captcha { get; set; }
		public string ConfirmCaptch { get; set; }
		public string Ftype { get; set; }
		public string Role { get; set; }

		//Forgot Password
		public string MOBILE { get; set; }
		public string OTP { get; set; }
		public string UNIQUEID { get; set; }
	}


	public class HeadertokenModel
	{
		public string Ftype { get; set; }
		public string UserId { get; set; }
		public string Token { get; set; }

	}

	public class LoginModelrb
	{
		public string Ftype { get; set; }
		public string Fusername { get; set; }
		public string FPassword { get; set; }
		public string FCapatchaId { get; set; }
		public string FconfId { get; set; }
	}
	public class ResponseModel
	{
		public int Status { get; set; }
		public string Reason { get; set; }
		public DataTable DataList { get; set; }
	}

	public class apEpdclModel : encryptionDataModel
	{
		public string ENCRYPTED_DATA { get; set; }
		public string IV { get; set; }

		public string secratariat_id { get; set; }
	}

	public class encryptionDataModel
	{
		public string PS_TXN_ID { get; set; }
		public string RETURN_URL { get; set; }
		public string USERNAME { get; set; }
		public string PASSWORD { get; set; }
	}


	public class Decryptdatamodel
	{
		public string encryprtext { get; set; }
		public string Ivval { get; set; }
		public string key { get; set; }
	}

	public class EncryptDataModel
	{
		public string USERNAME { get; set; }
		public string PASSWORD { get; set; }
		public string PS_TXN_ID { get; set; }
		public string RETURN_URL { get; set; }
		//public string DeptCode { get; set; }
	}

	public class TransactionInitateModel
	{
		public string DEPTCODE { get; set; }
		public string DEPTNAME { get; set; }
		public string HODCODE { get; set; }
		public string SERVICECODE { get; set; }
		public string SECRETRAINTCODE { get; set; }
		public string LOGINUSER { get; set; }
		public string URLID { get; set; }

	}

	public class LGDMasterModel:LogModel
	{
		public string FTYPE { get; set; }
		public string DEPTCODE { get; set; }
		public string HODCODE { get; set; }
		public string DISTCODE { get; set; }
		public string MCODE { get; set; }
		public string GPCODE { get; set; }
		public string RUFLAG { get; set; }
		public string ROLE { get; set; }
		public string MandalCode { get; set; }
	}

	public class captchgens
	{
		public string idval { get; set; }
		public string imgurl { get; set; }
		public string code { get; set; }
		public string Reason { get; set; }

	}

	public class captch
	{
		public string id { get; set; }
		public string type { get; set; }
		public string Capchid { get; set; }
		public string REQUESTIP { get; set; }
		public string browser { get; set; }
		public string SOURCE { get; set; }

	}

	public class TransRes
	{
		public string type { get; set; }
		public string user { get; set; }
		public string secid { get; set; }
		public string designationId { get; set; }

	}

	public class StatusTrackingModel
	{
		public string DEPTID { get; set; }
		public string HODID { get; set; }
		public string URLID { get; set; }
		public string DISTID { get; set; }
		public string MANDALID { get; set; }
		public string GPID { get; set; }
		public string SECCID { get; set; }
		public string USERID { get; set; }
		public string CHECKTRANSID { get; set; }

	}

	public class Profilemodel
	{
		public string UNIQUEID { get; set; }
		public string TYPE { get; set; }
		public string OTP { get; set; }
		public string STATUS { get; set; }
		public string MOBILE { get; set; }
		public string EMAILID { get; set; }

		public string PASSWORD { get; set; }
		public string DISTRICT { get; set; }
		public string DEPARTMENT { get; set; }
		public string FLAG { get; set; }
		public string MANDAL { get; set; }
		public string SECRETARIAT { get; set; }
		public string FROMDATE { get; set; }
		public string TODATE { get; set; }
		public string TOKEN { get; set; }
		public string URL_ID { get; set; }
		public string SERVICE { get; set; }
		public string LEVEL { get; set; }
		public string COL_NAME { get; set; }
		public string SERVICE_TYPE { get; set; }
	}

	//Aadhar OTP
	public class AadharOTP
	{
		public string AADHAR { get; set; }
		public string OTP { get; set; }
		public string ID { get; set; }
		public string Type { get; set; }
		public string Status { get; set; }
		public string Reason { get; set; }
		public string Result { get; set; }
		public string Details { get; set; }
	}

	//Aadhar Details 
	public class Aadhardetails : AadharOTP
	{
		public string NAME { get; set; }
		public string submitschname { get; set; }
		public string GENDER { get; set; }
		public string STRBASE64IMG { get; set; }
		public string DATEOFBIRTH { get; set; }
		public string DISTRICTNAME { get; set; }
		public string MANDALNAME { get; set; }
		public string VILLAGENAME { get; set; }
		public string PINCODE { get; set; }
		public string PSSDISTRICTID { get; set; }
		public string PSSMANDALID { get; set; }
		public string PSSVILLAGECODE { get; set; }
		public string PSSWARD { get; set; }
		public string PSSNAME { get; set; }
		public string PSSGENDER { get; set; }
		public string PSSDATEOFBIRTH { get; set; }
		public string PSSDOORNO { get; set; }
		public string PSSSTREETNAME { get; set; }
		public string PSSDISTRICTNAME { get; set; }
		public string PSSRURALURBANFLAG { get; set; }
		public string PSSMANDALNAME { get; set; }
		public string PSSVILLAGENAME { get; set; }
		public string PSSPINCODE { get; set; }
		public string PSSMOBILENUMBER { get; set; }
		public string PSSEMAILID { get; set; }

	}

	public class Navakasammodel
	{
		public string secr_user { get; set; }
		public string wsuser = "navasakam";
		public string wspasswd = "N@va$kam@123";
	}

	public class WalletModel
	{
		public string districtId { get; set; }
		public string gswsCode { get; set; }
		public string userName { get; set; }
		public string password { get; set; }
		public string operatorId { get; set; }
		public string updated_by { get; set; }
	}

	public class WalletAmountmodel
	{
		public string district_code { get; set; }
		public string gsws_code { get; set; }
	}

	public class APITRACKMODEL
	{
		public string Ptype { get; set; }
		public string DistrictCode { get; set; }
		public string MandalCode { get; set; }
		public string SceretriatCode { get; set; }
		public string DeptId { get; set; }
		public string HODId { get; set; }
		public string UrlId { get; set; }
		public string InputData { get; set; }
		public string TrackingId { get; set; }
		public string Status { get; set; }
		public string Remarks { get; set; }
		public string Loginid { get; set; }
	}

	public class Affidavitmodel
	{
		public string FType { get; set; }
		public string DistCode { get; set; }
		public string MCode { get; set; }
		public string SecCode { get; set; }
		public string Loginuser { get; set; }
		public string FilePath { get; set; }
	}
	public class Digitalmodels

	{
		public string P_TYPE { get; set; }
		public string USER_ID { get; set; }
		public string PASSWORD { get; set; }
		public string MOBILE_NO { get; set; }
		public string FROMDATE { get; set; }
		public string TODATE { get; set; }
		public string ROLL_ID { get; set; }
		public string ENTRY_BY { get; set; }
		public string SCRT_ID { get; set; }
		public string USERTYPE { get; set; }
	}

	public class RaminfoWalletModel
	{
		public string distCode { get; set; }
		public string gwsCode { get; set; }
		public string agencyType { get; set; }
		public string reqId { get; set; }
		public string userId { get; set; }
		public string password { get; set; }
		public string operatorId { get; set; }

	}
	public class SecretariatFormObject
	{
		public string Ftype { get; set; }
		public string UniqueId { get; set; }
		public string DistrictCode { get; set; }
		public string MandalCode { get; set; }
		public object RUFlag { get; set; }
		public string SecretariatCode { get; set; }
		public string SecterataitSmartPhone { get; set; }
		public string SecSmartPhoneMake { get; set; }
		public string SecSmartPhoneMakeOthers { get; set; }
		public string SecSIMOneIMEI { get; set; }
		public string SecIMEITwoSIM { get; set; }
		public string SecRecieveMobileNum { get; set; }
		public string SecMobileOperator { get; set; }
		public string SecMobileNum { get; set; }
		public string SecSIMSerialNum { get; set; }
		public string FPSScannerAviaiable { get; set; }
		public string FPSMake { get; set; }
		public string FPSModelNum { get; set; }
		public string FPSSerialNum { get; set; }
		public string PrinterAvailable { get; set; }
		public string PrinterMake { get; set; }
		public string PrinterModelNum { get; set; }
		public string PrinterSerialNum { get; set; }
		public string NumofDesktop { get; set; }
		public string DesktoponeMake { get; set; }
		public string DesktoponeSerial { get; set; }
		public string CPUoneMake { get; set; }
		public string CPUoneSerialNum { get; set; }
		public string MouseoneMake { get; set; }
		public string MouseoneSerialNum { get; set; }
		public string keyboardoneMake { get; set; }
		public string keyboardoneSerialNum { get; set; }
		public string DesktoptwoMake { get; set; }
		public string DesktoptwoSerial { get; set; }
		public string CPUtwoMake { get; set; }
		public string CPUtwoSerialNum { get; set; }
		public string MousetwoMake { get; set; }
		public string MousetwoSerialNum { get; set; }
		public string keyboardtwoMake { get; set; }
		public string keyboardtwoSerialNum { get; set; }
		public string UPSAvailable { get; set; }
		public string UPSMakeone { get; set; }
		public string UPSoneSerialnum { get; set; }
		public string UPSMaketwo { get; set; }
		public string UPStwoSerialnum { get; set; }
		public string RecieveMahilePoliceSmartPhone { get; set; }
		public string MahilaSmartPhMake { get; set; }
		public string MahileOtherSmartMake { get; set; }
		public string MahilaIMEINumone { get; set; }
		public string MahilaIMEINumtwo { get; set; }
		public string RecieveMobileNumber { get; set; }
		public string MahilaMobileOperator { get; set; }
		public string MahileMobileNumber { get; set; }
		public string MahilaSIMSerialNum { get; set; }
		public string VolunteerNoofSmartPh { get; set; }
		public string VolunteerNoofSIMCard { get; set; }

		public string InternetAvailble { get; set; }
		public string MobileMdmInstall { get; set; }
		public string NoofArogyasetuapp { get; set; }
		public string HighSecurityStationeryRec { get; set; }
		public string HighSecurityStationeryStock_Availoable { get; set; }
		public string EligibilityCriteria { get; set; }
		public string EligibilityImage { get; set; }
		public string Beneficiarylist { get; set; }
		public string BeneficiarylistImage { get; set; }

		public string SecTables { get; set; }
		public string SecPlasticchairs { get; set; }
		public string SecStypechairs { get; set; }
		public string Secnoticeboard { get; set; }
		public string SecIronsafes { get; set; }
		public string SecIronracks { get; set; }
		public string VolunteerPhaseone { get; set; }
		public string VolunteerPhasetwo { get; set; }
		public string FunctionariesTrained { get; set; }
		public string VolunteerVacant { get; set; }

		public string SecBuildingPaint { get; set; }
		public string Secnameboard { get; set; }
		public string SecElectrification { get; set; }
		public string SecDrinkwater { get; set; }
		public string SecToilets { get; set; }
		public string SecStationary { get; set; }
		public string SecSpandana { get; set; }
		public string SecBillingCounter { get; set; }
		public string MCassUserId { get; set; }
		public string MCassActive { get; set; }
		public string Insertby { get; set; }
	}

	public class HardwareValidateModel
	{
		public string FTYPE { get; set; }
		public string IMEINUM { get; set; }
		public string SERIALNUM { get; set; }
		public string MODELNUM { get; set; }

	}
	public class VolunteerFormObject
	{
		public string Ftype { get; set; }
		public string DistrictCode { get; set; }
		public string MandalCode { get; set; }
		public string RUFlag { get; set; }
		public string SecretariatCode { get; set; }
		public string VolunteerName { get; set; }
		public string VolunteerUID { get; set; }
		public string VolunteerCFMSID { get; set; }
		public string VolunteerClusterName { get; set; }
		public string VolunteertType { get; set; }
		public string VolunteerSmartPhone { get; set; }
		public string VolunteerSIMIMEIOne { get; set; }
		public string VolunteerSIMIMEITwo { get; set; }
		public string VolunteerMobileOperator { get; set; }
		public string VolunteerOfficialMobile { get; set; }
		public string VolunteerPersonalMobile { get; set; }
		public string VolunteerSerialNumber { get; set; }
		public string VolunteerFPS { get; set; }
		public string VolunteerFPSModelNumber { get; set; }
		public string VolunteerFPSSerialNumber { get; set; }
		public string VolunteerSmartPhoneRecieve { get; set; }
		public string VolunteerSIMMobileNumbers { get; set; }
		public string VolunteerFingerprintcumscanner { get; set; }
		public string VolunteerinstallTelgram { get; set; }
		public string VolunteerWtsclustergroup { get; set; }
		public string Insertby { get; set; }
		public string VolMCassUserId{ get; set; }
		public string VolMCassActive { get; set; }
	}

	public class LoginMailModel
	{
		public string Ftype { get; set; }
		public string FDistrictCode { get; set; }
		public string FMandalCode { get; set; }
		public string FSeccCode { get; set; }
		public string FUserId { get; set; }
		public string FMobileNumber { get; set; }
		public string FMailID { get; set; }
		public string FPassword { get; set; }
		public string FRoleId { get; set; }
		public string Insertby { get; set; }
	}
}

