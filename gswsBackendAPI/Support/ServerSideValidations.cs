using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using gswsBackendAPI.DL.CommonHel;
using gswsBackendAPI.Internal.Backend;

namespace gswsBackendAPI.Support
{
    public class ServerSideValidations
    {
        dynamic CatchData = new ExpandoObject();
        public dynamic CheckServicesURL(InternalURL root)
        {
            var Message = "";
            try
            {
                if (string.IsNullOrEmpty(root.DEPARTMENT))
                    Message = "Please Select Department";
                else if (string.IsNullOrEmpty(root.HOD))
                    Message = "Please Select HOD";
                else if (string.IsNullOrEmpty(root.SERVICE))
                    Message = "Please Select Service";
                else if (string.IsNullOrEmpty(root.REQUESTTYPE))
                    Message = "Please Select Request Type";
                else if (string.IsNullOrEmpty(root.URL))
                    Message = "Please Enter URL";
                else if (string.IsNullOrEmpty(root.URLDESCRIPTION))
                    Message = "Please Enter URL Description";
                else if (string.IsNullOrEmpty(root.SERVICETYPE))
                    Message = "Please Select Service Type";
                else if (string.IsNullOrEmpty(root.ACCESSLEVEL))
                    Message = "Please Select Access Level";
                else if (root.ACCESSLEVEL == "1" && string.IsNullOrEmpty(root.DISTRICT))
                    Message = "Please Select District";
                else if (root.ACCESSLEVEL == "2" && string.IsNullOrEmpty(root.MANDAL))
                    Message = "Please Select Mandal";
                else if (root.ACCESSLEVEL == "3" && string.IsNullOrEmpty(root.PANCHAYAT))
                    Message = "Please Select Panchayat";

                if (string.IsNullOrEmpty(Message))
                {
                    CatchData.Status = "Success";
                    CatchData.Reason = "Validation Successfully.";
                }
                else
                {
                    CatchData.Status = "Failure";
                    CatchData.Reason = Message;
                }
            }
            catch (Exception ex)
            {
                CatchData.Status = "Failure";
                CatchData.Reason = ex.Message;
            }

            return CatchData;
        }

		//Check Password Validation
		public dynamic CheckPasswordValidation(Profilemodel root)
		{
			var Message = "";
			try
			{
				if (string.IsNullOrEmpty(root.PASSWORD))
					Message = "Please Enter Password";

				else if (pwdregexpression(root.PASSWORD))
					Message = "Password Should Contain atleast 1 digit,1 Uppercase,1 Lowercase and 1 special character";


				if (string.IsNullOrEmpty(Message))
				{
					CatchData.Status = "Success";
					CatchData.Reason = "Validated Successfully.";
				}
				else
				{
					CatchData.Status = "Failure";
					CatchData.Reason = Message;
				}
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failure";
				CatchData.Reason = ex.Message;
			}

			return CatchData;
		}

		public bool pwdregexpression(String strToCheck)
		{
			Regex objAlphaPattern = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
			return !objAlphaPattern.IsMatch(strToCheck);
		}
	}
}