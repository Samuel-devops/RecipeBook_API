using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeBook_API.Application.Abstraction;
using RecipeBook_API.Contracts.Recipes;
using System.Security.Claims;

namespace RecipeBook_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("recipes")]
    public class RecipesController(IRecipeService svc) : ControllerBase
    {
        private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string? q, [FromQuery] string[]? tags, [FromQuery] string[]? includeIngredients,
            [FromQuery] string[]? excludeIngredients, [FromQuery] int? maxKcal, [FromQuery] string? sort, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            return Ok(await svc.SearchAsync(new(q, tags, includeIngredients, excludeIngredients, maxKcal, sort, page, pageSize), UserId));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return (await svc.GetAsync(id, UserId)) is { } dto ? Ok(dto) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RecipeCreateDto dto)
        {
            var created = await svc.CreateAsync(dto, UserId);

            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RecipeUpdateDto dto)
        {
            return await svc.UpdateAsync(id, dto, UserId) ? NoContent() : NotFound();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await svc.DeleteAsync(id, UserId) ? NoContent() : NotFound();
        }
    }
}