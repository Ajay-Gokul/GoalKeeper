using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model.Entity;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using TokenService.Interface;

namespace TokenService.Service
{
    public class TokenGenerationService : ITokenGenerationService
    {
        private readonly IConfiguration _config;

        public TokenGenerationService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.UID.ToString()),
            new Claim(ClaimTypes.Name, user.Name)
        };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
            
        }
    }
}