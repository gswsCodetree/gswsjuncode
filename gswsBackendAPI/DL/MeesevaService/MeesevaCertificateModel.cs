using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace gswsBackendAPI.DL.MeesevaService
{
	public class MeesevaCertificateModel
	{

	}
	public class MeesevaResponse
	{
		public string STATUS { get; set; }
		public string CERTIFICATEURL { get; set; }
		public string REASON { get; set; }
		
	}

	public class LateRegistrationsOfBirthAndDeath
	{

		private string serviceIDField { get; set; }

		private string application_NumberField { get; set; }

		private string service_TypeIDField { get; set; }

		private string aPPlicantNameField { get; set; }

		private string mobileNumberField { get; set; }

		private string rationCardNumberField { get; set; }

		private string aadharCardNoField { get; set; }

		private string eventRelationField { get; set; }

		private string circleField { get; set; }

		private string wardField { get; set; }

		private string serviceSpecificAddressField { get; set; }

		private string event_NameField { get; set; }

		private string dateofEventField { get; set; }

		private string placeofEventField { get; set; }

		private string eventGenderField { get; set; }

		private string deathCauseField { get; set; }

		private string motherNameField { get; set; }

		private string fatherNameField { get; set; }

		private string doorNumberField { get; set; }

		private string pincodeField { get; set; }

		private string districtField { get; set; }

		private string deliveryTypeField { get; set; }

		private string purposeField { get; set; }

		private string postal_DoorNoField { get; set; }

		private string postal_AddressField { get; set; }

		private string postal_DistrictField { get; set; }

		private string postal_MandalField { get; set; }

		private string postal_VillageField { get; set; }

		private string postal_PincodeField { get; set; }

		private string service_ChargesField { get; set; }

		private string user_ChargesField { get; set; }

		private string delivery_ChargesField { get; set; }

		private string total_ChargesField { get; set; }

		private string sLAField { get; set; }

		private string noofdeliveryField { get; set; }

		private string ageofmotherField { get; set; }

		private string deceasedageField { get; set; }

		private string yearsField { get; set; }

		private string stateIdField { get; set; }

		private string statusField { get; set; }

		private string createdByField { get; set; }

		private string documentsField { get; set; }
	}

	public class agriincomemodel
	{
		public string userId { get; set; }
		public string password { get; set; }
		public string logindid { get; set; }
		public string serviceId { get; set; }
		public string Documentrefnumber { get; set; }
		public string applicationid { get; set; }
		public string aadressflag { get; set; }
		public string realtion { get; set; }
		public string relationanme { get; set; }
		public string deliverytype { get; set; }
		public string Purposeofincomecertificate { get; set; }
		public string landlocatedDoorno { get; set; }
		public string landlocatedlocality { get; set; }
		public string landlocateddistrict { get; set; }
		public string landlocatedmandal { get; set; }
		public string landlocatedvillage { get; set; }
		public string landlocatedpincode { get; set; }
		public string stateId { get; set; }
		public string gridlanddeatils { get; set; }
		public string user_charge { get; set; }
		public string service_charge { get; set; }
		public string postal_charge { get; set; }
		public string Total_Amount { get; set; }

	}
	}