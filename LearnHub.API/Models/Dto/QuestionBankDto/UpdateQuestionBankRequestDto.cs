using LearnHub.API.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LearnHub.API.Models.Dto.QBAnswersDto;
using LearnHub.API.CustomValidation;

namespace LearnHub.API.Models.Dto.QuestionBankDto
{
    public class UpdateQuestionBankRequestDto
    {
        [Required]
        [CustomTopicExist]
        public int TopicId { get; set; }
        [Required]
        [CustomDifficultyExist]
        public int DifficultyId { get; set; }
        [Required]
        [CustomQuestionTypeExist]
        public int QuestionTypeId { get; set; }
        [Required]
        [MinLength(5)]
        public String QuestionText { get; set; }
        [Required]
        [CustomCheckUpdatedAnswers]
        public ICollection<UpdateQBAnswersRequestDto> Answers { get; set; }
    }
}
