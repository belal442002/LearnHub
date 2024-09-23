using LearnHub.API.CustomValidation;
using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.Models.Dto.ParentDto
{
    public class UpdateParentRequestDto
    {
        [Required]
        [MinLength(5), MaxLength(100)]
        public String Name { get; set; }
        [Required]
        [CustomGender]
        public String Gender { get; set; }
        [Required, MinLength(10), MaxLength(150)]
        public String Address { get; set; }
        [Required]
        public UpdateAccountDto Account { get; set; }
    }
}
