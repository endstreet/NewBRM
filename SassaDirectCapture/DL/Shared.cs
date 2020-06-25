using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace SASSADirectCapture
{
    public class Shared
    {
        public string connectionString;
        public Shared()
        {
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["OracleDBConnection"].ConnectionString;
        }
        public void ExecuteNonQuery(string sql)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(connectionString))
                {
                    OracleCommand command = new OracleCommand(sql, con);
                    command.XmlCommandType = OracleXmlCommandType.None;
                    command.Connection.Open();
                    int rowsUpdated = command.ExecuteNonQuery();
                }
            }
            catch 
            {
                throw;
            }
        }

        public DataTable GetTable(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                using (OracleConnection con = new OracleConnection(connectionString))
                {
                    OracleCommand cmd = con.CreateCommand();
                    cmd.BindByName = true;
                    cmd.CommandTimeout = 0;
                    cmd.FetchSize *= 8;

                    //Destruction List
                    cmd.CommandText = sql;
                    con.Open();

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                    con.Close();
                    return dt;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}