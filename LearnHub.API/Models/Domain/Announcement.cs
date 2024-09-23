using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Domain
{
    [Table("Announcement")]
    public class Announcement
    {
        public int Id { get; set; }
        public String Text { get; set; }
        [ForeignKey(nameof(Instructor))]
        public int InstructorId { get; set; }
        [ForeignKey(nameof(Course))]
        public String CourseId { get; set; }
        public DateTime DateOfAnnouncement { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual Instructor Instructor { get; set; }
        public virtual Course Course { get; set; }
    }
}
