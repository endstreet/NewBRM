using System.Data;
using System.Data.SqlClient;

namespace SASSADirectCapture.Utilities
{
    public class clsSQLExecute
    {
        #region GetDatasetConfig

        /// <summary>
        /// Get Dataset Config
        /// </summary>
        /// <param name="strConfigDatabase">Config Database</param>
        /// <param name="strSQLPassword">SQL Password</param>
        /// <param name="strSQLServer">SQL Server</param>
        /// <param name="strSQLUser">SQL User</param>
        /// <param name="strSQL">SQL</param>
        /// <returns>Dataset</returns>

        public static DataTable GetDatasetConfig(string strSQLServer, string strConfigDatabase, string strSQLUser, string strSQLPassword, string strSQL)
        {
            Utilities.clsConnection connect = new Utilities.clsConnection();
            using (SqlConnection conn = new SqlConnection(connect.BuildConfigSQLConnString(strSQLServer, strConfigDatabase, strSQLUser, strSQLPassword)))
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();

                cmd.Connection = conn;
                cmd.CommandText = strSQL;

                DataSet ds = new DataSet("Table");
                DataTable dt = new DataTable("dt");

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(ds);
                }

                conn.Close();

                return dt = ds.Tables[0];
            }
        }

        #endregion GetDatasetConfig
    }
}