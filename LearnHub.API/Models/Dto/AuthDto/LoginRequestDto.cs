using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.Models.Dto.AuthDto
{
    public class LoginRequestDto
    {
        [Required, DataType(DataType.EmailAddress)]
        public String Email { get; set; }
        [Required, DataType(DataType.Password)]
        public String Password { get; set; }
    }
}
