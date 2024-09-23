using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Domain
{
    [Table("MaterialType")]
    public class MaterialType
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }

        // Navigation property
        public virtual ICollection<Material> Materials { get; set; }
    }
}
