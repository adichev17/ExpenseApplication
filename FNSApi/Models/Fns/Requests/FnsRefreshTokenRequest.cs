using System.Text.Json.Serialization;

namespace FNSApi.Models.Fns.Requests
{
    public class FnsRefreshTokenRequest
    {
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonPropertyName("client_secret")]
        public string SecretKey { get; set; }
    }
}
