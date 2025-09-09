using System.Security.Claims;

namespace RecipeBook_API.Application.Abstraction
{
    public interface IJwtTokenService
    {
        string CreateToken(IEnumerable<Claim> claims, DateTimeOffset expires);
    }
}