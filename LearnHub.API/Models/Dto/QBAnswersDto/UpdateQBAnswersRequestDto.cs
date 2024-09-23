using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.Models.Dto.QBAnswersDto
{
    public class UpdateQBAnswersRequestDto
    {
        [Required]
        [MinLength(1)] 
        public String AnswerText { get; set; }
        [Required]
        public bool Answer_TF { get; set; }
    }
}
