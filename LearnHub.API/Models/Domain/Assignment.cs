using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.Models.Domain
{
    [Table("Assignment")]
    public class Assignment
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(AssignmentConfig))]
        public int AssignmentConfigId { get; set; }
        [ForeignKey(nameof(Course))]
        public String CourseId { get; set; }
        public int AssignmentNumber { get; set; }
        public bool Active_YN { get; set; } = true;
        // Navigation property
        public virtual Course Course { get; set; }
        public virtual AssignmentConfig AssignmentConfig { get; set; }
        public virtual ICollection<AssignmentQuestion> AssignmentQuestions { get; set; }
        public virtual ICollection<Evaluation> Evaluations { get; set; }
    }
}
