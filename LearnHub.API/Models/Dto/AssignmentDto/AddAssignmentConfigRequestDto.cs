using LearnHub.API.CustomValidation;
using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.Models.Dto.AssignmentDto
{
    public class AddAssignmentConfigRequestDto
    {
        [Required]
        public string CourseId { get; set; }
        [Required, MinLength(3), MaxLength(25)]
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
        public ICollection<AddAssignmentConfigTopicsRequestDto> AssignmentConfigTopics { get; set; }

    }
}
