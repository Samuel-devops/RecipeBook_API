using RecipeBook_API.Application.Models;
using RecipeBook_API.Contracts;
using RecipeBook_API.Contracts.Recipes;

namespace RecipeBook_API.Application.Abstraction
{
    public interface IRecipeService
    {
        Task<RecipeReadDto?> GetAsync(Guid id, Guid requesterId);

        Task<PagedResult<RecipeReadDto>> SearchAsync(RecipeQuery query, Guid requesterId);

        Task<RecipeReadDto> CreateAsync(RecipeCreateDto dto, Guid ownerId);

        Task<bool> UpdateAsync(Guid id, RecipeUpdateDto dto, Guid requesterId);

        Task<bool> DeleteAsync(Guid id, Guid requesterId);
    }
}