using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Domain
{
    [Table("Student")]
    public class Student
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Account))]
        public String AccountId { get; set; }
        public String Name { get; set; }
        public String? Gender { get; set; }
        public String? Address { get; set; }

        public String NationalId { get; set; }
        //new
        [ForeignKey(nameof(Parent))]
        public int? ParentId { get; set; }
        public DateTime DateOfJoin { get; set; } = DateTime.UtcNow;
        public bool Active_YN { get; set; } = true;

        // Navigation property
        public virtual IdentityUser Account { get; set; }
        //new
        public virtual Parent Parent { get; set; }
        public virtual ICollection<StudentCourse> StudentCourses { get; set; }
        public virtual ICollection<AssignmentQuestion> AssignmentQuestions { get; set; }
        public virtual ICollection<Evaluation> Evaluations { get; set; }

    }
}
