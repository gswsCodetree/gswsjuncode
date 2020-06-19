using gswsBackendAPI.DL.CommonHel;
using gswsBackendAPI.DL.DataConnection;
using gswsBackendAPI.transactionModule;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace gswsBackendAPI.Depts.CommercialTax
{
    public class CommercialTaxHelper : CommercialTaxSPHelper
	{
        #region Commertial Tax
        public dynamic GetDataByTIN_Helper(DocCls root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                var result = true;
                if (!string.IsNullOrEmpty(root.tin))
                    result = gswsBackendAPI.DL.CommonHel.Utils.IsAlphaNumeric(root.tin);
                if (result)
                {
                    var val = PostData("https://apct.gov.in/pspt/api/registration/get_reg_dtls", root);
                    var data = GetSerialzedData<dynamic>(val);

                    obj.Status = 100;
                    obj.Details = data;
                }
                else
                {
                    obj.Status = 102;
                    obj.Reason = "Special Characters Are Not Allowed";
                }

                return obj;
            }
            catch (Exception ex)
            {

                obj.Status = 102;
				obj.Reason = ThirdpartyMessage;
                Common_CommercialTax_Error(ex.Message.ToString(), "https://apct.gov.in/pspt/api/registration/get_reg_dtls", "2");
                return obj;
            }

        }

        public dynamic UploadDoc_Helper(DocCls root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                var val = PostData("https://apct.gov.in/pspt/api/Doc/docupld", root);
                var data = GetSerialzedData<dynamic>(val);

                obj.Status = 100;
                obj.Details = data;

                return obj;
            }
            catch (Exception ex)
            {
                obj.Status = 102;
                obj.Reason = ThirdpartyMessage;
                Common_CommercialTax_Error(ex.Message.ToString(), "https://apct.gov.in/pspt/api/Doc/docupld", "2");
                return obj;
            }

        }

        public dynamic SubmitData_Helper(dynamic root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                var val = PostData("https://apct.gov.in/pspt/api/registration/ins_reg_dtls", root);
                var data = GetSerialzedData<dynamic>(val);
				if (data.status_cd == "1")
				{
					try
					{
						transactionModel objtrans = new transactionModel();
						objtrans.TYPE = "2";
						objtrans.TXN_ID = root.GSWS_ID;
						objtrans.DEPT_ID = "3302";
						objtrans.DEPT_TXN_ID = data.RNR;
						objtrans.STATUS_CODE = "01";
						objtrans.REMARKS = JsonConvert.SerializeObject(data);

						DataTable dt = new transactionHelper().transactionInsertion(objtrans);
						if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
						{
							obj.Status = 100;
							obj.Details = data;
						}
						else
						{
							obj.Status = 100;
							obj.Details = data;
						}
					}
					catch (Exception ex)
					{
						string mappath = HttpContext.Current.Server.MapPath("CommerialExceptionLogs");
						Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From LoadDepartments:" + ex.Message.ToString() + data));
						obj.Status = 100;
						obj.Details = data;
                        Common_CommercialTax_Error(ex.Message.ToString(), "https://apct.gov.in/pspt/api/registration/ins_reg_dtls", "2");
                    }
				}
				else
				{
					obj.Status = 100;
					obj.Details = data;
				}

                return obj;
            }
            catch (Exception ex)
            {
				string mappath = HttpContext.Current.Server.MapPath("CommerialExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error FroM Submit:" + ex.Message.ToString()));

				obj.Status = 102;
                obj.Reason = ThirdpartyMessage;
                Common_CommercialTax_Error(ex.Message.ToString(), "https://apct.gov.in/pspt/api/registration/ins_reg_dtls", "2");
                return obj;
            }

        }

        public dynamic GetAppStatus_Helper(PTStatus root)
        {
            dynamic obj = new ExpandoObject();
            var result = true;
            try
            {
                if (!string.IsNullOrEmpty(root.rnr))
                    result = gswsBackendAPI.DL.CommonHel.Utils.IsAlphaNumeric(root.rnr);

                if (result)
                {
                    var val = PostData("https://apct.gov.in/pspt/api/Alert/get_all_alerts", root);
                    var data = GetSerialzedData<dynamic>(val);

                    obj.Status = 100;
                    obj.Details = data;

                    return obj;
                }
                else
                {
                    obj.Status = 102;
                    obj.Reason = "Special Characters Are Not Allowed";
                    return obj;
                }
            }
            catch (Exception ex)
            {
                obj.Status = 102;
                obj.Reason = "Error Occured While Getting Application Status";
                Common_CommercialTax_Error(ex.Message.ToString(), "https://apct.gov.in/pspt/api/Alert/get_all_alerts", "2");
                return obj;
            }

        }

        public dynamic GetDataByRNR_Helper(DocCls root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                var result = true;
                if (!string.IsNullOrEmpty(root.tin))
                    result = gswsBackendAPI.DL.CommonHel.Utils.IsAlphaNumeric(root.tin);
                if (result)
                {
                    var val = PostData("https://apct.gov.in/pspt/api/registration/get_dtls", root);
                    var data = GetSerialzedData<dynamic>(val);

                    obj.Status = 100;
                    obj.Details = data;
                }
                else
                {
                    obj.Status = 102;
                    obj.Reason = "Special Characters Are Not Allowed";
                }

                return obj;
            }
            catch (Exception ex)
            {
                obj.Status = 102;
                obj.Reason = ThirdpartyMessage;
                Common_CommercialTax_Error(ex.Message.ToString(), "https://apct.gov.in/pspt/api/registration/get_dtls", "2");
                return obj;
            }

        }

        public dynamic SubmitEditData_Helper(dynamic root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                var val = PostData("https://apct.gov.in/pspt/api/registration/mod_reg_dtls", root);
                var data = GetSerialzedData<dynamic>(val);

                obj.Status = 100;
                obj.Details = data;

                return obj;
            }
            catch (Exception ex)
            {
                obj.Status = 102;
                obj.Reason = ThirdpartyMessage;
                Common_CommercialTax_Error(ex.Message.ToString(), "https://apct.gov.in/pspt/api/registration/mod_reg_dtls", "2");
                return obj;
            }

        }

        public dynamic GetReturnsData_Helper(DocCls root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                var result = true;
                if (!string.IsNullOrEmpty(root.tin))
                    result = gswsBackendAPI.DL.CommonHel.Utils.IsAlphaNumeric(root.tin);
                if (result)
                {
                    var val = PostData("https://apct.gov.in/pspt/api/return/get_ret_dtls", root);
                    var data = GetSerialzedData<dynamic>(val);

                    obj.Status = 100;
                    obj.Details = data;
                }
                else
                {
                    obj.Status = 102;
                    obj.Reason = "Special Characters Are Not Allowed";
                }

                return obj;
            }
            catch (Exception ex)
            {
                obj.Status = 102;
                obj.Reason = ThirdpartyMessage;
                Common_CommercialTax_Error(ex.Message.ToString(), "https://apct.gov.in/pspt/api/return/get_ret_dtls", "2");
                return obj;
            }

        }

        public dynamic SubmitReturnsData_Helper(DocCls root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                var val = PostData("https://apct.gov.in/pspt/api/return/ins_ret_dtls", root);
                var data = GetSerialzedData<dynamic>(val);

                obj.Status = 100;
                obj.Details = data;

                return obj;
            }
            catch (Exception ex)
            {
                obj.Status = 102;
                obj.Reason = ThirdpartyMessage;
                Common_CommercialTax_Error(ex.Message.ToString(), "https://apct.gov.in/pspt/api/return/ins_ret_dtls", "2");
                return obj;
            }

        }

        public dynamic GetRCData_Helper(DocCls root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                var result = true;
                if (!string.IsNullOrEmpty(root.tin))
                    result = gswsBackendAPI.DL.CommonHel.Utils.IsAlphaNumeric(root.tin);
                if (result)
                {
                    var val = PostData("https://apct.gov.in/pspt/api/registration/get_rc", root);
                    var data = GetSerialzedData<dynamic>(val);

                    obj.Status = 100;
                    obj.Details = data;
                }
                else
                {
                    obj.Status = 102;
                    obj.Reason = "Special Characters Are Not Allowed";
                }

                return obj;
            }
            catch (Exception ex)
            {
                obj.Status = 102;
                obj.Reason = ThirdpartyMessage;
                Common_CommercialTax_Error(ex.Message.ToString(), "https://apct.gov.in/pspt/api/registration/get_rc", "2");
                return obj;
            }

        }

        public static bool Common_CommercialTax_Error(string msg, string url, string etype)
        {
            ExceptionDataModel objex = new ExceptionDataModel();
            try
            {
                objex.E_DEPTID = DepartmentEnum.Department.Revenue.ToString();
                objex.E_HODID = DepartmentEnum.HOD.Commercial_Taxes.ToString();
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