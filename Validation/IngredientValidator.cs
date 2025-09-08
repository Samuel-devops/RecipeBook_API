using FluentValidation;
using RecipeBook_API.Domain.Entities;

namespace RecipeBook_API.Validation
{
    public class IngredientValidator : AbstractValidator<Ingredient>
    {
        public IngredientValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Quantity).GreaterThan(0);
            RuleFor(x => x.Unit).NotEmpty().Must(u => RecipeValidator.AllowedUnits.Contains(u));
        }
    }
}