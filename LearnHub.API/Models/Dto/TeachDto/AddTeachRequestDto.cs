using LearnHub.API.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Dto.TeachDto
{
    public class AddTeachRequestDto
    {
        [Required]
        public int InstructorId { get; set; }
        [Required]
        public string CourseId { get; set; }

    }
}