namespace JwtAuthenticationManager.Models
{
    public record GenerateTokenRequest (
        Guid Id,
        string Login,
        string Password);
}
