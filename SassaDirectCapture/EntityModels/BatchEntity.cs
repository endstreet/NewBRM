using System;

namespace SASSADirectCapture.EntityModels
{
    public class BatchEntity
    {
        public decimal BATCH_NO { get; set; }
        public string OFFICE_NAME { get; set; }
        public decimal UPDATED_BY { get; set; }
        public string UPDATED_BY_AD { get; set; }//Johan Added
        public System.DateTime UPDATED_DATE { get; set; }
        public string BATCH_CURRENT { get; set; }
        public string BATCH_STATUS { get; set; }
        public string BATCH_COMMENT { get; set; }
        public string WAYBILL_NO { get; set; }
        public string COURIER_NAME { get; set; }
        public Nullable<System.DateTime> WAYBILL_DATE { get; set; }
        public int NO_OF_FILES { get; set; }
        public string UPDATED_NAME { get; set; }
    }
}