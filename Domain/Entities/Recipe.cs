using RecipeBook_API.Domain.ValueObjects;

namespace RecipeBook_API.Domain.Entities
{
    public class Recipe
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public List<Ingredient> Ingredients { get; set; } = new();
        public List<InstructionStep> Steps { get; set; } = new();
        public List<Tag> Tags { get; set; } = new();
        public Nutrition? Nutrition { get; set; }
        public int Servings { get; set; } = 1;
        public int? TotalMinutes { get; set; }
        public Guid OwnerId { get; set; }
        public User Owner { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}