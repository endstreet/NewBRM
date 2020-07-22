using SASSADirectCapture.BL;
using SASSADirectCapture.EntityModels;
using SASSADirectCapture.Sassa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SASSADirectCapture.Views
{
    public partial class FileRequestOut : SassaPage
    {
        #region Private Fields

        private Entities en = new Entities();

        #endregion Private Fields

        #region Public Methods

        public IQueryable<FileRequest> GetFileRequestHistory()
        {
            var x = Usersession.Office.OfficeId;

            IQueryable<FileRequest> query;
            try
            {
                query =
                    from fr in en.DC_FILE_REQUEST
                    where fr.CLOSED_DATE == null
                    && fr.REQUESTED_OFFICE_ID == x
                    orderby fr.REQUESTED_DATE, fr.ID_NO

                    select new FileRequest
                    {
                        UNQ_FILE_NO = fr.UNQ_FILE_NO,
                        ID_NO = fr.ID_NO,
                        BRM_BARCODE = fr.BRM_BARCODE,
                        MIS_FILE_NO = fr.MIS_FILE_NO,
                        NAME = fr.NAME,
                        SURNAME = fr.SURNAME,
                        REGION_ID = fr.REGION_ID,
                        REQUESTED_BY = (long)fr.REQUESTED_BY,
                        REQUESTER_NAME = Usersession.Name,
                        REQUESTED_DATE = fr.REQUESTED_DATE,
                        SCANNED_BY = fr.SCANNED_BY,
                        SCANNED_DATE = fr.SCANNED_DATE,
                        STATUS = fr.SCANNED_DATE == null ? (fr.SCANNED_BY == null ? "Pending" : "In Progress") : (fr.SCANNED_PHYSICAL_IND == "S" ? "Scanned to Livelink" : "Ready for Collection")
                    };
            }
            catch (Exception)
            {
                return null;
            }

            return query;
        }

        #endregion Public Methods

        #region Protected Methods

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            divError.Style.Add("display", "none");
            SearchFile();
        }

        protected void fileGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            fileGridView.PageIndex = e.NewPageIndex;
            fileGridView.SelectMethod = "GetFileRequestHistory";
        }

        protected void gvResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvResults.PageIndex = e.NewPageIndex;
            SearchFile();
        }

        protected void hiddenBtnData_Click(object sender, EventArgs e)
        {
            divError.Style.Add("display", "none");
            fileGridView.SelectMethod = "GetFileRequestHistory";

            gvResults.DataSource = null;
            gvResults.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Dont need to handle authentication as this is done on the master page load.
        }

        #endregion Protected Methods

        #region Private Methods

        private void SearchFile()
        {
            //This is to close the "Loading" popup on the screen when results are returned
            ScriptManager.RegisterStartupScript(this, GetType(), "hideLoadingMsg", "myECM.hidePleaseWait();", true);

            using (Entities context = new Entities())
            {
                using (DataTable DT = new DataTable())
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    DT.Columns.Add("ID_Number", typeof(string));
                    DT.Columns.Add("BRM_BARCODE", typeof(string));
                    DT.Columns.Add("BATCH_NO", typeof(string));
                    DT.Columns.Add("Type", typeof(string));
                    DT.Columns.Add("File_Number", typeof(string));
                    DT.Columns.Add("Name", typeof(string));
                    DT.Columns.Add("Surname", typeof(string));
                    DT.Columns.Add("Region", typeof(string));
                    DT.Columns.Add("RegionID", typeof(string));
                    DT.Columns.Add("MISBin", typeof(string));
                    DT.Columns.Add("MISBox", typeof(string));
                    DT.Columns.Add("MISPos", typeof(string));
                    DT.Columns.Add("TDWBoxno", typeof(string));
                    DT.Columns.Add("GrantType", typeof(string));
                    DT.Columns.Add("AppDate", typeof(string));
                    DT.Columns.Add("ArchiveDate", typeof(string));
                    DT.Columns.Add("UNQ_FILE_NO", typeof(string));
                    DT.Columns.Add("AppStatus", typeof(string));
                    DT.Columns.Add("UPDATED_BY", typeof(string));
                    DT.Columns.Add("PARENT_BRM_NUMBER", typeof(string));

                    string idNrValue = txtSearchID.Text.ToUpper();
                    string sBRMNoValue = txtSearchBRMNo.Text.ToUpper();
                    string sCLMNoValue = txtSearchCLMNo.Text.ToUpper();
                    string sSRDNoValue = txtSearchSrdNo.Text.ToUpper();
                    if (string.IsNullOrEmpty(idNrValue) && string.IsNullOrEmpty(sBRMNoValue) && string.IsNullOrEmpty(sCLMNoValue) && string.IsNullOrEmpty(sSRDNoValue))
                    {
                        lblError.Text = "Please enter a search value in at least one of the fields.";
                        divError.Style.Add("display", "");
                        return;
                    }

                    try
                    {
                        IEnumerable<FileRequest> query;
                        if (sBRMNoValue != "")
                        {
                            query = context.Database.SqlQuery<FileRequest>
                              (@" SELECT
                                    CAST(f.APPLICANT_NO AS VARCHAR(50)) AS ID_NO,
                                    f.FILE_NUMBER AS MIS_FILE_NO,
                                    f.BRM_BARCODE AS BRM_BARCODE,
                                    CAST(f.BATCH_NO AS VARCHAR(50)) AS BATCH_NO,
                                    b.REG_TYPE AS REG_TYPE,
                                    f.GRANT_TYPE AS GRANT_TYPE,
                                    f.USER_FIRSTNAME AS NAME,
                                    f.USER_LASTNAME AS SURNAME,
                                    r.REGION_ID AS REGION_ID,
                                    r.REGION_CODE AS REGION_CODE,
                                    r.REGION_NAME AS REGION_NAME,
                                    f.TDW_BOXNO AS TDW_BOXNO,
                                    f.TRANS_DATE AS APP_DATE_DT,
                                    f.UNQ_FILE_NO AS UNQ_FILE_NO,
                                    TO_NUMBER(f.ARCHIVE_YEAR) AS ARCHIVE_YEAR,
                                    f.APPLICATION_STATUS AS APPLICATION_STATUS,
                                    f.UPDATED_BY_AD AS UPDATED_BY,
                                    m.PARENT_BRM_BARCODE AS PARENT_BRM_NUMBER
                                FROM CONTENTSERVER.DC_FILE f
                                INNER JOIN CONTENTSERVER.DC_REGION r ON r.REGION_ID = f.REGION_ID
                                LEFT OUTER JOIN DC_MERGE m on m.BRM_BARCODE = f.BRM_BARCODE
                                LEFT OUTER JOIN DC_BATCH b on b.batch_no = f.Batch_no
                                WHERE "
                                  + (idNrValue == "" ? "" : "f.APPLICANT_NO IN (" + util.getIDList(idNrValue) + ")")
                                  + ((idNrValue == "" ? "" : " AND ") + "f.BRM_BARCODE = '" + sBRMNoValue + "'")
                                  + (sCLMNoValue == "" ? "" : " AND f.UNQ_FILE_NO = '" + sCLMNoValue + "'")
                              );
                        }
                        else if (sCLMNoValue != "")
                        {
                            query = context.Database.SqlQuery<FileRequest>
                              (@" SELECT
                                    f.APPLICANT_NO AS ID_NO,
                                    f.FILE_NUMBER AS MIS_FILE_NO,
                                    f.BRM_BARCODE AS BRM_BARCODE,
                                    CAST(f.BATCH_NO AS VARCHAR(50)) AS BATCH_NO,
                                    b.REG_TYPE AS REG_TYPE,
                                    f.GRANT_TYPE AS GRANT_TYPE,
                                    f.USER_FIRSTNAME AS NAME,
                                    f.USER_LASTNAME AS SURNAME,
                                    r.REGION_ID AS REGION_ID,
                                    r.REGION_CODE AS REGION_CODE,
                                    r.REGION_NAME AS REGION_NAME,
                                    f.TDW_BOXNO AS TDW_BOXNO,
                                    f.TRANS_DATE AS APP_DATE_DT,
                                    f.UNQ_FILE_NO AS UNQ_FILE_NO,
                                    TO_NUMBER(f.ARCHIVE_YEAR) AS ARCHIVE_YEAR,
                                    f.APPLICATION_STATUS AS APPLICATION_STATUS,
                                    f.UPDATED_BY_AD AS UPDATED_BY,
                                    m.PARENT_BRM_BARCODE AS PARENT_BRM_NUMBER
                                FROM CONTENTSERVER.DC_FILE f
                                INNER JOIN CONTENTSERVER.DC_REGION r ON r.REGION_ID = f.REGION_ID
                                LEFT OUTER JOIN DC_MERGE m on m.BRM_BARCODE = f.BRM_BARCODE
                                LEFT OUTER JOIN DC_BATCH b on b.batch_no = f.Batch_no
                                WHERE "
                                  + (idNrValue == "" ? "" : "f.APPLICANT_NO IN (" + util.getIDList(idNrValue) + ")")
                                  + ((idNrValue == "" ? "" : " AND ") + "f.UNQ_FILE_NO = '" + sCLMNoValue + "'")
                              );
                        }
                        else if (sSRDNoValue != "")
                        {
                            query = context.Database.SqlQuery<FileRequest>
                              ($@"SELECT DISTINCT
                                    CAST(SRDB.ID_NO AS VARCHAR(50)) AS ID_NO
                                    , CAST(SRDG.SOCIAL_RELIEF_NO AS VARCHAR(50))  AS MIS_FILE_NO
                                    , CAST(f.BRM_BARCODE AS VARCHAR(50))  AS BRM_BARCODE
                                    , CAST(f.BATCH_NO AS VARCHAR(50)) AS BATCH_NO
                                    , b.REG_TYPE AS REG_TYPE
                                    , CAST(SRDG.GRANT_TYPE AS VARCHAR(50))  AS GRANT_TYPE
                                    , CAST(SRDG.GRANT_TYPE AS VARCHAR(50))  AS MIS_GRANT_TYPE
                                    , CAST(SRDB.NAME AS VARCHAR(50))  AS NAME
                                    , CAST(SRDB.SURNAME AS VARCHAR(50))  AS SURNAME
                                    , CAST(SRDB.PROVINCE AS VARCHAR(50))  AS REGION_ID
                                    , CAST(DCR.REGION_CODE AS VARCHAR(50))  AS REGION_CODE
                                    , CAST(DCR.REGION_NAME AS VARCHAR(50))  AS REGION_NAME
                                    , CAST('' AS VARCHAR(50))  AS BIN_ID
                                    , CAST(f.MIS_BOXNO AS VARCHAR(50))  AS BOX_NUMBER
                                    , CAST(NULL AS VARCHAR(50))  AS POSITION
                                    , CAST(f.TDW_BOXNO AS VARCHAR(50))  AS TDW_BOXNO
                                    , f.TRANS_DATE AS APP_DATE_DT
                                    , CAST(SRDG.GRANT_APPLICATION_DATE AS VARCHAR(50)) AS APP_DATE
                                    , CAST(f.UNQ_FILE_NO AS VARCHAR(50))  AS UNQ_FILE_NO
                                    , TO_NUMBER(f.ARCHIVE_YEAR)  AS ARCHIVE_YEAR
                                    , CAST(f.APPLICATION_STATUS AS VARCHAR(50))  AS APPLICATION_STATUS
                                    , f.UPDATED_BY_AD AS UPDATED_BY
                                    , m.PARENT_BRM_BARCODE AS PARENT_BRM_NUMBER
                               FROM
                                    SASSA.SOCPEN_SRD_GRANTS SRDG
                                JOIN SASSA.SOCPEN_SRD_BEN SRDB ON SRDG.SOCIAL_RELIEF_NO = SRDB.SRD_NO
                                LEFT JOIN DC_FILE f ON f.SRD_NO = SRDG.SOCIAL_RELIEF_NO
                                JOIN DC_REGION DCR ON DCR.REGION_ID = SRDB.PROVINCE
                                LEFT OUTER JOIN DC_MERGE m on m.BRM_BARCODE = f.BRM_BARCODE
                                LEFT OUTER JOIN DC_BATCH b on b.batch_no = f.Batch_no
                                WHERE SRDG.SOCIAL_RELIEF_NO = '{sSRDNoValue}'");
                        }
                        else
                        {
                            if (Usersession.Office.RegionId == "2")
                            {
                                query = context.Database.SqlQuery<FileRequest>
                                ($@" SELECT
                                        CAST(CAST(mis.ID_NUMBER AS VARCHAR2 (20)) AS VARCHAR(50))  AS ID_NO,
                                        CAST(CAST(TRIM(mis.FORM_TYPE)||TRIM(mis.FORM_NUMBER) AS VARCHAR2 (30)) AS VARCHAR(50))  AS MIS_FILE_NO,
                                        CAST(f.BRM_BARCODE AS VARCHAR(50))  AS BRM_BARCODE,
                                        CAST(f.BATCH_NO AS VARCHAR(50)) AS BATCH_NO,
                                        b.REG_TYPE AS REG_TYPE,
                                        CAST(f.GRANT_TYPE AS VARCHAR(50))  AS GRANT_TYPE,
                                        CAST(UPPER(mis.GRANT_TYPE) AS VARCHAR(50))  AS MIS_GRANT_TYPE,
                                        CAST(mis.NAME  AS VARCHAR(50)) AS NAME,
                                        CAST(mis.SURNAME AS VARCHAR(50))  AS SURNAME,
                                        CAST(r.REGION_ID AS VARCHAR(50))  AS REGION_ID,
                                        CAST(r.REGION_CODE AS VARCHAR(50))  AS REGION_CODE,
                                        CAST(r.REGION_NAME AS VARCHAR(50))  AS REGION_NAME,
                                        CAST('' AS VARCHAR(50))  AS BIN_ID,
                                        CAST(CAST(mis.BOX AS VARCHAR2 (10)) AS VARCHAR(50))  AS BOX_NUMBER,
                                        CAST(CAST(mis.POSITN AS VARCHAR2 (10)) AS VARCHAR(50))  AS POSITION,
                                        CAST(f.TDW_BOXNO  AS VARCHAR(50)) AS TDW_BOXNO,
                                        f.TRANS_DATE AS APP_DATE_DT,
                                        mis.APPLICATION_DATE AS APP_DATE,
                                        CAST(f.UNQ_FILE_NO AS VARCHAR(50))  AS UNQ_FILE_NO,
                                        GREATEST(TO_NUMBER(SUBSTR(NVL(spn.STATUS_DATE1, '0'), 0, 4)), TO_NUMBER(SUBSTR(NVL(spn.STATUS_DATE2, '0'), 0, 4)), TO_NUMBER(SUBSTR(NVL(spn.STATUS_DATE3, '0'), 0, 4)), TO_NUMBER(SUBSTR(NVL(spn.STATUS_DATE4, '0'), 0, 4))) AS ARCHIVE_YEAR,
                                        CAST(CASE WHEN (CONCAT(SEC_STATUS1, PRIM_STATUS1) IN ('2A', '2B', '29')
                                                OR CONCAT(SEC_STATUS2, PRIM_STATUS2) IN ('2A', '2B', '29')
                                                OR CONCAT(SEC_STATUS3, PRIM_STATUS3) IN ('2A', '2B', '29')
                                                OR CONCAT(SEC_STATUS4, PRIM_STATUS4) IN ('2A', '2B', '29'))
                                        THEN 'MAIN' ELSE 'ARCHIVE' END AS VARCHAR(50))  AS APPLICATION_STATUS,
                                        f.UPDATED_BY_AD AS UPDATED_BY,
                                        m.PARENT_BRM_BARCODE AS PARENT_BRM_NUMBER
                                    FROM CONTENTSERVER.SS_APPLICATION mis
                                    INNER JOIN CONTENTSERVER.DC_REGION r ON r.REGION_ID = '2'
                                    LEFT OUTER JOIN CONTENTSERVER.SOCPENBRM spn ON mis.ID_NUMBER IN (spn.PENSION_NO, spn.OLD_ID1, spn.OLD_ID2, spn.OLD_ID3, spn.OLD_ID4, spn.OLD_ID5, spn.OLD_ID6, spn.OLD_ID7, spn.OLD_ID8, spn.OLD_ID9, spn.OLD_ID10)
                                    LEFT OUTER JOIN CONTENTSERVER.DC_FILE f ON f.FILE_NUMBER = CAST(TRIM(mis.FORM_TYPE)||TRIM(mis.FORM_NUMBER) AS VARCHAR2 (30))
                                        {(sBRMNoValue?.Length == 0 ? "" : " OR f.BRM_BARCODE = '" + sBRMNoValue + "'")}
                                        {(sCLMNoValue?.Length == 0 ? "" : " OR f.UNQ_FILE_NO = '" + sCLMNoValue + "'")}
                                    LEFT OUTER JOIN DC_MERGE m on m.BRM_BARCODE = f.BRM_BARCODE
                                    LEFT OUTER JOIN DC_BATCH b on b.batch_no = f.Batch_no
                                    WHERE
                                    {(idNrValue == "" ? "" : $"mis.ID_NUMBER = '{idNrValue}'")}
                                    {(sBRMNoValue == "" ? "" : ((idNrValue?.Length == 0 ? "" : " AND ") + "f.BRM_BARCODE IS NOT NULL"))}
                                    {(sCLMNoValue == "" ? "" : ((idNrValue?.Length == 0 && sBRMNoValue?.Length == 0 ? "" : " AND ") + "f.UNQ_FILE_NO IS NOT NULL"))}
                                    UNION ALL
                                        SELECT
                                            CAST(f.APPLICANT_NO AS VARCHAR(50)) AS ID_NO,
                                            CAST(f.FILE_NUMBER AS VARCHAR(50)) AS MIS_FILE_NO,
                                            CAST(f.BRM_BARCODE AS VARCHAR(50)) AS BRM_BARCODE,
                                            CAST(f.BATCH_NO AS VARCHAR(50)) AS BATCH_NO,
                                            b.REG_TYPE AS REG_TYPE,
                                            CAST(f.GRANT_TYPE AS VARCHAR(50)) AS GRANT_TYPE,
                                            CAST(f.GRANT_TYPE AS VARCHAR(50)) AS MIS_GRANT_TYPE,
                                            CAST(f.user_firstname AS VARCHAR(50)) AS NAME,
                                            CAST(f.user_lastname AS VARCHAR(50)) AS SURNAME,
                                            CAST(r.REGION_ID AS VARCHAR(50)) AS REGION_ID,
                                            CAST(r.REGION_CODE AS VARCHAR(50)) AS REGION_CODE,
                                            CAST(r.REGION_NAME AS VARCHAR(50)) AS REGION_NAME,
                                            NULL AS BIN_ID,
                                            CAST(f.MIS_BOXNO AS VARCHAR(50)) AS BOX_NUMBER,
                                            NULL AS POSITION,
                                            CAST(f.TDW_BOXNO AS VARCHAR(50)) AS TDW_BOXNO,
                                            f.TRANS_DATE AS APP_DATE_DT,
                                            NULL AS APP_DATE,
                                            CAST(f.UNQ_FILE_NO AS VARCHAR(50)) AS UNQ_FILE_NO,
                                            TO_NUMBER(f.ARCHIVE_YEAR) AS ARCHIVE_YEAR,
                                            CAST(f.APPLICATION_STATUS AS VARCHAR(50)) AS APPLICATION_STATUS,
                                            f.UPDATED_BY_AD AS UPDATED_BY,
                                            m.PARENT_BRM_BARCODE AS PARENT_BRM_NUMBER
                                        FROM DC_FILE F
                                        INNER JOIN DC_REGION r ON r.REGION_ID = f.REGION_ID
                                        LEFT JOIN CONTENTSERVER.SS_APPLICATION mis ON F.FILE_NUMBER = CAST(TRIM(mis.FORM_TYPE)||TRIM(mis.FORM_NUMBER) AS VARCHAR2 (30))
                                        {(sBRMNoValue?.Length == 0 ? "" : " OR f.BRM_BARCODE = '" + sBRMNoValue + "'")}
                                        {(sCLMNoValue?.Length == 0 ? "" : " OR f.UNQ_FILE_NO = '" + sCLMNoValue + "'")}
                                        LEFT OUTER JOIN DC_MERGE m on m.BRM_BARCODE = f.BRM_BARCODE
                                        LEFT OUTER JOIN DC_BATCH b on b.batch_no = f.Batch_no
                                        WHERE mis.FORM_NUMBER IS NULL
                                        AND APPLICANT_NO = '{idNrValue}'"
                                );
                            }
                            else
                            {//Tested
                                query = context.Database.SqlQuery<FileRequest>
                                ($@"SELECT
                                        CAST(mis.ID_NUMBER AS VARCHAR(50)) AS ID_NO,
                                        CAST(mis.FILE_NUMBER AS VARCHAR(50)) AS MIS_FILE_NO,
                                        CAST(f.BRM_BARCODE AS VARCHAR(50)) AS BRM_BARCODE,
                                        CAST(f.BATCH_NO AS VARCHAR(50)) AS BATCH_NO,
                                        b.REG_TYPE AS REG_TYPE,
                                        CAST(f.GRANT_TYPE AS VARCHAR(50)) AS GRANT_TYPE,
                                        CAST(UPPER(mis.GRANT_TYPE) AS VARCHAR(50)) AS MIS_GRANT_TYPE,
                                        CAST(mis.NAME AS VARCHAR(50)) AS NAME,
                                        CAST(mis.SURNAME AS VARCHAR(50)) AS SURNAME,
                                        CAST(r.REGION_ID AS VARCHAR(50)) AS REGION_ID,
                                        CAST(r.REGION_CODE AS VARCHAR(50)) AS REGION_CODE,
                                        CAST(r.REGION_NAME AS VARCHAR(50)) AS REGION_NAME,
                                        CAST(mis.BIN_ID AS VARCHAR(50)) AS BIN_ID,
                                        CAST(mis.BOX_NUMBER AS VARCHAR(50)) AS BOX_NUMBER,
                                        CAST(mis.POSITION AS VARCHAR(50)) AS POSITION,
                                        CAST(f.TDW_BOXNO AS VARCHAR(50)) AS TDW_BOXNO,
                                        f.TRANS_DATE AS APP_DATE_DT,
                                        mis.APP_DATE AS APP_DATE,
                                        CAST(f.UNQ_FILE_NO AS VARCHAR(50)) AS UNQ_FILE_NO,
                                        GREATEST(TO_NUMBER(SUBSTR(NVL(spn.STATUS_DATE1, '0'), 0, 4)), TO_NUMBER(SUBSTR(NVL(spn.STATUS_DATE2, '0'), 0, 4)), TO_NUMBER(SUBSTR(NVL(spn.STATUS_DATE3, '0'), 0, 4)), TO_NUMBER(SUBSTR(NVL(spn.STATUS_DATE4, '0'), 0, 4))) AS ARCHIVE_YEAR,
                                        CAST(CASE WHEN (CONCAT(SEC_STATUS1, PRIM_STATUS1) IN ('2A', '2B', '29')
                                            OR CONCAT(SEC_STATUS2, PRIM_STATUS2) IN ('2A', '2B', '29')
                                            OR CONCAT(SEC_STATUS3, PRIM_STATUS3) IN ('2A', '2B', '29')
                                            OR CONCAT(SEC_STATUS4, PRIM_STATUS4) IN ('2A', '2B', '29'))
                                        THEN 'MAIN' ELSE 'ARCHIVE' END AS VARCHAR(50)) AS APPLICATION_STATUS,
                                        f.UPDATED_BY_AD AS UPDATED_BY,
                                        m.PARENT_BRM_BARCODE AS PARENT_BRM_NUMBER
                                    FROM CONTENTSERVER.MIS_LIVELINK_TBL mis
                                    INNER JOIN CONTENTSERVER.DC_REGION r ON r.REGION_ID = mis.REGION_ID
                                    LEFT OUTER JOIN CONTENTSERVER.SOCPENBRM spn ON mis.ID_NUMBER IN (spn.PENSION_NO, spn.OLD_ID1, spn.OLD_ID2, spn.OLD_ID3, spn.OLD_ID4, spn.OLD_ID5, spn.OLD_ID6, spn.OLD_ID7, spn.OLD_ID8, spn.OLD_ID9, spn.OLD_ID10)
                                    LEFT OUTER JOIN CONTENTSERVER.DC_FILE f ON f.FILE_NUMBER = mis.FILE_NUMBER
                                    LEFT OUTER JOIN CONTENTSERVER.DC_MERGE m on m.BRM_BARCODE = f.BRM_BARCODE
                                    LEFT OUTER JOIN DC_BATCH b on b.batch_no = f.Batch_no
                                    WHERE mis.ID_NUMBER = '{idNrValue}'

                                    UNION ALL
                                        SELECT
                                            CAST(f.APPLICANT_NO AS VARCHAR(50)) AS ID_NO,
                                            CAST(f.FILE_NUMBER AS VARCHAR(50)) AS MIS_FILE_NO,
                                            CAST(f.BRM_BARCODE AS VARCHAR(50)) AS BRM_BARCODE,
                                            CAST(f.BATCH_NO AS VARCHAR(50)) AS BATCH_NO,
                                            b.REG_TYPE AS REG_TYPE,
                                            CAST(f.GRANT_TYPE AS VARCHAR(50)) AS GRANT_TYPE,
                                            CAST(f.GRANT_TYPE AS VARCHAR(50)) AS MIS_GRANT_TYPE,
                                            CAST(f.user_firstname AS VARCHAR(50)) AS NAME,
                                            CAST(f.user_lastname AS VARCHAR(50)) AS SURNAME,
                                            CAST(r.REGION_ID AS VARCHAR(50)) AS REGION_ID,
                                            CAST(r.REGION_CODE AS VARCHAR(50)) AS REGION_CODE,
                                            CAST(r.REGION_NAME AS VARCHAR(50)) AS REGION_NAME,
                                            NULL AS BIN_ID,
                                            CAST(f.MIS_BOXNO AS VARCHAR(50)) AS BOX_NUMBER,
                                            NULL AS POSITION,
                                            CAST(f.TDW_BOXNO AS VARCHAR(50)) AS TDW_BOXNO,
                                            f.TRANS_DATE AS APP_DATE_DT,
                                            NULL AS APP_DATE,
                                            CAST(f.UNQ_FILE_NO AS VARCHAR(50)) AS UNQ_FILE_NO,
                                            TO_NUMBER(f.ARCHIVE_YEAR) AS ARCHIVE_YEAR,
                                            CAST(f.APPLICATION_STATUS AS VARCHAR(50)) AS APPLICATION_STATUS,
                                            f.UPDATED_BY_AD AS UPDATED_BY,
                                            m.PARENT_BRM_BARCODE AS PARENT_BRM_NUMBER
                                        FROM DC_FILE F
                                        INNER JOIN DC_REGION r ON r.REGION_ID = f.REGION_ID
                                        LEFT JOIN CONTENTSERVER.MIS_LIVELINK_TBL mis ON F.FILE_NUMBER = mis.FILE_NUMBER
                                        LEFT OUTER JOIN CONTENTSERVER.DC_MERGE m on m.BRM_BARCODE = f.BRM_BARCODE
                                        LEFT OUTER JOIN DC_BATCH b on b.batch_no = f.Batch_no
                                        WHERE mis.FILE_NUMBER IS NULL
                                        AND APPLICANT_NO = '{idNrValue}'");
                            }
                        }

                        foreach (FileRequest value in query.OrderBy(x => x.ID_NO))
                        {
                            DataRow dr = DT.NewRow();
                            dr["ID_Number"] = value.ID_NO;
                            dr["BRM_BARCODE"] = value.BRM_BARCODE;
                            dr["Batch_No"] = value.BATCH_NO;
                            dr["Type"] = value.REG_TYPE;
                            dr["File_Number"] = value.MIS_FILE_NO;
                            dr["Name"] = value.NAME;
                            dr["Surname"] = value.SURNAME;
                            dr["Region"] = value.REGION_NAME;
                            dr["RegionID"] = value.REGION_ID;
                            dr["MISBin"] = value.BIN_ID;
                            dr["MISBox"] = value.BOX_NUMBER;
                            dr["MISPos"] = value.POSITION;
                            dr["TDWBoxno"] = value.TDW_BOXNO;
                            //dr["GrantID"] = value.GRANT_TYPE;
                            dr["AppDate"] = value.APP_DATE_DT.HasValue ? value.APP_DATE_DT.Value.ToString("yyyy/MM/dd") : (value.APP_DATE == null ? "" : DateTime.Parse(value.APP_DATE).ToString("yyyy/MM/dd"));
                            dr["ArchiveDate"] = value.ARCHIVE_YEAR.HasValue ? value.ARCHIVE_YEAR.Value.ToString() : "";
                            dr["UNQ_FILE_NO"] = value.UNQ_FILE_NO;
                            dr["UPDATED_BY"] = value.UPDATED_BY;
                            dr["PARENT_BRM_NUMBER"] = value.PARENT_BRM_BARCODE;

                            if (value.ARCHIVE_YEAR != null                                              //APPLICATION_STATUS not retrieved from DC_FILE
                                && value.APPLICATION_STATUS == "ARCHIVE"                                //File is in ARCHIVE status
                                && value.ARCHIVE_YEAR != 0                                              //ARCHIVE_YEAR is a proper year
                                && value.ARCHIVE_YEAR + 6 <= int.Parse(DateTime.Now.Year.ToString())    //Current year is at least 6 years after archive year
                                && String.IsNullOrEmpty(util.CheckExclusions(value.ID_NO)))  //No existing exclusions for this file
                            {
                                value.APPLICATION_STATUS = "DESTROY";
                            }
                            dr["AppStatus"] = value.APPLICATION_STATUS;

                            string sGrantName = "";
                            if (value.GRANT_TYPE == null)
                            {
                                if (util.getGrantTypes().TryGetValue(BoxFileActions.convertMISGrantTypeToSocpen(value.MIS_GRANT_TYPE), out sGrantName))
                                {
                                    dr["GrantType"] = sGrantName;
                                }
                            }
                            else
                            {
                                dr["GrantType"] = util.getGrantTypes()[value.GRANT_TYPE];
                            }

                            DT.Rows.Add(dr);
                        }
                        //}
                        //else
                        if (DT.Rows.Count == 0)
                        {
                            lblError.Text = "No results were found for the search value";
                            divError.Style.Add("display", "");
                            gvResults.DataSource = DT;
                            gvResults.DataBind();
                            return;
                        }

                        gvResults.DataSource = DT;
                        gvResults.DataBind();
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = ex.Message;
                        lblError.Text += "<br />" + ex.InnerException;
                        lblError.Text += "<br />Source: " + ex.Source;
                        lblError.Text += "<br />Source: " + ex.StackTrace;
                        divError.Style.Add("display", "");
                    }
                }
            }
        }

        #endregion Private Methods
    }
}
