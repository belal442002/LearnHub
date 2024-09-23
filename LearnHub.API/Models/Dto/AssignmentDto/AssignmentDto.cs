using LearnHub.API.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Dto.AssignmentDto
{
    public class AssignmentDto
    {
        public int Id { get; set; }
        public int AssignmentConfigId { get; set; }
        public String CourseId { get; set; }
        public int AssignmentNumber { get; set; }
        public bool Active_YN { get; set; } = true;
        public AssignmentConfigDto AssignmentConfig { get; set; }
    }
}
