using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using gswsBackendAPI.DL.DataConnection;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace gswsBackendAPI.Depts.Health
{
    public class HealthSPModel : CommonSPHel
    {
		public DataTable GetArogyaRakshaStatus_data_helper(AppStatus obj)
		{
			try
			{
				var comd = new OracleCommand();

				comd.InitialLONGFetchSize = 1000;
				comd.CommandType = CommandType.StoredProcedure;
				comd.CommandText = "ct_schema.gsws_check_status";
				comd.Parameters.Add("ftype", OracleDbType.Varchar2).Value = obj.ftype;
				comd.Parameters.Add("fdpart_id", OracleDbType.Varchar2).Value = obj.fdpart_id;
				comd.Parameters.Add("fadhar_no", OracleDbType.Varchar2).Value = obj.fadhar_no;
				comd.Parameters.Add("P_CUR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

				DataTable data = GetspsDataAdapter(comd);
				if (data != null && data.Rows.Count > 0)
					return data;
				else
					return null;
			}
			catch (WebException wex)
			{
				string mappath = HttpContext.Current.Server.MapPath("ArogyaRakshaExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error Arogya Raksha App Status API:" + wex.Message.ToString()));
				throw new Exception(wex.Message);
			}
		}

		#region YSR Kanti Velugu
		public dynamic GetData(string url)
		{
			var response = String.Empty;
			try
			{

				var req = (HttpWebRequest)WebRequest.Create(url);
				req.ContentType = "application/json; charset=utf-8";

				req.AllowAutoRedirect = false;
				var resp = req.GetResponse();
				var sr = new StreamReader(resp.GetResponseStream());
				response = sr.ReadToEnd().Trim();

				var data = JsonConvert.DeserializeObject<dynamic>(response);

				return data;
			}
			catch (WebException wex)
			{
				string mappath = HttpContext.Current.Server.MapPath("YSRKantiVeluguExceptionLogs");
				Task WriteTask = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath, "Error YSR Kanti velugu API:" + wex.Message.ToString()));

				throw new Exception(wex.Message);
			}


		}

		#endregion
	}
}