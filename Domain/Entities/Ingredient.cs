namespace RecipeBook_API.Domain.Entities
{
    public class Ingredient
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = "g";
        public Guid RecipeId { get; set; }
    }
}