using JwtAuthenticationManager.Models;

namespace JwtAuthenticationManager
{
    public interface IJwtTokenHandler
    {
        JwtTokenResponse? GenerateJwtToken(GenerateTokenRequest authenticationRequest);
    }
}
