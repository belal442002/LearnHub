namespace LearnHub.API.Models.Dto.TopicDto
{
    public class TopicDTO
    {
        public int Id { get; set; }
        public string TopicName { get; set; }
        public CourseDtoForTopic Course { get; set; }


    }
}
