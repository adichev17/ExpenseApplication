using FluentResults;

namespace Authentication.Domain.Common.Errors.ControlError
{
    public sealed class DuplicateLoginError : Error
    {
        public DuplicateLoginError(string message = "A user with this email already exists in the system") : base(message)
        {
        }
    }
}
