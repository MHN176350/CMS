using System;
using System.Collections.Generic;

namespace ProjectPRN.Models
{
    public partial class Student
    {
        public Student()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? DisplayName { get; set; }
        public string? Avatar { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
