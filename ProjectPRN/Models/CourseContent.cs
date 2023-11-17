using System;
using System.Collections.Generic;

namespace ProjectPRN.Models
{
    public partial class CourseContent
    {
        public int CId { get; set; }
        public string? W1 { get; set; }
        public string? W2 { get; set; }
        public string? W3 { get; set; }
        public string? W4 { get; set; }
        public string? W5 { get; set; }
        public string? W6 { get; set; }

        public virtual Course CIdNavigation { get; set; } = null!;
    }
}
