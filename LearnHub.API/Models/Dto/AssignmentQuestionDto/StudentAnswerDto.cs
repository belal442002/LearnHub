using LearnHub.API.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.Models.Dto.AssignmentQuestionDto
{
    public class StudentAnswerDto
    {
        [Required]
        public int QuestionId { get; set; }
        public string? ChoiceA { get; set; }
        public string? ChoiceB { get; set; }
        public string? ChoiceC { get; set; }
        public string? ChoiceD { get; set; }
        public string? StudentAnswer { get; set; }
    }
}
