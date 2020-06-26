using System;
using System.Collections.Generic;

namespace Sassa.BRM.Data.Models
{
    public partial class DcReqCategory
    {
        public DcReqCategory()
        {
            DcReqCategoryTypeLink = new HashSet<DcReqCategoryTypeLink>();
            DcStakeholder = new HashSet<DcStakeholder>();
        }

        public decimal CategoryId { get; set; }
        public string CategoryDescr { get; set; }

        public virtual ICollection<DcReqCategoryTypeLink> DcReqCategoryTypeLink { get; set; }
        public virtual ICollection<DcStakeholder> DcStakeholder { get; set; }
    }
}
