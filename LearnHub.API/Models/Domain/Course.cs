using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Domain
{
    [Table("Course")]
    public class Course
    {
        [Key]
        public String Id { get; set; }
        public string? CourseCode { get; set; } = Guid.NewGuid().ToString();
        public String Name { get; set; }
        public String? Description { get; set; }

        // Navigation property
        public virtual ICollection<Topic> Topics { get; set; }
        public virtual ICollection<Teach> Teaches { get; set; }
        public virtual ICollection<StudentCourse> StudentCourses { get; set; }
        public virtual ICollection<Material> Materials { get; set; }
        public virtual ICollection<Announcement> Announcements { get; set; }
        public virtual ICollection<QuestionBank> Questions { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; } 
        

    }
}
