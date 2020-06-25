using System;

namespace SASSADirectCapture.EntityModels
{
    public class PickLists
    {
        public string REGION_ID { get; set; }
        public string REGION_NAME { get; set; }
        public string UNQ_PICKLIST { get; set; }
        public string REGISTRY_TYPE { get; set; }
        public string REGISTRY_ABBR { get; set; }
        public string USERID { get; set; }
        public string USERNAME { get; set; }
        public DateTime? PICKLIST_DATE { get; set; }
        public string PICKLIST_STATUS { get; set; }
        public string NO_BOXES { get; set; }
    }
}