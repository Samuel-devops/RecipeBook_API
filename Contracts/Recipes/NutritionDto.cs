namespace RecipeBook_API.Contracts.Recipes
{
    public class NutritionDto
    {
        public int Kcal { get; set; }
        public decimal Protein { get; set; }
        public decimal Carbs { get; set; }
        public decimal Fat { get; set; }
    }
}