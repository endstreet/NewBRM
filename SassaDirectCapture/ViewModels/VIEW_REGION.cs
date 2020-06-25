namespace SASSADirectCapture.EntityModels
{
    public class VIEW_REGION
    {
        string _id;
        public string RegionString
        {
            set
            {
                this._id = value;
                RegionId = int.Parse(_id);
            }
            get
            {
                return _id;
            }
        }
        public int? RegionId { get; set; }

        public string RegionName { get; set; }
    }
}