using LearnHub.API.Models.Dto.AuthDto;
using Microsoft.AspNetCore.Identity;

namespace LearnHub.API.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> CreateUserAsync(IdentityUser user, String password, String role);
        Task<IdentityUser?> UserExistAsync(LoginRequestDto loginRequest);
        Task<List<String>?> GetRolesAsync(IdentityUser user);
        Task<String?> GetRoleByUserAsync(IdentityUser user); 
        Task<IdentityUser?> GetUserByIdAsync(string userId);
        Task<String?> GenerateTokenForResetPasswordAsync(IdentityUser user);
        Task<IdentityResult> ResetPasswordAsync(IdentityUser user, String token, String newPassword);
        Task<IdentityResult> ChangePasswordAsync(IdentityUser user, string currentPassword, string newPassword);
    }
}
