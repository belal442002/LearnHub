using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Domain
{
    [Table("Student")]
    public class Student
    {
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        public String AccountId { get; set; }
        public String Name { get; set; }
        public String NationalId { get; set; }
        public DateTime DateOfJoin { get; set; } = DateTime.UtcNow;
        public bool Active_YN { get; set; } = true;

        // Navigation property
        public virtual IdentityUser User { get; set; }
        public virtual ICollection<StudentCourse> StudentCourses { get; set; }

    }
}
