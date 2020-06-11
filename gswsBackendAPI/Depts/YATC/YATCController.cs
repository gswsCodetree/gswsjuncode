using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using gswsBackendAPI.DL.DataConnection;

namespace gswsBackendAPI.Depts.YATC
{
    [RoutePrefix("api/YATC")]
    public class YATCController : ApiController
    {
        dynamic CatchData = new ExpandoObject();
        YATCHelper YATChel = new YATCHelper();


        #region Skill Delelopment Corporation
        //Load Religions
        [HttpPost]
        [Route("SkillReligions")]
        public IHttpActionResult SkillReligions(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                usercred rootobj = JsonConvert.DeserializeObject<usercred>(value);
                List<Hearders> headers = new List<Hearders>();
                //headers.Add(new Hearders { key = "key",value= "RELIGION" });
                return Ok(YATChel.GetMethod("http://103.231.8.28/apssdc_unified/dropdownsettings/getdropdowndatawithoutrefid?key=RELIGION", headers));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = YATCHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        //Load Category
        [HttpPost]
        [Route("SkillCategory")]
        public IHttpActionResult SkillCategory(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                usercred rootobj = JsonConvert.DeserializeObject<usercred>(value);
                List<Hearders> headers = new List<Hearders>();
                //headers.Add(new Hearders { key = "key", value = "CATEGORY" });
                return Ok(YATChel.GetMethod("http://103.231.8.28/apssdc_unified/dropdownsettings/getdropdowndatawithoutrefid?key=CATEGORY", headers));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = YATCHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        //Load Category
        [HttpPost]
        [Route("SkillStates")]
        public IHttpActionResult SkillStates(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                usercred rootobj = JsonConvert.DeserializeObject<usercred>(value);
                List<Hearders> headers = new List<Hearders>();
                //headers.Add(new Hearders { key = "key", value = "STATE" });
                return Ok(YATChel.GetMethod("http://103.231.8.28/apssdc_unified/dropdownsettings/getdropdowndatawithoutrefid?key=STATE", headers));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = YATCHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        //Load Districs
        [HttpPost]
        [Route("SkillDistrics")]
        public IHttpActionResult SkillDistrics(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                inputParams rootobj = JsonConvert.DeserializeObject<inputParams>(value);
                List<Hearders> headers = new List<Hearders>();
                //headers.Add(new Hearders { key = "key", value = rootobj.key });
                //headers.Add(new Hearders { key = "ref_id", value = rootobj.ref_id });
                return Ok(YATChel.GetMethod("http://103.231.8.28/apssdc_unified/dropdownsettings/getdropdowndata?key="+ rootobj.key + "&ref_id="+ rootobj.ref_id, headers));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = YATCHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        //Load SkillConstituency
        [HttpPost]
        [Route("SkillConstituency")]
        public IHttpActionResult SkillConstituency(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                inputParams rootobj = JsonConvert.DeserializeObject<inputParams>(value);
                List<Hearders> headers = new List<Hearders>();
                //headers.Add(new Hearders { key = "key", value = rootobj.key });
                //headers.Add(new Hearders { key = "ref_id", value = rootobj.ref_id });
                return Ok(YATChel.GetMethod("http://103.231.8.28/apssdc_unified/dropdownsettings/getdropdowndata?key=" + rootobj.key + "&ref_id=" + rootobj.ref_id, headers));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = YATCHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        //Load Religions
        [HttpPost]
        [Route("SkillMandals")]
        public IHttpActionResult SkillMandals(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                inputParams rootobj = JsonConvert.DeserializeObject<inputParams>(value);
                List<Hearders> headers = new List<Hearders>();
                //headers.Add(new Hearders { key = "key", value = rootobj.key });
                //headers.Add(new Hearders { key = "ref_id", value = rootobj.ref_id });
                return Ok(YATChel.GetMethod("http://103.231.8.28/apssdc_unified/dropdownsettings/getdropdowndata?key=" + rootobj.key + "&ref_id=" + rootobj.ref_id, headers));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = YATCHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        //Load Religions
        [HttpPost]
        [Route("SkillMuncipality")]
        public IHttpActionResult SkillMuncipality(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                inputParams rootobj = JsonConvert.DeserializeObject<inputParams>(value);
                List<Hearders> headers = new List<Hearders>();
                //headers.Add(new Hearders { key = "key", value = rootobj.key });
                //headers.Add(new Hearders { key = "ref_id", value = rootobj.ref_id });
                return Ok(YATChel.GetMethod("http://103.231.8.28/apssdc_unified/dropdownsettings/getdropdowndata?key=" + rootobj.key + "&ref_id=" + rootobj.ref_id, headers));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = YATCHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        //User Login
        [HttpPost]
        [Route("VerifySkillCanLogin")]
        public IHttpActionResult VerifySkillCanLogin(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                usercred rootobj = JsonConvert.DeserializeObject<usercred>(value);
                return Ok(YATChel.VerifySkillCanLogin(rootobj));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = YATCHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        //User Login
        [HttpPost]
        [Route("SkillCandidateReg")]
        public IHttpActionResult SkillCandidateReg(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                CanReg rootobj = JsonConvert.DeserializeObject<CanReg>(value);
                return Ok(YATChel.SkillCandidateReg(rootobj));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = YATCHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        //Load Up Comming Jobs
        [HttpPost]
        [Route("UpCommingJobs")]
        public IHttpActionResult UpCommingJobs(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                inputParams rootobj = JsonConvert.DeserializeObject<inputParams>(value);
                List<Hearders> headers = new List<Hearders>();
                //headers.Add(new Hearders { key = "key", value = rootobj.key });
                //headers.Add(new Hearders { key = "ref_id", value = rootobj.ref_id });
                //return Ok(YATChel.GetMethod("http://103.231.8.28/apssdc_unified/api/mobile/getAllUpcomingJobDetails", headers));
                return Ok(YATChel.GetMethod("http://103.231.8.28/apssdc_unified/api/mobile/getUpComingJobMela?infoId=1", headers));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = YATCHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        //LoadApply For Jobs
        [HttpPost]
        [Route("ApplyForJobs")]
        public IHttpActionResult ApplyForJobs(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                JobsCls rootobj = JsonConvert.DeserializeObject<JobsCls>(value);
                return Ok(YATChel.ApplyForJobs(rootobj));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = YATCHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }
        }

        //Load UpcommingJobs
        [HttpPost]
        [Route("UpCommingBatches")]
        public IHttpActionResult UpCommingBatches(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                inputParams rootobj = JsonConvert.DeserializeObject<inputParams>(value);
                List<Hearders> headers = new List<Hearders>();
                return Ok(YATChel.GetMethod("http://103.231.8.28/apssdc_unified/api/student/getBatches?appKey=" + rootobj.appKey + "&district=" + rootobj.district, headers));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = YATCHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        //LoadApply For Batch
        [HttpPost]
        [Route("ApplyForBatch")]
        public IHttpActionResult ApplyForBatch(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                BatchesCls rootobj = JsonConvert.DeserializeObject<BatchesCls>(value);
                List<Hearders> headers = new List<Hearders>();
                return Ok(YATChel.GetMethod("http://103.231.8.28/apssdc_unified/api/mobile/student/applyforbatch?appKey=" + rootobj.appKey + "&tcId=" + rootobj.tcId + "&programId=" + rootobj.programId + "&applicationType=" + rootobj.applicationType + "&batchId=" + rootobj.batchId + "&userMasterId=" + rootobj.userMasterId + "", headers));
                //return Ok(YATChel.ApplyForBatch(rootobj));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = YATCHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }
        }

        //Load Candidate Batches
        [HttpPost]
        [Route("LoadCanJobs")]
        public IHttpActionResult LoadCanJobs(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                inputParams rootobj = JsonConvert.DeserializeObject<inputParams>(value);
                List<Hearders> headers = new List<Hearders>();
                return Ok(YATChel.GetMethod("http://103.231.8.28/apssdc_unified/api/getjobs?appKey=" + rootobj.appKey + "&userMasterId=" + rootobj.userMasterId, headers));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = YATCHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        //Load Candidate Batches
        [HttpPost]
        [Route("LoadCanBatches")]
        public IHttpActionResult LoadCanBatches(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                inputParams rootobj = JsonConvert.DeserializeObject<inputParams>(value);
                List<Hearders> headers = new List<Hearders>();
                return Ok(YATChel.GetMethod("http://103.231.8.28/apssdc_unified/api/mobile/getAppliedBatches?userId=" + rootobj.userMasterId, headers));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = YATCHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        //Get Appication Status
        [HttpPost]
        [Route("GetSkillAppStatus")]
        public IHttpActionResult GetSkillAppStatus(dynamic data)
        {
            string value = token_gen.Authorize_aesdecrpty(data);
            try
            {
                
                //string value = JsonConvert.SerializeObject(data);
                inputParams rootobj = JsonConvert.DeserializeObject<inputParams>(value);
                List<Hearders> headers = new List<Hearders>();
                return Ok(YATChel.GetMethod("http://103.231.8.28/apssdc_unified/api/mobile/getStatusOfCandidate?userMasterId=" + rootobj.userMasterId + "&batchId=" + rootobj.batchId + "&key=" + rootobj.key, headers));
            }
            catch (Exception ex)
            {
                CatchData.Status = 102;
                CatchData.Reason = YATCHelper.ThirdpartyMessage;
                return Ok(CatchData);
            }

        }

        #endregion
    }
}
