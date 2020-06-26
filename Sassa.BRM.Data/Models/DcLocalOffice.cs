using System;
using System.Collections.Generic;

namespace Sassa.BRM.Data.Models
{
    public partial class DcLocalOffice
    {
        public DcLocalOffice()
        {
            DcBatch = new HashSet<DcBatch>();
            DcFile = new HashSet<DcFile>();
            DcOfficeKuafLink = new HashSet<DcOfficeKuafLink>();
        }

        public string OfficeId { get; set; }
        public string OfficeName { get; set; }
        public string RegionId { get; set; }
        public string OfficeType { get; set; }
        public string District { get; set; }

        public virtual DcRegion Region { get; set; }
        public virtual ICollection<DcBatch> DcBatch { get; set; }
        public virtual ICollection<DcFile> DcFile { get; set; }
        public virtual ICollection<DcOfficeKuafLink> DcOfficeKuafLink { get; set; }
    }
}
