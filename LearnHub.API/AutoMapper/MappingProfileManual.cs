using LearnHub.API.Helper;
using LearnHub.API.Models.Domain;
using LearnHup.Models.Dto.AuthDto;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace LearnHub.API.AutoMapper
{
    public class MappingProfileManual
    {
        private readonly StudentService _studentService;
         public MappingProfileManual(StudentService studentService)
        {
            _studentService = studentService;
        }
        public IdentityUser GetStudentUserFromRegister(RegisterRequestDto registerRequest)
        {
            var studentId = _studentService.GenerateStudentId();
            return new IdentityUser
            {
                UserName = studentId,
                Email = studentId + "@LearnHub.com",
                PhoneNumber = registerRequest.ContactNumber
            };
        }

        public IdentityUser GetInstructorUserFromRegister(RegisterRequestDto registerRequest)
        {
            
            return new IdentityUser
            {
                UserName = registerRequest.Name.Replace(" ", "") + "@LearnHub.com",
                Email = registerRequest.Name.Replace(" ", "") + "@LearnHub.com",
                PhoneNumber = registerRequest.ContactNumber
            };
        }

        public Student GetStudentFromUser(IdentityUser user, RegisterRequestDto registerRequest)
        {
            return new Student
            {
                Id = int.Parse(user.UserName!),
                AccountId = user.Id,
                NationalId = registerRequest.NationalId,
                Name = registerRequest.Name
            };
        }

        public Instructor GetInstructorFromUser(IdentityUser user, RegisterRequestDto registerRequest)
        {
            return new Instructor
            {
                AccountId = user.Id,
                NationalId = registerRequest.NationalId,
                Name = registerRequest.Name
            };
        }
    }
}
