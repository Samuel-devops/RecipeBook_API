namespace RecipeBook_API.Domain.Entities
{
    public class InstructionStep
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
        public string Text { get; set; } = null!;
        public Guid RecipeId { get; set; }
    }
}