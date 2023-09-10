using System.Text.Json.Serialization;

namespace FNSApi.Models.Fns.Requests
{
    public class FnsRequestBase
    {
        [JsonPropertyName("phone")]
        public string PhoneNumber { get; set; }
        [JsonPropertyName("client_secret")]
        public string SecretKey { get; set; }
        [JsonPropertyName("os")]
        public string OperationSystem { get; set; }
    }
}
