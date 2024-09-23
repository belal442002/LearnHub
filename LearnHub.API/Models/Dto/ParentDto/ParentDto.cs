using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LearnHub.API.Models.Domain;

namespace LearnHub.API.Models.Dto.ParentDto
{
    public class ParentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public String? Gender { get; set; }
        public String? Address { get; set; }

        //public String AccountId { get; set; }
        public string NationalId { get; set; }
        public bool Active_YN { get; set; } = true;

        // Navigation property
        public AccountDto Account { get; set; }
        //public virtual ICollection<Student> Students { get; set; }
    }
}
