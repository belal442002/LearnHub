using LearnHub.API.Interfaces;
using LearnHub.API.Models.Dto.AuthDto;
using Microsoft.AspNetCore.Identity;

namespace LearnHub.API.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AuthRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> CreateUserAsync(IdentityUser user, String password, String role)
        {
            var result = await _userManager.CreateAsync(user, password);
            if(result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(user, role);
                if (result.Succeeded)
                    return true;
            }
            return false;
        }

        public async Task<List<string>?> GetRolesAsync(IdentityUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null)
                return null;
            return roles.ToList();
        }

        public async Task<IdentityUser?> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        public async Task<IdentityResult> ChangePasswordAsync(IdentityUser user, string currentPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result;
        }

        public async Task<IdentityUser?> UserExistAsync(LoginRequestDto loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if(user != null)
            {
                var isCorrectPassword = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
                if (isCorrectPassword)
                {
                    return user;
                }
            }
            return null;
        }

        public async Task<string?> GenerateTokenForResetPasswordAsync(IdentityUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<string?> GetRoleByUserAsync(IdentityUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null)
                return null;
            return roles[0];
        }

        public async Task<IdentityResult> ResetPasswordAsync(IdentityUser user, string token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }
    }
}
