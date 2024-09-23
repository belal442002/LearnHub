using LearnHub.API.Models.Dto.QuestionBankDto;

namespace LearnHub.API.Models.Dto.QuestionTypeDto
{
    public class QuestionTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<QuestionBankForDifficultyAndQuestionTypeDTO> Questions { get; set; }
    }
}
