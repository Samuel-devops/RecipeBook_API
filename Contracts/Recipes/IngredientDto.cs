namespace RecipeBook_API.Contracts.Recipes
{
    public class IngredientDto
    {
        public string Name { get; set; } = null!;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = "g";
    }
}