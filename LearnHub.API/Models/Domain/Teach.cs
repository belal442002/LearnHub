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
        public DateTime DateOfTeach { get; set; } = DateTime.UtcNow;

        // Navigation propery
        public virtual Instructor Instructor { get; set; }
        public virtual Course Course { get; set; }
    }
}
