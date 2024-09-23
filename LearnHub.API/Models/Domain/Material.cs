using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Domain
{
    [Table("Material")]
    public class Material
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Course))]
        public String CourseId { get; set; }
        [ForeignKey(nameof(MaterialType))]
        public int MaterialTypeId { get; set; }
        public String MaterialLink { get; set; }
        public String? MaterialPath { get; set; } //new

        public String? MaterilaTitle { get; set; }

        // Navigation property
        public virtual Course Course { get; set; }
        public virtual MaterialType MaterialType { get; set; }
    }
}
