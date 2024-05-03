using LearnHub;
using LearnHub.API.CustomValidation;
using System.ComponentModel.DataAnnotations;

namespace LearnHup.Models.Dto.AuthDto
{
    public class RegisterRequestDto
    {
        [Required]
        public String Name { get; set; }
        [Required, CustomDigitsOnly]
        public String NationalId { get; set; }
        [Required, DataType(DataType.PhoneNumber)]
        public String ContactNumber { get; set; }
        [Required, CustomRoleValidation]
        public String Role { get; set; }
    }
}
