using LearnHub.API.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Dto.StudentCourseDto
{
    public class StudentCourseDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public String StudentName { get; set; }
        public String CourseId { get; set; }
        public String CourseName { get; set; }
        public String Semester { get; set; }
        public int Year { get; set; }

    }
}
