using LearnHub.API.Models.Domain;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnHub.API.Models.Dto.StudentDto
{
    public class StudentDto
    {
        public int Id { get; set; }
        
        public String Name { get; set; }
        public String NationalId { get; set; }
        public String? Gender { get; set; }
        public String? Address { get; set; }
        public DateTime DateOfJoin { get; set; } = DateTime.UtcNow;

        // Navigation property
        public AccountDto Account { get; set; }
        
    }
}
