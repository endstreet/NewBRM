using System;

namespace SASSADirectCapture.EntityModels
{
    public class SocpenSRD
    {
        public Int64? APPLICANT_NO { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string GRANT_TYPE { get; set; }
        public DateTime? APP_DATE { get; set; }
        public string REGION_ID { get; set; }
        public string REGION_CODE { get; set; }
        public string REGION_NAME { get; set; }
        public DateTime? TRANS_DATE { get; set; }
        public string APPLICATION_STATUS { get; set; }
    }
}