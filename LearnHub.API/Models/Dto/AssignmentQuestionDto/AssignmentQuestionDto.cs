using LearnHub.API.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Dto.AssignmentQuestionDto
{
    public class AssignmentQuestionDto
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public String QuestionText { get; set; }
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }

        public string? ChoiceA { get; set; }
        public string? ChoiceB { get; set; }
        public string? ChoiceC { get; set; }
        public string? ChoiceD { get; set; }
        public string CorrectAnswer { get; set; }
        public string? StudentAnswer { get; set; }
    }
}
