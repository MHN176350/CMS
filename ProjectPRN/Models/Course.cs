using System;
using System.Collections.Generic;

namespace ProjectPRN.Models
{
    public partial class Course
    {
        public Course()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? LectureId { get; set; }
        public int? Weeks { get; set; }

        public virtual Lecture? Lecture { get; set; }
        public virtual CourseContent? CourseContent { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
