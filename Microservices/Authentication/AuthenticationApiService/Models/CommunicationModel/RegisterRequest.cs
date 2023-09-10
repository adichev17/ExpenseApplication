using AuthenticationApiService.Errors.Regex;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationApiService.Models.CommunicationModel
{
    public sealed class RegisterRequest
    {
        [MinLength(5)]
        [RegularExpression(Regex.Regex.Login, ErrorMessage = RegexError.Login)]
        public required string Login { get; set; }
        [RegularExpression(Regex.Regex.Password, ErrorMessage = RegexError.Password)]
        public required string Password { get; set; }
    }
}
