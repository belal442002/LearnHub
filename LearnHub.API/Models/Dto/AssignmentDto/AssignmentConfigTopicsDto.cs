using LearnHub.API.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Dto.AssignmentDto
{
    public class AssignmentConfigTopicsDto
    {
        public int Id { get; set; }
        public int AssignmentConfigId { get; set; }
        public int TopicId { get; set; }
        public String TopicName { get; set; }
        public int DifficultyId { get; set; }
        public String DifficultyLevel { get; set; }
        public int NumberOfQuestions { get; set; }
    }
}
