using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using gswsBackendAPI.DL.CommonHel;

namespace gswsBackendAPI.Depts.Energy
{
    public class EnergyHelper : EnergySPHelper
    {
        dynamic obj = new ExpandoObject();

        #region APSPDCL

        public dynamic GetServiceStatus_helper(AppStatus root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                if (Utils.IsAlphaNumeric(root.PS_TXN_ID) && Utils.IsAlphaNumeric(root.CSC_REGNO))
                {
                    var val = PostData("http://122.252.251.175:8080/Praja_Transactions/service/getSPDCLServices/Status", root);
                    var data = GetSerialzedData<dynamic>(val);
                    obj.Status = 100;
                    obj.Reason = "Data Loaded Successfully.";
                    obj.Details = data;
                }
                else
                {
                    obj.Status = 102;
                    obj.Reason = "Error Occured While Getting Status";
                }
                return obj;
            }
            catch (Exception ex)
            {
                Common_Energy_Error(ex.Message.ToString(), "http://122.252.251.175:8080/Praja_Transactions/service/getSPDCLServices/Status", "2");

                obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
                return obj;
            }

        }

        public dynamic GetTransactionHistory_helper(AppStatus root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                if (Utils.IsAlphaNumeric(root.PS_TXN_ID) && Utils.IsAlphaNumeric(root.CSC_REGNO))
                {
                    var val = PostData("http://122.252.251.175:8080/Praja_Transactions/service/getSPDCLServices/TranHistory", root);
                    var data = GetSerialzedData<dynamic>(val);
                    obj.Status = 100;
                    obj.Reason = "Data Loaded Successfully.";
                    obj.Details = data;
                }
                else
                {
                    obj.Status = 102;
                    obj.Reason = "Error Occured While Getting Status";
                }
                return obj;
            }
            catch (Exception ex)
            {
                Common_Energy_Error(ex.Message.ToString(), "http://122.252.251.175:8080/Praja_Transactions/service/getSPDCLServices/TranHistory", "2");
                obj.Status = 102;
                obj.Reason = ThirdpartyMessage;
				return obj;
            }

        }

        #endregion

        #region APEPDCL

        public dynamic GetAPEPDCLServiceStatus_helper(APEPDCLStatus root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                if (Utils.IsAlphaNumeric(root.REQUEST_NO) && Utils.IsAlphaNumeric(root.MOBILE))
                {
                    var val = PostData("http://59.144.184.77:8085/EPDCL_GSWS/rest/newConnectionStatus", root);
                    var data = GetSerialzedData<dynamic>(val);
                    obj.Status = 100;
                    obj.Reason = "Data Loaded Successfully.";
                    obj.Details = data;
                }
                else
                {
                    obj.Status = 102;
                    obj.Reason = "Error Occured While Getting Status";
                }
                return obj;
            }
            catch (Exception ex)
            {
                Common_Energy_Error(ex.Message.ToString(), "http://59.144.184.77:8085/EPDCL_GSWS/rest/newConnectionStatus", "2");
                obj.Status = 102;
                obj.Reason = ThirdpartyMessage;
				return obj;
            }

        }

        public dynamic GetAPEPDCLTransactionHistory_helper(APEPDCLHistory root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                if (Utils.IsAlphaNumeric(root.REG_NO))
                {
                    var val = PostData("http://59.144.184.77:8085/EPDCL_GSWS/rest/regHistory", root);
                    var data = GetSerialzedData<dynamic>(val);
                    obj.Status = 100;
                    obj.Reason = "Data Loaded Successfully.";
                    obj.Details = data;
                }
                else
                {
                    obj.Status = 102;
                    obj.Reason = "Error Occured While Getting Status";
                }
                return obj;
            }
            catch (Exception ex)
            {
                Common_Energy_Error(ex.Message.ToString(), "http://59.144.184.77:8085/EPDCL_GSWS/rest/regHistory", "2");
                obj.Status = 102;
                obj.Reason = ThirdpartyMessage;
				return obj;
            }

        }
        public bool Common_Energy_Error(string msg, string url, string etype)
        {
            ExceptionDataModel objex = new ExceptionDataModel();
            try
            {
                objex.E_DEPTID = DepartmentEnum.Department.Energy.ToString();
                objex.E_HODID = DepartmentEnum.HOD.Electrical_Safety.ToString();
                objex.E_ERRORMESSAGE = msg;
                objex.E_SERVICEAPIURL = url;
                objex.E_ERRORTYPE = etype;
                new LoginSPHelper().Save_Exception_Data(objex);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        #endregion
    }
}