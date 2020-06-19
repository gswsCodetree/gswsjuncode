using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using gswsBackendAPI.DL.CommonHel;

namespace gswsBackendAPI.Depts.Services
{
    public class ServicesHelper : ServicesSPHelper
    {
        public dynamic GetNPCIStatus_helper(AppStatus root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                DataTable data = GetNPCIStatus_data_helper(root);
                if (data != null)
                {
                    obj.Status = 100;
                    obj.Reason = "Data Loaded Successfully.";
                    obj.Details = data;
                }
                else
                {
                    obj.Status = 101;
                    obj.Reason = "No Data Found";
                }

            }
            catch (Exception ex)
            {
                obj.Status = 102;
                obj.Reason = "Error Occured While Getting Data ";

            }

            return obj;

        }


        #region "Integrated and Income Certificates Download"
        public dynamic Certificate_Download(string Certid, string Cert_type)
        {
            dynamic objdynamic = new ExpandoObject();
            try
            {
                var obj = new { strIntegratedID = Certid, CertType = Cert_type };
                var CertData = PostData_Auth_Certificate("http://ysrpk.ap.gov.in/Services/api/Mobile/Get_MeesevaCert_PDF_File_Sachivalayam", obj);
                var ResultData = JsonConvert.DeserializeObject(CertData);

                objdynamic.Status = ResultData.Status;
                objdynamic.Reason = ResultData.Reason;
                objdynamic.Data = ResultData.PDFPath;


            }
            catch (Exception ex)
            {
                Common_Services_Error(ex.Message.ToString(), "http://ysrpk.ap.gov.in/Services/api/Mobile/Get_MeesevaCert_PDF_File_Sachivalayam", "2");
                objdynamic.Status = "Failure";
                objdynamic.Reason = ThirdpartyMessage;
                objdynamic.Data = "";
            }

            return objdynamic;
        }


        public dynamic PostData_Auth_Certificate(string url, dynamic jsonData)
        {
            var da = String.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    string strKey = "fe782860648f02893ac3438c5e9565f5";
                    Random rn = new Random();
                    int RandomNumber = rn.Next(100000, 999999);
                    client.BaseAddress = new Uri("http://ysrpk.ap.gov.in/Services/api/Mobile/");//
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string strpswd = sha256_hash(strKey + "sachivalayam@123" + RandomNumber.ToString());
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Username", "sachivalayam");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Password", strpswd);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Key", strKey);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("RandomNumber", RandomNumber.ToString());

                    clsMeesevaCert_PDF_File objclsMeesevaCert_PDF_File = new clsMeesevaCert_PDF_File();
                    objclsMeesevaCert_PDF_File.strIntegratedID = jsonData.strIntegratedID;
                    objclsMeesevaCert_PDF_File.strType = jsonData.CertType;


                    string strResult = JsonConvert.SerializeObject(objclsMeesevaCert_PDF_File);

                    var content = new StringContent(strResult, Encoding.UTF8, "application/json");

                    var response = client.PostAsync("Get_MeesevaCert_PDF_File_Sachivalayam", content).Result;

                    da = response.Content.ReadAsStringAsync().Result;

                }
            }
            catch (WebException wex)
            {
                string mappath = HttpContext.Current.Server.MapPath("ServicesExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error From GetApplicantStatus:" + wex.Message.ToString()));

                throw wex;
            }

            return da;
        }


        public static String sha256_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        class clsMeesevaCert_PDF_File
        {
            public string strIntegratedID { set; get; }
            public string strType { set; get; }
        }

        #endregion

        #region Textile
        public dynamic GetTextileData(AppStatus root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                DataTable data = GetTextileData_helper(root);
                if (data != null)
                {
                    obj.Status = "Success";
                    obj.Reason = "Data Loaded Successfully.";
                    obj.Details = data;
                }
                else
                {
                    obj.Status = "Failure";
                    obj.Reason = "No Data Found";
                }

            }
            catch (Exception ex)
            {
                obj.Status = "Failure";
                obj.Reason = GetException(ex.Message);

            }

            return obj;

        }

        //Save Textile Data
        public dynamic SaveTextileData(TextileModel root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                string data = SaveTextileData_helper(root);
                if (data == "Success")
                {
                    obj.Status = "Success";
                    obj.Reason = "Data Inserted Successfully.";
                    obj.Details = data;
                }
                else
                {
                    obj.Status = "Failure";
                    obj.Reason = "Data Insertion Failed";
                }

            }
            catch (Exception ex)
            {
                obj.Status = "Failure";
                obj.Reason = GetException(ex.Message);

            }

            return obj;

        }
        #endregion

        #region Assettracking
        //Load Districts
        public dynamic LoadDistricts(Assetmodel oj)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                DataTable dt = LoadDepartments_helper(oj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    obj.Status = "Success";
                    obj.Reason = "";
                    obj.Details = dt;
                }
                else
                {
                    obj.Status = "Failure";
                    obj.Reason = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                obj.Status = "Failure";
                obj.Reason = GetException(ex.Message);
                return obj;
            }
            return obj;
        }

        //Loading Secratariats Data
        public dynamic LoadSeccDetails(Assetmodel oj)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                DataTable dt = seccdata_helper(oj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    obj.Status = "Success";
                    obj.Reason = "";
                    obj.Details = dt;
                }
                else
                {
                    obj.Status = "Failure";
                    obj.Reason = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                obj.Status = "Failure";
                obj.Reason = GetException(ex.Message);
                return obj;
            }
            return obj;
        }


        //Save Data
        public dynamic SaveSystemData(AssetTracking root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                string res = SaveSystemDataSpHelper(root);
                if (res == "Success")
                {
                    obj.Status = "Success";
                    obj.Reason = "Data Inserted Successfully.";
                }
                else
                {
                    obj.Status = "Failure";
                    obj.Reason = "No Data Found";
                }

            }
            catch (Exception ex)
            {
                obj.Status = "Failure";
                obj.Reason = GetException(ex.Message);

            }

            return obj;

        }

        //Reports
        public dynamic LoadRptDistrictData(Assetmodel oj)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                DataTable dt = LoadRptDistrictSPData(oj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    obj.Status = "Success";
                    obj.Reason = "";
                    obj.Details = dt;
                }
                else
                {
                    obj.Status = "Failure";
                    obj.Reason = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                obj.Status = "Failure";
                obj.Reason = GetException(ex.Message);
                return obj;
            }
            return obj;
        }

        //Hardware Issues Compnent Loading
        public dynamic Loadhwcomponent(Assetmodel oj)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                DataTable dt = Loadhwcomponent_helper(oj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    obj.Status = "Success";
                    obj.Reason = "";
                    obj.Details = dt;
                }
                else
                {
                    obj.Status = "Failure";
                    obj.Reason = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                obj.Status = "Failure";
                obj.Reason = GetException(ex.Message);
                return obj;
            }
            return obj;
        }

        //Save Hardware Issues
        public dynamic SaveHardwareIssue(Assetmodel root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                DataTable dt = SaveHardwareIssueSpHelper(root);
                if (dt != null && dt.Rows.Count > 0 && (dt.Rows[0]["REASON"] ?? "").ToString().Contains("Asset Hardware Issue Registered Successfully with UID"))
                {

                    obj.Status = "Success";
                    obj.Reason = "Data Inserted Successfully.Your Token ID is " + "  " + (dt.Rows[0]["REASON"] ?? "").ToString().Replace("Asset Hardware Issue Registered Successfully with UID:", "");
                }
                else
                {
                    obj.Status = "Failure";
                    obj.Reason = dt.Rows[0]["REASON"].ToString();
                }

            }
            catch (Exception ex)
            {
                obj.Status = "Failure";
                obj.Reason = GetException(ex.Message);

            }

            return obj;

        }

        #endregion

        public dynamic GetMethod(string url)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                dynamic val = GetData(url);
                obj.Status = 100;
                obj.Reason = "Data Loaded Successfully.";
                obj.Details = val;
                return obj;
            }
            catch (Exception ex)
            {
                obj.Status = 102;
                obj.Reason = ThirdpartyMessage;
                return obj;
            }

        }

        #region MUID
        public dynamic GetMUIDAppStatus(MUIDCls root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                TransCls tobj = new TransCls();
                tobj.txn_id = "";
                tobj.application_number = root.application_number;

                var val = PostData("https://" + root.ulb + "-uat.egovernments.org/wardsecretary/transaction/status", tobj);
                var data = GetSerialzedData<dynamic>(val);
                obj.Status = 100;
                obj.Reason = "Data Loaded Successfully.";
                obj.Details = data;
                return obj;
            }
            catch (Exception ex)
            {
                obj.Status = 102;
                obj.Reason = ThirdpartyMessage;
                return obj;
            }

        }
        #endregion

        public dynamic GetMeesevaAppStatus_helper(TransCls root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                DataTable data = GetMeesevaAppStatus_data_helper(root);
                obj.Status = 100;
                obj.Reason = "Data Loaded Successfully.";
                obj.Details = data;
                return obj;
            }
            catch (Exception ex)
            {
                obj.Status = 102;
                obj.Reason = ThirdpartyMessage;
                return obj;
            }

        }
        public bool Common_Services_Error(string msg, string url, string etype)
        {
            ExceptionDataModel objex = new ExceptionDataModel();
            try
            {
                objex.E_DEPTID = DepartmentEnum.Department.Social_Tribal_Welfare.ToString();
                objex.E_HODID = DepartmentEnum.HOD.Social_Welfare.ToString();
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
    }
}