using FluentResults;

namespace ExpenseTracker.Application.Common.Errors.Controls
{
    public sealed class DuplicateCardError : Error
    {
        public DuplicateCardError(string message = "The user already has a card with this name") : base(message)
        {
        }
    }
}
