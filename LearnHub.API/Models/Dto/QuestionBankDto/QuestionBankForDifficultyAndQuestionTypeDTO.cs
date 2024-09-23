namespace LearnHub.API.Models.Dto.QuestionBankDto
{
    public class QuestionBankForDifficultyAndQuestionTypeDTO
    {
        public int QuestionId { get; set; }
        public string CourseId { get; set; }
        public int InstructorId { get; set; }
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public int DifficultyId { get; set; }
        public string DifficultyName { get; set; }
        public int QuestionTypeId { get; set; }
        public string QuestionTypeName { get; set; }
        public string QuestionText { get; set; }
        public ICollection<QBAnswersDto.QBAnswersDto> Answers { get; set; }

    }
}
