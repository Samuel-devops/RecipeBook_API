namespace RecipeBook_API.Contracts.Recipes
{
    public class InstructionStepDto
    {
        public int Order { get; set; }
        public string Text { get; set; } = null!;
    }
}