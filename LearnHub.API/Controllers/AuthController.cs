using LearnHub.API.AutoMapper;
using LearnHub.API.CustomValidation;
using LearnHub.API.Helper;
using LearnHub.API.Interfaces;
using LearnHub.API.Models.Dto.AuthDto;
using LearnHub.API.UniteOfWork;
using LearnHup.Models.Dto.AuthDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace LearnHup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly MappingProfileManual _manualMapper;
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(IAuthRepository authRepository, 
            ITokenRepository tokenRepository, 
            MappingProfileManual manualMapper, 
            IUnitOfWork uniteOfWork)
        {
            _authRepository = authRepository;
            _tokenRepository = tokenRepository;
            _manualMapper = manualMapper;
            _unitOfWork = uniteOfWork;
        }

        [HttpPost]
        [Route("Register")]
        [ValidationModel]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
        {

            // creating student
            if(registerRequest.Role.Equals(Roles.Student.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    // Begin transaction
                    await _unitOfWork.BeginTransactionAsync();
                    // Create account (First transaction)
                    var studentUser = _manualMapper.GetStudentUserFromRegister(registerRequest);
                    var result = await _authRepository.CreateUserAsync(studentUser, registerRequest.NationalId, registerRequest.Role);
                    if (!result)
                    {
                        await _unitOfWork.RollbackAsync();
                        ModelState.AddModelError("", "Somthing went wrong while creating user");
                        return StatusCode(500, ModelState);
                    }
                    // Create Student Domain Model (Second transaction)
                    var studentDomainModel = _manualMapper.GetStudentFromUser(studentUser, registerRequest);
                    var studentCreated = await _unitOfWork.Student.AddAsync(studentDomainModel);
                    if (!studentCreated)
                    {
                        await _unitOfWork.RollbackAsync();
                        ModelState.AddModelError("", "Somthing went wrong while creating student");
                        return StatusCode(500, ModelState);
                    }
                    // End transaction
                        await _unitOfWork.SaveChangesAsync();
                        await _unitOfWork.CommitAsync();
                }
                catch (Exception)
                {
                    await _unitOfWork.RollbackAsync();
                    return StatusCode(500, "An error occurred while processing your request.");
                }
            }

            // creating instructor
            else if(registerRequest.Role.Equals(Roles.Instructor.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    // Begin transaction
                    await _unitOfWork.BeginTransactionAsync();
                    // Create account (First transaction)
                    var instructorUser = _manualMapper.GetInstructorUserFromRegister(registerRequest);
                    var result = await _authRepository.CreateUserAsync(instructorUser, registerRequest.NationalId, registerRequest.Role);
                    if (!result)
                    {
                        await _unitOfWork.RollbackAsync();
                        ModelState.AddModelError("", "Somthing went wrong while creating instructor account");
                        return StatusCode(500, ModelState);
                    }
                    // Create Instructor Domain Model (Second Transaction)
                    var instructorDomainModel = _manualMapper.GetInstructorFromUser(instructorUser, registerRequest);
                    var instructorCreated = await _unitOfWork.Instructor.AddAsync(instructorDomainModel);
                    if (!instructorCreated)
                    {
                        await _unitOfWork.RollbackAsync();
                        ModelState.AddModelError("", "Somthing went wrong while creating instructor");
                        return StatusCode(500, ModelState);
                    }
                    // End Transaction
                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception)
                {
                    await _unitOfWork.RollbackAsync();
                    return StatusCode(500, "An error occurred while processing your request.");
                }
            }

            //new
            // creating parent
            else if (registerRequest.Role.Equals(Roles.Parent.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    // Begin transaction
                    await _unitOfWork.BeginTransactionAsync();
                    // Create account (First transaction)
                    var parentUser = _manualMapper.GetParentUserFromRegister(registerRequest);
                    var result = await _authRepository.CreateUserAsync(parentUser, registerRequest.NationalId, registerRequest.Role);
                    if (!result)
                    {
                        await _unitOfWork.RollbackAsync();
                        ModelState.AddModelError("", "Somthing went wrong while creating parent account");
                        return StatusCode(500, ModelState);
                    }
                    // Create Parent Domain Model (Second Transaction)
                    var parentDomainModel = _manualMapper.GetParentFromUser(parentUser, registerRequest);
                    var parentCreated = await _unitOfWork.Parent.AddAsync(parentDomainModel);
                    if (!parentCreated)
                    {
                        await _unitOfWork.RollbackAsync();
                        ModelState.AddModelError("", "Somthing went wrong while creating parent");
                        return StatusCode(500, ModelState);
                    }
                    // End Transaction
                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception)
                {
                    await _unitOfWork.RollbackAsync();
                    return StatusCode(500, "An error occurred while processing your request.");
                }
            }

            return Ok(new { Message = "User created successfully" });
        }

        [HttpPost]
        [Route("Login")]
        [ValidationModel]
        //[AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var user = await _authRepository.UserExistAsync(loginRequest);
            if (user == null)
                return BadRequest(new { Message = "Incorrect Password or Email" });

            var roles = await _authRepository.GetRolesAsync(user);
            if (roles == null)
                return BadRequest( new { Message = "User do not have any role" });

            var bearerToken = await _tokenRepository.GenerateJwtToken(user, roles);
            var response = new LoginResponseDto
            {
                BearerToken = bearerToken
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("ChangePassword/")]
        //[Authorize(Roles = "Instructor, Student, Parent, Admin")]
        public async Task<IActionResult> ChangePassword([FromBody] UpdatePasswordDto updatePassword)
        {
            var user = await _authRepository.GetUserByIdAsync(updatePassword.UserId);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            var result = await _authRepository.ChangePasswordAsync(user, updatePassword.CurrentPassword, updatePassword.NewPassword);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Password changed successfully return to log in" });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("ResetPassword/")]
        [ValidationModel]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto resetPassword)
        {
            var user = await _authRepository.GetUserByIdAsync(resetPassword.UserId);

            if(user == null)
            {
                return BadRequest(new { Message = "User not found" });
            }

            var token = await _authRepository.GenerateTokenForResetPasswordAsync(user);
            if(token == null)
            {
                return BadRequest(new { Message = "Something went wrong while generating the token for reset password"});
            }

            var role = await _authRepository.GetRoleByUserAsync(user);
            if (role == null)
            {
                return BadRequest(new { Message = "No roles for this user"});
            }

            IdentityResult result = new IdentityResult();

            if (role == Roles.Student.ToString())
            {
                var studentDomainModel = await _unitOfWork.Student.GetStudentByAccountIdAsync(user.Id);
                if (studentDomainModel == null)
                    return NotFound(new { Message = "Student not found"});
                result = await _authRepository.ResetPasswordAsync(user, token, studentDomainModel.NationalId);
            }
            if (role == Roles.Instructor.ToString())
            {
                var instructorDomainModel = await _unitOfWork.Instructor.GetInstructorByAccountId(user.Id);
                if (instructorDomainModel == null)
                    return NotFound(new { Message = "Instructor not found" });
                result = await _authRepository.ResetPasswordAsync(user, token, instructorDomainModel.NationalId);
            }
            if (role == Roles.Parent.ToString())
            {
                var parentDomainModel = await _unitOfWork.Parent.GetParentByAccountId(user.Id);
                if (parentDomainModel == null)
                    return NotFound(new { Message = "Parent not found" });
                result = await _authRepository.ResetPasswordAsync(user, token, parentDomainModel.NationalId);
            }

            if(result.Succeeded)
            {
                return Ok(new { Message = "Password has been reset successfully to the nationalId of this user." });
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }

        }
        
    }
}
