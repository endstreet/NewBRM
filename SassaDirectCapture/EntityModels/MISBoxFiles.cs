using System;

namespace SASSADirectCapture.EntityModels
{
    public class MISBoxFiles
    {
        public string UNQ_FILE_NO { get; set; }
        public string ID_NUMBER { get; set; }
        public string BOX_NUMBER { get; set; }
        public string MIS_BOX_STATUS { get; set; }
        public string BRM_NO { get; set; }
        public string TDW_BOXNO { get; set; }
        public string MIS_REBOX_STATUS { get; set; }
        public bool REBOX_STATUS { get; set; }
        public string NAME { get; set; }
        public string SURNAME { get; set; }
        public string FULL_NAME { get; set; }
        public string GRANT_TYPE { get; set; }
        public string GRANT_NAME { get; set; }
        public string APP_DATE { get; set; }
        public string REGION_ID { get; set; }
        public string REGION_NAME { get; set; }
        public string REGISTRY_TYPE { get; set; }
        public DateTime? MIS_BOX_DATE { get; set; }
        public DateTime? MIS_REBOX_DATE { get; set; }
        public string FILE_STATUS { get; set; }
        public bool FILE_AUDITED { get; set; }
        public string REGION_ID_FROM { get; set; }
        public string REGION_NAME_FROM { get; set; }
        public string TRANSFER_REGION { get; set; }
        public int? TEMP_BOX_NO { get; set; }
        public string CHILD_ID_NO { get; set; }
        public int? PRINT_ORDER { get; set; }

        //SOCPEN FIELDS
        public string SOCPEN_GRANT_TYPE { get; set; }

        public string GRANT_TYPE1 { get; set; }
        public string GRANT_TYPE2 { get; set; }
        public string GRANT_TYPE3 { get; set; }
        public string GRANT_TYPE4 { get; set; }

        public string SOCPEN_APP_DATE { get; set; }
        public string APP_DATE1 { get; set; }
        public string APP_DATE2 { get; set; }
        public string APP_DATE3 { get; set; }
        public string APP_DATE4 { get; set; }

        public string SOCPEN_PRIM_STATUS { get; set; }
        public string PRIM_STATUS1 { get; set; }
        public string PRIM_STATUS2 { get; set; }
        public string PRIM_STATUS3 { get; set; }
        public string PRIM_STATUS4 { get; set; }

        public string SOCPEN_SEC_STATUS { get; set; }
        public string SEC_STATUS1 { get; set; }
        public string SEC_STATUS2 { get; set; }
        public string SEC_STATUS3 { get; set; }
        public string SEC_STATUS4 { get; set; }

        public string SOCPEN_STATUS_DATE { get; set; }
        public string STATUS_DATE1 { get; set; }
        public string STATUS_DATE2 { get; set; }
        public string STATUS_DATE3 { get; set; }
        public string STATUS_DATE4 { get; set; }

        public string SOCPEN_PROVINCE { get; set; }

        // CALCULATED FILEDS
        public string ARCHIVE_YEAR { get; set; }

        public string EXCLUSIONS { get; set; }
        public string MISSING { get; set; }
        public bool IsFound { get; set; }
        public string NON_COMPLIANT { get; set; }
        public bool IsNonCompliant { get; set; }
        public string FILE_NUMBER { get; set; }
        public string REGTYPE_MAIN { get; set; }
        public string REGTYPE_ARCHIVE { get; set; }
        public string TRANSFERRED { get; set; }
        public string STATUS_TRANSFERRED { get; set; }
        public string STATUS_NON_COMPLIANT { get; set; }
        public string STATUS_LEGAL { get; set; }
        public string STATUS_FRAUD { get; set; }
        public string STATUS_DEBTORS { get; set; }
        public string COVER_DATE { get; set; }
        public string DOCS_PRESENT { get; set; }

        // BOOLEANS FOR CALCULATED FIELDS
        // CALCULATED FILEDS
        public bool CHECK_MISSING { get; set; }

        public bool CHECK_NON_COMPLIANT { get; set; }
        public bool CHECK_REGTYPE_MAIN { get; set; }
        public bool CHECK_REGTYPE_ARCHIVE { get; set; }
        public bool CHECK_TRANSFERRED { get; set; }
        public bool CHECK_STATUS_TRANSFERRED { get; set; }
        public bool CHECK_STATUS_NON_COMPLIANT { get; set; }
        public bool CHECK_STATUS_LEGAL { get; set; }
        public bool CHECK_STATUS_FRAUD { get; set; }
        public bool CHECK_STATUS_DEBTORS { get; set; }
    }
}