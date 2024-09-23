using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Domain
{
    [Table("AssignmentConfigTopic")]
    public class AssignmentConfigTopic
    {
        public int Id { get; set; }
        [ForeignKey(nameof(AssignmentConfig))]
        public int AssignmentConfigId { get; set; }
        [ForeignKey(nameof(Topic))]
        public int TopicId { get; set; }
        [ForeignKey(nameof(Difficulty))]
        public int DifficultyId { get; set; }
        public int NumberOfQuestions { get; set; }

        // Navigation property
        public virtual AssignmentConfig AssignmentConfig { get; set; }
        public virtual Topic Topic { get; set; }
        public virtual Difficulty Difficulty { get; set; }
    }
}
