using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.MeesevaService.Backend
{
    public class MeesevaModel
    {
        public string STATUS { get; set; }
        public string TOKEN { get; set; }       
        public string LANDINGID { get; set; }
        public string SCAID { get; set; }
        public string CHANNELID { get; set; }
        public string OPERATORID { get; set; }
        public string OPERATOR_UNIQUENO { get; set; }
        public string SERVICEID { get; set; }
        public string ENCDATA { get; set; }
        public string PARAM1 { get; set; }
        public string PARAM2 { get; set; }
        public string REASON { get; set; }
		public string SECRETARIATCODE { get; set; }
	}

	public class MeesevaHOuseSitesModel
	{
		public string STATUS { get; set; }
		public string TOKEN { get; set; }
		public string LANDINGID { get; set; }
		public string SCAID { get; set; }
		public string CHANNELID { get; set; }
		public string USERID { get; set; }
		public string OPERATOR_UNIQUENO { get; set; }
		public string SERVICEID { get; set; }
		public string ENCDATA { get; set; }
		public string PARAM1 { get; set; }
		public string PARAM2 { get; set; }
		public string REASON { get; set; }
		public string RedirectUrl { get; set; }
		public string SECRETARIATCODE { get; set; }
	}
	public class RootOMeesevabject
	{
		public string status { get; set; }
		public string STATUSID { get; set; }
		public string STATUS { get; set; }
		public string APPNO { get; set; }
		public string remarks { get; set; }
	}

	public class MEESEVASTATUSMODEL
	{
		public string Ftype { get; set; }
		public string DeptrecieptCode { get; set; }
		public string Statuscode { get; set; }
		public string Statusmsg { get; set; }

	}
}