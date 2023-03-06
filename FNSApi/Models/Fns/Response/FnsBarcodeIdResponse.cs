using System.Text.Json.Serialization;

namespace FNSApi.Models.Fns.Response
{
    public class FnsBarcodeIdResponse
    {
        [JsonPropertyName("id")]
        public string BarcodeId { get; set; }
    }
}
