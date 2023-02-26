using FluentValidation;

namespace ExpenseTracker.Application.Expenses.Commands.CreateExpense
{
    public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator()
        {
            RuleFor(x => x.CardId).NotEmpty();
            RuleFor(x => x.Amount).GreaterThanOrEqualTo(0.5m);
            RuleFor(x => x.Comment).MaximumLength(100);
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }
}
