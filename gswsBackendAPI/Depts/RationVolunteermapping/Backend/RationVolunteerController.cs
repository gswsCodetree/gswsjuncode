using gswsBackendAPI.Depts.PensionVolunteerMapping.Backend;
using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static gswsBackendAPI.Depts.RationVolunteermapping.Backend.ResponseModel;

namespace gswsBackendAPI.Depts.RationVolunteermapping.Backend
{
    [RoutePrefix("api/RationVolunteer")]
    public class RationVolunteerController : ApiController
    {


        [HttpPost]
        [Route("loadRationMembers")]
        public IHttpActionResult loadRationMembers(dynamic data)
        {
            RationVolunteerHelper RationHelper = new RationVolunteerHelper();
            dynamic objdata = new ExpandoObject();
            RationInputs rootobj;

            try
            {
                string value = token_gen.Authorize_aesdecrpty(data);
                rootobj = JsonConvert.DeserializeObject<RationInputs>(value);
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
                return objdata;
            }

            return Ok(RationHelper.loadRationMembers(rootobj));
        }


        [HttpPost]
        [Route("loadClusters")]
        public IHttpActionResult loadClusters(dynamic data)
        {
            RationVolunteerHelper RationHelper = new RationVolunteerHelper();
            dynamic objdata = new ExpandoObject();
            RationInputs rootobj;
            try
            {
                string value = token_gen.Authorize_aesdecrpty(data);
                rootobj = JsonConvert.DeserializeObject<RationInputs>(value);
            }

            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
                return objdata;
            }
            return Ok(RationHelper.loadClusters(rootobj));
        }


        [HttpPost]
        [Route("assignRationToCluster")]
        public IHttpActionResult assignRationToCluster(dynamic data)
        {
            RationVolunteerHelper RationHelper = new RationVolunteerHelper();
            dynamic objdata = new ExpandoObject();
            RationInputs rootobj;
            try
            {
                string value = JsonConvert.SerializeObject(data);
                string mappath = HttpContext.Current.Server.MapPath("assignRationToClusterLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, value));
                rootobj = JsonConvert.DeserializeObject<RationInputs>(value);
            }

            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
                return objdata;
            }
            return Ok(RationHelper.assignRationToCluster(rootobj));
        }

        [HttpPost]
        [Route("assignRiceCardToCluster")]
        public IHttpActionResult assignRiceCardToCluster(dynamic data)
        {
            RationVolunteerHelper RationHelper = new RationVolunteerHelper();
            dynamic objdata = new ExpandoObject();
            RationInputs rootobj;
            try
            {
                string value = token_gen.Authorize_aesdecrpty(data);
                string mappath = HttpContext.Current.Server.MapPath("assignRationToClusterLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, value));
                rootobj = JsonConvert.DeserializeObject<RationInputs>(value);
            }

            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
                return objdata;
            }
            return Ok(RationHelper.assignRiceCardToCluster(rootobj));
        }

        [HttpPost]
        [Route("unassignRationToCluster")]
        public IHttpActionResult unassignRationToCluster(dynamic data)
        {
            RationVolunteerHelper RationHelper = new RationVolunteerHelper();
            dynamic objdata = new ExpandoObject();
            RationInputs rootobj;
            try
            {
                string value = JsonConvert.SerializeObject(data);
                string mappath = HttpContext.Current.Server.MapPath("unassignRationToClusterLogs");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, value));
                rootobj = JsonConvert.DeserializeObject<RationInputs>(value);
            }

            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
                return objdata;
            }
            return Ok(RationHelper.unassignRationToCluster(rootobj));
        }

        [HttpPost]
        [Route("secList")]
        public IHttpActionResult secList(dynamic data)
        {
            RationVolunteerHelper RationHelper = new RationVolunteerHelper();
            dynamic objdata = new ExpandoObject();
            RationInputs rootobj;
            try
            {
                string value = token_gen.Authorize_aesdecrpty(data);
                rootobj = JsonConvert.DeserializeObject<RationInputs>(value);
            }

            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
                return objdata;
            }
            return Ok(RationHelper.secList(rootobj));
        }

        [HttpPost]
        [Route("SearchRiceCard")]
        public IHttpActionResult SearchRiceCard(dynamic data)
        {
            RationVolunteerHelper RationHelper = new RationVolunteerHelper();
            dynamic objdata = new ExpandoObject();
            RationInputs rootobj;
            try
            {
                string value = token_gen.Authorize_aesdecrpty(data);
                rootobj = JsonConvert.DeserializeObject<RationInputs>(value);
            }

            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
                return objdata;
            }
            return Ok(RationHelper.SearchRiceCard(rootobj));
        }


        [HttpPost]
        [Route("reqRiceCardToCluster")]
        public IHttpActionResult reqRiceCardToCluster(dynamic data)
        {
            RationVolunteerHelper RationHelper = new RationVolunteerHelper();
            dynamic objdata = new ExpandoObject();
            RationInputs rootobj;
            try
            {
                string value = token_gen.Authorize_aesdecrpty(data);
                rootobj = JsonConvert.DeserializeObject<RationInputs>(value);
            }

            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
                return objdata;
            }
            return Ok(RationHelper.reqRiceCardToCluster(rootobj));
        }

    }
}
