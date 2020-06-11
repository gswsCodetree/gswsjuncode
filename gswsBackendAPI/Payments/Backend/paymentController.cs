using gswsBackendAPI.DL.CommonHel;
using gswsBackendAPI.DL.DataConnection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace gswsBackendAPI.Payments.Backend
{
    [RoutePrefix("api/payment")]
    public class paymentController : ApiController
    {
        paymentHelper _Hel = new paymentHelper();


        [HttpPost]
        [Route("Order")]
        public IHttpActionResult OrderDetails(dynamic data)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                //    byte[] dataArray = Convert.FromBase64String(data);
                //    string decodedString = Encoding.UTF8.GetString(dataArray);
                string serialized_data = JsonConvert.SerializeObject(data);
                MakePaymentModel rootobj = JsonConvert.DeserializeObject<MakePaymentModel>(serialized_data);

                //     encryptedData = encryptedData.Replace(' ', '+');    
                Decryptdatamodel decryptModel = new Decryptdatamodel();
                decryptModel.encryprtext = rootobj.encryptedData;
                decryptModel.key = "3fee5395f01bee349feed65629bd442a";
                decryptModel.Ivval = rootobj.iv;

                string mappath = HttpContext.Current.Server.MapPath("OrderDetails");
                Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, JsonConvert.SerializeObject(decryptModel)));

                string result = EncryptDecryptAlgoritham.DecryptStringAES(decryptModel.encryprtext, decryptModel.key, decryptModel.Ivval);
                paymentModel obj = JsonConvert.DeserializeObject<paymentModel>(result);
                
                return Ok(_Hel.OrderDetails(obj));
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return Ok(objdata);

        }

        [HttpPost]
        [Route("walletpay")]
        public IHttpActionResult walletpay(dynamic data)
        {

            string serialized_data = JsonConvert.SerializeObject(data);
            MakePaymentModel rootobj = JsonConvert.DeserializeObject<MakePaymentModel>(serialized_data);

            Decryptdatamodel decryptModel = new Decryptdatamodel();
            decryptModel.encryprtext = rootobj.encryptedData;
            decryptModel.key = "3fee5395f01bee349feed65629bd442a";
            decryptModel.Ivval = rootobj.iv;


            string mappath = HttpContext.Current.Server.MapPath("MakePayment");
            Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, JsonConvert.SerializeObject(rootobj)));

            string result = EncryptDecryptAlgoritham.DecryptStringAES(decryptModel.encryprtext, decryptModel.key, decryptModel.Ivval);
            paymentModel obj = JsonConvert.DeserializeObject<paymentModel>(result);

            if (rootobj.type == "TA")
            {
                return Ok(_Hel.TAWalletMakePayment(obj, rootobj.otp));
            }
            else if (rootobj.type == "WONE")
            {

                return Ok(_Hel.walletOnemakePayment(obj));
            }
            else
            {
                return Ok("WONE");
            }

        }

        [HttpPost]
        [Route("getEncrypt")]
        public IHttpActionResult getEncrypt(dynamic data)
        {
            dynamic objenc = new ExpandoObject();
            try
            {
                string json = JsonConvert.SerializeObject(data);

                string iv = CryptLib.GenerateRandomIV(16);
                string key = CryptLib.getHashSha256("GSWS TEST", 32);

                string encrypttext = EncryptDecryptAlgoritham.EncryptStringAES(json, key, iv);

                objenc.Status = 100;
                objenc.encrypttext = encrypttext;
                objenc.key = iv;
                objenc.Reason = "";
                return Ok(objenc);
            }
            catch (Exception ex)
            {
                objenc.Status = 102;
                objenc.Reason = ex.Message.ToString();
                return Ok(objenc);
            }
        }

        [HttpPost]
        [Route("TAWalletEncryptedKey")]
        public IHttpActionResult TAWalletEncryptedKey(dynamic data)
        {
            string serialized_data = JsonConvert.SerializeObject(data);
            TAWalletpaymentModel rootobj = JsonConvert.DeserializeObject<TAWalletpaymentModel>(serialized_data);
            return Ok(_Hel.TAWalletEncryptedKey(rootobj));
        }

        [HttpPost]
        [Route("TAresponseDecrypt")]
        public IHttpActionResult TAresponseDecrypt(dynamic data)
        {
            string serialized_data = JsonConvert.SerializeObject(data);
            TAWalletpaymentModel rootobj = JsonConvert.DeserializeObject<TAWalletpaymentModel>(serialized_data);
            return Ok(_Hel.TAresponseDecrypt(rootobj));
        }



    }
}
