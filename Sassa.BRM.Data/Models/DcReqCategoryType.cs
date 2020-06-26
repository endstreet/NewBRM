using System;
using System.Collections.Generic;

namespace Sassa.BRM.Data.Models
{
    public partial class DcReqCategoryType
    {
        public DcReqCategoryType()
        {
            DcReqCategoryTypeLink = new HashSet<DcReqCategoryTypeLink>();
        }

        public decimal TypeId { get; set; }
        public string TypeDescr { get; set; }

        public virtual ICollection<DcReqCategoryTypeLink> DcReqCategoryTypeLink { get; set; }
    }
}
