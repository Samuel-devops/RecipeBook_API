using Microsoft.EntityFrameworkCore;
using RecipeBook_API.Application.Abstraction;
using RecipeBook_API.Contracts.Auth;
using RecipeBook_API.Domain.Entities;
using RecipeBook_API.Domain.Infrastructure;
using RecipeBook_API.Infrastructure.Security;
using System.Security.Claims;

namespace RecipeBook_API.Application.Services
{
    public class AuthService(AppDbContext db, IJwtTokenService jwt) : IAuthService
    {
        public async Task<Guid> RegisterAsync(RegisterRequest req)
        {
            if (await db.Users.AnyAsync(u => u.Email == req.Email))
                throw new InvalidOperationException("Email already registered");

            var user = new User { Id = Guid.NewGuid(), Email = req.Email, PasswordHash = PasswordHasher.Hash(req.Password) };
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return user.Id;
        }

        public async Task<(string accessToken, DateTimeOffset expires)> LoginAsync(LoginRequest req)
        {
            var user = await db.Users.SingleOrDefaultAsync(u => u.Email == req.Email);
            if (user == null || !PasswordHasher.Verify(req.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };
            var expires = DateTimeOffset.UtcNow.AddHours(12);
            var token = jwt.CreateToken(claims, expires);
            return (token, expires);
        }
    }
}