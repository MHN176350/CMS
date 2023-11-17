using System;
using System.Collections.Generic;

namespace ProjectPRN.Models
{
    public partial class CourseDetail
    {
        public int Id { get; set; }
        public int? EId { get; set; }
        public DateTime? W1 { get; set; }
        public DateTime? W2 { get; set; }
        public DateTime? W3 { get; set; }
        public DateTime? W4 { get; set; }
        public DateTime? W5 { get; set; }
        public DateTime? W6 { get; set; }

        public virtual Enrollment? EIdNavigation { get; set; }
    }
}
