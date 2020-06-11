using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Depts.Services
{
    public class ServicesModel
    {
    }

    public class AppStatus
    {
        public string ftype { get; set; }
        public string fdpart_id { get; set; }
        public string fadhar_no { get; set; }
    }

	#region "Integrated / Income Certificates Download"
	public class MeesevaCertificates
	{
		public string strIntegratedID { set; get; }
		public string CertType { set; get; }
	}
	#endregion

	#region Textile
	public class TextileModel
	{
		public string NAME { get; set; }
		public string FATHERNAME { get; set; }
		public string AADHAAR { get; set; }
		public string MOBILE { get; set; }
		public string IDCARD { get; set; }
		public string AGE { get; set; }
		public string RATION { get; set; }
		public string ANNINCOME { get; set; }
		public string CASTE { get; set; }
		public string SUBCASTE { get; set; }
		public string FAMILYCOUNT { get; set; }
		public string LOOMCOUNT { get; set; }
		public string LOOMTYPE { get; set; }
		public string LOOMOWNERSHIP { get; set; }
		public string DOINGLOOM { get; set; }
		public string HOUSETYPE { get; set; }
		public string PSSREG { get; set; }
		public string BANK { get; set; }
		public string BRANCH { get; set; }
		public string ACCOUNT { get; set; }
		public string IFSC { get; set; }
		public string DOORNO { get; set; }
		public string DISTRICT { get; set; }
		public string RURUALURBAN { get; set; }
		public string MANDAL { get; set; }
		public string VILLAGEPANCHAYATH { get; set; }
		public string PINCODE { get; set; }
		public string SUBMITTEDBY { get; set; }
	}
	#endregion

	//Asset Trackig
	public class Assetmodel
	{
		public string USERNAME { get; set; }
		public string PWD { get; set; }


		public string TYPE { get; set; }
		public string DISTRICT { get; set; }
		public string MANDAL { get; set; }
		public string SECRATARIAT { get; set; }

		public string id { get; set; }
		public string Capchid { get; set; }
		public string requestip { get; set; }

		//Hardware Issue Component Loading
		public string COMPONENTID { get; set; }
		public string HWCOMPONENT { get; set; }
		public string HWISSUE { get; set; }
		public string IMAGEURL { get; set; }
		public string REMARKS { get; set; }
		public string SOURCE { get; set; }
	}


	public class insarray
	{
		public string USERNAME { get; set; }
		public string DISTRICT { get; set; }
		public string MANDAL { get; set; }
		public string SECRATARIAT { get; set; }


		public string CPUSERIALNO { get; set; }
		public string CPUCONN { get; set; }
		public string CPUWORKING { get; set; }
		public string CPUCONREMARKS { get; set; }
		public string CPUWORREMARKS { get; set; }


		public string MONITORSERIALNO { get; set; }
		public string MONITORCONN { get; set; }
		public string MONITORWORKING { get; set; }
		public string MONITORCONREMARKS { get; set; }
		public string MONITORWORREMARKS { get; set; }


		public string KEYBOARDSERIALNO { get; set; }
		public string KEYBOARDCONN { get; set; }
		public string KEYBOARDWORKING { get; set; }
		public string KEYBOARDCONREMARKS { get; set; }
		public string KEYBOARDWORREMARKS { get; set; }


		public string MOUSESERIALNO { get; set; }
		public string MOUSECONN { get; set; }
		public string MOUSEWORKING { get; set; }
		public string MOUSECONREMARKS { get; set; }
		public string MOUSEWORREMARKS { get; set; }


		public string INVERTORSERIALNO { get; set; }
		public string INVERTORCONN { get; set; }
		public string INVERTORWORKING { get; set; }
		public string INVERTORCONREMARKS { get; set; }
		public string INVERTORWORREMARKS { get; set; }


		public string BATTERIESSERIALNO { get; set; }
		public string BATTERIESCONN { get; set; }
		public string BATTERIESWORKING { get; set; }
		public string BATTERIESCONREMARKS { get; set; }
		public string BATTERIESWORREMARKS { get; set; }


		public string PRINTERSERIALNO { get; set; }
		public string PRINTERCONN { get; set; }
		public string PRINTERWORKING { get; set; }
		public string PRINTERCONREMARKS { get; set; }
		public string PRINTERWORREMARKS { get; set; }


		public string LAMINATORSERIALNO { get; set; }
		public string LAMINATORCONN { get; set; }
		public string LAMINATORWORKING { get; set; }
		public string LAMINATORCONREMARKS { get; set; }
		public string LAMINATORWORREMARKS { get; set; }


		public string BIOMETRICSERIALNO { get; set; }
		public string BIOMETRICCONN { get; set; }
		public string BIOMETRICWORKING { get; set; }
		public string BIOMETRICCONREMARKS { get; set; }
		public string BIOMETRICWORREMARKS { get; set; }

		public string MACADDRESS { get; set; }
		public string MODELNO { get; set; }
		public string BATCHNO { get; set; }
		public string SYSNO { get; set; }

	}

	public class AssetTracking
	{
		public string TYPE { get; set; }
		public List<insarray> DATAARRAY { get; set; }
	}

	public class AppStatusCls
	{
		public string AppType { get; set; }
		public string BenID { get; set; }
	}

	public class MUIDCls
	{
		public string ulb { get; set; }
		public string txn_id { get; set; }
		public string application_number { get; set; }
	}

	public class TransCls
	{
		public string txn_id { get; set; }
		public string application_number { get; set; }
	}
}