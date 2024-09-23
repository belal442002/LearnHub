using LearnHub.API.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Dto.MaterialDto
{
    public class AddMaterialRequestDto
    {
        [Required]
        public String CourseId { get; set; }
        [Required]
        public int MaterialTypeId { get; set; }
        [Required]
        [MinLength(2)]
        public String? MaterilaTitle { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}