using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.Models.Dto.AuthDto
{
    public class ResetPasswordRequestDto
    {
        [Required]
        public String UserId { get; set; }
    }
}
