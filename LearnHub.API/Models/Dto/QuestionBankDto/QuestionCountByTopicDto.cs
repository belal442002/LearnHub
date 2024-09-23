namespace LearnHub.API.Models.Dto.QuestionBankDto
{
    public class QuestionCountByTopicDto
    {
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public int DifficultyId { get; set; }
        public string DifficultyLevel { get; set; }

        public int QuestionCount { get; set; }
    }
}
