using System;

namespace SASSADirectCapture.EntityModels
{
    public class Applicant
    {
        public string UNQ_FILE_NO { get; set; }
        public string APPLICANT_NO { get; set; }

        public string MIS_FILE_NO { get; set; }

        public string FIRSTNAME { get; set; }

        public string LASTNAME { get; set; }
        public string GRANT_TYPE1 { get; set; }
        public string GRANT_TYPE2 { get; set; }
        public string GRANT_TYPE3 { get; set; }
        public string GRANT_TYPE4 { get; set; }

        public string GRANT_NAME { get; set; }

        public string APP_DATE1 { get; set; }
        public string APP_DATE2 { get; set; }
        public string APP_DATE3 { get; set; }
        public string APP_DATE4 { get; set; }

        public decimal? TRANS_TYPE { get; set; }
        public string TRANS_NAME { get; set; }

        public string REGION_ID { get; set; }

        public string REGION_CODE { get; set; }

        public string REGION_NAME { get; set; }

        public string TRANS_DATE { get; set; }

        public DateTime? TRANS_DATE_DATE { get; set; }

        public string DOCS_PRESENT { get; set; }

        public string APPLICATION_STATUS { get; set; }
        public string BRM_BARCODE { get; internal set; }

        public string MIS_BOXNO { get; set; }

        public DateTime? MIS_BOX_DATE { get; set; }

        public DateTime? MIS_REBOX_DATE { get; set; }
    }
}