using System;
using System.Collections.Generic;

namespace Sassa.BRM.Data.Models
{
    public partial class DcDestructionList
    {
        public string Region { get; set; }
        public string PensionNo { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string GrantType { get; set; }
        public string StatusDate { get; set; }
    }
}
