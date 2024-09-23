using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Domain
{
    [Table("StudentCourse")]
    public class StudentCourse
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Course))]
        public String CourseId { get; set; }
        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }
        [ForeignKey(nameof(Semester))]
        public int SemesterId { get; set; }
        public int Year { get; set; } = DateTime.UtcNow.Year;

        // Navigation property
        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }
        public virtual Semester Semester { get; set; }
    }
}
