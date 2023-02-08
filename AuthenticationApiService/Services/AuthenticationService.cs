using AuthenticationApiService.Errors.ControlError;
using AuthenticationApiService.Models.CommunicationModel;
using FluentResults;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;

namespace AuthenticationApiService.Services
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        public Result<JwtTokenResponse> Authenticate(string login, string password)
        {
            //string passwordHash = BCrypt.Net.BCrypt.HashPassword(authenticationRequest.Password);
            //var userAccount = _userAccounts.FirstOrDefault(x => x.UserName == authenticationRequest.UserName);

            //if (userAccount == null) { return null; }

            //if (!BCrypt.Net.BCrypt.Verify(userAccount.Password, passwordHash))
            //    return null;

            return Result.Ok(new JwtTokenResponse());
        }

        public Result<bool> Register(string login, string password)
        {
            return Result.Fail<bool>(new DublicateLoginError());
        }
    }
}
