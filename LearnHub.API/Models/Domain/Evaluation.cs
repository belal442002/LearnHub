using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.Models.Domain
{
    [Table("Evaluation")]
    public class Evaluation
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }
        [ForeignKey(nameof(Assignment))]
        public int AssignmentId { get; set; }

        public double Grade { get; set; }
        public bool Active_YN { get; set; } = true;

        // Navigation property
        public virtual Student Student { get; set; }
        public virtual Assignment Assignment { get; set; }
    }
}
