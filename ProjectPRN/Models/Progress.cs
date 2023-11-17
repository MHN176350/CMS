using System;
using System.Collections.Generic;

namespace ProjectPRN.Models
{
    public partial class Progress
    {
        public int Id { get; set; }
        public string? As1 { get; set; }
        public string? As2 { get; set; }
        public string? As3 { get; set; }
        public int? EId { get; set; }
        public int? Ed1 { get; set; }
        public int? Ed2 { get; set; }
        public int? Ed3 { get; set; }

        public virtual Enrollment? EIdNavigation { get; set; }
    }
}
