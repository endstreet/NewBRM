using System;
using System.Collections.Generic;

namespace Sassa.BRM.Data.Models
{
    public partial class DcReqCategoryTypeLink
    {
        public DcReqCategoryTypeLink()
        {
            DcFileRequest = new HashSet<DcFileRequest>();
        }

        public decimal CategoryId { get; set; }
        public decimal TypeId { get; set; }

        public virtual DcReqCategory Category { get; set; }
        public virtual DcReqCategoryType Type { get; set; }
        public virtual ICollection<DcFileRequest> DcFileRequest { get; set; }
    }
}
