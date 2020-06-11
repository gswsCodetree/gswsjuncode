using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace gswsBackendAPI.DL.DataConnection
{
	public class CommonSPHel: ConnectionHelper
	{
		OracleConnection con = new OracleConnection();
		OracleCommand cmd;
		OracleDataAdapter adp;
		OracleTransaction trans;
		#region "OracleCommon sps Methods"

		public DataTable GetspsDataAdapter(OracleCommand cmd)
		{
			try
			{
				con = new OracleConnection(ConSps);
				cmd.Connection = con;
				cmd.CommandTimeout = 180;
				con.Open();
				
				DataTable dt = new DataTable();
				adp = new OracleDataAdapter(cmd);
				adp.Fill(dt);
				con.Close();
				cmd.Dispose();
				return dt;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (con.State == ConnectionState.Open)
				{
					con.Close();
					cmd.Dispose();
				}
			}
		}
		public int getspsExecuteNonQuery(OracleCommand cmd)
		{
			try
			{
				con = new OracleConnection(ConSps);
				cmd.Connection = con;
				cmd.CommandTimeout = 180;
				con.Open();
				int i = cmd.ExecuteNonQuery();
				con.Close();
				cmd.Dispose();
				return i;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (con.State == ConnectionState.Open)
				{
					con.Close();
					cmd.Dispose();
				}
			}
		}

		public DataTable GetyouthserviceDataAdapter(OracleCommand cmd)
		{
			try
			{
				con = new OracleConnection(oradb_youth_service);
				cmd.Connection = con;
				cmd.CommandTimeout = 180;
				con.Open();
				DataTable dt = new DataTable();
				adp = new OracleDataAdapter(cmd);
				adp.Fill(dt);
				con.Close();
				cmd.Dispose();
				return dt;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (con.State == ConnectionState.Open)
				{
					con.Close();
					cmd.Dispose();
				}
			}
		}
		public int getyouthserviceExecuteNonQuery(OracleCommand cmd)
		{
			try
			{
				con = new OracleConnection(oradb_youth_service);
				cmd.Connection = con;
				cmd.CommandTimeout = 180;
				con.Open();
				int i = cmd.ExecuteNonQuery();
				con.Close();
				cmd.Dispose();
				return i;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (con.State == ConnectionState.Open)
				{
					con.Close();
					cmd.Dispose();
				}
			}
		}

		public DataTable GetgswsDataAdapter(OracleCommand cmd)
		{
			try
			{
				con = new OracleConnection(Congsws);
				cmd.Connection = con;
				cmd.CommandTimeout = 180;				
				con.Open();
				DataTable dt = new DataTable();
				adp = new OracleDataAdapter(cmd);				
				adp.Fill(dt);
				con.Close();
				cmd.Dispose();
				return dt;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (con.State == ConnectionState.Open)
				{
					con.Close();
					cmd.Dispose();
				}
			}
		}
		public int getgswsExecuteNonQuery(OracleCommand cmd)
		{
			try
			{
				con = new OracleConnection(Congsws);
				cmd.Connection = con;
				cmd.CommandTimeout = 180;
				con.Open();
				int i = cmd.ExecuteNonQuery();
				con.Close();
				cmd.Dispose();
				return i;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (con.State == ConnectionState.Open)
				{
					con.Close();
					cmd.Dispose();
				}
			}
		}
		public DataTable GetProdgswsDataAdapter(OracleCommand cmd)
		{
			try
			{

				con = new OracleConnection(Congswsprod);
				cmd.Connection = con;
				cmd.CommandTimeout = 180;
				con.Open();
				DataTable dt = new DataTable();
				adp = new OracleDataAdapter(cmd);
				adp.Fill(dt);
				con.Close();
				cmd.Dispose();
				return dt;

			}
			catch (Exception ex)
			{
				string mappath1 = HttpContext.Current.Server.MapPath("DataConcnnectionLogs");
				Task WriteTask1 = Task.Factory.StartNew(() => new Logdatafile().Write_Log_Exception(mappath1, "GetProdgswsDataAdapter" + ex.Message.ToString()));

				throw ex;
			}
			finally
			{
				if (con.State == ConnectionState.Open)
				{
					con.Close();
					cmd.Dispose();
				}
			}
		}
		public int getProdgswsExecuteNonQuery(OracleCommand cmd)
		{
			try
			{
				con = new OracleConnection(Congswsprod);
				cmd.Connection = con;
				cmd.CommandTimeout = 180;
				con.Open();
				int i = cmd.ExecuteNonQuery();
				con.Close();
				cmd.Dispose();
				return i;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (con.State == ConnectionState.Open)
				{
					con.Close();
					cmd.Dispose();
				}
			}
		}
		#endregion
		#region "OracleCommon srdh Methods"

		public DataTable GetsrdhDataAdapter(OracleCommand cmd)
		{
			try
			{
				con = new OracleConnection(Consrdh);
				cmd.Connection = con;
				cmd.CommandTimeout = 180;
				con.Open();
				DataTable dt = new DataTable();
				adp = new OracleDataAdapter(cmd);
				adp.Fill(dt);
				con.Close();
				cmd.Dispose();
				return dt;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (con.State == ConnectionState.Open)
				{
					con.Close();
					cmd.Dispose();
				}
			}
		}
		public int getsrdhExecuteNonQuery(OracleCommand cmd)
		{
			try
			{
				con = new OracleConnection(Consrdh);
				cmd.Connection = con;
				cmd.CommandTimeout = 180;
				con.Open();
				int i = cmd.ExecuteNonQuery();
				return i;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				con.Close();
				cmd.Dispose();
			}
		}

		public static string ThirdpartyMessage = "Department Service is Not Working.Please Try Again";
		public static string ErrMessage = "Server Busy.Please Try Again";

		public static string GetException(string err)
		{
			if (err.Contains("ORA-12520:"))
				return ErrMessage;
			if (err.Contains("ORA-"))
				return "Something Went Wrong.Please try again";
			else
				return err;
		}
		#endregion
	}
}