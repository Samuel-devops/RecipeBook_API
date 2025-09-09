using Microsoft.IdentityModel.Tokens;
using RecipeBook_API.Application.Abstraction;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RecipeBook_API.Application.Services
{
    public class JwtTokenService(SecurityKey key) : IJwtTokenService
    {
        public string CreateToken(IEnumerable<Claim> claims, DateTimeOffset expires)
        {
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                claims: claims,
                expires: expires.UtcDateTime,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}