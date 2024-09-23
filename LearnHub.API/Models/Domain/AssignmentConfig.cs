using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.Models.Domain
{
    [Table("AssignmentConfig")]
    public class AssignmentConfig
    {
        [Key]
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Type { get; set; }
        public int MaxScore { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Navigation property
        public virtual ICollection<AssignmentConfigTopic> AssignmentConfigTopics { get; set; }
    }
}
