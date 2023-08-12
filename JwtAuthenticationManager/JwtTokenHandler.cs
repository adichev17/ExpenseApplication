using JwtAuthenticationManager.Models;
using JwtAuthenticationManager.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthenticationManager
{
    public class JwtTokenHandler : IJwtTokenHandler
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly JwtSettings _jwtSettings;

        public JwtTokenHandler(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtOptions)
        {
            _dateTimeProvider = dateTimeProvider;
            _jwtSettings = jwtOptions.Value;
        }

        public JwtTokenResponse? GenerateJwtToken(GenerateTokenRequest authenticationRequest)
        {
            var tokenExpiryTimeStamp = _dateTimeProvider.Now.AddMinutes(_jwtSettings.ExpiryMinutes);
            var tokenKey = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, authenticationRequest.Login),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, authenticationRequest.Id.ToString())
            });

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = signingCredentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new JwtTokenResponse
            {
                UserName = authenticationRequest.Login,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(_dateTimeProvider.Now).TotalSeconds,
                JwtToken = token
            };
        }
    }
}
