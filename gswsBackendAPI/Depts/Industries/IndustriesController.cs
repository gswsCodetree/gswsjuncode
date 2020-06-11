using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using gswsBackendAPI.DL.DataConnection;
using gswsBackendAPI.DL.CommonHel;

namespace gswsBackendAPI.Depts.Industries
{
    [RoutePrefix("api/Industries")]
    public class IndustriesController : ApiController
    {
        dynamic CatchData = new ExpandoObject();
        IndustriesHelper Indhel = new IndustriesHelper();

        #region YSR Navodayam

        //Load Districts
        [HttpPost]
        [Route("LoadDistricts")]
        public IHttpActionResult LoadDistricts(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                usercred rootobj = JsonConvert.DeserializeObject<usercred>(value);
                return Ok(Indhel.GetMethod("http://125.17.121.166:8080/SDPIntegrationServices/YSRNavodayamService/GetDistrictDetails"));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = IndustriesHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        //Load Mandals
        [HttpPost]
        [Route("LoadMandals")]
        public IHttpActionResult LoadMandals(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                usercred rootobj = JsonConvert.DeserializeObject<usercred>(value);
                return Ok(Indhel.GetMethod("http://125.17.121.166:8080/SDPIntegrationServices/YSRNavodayamService/GetMandalDetails/" + rootobj.username));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = IndustriesHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        //Load Sectors
        [HttpPost]
        [Route("LoadSectors")]
        public IHttpActionResult LoadSectors(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                usercred rootobj = JsonConvert.DeserializeObject<usercred>(value);
                return Ok(Indhel.GetMethod("http://125.17.121.166:8080/SDPIntegrationServices/YSRNavodayamService/GetSectorDetails"));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = IndustriesHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        //Load Sectors
        [HttpPost]
        [Route("SearchIFSCCode")]
        public IHttpActionResult SearchIFSCCode(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                usercred rootobj = JsonConvert.DeserializeObject<usercred>(value);
                return Ok(Indhel.GetMethod(" http://125.17.121.166:8080/SDPIntegrationServices/YSRNavodayamService/GetBankDetails/" + rootobj.username));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = IndustriesHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        //Load Sectors
        [HttpPost]
        [Route("SaveOTRReg")]
        public IHttpActionResult SaveOTRReg(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                NavodayamOTR rootobj = JsonConvert.DeserializeObject<NavodayamOTR>(value);
				bool IsPDF = true;
				if (!string.IsNullOrEmpty(rootobj.UAM_Enclosure))
				{
					byte[] PdfBytes = Convert.FromBase64String(rootobj.UAM_Enclosure);
					if (!Utils.IsValidPDF(PdfBytes))
						IsPDF = false;
				}

				if (!string.IsNullOrEmpty(rootobj.Other_Enclosure))
				{
					byte[] PdfBytes = Convert.FromBase64String(rootobj.UAM_Enclosure);
					if (!Utils.IsValidPDF(PdfBytes))
						IsPDF = false;
				}
				if (IsPDF)
					return Ok(Indhel.SaveOTRReg_helper(rootobj));
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
                CatchData.Reason = IndustriesHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        #endregion
    }
}
