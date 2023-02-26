using FluentResults;

namespace ExpenseTracker.Application.Common.Errors.Controls
{
    public sealed class NotFoundUserCategoryError : Error
    {
        public NotFoundUserCategoryError(string message= "The user does not have the specified category") : base(message)
        {

        }
    }
}
