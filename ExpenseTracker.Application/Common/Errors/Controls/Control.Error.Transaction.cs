using FluentResults;

namespace ExpenseTracker.Application.Common.Errors.Controls
{
    public sealed class NotFoundUserCategoryError : Error
    {
        public NotFoundUserCategoryError(string message= "The user does not have the specified category") : base(message)
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
