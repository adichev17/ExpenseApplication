using System.Text.Json.Serialization;

namespace FNSApi.Models.Fns.Response
{
    public class FnsTokenResponse
    {
        [JsonPropertyName("sessionId")]
        public string Token { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("phone")]
        public string PhoneNumber { get; set; }
    }
}
