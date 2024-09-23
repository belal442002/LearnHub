using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.Models.Dto.ParentDto
{
    public class LinkStudentToParentDto
    {
        [Required]
        public int ParentId { get; set; }
        [Required]
        public int StudentId { get; set; }
    }
}
