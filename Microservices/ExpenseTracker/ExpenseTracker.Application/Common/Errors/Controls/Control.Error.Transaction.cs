using FluentResults;

namespace ExpenseTracker.Application.Common.Errors.Controls
{
    public sealed class UserCategoryNotFoundError : Error
    {
        public UserCategoryNotFoundError(string message= "The user does not have the specified category") : base(message)
        {

        }
    }

    public sealed class NotFoundTransactionError : Error
    {
        public NotFoundTransactionError(string message = "Transaction not found") : base(message)
        {

        }
    }
}
