using FluentValidation;

namespace ExpenseTracker.Application.UserCategories.Commands.CreateUserCategory
{
    public class CreateUserCategoryCommandValidator : AbstractValidator<CreateUserCategoryCommand>
    {
        public CreateUserCategoryCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.ActionTypeId).NotEmpty();
            RuleFor(x => x.CategoryName).MinimumLength(2).MaximumLength(100);
        }
    }
}
