using LearnHub.API.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Dto.AssignmentDto
{
    public class AddAssignmentConfigTopicsRequestDto
    {
        [Required]
        public int TopicId { get; set; }
        [Required]
        public int DifficultyId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Number of questions must be at least 1")]
        public int NumberOfQuestions { get; set; }
    }
}