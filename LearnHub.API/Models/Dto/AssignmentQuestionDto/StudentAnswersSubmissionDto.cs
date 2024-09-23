using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.Models.Dto.AssignmentQuestionDto
{
    public class StudentAnswersSubmissionDto
    {
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int AssignmentId { get; set; }
        [Required]
        public string CourseId { get; set; }
        [Required]
        public List<StudentAnswerDto> Answers { get; set; }
    }
}
