using LearnHub.API.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LearnHub.API.Models.Dto.QBAnswersDto;
using LearnHub.API.CustomValidation;

namespace LearnHub.API.Models.Dto.QuestionBankDto
{
    public class AddQuestionBankRequestDto
    {
        [Required]
        public string CourseId { get; set; }
        [Required]
        public int InstructorId { get; set; }
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
        public string QuestionText { get; set; }
        [Required]
        [CustomCheckAnswers]
        public ICollection<AddQBAnswerRequestDto> Answers { get; set; }
    }
}
