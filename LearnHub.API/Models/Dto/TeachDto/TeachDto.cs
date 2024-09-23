using LearnHub.API.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Dto.TeachDto
{
    public class TeachDto
    {
        public int Id { get; set; }
        public int InstructorId { get; set; }
        public String InstructorName { get; set; }
        public String CourseId { get; set; }
        public String CourseName { get; set; }
        public String Semester { get; set; }
        public int Year { get; set; }

    }
}
