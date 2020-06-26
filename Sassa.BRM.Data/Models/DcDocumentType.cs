using System;
using System.Collections.Generic;

namespace Sassa.BRM.Data.Models
{
    public partial class DcDocumentType
    {
        public DcDocumentType()
        {
            DcGrantDocLink = new HashSet<DcGrantDocLink>();
        }

        public decimal TypeId { get; set; }
        public string TypeName { get; set; }

        public virtual ICollection<DcGrantDocLink> DcGrantDocLink { get; set; }
    }
}
