using System;

namespace SASSADirectCapture.EntityModels
{
    public class MISBoxesPicked
    {
        public string UNQ_PICKLIST { get; internal set; }
        public string UNQ_NO { get; internal set; }
        public string BIN_NUMBER { get; set; }
        public string BOX_NUMBER { get; set; }
        public string BOX_RECEIVED { get; set; }
        public string BOX_COMPLETED { get; set; }
        public string ARCHIVE_YEAR { get; set; }
        public string REGION_ID { get; set; }
        public string REGISTRY_TYPE { get; set; }
        public decimal? USERID { get; set; }
        public DateTime? PICKLIST_DATE { get; set; }
        public string PICKLIST_STATUS { get; set; }
    }
}