using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Domain
{
    [Table("QuestionBank")]
    public class QuestionBank
    {
        [Key]
        public int QuestionId { get; set; }
        [ForeignKey(nameof(Course))]
        public String CourseId { get; set; }
        [ForeignKey(nameof(Instructor))]
        public int InstructorId { get; set; }
        [ForeignKey(nameof(Topic))]
        public int TopicId { get; set; }
        [ForeignKey(nameof(Difficulty))]
        public int DifficultyId { get; set; }
        [ForeignKey(nameof(QuestionType))]
        public int QuestionTypeId { get; set; }
        public String QuestionText { get; set; }

        // Navigation property
        public virtual Course Course { get; set; }
        public virtual Instructor Instructor { get; set; }
        public virtual Topic Topic { get; set; }
        public virtual Difficulty Difficulty { get; set; }
        public virtual QuestionType QuestionType { get; set; }
        public virtual ICollection<QBAnswers> Answers { get; set; }
        public virtual ICollection<AssignmentQuestion> AssignmentQuestions { get; set; }
    }
}
