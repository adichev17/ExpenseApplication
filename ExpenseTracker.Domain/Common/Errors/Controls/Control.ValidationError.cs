using FluentResults;

namespace ExpenseTracker.Domain.Common.Errors.Controls
{
    public sealed class ValidationError : Error
    {
        public ValidationError(string propertyName, string errorMessage, string message = "One or more validation errors occurred.") : base(message)
        {
            WithMetadata(propertyName, errorMessage);
        }
    }
}
