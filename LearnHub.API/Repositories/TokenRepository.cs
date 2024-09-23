using LearnHub.API.Data;
using LearnHub.API.Helper;
using LearnHub.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LearnHub.API.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;
        private readonly LearnHubDbContext _dbContext;

        public TokenRepository(IConfiguration configuration, LearnHubDbContext dbContext)
        {
            this.configuration = configuration;
            _dbContext = dbContext;
        }
        public async Task<String> GenerateJwtToken(IdentityUser user, List<String> roles)
        {
            //old
            //List<Claim> claims = new List<Claim>();
            //claims.Add(new Claim(ClaimTypes.Email, user.Email!));

            //new
            var claims = new List<Claim>
            {
                //new Claim(ClaimTypes.NameIdentifier, user.Id),
                //new Claim(ClaimTypes.Name, user.UserName!),
                //new Claim(ClaimTypes.Email, user.Email!)
                new Claim("userName", user.UserName!),
                new Claim("userId", user.Id),

            };
            //end new

            foreach (var role in roles)
            {
                //claims.Add(new Claim(ClaimTypes.Role, role));
                claims.Add(new Claim("role", role));

                if(role.Equals(Roles.Student.ToString()))
                {
                    var id = await _dbContext.Students
                        .Where(s => s.AccountId.Equals(user.Id)).Select(s => s.Id).FirstOrDefaultAsync();
                    claims.Add(new Claim("id", id.ToString()));
                }
                if (role.Equals(Roles.Instructor.ToString()))
                {
                    var id = await _dbContext.Instructors
                        .Where(i => i.AccountId.Equals(user.Id)).Select(i => i.Id).FirstOrDefaultAsync();
                    claims.Add(new Claim("id", id.ToString()));
                }
                if (role.Equals(Roles.Parent.ToString()))
                {
                    var id = await _dbContext.Parents
                        .Where(p => p.AccountId.Equals(user.Id)).Select(p => p.Id).FirstOrDefaultAsync();
                    claims.Add(new Claim("id", id.ToString()));
                }
            }

            //new
            var expirationDate = DateTime.UtcNow.AddMinutes(45);
            claims.Add(new Claim("exp", new DateTimeOffset(expirationDate).ToUnixTimeSeconds().ToString()));
            //end new

            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var key = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                //issuer: issuer,
                //audience: audience,
                claims: claims,
                //expires: DateTime.UtcNow.AddMinutes(45),
                //signingCredentials: credentials
                expires: expirationDate,
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
