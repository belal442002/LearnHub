using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.Models.Domain
{
    [Table("AssignmentQuestion")]
    public class AssignmentQuestion
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Question))]
        public int QuestionId { get; set; }

        [ForeignKey(nameof(Assignment))]
        public int AssignmentId { get; set; }
        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }

        public string? ChoiceA { get; set; }
        public string? ChoiceB { get; set; }
        public string? ChoiceC { get; set; }
        public string? ChoiceD { get; set; }
        public string CorrectAnswer { get; set; }
        public string? StudentAnswer { get; set; }
        public bool Active_YN { get; set; } = true;

        // Navigation property
        public virtual QuestionBank Question { get; set; }
        public virtual Assignment Assignment { get; set; }
        public virtual Student Student { get; set; }
    }
}
