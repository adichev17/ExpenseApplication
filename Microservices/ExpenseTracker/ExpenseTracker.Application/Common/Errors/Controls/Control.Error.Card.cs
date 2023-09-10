using FluentResults;

namespace ExpenseTracker.Application.Common.Errors.Controls
{
    public sealed class DuplicateCardError : Error
    {
        public DuplicateCardError(string message = "The user already has a card with this name") : base(message)
        {
        }
    }

    public sealed class CardNotFoundError : Error
    {
        public CardNotFoundError(string message = "") : base (message)
        {
            WithMetadata("CardId", "Card with given ID not found");
        }
    }
}
