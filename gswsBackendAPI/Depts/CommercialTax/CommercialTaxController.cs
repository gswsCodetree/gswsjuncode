using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using gswsBackendAPI.DL.CommonHel;
using System.Configuration;
using System.IO;
using System.Web;
using System.Threading.Tasks;

namespace gswsBackendAPI.Depts.CommercialTax
{
    [RoutePrefix("api/CT")]
    public class CommercialTaxController : ApiController
    {
        dynamic CatchData = new ExpandoObject();
        CommercialTaxHelper cthel = new CommercialTaxHelper();

        #region Commercial Tax
        //Get Deatils by LTIN / 
        [HttpPost]
        [Route("GetDetailsByTIN")]
        public IHttpActionResult GetDetailsByTIN(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                DocCls rootobj = JsonConvert.DeserializeObject<DocCls>(value);
                return Ok(cthel.GetDataByTIN_Helper(rootobj));

            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
				CatchData.Reason = CommercialTaxHelper.ThirdpartyMessage;

				return Ok(CatchData);
            }
        }


		[HttpGet]
		[Route("CommercialMaster")]
		public IHttpActionResult CommercialMaster(dynamic data)
		{
			dynamic objdata = new ExpandoObject();
			try
			{
				string CommercialPTCountriesPath = ConfigurationManager.AppSettings["fMastername"].ToString()+ "MasterTextFiles\\CommercialPTCountries.txt";

				string CommercialPTStatesPath = ConfigurationManager.AppSettings["fMastername"].ToString() + "MasterTextFiles\\CommercialPTStates.txt";

				string CommercialPTbanksPath = ConfigurationManager.AppSettings["fMastername"].ToString() + "MasterTextFiles\\CommercialmasterPTbanks.txt";

				string json = File.ReadAllText(CommercialPTCountriesPath);
				string json1 = File.ReadAllText(CommercialPTStatesPath);
				string json2 = File.ReadAllText(CommercialPTbanksPath);

				objdata.Status = 100;
				objdata.PTCountries = json;
				objdata.PTStates = json1;
				objdata.PTBanks = json2;

			}
			catch (Exception ex)
			{
				objdata.status = 500;
				objdata.result = CommercialTaxHelper.ThirdpartyMessage;
			}
			return Ok(objdata);
		}




		//Upload Documents
		[HttpPost]
        [Route("UploadDoc")]
        public IHttpActionResult UploadDoc(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                DocCls rootobj = JsonConvert.DeserializeObject<DocCls>(value);
                bool IsPDF = true;
                if (!string.IsNullOrEmpty(rootobj.data))
                {
                    byte[] PdfBytes = Convert.FromBase64String(rootobj.data);
                    if (!Utils.IsValidPDF(PdfBytes) && !Utils.IsValidImage(PdfBytes))
                        IsPDF = false;
                }
                if (IsPDF)
                    return Ok(cthel.UploadDoc_Helper(rootobj));
                else
                {
                    CatchData.Status = 102;
                    CatchData.Reason = "Invalid File Format";
                    return Ok(CatchData);
                }
               
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = CommercialTaxHelper.ThirdpartyMessage;
				return Ok(CatchData);
            }
        }

        //Submit Data
        [HttpPost]
        [Route("SubmitData")]
        public IHttpActionResult SubmitData(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
				string mappath = HttpContext.Current.Server.MapPath("ProfessionTaxData");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, value));
				//string value = JsonConvert.SerializeObject(data);
				dynamic rootobj = JsonConvert.DeserializeObject<dynamic>(value);
                return Ok(cthel.SubmitData_Helper(rootobj));

            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = CommercialTaxHelper.ThirdpartyMessage;
				return Ok(CatchData);
            }
        }

        //Get Application Status
        [HttpPost]
        [Route("GetAppStatus")]
        public IHttpActionResult GetAppStatus(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                PTStatus rootobj = JsonConvert.DeserializeObject<PTStatus>(value);
                return Ok(cthel.GetAppStatus_Helper(rootobj));

            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = CommercialTaxHelper.ThirdpartyMessage;
				return Ok(CatchData);
            }
        }

        //Get Deatils by RNR / 
        [HttpPost]
        [Route("GetDetailsByRNR")]
        public IHttpActionResult GetDetailsByRNR(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                DocCls rootobj = JsonConvert.DeserializeObject<DocCls>(value);
                return Ok(cthel.GetDataByRNR_Helper(rootobj));

            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = CommercialTaxHelper.ThirdpartyMessage;
				return Ok(CatchData);
            }
        }

        //Submit Data
        [HttpPost]
        [Route("SubmitEditData")]
        public IHttpActionResult SubmitEditData(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                dynamic rootobj = JsonConvert.DeserializeObject<dynamic>(value);
                return Ok(cthel.SubmitEditData_Helper(rootobj));

            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = CommercialTaxHelper.ThirdpartyMessage;
				return Ok(CatchData);
            }
        }

        //Get Return Data
        [HttpPost]
        [Route("GetReturnsData")]
        public IHttpActionResult GetReturnsData(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                DocCls rootobj = JsonConvert.DeserializeObject<DocCls>(value);
                return Ok(cthel.GetReturnsData_Helper(rootobj));

            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = CommercialTaxHelper.ThirdpartyMessage;
				return Ok(CatchData);
            }
        }

        //Submit Return Data
        [HttpPost]
        [Route("SubmitReturnsData")]
        public IHttpActionResult SubmitReturnsData(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                DocCls rootobj = JsonConvert.DeserializeObject<DocCls>(value);
                return Ok(cthel.SubmitReturnsData_Helper(rootobj));

            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = CommercialTaxHelper.ThirdpartyMessage;
				return Ok(CatchData);
            }
        }

        //Get RC Data
        [HttpPost]
        [Route("GetRCData")]
        public IHttpActionResult GetRCData(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                DocCls rootobj = JsonConvert.DeserializeObject<DocCls>(value);
                return Ok(cthel.GetRCData_Helper(rootobj));

            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = CommercialTaxHelper.ThirdpartyMessage;
				return Ok(CatchData);
            }
        }

        #endregion

    }
}
