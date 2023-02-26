using FluentValidation;

namespace ExpenseTracker.Application.Cards.Commands.DeleteCard
{
    public class DeleteCardCommandValidator : AbstractValidator<DeleteCardCommand>
    {
        public DeleteCardCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.CardId).NotEmpty();
        }
    }
}
