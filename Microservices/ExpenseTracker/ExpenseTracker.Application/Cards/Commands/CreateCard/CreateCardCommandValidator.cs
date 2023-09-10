using FluentValidation;

namespace ExpenseTracker.Application.Cards.Commands.CreateCard
{
    public class CreateCardCommandValidator : AbstractValidator<CreateCardCommand>
    {
        public CreateCardCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.CardName).NotEmpty().MinimumLength(3).MaximumLength(100);
            RuleFor(x => x.ColorId).NotEmpty();
        }
    }
}
