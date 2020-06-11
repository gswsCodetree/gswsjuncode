using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.DL.DataConnection
{
	public  class ConnectionHelper
	{
		public  string  ConSps= "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=apexadata-scan1.apsdc.ap.gov.in)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=APSPS16)));User Id=" + ConfigurationManager.AppSettings["spsuser"].ToString() + ";Password=" + ConfigurationManager.AppSettings["spspwd"].ToString() + ";";

		public string Congsws = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=apexadata-scan1.apsdc.ap.gov.in)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=PRSCHLM))); Min Pool Size="+Convert.ToInt32(ConfigurationManager.AppSettings["minpool"].ToString())+ "; Connection Lifetime=120; Connection Timeout=150;  Max Pool Size="+ Convert.ToInt32(ConfigurationManager.AppSettings["maxpool"].ToString()) + "; User Id=" + ConfigurationManager.AppSettings["gsws"].ToString() + ";Password=" + ConfigurationManager.AppSettings["gswspwd"].ToString() + ";";
		public string Congswsprod = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=apexadata-scan1.apsdc.ap.gov.in)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=PRSCHLM))); Min Pool Size=" + Convert.ToInt32(ConfigurationManager.AppSettings["minpool"].ToString()) + "; Connection Lifetime=120; Connection Timeout=150;  Max Pool Size=" + Convert.ToInt32(ConfigurationManager.AppSettings["maxpool"].ToString()) + "; User Id=" + ConfigurationManager.AppSettings["pgsws"].ToString() + ";Password=" + ConfigurationManager.AppSettings["pgswspwd"].ToString() + ";";

		public static string oradb_youth_service = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=apexadata-scan1.apsdc.ap.gov.in)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=APSPS16)));User Id=" + ConfigurationManager.AppSettings["youth_service_username"].ToString() + ";Password=" + ConfigurationManager.AppSettings["youth_service_password"].ToString() + ";";
		//public string Congsws = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=gramasachivalayam01.czpkl9e376xn.ap-south-1.rds.amazonaws.com)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=ORCL)));User Id=" + ConfigurationManager.AppSettings["gsws"].ToString() + "; Password=" + ConfigurationManager.AppSettings["gswspwd"].ToString() + ";";
		public  string Consrdh = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=apexadata-scan1.apsdc.ap.gov.in)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=apsrdh)));User Id=" + ConfigurationManager.AppSettings["srdhuser"].ToString() + ";Password=" + ConfigurationManager.AppSettings["srdhpwd"].ToString() + ";";
	}
}