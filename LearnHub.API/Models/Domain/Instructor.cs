using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Domain
{
    [Table("Instructor")]
    public class Instructor
    {
        public int Id { get; set; }
        [ForeignKey(nameof(UserAccount))]
        public String AccountId { get; set; }
        public String Name { get; set; }
        public String NationalId { get; set; }
        public DateTime DateOfJoin { get; set; } = DateTime.UtcNow;
        public bool Active_YN { get; set; } = true;

        // Navigation property
        public virtual IdentityUser UserAccount { get; set; }
        public virtual ICollection<Teach> Teaches { get; set; }
        public virtual ICollection<Announcement> Announcements { get; set; }
    }
}
