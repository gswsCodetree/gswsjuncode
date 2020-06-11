using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using gswsBackendAPI.DL.DataConnection;
using gswsBackendAPI.DL.CommonHel;

namespace gswsBackendAPI.Controllers
{
	
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.version = ConfigurationManager.AppSettings["Ver"].ToString();

			return View();
		}

		public ActionResult Dashboard()
		{
			ViewBag.version = ConfigurationManager.AppSettings["Ver"].ToString();

			return View();
		}

		public ActionResult Main()
		{
			ViewBag.version = ConfigurationManager.AppSettings["Ver"].ToString();

			return View();
		}
		public ActionResult Navarantanalu()
		{
			ViewBag.version = ConfigurationManager.AppSettings["Ver"].ToString();

			return View();
		}
		[HttpPost]
		//[Route("api/Rakshana/POSTData")]
		public string POSTData(FilesModel CPFileService)
		{
			try
			{
				var file = CPFileService.Attachment;
				string fileexten = (file.ContentType.Split('/')[1] ?? "").ToLower();
				int imagesize = file.ContentLength;
				if (fileexten == "pdf" && Utils.IsPdf(file))
				{

				}
				else
				{
					return "Failure Only PDF Files Accept";
				}
				if (imagesize > 5000000)
				{
					return "Failure File size exceeded 1 MB";
				}
				if (file == null) return "Please upload PDF";

				var location = Server.MapPath("~/GSWSUserManuals");
				var AddFolder = DateTime.Now.ToString("ddMMyy") + "\\" + DateTime.Now.ToString("HH").ToString() + "\\" + DateTime.Now.ToString("mm").ToString();
				location = location + "/" + AddFolder;


				Random rn = new Random();
				int val = rn.Next(1000000, 9999999);
				if (!Directory.Exists(location))
					Directory.CreateDirectory(location);
				var CertificateCategory = CPFileService.CertificateCategory.Replace("/", "_");

				var filename = CPFileService.CertifcateID + "_" + CertificateCategory + "." + fileexten;
				var filepath = location + "\\" + filename.Replace(" ", "_");
				file.SaveAs(filepath);
				string domainName = Request.Url.GetLeftPart(UriPartial.Authority);
				domainName=domainName+"/"+ ConfigurationManager.AppSettings["sitename"].ToString();

				var returnfileName = domainName + "GSWSUserManuals/" + AddFolder + "/" + filename.Replace(" ", "_");
				//var returnfileName = domainName + "/GVTESTAPP/ApplicationUploads/" + AddFolder + "/" + filename.Replace(" ", "_");

				//UnEmployeeHelper _hep = new UnEmployeeHelper();/CMyuvaNapp
				//	_hep.SaveCertificateImages(CPFileService, returnfileName);
				//string mappath1 = Server.MapPath("UpdateUserManualLogs");

				return returnfileName;
			}
			catch (Exception ex)
			{

				return "Failutre" + ex.Message;
			}

		}

		[HttpPost]
		//[Route("api/Rakshana/POSTData")]
		public string POSTAffidvitData(FilesModel CPFileService)
		{
			try
			{
				var file = CPFileService.Attachment;
				string fileexten = (file.ContentType.Split('/')[1] ?? "").ToLower();
				int imagesize = file.ContentLength;
				if (fileexten == "pdf" && Utils.IsPdf(file))
				{

				}
				else
				{
					return "Failure Only PDF Files Accept";
				}
				if (imagesize > 5000000)
				{
					return "Failure File size exceeded 1 MB";
				}
				if (file == null) return "Please upload PDF";

				var location = Server.MapPath("~/AffidavitFiles");
				var AddFolder = DateTime.Now.ToString("ddMMyy") + "\\" + DateTime.Now.ToString("HH").ToString() + "\\" + DateTime.Now.ToString("mm").ToString();
				location = location + "/" + AddFolder;


				Random rn = new Random();
				int val = rn.Next(1000000, 9999999);
				if (!Directory.Exists(location))
					Directory.CreateDirectory(location);
				var CertificateCategory = CPFileService.CertificateCategory.Replace("/", "_");

				var filename = CPFileService.CertifcateID + "_" + CertificateCategory + "." + fileexten;
				var filepath = location + "\\" + filename.Replace(" ", "_");
				file.SaveAs(filepath);
				string domainName = Request.Url.GetLeftPart(UriPartial.Authority);
				domainName = domainName + "/" + ConfigurationManager.AppSettings["sitename"].ToString();

				var returnfileName = domainName + "AffidavitFiles/" + AddFolder + "/" + filename.Replace(" ", "_");
				//var returnfileName = domainName + "/GVTESTAPP/ApplicationUploads/" + AddFolder + "/" + filename.Replace(" ", "_");

				//UnEmployeeHelper _hep = new UnEmployeeHelper();/CMyuvaNapp
				//	_hep.SaveCertificateImages(CPFileService, returnfileName);
				//string mappath1 = Server.MapPath("UpdateUserManualLogs");

				return returnfileName;
			}
			catch (Exception ex)
			{

				return "Failutre" + ex.Message;
			}

		}


		[HttpPost]
		//[Route("api/Rakshana/POSTData")]
		public string POSTImageData(FilesModel CPFileService)
		{
			try
			{
				var returnfileName = "";

				var file = CPFileService.Attachment;
				string fileexten = (file.ContentType.Split('/')[1] ?? "").ToLower();
				int imagesize = file.ContentLength;
				if (fileexten == "jpg" || fileexten == "jpeg")
				{
					if (file == null)
						return "Please upload Image";
					if (imagesize > 1000000)
					{
						return "Failure File size exceeded 1 MB";
					}

					using (var bitmap = new System.Drawing.Bitmap(file.InputStream))
					{
						var location = Server.MapPath("~/FeedBackImages");
						var AddFolder = DateTime.Now.ToString("ddMMyy") + "\\" + DateTime.Now.ToString("HH").ToString() + "\\" + DateTime.Now.ToString("mm").ToString();
						location = location + "/" + AddFolder;


						Random rn = new Random();
						int val = rn.Next(1000000, 9999999);
						if (!Directory.Exists(location))
							Directory.CreateDirectory(location);
						var CertificateCategory = CPFileService.CertificateCategory.Replace("/", "_");

						var filename = CPFileService.AadharCardNumber + "_" + CPFileService.CertifcateID + "_" + CertificateCategory + "." + fileexten;
						var filepath = location + "\\" + filename.Replace(" ", "_");
						file.SaveAs(filepath);
						string domainName = Request.Url.GetLeftPart(UriPartial.Authority);

						domainName = domainName + "/" + ConfigurationManager.AppSettings["sitename"].ToString();

						returnfileName = domainName + "FeedBackImages/" + AddFolder + "/" + filename.Replace(" ", "_");
					}

				}
				else
				{
					return "Failure Only JPG Files Accept";
				}

				//var returnfileName = domainName + "/GVTESTAPP/ApplicationUploads/" + AddFolder + "/" + filename.Replace(" ", "_");

				//UnEmployeeHelper _hep = new UnEmployeeHelper();/CMyuvaNapp
				//	_hep.SaveCertificateImages(CPFileService, returnfileName);
				//string mappath1 = Server.MapPath("UpdateUserManualLogs");

				return string.IsNullOrEmpty(returnfileName) ? "Failure Only JPG Files Accept" : returnfileName;
			}
			catch (Exception ex)
			{

				return "Failutre" + ex.Message;
			}

		}

		[HttpPost]
		//[Route("api/Rakshana/POSTData")]
		public string SecImageData(FilesModel CPFileService)
		{
			try
			{
				var returnfileName = "";

				var file = CPFileService.Attachment;
				string fileexten = (file.ContentType.Split('/')[1] ?? "").ToLower();
				int imagesize = file.ContentLength;
				if (fileexten == "jpg" || fileexten == "jpeg")
				{
					if (file == null)
						return "Failure Please upload Image";
					if (imagesize > 1000000)
					{
						return "Failure File size exceeded 1 MB";
					}

					using (var bitmap = new System.Drawing.Bitmap(file.InputStream))
					{
						var location = Server.MapPath("~/SecretariatImages");
						var AddFolder = DateTime.Now.ToString("ddMMyy") + "\\" + DateTime.Now.ToString("HH").ToString() + "\\" + DateTime.Now.ToString("mm").ToString();
						location = location + "/" + AddFolder;


						Random rn = new Random();
						int val = rn.Next(1000000, 9999999);
						if (!Directory.Exists(location))
							Directory.CreateDirectory(location);
						var CertificateCategory = CPFileService.CertificateCategory.Replace("/", "_");

						var filename = CPFileService.AadharCardNumber + "_" + CertificateCategory + "." + fileexten;
						var filepath = location + "\\" + filename.Replace(" ", "_");
						file.SaveAs(filepath);
						string domainName = Request.Url.GetLeftPart(UriPartial.Authority);

						domainName = domainName + "/" + ConfigurationManager.AppSettings["sitename"].ToString();

						returnfileName = domainName + "SecretariatImages/" + AddFolder + "/" + filename.Replace(" ", "_");
					}

				}
				else
				{
					return "Failure Only JPG Files Accept";
				}

				//var returnfileName = domainName + "/GVTESTAPP/ApplicationUploads/" + AddFolder + "/" + filename.Replace(" ", "_");

				//UnEmployeeHelper _hep = new UnEmployeeHelper();/CMyuvaNapp
				//	_hep.SaveCertificateImages(CPFileService, returnfileName);
				//string mappath1 = Server.MapPath("UpdateUserManualLogs");

				return string.IsNullOrEmpty(returnfileName) ? "Failure Only JPG Files Accept" : returnfileName;
			}
			catch (Exception ex)
			{

				return "Failutre" + ex.Message;
			}

		}

		[HttpPost]
		//[Route("api/Rakshana/POSTData")]
		public string JobCardPassbook(FilesModel CPFileService)
		{

			//string jsondata = JsonConvert.SerializeObject(CPFileService);
			//FilesModel val = JsonConvert.DeserializeObject<FilesModel>(jsondata);
			//LogModel ologmodel = new LogModel();
			//ologmodel.UserId = val.UserId;
			//ologmodel.SacId = val.SacId;
			//ologmodel.DesignId = val.DesignId;
			//ologmodel.DeptId = string.Empty;
			//ologmodel.TranId = val.TranId;
			//logshel.WriteLogParameters(ologmodel);
			try
			{
				//  //_log.Info("In the HomeController => JobCardPassbook: " + JsonConvert.SerializeObject(CPFileService).ToString());
				var returnfileName = "";

				var file = CPFileService.Attachment;
				string fileexten = (file.ContentType.Split('/')[1] ?? "").ToLower();
				int imagesize = file.ContentLength;
				if (fileexten.ToLower() == "jpg" || fileexten.ToLower() == "jpeg" || fileexten.ToLower() == "png")
				{
					if (file == null)
						return "Please upload Image";
					if (imagesize > 1000000)
					{
						return "Failure File size exceeded 1 MB";
					}

					using (var bitmap = new System.Drawing.Bitmap(file.InputStream))
					{
						var location = Server.MapPath("~/" + ConfigurationManager.AppSettings["PRRDfilepath"].ToString());
						//_log.Info("In the HomeController => JobCardPassbook => filepath: " + location);
						if (!Directory.Exists(location))
							Directory.CreateDirectory(location);
						var CertificateCategory = CPFileService.CertificateCategory.Replace("/", "_");

						var filename = CertificateCategory + "_" + (CPFileService.CertifcateID ?? "").Replace("_", "") + "." + fileexten;
						var filepath = location + "\\" + filename.Replace(" ", "_");
						file.SaveAs(filepath);
						string domainName = Request.Url.GetLeftPart(UriPartial.Authority);

						domainName = domainName + "/" + ConfigurationManager.AppSettings["sitename"].ToString();

						returnfileName = domainName + ConfigurationManager.AppSettings["PRRDfilepath"].ToString() + filename.Replace(" ", "_");
						//_log.Info("In the HomeController => JobCardPassbook => filepath: " + returnfileName);
					}
				}
				else
				{
					//_log.Error("In the HomeController => JobCardPassbook => Failure Only JPG Files Accept");
					return "Failure Only JPG Files Accept";
				}

				//var returnfileName = domainName + "/GVTESTAPP/ApplicationUploads/" + AddFolder + "/" + filename.Replace(" ", "_");

				//UnEmployeeHelper _hep = new UnEmployeeHelper();/CMyuvaNapp
				// _hep.SaveCertificateImages(CPFileService, returnfileName);
				//string mappath1 = Server.MapPath("UpdateUserManualLogs");

				return string.IsNullOrEmpty(returnfileName) ? "Failure Only JPG Files Accept" : returnfileName;
			}
			catch (Exception ex)
			{
				//_log.Error("In the HomeController => JobCardPassbook :" + ex.Message + "__" + ex.InnerException + "__" + ex.StackTrace.ToString());
				return "Failutre" + ex.Message;
			}

		}

	}

	public class FilesModel
	{
		public string FullName { get; set; }
		public string AadharCardNumber { get; set; }
		public HttpPostedFileBase Attachment { get; set; }
		public string CertifcateID { get; set; }
		public string CertificateCategory { get; set; }
		public string ImagePath { get; set; }
		public string base64String { get; set; }
	} 
}
