using gswsBackendAPI.DL.CommonHel;
using gswsBackendAPI.DL.DataConnection;
using gswsBackendAPI.transactionModule;
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
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace gswsBackendAPI.Depts.RBKPayments.Backend
{
	[RoutePrefix("api/rbkPayments")]
	public class rbkPaymentsController : ApiController
	{
		[Route("orderDetails")]
		[HttpPost]
		public IHttpActionResult orderDetails(dynamic data)
		{
			string serialized_data = token_gen.Authorize_aesdecrpty(data);
			rbkPaymentsModel rootobj = JsonConvert.DeserializeObject<rbkPaymentsModel>(serialized_data);
			return Ok(rbkPaymentsHelper.orderDetails(rootobj));
		}


		[Route("paymentOrderDetails")]
		[HttpPost]
		public IHttpActionResult paymentOrderDetails(dynamic data)
		{
			string serialized_data = token_gen.Authorize_aesdecrpty(data);
			rbkPaymentsModel rootobj = JsonConvert.DeserializeObject<rbkPaymentsModel>(serialized_data);
			return Ok(rbkPaymentsHelper.paymentOrderDetails(rootobj));
		}


		[Route("makePayment")]
		[HttpPost]
		public IHttpActionResult makePayment(dynamic data)
		{
			string serialized_data = token_gen.Authorize_aesdecrpty(data);
			string mappath2 = HttpContext.Current.Server.MapPath("MakePaymentLogs");
			Task WriteTask2 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath2,serialized_data));

			walletProcModel rootobj = JsonConvert.DeserializeObject<walletProcModel>(serialized_data);
			return Ok(rbkPaymentsHelper.PaymentDone_Update(rootobj));
		}


		[Route("recieptData")]
		[HttpPost]
		public IHttpActionResult recieptData(dynamic data)
		{
			string serialized_data = token_gen.Authorize_aesdecrpty(data);
			recieptModel rootobj = JsonConvert.DeserializeObject<recieptModel>(serialized_data);
			return Ok(rbkPaymentsHelper.recieptData(rootobj));
		}

	}

	public static class rbkPaymentsHelper
	{
		static CommonSPHel _Hel = new CommonSPHel();
		static string EncryptionKey = "R@m!nf0@!@#";
		private static readonly HttpClient client = new HttpClient();



		public static dynamic paymentOrderDetails(rbkPaymentsModel obj)
		{
			dynamic objdata = new ExpandoObject();
			try
			{

				obj.encrypted_data = obj.encrypted_data.Replace(" ", "+");
				string decrypted_text = EncryptDecryptAlgoritham.DecryptStringAES(obj.encrypted_data, "3fee5395f01bee349feed65629bd442a", obj.iv);

				var input = new
				{
					id = obj.orderId
				};
				string input_data = JsonConvert.SerializeObject(input);
				decModel decData = JsonConvert.DeserializeObject<decModel>(decrypted_text);
				decData.PS_TXN_ID = decData.PS_TXN_ID;//DateTime.Now.ToString("yyyymmddhhmmssfff");
				string url = "https://hub.rbk.apagros.ap.gov.in/v1/gew/orderDetails";
				string response = POST_RequestAsync(url, input_data);
				orderDerailsRespModel rootobj = JsonConvert.DeserializeObject<orderDerailsRespModel>(response);

				if (rootobj.Status == 200)
				{
					resProcModel objRes = new resProcModel();
					objRes.type = "7";
					objRes.txnId = decData.PS_TXN_ID;
					objRes.orderId = obj.orderId;
					objRes.deptId = "11";
					objRes.statusCode = rootobj.Status.ToString();
					objRes.remarks = rootobj.Message;
					objRes.serviceId = "110102601";
					objRes.amount = rootobj.Data.Amount.ToString();
					objRes.serviceCharge = "0"; //rootobj.Data.Amount.ToString();
					objRes.totalAmount = (0 + rootobj.Data.Amount).ToString();

					DataTable dt = resProc(objRes);
					if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
					{
						rootobj.Data.gswsTxnId = decData.PS_TXN_ID;
						objdata.status = true;
						objdata.result = rootobj.Data;
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
					objdata.result = rootobj.Message;
				}
			}
			catch (Exception ex)
			{
				objdata.status = false;
				objdata.result = ex.Message.ToString();

			}
			return objdata;
		}

		public static dynamic orderDetails(rbkPaymentsModel obj)
		{
			dynamic objdata = new ExpandoObject();
			try
			{

				var input = new
				{
					id = obj.orderId
				};
				string input_data = JsonConvert.SerializeObject(input);
				string url = "https://hub.rbk.apagros.ap.gov.in/v1/gew/orderDetails";
				string response = POST_RequestAsync(url, input_data);
				orderDerailsRespModel rootobj = JsonConvert.DeserializeObject<orderDerailsRespModel>(response);

				if (rootobj.Status == 200)
				{
					resProcModel objRes = new resProcModel();
					objRes.type = "8"; //update
					objRes.txnId = obj.orderId;
					DataTable dt1 = resProc(objRes);
					objdata.status = true;
					objdata.result = rootobj.Data;
					objdata.OrderDetails = dt1;
				}
				else
				{
					objdata.status = false;
					objdata.result = rootobj.Message;
				}
			}
			catch (Exception ex)
			{
				objdata.status = false;
				objdata.result = ex.Message.ToString();

			}
			return objdata;
		}

		public static dynamic makePayment(walletProcModel obj)
		{
			dynamic objdata = new ExpandoObject();
			try
			{
				urlRedirectionModel objPaymentModel = new urlRedirectionModel();
				objPaymentModel.GSWSCODE = obj.GSWSCODE;
				objPaymentModel.OPERATORID = obj.OPERATORID;
				objPaymentModel.DEPTCODE = "11";
				objPaymentModel.SERVICECODE = "1104";
				objPaymentModel.APPLICATIONNO = obj.APPLICATIONNO;
				objPaymentModel.CONSUMERNAME = obj.CONSUMERNAME;
				objPaymentModel.GSWSREFNO = obj.GSWSREFNO;
				objPaymentModel.PAYMODE = "C";
				objPaymentModel.userCharges = "0";
				objPaymentModel.serviceAmt = obj.serviceAmt;
				objPaymentModel.CallBackURI = "http://uat.gramawardsachivalayam.ap.gov.in/GSWSUAT/#!/rbkPaymentResp"; // "http://localhost:3831/#!/rbkPaymentResp";
				

				string serialized_string = JsonConvert.SerializeObject(objPaymentModel);
				string url = "http://43.241.39.112/STG/WalletPayment/PaymentConfirm?walletOne=" + encryptString(serialized_string);
				obj.type = "1";
				obj.userCharges = "0";
				obj.totalAmount = (float.Parse(obj.serviceAmt, CultureInfo.InvariantCulture.NumberFormat) + float.Parse(obj.userCharges, CultureInfo.InvariantCulture.NumberFormat)).ToString();
				DataTable dt = walletProc(obj);
				if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
				{
					objdata.status = true;
					objdata.result = url;
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

		public static dynamic PaymentDone_Update(walletProcModel obj1)
		{
			dynamic objdata = new ExpandoObject();
			try
			{
				

						var input = new
						{
							id = obj1.APPLICATIONNO,
							transactionId = obj1.GSWSREFNO
						};
						string input_data = JsonConvert.SerializeObject(input);
						string url = "https://hub.rbk.apagros.ap.gov.in/v1/gew/updateTransaction"; //"https://hub.nukkadshops.com/v1/gew/updateTransaction";
						string response = POST_RequestAsync(url, input_data);
				string mappath1 = HttpContext.Current.Server.MapPath("RBKPaymentUpdationLogs");
				Task WriteTask1 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath1, response));

				rbkPayRespModel rootobj = JsonConvert.DeserializeObject<rbkPayRespModel>(response);
						if (rootobj.status == 200)
						{
					resProcModel objRes = new resProcModel();
					objRes.type = "9"; //update
					objRes.txnId = obj1.GSWSREFNO;
					objRes.orderId = obj1.APPLICATIONNO;
					objRes.isPaymentSuccess = "1";
					DataTable dt1 = resProc(objRes);

					if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0]["STATUS"].ToString() == "1")
					{
						var Innerinput = new
							{
								id = obj1.APPLICATIONNO
							};
							input_data = JsonConvert.SerializeObject(Innerinput);
							url = "https://hub.rbk.apagros.ap.gov.in/v1/gew/orderDetails";
							response = POST_RequestAsync(url, input_data);
							orderDerailsRespModel rootobj1 = JsonConvert.DeserializeObject<orderDerailsRespModel>(response);
							if (rootobj1.Status == 200)
							{
								//call wallet table and resp table and txn cancel servie
								objdata.status = true;
								objdata.rbkDetails = rootobj1.Data;
								objdata.walletDetails = obj1;
								objdata.orderDetails = dt1;
							}
							else
							{
								//Auto Reconcile Module
								walletAutoReconcile(obj1.APPLICATIONNO, obj1.GSWSREFNO);
								objdata.status = false;
								objdata.result = "Failed Make Payment Please try again !!!";
							}
						}
						else
						{
							//Auto Reconcile Module
							walletAutoReconcile(obj1.APPLICATIONNO, obj1.GSWSREFNO);
							objdata.status = false;
						objdata.result = rootobj.message.ToString();
						}
					}
					else
					{

					resProcModel objRes = new resProcModel();
					objRes.type = "9"; //update
					objRes.txnId = obj1.GSWSREFNO;
					objRes.orderId = obj1.APPLICATIONNO;
					objRes.isPaymentSuccess = "2";
					//DataTable dt1 = resProc(objRes);
					//Auto Reconcile Module
					walletAutoReconcile(obj1.APPLICATIONNO, obj1.GSWSREFNO);
						objdata.status = false;
						objdata.result = "Failed Make Payment Please try again !!!";

					}

				
			}
			catch (Exception ex)
			{
				string mappath2 = HttpContext.Current.Server.MapPath("MakePaymentExceptionLogs");
				Task WriteTask2 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath2, "Error from Paymentdone Update:"+ex.Message.ToString()+JsonConvert.SerializeObject(obj1)));
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
				string serialized_data = decryptString(obj.encryptedString);
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
						DataTable dt = walletProc(walletObj);
						if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
						{
							resProcModel objRes = new resProcModel();
							objRes.type = "5";
							objRes.txnId = objResp.GSWSREFNO;
							DataTable dt1 = resProc(objRes);
							if (dt1 != null && dt1.Rows.Count > 0)
							{

								var input = new
								{
									id = objResp.APPLICATIONNO,
									transactionId = objResp.GSWSREFNO
								};
								string input_data = JsonConvert.SerializeObject(input);
								string url = "https://hub.nukkadshops.com/v1/gew/updateTransaction";
								string response = POST_RequestAsync(url, input_data);
								rbkPayRespModel rootobj = JsonConvert.DeserializeObject<rbkPayRespModel>(response);
								if (rootobj.status == 200)
								{
									var Innerinput = new
									{
										id = objResp.APPLICATIONNO
									};
									input_data = JsonConvert.SerializeObject(Innerinput);
									url = "https://hub.nukkadshops.com/v1/gew/orderDetails";
									response = POST_RequestAsync(url, input_data);
									orderDerailsRespModel rootobj1 = JsonConvert.DeserializeObject<orderDerailsRespModel>(response);
									if (rootobj1.Status == 200)
									{
										//call wallet table and resp table and txn cancel servie
										objdata.status = true;
										objdata.rbkDetails = rootobj1.Data;
										objdata.walletDetails = objResp;
										objdata.orderDetails = dt1;
									}
									else
									{
										//Auto Reconcile Module
										walletAutoReconcile(objResp.APPLICATIONNO, objResp.GSWSREFNO);
										objdata.status = false;
										objdata.result = "Failed Make Payment Please try again !!!";
									}
								}
								else
								{
									//Auto Reconcile Module
									walletAutoReconcile(objResp.APPLICATIONNO, objResp.GSWSREFNO);
									objdata.status = false;
									objdata.result = "Failed Make Payment Please try again !!!";
								}
							}
							else
							{
								//Auto Reconcile Module
								walletAutoReconcile(objResp.APPLICATIONNO, objResp.GSWSREFNO);
								objdata.status = false;
								objdata.result = "Failed Make Payment Please try again !!!";

							}

						}
						else
						{
							//Auto Reconcile Module
							walletAutoReconcile(objResp.APPLICATIONNO, objResp.GSWSREFNO);
							objdata.status = false;
							objdata.result = "Failed Make Payment Please try again !!!";
						}



					}
					catch (Exception ex1)
					{
						//Auto Reconcile Module
						walletAutoReconcile(objResp.APPLICATIONNO, objResp.GSWSREFNO);
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

		public static void walletAutoReconcile(string orderId, string txnId)
		{

			try
			{
				if (GSWSResTxnCancel(orderId, txnId, "2", "1") && GSWSWalletTxnCancel(orderId, txnId, "3", "1"))
				{
					if (WalletTxnCancel(orderId))
					{
						if (GSWSResTxnCancel(orderId, txnId, "3", null, "1") && GSWSWalletTxnCancel(orderId, txnId, "4", null, "1"))
						{
							if (RBKAPITxnCancel(orderId))
							{
								GSWSResTxnCancel(orderId, txnId, "3", null, null, "1");
								GSWSWalletTxnCancel(orderId, txnId, "5", null, null, "1");
							}
							else
							{
								GSWSWalletTxnCancel(orderId, txnId, "4", null, null, "0");
								GSWSResTxnCancel(orderId, txnId, "5", null, null, "0");
							}
						}
						else
						{
							GSWSResTxnCancel(orderId, txnId, "3", null, "0");
							GSWSWalletTxnCancel(orderId, txnId, "4", null, "0");
						}
					}
					else
					{
						GSWSWalletTxnCancel(orderId, txnId, "4", null, "0");
						GSWSResTxnCancel(orderId, txnId, "3", null, "0");
					}
				}
				else
				{
					GSWSWalletTxnCancel(orderId, txnId, "3", "0");
					GSWSResTxnCancel(orderId, txnId, "2", "0");
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public static bool RBKAPITxnCancel(string applicationId)
		{
			try
			{
				var input1 = new
				{
					id = applicationId
				};
				string input_data = JsonConvert.SerializeObject(input1);
				string url = "https://hub.nukkadshops.com/v1/gew/cancelPayment";
				string response = POST_RequestAsync(url, input_data);
				rbkPayRespModel respObj = JsonConvert.DeserializeObject<rbkPayRespModel>(response);
				if (respObj.status == 200)
					return true;
				return false;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public static bool WalletTxnCancel(string orderId)
		{
			string response = GET_RequestAsync(orderId);
			walletReconModel obj = JsonConvert.DeserializeObject<walletReconModel>(response);
			string decData = decryptString(obj.rps);
			walletRconModel rootobj = JsonConvert.DeserializeObject<walletRconModel>(decData);
			if (rootobj.Status.ERRORCODE == "101")
				return true;
			return false;
		}

		public static bool GSWSWalletTxnCancel(string orderId, string txnId, string type, string isPaymentReversal = null, string isPaymentRevSuccess = null, string isTxnRevSeccess = null)
		{
			try
			{
				walletProcModel walletObj = new walletProcModel();
				walletObj.type = type;
				walletObj.APPLICATIONNO = orderId;
				walletObj.GSWSREFNO = txnId;
				walletObj.txnReversal = isPaymentReversal;
				walletObj.txnReversalSuccess = isPaymentRevSuccess;
				walletObj.deptTxnCancelled = isTxnRevSeccess;
				DataTable dt = walletProc(walletObj);
				if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
					return true;
				return false;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public static bool GSWSResTxnCancel(string orderId, string txnId, string type, string isPaymentReversal = null, string isPaymentRevSuccess = null, string isTxnRevSeccess = null)
		{
			try
			{
				resProcModel obj = new resProcModel();
				obj.txnId = txnId;
				obj.type = type;
				obj.orderId = orderId;
				obj.isPaymentReversal = isPaymentReversal;
				DataTable dt = resProc(obj);
				if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
					return true;
				return false;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public static string encryptString(string input)
		{
			try
			{
				byte[] clearBytes = Encoding.Unicode.GetBytes(input);
				using (Aes encryptor = Aes.Create())
				{
					Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x52, 0x40, 0x57, 0x46, 0x30, 0x52, 0x57, 0x41, 0x74, 0x6c, 0x54, 0x64 });
					encryptor.Key = pdb.GetBytes(32);
					encryptor.IV = pdb.GetBytes(16);
					using (MemoryStream ms = new MemoryStream())
					{
						using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
						{
							cs.Write(clearBytes, 0, clearBytes.Length);
							cs.Close();
						}
						return Convert.ToBase64String(ms.ToArray());
					}
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static string decryptString(string input)
		{
			try
			{
				input = input.Replace(" ", "+");
				byte[] cipherBytes = Convert.FromBase64String(input);
				using (Aes encryptor = Aes.Create())
				{
					Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x52, 0x40, 0x57, 0x46, 0x30, 0x52, 0x57, 0x41, 0x74, 0x6c, 0x54, 0x64 });
					encryptor.Key = pdb.GetBytes(32);
					encryptor.IV = pdb.GetBytes(16);
					using (MemoryStream ms = new MemoryStream())
					{
						using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
						{
							cs.Write(cipherBytes, 0, cipherBytes.Length);
							cs.Close();
						}
						return Encoding.Unicode.GetString(ms.ToArray());
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public static string POST_RequestAsync(string uri, string json, int count = 0)
		{
			try
			{
				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				var request = (HttpWebRequest)WebRequest.Create(uri);
				request.Credentials = CredentialCache.DefaultCredentials;
				WebProxy myProxy = new WebProxy();
				
				request.Proxy = myProxy;
				request.Headers["X-Nukkad-Api-Token"] = "adc86bed-0022-5bdf-922f-997c0ad8e48e";//"ec287cd1-fab3-4127-b554-78e69311f9ba";
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

				//var content = new FormUrlEncodedContent(json);
				//var response = await client.PostAsync(uri, content);
				//var responseString = await response.Content.ReadAsStringAsync();
				//return responseString;
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

		public static string GET_RequestAsync(string orderId, int count = 0)
		{
			try
			{
				string uri = "http://43.241.39.112/WalletPayment/PaymentRefund?walletOne=" + encryptString(orderId);
				var request = (HttpWebRequest)WebRequest.Create(uri);
				request.Headers["X-Nukkad-Api-Token"] = "ec287cd1-fab3-4127-b554-78e69311f9ba";
				//request.ContentType = "application/json";
				//request.Method = "POST";

				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				using (Stream stream = response.GetResponseStream())
				using (StreamReader reader = new StreamReader(stream))
				{
					return reader.ReadToEnd();
				}

				//var response = (HttpWebResponse)request.GetResponse();
				//string result = "";
				//using (var streamReader = new StreamReader(response.GetResponseStream()))
				//{
				//    result = streamReader.ReadToEnd();
				//}
				//return result;
			}
			catch (Exception ex)
			{
				if (count < 3)
				{
					Thread.Sleep(3000);
					count++;
					return GET_RequestAsync(orderId, count);
				}
				else
				{
					throw ex;
				}
			}


		}


		public static DataTable walletProc(walletProcModel obj)
		{
			try
			{
				OracleCommand cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_RBK_REQ_RES_INS_PROC";
				cmd.Parameters.Add("ptype", OracleDbType.Varchar2).Value = obj.type;
				cmd.Parameters.Add("pAPPLICANT_NAME", OracleDbType.Varchar2).Value = obj.CONSUMERNAME;
				cmd.Parameters.Add("prbk_name", OracleDbType.Varchar2).Value = obj.rbkName;
				cmd.Parameters.Add("phub_name", OracleDbType.Varchar2).Value = obj.hubName;
				cmd.Parameters.Add("puser_charges", OracleDbType.Varchar2).Value = obj.userCharges;
				cmd.Parameters.Add("pservice_charges", OracleDbType.Varchar2).Value = obj.serviceAmt;
				cmd.Parameters.Add("ptotal_amount", OracleDbType.Varchar2).Value = obj.totalAmount;
				cmd.Parameters.Add("pdept_txn_id", OracleDbType.Varchar2).Value = obj.APPLICATIONNO;
				cmd.Parameters.Add("pgsws_txn_id", OracleDbType.Varchar2).Value = obj.GSWSREFNO;
				cmd.Parameters.Add("pwallet_txn_id", OracleDbType.Varchar2).Value = obj.walletTxnId;
				cmd.Parameters.Add("pis_payment_status", OracleDbType.Varchar2).Value = obj.paymentStatus;
				cmd.Parameters.Add("pis_txn_reversal", OracleDbType.Varchar2).Value = obj.txnReversal;
				cmd.Parameters.Add("pis_txn_reversal_succ", OracleDbType.Varchar2).Value = obj.txnReversalSuccess;
				cmd.Parameters.Add("pis_dept_txn_canciled", OracleDbType.Varchar2).Value = obj.deptTxnCancelled;
				cmd.Parameters.Add("pgsws_code", OracleDbType.Varchar2).Value = obj.GSWSCODE;
				cmd.Parameters.Add("ptxn_initiate_date ", OracleDbType.TimeStamp).Value = DateTime.Now;
				cmd.Parameters.Add("ptxn_response_date ", OracleDbType.TimeStamp).Value = DateTime.Now;
				cmd.Parameters.Add("p_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtstatus = _Hel.GetProdgswsDataAdapter(cmd);
				return dtstatus;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static DataTable reqProc(reqProcModel obj)
		{
			try
			{
				OracleCommand cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_RBK_IN_REQ_PROC";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2).Value = obj.type;
				cmd.Parameters.Add("P_TRANSACTION_ID", OracleDbType.Varchar2).Value = obj.txnId;
				cmd.Parameters.Add("P_DEPT_ID", OracleDbType.Varchar2).Value = obj.deptId;
				cmd.Parameters.Add("P_SERVICE_NAME", OracleDbType.Varchar2).Value = obj.serviceName;
				cmd.Parameters.Add("P_IP_ADDRESS", OracleDbType.Varchar2).Value = obj.ipAddress;
				cmd.Parameters.Add("P_SYSTEM_NAME", OracleDbType.Varchar2).Value = obj.systemName;
				cmd.Parameters.Add("P_DISTRICT_ID", OracleDbType.Varchar2).Value = obj.districtId;
				cmd.Parameters.Add("P_MANDAL_ID", OracleDbType.Varchar2).Value = obj.mandalId;
				cmd.Parameters.Add("P_GP_WARD_ID", OracleDbType.Varchar2).Value = obj.gpWardId;
				cmd.Parameters.Add("P_GSWS_ID", OracleDbType.Varchar2).Value = obj.secId;
				cmd.Parameters.Add("P_LOGIN_USER", OracleDbType.Varchar2).Value = obj.loginuser;
				cmd.Parameters.Add("P_TYPE_OF_REQUEST", OracleDbType.Varchar2).Value = obj.typeOfRequest;
				cmd.Parameters.Add("P_ACTIVE_STATUS", OracleDbType.Varchar2).Value = obj.activeStatus;
				cmd.Parameters.Add("P_HOD_ID", OracleDbType.Varchar2).Value = obj.hodId;
				cmd.Parameters.Add("P_URL_ID", OracleDbType.Varchar2).Value = obj.urlId;
				cmd.Parameters.Add("P_DESIGNATION_ID ", OracleDbType.Varchar2).Value = obj.desgId;
				cmd.Parameters.Add("P_CITIZEN_NAME ", OracleDbType.Varchar2).Value = obj.citizenName;
				cmd.Parameters.Add("P_MOBILE_NUMBER", OracleDbType.Varchar2).Value = obj.mobileNumber;
				cmd.Parameters.Add("P_UID_NUM", OracleDbType.Varchar2).Value = obj.uidNum;
				cmd.Parameters.Add("p_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtstatus = _Hel.GetProdgswsDataAdapter(cmd);
				return dtstatus;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static DataTable resProc(resProcModel obj)
		{
			try
			{
				OracleCommand cmd = new OracleCommand();
				cmd.InitialLONGFetchSize = 1000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "GSWS_APSEEDS_CORP_RESP_PROC"; //"GSWS_RBK_IN_RESPONSE_PROC";
				cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2).Value = obj.type;
				cmd.Parameters.Add("P_TRANSACTION_ID", OracleDbType.Varchar2).Value = obj.txnId;
				cmd.Parameters.Add("P_DEPT_TRANS_ID", OracleDbType.Varchar2).Value = obj.orderId;
				cmd.Parameters.Add("P_DEPT_ID", OracleDbType.Varchar2).Value = obj.deptId;
				cmd.Parameters.Add("P_STATUS_CODE", OracleDbType.Varchar2).Value = obj.statusCode;
				cmd.Parameters.Add("P_REMARKS", OracleDbType.Varchar2).Value = obj.remarks;
				cmd.Parameters.Add("P_SERVICE_ID", OracleDbType.Varchar2).Value = obj.serviceId;
				cmd.Parameters.Add("P_AMOUNT", OracleDbType.Varchar2).Value = obj.amount;
				cmd.Parameters.Add("P_SERVICE_CHARGE", OracleDbType.Varchar2).Value = obj.serviceCharge;
				cmd.Parameters.Add("P_TOTAL_AMOUNT", OracleDbType.Varchar2).Value = obj.totalAmount;
				cmd.Parameters.Add("P_IS_PAYMENT_SUCCESS", OracleDbType.Varchar2).Value = obj.isPaymentSuccess;
				cmd.Parameters.Add("P_IS_PAYMENT_REVERSAL", OracleDbType.Varchar2).Value = obj.isPaymentReversal;
				cmd.Parameters.Add("P_IS_PAY_REV_SUCCESS", OracleDbType.Varchar2).Value = obj.isPaymentRevSuccess;
				cmd.Parameters.Add("P_IS_TXNID_CANCEL_SUCCESS", OracleDbType.Varchar2).Value = obj.isTxnCancelSuccess;
				cmd.Parameters.Add("p_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
				DataTable dtstatus = _Hel.GetProdgswsDataAdapter(cmd);
				return dtstatus;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


	}

	public class walletRconModel
	{
		public Status Status { get; set; }
	}

	public class Status
	{
		public string ERRORMSG { get; set; }
		public string ERRORCODE { get; set; }
		public string APPLICATIONNO { get; set; }
		public string RefundID { get; set; }
	}

	public class walletReconModel
	{
		public string rps { get; set; }
	}

	public class decModel
	{
		public string USERNAME { get; set; }
		public string PASSWORD { get; set; }
		public string PS_TXN_ID { get; set; }
		public string RETURN_URL { get; set; }
	}
	public class rbkPayRespModel
	{
		public string response { get; set; }
		public int status { get; set; }
		public string message { get; set; }
	}

	public class walletRespModel
	{
		public string ERRORMSG { get; set; }
		public string ERRORCODE { get; set; }
		public string APPLICATIONNO { get; set; }
		public string WALLETREFNO { get; set; }
		public string GSWSREFNO { get; set; }
		public string serviceAmt { get; set; }
		public string userCharges { get; set; }
		public string secId { get; set; }
	}
	public class recieptModel
	{
		public string secId { get; set; }
		public string encryptedString { get; set; }
	}

	public class walletProcModel : urlRedirectionModel
	{
		public string type { get; set; }
		public string rbkName { get; set; }
		public string hubName { get; set; }
		public string totalAmount { get; set; }
		public string walletTxnId { get; set; }
		public string paymentStatus { get; set; }
		public string txnReversal { get; set; }
		public string txnReversalSuccess { get; set; }
		public string deptTxnCancelled { get; set; }
		public string serviceCharge { get; set; }
	}

	public class urlRedirectionModel
	{

		public string GSWSCODE { get; set; }
		public string OPERATORID { get; set; }
		public string DEPTCODE { get; set; }
		public string SERVICECODE { get; set; }
		public string APPLICATIONNO { get; set; }
		public string CONSUMERNAME { get; set; }
		public string GSWSREFNO { get; set; }
		public string PAYMODE { get; set; }
		public string userCharges { get; set; }
		public string serviceAmt { get; set; }
		public string CallBackURI { get; set; }

	}
	public partial class orderDerailsRespModel
	{
		public bool Response { get; set; }
		public long Status { get; set; }
		public string Message { get; set; }
		public Data Data { get; set; }
	}

	public partial class Data
	{
		public string gswsTxnId { get; set; }
		public long Amount { get; set; }
		public string HubName { get; set; }
		public string hubId { get; set; }
		public string hubMobile { get; set; }
		public string hubOwner { get; set; }
		public string RbkName { get; set; }
		public string rbkId { get; set; }
		public string Name { get; set; }
		public string OrderStatus { get; set; }
		public long PaymentStatus { get; set; }
		public string vaaName { get; set; }
		public string vaaNumber { get; set; }

		public List<items> items { get; set; }
	}

	public class items
	{
		public string name { get; set; }
		public string qty { get; set; }
	}

	public class rbkPaymentsModel
	{
		public string encrypted_data { get; set; }
		public string iv { get; set; }
		public string serviceId { get; set; }
		public string uniqueId { get; set; }
		public string orderId { get; set; }
		public string deptId { get; set; }
		public string serviceName { get; set; }
		public string districtId { get; set; }
		public string mandalId { get; set; }
		public string gpWardId { get; set; }
		public string secId { get; set; }
		public string loginuser { get; set; }
		public string typeOfRequest { get; set; }
		public string activeStatus { get; set; }
		public string hodId { get; set; }
		public string urlId { get; set; }
		public string desgId { get; set; }
		public string mobileNumber { get; set; }
	}

	public class resProcModel : rbkPaymentsModel
	{
		public string type { get; set; }
		public string txnId { get; set; }
		public string statusCode { get; set; }
		public string remarks { get; set; }
		public string amount { get; set; }
		public string serviceCharge { get; set; }
		public string totalAmount { get; set; }
		public string isPaymentSuccess { get; set; }
		public string isPaymentReversal { get; set; }
		public string isPaymentRevSuccess { get; set; }
		public string isTxnCancelSuccess { get; set; }
		public string userCharge { get; set; }
	}

	public class reqProcModel : rbkPaymentsModel
	{
		public string type { get; set; }
		public string txnId { get; set; }
		public string ipAddress { get; set; }
		public string systemName { get; set; }
		public string citizenName { get; set; }
		public string uidNum { get; set; }
	}

}
