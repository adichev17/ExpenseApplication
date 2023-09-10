using FluentValidation;

namespace ExpenseTracker.Application.Categories.Commands
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.CategoryName).NotEmpty().MinimumLength(2).MaximumLength(100);
            RuleFor(x => x.ActionTypeId).NotEmpty();
        }
    }
}
