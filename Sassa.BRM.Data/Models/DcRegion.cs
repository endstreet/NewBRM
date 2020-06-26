using System;
using System.Collections.Generic;

namespace Sassa.BRM.Data.Models
{
    public partial class DcRegion
    {
        public DcRegion()
        {
            DcFileRequest = new HashSet<DcFileRequest>();
            DcLocalOffice = new HashSet<DcLocalOffice>();
        }

        public string RegionId { get; set; }
        public string RegionName { get; set; }
        public string RegionCode { get; set; }

        public virtual ICollection<DcFileRequest> DcFileRequest { get; set; }
        public virtual ICollection<DcLocalOffice> DcLocalOffice { get; set; }
    }
}
