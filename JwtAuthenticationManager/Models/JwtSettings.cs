namespace JwtAuthenticationManager.Models
{
    public class JwtSettings
    {
        public const string SectionName = "JwtSettings";
        public string Secret { get; init; } = "super-secret-key";
        public int ExpiryMinutes { get; init; }
    }
}
