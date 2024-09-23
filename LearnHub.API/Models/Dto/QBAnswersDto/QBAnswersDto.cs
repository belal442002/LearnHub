using LearnHub.API.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.Models.Dto.QBAnswersDto
{
    public class QBAnswersDto
    {
        public int AnswerId { get; set; }
        public String AnswerText { get; set; }
        public bool Answer_TF { get; set; }

    }
}
