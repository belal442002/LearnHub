using LearnHub.API.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Dto.StudentCourseDto
{
    public class AddStudentCourseRequestDto
    {
        [Required]
        public String CourseId { get; set; }
        [Required]
        public int StudentId { get; set; }
        [Required]
        public string CourseCode { get; set; }
    }
}
