using System.ComponentModel.DataAnnotations;

namespace LearnHub.API.Models.Dto.TopicDto
{
    public class UpdateTopicRequestDto
    {
        [Required]
        [MinLength(3)]
        public String TopicName { get; set; }
    }
}
