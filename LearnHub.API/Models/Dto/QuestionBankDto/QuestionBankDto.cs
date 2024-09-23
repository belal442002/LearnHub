using LearnHub.API.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace LearnHub.API.Models.Dto.QuestionBankDto
{
    public class QuestionBankDto
    {
        public int QuestionId { get; set; }

        public String QuestionText { get; set; }
        public int DifficultyId { get; set; }
        public String Difficulty { get; set; }
        public int QuestionTypeId { get; set; }
        public String Type { get; set; }
        public String Topic { get; set; }
        public int TopicId { get; set; }
        public ICollection<Models.Dto.QBAnswersDto.QBAnswersDto> Answers { get; set; }
    }
}
