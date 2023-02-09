namespace AuthenticationApiService.Errors.Regex
{
    public sealed class RegexError
    {
        public const string Password = @"Has minimum 4 characters in length. At least one uppercase English letter. At least one lowercase English letter. At least one digit";
        public const string Login = "Login must contain only letters and/or numbers";
    }
}
