using FluentResults;
using JwtAuthenticationManager.Models;

namespace AuthenticationApiService.Services
{
    public interface IAuthenticationService
    {
        Result<JwtTokenResponse> Authenticate(string login, string password);
        Result<bool> Register(string login, string password); 
    }
}
