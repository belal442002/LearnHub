using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Domain
{
    [Table("QuestionType")]
    public class QuestionType
    {
        public int Id { get; set; }
        public String Name { get; set; }

        // Navigation property
        public virtual ICollection<QuestionBank> Questions { get; set; }
    }
}
