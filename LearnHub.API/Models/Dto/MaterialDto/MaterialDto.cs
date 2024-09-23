namespace LearnHub.API.Models.Dto.MaterialDto
{
    public class MaterialDto
    {
        public int Id { get; set; }
        public string CourseId { get; set; }
        public String CourseName { get; set; }
        public String? MaterilaTitle { get; set; }
        public int MaterialTypeId { get; set; }
        public String MaterialType { get; set; }
        public string MaterialLink { get; set; }
    }
}
