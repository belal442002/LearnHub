using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.Models.Dto
{
    public class UpdateAccountDto
    {
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}
