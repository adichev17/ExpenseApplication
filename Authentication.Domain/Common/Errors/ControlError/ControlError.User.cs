using FluentResults;

namespace Authentication.Domain.Common.Errors.ControlError
{
    public sealed class DublicateLoginError : Error
    {
        public DublicateLoginError(string message = "A user with this email already exists in the system") : base(message)
        {
        }
    }
}
