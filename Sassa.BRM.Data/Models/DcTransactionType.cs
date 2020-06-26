using System;
using System.Collections.Generic;

namespace Sassa.BRM.Data.Models
{
    public partial class DcTransactionType
    {
        public DcTransactionType()
        {
            DcFile = new HashSet<DcFile>();
            DcGrantDocLink = new HashSet<DcGrantDocLink>();
        }

        public decimal TypeId { get; set; }
        public string TypeName { get; set; }
        public string ServiceCategory { get; set; }

        public virtual ICollection<DcFile> DcFile { get; set; }
        public virtual ICollection<DcGrantDocLink> DcGrantDocLink { get; set; }
    }
}
