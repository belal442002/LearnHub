namespace LearnHub.API.Models.Dto.AssignmentDto
{
    public class AssignmentConfigDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int MaxScore { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<AssignmentConfigTopicsDto> AssignmentConfigTopics { get; set; }
    }
}
