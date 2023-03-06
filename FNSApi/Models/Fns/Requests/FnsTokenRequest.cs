using System.Text.Json.Serialization;

namespace FNSApi.Models.Fns.Requests
{
    public class FnsTokenRequest : FnsRequestBase
    {
        [JsonPropertyName("code")]
        public string PhoneCode { get; set; }
    }
}
