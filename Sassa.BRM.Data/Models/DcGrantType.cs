using System;
using System.Collections.Generic;

namespace Sassa.BRM.Data.Models
{
    public partial class DcGrantType
    {
        public DcGrantType()
        {
            DcFile = new HashSet<DcFile>();
            DcGrantDocLink = new HashSet<DcGrantDocLink>();
        }

        public string TypeId { get; set; }
        public string TypeName { get; set; }

        public virtual ICollection<DcFile> DcFile { get; set; }
        public virtual ICollection<DcGrantDocLink> DcGrantDocLink { get; set; }
    }
}
