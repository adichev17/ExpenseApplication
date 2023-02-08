using JwtAuthenticationManager.Models;
using JwtAuthenticationManager.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

namespace JwtAuthenticationManager
{
    public class JwtTokenHandler : IJwtTokenHandler
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly JwtSettings _jwtSettings;
        private readonly List<UserAccount> _userAccounts;

        public JwtTokenHandler(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtOptions)
        {
            _dateTimeProvider = dateTimeProvider;
            _jwtSettings = jwtOptions.Value;
            // TODO: from db
            _userAccounts = new List<UserAccount>()
            {
                new UserAccount { Id = 1, UserName = "admin", Password = "admin"},
                new UserAccount { Id = 2, UserName = "user1", Password = "$2a$11$91IDeJxPz2/QDzhZevHNTeruLGwQSlcsSxdwNu5c2612wrgZoQIYy"}
            };
        }

        public AuthenticationResponse? GenerateJwtToken(AuthenticationRequest authenticationRequest)
        {
            if (string.IsNullOrWhiteSpace(authenticationRequest.UserName) || string.IsNullOrEmpty(authenticationRequest.Password))
            {
                return null;
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(authenticationRequest.Password);
            var userAccount = _userAccounts.FirstOrDefault(x => x.UserName == authenticationRequest.UserName);

            if (userAccount == null) { return null; }

            if (!BCrypt.Net.BCrypt.Verify(userAccount.Password, passwordHash))
                return null;

            var tokenExpiryTimeStamp = _dateTimeProvider.Now.AddMinutes(_jwtSettings.ExpiryMinutes);
            var tokenKey = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, authenticationRequest.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, userAccount.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
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

            return new AuthenticationResponse
            {
                UserName = userAccount.UserName,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(_dateTimeProvider.Now).TotalSeconds,
                JwtToken = token
            };
        }
    }
}
