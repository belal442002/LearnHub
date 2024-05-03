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

    }
}
