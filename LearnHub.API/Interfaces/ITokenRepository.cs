using Microsoft.AspNetCore.Identity;

namespace LearnHub.API.Interfaces
{
    public interface ITokenRepository
    {
        Task<String> GenerateJwtToken(IdentityUser user, List<String> roles);
    }
}
