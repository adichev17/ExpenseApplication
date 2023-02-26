using FluentValidation;

namespace ExpenseTracker.Application.Cards.Commands.EditCard
{
    public class EditCardCommandValidator : AbstractValidator<EditCardCommand>
    {
        public EditCardCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.CardId).NotEmpty();
            RuleFor(x => x.CardName).NotEmpty().MinimumLength(3).MaximumLength(100);
            RuleFor(x => x.ColorId).NotEmpty();
        }
    }
}
