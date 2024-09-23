namespace LearnHub.API.Models.Dto.EvaluationDto
{
    public class EvaluationDto
    {
        public int StudentId { get; set; }
        public string Title { get; set; }
        public int AssignmentId { get; set; }
        public int AssignmentNumber { get; set; }
        public double Grade { get; set; }
        public int MaxScore { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Duration { get; set; }
    }
}
