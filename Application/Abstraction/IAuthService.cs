using RecipeBook_API.Contracts.Auth;

namespace RecipeBook_API.Application.Abstraction
{
    public interface IAuthService
    {
        Task<Guid> RegisterAsync(RegisterRequest req);

        Task<(string accessToken, DateTimeOffset expires)> LoginAsync(LoginRequest req);
    }
}
}