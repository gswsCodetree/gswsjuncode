using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using gswsBackendAPI.Depts.SocialWelfare_Tribal;
using System.Dynamic;
using gswsBackendAPI.DL.DataConnection;
using gswsBackendAPI.DL.CommonHel;

namespace gswsBackendAPI.Depts.PRRD
{
	[RoutePrefix("api/PRRD")]
	public class PRRDController : ApiController
	{
        dynamic CatchData = new ExpandoObject();
		PRRDHelper hlpval = new PRRDHelper();

		#region "fetchTransactionData"
		[HttpPost]
		[Route("fetchPanchayats")]
		public IHttpActionResult fetchPanchayats(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);

			try
			{
				string mappath = HttpContext.Current.Server.MapPath("TaxPaymentsSaveLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, "fetchPanchayats Data :" + value));

				dynamic objCert = JsonConvert.DeserializeObject<dynamic>(value);
				return Ok(hlpval.fetchPanchayatsData(objCert));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failed";
				CatchData.data = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		[HttpPost]
		[Route("GetTaxAppStatus")]
		public IHttpActionResult GetTaxAppStatus(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);

			try
			{
				string mappath = HttpContext.Current.Server.MapPath("TaxPaymentsSaveLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, "GetTaxAppStatus Data :" + value));

				dynamic objCert = JsonConvert.DeserializeObject<dynamic>(value);
				return Ok(hlpval.GetTaxAppStatusData(objCert));
			}
			catch (Exception ex)
			{
				CatchData.Status = "Failed";
				CatchData.data = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}

		[HttpPost]
		[Route("fetchTransactionData")]
		public IHttpActionResult fetchTransactionData(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);

			try
			{
				string mappath = HttpContext.Current.Server.MapPath("TaxPaymentsSaveLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, "fetchTransactionData Data :" + value));

				dynamic objCert = JsonConvert.DeserializeObject<dynamic>(value);
				return Ok(hlpval.fetchTransactionData(objCert));

			}
			catch (Exception ex)
			{
				CatchData.Status = "Failed";
				CatchData.data = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}
		#endregion


		#region "SendTransactionRequest"
		[HttpPost]
		[Route("SendTransactionRequest")]
		public IHttpActionResult SendTransactionRequest(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				string mappath = HttpContext.Current.Server.MapPath("TaxPaymentsSaveLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, "SendTransactionRequest Data :" + value));

				dynamic objCert = JsonConvert.DeserializeObject<dynamic>(value);
				return Ok(hlpval.SendTransactionRequest(objCert));

			}
			catch (Exception ex)
			{
				CatchData.Status = "Failed";
				CatchData.data = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}

		}
		#endregion


		//NREGA CONTROLL
		#region "PostData_Basic_Auth_PaymentDetails"
		[HttpPost]
		[Route("PostData_Basic_Auth_PaymentDetails")]
		public IHttpActionResult PostData_Basic_Auth_PaymentDetails(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				dynamic objdata = JsonConvert.DeserializeObject<dynamic>(value);
				if (Utils.IsAlphaNumeric(objdata.jobCardNo))
					return Ok(hlpval.PostData_Basic_Auth_PaymentDetails(objdata));
				else
				{
					dynamic RData = new ExpandoObject();
					RData.Status = "Failed";
					RData.data = "Special Characters are Not Allowed.";
					return Ok(RData);
				}

			}
			catch (Exception ex)
			{
                CatchData.Status = "Failed";
                CatchData.data = CommonSPHel.ThirdpartyMessage;
                return Ok(CatchData);
            }

		}
		
		[HttpPost]
		[Route("PostData_Basic_Auth_PaymentDetails_BY_UID")]
		public IHttpActionResult PostData_Basic_Auth_PaymentDetails_BY_UID(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				dynamic objCert = JsonConvert.DeserializeObject<dynamic>(value);
				return Ok(hlpval.PostData_Basic_Auth_PaymentDetails_BY_UID(objCert));

			}
			catch (Exception ex)
			{
                CatchData.Status = "Failed";
                CatchData.data = CommonSPHel.ThirdpartyMessage;
                return Ok(CatchData);
            }

		}
		#endregion


		//NREGA CONTROLL
		#region "MasterData"
		[HttpPost]
		[Route("Project_Work_MasterData")]
		public IHttpActionResult Project_Work_MasterData(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				dynamic objCert = JsonConvert.DeserializeObject<dynamic>(value);
				if (Utils.IsAlphaNumeric(objCert.FarmerId) && Utils.IsAlphaNumeric(objCert.UID))
					return Ok(hlpval.Project_Work_MasterData());
				else
				{
					dynamic RData = new ExpandoObject();
					RData.Status = "Failed";
					RData.data = "Special Characters are Not Allowed.";
					return Ok(RData);
				}

			}
			catch (Exception ex)
			{
                CatchData.Status = "Failed";
                CatchData.data = CommonSPHel.ThirdpartyMessage;
                return Ok(CatchData);
            }

		}
		
		[HttpPost]
		[Route("Get_FarmerData")]
		public IHttpActionResult Get_FarmerData(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				dynamic objCert = JsonConvert.DeserializeObject<dynamic>(value);
				return Ok(hlpval.Get_FarmerData(objCert));

			}
			catch (Exception ex)
			{
                CatchData.Status = "Failed";
                CatchData.data = CommonSPHel.ThirdpartyMessage;
                return Ok(CatchData);
            }

		}
		#endregion

		//NREGA CONTROLL
		#region "Save_FarmerData"
		[HttpPost]
		[Route("Save_FarmerData")]
		public IHttpActionResult Save_FarmerData(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				dynamic objCert = JsonConvert.DeserializeObject<dynamic>(value);
				return Ok(hlpval.Save_FarmerData(objCert));

			}
			catch (Exception ex)
			{
                CatchData.Status = "Failed";
                CatchData.data = CommonSPHel.ThirdpartyMessage;
                return Ok(CatchData);
            }

		}
		#endregion

		//NREGA CONTROLL
		#region "DemandCapture"
		[HttpPost]
		[Route("DemandCapture")]
		public IHttpActionResult DemandCapture(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				dynamic objCert = JsonConvert.DeserializeObject<dynamic>(value);
				if (Utils.IsAlphaNumeric(objCert.SeqID))
					return Ok(hlpval.DemandCapture(objCert));
				else
				{
					dynamic RData = new ExpandoObject();
					RData.Status = "Failed";
					RData.data = "Special Characters are Not Allowed.";
					return Ok(RData);
				}
			}
			catch (Exception ex)
			{
                CatchData.Status = "Failed";
                CatchData.data = CommonSPHel.ThirdpartyMessage;
                return Ok(CatchData);
            }

		}
		#endregion

		//NREGA CONTROLL
		#region "ConfirmDemand"
		[HttpPost]
		[Route("ConfirmDemand")]
		public IHttpActionResult ConfirmDemand(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				dynamic objCert = JsonConvert.DeserializeObject<dynamic>(value);
				if (Utils.IsAlphaNumeric(objCert.SeqID) && Utils.IsAlphaNumeric(objCert.ReceiptNo))
					return Ok(hlpval.ConfirmDemand(objCert));
				{
					dynamic RData = new ExpandoObject();
					RData.Status = "Failed";
					RData.data = "Special Characters are Not Allowed.";
					return Ok(RData);
				}

			}
			catch (Exception ex)
			{
                CatchData.Status = "Failed";
                CatchData.data = CommonSPHel.ThirdpartyMessage;
                return Ok(CatchData);
            }

		}
		#endregion

		//Buildinig Plan
		#region "Buildinig Plan"
		[HttpPost]
		[Route("BuildingPlanApplicationStatus")]
		public IHttpActionResult BuildingPlanApplicationStatus(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				dynamic objCert = JsonConvert.DeserializeObject<dynamic>(value);
				if (Utils.IsAlphaNumeric(objCert.id))
					return Ok(hlpval.BuildingPlanApplicationStatus(objCert));
				else
				{
					dynamic RData = new ExpandoObject();
					RData.Status = "Failed";
					RData.data = "Special Characters are Not Allowed.";
					return Ok(RData);
				}
			}
			catch (Exception ex)
			{
                CatchData.Status = "Failed";
                CatchData.data = CommonSPHel.ThirdpartyMessage;
                return Ok(CatchData);
            }

		}
		#endregion

		//Layout Plan
		#region "LayoutPlanApplicationStatus Plan"
		[HttpPost]
		[Route("LayoutPlanApplicationStatus")]
		public IHttpActionResult LayoutPlanApplicationStatus(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				dynamic objCert = JsonConvert.DeserializeObject<dynamic>(value);
				if (Utils.IsAlphaNumeric(objCert.id))
					return Ok(hlpval.LayoutPlanApplicationStatus(objCert));
				else
				{
					dynamic RData = new ExpandoObject();
					RData.Status = "Failed";
					RData.data = "Special Characters are Not Allowed.";
					return Ok(RData);
				}

			}
			catch (Exception ex)
			{
                CatchData.Status = "Failed";
                CatchData.data = CommonSPHel.ThirdpartyMessage;
                return Ok(CatchData);
            }

		}
		#endregion

		//Layout Plan
		#region "LayoutPlanApplicationInformation Plan"
		[HttpPost]
		[Route("LayoutPlanApplicationInformation")]
		public IHttpActionResult LayoutPlanApplicationInformation(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				dynamic objCert = JsonConvert.DeserializeObject<dynamic>(value);
				if (Utils.IsAlphaNumeric(objCert.search_val))
					return Ok(hlpval.LayoutPlanApplicationInformation(objCert));
				else
				{
					dynamic RData = new ExpandoObject();
					RData.Status = "Failed";
					RData.data = "Special Characters are Not Allowed.";
					return Ok(RData);
				}

			}
			catch (Exception ex)
			{
                CatchData.Status = "Failed";
                CatchData.data = CommonSPHel.ThirdpartyMessage;
                return Ok(CatchData);
            }

		}
		#endregion

		//Layout Plan
		#region "TransportDrinkingWater Plan"
		[HttpPost]
		[Route("TransportDrinkingWater")]
		public IHttpActionResult TransportDrinkingWater(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				dynamic objCert = JsonConvert.DeserializeObject<dynamic>(value);
				return Ok(hlpval.TransportDrinkingWater());

			}
			catch (Exception ex)
			{
                CatchData.Status = "Failed";
                CatchData.data = CommonSPHel.ThirdpartyMessage;
                return Ok(CatchData);
            }

		}
		#endregion

		//Marriage Registration
		#region "Marriage Registration"
		[HttpPost]
		[Route("PRRDMarriageRegistration")]
		public IHttpActionResult PRRDMarriageRegistration(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
            dynamic objres = new ExpandoObject();
            try
			{
				dynamic objCert = JsonConvert.DeserializeObject<dynamic>(value);
				return Ok(hlpval.AuthentiateCall(objCert));

			}
			catch (Exception ex)
			{
                objres.Status = 102;
                objres.Reason = "Error Occured While Getting  Data";
                return Ok(objres);
            }

		}

		[HttpPost]
		[Route("LoadNICPanchayats")]
		public IHttpActionResult LoadNICPanchayats(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			dynamic objres = new ExpandoObject();
			try
			{
				dynamic objCert = JsonConvert.DeserializeObject<dynamic>(value);
				return Ok(hlpval.LoadNICPanchayats_helper(objCert));

			}
			catch (Exception ex)
			{
				objres.Status = 102;
				objres.Reason = "Error Occured While Getting  Data";
				return Ok(objres);
			}

		}
		#endregion

		#region "Marriage certificate"
		[HttpPost]
		[Route("GSWS_MarriageAppRequest")]
		public List<PRRDMRGCHECK> GSWS_MarriageAppRequest(dynamic redata)
		{
			dynamic objres = new ExpandoObject();
			try
			{
				PRRDMRGCHECK data = null;

				List<PRRDMRGCHECK> datalist = new List<PRRDMRGCHECK>();

				data = new PRRDMRGCHECK();
				data.status = "1";
				data.Reason = DateTime.Now.ToString();

				datalist.Add(data);

				return datalist;
			}
			catch (Exception ex)
			{
				objres.Status = 102;
				objres.Reason = "Error Occured While Getting  Data";
				return Ok(objres);
			}

		}

		[HttpPost]
		[Route("GSWS_MarriageacknowledgmentRequest")]
		public dynamic GSWS_MarriageacknowledgmentRequest(dynamic recdata)
		{

			dynamic objres = new ExpandoObject();
			try
			{
				PRRDMRGCHECK data = null;

				List<PRRDMRGCHECK> datalist = new List<PRRDMRGCHECK>();

				data = new PRRDMRGCHECK();
				data.status = "1";
				data.Reason = DateTime.Now.ToString();

				datalist.Add(data);

				return datalist;
			}
			catch (Exception ex)
			{
				objres.Status = 102;
				objres.Reason = "Error Occured While Getting  Data";

				return Ok(objres);
			}

		}

		#endregion

		#region "NOC"
		[HttpPost]
		[Route("GSWS_NOCAppRequest")]
		public List<PRRDMRGCHECK> GSWS_NOCAppRequest(dynamic objdata)
		{

			dynamic objres = new ExpandoObject();
			try
			{
				PRRDMRGCHECK data = null;

				List<PRRDMRGCHECK> datalist = new List<PRRDMRGCHECK>();

				data = new PRRDMRGCHECK();
				data.status = "1";
				data.Reason = DateTime.Now.ToString();

				datalist.Add(data);

				return datalist;
			}
			catch (Exception ex)
			{
				objres.Status = 102;
				objres.Reason = "Error Occured While Getting  Data";

				return Ok(objres);
			}

		}

		[HttpPost]
		[Route("GSWS_NOCacknowledgmentRequest")]
		public dynamic GSWS_NOCacknowledgmentRequest(dynamic objdata)
		{
			dynamic objres = new ExpandoObject();
			try
			{
				PRRDMRGCHECK data = null;

				List<PRRDMRGCHECK> datalist = new List<PRRDMRGCHECK>();

				data = new PRRDMRGCHECK();
				data.status = "1";
				data.Reason = DateTime.Now.ToString();

				datalist.Add(data);

				return datalist;
			}
			catch (Exception ex)
			{
				objres.Status = 102;
				objres.Reason = "Error Occured While Getting  Data";

				return Ok(objres);
			}

		}
		#endregion

		#region Valunteer

		[HttpPost]
		[Route("GetValunteerDetails")]
		public IHttpActionResult GetValunteerDetails(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			dynamic obj = new ExpandoObject();
			try
			{

				//string value = JsonConvert.SerializeObject(data);
				VolunteerCls rootobj = JsonConvert.DeserializeObject<VolunteerCls>(value);
				if (Utils.IsAlphaNumeric(rootobj.fuid_num) && Utils.IsAlphaNumeric(rootobj.fvv_id))
					return Ok(hlpval.GetValunteerMapping_helper(rootobj));
				else
				{
					dynamic RData = new ExpandoObject();
					RData.Status = 102;
					RData.Reason = "Special Characters are Not Allowed.";
					return Ok(RData);
				}

			}
			catch (Exception ex)
			{
				obj.Status = 102;
				obj.Reason = "Error Occured While Getting Volunteer Data";

				return Ok(obj);
			}

		}

		#endregion

		#region Birth Death Registration
		//Load Departments
		[HttpPost]
		[Route("LoadDistricts")]
		public IHttpActionResult LoadDistricts(dynamic data)
		{
			dynamic obj = new ExpandoObject();
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{

				//string value = JsonConvert.SerializeObject(data);
				DistrictsCls rootobj = JsonConvert.DeserializeObject<DistrictsCls>(value);
				return Ok(hlpval.LoadDistricts(rootobj));
			}
			catch (Exception ex)
			{
				obj.Status = 102;
				obj.Reason = "Error Occued While Loading Data";
				return Ok(obj);
			}

		}
		#endregion

		#region "Secretriat to volunteer mapping approval"

		[HttpPost]
		[Route("SecretriattoVoluteerData")]
		public IHttpActionResult SecretriattoVoluteerData(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			try
			{
				SecretriatVVModel root = JsonConvert.DeserializeObject<SecretriatVVModel>(value);
				return Ok(hlpval.getSecretriattovolunteer(root));

			}
			catch (Exception ex)
			{
                CatchData.Status = 102;
                CatchData.Reason = "Error Occured While Getting Data";
                return Ok(CatchData);
            }

		}
		#endregion


		#region JobcardRegistration
		[HttpPost]
		[Route("GetJobCardMaster")]

		public dynamic GetJobCardMaster(dynamic data)
		{

			string jsondata = token_gen.Authorize_aesdecrpty(data);
			LGDMasterModel val = JsonConvert.DeserializeObject<LGDMasterModel>(jsondata);
			LogModel ologmodel = new LogModel();
			ologmodel.UserId = val.UserId;
			ologmodel.SacId = val.SacId;
			ologmodel.DesignId = val.DesignId;
			ologmodel.DeptId = string.Empty;
			ologmodel.TranId = val.TranId;
			//olog.WriteLogParameters(ologmodel);
			try
			{

				JobCardHelper hlpval = new JobCardHelper();
				//_log.Info("In the RevenueController => GetRevenueMaster: " + jsondata.ToString());
				return Ok(hlpval.LoadPanchayatlist(val));
				//	return "SuccessEncryptDataModel

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobcardControllrExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "getting error GetJobCardMaster :" + ex.Message.ToString()));
				CatchData.Status = "Failure";
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}
		[HttpPost]
		[Route("GetJobCardBankMaster")]
		public dynamic GetJobCardBankMaster(dynamic data)
		{

			string jsondata = token_gen.Authorize_aesdecrpty(data);
			JobCardBankModel val = JsonConvert.DeserializeObject<JobCardBankModel>(jsondata);
			LogModel ologmodel = new LogModel();
			ologmodel.UserId = val.UserId;
			ologmodel.SacId = val.SacId;
			ologmodel.DesignId = val.DesignId;
			ologmodel.DeptId = string.Empty;
			ologmodel.TranId = val.TranId;
			//olog.WriteLogParameters(ologmodel);
			try
			{

				JobCardHelper hlpval = new JobCardHelper();
				//_log.Info("In the RevenueController => GetRevenueMaster: " + jsondata.ToString());
				return Ok(hlpval.LoadBankDetails(val));
				//	return "SuccessEncryptDataModel

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobcardControllrExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "getting error GetJobCardBankMaster :" + ex.Message.ToString()));
				CatchData.Status = "Failure";
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}


		[HttpPost]
		[Route("JobcardDistandMandalCode")]
		public dynamic JobcardDistandMandalCode(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			LGDMasterModel val = JsonConvert.DeserializeObject<LGDMasterModel>(jsondata);
			LogModel ologmodel = new LogModel();
			//ologmodel.UserId = val.UserId;
			//ologmodel.SacId = val.SacId;
			//ologmodel.DesignId = val.DesignId;
			//ologmodel.DeptId = string.Empty;
			//ologmodel.TranId = val.TranId;
			//olog.WriteLogParameters(ologmodel);
			try
			{

				JobCardHelper hlpval = new JobCardHelper();

				return Ok(hlpval.GetDistandMandalCode(val));
				// return "SuccessEncryptDataModel

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobcardControllrExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "getting error JobcardDistandMandalCode :" + ex.Message.ToString()));
				CatchData.Status = "Failure";
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}


		[HttpPost]
		[Route("GetHabitationCode")]
		public dynamic GetHabitationCode(dynamic data)
		{

			string jsondata = token_gen.Authorize_aesdecrpty(data);
			LGDMasterModel val = JsonConvert.DeserializeObject<LGDMasterModel>(jsondata);
			LogModel ologmodel = new LogModel();
			ologmodel.UserId = val.UserId;
			ologmodel.SacId = val.SacId;
			ologmodel.DesignId = val.DesignId;
			ologmodel.DeptId = string.Empty;
			ologmodel.TranId = val.TranId;
			//olog.WriteLogParameters(ologmodel);
			try
			{

				JobCardHelper hlpval = new JobCardHelper();
				//_log.Info("In the RevenueController => GetRevenueMaster: " + jsondata.ToString());
				return Ok(hlpval.LoadHabitationCode(val));
				//	return "SuccessEncryptDataModel

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobcardControllrExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "getting error GetHabitationCode :" + ex.Message.ToString()));
				CatchData.Status = "Failure";
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}

		[HttpPost]
		[Route("CreateJobCard")]
		public dynamic CreateJobCard(dynamic data)
		{

			// var jsondata = token_gen.Authorize_aesdecrpty(data);
			var jsondata = JsonConvert.SerializeObject(data);
			List<JobCardModel> val = JsonConvert.DeserializeObject<List<JobCardModel>>(jsondata);


			//Jobmultiplejobcard val= JsonConvert.DeserializeObject<Jobmultiplejobcard>(jsondata);
			// JobCardModel val = JsonConvert.DeserializeObject<JobCardModel>(jsondata);
			LogModel ologmodel = new LogModel();
			//ologmodel.UserId = val.UserId;
			//ologmodel.SacId = val.SacId;
			//ologmodel.DesignId = val.DesignId;
			//ologmodel.DeptId = string.Empty;
			//ologmodel.TranId = val.TranId;
			// //olog.WriteLogParameters(ologmodel);
			try
			{

				JobCardHelper _Prrdhel = new JobCardHelper();
				//_log.Info("In the PRRDController => CreateJobCard: " + jsondata.ToString());
				return Ok(_Prrdhel.CreateJobCard_hel(val));
				//return "SuccessEncryptDataModel";

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobcardControllrExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "getting error CreateJobCard :" + ex.Message.ToString()));
				CatchData.Status = "Failure";
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}
		#endregion
		#region JobCardDetails
		[HttpPost]
		[Route("GetJobcardetails")]
		public IHttpActionResult GetJobcardetails(dynamic data)
		{
			string value = token_gen.Authorize_aesdecrpty(data);
			JobCardModel root = JsonConvert.DeserializeObject<JobCardModel>(value);
			LogModel ologmodel = new LogModel();
			ologmodel.UserId = root.UserId;
			ologmodel.SacId = root.SacId;
			ologmodel.DesignId = root.DesignId;
			ologmodel.DeptId = "";//Departments.Panchayati_Raj.ToString();
			ologmodel.TranId = root.TranId;
			//olog.WriteLogParameters(ologmodel);
			JobCardHelper hlpval = new JobCardHelper();
			try
			{
				//_log.Info("In the PRRDController => GetJobcardetails: " + value.ToString());
				return Ok(hlpval.GetJobcardetails(root));
			}
			catch (Exception ex)
			{

				string mappath = HttpContext.Current.Server.MapPath("JobcardControllrExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "getting error GetJobcardetails :" + ex.Message.ToString()));
				CatchData.Status = 102;
				CatchData.Reason = "Error Occured While Getting Data";
				return Ok(CatchData);
			}
		}



		[HttpPost]
		[Route("EachJobCardData")]
		public dynamic EachJobCardData(dynamic data)
		{

			string jsondata = token_gen.Authorize_aesdecrpty(data);
			JobCardModel val = JsonConvert.DeserializeObject<JobCardModel>(jsondata);
			LogModel ologmodel = new LogModel();
			//ologmodel.UserId = val.UserId;
			//ologmodel.SacId = val.SacId;
			//ologmodel.DesignId = val.DesignId;
			//ologmodel.DeptId = string.Empty;
			//ologmodel.TranId = val.TranId;
			////olog.WriteLogParameters(ologmodel);
			try
			{

				JobCardHelper hel = new JobCardHelper();
				//_log.Info("In the PRRDController => EachJobCardData: " + jsondata.ToString());
				return Ok(hel.GetJobCardDataHel(val));
				// return "SuccessEncryptDataModel

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobcardControllrExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "getting error EachJobCardData :" + ex.Message.ToString()));
				CatchData.Status = 102;
				CatchData.Status = "Failure";
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}


		[HttpPost]
		[Route("SendJobcardAPI")]
		public dynamic SendJobcardAPI(dynamic data)
		{
			//var jsondata = token_gen.Authorize_aesdecrpty(data);
			var jsondata = JsonConvert.SerializeObject(data);
			dynamic val = JsonConvert.DeserializeObject<dynamic>(jsondata);
			// List<JobCardAPIModel> val = JsonConvert.DeserializeObject<List<JobCardAPIModel>>(jsondata);            
			LogModel ologmodel = new LogModel();

			//ologmodel.UserId = val.UserId;
			//ologmodel.SacId = val.SacId;
			//ologmodel.DesignId = val.DesignId;
			//ologmodel.DeptId = string.Empty;
			//ologmodel.TranId = val.TranId;
			////olog.WriteLogParameters(ologmodel);
			try
			{

				JobCardHelper hel = new JobCardHelper();
				//_log.Info("In the PRRDController => EachJobCardData: " + jsondata.ToString());
				return Ok(hel.GetJobCardDetailsbyTransIdandUIDhelper(val));

				// return "SuccessEncryptDataModel

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobcardControllrExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "getting error SendJobcardAPI :" + ex.Message.ToString()));
				CatchData.Status = "Failure";
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}

		[HttpPost]
		[Route("RejectJobcard")]
		public dynamic RejectJobcard(dynamic data)
		{

			string jsondata = token_gen.Authorize_aesdecrpty(data);
			JobCardModel val = JsonConvert.DeserializeObject<JobCardModel>(jsondata);
			LogModel ologmodel = new LogModel();
			//ologmodel.UserId = val.UserId;
			//ologmodel.SacId = val.SacId;
			//ologmodel.DesignId = val.DesignId;
			//ologmodel.DeptId = string.Empty;
			//ologmodel.TranId = val.TranId;
			////olog.WriteLogParameters(ologmodel);
			try
			{

				JobCardHelper hel = new JobCardHelper();
				//_log.Info("In the PRRDController => EachJobCardData: " + jsondata.ToString());

				return Ok(hel.UpdateJobcardStatus(val));

				// return "SuccessEncryptDataModel

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobcardControllrExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "getting error RejectJobcard :" + ex.Message.ToString()));
				CatchData.Status = "Failure";
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}


		[HttpPost]
		[Route("TranslateTextAPI")]
		public dynamic TranslateTextAPI(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			TranslationModel val = JsonConvert.DeserializeObject<TranslationModel>(jsondata);
			LogModel ologmodel = new LogModel();
			//ologmodel.UserId = val.UserId;
			//ologmodel.SacId = val.SacId;
			//ologmodel.DesignId = val.DesignId;
			//ologmodel.DeptId = string.Empty;
			//ologmodel.TranId = val.TranId;
			////olog.WriteLogParameters(ologmodel);
			try
			{

				JobCardHelper hel = new JobCardHelper();
				//_log.Info("In the PRRDController => TranslateTextAPI: " + jsondata.ToString());

				return Ok(hel.GetTranslationText(val));

				// return "SuccessEncryptDataModel

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobcardControllrExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "getting error TranslateTextAPI :" + ex.Message.ToString()));
				CatchData.Status = "Failure";
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}


		}

		[HttpPost]
		[Route("Aadharvalidate")]
		public dynamic Aadharvalidate(dynamic data)
		{

			string jsondata = token_gen.Authorize_aesdecrpty(data);
			//JobCardModel val = JsonConvert.DeserializeObject<JobCardModel>(jsondata);
			dynamic val = JsonConvert.DeserializeObject<dynamic>(jsondata);
			LogModel ologmodel = new LogModel();
			try
			{

				JobCardHelper hel = new JobCardHelper();
				//_log.Info("In the PRRDController => Aadharvalidate: " + jsondata.ToString());

				return Ok(hel.AadharVaiidateAPI(val));

				// return "SuccessEncryptDataModel

			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("JobcardControllerExceptions");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "getting error Aadharvalidate :" + ex.Message.ToString()));
				CatchData.Status = "Failure";
				CatchData.Reason = CommonSPHel.ThirdpartyMessage;
				return Ok(CatchData);
			}
		}
		#endregion

	}
}
