using FluentResults;

namespace ExpenseTracker.Application.Common.Errors.Controls
{
    public sealed class ColorNotFoundError : Error
    {
        public ColorNotFoundError(string message= "Color is not found in the system") : base(message) { }
    }
}
