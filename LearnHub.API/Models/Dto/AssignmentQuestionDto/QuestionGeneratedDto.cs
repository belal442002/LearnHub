namespace LearnHub.API.Models.Dto.AssignmentQuestionDto
{
    public class QuestionsGeneratedDto
    {
        public int QuestionId { get; set; }
        public String QuestionText { get; set; }
        public string? ChoiceA { get; set; }
        public string? ChoiceB { get; set; }
        public string? ChoiceC { get; set; }
        public string? ChoiceD { get; set; }
        public string? StudentAnswer { get; set; }
    }
}
