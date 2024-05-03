using LearnHub.API.Models.Dto.AuthDto;
using Microsoft.AspNetCore.Identity;

namespace LearnHub.API.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> CreateUserAsync(IdentityUser user, String password, String role);
        Task<IdentityUser?> UserExistAsync(LoginRequestDto loginRequest);
        Task<List<String>?> GetRolesAsync(IdentityUser user);
    }
}
