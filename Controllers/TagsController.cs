using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeBook_API.Domain.Entities;
using RecipeBook_API.Domain.Infrastructure;

namespace RecipeBook_API.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("tags")]
    public class TagsController(AppDbContext db) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Search([FromQuery] string? search)
        {
            var q = db.Tags.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                q = q.Where(t => t.Name.Contains(search));
            }

            return Ok(await q.OrderBy(t => t.Name).Take(50).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string name)
        {
            if (await db.Tags.AnyAsync(t => t.Name == name))
            {
                return Conflict();
            }

            var tag = new Tag { Id = Guid.NewGuid(), Name = name };

            db.Tags.Add(tag);
            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(Search), new { search = name }, tag);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var tag = await db.Tags.FindAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            db.Tags.Remove(tag);

            await db.SaveChangesAsync();
            return NoContent();
        }
    }
}