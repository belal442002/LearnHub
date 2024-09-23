using LearnHub.API.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.Models.Dto.QBAnswersDto
{
    public class AddQBAnswerRequestDto
    {
        [Required]
        [MinLength(1)]
        public String AnswerText { get; set; }
        [Required]
        public bool Answer_TF { get; set; }
    }
}