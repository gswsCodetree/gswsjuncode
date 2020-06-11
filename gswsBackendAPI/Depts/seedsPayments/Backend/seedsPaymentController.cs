using gswsBackendAPI.Depts.RBKPayments.Backend;
using gswsBackendAPI.DL.CommonHel;
using gswsBackendAPI.DL.DataConnection;
//using gswsBackendAPI.eseedService;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace gswsBackendAPI.Depts.seedsPayments.Backend
{
	[RoutePrefix("api/seedsPayment")]
	public class seedsPaymentController : ApiController
	{
		[Route("OrderDetails")]
		[HttpPost]
		public IHttpActionResult OrderDetails(dynamic data)
		{
			dynamic objdata = new ExpandoObject();
			try
			{
				string serialized_data = token_gen.Authorize_aesdecrpty(data);
				seedPaymentModel rootobj = JsonConvert.DeserializeObject<seedPaymentModel>(serialized_data);
				return Ok(seedsPaymentHelper.OrderDetails(rootobj));
			}
			catch (Exception ex)
			{
				objdata.status = false;
				objdata.result = ex.Message.ToString();
			}
			return Ok(objdata);
		}


		[Route("paymentOrderDetails")]
		[HttpPost]
		public IHttpActionResult paymentOrderDetails(dynamic data)
		{
			dynamic objdata = new ExpandoObject();
			string serialized_data = token_gen.Authorize_aesdecrpty(data);
			try
			{
				
				seedPaymentModel rootobj = JsonConvert.DeserializeObject<seedPaymentModel>(serialized_data);
				return Ok(seedsPaymentHelper.paymentOrderDetails(rootobj));
			}
			catch (Exception ex)
			{
				string mappath2 = HttpContext.Current.Server.MapPath("PaymentOrderExceptionGatewayLogs");
				Task WriteTask2 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath2, "input Data:" + serialized_data + "error:" + ex.Message.ToString()));

				objdata.status = false;
				objdata.result = ex.Message.ToString();
			}
			return Ok(objdata);
		}


		[Route("makePayment")]
		[HttpPost]
		public IHttpActionResult makePayment(dynamic data)
		{
			string serialized_data = token_gen.Authorize_aesdecrpty(data);
			walletProcModel rootobj = JsonConvert.DeserializeObject<walletProcModel>(serialized_data);
			return Ok(seedsPaymentHelper.makePayment(rootobj));
		}


		[Route("recieptData")]
		[HttpPost]
		public IHttpActionResult recieptData(dynamic data)
		{
			string serialized_data = token_gen.Authorize_aesdecrpty(data);
			recieptModel rootobj = JsonConvert.DeserializeObject<recieptModel>(serialized_data);
			return Ok(seedsPaymentHelper.recieptData(rootobj));
		}


		[Route("PrintChallan")]
		[HttpPost]
		public IHttpActionResult PrintChallan(dynamic data)
		{
			string serialized_data = token_gen.Authorize_aesdecrpty(data);
			SeedDepositModal rootobj = JsonConvert.DeserializeObject<SeedDepositModal>(serialized_data);
			return Ok(seedsPaymentHelper.PrintChallanData(rootobj));
		}

		[HttpPost]
		[Route("SeedbankDepositModule")]
		public dynamic SeedbankDepositModule(dynamic data)
		{
			string jsondata = token_gen.Authorize_aesdecrpty(data);
			try
			{
				SeedDepositModal val = JsonConvert.DeserializeObject<SeedDepositModal>(jsondata);
				return Ok(seedsPaymentHelper.SeedbankDepositModule_hel(val));
			}
			catch (Exception ex)
			{
				dynamic _response = new ExpandoObject();
				_response.Status = 102;
				_response.Reason = ex.Message.ToString();
				return Ok(_response);
			}
		}

	}

	public static class seedsPaymentHelper
	{
		static CommonSPHel _Hel = new CommonSPHel();
		static string password = "f794b75239e84b1675a100fc85efb213";

		public static dynamic OrderDetails(seedPaymentModel obj)
		{
			dynamic objdata = new ExpandoObject();
			try
			{
				var input = new
				{
					userid = "gsws",
					password = password,
					otpid = obj.orderId
				};
				string input_data = JsonConvert.SerializeObject(input);
				string url = "https://eseed.ap.gov.in/eseed/RestFul/VSServices/ReceivePayment";
				string response = POST_RequestAsync(url, input_data);
				orderDetailesRespModel rootobj = JsonConvert.DeserializeObject<orderDetailesRespModel>(response);

				if (rootobj.response.status == "1" || rootobj.response.status == "2")
				{
					resProcModel objres = new resProcModel();
					objres.type = "5";
					objres.txnId = obj.orderId;
					objdata.status = true;
					objdata.result = rootobj.response;
					objdata.DataList = resProc(objres,0);
					objdata.Pflag = rootobj.response.status;

				}
				else
				{
					objdata.status = false;
					objdata.result = rootobj.response.msg;
				}


			}
			catch (Exception ex)
			{
				objdata.status = false;
				objdata.result = ex.Message.ToString();
			}
			return objdata;
		}

		public static dynamic paymentOrderDetails(seedPaymentModel obj)
		{
			dynamic objdata = new ExpandoObject();
			resProcModel objRes = new resProcModel();
			orderDetailesRespModel rootobj = new orderDetailesRespModel();
			try
			{
				string response=string.Empty;
				string input_data = string.Empty;
				decModel decData = new decModel();
				obj.encrypted_data = obj.encrypted_data.Replace(" ", "+");
				string decrypted_text = EncryptDecryptAlgoritham.DecryptStringAES(obj.encrypted_data, "3fee5395f01bee349feed65629bd442a", obj.iv);
				try
				{

					var input = new
					{
						userid = "gsws",
						password = password,
						otpid = obj.orderId
					};
					 input_data = JsonConvert.SerializeObject(input);
					 decData = JsonConvert.DeserializeObject<decModel>(decrypted_text);
					//   decData.PS_TXN_ID = DateTime.Now.ToString("yyyymmddhhmmssfff");
					string url = "https://eseed.ap.gov.in/eseed/RestFul/VSServices/ReceivePayment";
					 response = POST_RequestAsync(url, input_data);
					
					string mappath2 = HttpContext.Current.Server.MapPath("PaymentOrderDetailLogs");
					Task WriteTask2 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath2, response));
					if (response == "{}")
					{
						string mappath3 = HttpContext.Current.Server.MapPath("PaymentOrderGatewayLogs");
						Task WriteTask3 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath3, "input Data:" + input_data + response));
						objdata.status = false;
						objdata.result = "Response From D-Krishi : Deatails Not Available for given OTP ID:"+ obj.orderId;
						return objdata;
					}
					else
					rootobj = JsonConvert.DeserializeObject<orderDetailesRespModel>(response);
				}
				catch (Exception ex)
				{
					string mappath2 = HttpContext.Current.Server.MapPath("PaymentOrderExceptionGatewayLogs");
					Task WriteTask2 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath2, "input Data:" + input_data + response + "error:" + ex.Message.ToString()));
					throw ex;
				}
				rootobj.response.gswsTxnId = decData.PS_TXN_ID;
				objdata.status = true;
				objdata.result = rootobj.response;
				return objdata;

				if (rootobj.response.status == "1" || rootobj.response.status == "2")
				{
					 objRes = new resProcModel();
					objRes.type = "1";
					objRes.txnId = decData.PS_TXN_ID;
					objRes.orderId = obj.orderId;
					objRes.deptId = "11";
					objRes.statusCode = rootobj.response.status.ToString();
					objRes.remarks = rootobj.response.msg;
					objRes.serviceId = "1101025";
					objRes.userCharge = rootobj.response.amountTobePaid.ToString();
					objRes.serviceCharge = "0";
					objRes.totalAmount = (float.Parse(objRes.userCharge, CultureInfo.InvariantCulture.NumberFormat) + float.Parse(objRes.serviceCharge, CultureInfo.InvariantCulture.NumberFormat)).ToString();

					DataTable dt = resProc(objRes,0);
					if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
					{
						rootobj.response.gswsTxnId = decData.PS_TXN_ID;
						objdata.status = true;
						objdata.result = rootobj.response;
					}
					else if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "2")
					{
						objdata.status = false;
						objdata.result = "These transaction id is already used please login and try again !!!";
					}
					else
					{
						objdata.status = false;
						objdata.result = "Failed to Fetch Record";
					}
				}
				else
				{
					objdata.status = false;
					objdata.result = rootobj.response.msg;
				}

				


			}
			catch (Exception ex)
			{
				string mappath2 = HttpContext.Current.Server.MapPath("SeedsExceptionLogs");
				Task WriteTask2 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath2, "paymentOrderDetails" + ex.Message.ToString() + ":" + JsonConvert.SerializeObject(objRes)));

				objdata.status = false;
				objdata.result = ex.Message.ToString();
			}
			return objdata;
		}

		public static dynamic makePayment(walletProcModel obj)
		{
			dynamic objdata = new ExpandoObject();
			orderDetailesRespModel rootobj = new orderDetailesRespModel();

			try
			{
				if (obj == null)
				{
					objdata.status = false;
					objdata.result = "Failed Make Payment Please try again !!!";
					return objdata;
				}
				var input = new
				{
					vscode = obj.GSWSCODE,
					userid = "gsws",
					password = password,
					otpid = obj.APPLICATIONNO,
					transactionid = obj.GSWSREFNO,
					amounttobepaid = (float.Parse(obj.serviceCharge, CultureInfo.InvariantCulture.NumberFormat) + float.Parse("0", CultureInfo.InvariantCulture.NumberFormat)).ToString()

				};
				string input_data = JsonConvert.SerializeObject(input);
				string url = "https://eseed.ap.gov.in/eseed/RestFul/VSServices/updatePaymentStatus";
				string response = POST_RequestAsync(url, input_data);
				string mappath1 = HttpContext.Current.Server.MapPath("SeedsPaymentUpdationLogs");
				Task WriteTask1 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath1, response));
				rootobj = JsonConvert.DeserializeObject<orderDetailesRespModel>(response);
				resProcModel objRes = new resProcModel();
				objRes.type = "6";
				objRes.txnId = obj.GSWSREFNO;
				objRes.orderId = obj.APPLICATIONNO;
				if (rootobj.response.status == "1")
				{
					objRes.isPaymentSuccess = "1";
					
					DataTable dt = resProc(objRes,0);
					if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["STATUS"].ToString() == "1" && rootobj.response.status == "1")
					{
						objdata.status = true;
						objdata.result = dt;
					}
					else
					{
						objdata.status = false;
						objdata.result = "Failed Make Payment Please try again !!!";
					}
				}
				else
				{
					objdata.status = false;
					objdata.result = "Failed Make Payment Please try again !!!";
				}
				//else
				//{
				//    objRes.isPaymentSuccess = "0";
				//}



			}
			catch (Exception ex)
			{
				string mappath2 = HttpContext.Current.Server.MapPath("SeedsExceptionLogs");
				Task WriteTask2 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath2, "makePayment" + ex.Message.ToString() + ":" + JsonConvert.SerializeObject(rootobj)));

				objdata.status = false;
				objdata.result = ex.Message.ToString();

			}
			return objdata;
		}

		public static dynamic recieptData(recieptModel obj)
		{
			dynamic objdata = new ExpandoObject();
			try
			{
				string serialized_data = rbkPaymentsHelper.decryptString(obj.encryptedString);
				walletRespModel objResp = JsonConvert.DeserializeObject<walletRespModel>(serialized_data);
				if (objResp.ERRORCODE == "101")
				{
					try
					{
						walletProcModel walletObj = new walletProcModel();
						walletObj.type = "2";
						walletObj.walletTxnId = objResp.WALLETREFNO;
						walletObj.paymentStatus = "1";
						walletObj.APPLICATIONNO = objResp.APPLICATIONNO;
						walletObj.GSWSREFNO = objResp.GSWSREFNO;
						DataTable dt = rbkPaymentsHelper.walletProc(walletObj);
						if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
						{
							resProcModel objRes = new resProcModel();
							objRes.type = "5";
							objRes.txnId = objResp.GSWSREFNO;
							DataTable dt1 = rbkPaymentsHelper.resProc(objRes);
							if (dt1 != null && dt1.Rows.Count > 0)
							{

								obj.secId = "11190157";
								var input = new
								{
									vscode = obj.secId,
									userid = "gsws",
									password = password,
									otpid = objResp.APPLICATIONNO,
									transactionid = objResp.GSWSREFNO,
									amounttobepaid = (float.Parse(objResp.serviceAmt, CultureInfo.InvariantCulture.NumberFormat) + float.Parse(objResp.userCharges, CultureInfo.InvariantCulture.NumberFormat)).ToString()

								};
								string input_data = JsonConvert.SerializeObject(input);
								string url = "https://eseed.ap.gov.in/eseed/RestFul/VSServices/updatePaymentStatus";
								string response = POST_RequestAsync(url, input_data);
								orderDetailesRespModel rootobj = JsonConvert.DeserializeObject<orderDetailesRespModel>(response);
								if (rootobj.response.status == "1")
								{
									var input1 = new
									{
										userid = "gsws",
										password = password,
										otpid = objResp.APPLICATIONNO
									};
									input_data = JsonConvert.SerializeObject(input1);
									url = "https://eseed.ap.gov.in/eseed/RestFul/VSServices/ReceivePayment";
									response = POST_RequestAsync(url, input_data);
									orderDetailesRespModel rootobj1 = JsonConvert.DeserializeObject<orderDetailesRespModel>(response);
									if (rootobj1.response.status == "2")
									{
										//call wallet table and resp table and txn cancel servie
										objdata.status = true;
										objdata.seedDetails = rootobj1.response;
										objdata.walletDetails = objResp;
										objdata.orderDetails = dt1;
									}
									else
									{
										objdata.status = false;
										objdata.result = "Failed Make Payment Please try again !!!";
									}
								}
								else
								{
									objdata.status = false;
									objdata.result = "Failed Make Payment Please try again !!!";
								}
							}
							else
							{
								objdata.status = false;
								objdata.result = "Failed Make Payment Please try again !!!";

							}

						}
						else
						{
							objdata.status = false;
							objdata.result = "Failed Make Payment Please try again !!!";
						}



					}
					catch (Exception ex1)
					{
						objdata.status = false;
						objdata.result = ex1.Message.ToString();
					}



				}
				else
				{
					objdata.status = false;
					objdata.result = "Failed Make Payment Please try again !!!";
				}
			}
			catch (Exception ex)
			{
				objdata.status = false;
				objdata.result = ex.Message.ToString();

			}
			return objdata;
		}

		public static dynamic PrintChallanData(SeedDepositModal objs)
		{
			dynamic _ResObj = new ExpandoObject();
			try
			{
				DataTable dt = SeedbankDepositModule_sphel(objs);
				if (dt != null && dt.Rows.Count > 0)
				{
					_ResObj.Status = 100;
					_ResObj.Reason = "Data Loaded Successfully";
					_ResObj.DataList = dt;
				}
				else
				{
					_ResObj.Status = 102;
					_ResObj.Reason = "Data Loading Failed";
				}

			}
			catch (Exception ex)
			{

				_ResObj.Status = 102;
				_ResObj.Reason = ex.Message.ToString();
			}
			return _ResObj;
		}
		static int excount = 0;
		public static DataTable resProc(resProcModel obj, int i)
		{
			OracleCommand cmd = new OracleCommand();
			OracleConnection con = new OracleConnection(new ConnectionHelper().Congswsprod);
			OracleDataAdapter adp = new OracleDataAdapter();
			try
			{
				cmd.Connection = con;
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_APSEEDS_CORP_RESP_PROC";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2).Value = obj.type;
				cmd.Parameters.Add("P_TRANSACTION_ID", OracleDbType.Varchar2).Value = obj.txnId;
				cmd.Parameters.Add("P_DEPT_TRANS_ID", OracleDbType.Varchar2).Value = obj.orderId;
				cmd.Parameters.Add("P_DEPT_ID", OracleDbType.Varchar2).Value = obj.deptId;
				cmd.Parameters.Add("P_STATUS_CODE", OracleDbType.Varchar2).Value = obj.statusCode;
				cmd.Parameters.Add("P_REMARKS", OracleDbType.Varchar2).Value = obj.remarks;
				cmd.Parameters.Add("P_SERVICE_ID", OracleDbType.Varchar2).Value = obj.serviceId;
				cmd.Parameters.Add("P_AMOUNT", OracleDbType.Varchar2).Value = obj.userCharge;
				cmd.Parameters.Add("P_SERVICE_CHARGE", OracleDbType.Varchar2).Value = obj.serviceCharge;
				cmd.Parameters.Add("P_TOTAL_AMOUNT", OracleDbType.Varchar2).Value = obj.totalAmount;
				cmd.Parameters.Add("P_IS_PAYMENT_SUCCESS", OracleDbType.Varchar2).Value = obj.isPaymentSuccess;
				cmd.Parameters.Add("P_IS_PAYMENT_REVERSAL", OracleDbType.Varchar2).Value = obj.isPaymentReversal;
				cmd.Parameters.Add("P_IS_PAY_REV_SUCCESS", OracleDbType.Varchar2).Value = obj.isPaymentRevSuccess;
				cmd.Parameters.Add("P_IS_TXNID_CANCEL_SUCCESS", OracleDbType.Varchar2).Value = obj.isTxnCancelSuccess;
				cmd.Parameters.Add("p_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				//DataTable dtstatus = _Hel.GetProdgswsDataAdapter(cmd);
				con.Open();
				DataTable dtstatus = new DataTable();
				adp = new OracleDataAdapter(cmd);
				adp.Fill(dtstatus);
				con.Close();
				cmd.Dispose();
				return dtstatus;
			}
			catch (Exception ex)
			{
				excount++;
				string mappath1 = HttpContext.Current.Server.MapPath("SeedsExceptionLogs");
				Task WriteTask1 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath1, "ResProc:"+excount+":" + ex.Message.ToString() + ":" + JsonConvert.SerializeObject(obj)));
				if (excount < 3)				
				return resProc(obj, excount);				
				else
				throw ex;
			}
		}

		public static string POST_RequestAsync(string uri, string json, int count = 0)
		{
			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

				var request = (HttpWebRequest)WebRequest.Create(uri);
				request.ContentType = "application/json";
				request.Method = "POST";

				using (var streamWriter = new StreamWriter(request.GetRequestStream()))
				{
					streamWriter.Write(json);
				}
				var response = (HttpWebResponse)request.GetResponse();
				string result = "";
				using (var streamReader = new StreamReader(response.GetResponseStream()))
				{
					result = streamReader.ReadToEnd();
				}
				return result;

			}
			catch (Exception ex)
			{
				if (count < 3)
				{
					Thread.Sleep(3000);
					count++;
					return POST_RequestAsync(uri, json, count);
				}
				else
				{
					throw ex;
				}
			}


		}

		// Seed bank Deposit Module
		public static dynamic SeedbankDepositModule_hel(SeedDepositModal Lobj)
		{
			dynamic _ResObj = new ExpandoObject();
			try
			{
				DataTable dt = SeedbankDepositModule_sphel(Lobj);

				if (dt != null && dt.Rows.Count > 0)
				{
					_ResObj.Status = 100;
					_ResObj.Reason = "Data Loaded Successfully";
					_ResObj.DataList = dt;
				}
				else
				{
					_ResObj.Status = 102;
					_ResObj.Reason = "Data Loading Failed";
				}
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("ORA-12520:"))
				{
					_ResObj.Status = 102;
					_ResObj.Reason = "Server busy.Please try again after sometime";

				}
				else
				{
					_ResObj.Status = 102;
					_ResObj.Reason = ex.Message;
				}
			}
			return _ResObj;
		}

		// Seed bank Deposit Module
		public static DataTable SeedbankDepositModule_sphel(SeedDepositModal Obj)
		{
			try
			{
				OracleCommand cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_APSEEDS_CHALLAN_PROC";

				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2, 200).Value = Obj.P_TYPE;
				cmd.Parameters.Add("P_DISTRICT", OracleDbType.Varchar2, 200).Value = Obj.P_DISTRICT;
				cmd.Parameters.Add("P_MANDAL", OracleDbType.Varchar2, 200).Value = Obj.P_MANDAL;
				cmd.Parameters.Add("P_SEC", OracleDbType.Varchar2, 200).Value = Obj.P_SEC;
				cmd.Parameters.Add("P_CHALLAN_TXN_ID", OracleDbType.Varchar2, 200).Value = Obj.P_CHALAN_TXN_ID;
				cmd.Parameters.Add("P_BANK_REF_NO", OracleDbType.Varchar2, 200).Value = Obj.P_BANK_REF_NO;
				cmd.Parameters.Add("P_BANK_TXN_DATE", OracleDbType.Varchar2, 200).Value = Obj.P_BANK_TXN_DATE;

				cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtLogin = _Hel.GetProdgswsDataAdapter(cmd);
				if (dtLogin != null)
				{
					return dtLogin;
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				string mappath1 = HttpContext.Current.Server.MapPath("SeedsDepositBankExceptionLogs");
				Task WriteTask1 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath1, "ResProc:" + excount + ":" + ex.Message.ToString() + ":" + JsonConvert.SerializeObject(Obj)));
				throw ex;
			}
		}

	}

	public class SeedDepositModal
	{
		public string P_TYPE { get; set; }
		public string P_DISTRICT { get; set; }
		public string P_MANDAL { get; set; }
		public string P_FLAG_U_R { get; set; }
		public string P_SEC { get; set; }
		public string P_CHALAN_TXN_ID { get; set; }
		public string P_BANK_REF_NO { get; set; }
		public string P_BANK_TXN_DATE { get; set; }
	}

	public class Response
	{
		public string gswsTxnId { get; set; }
		public string seedVariety { get; set; }
		public string farmerName { get; set; }
		public string status { get; set; }
		public string qtyInBags { get; set; }
		public string otpid { get; set; }
		public string cropName { get; set; }
		public string amountTobePaid { get; set; }
		public string vscode { get; set; }
		public string qtyInKgs { get; set; }
		public string msg { get; set; }

	}
	public class orderDetailesRespModel
	{
		public Response response { get; set; }

	}


	public class seedPaymentModel
	{
		public string secId { get; set; }
		public string orderId { get; set; }
		public string userId { get; set; }
		public string password { get; set; }
		public string encrypted_data { get; set; }
		public string iv { get; set; }
		public string mandalId { get; set; }
		public string gpWardId { get; set; }
		public string districtId { get; set; }
		public string uniqueId { get; set; }
		public string desgId { get; set; }
		public string loginuser { get; set; }
		public string serviceId { get; set; }

	}
}
