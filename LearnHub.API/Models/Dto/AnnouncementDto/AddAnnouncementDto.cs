using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.Models.Dto.AnnouncementDto
{
    public class AddAnnouncementDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Announcement length should not be less than 3 letters")]
        public string Text { get; set; }
        [Required]
        public int InstructorId { get; set; }
        [Required]
        public string CourseId { get; set; }
    }
}
