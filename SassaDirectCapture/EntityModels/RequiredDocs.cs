namespace SASSADirectCapture.EntityModels
{
    public class RequiredDocs
    {
        public decimal DOC_ID { get; set; }
        public string DOC_NAME { get; set; }

        public string DOC_SECTION { get; set; }

        public string DOC_CRITICAL { get; set; }
        public bool DOC_PRESENT { get; set; }
    }
}