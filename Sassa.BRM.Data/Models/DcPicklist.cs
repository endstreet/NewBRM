using System;
using System.Collections.Generic;

namespace Sassa.BRM.Data.Models
{
    public partial class DcPicklist
    {
        public string UnqPicklist { get; set; }
        public string RegionId { get; set; }
        public string RegistryType { get; set; }
        public decimal? Userid { get; set; }
        public DateTime? PicklistDate { get; set; }
        public string PicklistStatus { get; set; }
    }
}
