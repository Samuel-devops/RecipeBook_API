namespace RecipeBook_API.Application.Models
{
    public record RecipeQuery(string? Q, string[]? Tags, string[]? IncludeIngredients, string[]? ExcludeIngredients, int? MaxKcal, string? Sort, int Page = 1, int PageSize = 20);
}