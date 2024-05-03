using LearnHub.API.AutoMapper;
using LearnHub.API.CustomValidation;
using LearnHub.API.Data;
using LearnHub.API.Helper;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Domain;
using LearnHub.API.Models.Dto.AuthDto;
using LearnHup.Models.Dto.AuthDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace LearnHup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IInstructorRepository _instructorRepository;
        private readonly MappingProfileManual _manualMapper;
        private readonly LearnHubDbContext _context;

        public AuthController(IAuthRepository authRepository, 
            ITokenRepository tokenRepository, 
            IStudentRepository studentRepository, 
            IInstructorRepository instructorRepository, 
            StudentService studentService, 
            MappingProfileManual manualMapper, 
            LearnHubDbContext context)
        {
            _authRepository = authRepository;
            _tokenRepository = tokenRepository;
            _studentRepository = studentRepository;
            _instructorRepository = instructorRepository;
            _manualMapper = manualMapper;
            _context = context;
            
        }

        [HttpPost]
        [Route("Register")]
        [ValidationModel]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
        {

            // creating student
            if(registerRequest.Role.Equals(Roles.Student.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Create account
                        var studentUser = _manualMapper.GetStudentUserFromRegister(registerRequest);
                        var result = await _authRepository.CreateUserAsync(studentUser, registerRequest.NationalId, registerRequest.Role);
                        if (!result)
                        {
                            await transaction.RollbackAsync();
                            ModelState.AddModelError("", "Somthing went wrong while creating user");
                            return StatusCode(500, ModelState);
                        }
                        // Create Student Domain Model
                        var studentDomainModel = _manualMapper.GetStudentFromUser(studentUser, registerRequest);
                        var studentCreated = await _studentRepository.CreateStudentAsync(studentDomainModel);
                        if (!studentCreated)
                        {
                            await transaction.RollbackAsync();
                            ModelState.AddModelError("", "Somthing went wrong while creating student");
                            return StatusCode(500, ModelState);
                        }
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        return StatusCode(500, "An error occurred while processing your request.");
                    }
                }
            }
            // creating instructor
            else if(registerRequest.Role.Equals(Roles.Instructor.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Create account 
                        var instructorUser = _manualMapper.GetInstructorUserFromRegister(registerRequest);
                        var result = await _authRepository.CreateUserAsync(instructorUser, registerRequest.NationalId, registerRequest.Role);
                        if (!result)
                        {
                            await transaction.RollbackAsync();
                            ModelState.AddModelError("", "Somthing went wrong while creating instructor account");
                            return StatusCode(500, ModelState);
                        }
                        // Create Instructor Domain Model
                        var instructorDomainModel = _manualMapper.GetInstructorFromUser(instructorUser, registerRequest);
                        var instructorCreated = await _instructorRepository.CreateInstructorAsync(instructorDomainModel);
                        if (!instructorCreated)
                        {
                            await transaction.RollbackAsync();
                            ModelState.AddModelError("", "Somthing went wrong while creating instructor");
                            return StatusCode(500, ModelState);
                        }
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch(Exception)
                    {
                        await transaction.RollbackAsync();
                        return StatusCode(500, "An error occurred while processing your request.");
                    }   
                } 
            }
            return Ok("User created successfully");
        }

        [HttpPost]
        [Route("Login")]
        [ValidationModel]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var user = await _authRepository.UserExistAsync(loginRequest);
            if (user == null)
                return BadRequest("Incorrect Password or Email");

            var roles = await _authRepository.GetRolesAsync(user);
            if (roles == null)
                return BadRequest("User do not have any role");

            var bearerToken = _tokenRepository.GenerateJwtToken(user, roles);
            var response = new LoginResponseDto
            {
                BearerToken = bearerToken
            };

            return Ok(response);
        }
        
    }
}
