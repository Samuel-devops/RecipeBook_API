using FluentValidation;
using RecipeBook_API.Domain.Entities;

namespace RecipeBook_API.Validation
{
    public class RecipeValidator : AbstractValidator<Recipe>
    {
        public static readonly string[] AllowedUnits = ["g", "ml", "tbsp", "tsp", "piece"];

        public RecipeValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MinimumLength(3).MaximumLength(120);
            RuleFor(x => x.Servings).GreaterThan(0);
            RuleFor(x => x.Ingredients).NotEmpty();
            RuleForEach(x => x.Ingredients).SetValidator(new IngredientValidator());
            RuleForEach(x => x.Steps).SetValidator(new StepValidator());
        }
    }
}