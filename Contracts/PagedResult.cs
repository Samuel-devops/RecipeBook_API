using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RecipeBook_API.Contracts
{
    public class PagedResult<T>(IReadOnlyList<T> items, int total, int page, int pageSize)
    {
        public IReadOnlyList<T> Items { get; } = items;
        public int Total { get; } = total;
        public int Page { get; } = page;
        public int PageSize { get; } = pageSize;
    }
}