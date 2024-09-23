using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Domain
{
    [Table("Semester")]
    public class Semester
    {
        public int Id { get; set; }
        public String SemesterName { get; set; }

        // Navigation property
        public virtual ICollection<StudentCourse> StudentCourses { get; set; }
        public virtual ICollection<Teach> Teaches { get; set; }

    }
}
