using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Domain
{
    [Table("Teach")]
    public class Teach
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Instructor))]
        public int InstructorId { get; set; }
        [ForeignKey(nameof(Course))]
        public String CourseId { get; set; }
        [ForeignKey(nameof(Semester))]
        public int SemesterId { get; set; }
        public int Year { get; set; } = DateTime.UtcNow.Year;

        // Navigation propery
        public virtual Instructor Instructor { get; set; }
        public virtual Course Course { get; set; }
        public virtual Semester Semester { get; set; }
    }
}
