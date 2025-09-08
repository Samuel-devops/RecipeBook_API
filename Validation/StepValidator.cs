using FluentValidation;
using RecipeBook_API.Domain.Entities;

namespace RecipeBook_API.Validation
{
    public class StepValidator : AbstractValidator<InstructionStep>
    {
        public StepValidator()
        {
            RuleFor(x => x.Order).GreaterThan(0);
            RuleFor(x => x.Text).NotEmpty().MaximumLength(1000);
        }
    }
}