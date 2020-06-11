using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.DL.MeesevaService
{
	public class MeesevaHelper
	{
		UatMeeseva.MeesevaMobileWebservice _uatMeeseva = new UatMeeseva.MeesevaMobileWebservice();

		MeesevaResponse _resMeeseva = new MeesevaResponse();
		public dynamic GetIntergratedCertificate( string CertificateID)
		{
			try
			{			

				var data = _uatMeeseva.INTEGRATEDCERTIFICATEDetails(CertificateID);

				if (data != null)
				{
					_resMeeseva.STATUS = "100";
					_resMeeseva.CERTIFICATEURL = data;

				}
				else
				{
					_resMeeseva.STATUS = "102";
					_resMeeseva.CERTIFICATEURL = data;
					_resMeeseva.REASON = "Invalid Certificate Number";
				}
			}
			catch (Exception ex)
			{
				_resMeeseva.STATUS = "102";
				_resMeeseva.CERTIFICATEURL = null;
				_resMeeseva.REASON = ex.Message.ToString();
			}
			return _resMeeseva;
		}
		public dynamic GetLateRegistrationBirthandDeath(string CertificateID)
		{
			try
			{
				//_uatMeeseva.fam();

				UatMeeseva.LateRegistrationsOfBirthAndDeath _objlate = new UatMeeseva.LateRegistrationsOfBirthAndDeath();
				UatMeeseva.MobileUserDetails _mobuser = new UatMeeseva.MobileUserDetails();
				var data = _uatMeeseva.LATEREGISTRATIONOFBIRTHDEATH_GetTransactionID(_objlate, _mobuser);

				return data;
			}
			catch (Exception ex)
			{
				_resMeeseva.STATUS = "102";
				_resMeeseva.CERTIFICATEURL = null;
				_resMeeseva.REASON = ex.Message.ToString();
			}
			return _resMeeseva;
		}
	}
}