using System;
using System.Collections.Generic;

namespace ProjectPRN.Models
{
    public partial class Lecture
    {
        public Lecture()
        {
            Courses = new HashSet<Course>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Avatar { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
