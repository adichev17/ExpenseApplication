using JwtAuthenticationManager.Models;

namespace JwtAuthenticationManager
{
    public interface IJwtTokenHandler
    {
        AuthenticationResponse? GenerateJwtToken(AuthenticationRequest authenticationRequest);
    }
}
