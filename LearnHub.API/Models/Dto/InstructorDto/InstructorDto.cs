using LearnHub.API.Models.Domain;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Dto.InstructorDto
{
    public class InstructorDto
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String? Gender { get; set; }
        public String? Address { get; set; }
        public String NationalId { get; set; }
        public DateTime DateOfJoin { get; set; } = DateTime.UtcNow;
        public bool Active_YN { get; set; } = true;

        // Navigation property
        public AccountDto Account { get; set; }
    }
}
