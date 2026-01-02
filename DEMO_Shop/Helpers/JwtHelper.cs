using DEMO_Shop.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DEMO_Shop.Helpers
{
    public static class JwtHelper
    {
        public static string GenerateToken(User user, IConfiguration config)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["Jwt:Key"]));

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
