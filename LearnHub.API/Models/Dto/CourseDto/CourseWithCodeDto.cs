namespace LearnHub.API.Models.Dto.CourseDto
{
    public class CourseWithCodeDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string CourseCode { get; set; }
    }
}
