using FluentResults;

namespace Authentication.Domain.Common.Errors.ControlError
{
    public sealed class InvalidCredentialsError : Error
    {
        public InvalidCredentialsError(string message = "Invalid credentials") : base(message)
        {
        }
    }
}
