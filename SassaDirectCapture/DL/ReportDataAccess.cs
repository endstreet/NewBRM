using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace SASSADirectCapture.DL
{
    public class ReportDataAccess
    {
        #region Private Fields

        private string connectionString = string.Empty;

        private string schema = System.Configuration.ConfigurationManager.AppSettings["sGenServiceDBName"].ToString();

        #endregion Private Fields

        #region Public Constructors

        public ReportDataAccess()
        {
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["OracleDBConnection"].ConnectionString;
        }

        #endregion Public Constructors

        #region Public Methods

        public DataTable getReportData(string dateFrom, string dateTo, string Index, string office_id, string office_type, string region_id, string grant_type, string status)
        {
            DataTable dt = new DataTable();


            string region = "";
            string regionSQL = "";
            string region1SQL = "";
            string region2SQL = "";
            int region_code = -1;
            string PaypointSQL = "";
            string OfficeSQL = "";
            string statusSQL = "";

            if (dateFrom.StartsWith("<"))
            {
                dateFrom = "0000";
            }

            if (!string.IsNullOrEmpty(status))
            {
                statusSQL = " AND D.STATUS = '" + status + "' ";
            }

            if (region_id.Contains(":"))
            {
                region = region_id.Split(':')[1];
                region_code = int.Parse(region_id.Split(':')[1]);
                regionSQL = " AND b.PROVINCE = '" + region + "'";
                region1SQL = " AND REGION = '" + region + "'";
                region2SQL = " AND A.REGION_ID = '" + region + "'";
            }
            else { region = region_id; }
            if (!String.IsNullOrEmpty(office_id))
            {
                PaypointSQL = " AND b.SEC_PAYPOINT ='" + office_id + "' ";
                OfficeSQL = " AND A.OFFICE_ID =" + office_id + " ";
            }
            try
            {
                using (OracleConnection con = new OracleConnection(connectionString))
                {
                    OracleCommand cmd = con.CreateCommand();
                    cmd.BindByName = true;
                    cmd.CommandTimeout = 0;
                    cmd.FetchSize *= 8;

                    //Destruction List
                    if (Index == "1")
                    {
                        if (dateFrom.StartsWith("<"))
                        {

                            dateFrom = "0000";
                        }

                        cmd.CommandText = "SELECT REGION_NAME, Pension_No,NAME,SURNAME, GRANT_TYPE, STATUS FROM DC_DESTRUCTION D JOIN contentserver.DC_REGION R ON D.REGION_ID = R.REGION_ID " +
                                           string.Format("WHERE SUBSTR(DESTRUCTIO_DATE,1,4) =  '{0}' ", dateFrom) +
                                           (region == "" ? "" : string.Format("AND D.REGION_ID = {0} ", region)) +
                                           (status == "" ? "" : string.Format("AND D.STATUS = '{0}' ", status)) +
                                           "ORDER BY REGION_NAME";

                    }
                    if (Index == "2")
                    {

                        cmd.CommandText = string.Format("SELECT * FROM " +
                            "(" +
                            "SELECT SUBSTR(D.DESTRUCTIO_DATE,1,4) AS YEAR,R.REGION_NAME,D.STATUS FROM contentserver.DC_destruction D " +
                            "JOIN contentserver.DC_REGION R ON D.REGION_ID = R.REGION_ID " +
                            "Where SUBSTR(DESTRUCTIO_DATE,1,4) = '{1}' " + (region == "" ? ")" : string.Format("AND D.REGION_ID = {0}) ", region)) +
                            "PIVOT (  Count(STATUS)  FOR STATUS IN ('Selected', 'Excluded', 'TDWFound', 'TDWNotFound', 'Approved', 'Destroyed', 'Exception')" +
                            ")" +
                            "ORDER BY YEAR,REGION_NAME", region, dateFrom);
                    }
                    //Missing files New
                    if (Index == "3")
                    {
                        cmd.CommandText = @"select b.PROVINCE as REGION,b.PENSION_NO,b.NAME,b.SURNAME,b.original_Application_date,b.GRANT_TYPE,b.ADDRESS,b.AGE, b.SEC_PAYPOINT from ACTIVE_GRANTS b
                                            where NOT EXISTS (Select ID_NUMBER from ALL_FILES a where a.ID_NUMBER = b.PENSION_NO and ((a.GRANT_TYPE = b.GRANT_TYPE) or (a.GRANT_TYPE = '3' and b.GRANT_TYPE = '0')))" +
                                            " and b.original_Application_date  >= to_date('" + dateFrom + "', 'YYYY/mm/dd')" +
                                            " and b.original_Application_date <= to_date('" + dateTo + "', 'YYYY/mm/dd')" +
                                            (string.IsNullOrEmpty(grant_type) ? "":" and b.GRANT_TYPE = '" + grant_type + "'");
                        //cmd.CommandText += regionSQL + PaypointSQL + " ORDER BY b.Province,b.SEC_PAYPOINT, b.GRANT_TYPE,b.original_Application_date";
                        cmd.CommandText += regionSQL + " ORDER BY b.Province, b.SEC_PAYPOINT, b.GRANT_TYPE,b.original_Application_date";
                    }
                    //All Grants
                    if (Index == "4")
                    {
                        cmd.CommandText = @"select b.PROVINCE as REGION,b.PENSION_NO,b.NAME,b.SURNAME,b.original_Application_date,b.GRANT_TYPE,b.ADDRESS,b.AGE, b.SEC_PAYPOINT, a.SOURCETBL from ACTIVE_GRANTS b " +
                                            " INNER JOIN ALL_FILES a ON a.ID_NUMBER = b.PENSION_NO and a.GRANT_TYPE = b.GRANT_TYPE " +
                                            " WHERE b.original_Application_date  >= to_date('" + dateFrom + "', 'YYYY/mm/dd')" +
                                            " and b.original_Application_date <= to_date('" + dateTo + "', 'YYYY/mm/dd')" +
                                            " and b.GRANT_TYPE = '" + grant_type + "'";
                        //cmd.CommandText += regionSQL + PaypointSQL + " ORDER BY b.Province,b.SEC_PAYPOINT, b.GRANT_TYPE,b.original_Application_date";
                        cmd.CommandText += regionSQL + " ORDER BY b.Province, b.SEC_PAYPOINT, b.GRANT_TYPE,b.original_Application_date";
                    }
                    //User Activity
                    if (Index == "5")
                    {
                        cmd.CommandText = "SELECT DISTINCT LO.OFFICE_NAME, A.USERNAME, A.AREA, A.ACTIVITY, A.ACTIVITY_DATE " +
                                          "FROM " + schema + ".DC_ACTIVITY A " +
                                          "INNER JOIN " + schema + ".DC_LOCAL_OFFICE LO ON A.OFFICE_ID = LO.OFFICE_ID " +
                                          "INNER JOIN " + schema + ".DC_LOCAL_OFFICE LOR ON LO.REGION_ID = LOR.REGION_ID " +
                                          "WHERE A.ACTIVITY_DATE >= to_date('" + dateFrom + "', 'YYYY/mm/dd') AND A.ACTIVITY_DATE <= (to_date('" + dateTo + "', 'YYYY/mm/dd') + 1) " +
                                          region2SQL + OfficeSQL +
                                          "ORDER by A.USERNAME , A.ACTIVITY_DATE";
                    }
                    //Activity Pivot
                    if (Index == "6")
                    {
                        cmd.CommandText = @"SELECT * FROM
                                        (
	                                        SELECT DISTINCT  CONCAT( LO.OFFICE_NAME ,CONCAT(' - ', A.USERNAME)) AS USERNAME, A.AREA, A.ACTIVITY_DATE FROM " + schema + ".DC_ACTIVITY A " +
                                           "JOIN " + schema + ".DC_LOCAL_OFFICE LO ON A.OFFICE_ID = LO.OFFICE_ID " +
                                           "JOIN " + schema + ".DC_LOCAL_OFFICE LOR ON LO.REGION_ID = LOR.REGION_ID " +
                                           "WHERE A.ACTIVITY_DATE >= to_date('" + dateFrom + "', 'YYYY/mm/dd') " +
                                           "AND A.ACTIVITY_DATE <= (to_date('" + dateTo + "', 'YYYY/mm/dd') + 1) " +
                                           region2SQL + OfficeSQL +
                                           "ORDER by A.USERNAME " +
                                           " ) " +
                                        "PIVOT ( " +
                                          " Count(ACTIVITY_DATE) " +
                                          " FOR AREA " +
                                          " IN ('Files','FileRequest', 'Batching', 'Boxing', 'Receiving','Transport','Office','QCFile') " +
                                        Environment.NewLine + " )";
                    }

                    con.Open();

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                    con.Close();
                }
            }
            catch
            {
                throw;
            }

            return dt;
        }

        #endregion Public Methods
    }
}
