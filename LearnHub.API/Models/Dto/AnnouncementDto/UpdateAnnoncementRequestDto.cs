using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.Models.Dto.AnnouncementDto
{
    public class UpdateAnnoncementRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Announcement length should not be less than 3 letters")]
        public String Text { get; set; }
    }
}
