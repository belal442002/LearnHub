using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Domain
{
    [Table("QBAnswers")]
    public class QBAnswers
    {
        [Key]
        public int AnswerId { get; set; }
        [ForeignKey(nameof(Question))]
        public int QuestionId { get; set; }
        public String AnswerText { get; set; }
        public bool Answer_TF { get; set; }

        // Navigation property
        public virtual QuestionBank Question { get; set; }
    }
}
