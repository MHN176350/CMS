using System;
using System.Collections.Generic;

namespace ProjectPRN.Models
{
    public partial class Enrollment
    {
        public Enrollment()
        {
            CourseDetails = new HashSet<CourseDetail>();
            Progresses = new HashSet<Progress>();
        }

        public int Id { get; set; }
        public int? SId { get; set; }
        public int? CId { get; set; }
        public DateTime? EDate { get; set; }
        public int? Status { get; set; }
        public int? Ovd { get; set; }

        public virtual Course? CIdNavigation { get; set; }
        public virtual Student? SIdNavigation { get; set; }
        public virtual ICollection<CourseDetail> CourseDetails { get; set; }
        public virtual ICollection<Progress> Progresses { get; set; }
    }
}
