using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LearnHub.API.Models.Domain
{
    [Table("Parent")]
    public class Parent
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public String? Gender { get; set; }
        public String? Address { get; set; }
        //new
        [ForeignKey(nameof(Account))]
        public String AccountId { get; set; }

        public string NationalId { get; set; }
        public bool Active_YN { get; set; } = true;

        // Navigation property
        //new
        public virtual IdentityUser Account { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
