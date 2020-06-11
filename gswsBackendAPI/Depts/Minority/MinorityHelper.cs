using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace gswsBackendAPI.Depts.Minority
{
    public class MinorityHelper : CommonSPHel
    {

        minorityservice.MinorityService _minorityservice = new minorityservice.MinorityService();

        MinorityModel _response = new MinorityModel();
        public dynamic GetWomenDivorcedDetails(dynamic objCert)
        {
            try
            {
                string MCNO = objCert.MCNO;
                var data = _minorityservice.GetWomenDivorcedDetails(MCNO);

                if (data != null && data!="")
                {
                    _response.Status = "Success";
                    _response.data = JsonConvert.DeserializeObject<List<WomenDiverced>>(data);

                }
                else
                {
                    _response.Status = "No Data Found";
                    _response.data = "Invalid Certificate Number";
                }
            }
            catch (Exception ex)
            {
                string mappath = HttpContext.Current.Server.MapPath("MinorityExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Data API:" + ex.Message.ToString()));

                _response.Status = "Failed";
                _response.data = ThirdpartyMessage;
            }
            return _response;
        }

        public dynamic GetHonorariumToImamAndMouzansDetails(dynamic objCert)
        {
            try
            {
                string BeneficiaryCode = objCert.BeneficiaryCode;
                var data = _minorityservice.GetHonorariumToImamAndMouzansDetails(BeneficiaryCode);

                if (data != null && data != "")
                {
                    _response.Status = "Success";
                    _response.data = JsonConvert.DeserializeObject<List<IMAMANDMOUZANS>>(data);

                }
                else
                {
                    _response.Status = "No Data Found";
                    _response.data = "Invalid Certificate Number";
                }
            }
            catch (Exception ex)
            {
                string mappath = HttpContext.Current.Server.MapPath("MinorityExceptionLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Data API:" + ex.Message.ToString()));

                _response.Status = "Failed";
                _response.data = ThirdpartyMessage;
            }
            return _response;
        }
    }
}