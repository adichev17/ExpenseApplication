namespace JwtAuthenticationManager.Models
{
    public record GenerateTokenRequest (
        int Id,
        string Login,
        string Password);
}
