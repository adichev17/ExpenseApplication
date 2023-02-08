using FluentResults;

namespace AuthenticationApiService.Errors.ControlError
{
    public class DublicateLoginError : Error
    {
        public DublicateLoginError(string message = "A user with this email already exists in the system") : base(message)
        {
            WithMetadata("Login", "please enter a different login");
        }
    }
}
