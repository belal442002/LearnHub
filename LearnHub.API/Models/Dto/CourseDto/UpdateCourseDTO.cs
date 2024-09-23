using LearnHub.API.CustomValidation;
using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.Models.Dto.CourseDto
{
    public class UpdateCourseDTO
    {
        [Required]
        [MinLength(3), MaxLength(30)]
        [CustomUniqueCourseName]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
