using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using gswsBackendAPI.DL.CommonHel;
using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
namespace gswsBackendAPI.Depts.Home.Backend
{
    [RoutePrefix("api/HomeWeb")]
    public class HomeWebController : ApiController
    {
        HomeWebLHMS _lhms = new HomeWebLHMS();

        dynamic RData = new ExpandoObject();

        [Route("LHMSAPPoliceDashbooard")]
        public IHttpActionResult LHMSAPPoliceDashbooard(dynamic data)
        {
            string jsondata = token_gen.Authorize_aesdecrpty(data);
            try
            {
                LHMSMODEL obj = JsonConvert.DeserializeObject<LHMSMODEL>(jsondata);
                if (obj.Ftype == "1")
                    return Ok(_lhms.GetLHMSCMDashboard());
                else
                    return Ok(_lhms.GetLHMSCMDashboardbydate(obj.Startdate, obj.enddate));

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }

        [Route("EchallanStatus")]
        public IHttpActionResult EchallanStatus(dynamic data)
        {
            string jsondata = token_gen.Authorize_aesdecrpty(data);
            try
            {
                EchallanModel obj = JsonConvert.DeserializeObject<EchallanModel>(jsondata);
                if (Utils.IsAlphaNumeric(obj.VehicleNum))
                    return Ok(_lhms.GetEchallanStatus(obj));
                else
                {
                    RData.status = 102;
                    RData.Reason = "Error Occured While Getting Data.";
                    return Ok(RData);
                }

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }


        [HttpPost]
        [Route("VehicleRecoverStatusCheck")]
        public IHttpActionResult VehicleRecoverStatusCheck(dynamic data)
        {
            string jsondata = token_gen.Authorize_aesdecrpty(data);
            try
            {
                HOMERecoverVehcle obj = JsonConvert.DeserializeObject<HOMERecoverVehcle>(jsondata);
                if (Utils.IsAlphaNumeric(obj.vehicleRegnum))
                    return Ok(_lhms.GetVehicleStatus(obj));
                else
                {
                    RData.status = 102;
                    RData.Reason = "Error Occured While Getting Data.";
                    return Ok(RData);
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }

        [HttpPost]
        [Route("PropertyStatusCheck")]
        public IHttpActionResult PropertyStatusCheck(dynamic data)
        {
            string jsondata = token_gen.Authorize_aesdecrpty(data);
            try
            {
                HOMEKnowCaseStatus obj = JsonConvert.DeserializeObject<HOMEKnowCaseStatus>(jsondata);
                if (Utils.IsAlphaNumeric(obj.firNo))
                    return Ok(_lhms.GetPropertyStatus(obj));
                else
                {
                    RData.status = 102;
                    RData.Reason = "Error Occured While Getting Data.";
                    return Ok(RData);
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }
        [HttpPost]
        [Route("CheckPetitionStatus")]
        public IHttpActionResult CheckPetitionStatus(dynamic data)
        {
            string jsondata = token_gen.Authorize_aesdecrpty(data);
            try
            {
                HOMEPetitionCheck obj = JsonConvert.DeserializeObject<HOMEPetitionCheck>(jsondata);
                if (Utils.IsAlphaNumeric(obj.petitionId) && Utils.IsAlphaNumeric(obj.petName))
                    return Ok(_lhms.GetPetitionStatus(obj));
                else
                {
                    RData.status = 102;
                    RData.Reason = "Error Occured While Getting Data.";
                    return Ok(RData);
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }

        [HttpPost]
        [Route("CheckCaseStatus")]
        public IHttpActionResult CheckCaseStatus(dynamic data)
        {
            string jsondata = token_gen.Authorize_aesdecrpty(data);
            try
            {
                HOMEKnowCaseStatus obj = JsonConvert.DeserializeObject<HOMEKnowCaseStatus>(jsondata);
                if (Utils.IsAlphaNumeric(obj.firNo))
                    return Ok(_lhms.GetCaseStatus(obj));
                else
                {
                    RData.status = 102;
                    RData.Reason = "Error Occured While Getting Data.";
                    return Ok(RData);
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }

        [HttpPost]
        [Route("CheckViewFIRStatus")]
        public IHttpActionResult CheckViewFIRStatus(dynamic data)
        {
            string jsondata = token_gen.Authorize_aesdecrpty(data);
            try
            {
                HOMEFIRStatus obj = JsonConvert.DeserializeObject<HOMEFIRStatus>(jsondata);
                if (Utils.IsAlphaNumeric(obj.firNo))
                    return Ok(_lhms.GetFIRStatus(obj));
                else
                {
                    RData.status = 102;
                    RData.Reason = "Error Occured While Getting Data.";
                    return Ok(RData);
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }


        [HttpPost]
        [Route("CheckArrestParticularsReport")]
        public IHttpActionResult CheckArrestParticularsReport(dynamic data)
        {
            string jsondata = token_gen.Authorize_aesdecrpty(data);
            try
            {
                ArrestParticulars obj = JsonConvert.DeserializeObject<ArrestParticulars>(jsondata);
                if (Utils.IsAlphaNumeric(obj.name) && Utils.IsNumeric(obj.age))
                    return Ok(_lhms.GetArrestParticulars(obj));
                else
                {
                    RData.status = 102;
                    RData.Reason = "Error Occured While Getting Data.";
                    return Ok(RData);
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }
        [HttpPost]
        [Route("CheckWantedcriminals")]
        public IHttpActionResult CheckWantedcriminals(dynamic data)
        {
            string jsondata = token_gen.Authorize_aesdecrpty(data);
            try
            {
                HOMEWantedCriminals obj = JsonConvert.DeserializeObject<HOMEWantedCriminals>(jsondata);
                if (Utils.IsAlphaNumeric(obj.name) && Utils.IsNumeric(obj.age))
                    return Ok(_lhms.GetWantedCriminals(obj));
                else
                {
                    RData.status = 102;
                    RData.Reason = "Error Occured While Getting Data.";
                    return Ok(RData);
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }
        [HttpPost]
        [Route("CheckUnknownBodies")]
        public IHttpActionResult CheckUnknownBodies(dynamic data)
        {
            string jsondata = token_gen.Authorize_aesdecrpty(data);
            try
            {
                HOMEUnknownBodies obj = JsonConvert.DeserializeObject<HOMEUnknownBodies>(jsondata);
                if (Utils.IsAlphaNumeric(obj.heightFeet) && Utils.IsAlphaNumeric(obj.heightInch) && Utils.IsNumeric(obj.age))
                    return Ok(_lhms.GetUnknownBodies(obj));
                else
                {
                    RData.status = 102;
                    RData.Reason = "Error Occured While Getting Data.";
                    return Ok(RData);
                }


            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }
        [HttpPost]
        [Route("CheckMissedPersons")]
        public IHttpActionResult CheckMissedPersons(dynamic data)
        {
            string jsondata = token_gen.Authorize_aesdecrpty(data);
            try
            {
                HOMEMissedKidnapped obj = JsonConvert.DeserializeObject<HOMEMissedKidnapped>(jsondata);
                if (Utils.IsAlphaNumeric(obj.name) && Utils.IsNumeric(obj.age) && Utils.IsAlphaNumeric(obj.heightFeet) && Utils.IsAlphaNumeric(obj.heightInch))
                    return Ok(_lhms.GetMissedPersons(obj));
                else
                {
                    RData.status = 102;
                    RData.Reason = "Error Occured While Getting Data.";
                    return Ok(RData);
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }
        [HttpPost]
        [Route("GetPetitionStatus_Print")]
        public IHttpActionResult GetPetitionStatus_Print(dynamic data)
        {
            string jsondata = token_gen.Authorize_aesdecrpty(data);
            try
            {
                return Ok(_lhms.GetPetition_Print(jsondata));


            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }

        [HttpPost]
        [Route("GetDistricts")]
        public IHttpActionResult GetDistricts(dynamic data)
        {
            string jsondata = token_gen.Authorize_aesdecrpty(data);
            //string jsondata = JsonConvert.SerializeObject(data);
            try
            {
                HomeSPModel val = JsonConvert.DeserializeObject<HomeSPModel>(jsondata);
                return Ok(_lhms.Getdistricts(val));


            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
            }
        }

		#region LHMS

		//Get District Data for LHMS
		[HttpPost]
		[Route("GetLHMSDistrictData")]
		public IHttpActionResult GetLHMSDistrictData(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			//string jsondata = JsonConvert.SerializeObject(data);
			try
			{
				HomeSPModel val = JsonConvert.DeserializeObject<HomeSPModel>(jsondata);
				return Ok(_lhms.GetLHMSDistrictData(val));


			}
			catch (Exception ex)
			{
				throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
			}
		}

		//Registration for LHMS
		[HttpPost]
		[Route("SaveLHMSData")]
		public IHttpActionResult SaveLHMSData(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			//string jsondata = JsonConvert.SerializeObject(data);
			try
			{
				string mappath = HttpContext.Current.Server.MapPath("LHMSSubmitLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, jsondata));
				LHMSMODELNEW val = JsonConvert.DeserializeObject<LHMSMODELNEW>(jsondata);
				return Ok(_lhms.SaveLHMSData(val));


			}
			catch (Exception ex)
			{
				throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
			}
		}

		//Registration for LHMS WatchList
		[HttpPost]
		[Route("SaveLHMSWatchRequestData")]
		public IHttpActionResult SaveLHMSWatchRequestData(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			//string jsondata = JsonConvert.SerializeObject(data);
			try
			{
				LHMSWATCHREQ val = JsonConvert.DeserializeObject<LHMSWATCHREQ>(jsondata);
				return Ok(_lhms.SaveLHMSWatchRequestData(val));


			}
			catch (Exception ex)
			{
				throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
			}
		}

		#endregion

		[HttpPost]
		[Route("DownloadFIR")]
		public IHttpActionResult DownloadFIR(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			try
			{
				HOMEFIRDownload obj = JsonConvert.DeserializeObject<HOMEFIRDownload>(jsondata);
				if (Utils.IsAlphaNumeric(obj.firNo))
				{
					return Ok(_lhms.DownloadFIRFile(obj));
				}

				else
				{
					RData.status = 102;
					RData.Reason = "Error Occured While Getting Data.";
					return Ok(RData);
				}
			}
			catch (Exception ex)
			{
				throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
			}
		}

		[HttpPost]
		[Route("SaveCrimePetition")]
		public IHttpActionResult SaveCrimePetition(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);

			string mappath = HttpContext.Current.Server.MapPath("CrimePetitionData");
			Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Submit Data :" + jsondata));

			try
			{
				dynamic val = JsonConvert.DeserializeObject<dynamic>(jsondata);
				return Ok(_lhms.SaveHome_Crime_Petition(val));


			}
			catch (Exception ex)
			{
				throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
			}
		}

	}
}
