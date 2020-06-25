namespace SASSADirectCapture.EntityModels
{
    public partial class VIEW_EXCLUSION_BATCH
    {
        public int BATCH_ID { get; set; }
        public string REGION_NAME { get; set; }
        public int? REGION_ID { get; set; }
        public string EXCLUSION_YEAR { get; set; }
        public string CREATED_BY { get; set; }
        public string CREATED_DATE { get; set; }
        public string APPROVED_BY { get; set; }
        public string APPROVED_DATE { get; set; }
    }
}