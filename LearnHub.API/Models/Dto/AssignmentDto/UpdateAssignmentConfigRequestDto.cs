using LearnHub.API.CustomValidation;
using LearnHub.API.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.Models.Dto.AssignmentDto
{
    public class UpdateAssignmentConfigRequestDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        [CustomAssignmentType]
        public string Type { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Max score must be at least 1")]
        public int MaxScore { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public virtual ICollection<UpdateAssignmentConfigTopicRequestDto> AssignmentConfigTopics { get; set; }
    }
}