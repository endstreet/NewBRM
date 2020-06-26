using System;
using System.Collections.Generic;

namespace Sassa.BRM.Data.Models
{
    public partial class DcExclusions
    {
        public string IdNo { get; set; }
        public DateTime? ExclDate { get; set; }
        public decimal RegionId { get; set; }
        public string Username { get; set; }
        public string ExclusionType { get; set; }
        public decimal? ExclusionBatchId { get; set; }
    }
}
