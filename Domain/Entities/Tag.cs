namespace RecipeBook_API.Domain.Entities
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Recipe> Recipes { get; set; } = new();
    }
}