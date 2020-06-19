using gswsBackendAPI.DL.DataConnection;
using gswsBackendAPI.MeesevaService.Backend;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gswsBackendAPI
{
	public partial class UrlRedirection : System.Web.UI.Page
	{
		byte[] key = { 0xA2, 0x15, 0x37, 0x07, 0xCB, 0x62, 0xC1, 0xD3, 0xF8, 0xF1, 0x97, 0xDF, 0xD0, 0x13, 0x4F, 0x79, 0x01, 0x67, 0x7A, 0x85, 0x94, 0x16, 0x31, 0x92 };
		byte[] iv = { 50, 51, 52, 53, 54, 55, 56, 57 };
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				//ddlService.Items.Insert(0, "All");

				string seccode = Page.Request.Form["Seccode"];
				string userid = Page.Request.Form["userid"];
				string distcode = Page.Request.Form["DistCode"];
				if (string.IsNullOrEmpty(seccode) || string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(distcode))
				{

					Response.Redirect("https://gramawardsachivalayam.ap.gov.in/GSWS/#!/Login");
					return;
				}
				//distcode = "502";
				
					string requestId = string.Empty;
				System.Guid guid = System.Guid.NewGuid();
				string strguid = guid.ToString();
				requestId = strguid.Substring(strguid.LastIndexOf("-") + 1);
				requestId = requestId.ToUpper().Replace('O', 'W').Replace('0', '4');
				requestId = requestId.Substring(0, 11);
				//LandingDetails LD = new LandingDetails();
				MeesevaModel LD = new MeesevaModel();
				MeesevaProductionService.MeeSevaWebService objPromeeseva = new MeesevaProductionService.MeeSevaWebService();
				try
				{
					var data = objPromeeseva.VSWS_GETTOKEN("VSWS-APTS", "P$W$@13112019"); //[{"Status":"100","token":"asd$#@4568"}]
																						  //var data = objPromeeseva.get("VSWS-APTS", "P$W$@13112019"); //[{"Status":"100","token":"asd$#@4568"}]
																						  //string mappath = HttpContext.Current.Server.MapPath("MeesevaExceptionLogs");
																						  //Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, data));

					//var data2 = JsonConvert.DeserializeObject<dynamic>(data);
					//_objmweb.VSWS_GETAPPDETAILS("VSWS-APTS", "P$W$@13112019", data2[0].token,obj2.PARAM1);
					string status = "100";
					MeesevaModel _OBJMES = new MeesevaModel();
					if (status == "100")
					{
						string channelid = string.Empty;
						string uniqueid = string.Empty;
						string operatorid = string.Empty;
						
							channelid = userid.Split('-')[0].ToString(); ;
							operatorid =userid.ToString();
							uniqueid = userid.Split('-')[0].ToString();
						

						lblerror.Text = "";
						LD.TOKEN = "asd$#@4568";
						LD.LANDINGID = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999).ToString();
						LD.SCAID = "35";
						LD.CHANNELID = channelid;//"CODETREE";
						LD.OPERATORID = operatorid;// Request.QueryString["userid"] ?? ""; //"CODETREE-1";
						LD.OPERATOR_UNIQUENO = "CCSP35" + uniqueid + 1; //"CCSP35CODETREE1";
						LD.SECRETARIATCODE = seccode ?? "";
						//LD.SERVICEID = Request.QueryString["Serviceid"] ?? ""; 
						cTripleDES des = new cTripleDES(key, iv);
						LD.ENCDATA = des.Encrypt(Create_ENCDATA(LD.TOKEN, LD.LANDINGID, LD.SCAID, LD.CHANNELID, LD.OPERATORID, LD.OPERATOR_UNIQUENO));
						NameValueCollection DATA = new NameValueCollection();
						DATA.Add("TOKEN", LD.TOKEN);
						DATA.Add("LANDINGID", LD.LANDINGID);
						DATA.Add("SCAID", LD.SCAID);
						DATA.Add("CHANNELID", LD.CHANNELID);
						DATA.Add("OPERATORID", LD.OPERATORID);
						DATA.Add("OPERATOR_UNIQUENO", LD.OPERATOR_UNIQUENO);
						DATA.Add("ENCDATA", LD.ENCDATA);

						new EncryptMeeseva().GetMeesevaInitiate(LD);

						string msdistkey = ConfigurationManager.AppSettings["MSdistkey"].ToString();
						string msuserkey = ConfigurationManager.AppSettings["MSUserkey"].ToString();
						string[] strdistarr = msdistkey.Split(',');
						string[] strusertarr = msuserkey.Split(',');
						bool distflag = Array.Exists(strdistarr, element => element.Equals(distcode));
						bool userflag = Array.Exists(strusertarr, element => element.Equals(userid));
						
						//23456789-WEDS
						//HtmlHelper.RedirectAndPOST(this.Page, "http://meeseva.gov.in/GSVWIMeeseva/UserInterface/DC/VSWSRedirection.aspx", DATA);
						if (distflag || userflag)
							HtmlHelper.RedirectAndPOST(this.Page, "https://onlineap.meeseva.gov.in/GSVWIMeeseva/UserInterface/DC/VSWSRedirection.aspx ", DATA);
						
						else
							HtmlHelper.RedirectAndPOST(this.Page, "http://reports.meeseva.gov.in/GSVWIMeeseva/UserInterface/DC/VSWSRedirection.aspx ", DATA);
					}
					else
					{
						lblerror.Text = "Invalid Request";
					}
				}
				catch (Exception ex)
				{
					string mappath = HttpContext.Current.Server.MapPath("MeesevaExceptionLogs");
					Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log(mappath, ex.Message.ToString()));
					lblerror.Text = ex.Message.ToString();
				}
			}
		}

		public class LandingDetails
		{
			public string TOKEN { get; set; }
			public string LANDINGID { get; set; }
			public string SCAID { get; set; }
			public string CHANNELID { get; set; }
			public string OPERATORID { get; set; }
			public string OPERATOR_UNIQUENO { get; set; }
			// public string SERVICEID { get; set; }
			public string ENCDATA { get; set; }
		}

		private string Create_ENCDATA(string TOKEN, string LANDINGID, string SCAID, string CHANNELID, string OPERATORID, string OPERATOR_UNIQUENO)
		{
			string ENCDATA = "";
			try
			{
				ENCDATA = TOKEN + "|" + LANDINGID + "|" + SCAID + "|" + CHANNELID + "|" + OPERATORID + "|" + OPERATOR_UNIQUENO;
			}
			catch (Exception ex)
			{
				ex.Message.ToString();
			}
			return ENCDATA;
		}

		protected void btnSubmit_Click(object sender, EventArgs e)
		{

			string requestId = string.Empty;
			System.Guid guid = System.Guid.NewGuid();
			string strguid = guid.ToString();
			requestId = strguid.Substring(strguid.LastIndexOf("-") + 1);
			requestId = requestId.ToUpper().Replace('O', 'W').Replace('0', '4');
			requestId = requestId.Substring(0, 11);
			LandingDetails LD = new LandingDetails();
			LD.TOKEN = "asd$#@4568";
			LD.LANDINGID = requestId;
			LD.SCAID = "35";
			LD.CHANNELID = "CODETREE";
			LD.OPERATORID = "CODETREE-1";
			LD.OPERATOR_UNIQUENO = "CCSP35CODETREE1";
			cTripleDES des = new cTripleDES(key, iv);
			LD.ENCDATA = des.Encrypt(Create_ENCDATA(LD.TOKEN, LD.LANDINGID, LD.SCAID, LD.CHANNELID, LD.OPERATORID, LD.OPERATOR_UNIQUENO));
			NameValueCollection DATA = new NameValueCollection();
			DATA.Add("TOKEN", LD.TOKEN);
			DATA.Add("LANDINGID", LD.LANDINGID);
			DATA.Add("SCAID", LD.SCAID);
			DATA.Add("CHANNELID", LD.CHANNELID);
			DATA.Add("OPERATORID", LD.OPERATORID);
			DATA.Add("OPERATOR_UNIQUENO", LD.OPERATOR_UNIQUENO);
			DATA.Add("ENCDATA", LD.ENCDATA);
			HtmlHelper.RedirectAndPOST(this.Page, "http://meeseva.gov.in/GSVWIMeeseva/UserInterface/DC/VSWSRedirection.aspx", DATA);
		}
	}
}