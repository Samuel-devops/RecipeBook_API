namespace RecipeBook_API.Contracts.Recipes
{
    public class RecipeCreateDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int Servings { get; set; } = 1;
        public int? TotalMinutes { get; set; }
        public List<IngredientDto> Ingredients { get; set; } = new();
        public List<InstructionStepDto> Steps { get; set; } = new();
        public List<string> Tags { get; set; } = new();
        public NutritionDto? Nutrition { get; set; }
    }
}