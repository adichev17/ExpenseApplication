using FluentResults;

namespace ExpenseTracker.Domain.Common.Errors.Controls
{
    public sealed class UserNotFoundError : Error
    {
        public UserNotFoundError(string message = "The user is not found in the system") : base(message) { }
    }
}
