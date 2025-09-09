using Microsoft.AspNetCore.Mvc;
using RecipeBook_API.Application.Abstraction;
using RecipeBook_API.Contracts.Auth;

namespace RecipeBook_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService svc) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest req)
        {
            var userId = await svc.RegisterAsync(req);
            return CreatedAtAction(nameof(Register), new { id = userId }, null);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest req)
        {
            var (token, expires) = await svc.LoginAsync(req);
            return Ok(new { token, expires });
        }
    }
}