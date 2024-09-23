using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Domain
{
    [Table("Difficulty")]
    public class Difficulty
    {
        public int Id { get; set; }
        public String Name { get; set; }

        // Navigation property
        public virtual ICollection<QuestionBank> Questions { get; set; }
        
        //new
        public virtual ICollection<AssignmentConfigTopic> AssignmentConfigTopics { get; set; }
    }
}
