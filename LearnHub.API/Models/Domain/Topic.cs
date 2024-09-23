using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Domain
{
    [Table("Topic")]
    public class Topic
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Course))]
        public String CourseId { get; set; }
        public String TopicName { get; set; }

        // Navigation property
        public virtual Course Course { get; set; }
        public virtual ICollection<QuestionBank> Questions { get; set; }
    }
}
