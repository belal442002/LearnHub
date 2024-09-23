using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.Models.Dto.TopicDto
{
    public class CreateTopicDto
    {
        [Required]
        public string CourseId { get; set; }
        [Required]
        [MinLength(3)]
        public string TopicName { get; set; }
    }
}
