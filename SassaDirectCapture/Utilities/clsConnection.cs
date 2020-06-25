using System;
using System.Data;
using System.Data.SqlClient;

namespace SASSADirectCapture.Utilities
{
    public class clsConnection
    {
        #region Build CS SQL Connection String

        /// <summary>
        /// Build Syspro SQL connection string Client
        /// </summary>
        /// <param name="strSQLPassword">SQL Password</param>
        /// <param name="strSQLServer">SQL Server</param>
        /// <param name="strSQLUser">SQL User</param>
        /// <param name="strSysproAdmin">Syspro Admin</param>
        /// <param name="strCSDatabase">Sysprodb Database</param>
        /// <param name="strCompany">Company</param>
        /// <returns>Connection string to Syspro Company database</returns>
        /// <remarks>Sive Sobantu</remarks>
        public string BuildCSSQLConnString(string strSQLServer, string strCSDatabase, string strSysproAdmin, string strSQLUser, string strSQLPassword, string strCompany)
        {
            SqlConnection sqlConn = new SqlConnection();
            SqlCommand sqlCmnd = new SqlCommand();
            try
            {
                string strSysprodbConn = "";
                strSysprodbConn = "Persist Security Info=False;";
                strSysprodbConn += "User ID=" + strSQLUser + ";";
                strSysprodbConn += "Password=" + strSQLPassword + ";";
                strSysprodbConn += "Initial Catalog=" + strCSDatabase + ";";
                strSysprodbConn += "Data Source=" + strSQLServer + ";";

                sqlConn = new SqlConnection(strSysprodbConn);
                sqlConn.Open();
                sqlCmnd = new SqlCommand("Select DatabaseName from " + strSysproAdmin + " WHERE Company = '" +
                    strCompany + "' ", sqlConn);
                //charlene 21 Oct 09
                object obj = sqlCmnd.ExecuteScalar();
                if (obj == null)
                    throw new Exception("Company " + strCompany + " could not be found.");
                string strDbName = obj.ToString();
                string strConn = "";
                strConn = "Persist Security Info=False;";
                strConn += "User ID=" + strSQLUser + ";";
                strConn += "Password=" + strSQLPassword + ";";
                strConn += "Initial Catalog=" + strDbName + ";";
                strConn += "Data Source=" + strSQLServer + ";";
                return strConn;
            }
            finally
            {
                if (sqlConn != null)
                {
                    if (sqlConn.State == ConnectionState.Open)
                        sqlConn.Close();
                    sqlConn.Dispose();
                }
                if (sqlCmnd != null)
                    sqlCmnd.Dispose();
            }
        }

        #endregion Build CS SQL Connection String

        #region Build CS SQL Connection String

        /// <summary>
        /// Build Sysprodb SQL Connection string
        /// </summary>
        /// <param name="strCSDatabase">Sysprodb Database</param>
        /// <param name="strSQLPassword">SQL Password</param>
        /// <param name="strSQLServer">SQL Server</param>
        /// <param name="strSQLUser">SQL User</param>
        /// <returns>Connection string to Sysprodb database</returns>
        /// <remarks>Sive Sobantu</remarks>
        public string BuildSysprodbSQLConnString(string strSQLServer, string strCSDatabase, string strSQLUser, string strSQLPassword)
        {
            string strConn = "";
            strConn = "Persist Security Info=False;";
            strConn += "User ID=" + strSQLUser + ";";
            strConn += "Password=" + strSQLPassword + ";";
            strConn += "Initial Catalog=" + strCSDatabase + ";";
            strConn += "Data Source=" + strSQLServer + ";";
            return strConn;
        }

        #endregion Build CS SQL Connection String

        #region Build Config SQL Connection String

        /// <summary>
        /// Build configuration SQL Connection string
        /// </summary>
        /// <param name="strConfigDatabase">Config Database</param>
        /// <param name="strSQLPassword">SQL Password</param>
        /// <param name="strSQLServer">SQL Server</param>
        /// <param name="strSQLUser">SQL User</param>
        /// <returns>Connection string to configuration database</returns>
        /// <remarks>Sive Sobantu</remarks>
        public string BuildConfigSQLConnString(string strSQLServer, string strConfigDatabase, string strSQLUser, string strSQLPassword)
        {
            string strConn = "";
            strConn = "Persist Security Info=False;";
            strConn += "User ID=" + strSQLUser + ";";
            strConn += "Password=" + strSQLPassword + ";";
            strConn += "Initial Catalog=" + strConfigDatabase + ";";
            strConn += "Data Source=" + strSQLServer + ";";
            return strConn;
        }

        #endregion Build Config SQL Connection String

        #region Test SQL Connection

        /// <summary>
        /// Tests the given SQL connection settings
        /// </summary>
        /// <param name="strServer">Server</param>
        /// <param name="strDatabase">Database</param>
        /// <param name="bWinAuth">Windows Authentication</param>
        /// <param name="strUser">User</param>
        /// <param name="strPassword">Password</param>
        /// <returns>String error message if any</returns>
        /// <remarks>Sive Sobantu</remarks>
        public string TestSQLConnection(string strServer, string strDatabase, bool bWinAuth, string strUser,
            string strPassword)
        {
            try
            {
                string strConn = "";
                if (bWinAuth != true)
                {
                    strConn = "Persist Security Info=False;";
                    strConn += "User ID=" + strUser + ";";
                    strConn += "Password=" + strPassword + ";";
                    strConn += "Initial Catalog=" + strDatabase + ";";
                    strConn += "Data Source=" + strServer + ";";
                }
                else
                {
                    strConn = "Integrated Security=SSPI;Persist Security Info=False;";
                    strConn += "Initial Catalog=" + strDatabase + ";";
                    strConn += "Data Source=" + strServer + ";";
                }

                SqlConnection conn = new SqlConnection(strConn);
                conn.Open();
                conn.Close();

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion Test SQL Connection
    }
}