using gswsBackendAPI.DL.DataConnection;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace gswsBackendAPI.DL.CommonHel
{
	public class captchahelper:LoginSPHelper
	{
		public dynamic check_s_captch(captch root)
		{
			captchgens retrn = new captchgens();
			try
			{

				Random rn = new Random();
				int rnval = rn.Next(100000, 999999);
				var ids = "";
				ids = DateTime.Now.ToString("ddMMyyyyhhmmssfff") + rnval.ToString();


				Bitmap objBitmap = new Bitmap(150, 90);
				Graphics objGraphics = Graphics.FromImage(objBitmap);
				objGraphics.Clear(Color.White);
				Random objRandom = new Random();
				objGraphics.DrawLine(Pens.White, objRandom.Next(0, 50), objRandom.Next(10, 30), objRandom.Next(0, 200), objRandom.Next(0, 50));
				objGraphics.DrawRectangle(Pens.White, objRandom.Next(0, 20), objRandom.Next(0, 20), objRandom.Next(50, 80), objRandom.Next(0, 20));
				objGraphics.DrawLine(Pens.White, objRandom.Next(0, 20), objRandom.Next(10, 50), objRandom.Next(100, 200), objRandom.Next(0, 80));
				Brush objBrush =
					default(Brush);
				HatchStyle[] aHatchStyles = new HatchStyle[]
				{
			   HatchStyle.LargeGrid, HatchStyle.LightDownwardDiagonal, HatchStyle.LightHorizontal
				};
				RectangleF oRectangleF = new RectangleF(0, 0, 400, 400);
				objBrush = new HatchBrush(aHatchStyles[objRandom.Next(aHatchStyles.Length - 3)], Color.FromArgb((objRandom.Next(100, 255)), (objRandom.Next(100, 255)), (objRandom.Next(100, 255))), Color.Blue);
				objGraphics.FillRectangle(objBrush, oRectangleF);
				string captchaText = string.Format("{0}", objRandom.Next(100000, 999999));
				Font objFont = new Font("Courier New", 25, FontStyle.Bold);
				objGraphics.DrawString(captchaText, objFont, Brushes.White, 20, 30);
				objGraphics.Flush();
				objGraphics.Dispose();

				//string path = HttpContext.Current.Server.MapPath("../../capth");

				string fileName = captchaText + ".Gif";
				//path = Path.Combine(path, fileName);
				string newpath = ids + fileName;
				objBitmap.Save(Path.Combine(HttpContext.Current.Server.MapPath("../../capth"), newpath), ImageFormat.Gif);

				root.type = "1";
				root.Capchid = captchaText;
				root.id = ids.ToString().Trim();

				//bool captchgen = true;
				bool captchgen = GSWS_SP_IN_CAPTCHA(root);
				if (captchgen == true)
				{

					byte[] imageBytes = System.IO.File.ReadAllBytes(Path.Combine(HttpContext.Current.Server.MapPath("../../capth"), newpath));
					string base64String = Convert.ToBase64String(imageBytes);
					retrn.idval = Encrypt(ids, "");

					retrn.code = "100";
					retrn.imgurl = base64String;

					//DirectoryInfo diInfo = new DirectoryInfo(Path.Combine(HttpContext.Current.Server.MapPath("../../capth")));
					//FileInfo[] files = diInfo.GetFiles();
					//for (int i = 0; i < files.Length; i++)
					//{
					string filePath = Path.Combine(HttpContext.Current.Server.MapPath("../../capth/" + newpath));
					if (File.Exists(filePath))
					{
						File.Delete(filePath);
					}
					//}
					return retrn;

				}
				else
				{
					retrn.idval = "";
					retrn.code = "99";
					retrn.imgurl = "Error.htm";
					return retrn;
				}
			}
			catch (Exception ex)
			{
				string mappath = HttpContext.Current.Server.MapPath("ExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_ReportLog_Exception(mappath, "Error Deleting Beneficiary Data" + ex.Message.ToString()));

				retrn.code = "102";
				retrn.Reason = "Capatcha Not Loaded Properly";

				return retrn;
			}
		}

		public static string Encrypt(string strMessage, string sTokenKey)
		{
			return Convert.ToBase64String(Utilities.Encrypt(Encoding.UTF8.GetBytes(strMessage), Utilities.suresh(sTokenKey)));
		}
		public static string Decrypt(string strEncMsg, string sTokenKey)
		{
			return Encoding.UTF8.GetString(Utilities.Decrypt(Convert.FromBase64String(strEncMsg), Utilities.suresh(sTokenKey)));
		}

		public string CaptchVerify(string cap, string confirm)
		{
			try
			{

				captch objcap = new captch();
				objcap.type = "2";
				objcap.id = Decrypt(confirm, "");
				objcap.Capchid = cap;
				bool status = GSWS_SP_IN_CAPTCHA(objcap);
				if (status)
				{
					return "Success";
				}
				else
				{
					return "Failure";
				}


			}
			catch (Exception ex)
			{
				//_log.Error("An error occurred in LoginSPHelper => GetCaptchVerify: " + ex.Message + "__" + ex.InnerException + "__" + ex.StackTrace.ToString());
				throw ex;
			}

		}
		public string GetCaptchVerify(LoginModel ObjL)
		{
			try
			{
				captch objcap = new captch();
				objcap.type = "2";
				objcap.id = Decrypt(ObjL.ConfirmCaptch, "");
				objcap.Capchid = ObjL.Captcha;
				bool status = GSWS_SP_IN_CAPTCHA(objcap);
				if (status)
				{
					return "Success";
				}
				else
				{
					return "Failure";
				}


			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public bool GetroleAccess(captch obj)
		{
			try
			{
				if (obj.SOURCE.ToUpper() == "MAINDASHBOARD" || obj.SOURCE.ToUpper() == "LOGIN")
				{
					return true;
				}
				int val = GSWS_SP_IN_RoleAccess(obj);
				if (val == 1)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

	}
	public class Utilities
	{
		public static RijndaelManaged suresh(string secretKey)
		{
			byte[] numArray = new byte[16];
			byte[] bytes = Encoding.UTF8.GetBytes(secretKey);
			Array.Copy((Array)bytes, (Array)numArray, Math.Min(numArray.Length, bytes.Length));
			RijndaelManaged rijndaelManaged = new RijndaelManaged();
			rijndaelManaged.Mode = CipherMode.CBC;
			rijndaelManaged.Padding = PaddingMode.PKCS7;
			rijndaelManaged.KeySize = 128;
			rijndaelManaged.BlockSize = 128;
			rijndaelManaged.Key = numArray;
			rijndaelManaged.IV = numArray;
			return rijndaelManaged;
		}

		public static byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
		{
			return rijndaelManaged.CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);
		}

		public static byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
		{
			return rijndaelManaged.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
		}

		public static string Encrypt(string strMessage, string sTokenKey)
		{
			return Convert.ToBase64String(Utilities.Encrypt(Encoding.UTF8.GetBytes(strMessage), Utilities.suresh(sTokenKey)));
		}

		public static string Decrypt(string strEncMsg, string sTokenKey)
		{
			return Encoding.UTF8.GetString(Utilities.Decrypt(Convert.FromBase64String(strEncMsg), Utilities.suresh(sTokenKey)));
		}

		public static T GetFromQueryString<T>(string QString) where T : new()
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			foreach (string str in QString.Split("&".ToCharArray()))
			{
				string[] strArray = str.Split("=".ToCharArray());
				nameValueCollection.Add(strArray[0], HttpContext.Current.Server.UrlDecode(strArray[1]));
			}
			T obj1 = new T();
			foreach (PropertyInfo property in typeof(T).GetProperties())
			{
				string ValueToConvert = nameValueCollection[property.Name];
				object obj2 = Utilities.Parse(property.PropertyType, ValueToConvert);
				if (obj2 != null)
					property.SetValue((object)obj1, obj2, (object[])null);
			}
			return obj1;
		}

		public static object Parse(Type dataType, string ValueToConvert)
		{
			return TypeDescriptor.GetConverter(dataType).ConvertFromString((ITypeDescriptorContext)null, CultureInfo.InvariantCulture, ValueToConvert);
		}



		public static string SecurityKey()
		{
			return "RaithuBharosa@341";
		}
	}
}