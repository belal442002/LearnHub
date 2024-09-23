namespace LearnHub.API.Models.Dto.AssignmentQuestionDto
{
    public class AssignmentQuestionsGeneratedWithDurationDto
    {
        
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public double Duration { get; set; }
        public List<QuestionsGeneratedDto> QuestionsGenerated { get; set; }
    }
}
