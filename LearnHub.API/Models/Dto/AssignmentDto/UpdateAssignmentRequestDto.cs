using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.Models.Dto.AssignmentDto
{
    public class UpdateAssignmentRequestDto
    {
        [Required]
        public UpdateAssignmentConfigRequestDto AssignmentConfig { get; set; }
    }
}